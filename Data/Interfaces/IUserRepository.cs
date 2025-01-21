using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task CreateAsync(User newUser);
        Task DeleteAsync(int id);
        Task<User> GetAsync(string login);
        Task<bool> IsLoginExists(string login);
        Task UpdateAsync(User user);
    }
}