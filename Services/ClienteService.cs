using Barber.UI.Entities;
using Barber.UI.Entities.Responses;
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

        public async Task<ObjectResponse<ClientDTO>> GetAllAsync(ParametersToPagination parameters)
        {
            using var response = await client.GetAsync(apiEndPoint + "all" + $"{parameters.PageNumber}/ + {parameters.PageSize}");
            {
                ObjectResponse<ClientDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var DeserializerItem = JsonSerializer.Deserialize<List<ClientDTO>>(content);

                    _objectResponse.Objects = DeserializerItem;
                    _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                }
                return _objectResponse;
                
            }
        }
        public async Task<ObjectResponse<ClientDTO>> GetByIdAsync(int id)
        {
            using var response = await client.GetAsync(apiEndPoint + id);
            {
                ObjectResponse<ClientDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var DeserializerItem = JsonSerializer.Deserialize<ClientDTO>(content);

                    _objectResponse.OneObject = DeserializerItem;
                    _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                }
                return _objectResponse;
            }
        }

        public async Task<bool> RemoveAsync(int? id)
        {
            if (id.HasValue)
            {
                using (var response = await client.DeleteAsync(apiEndPoint + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> UpdateAsync(ClientDTO clientDTO, int? id)
        {
            var json = JsonSerializer.Serialize(clientDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            using(var response = await client.PutAsync(apiEndPoint + id, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> UpdatePointsAsync(int id)
        {
            var json = JsonSerializer.Serialize(id);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await client.PatchAsync(apiEndPoint + id,content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
