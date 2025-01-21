using ProjectTracker.MVVM.Model;

namespace ProjectTracker.Services.Authentication
{
    public interface IAccount
    {
        User CurrentUser { get; set; }
        Task UpdateUserPersonalInfoAsync(string newLogin, string newRole);
        Task UpdateUserPasswordAsync(string newPassword);
        Task DeleteAccountAsync();
        Task<bool> CheckIfLoginExistsAsync(string login);
    }
}