using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Barber.UI.Entities.Register
{
    public sealed record BarberRegisterDTO
    {
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(200, ErrorMessage = "Max of characters: 200")]
        [DisplayName("Nome")]
        public string Name { get; init; }
    }
}
