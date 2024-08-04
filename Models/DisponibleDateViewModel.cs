namespace Barber.UI.Models
{
    public class DisponibleDateViewModel
    {
        public bool IsDisponible { get; set; }  
        public int BarberId { get; set; }
        public DisponibleDateViewModel(bool isDisponible, int barberId)
        {
            IsDisponible = isDisponible;
            BarberId = barberId;
        }
    }
}
