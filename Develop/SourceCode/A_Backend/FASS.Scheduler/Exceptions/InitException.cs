namespace FASS.Scheduler.Exceptions
{
    public class InitException : Exception
    {
        public string car { get; set; }
        public InitException(string message, string car) : base(message)
        {
            this.car = car;
        }
    }
}
