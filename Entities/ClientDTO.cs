using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities
{
    public sealed record ClientDTO
    {
        [Key]
        public int Id { get; init; }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(200, ErrorMessage = "Max 200 characters")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Scheduled  is required!")]
        public bool Scheduled { get; init; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime LastTimeHere { get; init; }
    }
}
