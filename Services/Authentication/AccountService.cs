using ProjectTracker.Data.Interfaces;
using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Services.Authentication
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        public AccountService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? CurrentUser { get; set; }

        public async Task UpdateUserPersonalInfoAsync(string newLogin, string newRole)
        {
            CurrentUser!.Login = newLogin;
            CurrentUser!.Role = newRole;
            User updateUser = CurrentUser;

            await _userRepository.UpdateAsync(updateUser);

            CurrentUser = await _userRepository.GetAsync(newLogin);
        }

        public async Task<bool> CheckIfLoginExistsAsync(string login)
        {
            if (CurrentUser != null && login.Equals(CurrentUser.Login))
                return false;
            else return await _userRepository.IsLoginExists(login);
        }

        public async Task UpdateUserPasswordAsync(string newPassword)
        {
            CurrentUser!.Password = _userRepository.GetPasswordHashCode(newPassword);
            User updatedUser = CurrentUser;

            await _userRepository.UpdateAsync(updatedUser);

            CurrentUser = await _userRepository.GetAsync(CurrentUser.Login);
        }

        public async Task DeleteAccountAsync()
        {
            await _userRepository.DeleteAsync(CurrentUser!.Id);
        }
    }
}
