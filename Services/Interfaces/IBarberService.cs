using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;

namespace Barber.UI.Services.Interfaces
{
    public interface IBarberService
    {
        Task<ObjectResponse<BarberRegisterDTO>> AddAsync(BarberRegisterDTO barberDTO, string token);

        Task<ObjectResponse<BarberDTO>> RemoveByIdAsync(int id, string token);

        Task<ObjectResponse<BarberDTO>> GetAllAsync(ParametersToPagination parameters, string token);

        Task<ObjectResponse<BarberDTO>> SetDisponibilityAsync(int id, bool disponibility, string token);

        Task<ObjectResponse<BarberDTO>> GetById(int id, string token);

        Task<ObjectResponse<List<DateTime>>> GetIndisponibleDateAsync(int idBarber, string token);
    }
}
