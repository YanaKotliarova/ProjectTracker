namespace ProjectTracker.Services.Authentication.Interfaces
{
    public interface IAutorization
    {
        Task<bool> LogInAsync(string login, string password);
    }
}
