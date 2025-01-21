namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IRegistration
    {
        Task SingUpAsync(string login, string password, string role);
    }
}