using Common.NETCore.Extensions;
using Common.NETCore.Helpers;
using FASS.Boot.Services.Cars;
using FASS.Boot.Services.Cars.Interfaces;
using FASS.Data.Consts.Record;
using FASS.Data.Models.Data;
using FASS.Data.Models.Record;
using FASS.Extend.Car.Fairyland.Plc;
using FASS.Scheduler.Exceptions;
using FASS.Scheduler.Models;
using System.Net;
using TcpClient = Common.Net.Tcp.TcpClient;

namespace FASS.Scheduler.Services.Cars.Fairyland.Plc
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
            _carCommand = new Command(new IPEndPoint(IPAddress.Parse(_carService.Car.IpAddress), _carService.Car.Port));
            _carCommand.Client.Connected += Client_Connected;
            _carCommand.Client.Disconnected += Client_Disconnected;
            _carCommand.Client.Sent += Client_Sent;
            _carCommand.Client.Received += Client_Received;
            //CarCommand.Client.ConnectAndReceive();
            return _carCommand;
        }

        private void Client_Connected(TcpClient client)
        {
            _carService.Car.IsOnline = true;
        }

        private void Client_Disconnected(TcpClient client)
        {
            _carService.Car.IsOnline = false;
        }

        private void Client_Sent(TcpClient client, byte[] sendByteArray)
        {
            _logger.LogDebug($"地址[{client.RemoteEndPoint}] [通讯记录] 发送[{ByteHelper.ByteArrayToHexString(sendByteArray)}]");
        }

        private void Client_Received(TcpClient client, byte[] receiveByteArray)
        {
            _logger.LogDebug($"地址[{client.RemoteEndPoint}] [通讯记录] 接收[{ByteHelper.ByteArrayToHexString(receiveByteArray)}]");
            Car? currCar = null;
            try
            {
                currCar = GetCarState(receiveByteArray);
                if (currCar is not null) currCar.IsNormal = true;
            }
            catch (AlarmException ex)
            {
                currCar = _carService.Car;
                if (currCar is not null)
                {
                    var alarm = new Alarm()
                    {
                        Level = AlarmConst.Level.Warning,
                        Type = AlarmConst.Type.CarAlarm,
                        Code = currCar.Code,
                        State = "31",
                        Message = $"[{currCar.Code}] [地标读取错误报警]",
                        Data = currCar.ToJson()
                    };
                    currCar.Alarms.Add(alarm);
                    currCar.IsNormal = false;
                }
                _logger.LogError($"车辆[{currCar?.Code}] {ex.Message} 信息[{ex}]");
            }
            catch (Exception ex)
            {
                if (currCar is not null) currCar.IsNormal = false;
                _logger.LogError($"车辆[{currCar?.Code}] {ex.Message} 信息[{ex}]");
            }
            finally
            {
                if (currCar is not null) currCar.IsOnline = true;
            }
        }

        public Car GetCarState(byte[] data)
        {
            var message = Command.GetReceiveStateMessage(data);
            if (message is null)
            {
                throw new Exception($"[报文错误] 报文[{ByteHelper.ByteArrayToHexString(data)}]");
            }
            var currCar = _carService.Car;
            if (currCar is null)
            {
                throw new Exception($"[车辆错误] 消息[{message.ToJson()}]");
            }
            if (message.Length > 0)
            {
                currCar.Length = message.Length;
            }
            if (message.Width > 0)
            {
                currCar.Width = message.Width;
            }
            currCar.Battery = message.BatteryCharge;
            var node = _carService.BootService.Nodes.FirstOrDefault(e => e.Code == message.NodeMessage.Node.ToString());
            if (node is null)
            {
                throw new AlarmException($"[站点错误] 当前站点[{currCar.CurrNode?.Code}] 上报站点[{message.NodeMessage.Node}]", currCar.Code);
            }
            if (node != currCar.CurrNode)
            {
                currCar.PrevNode = currCar.CurrNode;
                currCar.PrevNodeId = currCar.CurrNodeId;
                currCar.CurrNode = node;
                currCar.CurrNodeId = node.Id;
            }
            if (currCar.CurrNode is not null)
            {
                currCar.X = currCar.CurrNode.NodePosition.X;
                currCar.Y = currCar.CurrNode.NodePosition.Y;
            }
            var edge = currCar.CarEdges.FirstOrDefault(e => e.Edge.StartNode == currCar.CurrNode)?.Edge;
            if (edge != currCar.CurrEdge)
            {
                currCar.PrevEdge = currCar.CurrEdge;
                currCar.PrevEdgeId = currCar.CurrEdgeId;
                currCar.CurrEdge = edge;
                currCar.CurrEdgeId = edge?.Id;
            }
            if (currCar.CurrEdge is not null)
            {
                var point = MathHelper.GetBeelinePoint(currCar.CurrEdge.StartNode.NodePosition.X, currCar.CurrEdge.StartNode.NodePosition.Y, currCar.CurrEdge.EndNode.NodePosition.X, currCar.CurrEdge.EndNode.NodePosition.Y, message.NodeMessage.Distance / currCar.CurrEdge.Length);
                currCar.X = point.x;
                currCar.Y = point.y;
            }
            //var direction = _carService.BootService.DictItems.Where(x => x.Dict.Code == "CarDirection").FirstOrDefault(e => e.Value == message.NodeMessage.Direction);
            //if (direction is null)
            //{
            //    throw new Exception($"[方向错误] 当前角度[{currCar.Theta}] 上报方向[{message.NodeMessage.Direction}]");
            //}
            //currCar.Theta = direction.Param.ToDouble();
            if (message.HeadingAngle > 0)
            {
                currCar.Theta = message.HeadingAngle;
            }
            if (_carService.BootService.TryUpdateCurrEdgePosture(currCar, message.NodeMessage.Distance, out var car))
            {
                currCar = car;
            }
            var state = _carService.BootService.DictItems.Where(x => x.Dict.Code == "CarState").FirstOrDefault(e => e.Value == message.State);
            if (state is null)
            {
                throw new Exception($"[状态错误] 当前状态[{currCar.CurrState}] 上报状态[{message.State}]");
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
            currCar.Speed = message.NodeMessage.Speed * 0.1;
            var byteArray = BitConverter.GetBytes(message.Alarm);
            var boolArray = ByteHelper.ByteArrayToBoolArray(byteArray);
            for (var i = 0; i < boolArray.Length; i++)
            {
                if (boolArray[i])
                {
                    var carAlarm = _carService.BootService.DictItems.Where(x => x.Dict.Code == "CarAlarm").FirstOrDefault(e => e.Value == i);
                    if (carAlarm is not null)
                    {
                        var alarm = new Alarm
                        {
                            Level = AlarmConst.Level.Warning,
                            Type = AlarmConst.Type.CarAlarm,
                            Code = currCar.Code,
                            State = carAlarm.Code,
                            Message = $"[{currCar.Code}] [{carAlarm.Name}]",
                            Data = currCar.ToJson()
                        };
                        currCar.Alarms.Add(alarm);
                    }
                }
            }
            currCar.CarState = message.ToJson();
            return currCar;
        }

        public void SendState(byte command, ushort car)
        {
            try
            {
                _carCommand.SendState(command, car);
                _carService.Car.IsOnline = true;
            }
            catch (Exception ex)
            {
                _carService.Car.IsOnline = false;
                _logger.LogError($"车辆[{_carService.Car.Code}] [通讯错误] 地址[{_carCommand.Client.RemoteEndPoint}] 信息[{ex}]");
            }
        }
        public void SendState() => SendState(0x00, 0);

        public void SendControl(byte command, ushort car, ushort param)
        {
            try
            {
                _carCommand.SendControl(command, car, param);
                _carService.Car.IsOnline = true;
            }
            catch (Exception ex)
            {
                _carService.Car.IsOnline = false;
                _logger.LogError($"车辆[{_carService.Car.Code}] [通讯错误] 地址[{_carCommand.Client.RemoteEndPoint}] 信息[{ex}]");
            }
        }
        public void SendControlStart(ushort carCode = 0, ushort direction = 0x00) => SendControl(0x01, carCode, direction);
        public void SendControlStop(ushort carCode = 0, ushort second = 0x00) => SendControl(0x02, carCode, second);
        public void SendControlEmergencyStop(ushort carCode = 0, ushort second = 0x00) => SendControl(0x03, carCode, second);
        public void SendControlReset(ushort carCode = 0, ushort second = 0x00) => SendControl(0x04, carCode, second);
        public void SendControlRest(ushort carCode = 0, ushort second = 0x00) => SendControl(0x05, carCode, second);
        public void SendControlShutdown(ushort carCode = 0, ushort second = 0x00) => SendControl(0x06, carCode, second);

        public void SendAction(ushort car, ulong actionId, NodeMessage nodeMessage)
        {
            try
            {
                _carCommand.SendAction(car, actionId, nodeMessage);
                _carService.Car.IsOnline = true;
            }
            catch (Exception ex)
            {
                _carService.Car.IsOnline = false;
                _logger.LogError($"车辆[{_carService.Car.Code}] [通讯错误] 地址[{_carCommand.Client.RemoteEndPoint}] 信息[{ex}]");
            }
        }
        public void SendAction(NodeMessage nodeMessage) => SendAction(0, 0, nodeMessage);
        public void SendActionStartStop(ushort car, ushort node, byte startStop, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, StartStop = startStop });
        public void SendActionDirection(ushort car, ushort node, byte direction, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Direction = direction });
        public void SendActionOrientation(ushort car, ushort node, byte orientation, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Orientation = orientation });
        public void SendActionByroad(ushort car, ushort node, byte byroad, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Byroad = byroad });
        public void SendActionSpeed(ushort car, ushort node, ushort speed, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Speed = speed });
        public void SendActionObstacle(ushort car, ushort node, byte obstacle, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Obstacle = obstacle });
        public void SendActionAudio(ushort car, ushort node, byte audio, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Audio = audio });
        public void SendActionLight(ushort car, ushort node, byte light, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Light = light });
        public void SendActionCharge(ushort car, ushort node, byte charge, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Charge = charge });
        public void SendActionRest(ushort car, ushort node, byte sleep, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Rest = sleep });
        public void SendActionLift(ushort car, ushort node, byte lift, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Lift = lift });
        public void SendActionClamp(ushort car, ushort node, byte clamp, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Clamp = clamp });
        public void SendActionTray(ushort car, ushort node, byte tray, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Tray = tray });
        public void SendActionRoll(ushort car, ushort node, byte roll, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Roll = roll });
        public void SendActionShutdown(ushort car, ushort node, byte shutdown, ulong actionId) => SendAction(car, actionId, new NodeMessage() { Node = node, Shutdown = shutdown });

        public void SendNodes(ushort car, ulong task, ushort count, NodeMessage[] nodeMessages)
        {
            try
            {
                _carCommand.SendNodes(car, task, count, nodeMessages);
                _carService.Car.IsOnline = true;
            }
            catch (Exception ex)
            {
                _carService.Car.IsOnline = false;
                _logger.LogError($"车辆[{_carService.Car.Code}] [通讯错误] 地址[{_carCommand.Client.RemoteEndPoint}] 信息[{ex}]");
            }
        }
        public void SendNodes(ulong task, ushort count, NodeMessage[] nodeMessages) => SendNodes(0, task, count, nodeMessages);
    }
}