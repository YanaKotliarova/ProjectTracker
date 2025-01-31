using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;

namespace ProjectTracker.Services.Authentication
{
    public class Registration : IRegistration
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccount _account;

        public Registration(IUserRepository userRepository, IAccount account)
        {
            _userRepository = userRepository;
            _account = account;
        }

        public async Task SingUpAsync(string login, string password, string role)
        {
            if (!await _userRepository.IsLoginExists(login))
            {
                User newUser = new User(login, _userRepository.GetPasswordHashCode(password), role);
                await _userRepository.CreateAsync(newUser);
                _account.CurrentUser = await _userRepository.GetAsync(login);
            }
        }
    }
}
