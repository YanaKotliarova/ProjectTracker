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

        /// <summary>
        /// The method for setting CurrentPrincipal of app.
        /// </summary>
        /// <param name="currentUser"></param>
        public void SetCurrentUser(User currentUser)
        {
            CustomPrincipal.Identity = new CustomIdentity(currentUser.Id, currentUser.Login, currentUser.Role);
        }

        /// <summary>
        /// The method for resetting CurrentPrincipal of app.
        /// </summary>
        public void ResetCurrentUser()
        {
            CustomPrincipal.Identity = new AnonymousIdentity();
        }

        /// <summary>
        /// A property to get CurrentPrincipal of app.
        /// </summary>
        public CustomPrincipal CustomPrincipal => Thread.CurrentPrincipal as CustomPrincipal;

        /// <summary>
        /// The method for updating user personal info.
        /// </summary>
        /// <param name="newLogin"> User's new login. </param>
        /// <param name="newRole"> User's new role. </param>
        /// <returns></returns>
        public async Task UpdateUserPersonalInfoAsync(string newLogin, string newRole)
        {
            User currentUser = await _userRepository.GetAsync(CustomPrincipal.Identity.Login);

            currentUser.Login = newLogin;
            currentUser.Role = newRole;

            await _userRepository.UpdateAsync(currentUser);

            SetCurrentUser(await _userRepository.GetAsync(newLogin));
        }

        /// <summary>
        /// The method for checking if entered login is exists.
        /// </summary>
        /// <param name="login"> Login for checking. </param>
        /// <returns> True if it exists, otherwise false. </returns>
        public async Task<bool> CheckIfLoginExistsAsync(string login)
        {
            if (CustomPrincipal != null && login.Equals(CustomPrincipal.Identity.Login))
                return false;
            else return await _userRepository.IsLoginExists(login);
        }

        /// <summary>
        /// The method for updating user password.
        /// </summary>
        /// <param name="newPassword"> New user's password. </param>
        /// <returns></returns>
        public async Task UpdateUserPasswordAsync(string newPassword)
        {
            User currentUser = await _userRepository.GetAsync(CustomPrincipal.Identity.Login);
            currentUser.Password = _userRepository.GetPasswordHashCode(newPassword);

            await _userRepository.UpdateAsync(currentUser);

            SetCurrentUser(await _userRepository.GetAsync(CustomPrincipal.Identity.Login));
        }

        /// <summary>
        /// The method for deleting user's account from app.
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAccountAsync()
        {
            await _userRepository.DeleteAsync(CustomPrincipal.Identity.Id);
            ResetCurrentUser();
        }
    }
}
