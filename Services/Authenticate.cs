using Barber.UI.Models;
using Barber.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Text;
using System.Text.Json;

namespace Barber.UI.Services
{
    public class Authenticate : IAuthenticate
    {
        private const string apiEndPoint = "/api/Token/";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _options;
        private readonly HttpClient _client;
        private TokenViewModel TokenViewModel;
        public Authenticate(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions{
                PropertyNameCaseInsensitive = false,
                Converters = { new DateTimeConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _client = _httpClientFactory.CreateClient("API_Barber");
        }

        public async Task<HttpResponseMessage> Register(LoginViewModel model)
        {
            
            var json = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using(var response = await _client.PostAsync(apiEndPoint + "register", content))
            {
                return response;
            }
        }

        public async Task<TokenViewModel> Authenticates(LoginViewModel model)
        {
            var json = JsonSerializer.Serialize(model);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            using (var response = await _client.PostAsync(apiEndPoint + "login", content))
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    
                    try
                    {
                        var tokenViewModel = JsonSerializer.Deserialize<TokenViewModel>(responseContent, _options);
                        return tokenViewModel;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
