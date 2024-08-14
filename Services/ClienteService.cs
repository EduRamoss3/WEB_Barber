using Barber.UI.Entities;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Barber.UI.Services
{
    public class ClienteService : IClienteService
    {
        private const string apiEndPoint = "/api/client/";
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;

        public async Task<bool> AddAsync(ClientRegisterDTO clientDTO)
        {
            var itemSerialized = JsonSerializer.Serialize(clientDTO);
            StringContent content = new StringContent(itemSerialized, Encoding.UTF8, "application/json");

            using(var response = await client.PostAsync(apiEndPoint + "add", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public Task<IEnumerable<ClientDTO>> GetAllAsync(ParametersToPagination parameters)
        {
            throw new NotImplementedException();
        }

        public Task<ClientDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ClientDTO clientDTO, int? id)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePointsAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
