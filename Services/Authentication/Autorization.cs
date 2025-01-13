using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;
using System.Security;

namespace ProjectTracker.Services.Authentication
{
    public class Autorization : IAutorization
    {
        private readonly IUserRepository _userRepository;

        public Autorization(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> LogIn(string login, SecureString password)
        {
            bool isUserRegistered = false;
            User existingUser = await _userRepository.GetAsync(login);

            if (existingUser != null)
            {
                User currentUser = new User(login, password, existingUser.Role);
                if (existingUser.Equals(currentUser)) isUserRegistered = true;
                else isUserRegistered = false;
            }         

            return isUserRegistered;
        }
    }
}
