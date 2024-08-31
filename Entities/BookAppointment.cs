using Barber.UI.Entities.Enums;

namespace Barber.UI.Entities
{
    public class BookAppointment
    {
        public Horarios Horarios { get; set; }
        public DaysOfMonth DaysOfMonth { get; set; }    
        public Month Month { get; set; }
        public Year Year { get; set; }
        public int IdBarber { get; set; }
    }
}
