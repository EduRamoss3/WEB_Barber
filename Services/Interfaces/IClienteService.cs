using Barber.UI.Entities;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;

namespace Barber.UI.Services.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AddAsync(ClientRegisterDTO clientDTO);
        Task<bool> RemoveAsync(int? id);
        Task<ObjectResponse<ClientDTO>> GetByIdAsync(int id);
        Task<ObjectResponse<ClientDTO>> GetAllAsync(ParametersToPagination parameters);
        Task<bool> UpdateAsync(ClientDTO clientDTO, int? id);
        Task<bool> UpdatePointsAsync(int id);
    }
}
