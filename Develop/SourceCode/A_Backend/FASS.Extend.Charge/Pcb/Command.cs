using Common.Net.Udp;
using FASS.Data.Models.Data;
using FASS.Extend.Charge.Pcb.Model;
using FASS.Service.Dtos.DataExtend;
using System.Collections.Concurrent;
using System.Net;

namespace FASS.Extend.Charge.Pcb
{
    public class Command
    {
        public int listenPort = 40001;

        /// <summary>
        /// 所有充电站集合
        /// </summary>
        public static Dictionary<int, Charger> ChargeStationPairs = new Dictionary<int, Charger>();

        /// <summary>
        /// 存储站点ID和通讯实体类之间的关系
        /// </summary>
        private Dictionary<int, AsyncUdpClient> stations = new Dictionary<int, AsyncUdpClient>();

        /// <summary>
        /// 存储站点IP和站点ID之间的关系
        /// </summary>
        private Dictionary<string, int> ipList = new Dictionary<string, int>();

        public ConcurrentDictionary<int, byte> chargeSafe = new ConcurrentDictionary<int, byte>();

        //充电点位信息
        #region
        public Command(List<ChargingStationDto> ChargeStations, List<Car> cars)
        {
            new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        //遍历所有的充电点位
                        foreach (var cs in ChargeStations)
                        {
                            AddCommunication(int.Parse(cs.ChargeId), cs.Ip, 2000);
                            // var openCharge = 0;
                            var car = cars.FirstOrDefault(cCar => cCar.CurrNodeId == cs.ChargeId);
                            //充电电有车且电量<98
                            if (car != null && car.Battery < 98)
                            {
                                SwitchCharge(int.Parse(cs.ChargeId), 1, car);
                            }
                            else if (car != null && car.Battery >= 98)
                            {
                                SwitchCharge(int.Parse(cs.ChargeId), 0, car);
                            }
                            else
                            {
                                SwitchCharge(int.Parse(cs.ChargeId), 0);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        // Diagnosis.Post($"{ExceptionFormatter.FormatEx(ex)}");
                    }
                    Thread.Sleep(500);
                }
            }).Start();
            ;
        }
        #endregion

        public UdpServer Server { get; private set; }

        public EndPoint Remote { get => Server.RemoteEndPoint; set => Server.RemoteEndPoint = value; }

        public Command(IPEndPoint local)
        {
            Server = new UdpServer()
            {
                LocalEndPoint = local
            };
        }

        public void SendState(
            byte command,
            ushort car,
            float chargeElectric,
            float chargeVoltage,
            ushort chargeTimeSpan,
            ushort soc,
            float electricCurrent,
            float voltage, int chargeIndex)
        {
            var sendMessage = new SendMessage().SetMessage(command, car, chargeElectric, chargeVoltage, chargeTimeSpan, soc, electricCurrent, voltage, chargeIndex);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }
        public void SwitchCharge(int id, int charge, Car? car = null)
        {
            try
            {
                float soc = 0f,
                    voltage = 0,
                    electricCurrent = 0,
                    chargeTimeSpan = 30f;
                ushort carId = 0;
                if (car != null)
                {
                    soc = (float)car.Battery;
                    carId = ushort.Parse(car.Code);
                    electricCurrent = 0.5f;
                }

                var sendMessage = new SendMessage().SetMessage(
                    (byte)charge,
                    carId,
                    10.0f,
                    29.0f,
                    (ushort)chargeTimeSpan,
                    (ushort)soc,
                    electricCurrent,
                    voltage,
                    id
                );
                Console.WriteLine($"SwitchCharge:id:{id}--SendMsg--{string.Join(",", sendMessage.GetByteArray())}");
                stations[id].SendMsg(sendMessage.GetByteArray());

                /*  Diagnosis.Post(
                      $"SwitchCharge:id:{id}--SendMsg--{string.Join(",", sendMessage.GetByteArray())}",
                      "充电",
                      true
                  );*/
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                /*  Diagnosis.Post(
                      $"SwitchCharge:id:{id}--{ExceptionFormatter.FormatEx(ex)}",
                      "充电",
                      true
                  );*/
            }
        }

        /// <summary>
        /// 添加充电桩通信对象。指定id对应的ip地址。
        /// </summary>
        /// <param name="id">站点id</param>
        /// <param name="ip">充电桩IP</param>
        public void AddCommunication(int id, string ip, int remotePort)
        {
            try
            {
                if (ipList.ContainsKey(ip))
                    return;
                //如果集合中没有则添加
                ipList[ip] = id;
                chargeSafe[ipList[ip]] = 0;
                var client = new AsyncUdpClient(ip, remotePort, listenPort);
                listenPort++;
                client.OnMsgChange += OnMsgChange;
                //向充电站点和交互对象的集合中添加
                stations[id] = client;

                //充电点和充电状态关系集合
                ChargeStationPairs[ipList[ip]] = new Charger
                {
                    SiteId = id,
                    Ip = ip,
                    LinkState = false,
                    ChargeState = ChargeState.Idle,
                    ElectrodeState = ElectrodeState.None,
                    ChargeVoltage = 0,
                    ChargeElectricity = 0,
                    ChargingCapacity = 0,
                    LastRecvTime = DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                /*     Diagnosis.Post(
                         $"AddCommunication:ip:{ip}--{ExceptionFormatter.FormatEx(ex)}",
                         "充电",
                         true
                     );*/
            }
        }

        private void OnMsgChange(AsyncUdpClient.MyEventArgs e)
        {
            try
            {
                var bytes = e.Msg;
                ReceiveMessage receiveMessage = new ReceiveMessage().GetMessage(e.Msg);
                chargeSafe[ipList[e.IP]] = bytes[28];
                ChargeStationPairs[ipList[e.IP]] = new Charger
                {
                    SiteId = ipList[e.IP],
                    Ip = e.IP,
                    LinkState = true,
                    ChargeState =
                        receiveMessage.ChargeState == 0
                            ? ChargeState.None
                            : receiveMessage.ChargeState == 1
                                ? ChargeState.Charging
                                : receiveMessage.ChargeState == 2
                                    ? ChargeState.Alarming
                                    : receiveMessage.ChargeState == 3
                                        ? ChargeState.BatteryConnected
                                        : ChargeState.None,
                    ElectrodeState =
                        receiveMessage.ElectrodeState == 1
                            ? ElectrodeState.Retracted
                            : receiveMessage.ElectrodeState == 2
                                ? ElectrodeState.Extended
                                : receiveMessage.ElectrodeState == 3
                                    ? ElectrodeState.Runing
                                    : ElectrodeState.None,
                    ChargeVoltage = receiveMessage.CurrentVoltage,
                    ChargeElectricity = receiveMessage.CurrentElectric,
                    ChargingCapacity = receiveMessage.AccumulateCharging,
                    LastRecvTime = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                /*   Diagnosis.Post(
                       $"OnMsgChange:ip:{e.IP}--{ExceptionFormatter.FormatEx(ex)}",
                       "充电",
                       true
                   );*/
            }
        }
    }
}
