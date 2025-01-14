using System.Security;

namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IAutorization
    {
        Task<bool> LogIn(string login, string password);
    }
}
