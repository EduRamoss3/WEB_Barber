using Barber.UI.Entities.DTO;
using Barber.UI.Entities.Responses;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace Barber.UI.Services
{
    public class SchedulesService : IScheduleServices
    {
        private const string apiEndPoint = "/api/Schedules/";
        private readonly JsonSerializerOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient client;

        public SchedulesService(IHttpClientFactory httpClientFactory, ObjectResponse<SchedulesDTO> objectResponse)
        {
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new DateTimeConverter()
                },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _httpClientFactory = httpClientFactory;
            client = _httpClientFactory.CreateClient("API_Barber");
        }
        private static void PutTokenInHeadersAuthorization(string token, HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpStatusCode> AddAsync(SchedulesDTO scheduleDTO, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var serializeItem = JsonSerializer.Serialize(scheduleDTO,_options);
            StringContent content = new(serializeItem, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndPoint + "add", content))
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
                return response.StatusCode;
            }

        }
        public async Task<bool> OpenServiceAsync(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var serializeItem = JsonSerializer.Serialize(id);
            StringContent content = new(serializeItem, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndPoint + "end-service", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<bool>(apiResponse);
                    return itemDeserialized;
                }
                return false;
            }
        }
        public async Task<bool> EndServiceAsync(int id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var item = JsonSerializer.Serialize(id);
            StringContent content = new StringContent(item, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndPoint + "end-service", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemSerialized = await JsonSerializer.DeserializeAsync<bool>(apiResponse, _options);
                }
            }
            return true;
        }

        public async Task<ObjectResponse<SchedulesDTO>> GetAllAsync(ParametersToPagination parameters, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            using (var response = await client.GetAsync(apiEndPoint + $"all?PageNumber={parameters.PageNumber}&PageSize={parameters.PageSize}" ))
            {
                ObjectResponse<SchedulesDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    _objectResponse.Objects = itemDeserialized;
                }
                return _objectResponse;
            }
        }
        public async Task<ObjectResponse<SchedulesDTO>> GetWithDataAsync(string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            using (var response = await client.GetAsync(apiEndPoint + "data"))
            {
                ObjectResponse<SchedulesDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;
                _objectResponse.RequestUri = response.RequestMessage.RequestUri;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    _objectResponse.Objects = itemDeserialized;
                }
                return _objectResponse;
            }
        }
        public async Task<ObjectResponse<SchedulesDTO>> GetByBarberIdAsync(int barberId, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            ObjectResponse<SchedulesDTO> _objectResponse = new();
            var parameterId = JsonSerializer.Serialize(barberId);
            using (var response = await client.GetAsync(apiEndPoint + "barber/" + parameterId))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemSerialized = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    _objectResponse.Objects = itemSerialized;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<SchedulesDTO>> GetByBarberIdAsync(int? barberId, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            using (var response = await client.GetAsync(apiEndPoint + "barber/" + barberId.Value))
            {
                ObjectResponse<SchedulesDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    _objectResponse.Objects = itemDeserialized;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<SchedulesDTO>> GetByClientIdAsync(int clientId, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            ObjectResponse<SchedulesDTO> _objectResponse = new();

            using (var response = await client.GetAsync(apiEndPoint + $"client/{clientId}"))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var responseObject = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    _objectResponse.Objects = responseObject;
                }
                return _objectResponse;
            }
        }
        public async Task<ObjectResponse<SchedulesDTO>> GetByIdAsync(int? id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);

            using (var response = await client.GetAsync(apiEndPoint + $"id/{id.Value}"))
            {
                ObjectResponse<SchedulesDTO> _objectResponse = new();
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var responseObject = await JsonSerializer.DeserializeAsync<SchedulesDTO>(apiResponse, _options);
                    _objectResponse.OneObject = responseObject;
                }
                return _objectResponse;
            }
        }

        public async Task<ObjectResponse<List<DateTime>>> GetByDateDisponible(int barberId, DateTime dateSearch, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var parameterId = JsonSerializer.Serialize(barberId);
            var parameterDate = JsonSerializer.Serialize(dateSearch);

            ObjectResponse<List<DateTime>> _objectResponse = new();
            using (var response = await client.GetAsync(apiEndPoint + $"barbers/{parameterId}/availability/{parameterDate}"))
            {
                _objectResponse.StatusCode = response.StatusCode;
                _objectResponse.Message = response.ReasonPhrase;
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemDeserialized = await JsonSerializer.DeserializeAsync<List<DateTime>>(apiResponse, _options);
                    _objectResponse.OneObject = itemDeserialized;
                }
                return _objectResponse;

            }
        }

        public async Task<HttpStatusCode> RemoveAsync(int? id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var itemSerialized = JsonSerializer.Serialize(id.Value);

            using (var response = await client.DeleteAsync(apiEndPoint + $"{itemSerialized}"))
            {
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> UpdateAsync(SchedulesDTO scheduleDTO, int? id, string token)
        {
            PutTokenInHeadersAuthorization(token, client);
            var itemSerialized = JsonSerializer.Serialize(scheduleDTO);
            StringContent content = new(itemSerialized, Encoding.UTF8, "application/json");

            using (var response = await client.PutAsync(apiEndPoint + $"{id.Value}", content))
            {
                return response.StatusCode;
            }
        }

        public async Task<HttpStatusCode> UpdateValueForAsync(int id, decimal valueForService, string token)
        {
            var client = _httpClientFactory.CreateClient("API_Barber");
            PutTokenInHeadersAuthorization(token, client);
            var parameters = new
            {
                Id = id,
                ValueForService = valueForService
            };

            var json = JsonSerializer.Serialize(parameters);

            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var response = await client.PatchAsync(apiEndPoint + $"{parameters.Id}/value-service/{parameters.ValueForService}", content))
            {
                return response.StatusCode;
            }
        }
    }
}
