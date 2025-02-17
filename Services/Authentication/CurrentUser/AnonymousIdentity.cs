namespace ProjectTracker.Services.Authentication.CurrentUser
{
    public class AnonymousIdentity : CustomIdentity
    {
        public AnonymousIdentity()
            : base(0, string.Empty, string.Empty)
        { }
    }
}
