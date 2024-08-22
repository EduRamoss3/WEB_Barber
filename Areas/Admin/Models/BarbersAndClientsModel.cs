using Barber.UI.Entities.DTO;

namespace Barber.UI.Areas.Admin.Models
{
    public class BarbersAndClientsModel
    {
        public List<BarberDTO> Barbers { get; set; } = new List<BarberDTO>();
        public List<ClientDTO> Clients { get; set; } = new List<ClientDTO>();

        public BarbersAndClientsModel(List<BarberDTO> barbers,  List<ClientDTO> clients)
        {
            Barbers = barbers;
            Clients = clients;
        }
    }
}
