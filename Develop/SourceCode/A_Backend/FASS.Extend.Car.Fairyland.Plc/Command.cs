using System.Net;
using TcpClient = Common.Net.Tcp.TcpClient;

namespace FASS.Extend.Car.Fairyland.Plc
{
    public class Command
    {
        public TcpClient Client { get; private set; }

        public Command(IPEndPoint remote)
        {
            Client = new TcpClient()
            {
                RemoteEndPoint = remote,
                ConnectTimeout = 500
            };
        }

        public byte[]? SendState(byte command, ushort car)
        {
            var sendMessage = new SendControlMessage().SetMessage(command, car, 0);
            var sendByteArray = sendMessage.GetByteArray();
            var receiveByteArray = Client.SendAndReceive(sendByteArray);
            return receiveByteArray;
        }
        public byte[]? SendState() => SendState(0x00, 0);

        public static ReceiveStateMessage GetReceiveStateMessage(byte[] byteArray) => new ReceiveStateMessage().GetMessage(byteArray);
        public static byte[] GetReceiveStateByteArray(ReceiveStateMessage message) => message.GetByteArray();

        public byte[]? SendControl(byte command, ushort car, ushort param)
        {
            var sendMessage = new SendControlMessage().SetMessage(command, car, param);
            var sendByteArray = sendMessage.GetByteArray();
            var receiveByteArray = Client.SendAndReceive(sendByteArray);
            return receiveByteArray;
        }
        public byte[]? SendControlStart(ushort direction = 0x00) => SendControl(0x01, 0, direction);
        public byte[]? SendControlStop(ushort second = 0x00) => SendControl(0x02, 0, second);
        public byte[]? SendControlEmergencyStop(ushort second = 0x00) => SendControl(0x03, 0, second);
        public byte[]? SendControlReset(ushort second = 0x00) => SendControl(0x04, 0, second);
        public byte[]? SendControlRest(ushort second = 0x00) => SendControl(0x05, 0, second);
        public byte[]? SendControlShutdown(ushort second = 0x00) => SendControl(0x06, 0, second);

        public static SendControlMessage GetSendControlMessage(byte[] byteArray) => new SendControlMessage().GetMessage(byteArray);
        public static byte[] GetSendControlByteArray(SendControlMessage message) => message.GetByteArray();

        public byte[]? SendAction(ushort car, ulong actionId, NodeMessage nodeMessage)
        {
            var sendMessage = new SendActionMessage().SetMessage(car, actionId, nodeMessage);
            var sendByteArray = sendMessage.GetByteArray();
            var receiveByteArray = Client.SendAndReceive(sendByteArray);
            return receiveByteArray;
        }
        public byte[]? SendAction(NodeMessage nodeMessage) => SendAction(0, 0, nodeMessage);
        public byte[]? SendActionStartStop(ushort node, byte startStop) => SendAction(0, 0, new NodeMessage() { Node = node, StartStop = startStop });
        public byte[]? SendActionDirection(ushort node, byte direction) => SendAction(0, 0, new NodeMessage() { Node = node, Direction = direction });
        public byte[]? SendActionOrientation(ushort node, byte orientation) => SendAction(0, 0, new NodeMessage() { Node = node, Orientation = orientation });
        public byte[]? SendActionByroad(ushort node, byte byroad) => SendAction(0, 0, new NodeMessage() { Node = node, Byroad = byroad });
        public byte[]? SendActionSpeed(ushort node, ushort speed) => SendAction(0, 0, new NodeMessage() { Node = node, Speed = speed });
        public byte[]? SendActionObstacle(ushort node, byte obstacle) => SendAction(0, 0, new NodeMessage() { Node = node, Obstacle = obstacle });
        public byte[]? SendActionAudio(ushort node, byte audio) => SendAction(0, 0, new NodeMessage() { Node = node, Audio = audio });
        public byte[]? SendActionLight(ushort node, byte light) => SendAction(0, 0, new NodeMessage() { Node = node, Light = light });
        public byte[]? SendActionCharge(ushort node, byte charge) => SendAction(0, 0, new NodeMessage() { Node = node, Charge = charge });
        public byte[]? SendActionRest(ushort node, byte rest) => SendAction(0, 0, new NodeMessage() { Node = node, Rest = rest });
        public byte[]? SendActionLift(ushort node, byte lift) => SendAction(0, 0, new NodeMessage() { Node = node, Lift = lift });
        public byte[]? SendActionClamp(ushort node, byte clamp) => SendAction(0, 0, new NodeMessage() { Node = node, Clamp = clamp });
        public byte[]? SendActionTray(ushort node, byte tray) => SendAction(0, 0, new NodeMessage() { Node = node, Tray = tray });
        public byte[]? SendActionRoll(ushort node, byte roll) => SendAction(0, 0, new NodeMessage() { Node = node, Roll = roll });
        public byte[]? SendActionShutdown(ushort node, byte shutdown) => SendAction(0, 0, new NodeMessage() { Node = node, Shutdown = shutdown });

        public static SendActionMessage GetSendActionMessage(byte[] byteArray) => new SendActionMessage().GetMessage(byteArray);
        public static byte[] GetSendActionByteArray(SendActionMessage message) => message.GetByteArray();

        public byte[]? SendNodes(ushort car, ulong task, ushort count, NodeMessage[] nodeMessages)
        {
            var sendMessage = new SendNodesMessage().SetMessage(car, task, count, nodeMessages);
            var sendByteArray = sendMessage.GetByteArray();
            var receiveByteArray = Client.SendAndReceive(sendByteArray);
            return receiveByteArray;
        }
        public byte[]? SendNodes(ulong task, ushort count, NodeMessage[] nodeMessages) => SendNodes(0, task, count, nodeMessages);

        public static SendNodesMessage GetSendNodesMessage(byte[] byteArray) => new SendNodesMessage().GetMessage(byteArray);
        public static byte[] GetSendNodesByteArray(SendNodesMessage message) => message.GetByteArray();
    }
}