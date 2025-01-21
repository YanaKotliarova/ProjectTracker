using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;

namespace ProjectTracker.Services.Authentication
{
    public class Autorization : IAutorization
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccount _account;

        public Autorization(IUserRepository userRepository, IAccount account)
        {
            _userRepository = userRepository;
            _account = account;
        }

        public async Task<bool> LogInAsync(string login, string password)
        {
            bool isUserRegistered = false;
            User existingUser = await _userRepository.GetAsync(login);

            if (existingUser != null)
            {
                User currentUser = new User(login, password);
                if (existingUser.Login.Equals(currentUser.Login) && existingUser.Password.Equals(currentUser.Password))
                {
                    isUserRegistered = true;
                    _account.CurrentUser = await _userRepository.GetAsync(login);
                }                    
                else isUserRegistered = false;
            }         

            return isUserRegistered;
        }
    }
}
