using Barber.UI.Entities;
using Barber.UI.Models;
using System.Net;

namespace Barber.UI.Services.Interfaces
{
    public interface IScheduleServices
    {
        Task<HttpStatusCode> UpdateValueFor(int id, decimal valueForService);

        Task<List<SchedulesDTO>> GetByBarberIdAsync(int barberId);

        Task<List<SchedulesDTO>> GetByClientIdAsync(int clientId);

        Task<bool> EndOrOpenServiceByIdAsync(int id, bool endOrOpen);

        Task<List<DateTime>> GetIndisponibleDatesByBarberId(int barberId, DateTime dateSearch);

        Task<DisponibleDateViewModel> IsDisponibleInThisDate(int barberId, DateTime dateTime);

    }
}
