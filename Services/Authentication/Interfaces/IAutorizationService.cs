namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IAutorizationService
    {
        Task<bool> LogInAsync(string login, string password);
    }
}
