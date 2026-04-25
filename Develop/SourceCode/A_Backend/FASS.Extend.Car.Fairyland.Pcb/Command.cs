using System.Net;
using UdpServer = Common.Net.Udp.UdpServer;

namespace FASS.Extend.Car.Fairyland.Pcb
{
    public class Command
    {
        public UdpServer Server { get; private set; }

        public EndPoint Remote { get => Server.RemoteEndPoint; set => Server.RemoteEndPoint = value; }

        public Command(IPEndPoint local)
        {
            Server = new UdpServer()
            {
                LocalEndPoint = local
            };
        }

        public void SendState(byte command, ushort car)
        {
            var sendMessage = new SendControlMessage().SetMessage(command, car, 0);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }
        public void SendState() => SendState(0x00, 0);

        public static ReceiveStateMessage GetReceiveStateMessage(byte[] byteArray) => new ReceiveStateMessage().GetMessage(byteArray);
        public static byte[] GetReceiveStateByteArray(ReceiveStateMessage message) => message.GetByteArray();

        public void SendControl(byte command, ushort car, ushort param)
        {
            var sendMessage = new SendControlMessage().SetMessage(command, car, param);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }
        public void SendControlStart(ushort direction = 0x00) => SendControl(0x01, 0, direction);
        public void SendControlStop(ushort second = 0x00) => SendControl(0x02, 0, second);
        public void SendControlEmergencyStop(ushort second = 0x00) => SendControl(0x03, 0, second);
        public void SendControlReset(ushort second = 0x00) => SendControl(0x04, 0, second);
        public void SendControlRest(ushort second = 0x00) => SendControl(0x05, 0, second);
        public void SendControlShutdown(ushort second = 0x00) => SendControl(0x06, 0, second);

        public static SendControlMessage GetSendControlMessage(byte[] byteArray) => new SendControlMessage().GetMessage(byteArray);
        public static byte[] GetSendControlByteArray(SendControlMessage message) => message.GetByteArray();

        public void SendAction(ushort car, ulong actionId, NodeMessage nodeMessage)
        {
            var sendMessage = new SendActionMessage().SetMessage(car, actionId, nodeMessage);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }
        public void SendAction(NodeMessage nodeMessage) => SendAction(0, 0, nodeMessage);
        public void SendActionStartStop(ushort node, byte startStop) => SendAction(0, 0, new NodeMessage() { Node = node, StartStop = startStop });
        public void SendActionDirection(ushort node, byte direction) => SendAction(0, 0, new NodeMessage() { Node = node, Direction = direction });
        public void SendActionOrientation(ushort node, byte orientation) => SendAction(0, 0, new NodeMessage() { Node = node, Orientation = orientation });
        public void SendActionByroad(ushort node, byte byroad) => SendAction(0, 0, new NodeMessage() { Node = node, Byroad = byroad });
        public void SendActionSpeed(ushort node, ushort speed) => SendAction(0, 0, new NodeMessage() { Node = node, Speed = speed });
        public void SendActionObstacle(ushort node, byte obstacle) => SendAction(0, 0, new NodeMessage() { Node = node, Obstacle = obstacle });
        public void SendActionAudio(ushort node, byte audio) => SendAction(0, 0, new NodeMessage() { Node = node, Audio = audio });
        public void SendActionLight(ushort node, byte light) => SendAction(0, 0, new NodeMessage() { Node = node, Light = light });
        public void SendActionCharge(ushort node, byte charge) => SendAction(0, 0, new NodeMessage() { Node = node, Charge = charge });
        public void SendActionRest(ushort node, byte rest) => SendAction(0, 0, new NodeMessage() { Node = node, Rest = rest });
        public void SendActionLift(ushort node, byte lift) => SendAction(0, 0, new NodeMessage() { Node = node, Lift = lift });
        public void SendActionClamp(ushort node, byte clamp) => SendAction(0, 0, new NodeMessage() { Node = node, Clamp = clamp });
        public void SendActionTray(ushort node, byte tray) => SendAction(0, 0, new NodeMessage() { Node = node, Tray = tray });
        public void SendActionRoll(ushort node, byte roll) => SendAction(0, 0, new NodeMessage() { Node = node, Roll = roll });
        public void SendActionShutdown(ushort node, byte shutdown) => SendAction(0, 0, new NodeMessage() { Node = node, Shutdown = shutdown });

        public static SendActionMessage GetSendActionMessage(byte[] byteArray) => new SendActionMessage().GetMessage(byteArray);
        public static byte[] GetSendActionByteArray(SendActionMessage message) => message.GetByteArray();

        public void SendNodes(ushort car, ulong task, ushort count, NodeMessage[] nodeMessages)
        {
            var sendMessage = new SendNodesMessage().SetMessage(car, task, count, nodeMessages);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }
        public void SendNodes(ulong task, ushort count, NodeMessage[] nodeMessages) => SendNodes(0, task, count, nodeMessages);

        public static SendNodesMessage GetSendNodesMessage(byte[] byteArray) => new SendNodesMessage().GetMessage(byteArray);
        public static byte[] GetSendNodesByteArray(SendNodesMessage message) => message.GetByteArray();

        public void SendStateResponse(byte command, ushort car, ulong param)
        {
            var sendMessage = new SendStateResponseMessage().SetMessage(command, car, param);
            var sendByteArray = sendMessage.GetByteArray();
            Server.Send(sendByteArray);
        }

        public static SendStateResponseMessage GetStateResponseMessage(byte[] byteArray) => new SendStateResponseMessage().GetMessage(byteArray);

        public static byte[] GetStateResponseByteArray(SendStateResponseMessage message) => message.GetByteArray();

    }
}