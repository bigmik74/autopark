using System.ComponentModel;

namespace autopark
{
    public class Autobus
    {
        [DisplayName("№ автобуса")]           
        public int Number { get; set; }
        [DisplayName("ФИО водителя")]
        public string DriverFIO { get; set; }
        [DisplayName("№ маршрута")]
        public int RouteNumber { get; set; }
    }
}
