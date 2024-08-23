using Barber.UI.Models;
using System.Net;

namespace Barber.UI.Services.Interfaces
{
    public interface IAuthenticate
    {
        Task<TokenViewModel> Authenticates(LoginViewModel model);
        Task<HttpResponseMessage> Register(RegisterViewModel model);
        Task<HttpStatusCode> Logout(string token);
    }
}
