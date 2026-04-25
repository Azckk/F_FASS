using Common.Device.Clients.Modbus;
using System.IO.Ports;

namespace FASS.Extend.Light
{
    public class Command
    {
        public ModbusRtuClient Client { get; private set; }

        public byte StationNumber { get; private set; }

        public Command(string portName, string baudRate, string stationNumber)
        {
            Client = new ModbusRtuClient(portName, int.Parse(baudRate), 8, StopBits.One, Parity.None, 500);
            StationNumber = byte.Parse(stationNumber);
        }

        public bool Switch(string close, string open)
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write(close, false, StationNumber);
                Client.Write(open, true, StationNumber);
                var closeResult = Client.ReadCoil(close, StationNumber);
                var openResult = Client.ReadCoil(open, StationNumber);
                if (closeResult.IsSucceed && openResult.IsSucceed)
                {
                    if (closeResult.Value == false && openResult.Value == true)
                    {
                        result = true;
                    }
                }
            }
            catch
            {
                result = false;
            }
            finally
            {
                Client.Close();
            }
            return result;
        }
    }
}