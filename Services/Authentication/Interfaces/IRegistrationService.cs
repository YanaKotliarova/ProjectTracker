namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IRegistrationService
    {
        Task SingUpAsync(string login, string password, string role);
    }
}