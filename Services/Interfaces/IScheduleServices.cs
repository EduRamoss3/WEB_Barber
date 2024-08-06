using Barber.UI.Entities;
using Barber.UI.Models;
using System.Net;

namespace Barber.UI.Services.Interfaces
{
    public interface IScheduleServices
    {
        Task<HttpStatusCode> AddAsync(SchedulesDTO scheduleDTO);
        Task<HttpStatusCode> RemoveAsync(int? id);
        Task<List<SchedulesDTO>> GetAllAsync(ParametersToPagination parameters);
        Task<HttpStatusCode> UpdateAsync(SchedulesDTO scheduleDTO, int? id);
        Task<HttpStatusCode> UpdateValueForAsync(int id, decimal amount);
        Task<SchedulesDTO> GetByIdAsync(int? id);
        Task<List<SchedulesDTO>> GetByBarberIdAsync(int? barberId);
        Task<List<SchedulesDTO>> GetByClientIdAsync(int? clientId);
        Task<HttpStatusCode> EndServiceAsync(int id);
        Task<HttpStatusCode> OpenServiceAsync(int id);
        Task<List<DateTime>> GetByDateDisponible(int idBarber, DateTime dateTimeSearch);

    }
}
