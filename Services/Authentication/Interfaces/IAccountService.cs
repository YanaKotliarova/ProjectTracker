using ProjectTracker.MVVM.Model;
using ProjectTracker.Services.Authentication.CurrentUser;

namespace ProjectTracker.Services.Authentication
{
    public interface IAccountService
    {
        CustomPrincipal CustomPrincipal { get; }

        //User CurrentUser { get; set; }
        Task UpdateUserPersonalInfoAsync(string newLogin, string newRole);
        Task UpdateUserPasswordAsync(string newPassword);
        Task DeleteAccountAsync();
        Task<bool> CheckIfLoginExistsAsync(string login);
        void ResetCurrentUser();
        void SetCurrentUser(User currentUser);
    }
}