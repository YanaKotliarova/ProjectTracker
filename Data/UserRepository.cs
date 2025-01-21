using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _db;
        private readonly IRepository _repository;

        public UserRepository(IRepository repository)
        {
            _repository = repository;
            _db = _repository.GetDB();
        }

        public async Task CreateAsync(User newUser)
        {
            await _db.Users.AddAsync(newUser);
            await _repository.SaveChangesAsync();
        }

        public async Task<User> GetAsync(string login)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        public async Task<bool> IsLoginExists(string login)
        {
            if (await GetAsync(login) != null)
                return true;
            else return false;
        }

        public async Task UpdateAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            User user = await _db.Users.FindAsync(id);
            if (user != null) _db.Users.Remove(user);
            await _repository.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
