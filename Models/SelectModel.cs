using Barber.UI.Entities.Enums;

namespace Barber.UI.Models
{
    public class SelectModel
    {
        public int IdBarber { get; set; }
        public DateTime Date { get; set; }
        public TypeOfService TypeOfService { get; set; }
        public string InitialDate { get; set; }
    }
}
