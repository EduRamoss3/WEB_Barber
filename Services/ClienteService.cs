using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using System.Text;
using System.Text.Json;

namespace Barber.UI.Services
{
    public class ClienteService : IClienteService
    {
        private const string apiEndPoint = "/api/Client/";
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;
        
        public ClienteService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient("API_Barber");
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new DateTimeConverter()
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        private static void PutTokenInHeadersAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
        public async Task<bool> AddAsync(ClientRegisterDTO clientDTO, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

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

        public async Task<ObjectResponse<ClientDTO>> GetAllAsync(ParametersToPagination parameters, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            using var response = await client.GetAsync(apiEndPoint + $"all?PageNumber={parameters.PageNumber}&PageSize={parameters.PageSize}");
            {
                ObjectResponse<ClientDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<ClientDTO>>(apiResponse, _options);

                    _objectResponse.Objects = itemDeserialized;
                    _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                }
                return _objectResponse;
                
            }
        }
        public async Task<ObjectResponse<ClientDTO>> GetByIdAsync(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            using var response = await client.GetAsync(apiEndPoint + id);
            {
                ObjectResponse<ClientDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var DeserializerItem = JsonSerializer.Deserialize<ClientDTO>(content, _options);

                    _objectResponse.OneObject = DeserializerItem;
                    _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                }
                return _objectResponse;
            }
        }

        public async Task<int> GetIdByEmail(string email, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            using (var response = await client.GetAsync(apiEndPoint + $"Email/{email}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<int>(apiResponse, _options);
                    return itemDeserialized;
                }
                return 0;
            }
        }

        public async Task<bool> RemoveAsync(int? id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

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

        public async Task<bool> UpdateAsync(ClientDTO clientDTO, int? id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            var json = JsonSerializer.Serialize(clientDTO,_options);
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

        public async Task<bool> UpdatePointsAsync(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

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
