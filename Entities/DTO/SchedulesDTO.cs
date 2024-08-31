using Barber.UI.Entities.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities.DTO
{
    public class SchedulesDTO
    {
        [Key]
        public int Id { get; init; }

        [Required(ErrorMessage = "ID Barber é obrigatório!")]
        [DisplayName("Barbeiro")]
        public int IdBarber { get; init; }

        [Required(ErrorMessage = "ID Client é obrigatório!")]
        [DisplayName("Cliente")]
        public int IdClient { get; set; }

        [DisplayName("Serviço")]
        [Required(ErrorMessage = "O tipo de serviço é obrigatório!")]
        public TypeOfService TypeOfService { get; init; }

        [Required(ErrorMessage = "A data é obrigatória!")]
        [DisplayName("Data")]
        public DateTime DateSchedule { get; init; }

        [Required(ErrorMessage = "O valor de serviço é obrigatório!")]
        [DisplayName("Valor")]
        public decimal ValueForService { get; init; }

        [Required(ErrorMessage = "Se foi finalizado ou não é obrigatório!")]
        [DisplayName("Finalizado?")]
        public bool IsFinalized { get; init; }

        [DisplayName("Cliente")]
        public string ClientName { get; set; }

        [DisplayName("Barbeiro")]
        public string BarberName { get; set; }


        public SchedulesDTO()
        {

        }
        public SchedulesDTO(int idBarber, int idClient, TypeOfService type, DateTime date, bool isFinalized)
        {
            IdBarber = idBarber;
            IdClient = idClient;
            TypeOfService = type;
            DateSchedule = date;
            ValueForService = CalcValueForService(type);
            IsFinalized = isFinalized;
        }
        private decimal CalcValueForService(TypeOfService type)
        {
            if(type.ToString() == "Gradient")
            {
                return 45;
            }
            return 40;
        }
    }
}
