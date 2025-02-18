using Microsoft.EntityFrameworkCore;
using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using System.Security.Cryptography;
using System.Text;

namespace ProjectTracker.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _db;

        public UserRepository(ApplicationContext applicationContext)
        {
            _db = applicationContext;
        }

        /// <summary>
        /// The method for adding new user to database.
        /// </summary>
        /// <param name="newUser"> User object for creating. </param>
        /// <returns></returns>
        public async Task CreateAsync(User newUser)
        {
            await _db.Users.AddAsync(newUser);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for getting user from database by login.
        /// </summary>
        /// <param name="login"> Entered login for search. </param>
        /// <returns> Required user or null. </returns>
        public async Task<User> GetAsync(string login)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Login == login);
        }

        /// <summary>
        /// The method for checking if entered login is already exists in database.
        /// </summary>
        /// <param name="login"> Entered login for checking. </param>
        /// <returns></returns>
        public async Task<bool> IsLoginExists(string login)
        {
            if (await GetAsync(login) != null)
                return true;
            else return false;
        }

        /// <summary>
        /// The method for update user information in database.
        /// </summary>
        /// <param name="user"> User object with updated information. </param>
        /// <returns></returns>
        public async Task UpdateAsync(User user)
        {
            _db.Entry(user).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for deleting user from database by id.
        /// </summary>
        /// <param name="id"> Id of user for deleting. </param>
        /// <returns></returns>
        public async Task DeleteAsync(int id)
        {
            User user = await _db.Users.FindAsync(id);
            if (user != null) _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// The method for hashing user password for storage in the database.
        /// </summary>
        /// <param name="password"> Crear string of user password. </param>
        /// <returns></returns>
        public string GetPasswordHashCode(string password)
        {
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] passwordByteArray = Encoding.ASCII.GetBytes(password);
                return BitConverter.ToString(mySHA256.ComputeHash(passwordByteArray));
            }
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
