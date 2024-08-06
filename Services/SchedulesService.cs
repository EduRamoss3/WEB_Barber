using Barber.UI.Entities;
using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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

        public SchedulesService(IHttpClientFactory httpClientFactory)
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

        public Task<HttpStatusCode> AddAsync(SchedulesDTO scheduleDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EndOrOpenServiceByIdAsync(int id, bool endOrOpen)
        {
            var item = JsonSerializer.Serialize(endOrOpen);
            StringContent content = new StringContent(item, Encoding.UTF8, "application/json");

            using (var response = await client.PostAsync(apiEndPoint + "end-service", content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemSerialized = await JsonSerializer.DeserializeAsync<bool>(apiResponse, _options);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public Task<HttpStatusCode> EndServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<SchedulesDTO>> GetAllAsync(ParametersToPagination parameters)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SchedulesDTO>> GetByBarberIdAsync(int barberId)
        {
            var parameterId = JsonSerializer.Serialize(barberId);
            using (var response = await client.GetAsync(apiEndPoint + "barber/" + parameterId))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemSerialized = await JsonSerializer.DeserializeAsync<List<SchedulesDTO>>(apiResponse, _options);
                    return itemSerialized;
                }
                else
                {
                    return new List<SchedulesDTO>();
                }
            }

        }

        public Task<List<SchedulesDTO>> GetByBarberIdAsync(int? barberId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SchedulesDTO>> GetByClientIdAsync(int clientId)
        {
            using (var response = await client.GetAsync(apiEndPoint + $"client/{clientId}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var responseObject = await JsonSerializer.DeserializeAsync<SchedulesResponse>(apiResponse, _options);

                    return responseObject?.Values ?? new List<SchedulesDTO>();
                }
                else
                {
                    return new List<SchedulesDTO>();
                }
            }
        }

        public Task<List<SchedulesDTO>> GetByClientIdAsync(int? clientId)
        {
            throw new NotImplementedException();
        }

        public Task<SchedulesDTO> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DateTime>> GetByDateDisponible(int barberId, DateTime dateSearch)
        {
            var parameterId = JsonSerializer.Serialize(barberId);
            var parameterDate = JsonSerializer.Serialize(dateSearch);
            using (var response = await client.GetAsync(apiEndPoint + $"barbers/{parameterId}/availability/{parameterDate}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();
                    var itemSerialized = await JsonSerializer.DeserializeAsync<List<DateTime>>(apiResponse, _options);
                    return itemSerialized;
                }
                else
                {
                    return new List<DateTime>();
                }

            }
        }

       

        public Task<HttpStatusCode> OpenServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> RemoveAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public Task<HttpStatusCode> UpdateAsync(SchedulesDTO scheduleDTO, int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<HttpStatusCode> UpdateValueForAsync(int id, decimal valueForService)
        {
            var client = _httpClientFactory.CreateClient("API_Barber");

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
