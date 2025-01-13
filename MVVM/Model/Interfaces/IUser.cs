using System.Security;

namespace ProjectTracker.MVVM.Model.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string Login { get; set; }
        SecureString Password { get; set; }
        List<Project> Projects { get; set; }
        string Role { get; set; }
    }
}