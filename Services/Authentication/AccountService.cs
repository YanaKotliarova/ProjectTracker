using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.CurrentUser;

namespace ProjectTracker.Services.Authentication
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void SetCurrentUser(User currentUser)
        {
            CustomPrincipal.Identity = new CustomIdentity(currentUser.Id, currentUser.Login, currentUser.Role);
        }

        public void ResetCurrentUser()
        {
            CustomPrincipal.Identity = new AnonymousIdentity();
        }

        public CustomPrincipal CustomPrincipal => Thread.CurrentPrincipal as CustomPrincipal;

        public async Task UpdateUserPersonalInfoAsync(string newLogin, string newRole)
        {
            User currentUser = await _userRepository.GetAsync(CustomPrincipal.Identity.Login);

            currentUser.Login = newLogin;
            currentUser.Role = newRole;

            await _userRepository.UpdateAsync(currentUser);

            SetCurrentUser(await _userRepository.GetAsync(newLogin));
        }

        public async Task<bool> CheckIfLoginExistsAsync(string login)
        {
            if (CustomPrincipal != null && login.Equals(CustomPrincipal.Identity.Login))
                return false;
            else return await _userRepository.IsLoginExists(login);
        }

        public async Task UpdateUserPasswordAsync(string newPassword)
        {
            User currentUser = await _userRepository.GetAsync(CustomPrincipal.Identity.Login);
            currentUser.Password = _userRepository.GetPasswordHashCode(newPassword);

            await _userRepository.UpdateAsync(currentUser);

            SetCurrentUser(await _userRepository.GetAsync(CustomPrincipal.Identity.Login));
        }

        public async Task DeleteAccountAsync()
        {
            await _userRepository.DeleteAsync(CustomPrincipal.Identity.Id);
            ResetCurrentUser();
        }
    }
}
