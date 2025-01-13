using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task CreateAsync(User newUser);
        Task DeleteAsync(string login);
        Task<User> GetAsync(string login);
        Task UpdateAsync(User user);
    }
}