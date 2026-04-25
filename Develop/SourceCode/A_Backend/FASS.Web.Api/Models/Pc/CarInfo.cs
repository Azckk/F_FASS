namespace FASS.Web.Api.Models.Pc
{
    public class CarInfo
    {
        public required string Code { get; set; }
        public string? Name { get; set; }
        public string? CarType { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public required string CurrNodeCode { get; set; }
        public string? CurrState { get; set; }
        public double Battery { get; set; }
        public required string Address { get; set; }
        public double Speed { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Theta { get; set; }
        public double Voltage { get; set; }
        public double ElectricCurrent { get; set; }
        public bool IsOnline { get; set; } = false;
    }
}
