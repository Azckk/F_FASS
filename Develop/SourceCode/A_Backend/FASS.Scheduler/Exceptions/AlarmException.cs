namespace FASS.Scheduler.Exceptions
{
    public class AlarmException : Exception
    {
        public string car { get; set; }
        public AlarmException(string message, string car) : base(message)
        {
            this.car = car;
        }
    }
}
