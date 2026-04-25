using Common.Device.Clients.PLC;
using Common.Device.Clients.PLC.Enums;

namespace FASS.Extend.Conveyor
{
    public class Command
    {
        public SiemensClient Client { get; private set; }

        public Command(string ip, string port)
        {
            Client = new SiemensClient(SiemensVersion.S7_1200, ip, int.Parse(port), timeout: 500);
        }

        public bool LeftInRequest()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.3", false);
                Client.Write("DB6.1.0", true);
                if (Client.ReadBoolean("DB6.3.0").Value)
                {
                    Client.Write("DB6.1.0", false);
                    Client.Write("DB6.1.1", true);
                    result = true;
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

        public bool LeftOutRequest()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.1", false);
                Client.Write("DB6.1.2", true);
                if (Client.ReadBoolean("DB6.3.2").Value)
                {
                    Client.Write("DB6.1.2", false);
                    result = true;
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

        public bool LeftOutReponse()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.3", true);
                result = true;
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

        public bool RightInRequest()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.7", false);
                Client.Write("DB6.1.4", true);
                if (Client.ReadBoolean("DB6.3.1").Value)
                {
                    Client.Write("DB6.1.4", false);
                    Client.Write("DB6.1.5", true);
                    result = true;
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

        public bool RightOutRequest()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.5", false);
                Client.Write("DB6.1.6", true);
                if (Client.ReadBoolean("DB6.3.3").Value)
                {
                    Client.Write("DB6.1.6", false);
                    result = true;
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

        public bool RightOutReponse()
        {
            var result = false;
            try
            {
                Client.Open();
                Client.Write("DB6.1.7", true);
                result = true;
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

        //public bool LeftInRequest()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.0", true);
        //        if (Client.ReadBoolean("DB6.2.0").Value)
        //        {
        //            Client.Write("DB6.0.0", false);
        //            Client.Write("DB6.0.1", true);
        //            result = true;
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}

        //public bool LeftOutRequest()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.1", false);
        //        Client.Write("DB6.0.2", true);
        //        if (Client.ReadBoolean("DB6.2.2").Value)
        //        {
        //            Client.Write("DB6.0.2", false);
        //            result = true;
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}

        //public bool LeftOutReponse()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.3", true);
        //        Thread.Sleep(1000);
        //        Client.Write("DB6.0.3", false);
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}

        //public bool RightInRequest()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.4", true);
        //        if (Client.ReadBoolean("DB6.2.1").Value)
        //        {
        //            Client.Write("DB6.0.4", false);
        //            Client.Write("DB6.0.5", true);
        //            result = true;
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}

        //public bool RightOutRequest()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.5", false);
        //        Client.Write("DB6.0.6", true);
        //        if (Client.ReadBoolean("DB6.2.3").Value)
        //        {
        //            Client.Write("DB6.0.6", false);
        //            result = true;
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}

        //public bool RightOutReponse()
        //{
        //    var result = false;
        //    try
        //    {
        //        Client.Open();
        //        Client.Write("DB6.0.7", true);
        //        result = true;
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        Client.Close();
        //    }
        //    return result;
        //}
    }
}