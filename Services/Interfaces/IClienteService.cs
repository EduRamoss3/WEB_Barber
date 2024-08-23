using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;

namespace Barber.UI.Services.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AddAsync(ClientRegisterDTO clientDTO, string token);

        Task<bool> RemoveAsync(int? id, string token);

        Task<ObjectResponse<ClientDTO>> GetByIdAsync(int id, string token);

        Task<ObjectResponse<ClientDTO>> GetAllAsync(ParametersToPagination parameters, string token);

        Task<bool> UpdateAsync(ClientDTO clientDTO, int? id, string token);

        Task<bool> UpdatePointsAsync(int id, string token);

        Task<int> GetIdByEmail(string email, string token);
    }
}
