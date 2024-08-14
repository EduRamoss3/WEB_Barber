using Barber.UI.Entities;
using Barber.UI.Models;

namespace Barber.UI.Services.Interfaces
{
    public interface IClienteService
    {
        Task<bool> AddAsync(ClientRegisterDTO clientDTO);
        Task<bool> RemoveAsync(int? id);
        Task<ClientDTO> GetByIdAsync(int id);
        Task<IEnumerable<ClientDTO>> GetAllAsync(ParametersToPagination parameters);
        Task<bool> UpdateAsync(ClientDTO clientDTO, int? id);
        Task UpdatePointsAsync(int id);
    }
}
