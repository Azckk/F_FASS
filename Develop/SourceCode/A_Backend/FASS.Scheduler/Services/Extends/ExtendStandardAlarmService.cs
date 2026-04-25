using Common.AspNetCore.Extensions;
using Common.Frame.Services.Frame.Interfaces;
using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Boot.Services;
using FASS.Extend.Car.Fairyland.Pcb;
using FASS.Scheduler.Utility;
using FASS.Service.Models.RecordExtend;
using FASS.Service.Services.RecordExtend.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace FASS.Scheduler.Services.Extends
{
    public class ExtendStandardAlarmService
    {
        public ExtendService ExtendService { get; }

        public IBootService BootService { get; }

        public bool IsStarted { get; private set; }

        private readonly IAlarmMdcsService _alarmMdcsService;
        private readonly IConfigService _configService;
        private readonly IDistributedCache _distributedCache;

        private Dictionary<string, Dictionary<int, string>>? cartAlarmCodeMap = new Dictionary<string, Dictionary<int, string>>();

        private Dictionary<string, Dictionary<int, DateTime>>? alarmTriggerRecord = new Dictionary<string, Dictionary<int, DateTime>>();

        public int StandardAlarmDueTime { get; set; }
        public int AlarmDuration { get; set; } = 1000;

        public ExtendStandardAlarmService(
            ExtendService eventService)
        {
            ExtendService = eventService;

            BootService = ExtendService.ServiceProvider.GetRequiredService<IBootService>();

            _alarmMdcsService = ExtendService.ServiceProvider.GetScopeService<IAlarmMdcsService>();
            _configService = ExtendService.ServiceProvider.GetScopeService<IConfigService>();
            _distributedCache = ExtendService.ServiceProvider.GetScopeService<IDistributedCache>();

            Init();
        }

        public void Init()
        {
            //初始化参数
            var configDto = _configService.Set().Where(e => e.Key == "AlarmTriggerDuration").FirstOrDefault();
            AlarmDuration = configDto == null ? AlarmDuration : configDto.Value.ToInt();
            StandardAlarmDueTime = ExtendService.AppSettings.Extend.StandardAlarmDueTime;
            var codeMapCache = _distributedCache.GetString("StandardAlarm.cartAlarmCodeMap");
            if (codeMapCache is not null)
            {
                cartAlarmCodeMap = codeMapCache.JsonTo<Dictionary<string, Dictionary<int, string>>>();
            }
            var triggerRecordCache = _distributedCache.GetString("StandardAlarm.alarmTriggerRecord");
            if (triggerRecordCache is not null)
            {
                alarmTriggerRecord = triggerRecordCache.JsonTo<Dictionary<string, Dictionary<int, DateTime>>>();
            }
        }

        public void Start()
        {
            try
            {
                if (IsStarted)
                {
                    return;
                }
                IsStarted = true;
                Task.Factory.StartNew(() =>
                {
                    var last = DateTime.Now;
                    while (IsStarted)
                    {
                        if ((DateTime.Now - last).TotalMilliseconds >= StandardAlarmDueTime)
                        {
                            last = DateTime.Now;
                            ScanAlarm();
                        }
                        Thread.Sleep(1000);
                    }
                }, TaskCreationOptions.LongRunning);
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        public void Stop()
        {
            try
            {
                if (!IsStarted)
                {
                    return;
                }
                IsStarted = false;
            }
            catch (Exception ex)
            {
                ExtendService.Logger.LogError(ex.ToString());
            }
        }

        private List<AlarmMdcs> modelAddList = new List<AlarmMdcs>();
        private List<AlarmMdcs> modelUpdateList = new List<AlarmMdcs>();
        private void ScanAlarm()
        {
            var cars = BootService.Cars
                .Where(e => e.Type is not null && (e.Type.ToLower().Contains("plc") || e.Type.ToLower().Contains("pcb"))
                  && e.IsOnline)
                .ToList();
            foreach (var cart in cars)
            {
                //解析车辆当前报警状态
                if (cart.CarState is null || !cart.CarState.TryJsonTo<ReceiveStateMessage>(out var recvStateMessage) || recvStateMessage?.Alarm is null)
                {
                    continue;
                }
                var byteArray = BitConverter.GetBytes(recvStateMessage.Alarm);
                var boolArray = ByteHelper.ByteArrayToBoolArray(byteArray);
                var alarmCodeDic = new Dictionary<int, string>();//当前报警(key,value)字典
                for (var i = 0; i < boolArray.Length; i++)
                {
                    if (boolArray[i])
                    {
                        var carAlarm = BootService.DictItems.Where(x => x.Dict.Code == "CarAlarm").FirstOrDefault(e => e.Value == i);
                        if (carAlarm is not null)
                        {
                            alarmCodeDic.Add(i, carAlarm.Name!);
                        }
                    }
                }
                if (cartAlarmCodeMap is not null && cartAlarmCodeMap.ContainsKey(cart.Code))
                {
                    var last = cartAlarmCodeMap[cart.Code];
                    // 扫描解除的报警
                    if (alarmTriggerRecord is not null && alarmTriggerRecord.ContainsKey(cart.Code))
                    {
                        foreach (var alarmCode in alarmTriggerRecord[cart.Code].Keys)
                        {
                            var triggerCase = !alarmCodeDic.ContainsKey(alarmCode);
                            CommonHelper.TriggerOnce(triggerCase, AlarmDuration, () =>
                            {
                                var mode = new AlarmMdcs
                                {
                                    CarCode = cart.Code,
                                    Code = alarmCode.ToString(),
                                    EndTime = DateTime.Now
                                };
                                modelUpdateList.Add(mode);
                                alarmTriggerRecord[cart.Code].Remove(alarmCode);
                            }, cart.Code.ToInt(), $"{cart.Code}||{alarmCode}");
                        }
                    }
                    else
                    {
                        //不在触发记录缓存中，直接根据根据上一个点的报警状态和当前报警状态来结束报警
                        foreach (var alarmCode in last.Keys)
                        {
                            if (!alarmCodeDic.ContainsKey(alarmCode))
                            {
                                var mode = new AlarmMdcs
                                {
                                    CarCode = cart.Code,
                                    Code = alarmCode.ToString(),
                                    EndTime = DateTime.Now
                                };
                                modelUpdateList.Add(mode);
                            }
                        }
                    }

                    // 扫描触发的报警
                    foreach (var alarmCode in alarmCodeDic.Keys)
                    {
                        var triggerCase = last.ContainsKey(alarmCode);
                        CommonHelper.TriggerOnce(triggerCase, AlarmDuration, () =>
                        {
                            if (alarmTriggerRecord is not null && alarmTriggerRecord.ContainsKey(cart.Code))
                            {
                                if (!alarmTriggerRecord[cart.Code].ContainsKey(alarmCode))
                                {
                                    alarmTriggerRecord[cart.Code].Add(alarmCode, DateTime.Now);
                                    var mode = new AlarmMdcs
                                    {
                                        CarCode = cart.Code,
                                        CarName = cart.Name,
                                        Code = alarmCode.ToString(),
                                        Name = alarmCodeDic[alarmCode],
                                        StartTime = DateTime.Now
                                    };
                                    modelAddList.Add(mode);
                                }
                            }
                            else
                            {
                                alarmTriggerRecord?.Add(cart.Code, new Dictionary<int, DateTime>() { { alarmCode, DateTime.Now } });
                                var mode = new AlarmMdcs
                                {
                                    CarCode = cart.Code,
                                    CarName = cart.Name,
                                    Code = alarmCode.ToString(),
                                    Name = alarmCodeDic[alarmCode],
                                    StartTime = DateTime.Now
                                };
                                modelAddList.Add(mode);
                            }
                        }, cart.Code.ToInt(), $"{cart.Code}||{alarmCode}");
                    }

                    cartAlarmCodeMap[cart.Code] = alarmCodeDic;
                }
                else
                {
                    cartAlarmCodeMap?.Add(cart.Code, alarmCodeDic);
                }
            }

            //批量更新
            if (modelUpdateList.Count > 0)
            {
                _alarmMdcsService.UpdateModels(modelUpdateList);
                modelUpdateList.Clear();
            }
            //批量插入
            if (modelAddList.Count > 0)
            {
                _alarmMdcsService.AddModels(modelAddList);
                modelAddList.Clear();
            }

            //_distributedCache.SetString("StandardAlarm.cartAlarmCodeMap", cartAlarmCodeMap.ToJson());
            //_distributedCache.SetString("StandardAlarm.alarmTriggerRecord", alarmTriggerRecord.ToJson());
        }

    }
}
