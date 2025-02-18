using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;

namespace ProjectTracker.Services.Authentication
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountService _account;

        public RegistrationService(IUserRepository userRepository, IAccountService account)
        {
            _userRepository = userRepository;
            _account = account;
        }

        /// <summary>
        /// The method for registrate new user.
        /// </summary>
        /// <param name="login"> New user's login. </param>
        /// <param name="password"> New user's password. </param>
        /// <param name="role"> New user's role. </param>
        /// <returns></returns>
        public async Task SingUpAsync(string login, string password, string role)
        {
            if (!await _userRepository.IsLoginExists(login))
            {
                User newUser = new User(login, _userRepository.GetPasswordHashCode(password), role);
                await _userRepository.CreateAsync(newUser);
                _account.SetCurrentUser(await _userRepository.GetAsync(login));
            }
        }
    }
}
