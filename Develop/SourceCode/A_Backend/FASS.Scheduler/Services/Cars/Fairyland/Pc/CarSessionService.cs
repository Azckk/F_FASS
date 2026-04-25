using Common.NETCore.Extensions;
using Common.NETCore.Models;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Record;
using FASS.Data.Models.Data;
using FASS.Data.Models.Record;
using FASS.Extend.Car.Fairyland.Pc;
using FASS.Scheduler.Exceptions;
using FASS.Scheduler.Models;
using HttpClient = Common.Net.Http.HttpClient;

namespace FASS.Scheduler.Services.Cars.Fairyland.Pc
{
    public class CarSessionService : ICarSessionService
    {
        private readonly ILogger<CarSessionService> _logger;
        private readonly AppSettings _appSettings;
        private CarService _carService = null!;
        private Command _carCommand = null!;

        public CarSessionService(
            ILogger<CarSessionService> logger,
            AppSettings appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public void Initialize(CarService carService)
        {
            _carService = carService;
            _carCommand ??= GetCarCommand();
        }

        public Command GetCarCommand()
        {
            //_carCommand = new Command(new Uri($"http://{_carService.Car.IpAddress}:{_carService.Car.Port}"));
            _carCommand = new Command(new Uri(_appSettings.Mdcs.SimpleUrl));//初始化为simple地址
            return _carCommand;
        }

        private void Client_Sent(HttpClient client, string request)
        {
            _logger.LogDebug($"地址[{client.BaseAddress}] [通讯记录] 发送[{request}]");
        }

        private void Client_Received(HttpClient client, string response)
        {
            _logger.LogDebug($"地址[{client.BaseAddress}] [通讯记录] 接收[{response}]");
            Car? currCar = null;
            try
            {
                currCar = GetCarState(response);
                if (currCar != null) currCar.IsNormal = true;
            }
            catch (InitException ex)
            {
                currCar = _carService.Car;
                if (currCar != null)
                {
                    currCar.IsOnline = false;
                }
                _logger.LogError($"车辆[{currCar?.Code}] 信息[{ex}]");
            }
            catch (AlarmException ex)
            {
                currCar = _carService.Car;
                if (currCar != null)
                {
                    currCar.IsNormal = false;
                }
                _logger.LogError($"车辆[{currCar?.Code}] 信息[{ex}]");
            }
            catch (Exception ex)
            {
                if (currCar != null) currCar.IsNormal = false;
                _logger.LogError($"车辆[{currCar?.Code}] {ex.Message} 信息[{ex}]");
            }
            //finally
            //{
            //    if (currCar != null) currCar.IsOnline = true;
            //}
        }

        public Car GetCarState(string data)
        {
            var message = data.JsonTo<ResponseResult<Extend.Car.Fairyland.Pc.Models.Response.CarState>>()?.Data;
            var currCar = _carService.Car;
            if (currCar == null)
            {
                throw new Exception($"[车辆错误] 消息[{message?.ToJson()}]");
            }
            if (message == null)
            {
                throw new Exception($"[报文错误] 报文[{data}]");
            }
            if (message.Code == null)
            {
                throw new InitException($"[报文错误] 报文[{data}]", currCar.Code);
            }
            currCar.IsOnline = message.IsOnline;
            if (message.Length > 0)
            {
                currCar.Length = message.Length;
            }
            if (message.Width > 0)
            {
                currCar.Width = message.Width;
            }
            currCar.Battery = message.Battery;
            var node = _carService.BootService.Nodes.FirstOrDefault(e => e.Code == message.CurrNodeCode);
            if (node is null)
            {
                throw new AlarmException($"[站点错误] 当前站点[{currCar.CurrNode?.Code}] 上报站点[{message.CurrNodeCode}]", currCar.Code);
            }
            if (node != currCar.CurrNode)
            {
                if (currCar.CurrNode != null)
                {
                    currCar.PrevNode = currCar.CurrNode;
                    currCar.PrevNodeId = currCar.CurrNodeId;
                }
                currCar.CurrNode = node;
                currCar.CurrNodeId = node.Id;
            }
            if (currCar.CurrNode is not null)
            {
                //currCar.X = currCar.CurrNode.NodePosition.X;
                //currCar.Y = currCar.CurrNode.NodePosition.Y;

                currCar.X = message.X;
                currCar.Y = message.Y;
                currCar.Theta = message.Theta;
            }
            var edge = currCar.CarEdges.FirstOrDefault(e => e.Edge.StartNode == currCar.CurrNode)?.Edge;
            if (edge != currCar.CurrEdge)
            {
                if (currCar.CurrEdge != null)
                {
                    currCar.PrevEdge = currCar.CurrEdge;
                    currCar.PrevEdgeId = currCar.CurrEdgeId;
                }
                currCar.CurrEdge = edge;
                currCar.CurrEdgeId = edge?.Id;
            }
            //var direction = CarService.BootService.DictItems.Where(x => x.Dict.Code == "CarDirection").FirstOrDefault(e => e.Value == message.NodeMessage.Direction);
            //if (direction == null)
            //{
            //    throw new Exception($"[方向错误] 当前角度[{currCar?.Theta}] 上报方向[{message.NodeMessage.Direction}]");
            //}
            //currCar.Theta = direction.Param.ToDouble();
            //if (currCar.CurrEdge != null)
            //{
            //    var point = MathHelper.GetBeelinePoint(currCar.CurrEdge.StartNode.NodePosition.X, currCar.CurrEdge.StartNode.NodePosition.Y, currCar.CurrEdge.EndNode.NodePosition.X, currCar.CurrEdge.EndNode.NodePosition.Y, message.NodeMessage.Distance / currCar.CurrEdge.Length);
            //    currCar.X = point.x;
            //    currCar.Y = point.y;
            //}
            //if (CarService.BootService.TryUpdateCurrEdgePosture(currCar, message.NodeMessage.Distance, out var car))
            //{
            //    currCar = car;
            //}

            var state = _carService.BootService.DictItems.Where(x => x.Dict.Code == "CarState").FirstOrDefault(e => e.Code == message.CurrState);
            if (state is null)
            {
                throw new Exception($"[状态错误] 车辆[{currCar?.Code}] 当前状态[{currCar?.CurrState}] 上报状态[{message.CurrState}]");
            }
            if (state.Code != currCar.CurrState)
            {
                currCar.PrevState = currCar.CurrState;
                currCar.CurrState = state.Code;

                var diary = new Diary
                {
                    Level = DiaryConst.Level.Information,
                    Type = DiaryConst.Type.CarState,
                    Code = currCar.Code,
                    State = state.Code,
                    Message = $"[{currCar.Code}] [{state.Name}]",
                    Data = currCar.ToJson()
                };
                currCar.Diarys.Add(diary);
            }
            //var speed = CarService.BootService.DictItems.Where(x => x.Dict.Code == "CarSpeed").FirstOrDefault(e => e.Value == message.Speed);
            //if (speed == null)
            //{
            //    throw new Exception($"状态错误：车辆[{currCar.Code}] 当前速度[{currCar?.Speed}] 上报速度[{message.Speed}]");
            //}
            currCar.Speed = message.Speed;
            foreach (var a in message.Alarms)
            {
                var alarm = new Alarm
                {
                    Level = AlarmConst.Level.Warning,
                    Type = AlarmConst.Type.CarAlarm,
                    Code = currCar.Code,
                    State = a.Code,
                    Message = $"[{currCar.Code}] [{a.Name}]",
                    Data = currCar.ToJson()
                };
                currCar.Alarms.Add(alarm);
            }
            
            currCar.CarState = message.ToJson();
            return currCar;
        }

        public void Send(Func<string, string> func, string request)
        {
            try
            {
                Client_Sent(_carCommand.Client, request);
                var response = func.Invoke(request);
                Client_Received(_carCommand.Client, response);
                //_carService.Car.IsOnline = true;
            }
            catch (Exception ex)
            {
                _carService.Car.IsOnline = false;
                _logger.LogError($"车辆[{_carService.Car.Code}] [通讯错误] 地址[{_carCommand.Client.BaseAddress}] 信息[{ex}]");
            }
        }

        public void SendStart() => Send(_carCommand.Start, new Extend.Car.Fairyland.Pc.Models.Request.CarStart() { CarCode = _carService.Car.Code }.ToJson());

        public void SendStop(int dueTime = 0) => Send(_carCommand.Stop, new Extend.Car.Fairyland.Pc.Models.Request.CarStop() { CarCode = _carService.Car.Code, DueTime = dueTime }.ToJson());

        public void SendEmergencyStop() => Send(_carCommand.EmergencyStop, new Extend.Car.Fairyland.Pc.Models.Request.CarEmergencyStop() { CarCode = _carService.Car.Code }.ToJson());

        public void SendReset() => Send(_carCommand.Reset, new Extend.Car.Fairyland.Pc.Models.Request.CarReset() { CarCode = _carService.Car.Code }.ToJson());

        public void SendAction(Extend.Car.Fairyland.Pc.Models.Request.CarAction carAction) => Send(_carCommand.Action, carAction.ToJson());

        public void SendTask(Extend.Car.Fairyland.Pc.Models.Request.CarTask carTask) => Send(_carCommand.Task, carTask.ToJson());

        public void SendState() => Send(_carCommand.State, new Extend.Car.Fairyland.Pc.Models.Request.CarState() { CarCode = _carService.Car.Code }.ToJson());
    }
}