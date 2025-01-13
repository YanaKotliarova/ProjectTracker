using ProjectTracker.MVVM.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace ProjectTracker.MVVM.Model
{
    public class User : IUser
    {
        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public SecureString Password { get; set; }
        public string Role { get; set; }
        public List<Project> Projects { get; set; } = new();

        public User() { }
        public User(string login, SecureString password, string role)
        {
            Login = login;
            Password = password;
            Role = role;
        }
    }
}
