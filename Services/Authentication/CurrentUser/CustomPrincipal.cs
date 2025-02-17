using System.Security.Principal;

namespace ProjectTracker.Services.Authentication.CurrentUser
{
    public class CustomPrincipal : IPrincipal
    {
        private CustomIdentity _identity;

        public CustomIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        IIdentity IPrincipal.Identity
        {
            get { return (IIdentity)Identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.Role.Equals(role);
        }
    }
}
