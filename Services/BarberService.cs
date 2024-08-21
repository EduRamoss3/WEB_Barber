using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Register;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using NuGet.Common;
using System.Text;
using System.Text.Json;

namespace Barber.UI.Services
{
    public class BarberService : IBarberService
    {
        private const string apiEndPoint = "/api/v1/Barber/";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        
        public BarberService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("API_Barber");
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
        public async Task<ObjectResponse<BarberRegisterDTO>> AddAsync(BarberRegisterDTO barberDTO, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            var json = JsonSerializer.Serialize(barberDTO);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            ObjectResponse<BarberRegisterDTO> _objectResponse = new();

            using (var response = await _client.PostAsync(apiEndPoint + "add", content))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;
            }
            return _objectResponse;
        }
        public async Task<ObjectResponse<BarberDTO>> GetAllAsync(ParametersToPagination parameters, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            ObjectResponse<BarberDTO> _objectResponse = new();

            using (var response = await _client.GetAsync(apiEndPoint + $"all?PageNumber={parameters.PageNumber}&PageSize={parameters.PageSize}"))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<BarberDTO>>(apiResponse, _options);
                    _objectResponse.Objects = itemDeserialized;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<BarberDTO>> GetById(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            ObjectResponse<BarberDTO> _objectResponse = new();

            using (var response = await _client.GetAsync(apiEndPoint + id))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var json = await JsonSerializer.DeserializeAsync<BarberDTO>(apiResponse);
                    _objectResponse.OneObject = json;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<List<DateTime>>> GetIndisponibleDateAsync(int idBarber, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            ObjectResponse<List<DateTime>> _objectResponse = new();

            using (var response = await _client.GetAsync(apiEndPoint + $"{idBarber}/disponibleDates"))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var json = await JsonSerializer.DeserializeAsync<List<DateTime>>(apiResponse);
                    _objectResponse.OneObject = json;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<BarberDTO>> RemoveByIdAsync(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            ObjectResponse<BarberDTO> _objectResponse = new();

            using (var response = await _client.DeleteAsync(apiEndPoint + id))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;
            }
            return _objectResponse;
        }

        public async Task<ObjectResponse<BarberDTO>> SetDisponibilityAsync(int id, bool disponibility, string token)
        {
            PutTokenInHeadersAuthorization(token, _client);
            ObjectResponse<BarberDTO> _objectResponse = new();
            var json = JsonSerializer.Serialize(disponibility);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await _client.PatchAsync(apiEndPoint + id + $"set-disponibility/{disponibility}",content))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;
                _objectResponse.Message = response.ReasonPhrase;
            }
            return _objectResponse;
        }
    }
}
