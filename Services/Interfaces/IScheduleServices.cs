using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using System.Net;

namespace Barber.UI.Services.Interfaces
{
    public interface IScheduleServices
    {
        Task<HttpStatusCode> AddAsync(SchedulesDTO scheduleDTO, string token);

        Task<HttpStatusCode> RemoveAsync(int? id, string token);

        Task<ObjectResponse<SchedulesDTO>> GetAllAsync(ParametersToPagination parameters, string token);

        Task<HttpStatusCode> UpdateAsync(SchedulesDTO scheduleDTO, int? id, string token);

        Task<HttpStatusCode> UpdateValueForAsync(int id, decimal amount, string token);

        Task<ObjectResponse<SchedulesDTO>> GetByIdAsync(int? id, string token);

        Task<ObjectResponse<SchedulesDTO>> GetByBarberIdAsync(int? barberId, string token);

        Task<ObjectResponse<SchedulesDTO>> GetByClientIdAsync(int clientId,string token);

        Task<bool> EndServiceAsync(int id,string token);

        Task<bool> OpenServiceAsync(int id, string token);

        Task<bool> GetByDateDisponible(int idBarber, DateTime dateTimeSearch, string token);

        Task<ObjectResponse<SchedulesDTO>> GetWithDataAsync(string token);
    }
}
