using Barber.UI.Models;

namespace Barber.UI.Services.Interfaces
{
    public interface IAuthenticate
    {
        Task<TokenViewModel> Authenticates(LoginViewModel model);
        Task<HttpResponseMessage> Register(LoginViewModel model);
    }
}
