using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;

namespace ProjectTracker.Services.Authentication
{
    public class AutorizationService : IAutorizationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _account;

        public AutorizationService(IUserRepository userRepository, IAccountService account)
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
                if (existingUser.Login.Equals(login) && existingUser.Password.Equals(_userRepository.GetPasswordHashCode(password)))
                {
                    isUserRegistered = true;
                    _account.SetCurrentUser(existingUser);
                }                    
                else isUserRegistered = false;
            }       

            return isUserRegistered;
        }
    }
}
