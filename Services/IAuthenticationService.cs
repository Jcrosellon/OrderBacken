// Para encapsular la lógica de autenticación.

using OrderBackend.Models;

namespace OrderBackend.Services
{
    public interface IAuthenticationService
    {
        string? Authenticate(UserLogin loginRequest);
    }
}
