using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.Interfaces;

namespace ProjectTracker.Services.Authentication
{
    public class Registration : IRegistration
    {
        private readonly IUserRepository _userRepository;

        public Registration(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task SingUp(string login, string password, string role)
        {
            User newUser = new User(login, password, role);
            await _userRepository.CreateAsync(newUser);
        }
    }
}
