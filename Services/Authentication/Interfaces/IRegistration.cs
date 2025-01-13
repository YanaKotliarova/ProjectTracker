using System.Security;

namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IRegistration
    {
        Task SingUp(string login, SecureString password, string role);
    }
}