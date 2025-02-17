namespace ProjectTracker.Services.Authentication.CurrentUser
{
    public class CustomIdentity
    {
        public CustomIdentity(int id, string login, string role)
        {
            Id = id;
            Login = login;
            Role = role;
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }

        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(Login); } }
    }
}
