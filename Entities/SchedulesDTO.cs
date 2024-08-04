using Barber.UI.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities
{
    public class SchedulesDTO
    {
        [Key]
        public int Id { get; init; }

        [Required(ErrorMessage = "ID Barber is required!")]
        public int IdBarber { get; init; }

        [Required(ErrorMessage = "ID Client is required!")]
        public int IdClient { get; init; }

        [Required(ErrorMessage = "Type of service is required!")]
        public TypeOfService TypeOfService { get; init; }

        [Required(ErrorMessage = "Date to schedule is required!")]
        public DateTime DateSchedule { get; init; }

        [Required(ErrorMessage = "The value for service is required!")]
        public decimal ValueForService { get; init; }
        [Required(ErrorMessage = "Finalized is required")]
        public bool IsFinalized { get; init; }
    }
}
