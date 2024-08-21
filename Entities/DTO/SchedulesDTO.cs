using Barber.UI.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities.DTO
{
    public class SchedulesDTO
    {
        [Key]
        public int Id { get; init; }

        [Required(ErrorMessage = "ID Barber is required!")]
        [DisplayName("Barbeiro")]
        public int IdBarber { get; init; }

        [Required(ErrorMessage = "ID Client is required!")]
        [DisplayName("Cliente")]
        public int IdClient { get; init; }

        [DisplayName("Serviço")]
        [Required(ErrorMessage = "Type of service is required!")]
        public TypeOfService TypeOfService { get; init; }

        [Required(ErrorMessage = "Date to schedule is required!")]
        [DisplayName("Data")]
        public DateTime DateSchedule { get; init; }

        [Required(ErrorMessage = "The value for service is required!")]
        [DisplayName("Valor")]
        public decimal ValueForService { get; init; }

        [Required(ErrorMessage = "Finalized is required")]
        [DisplayName("Finalizado?")]
        public bool IsFinalized { get; init; }

        [DisplayName("Nome")]
        public string ClientName { get; set; }

        [DisplayName("Barbeiro")]
        public string BarberName { get; set; }
    }
}
