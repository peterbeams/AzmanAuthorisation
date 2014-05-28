using Lockdown.Messages;

namespace Lockdown.MVC.Client
{
    public class DebugClientFactory : IAuthorizationClientFactory
    {
        private readonly string[] _roles;

        public DebugClientFactory(string[] roles)
        {
            _roles = roles;
        }

        public AuthorizationService CreateClient()
        {
            return new DebugClient(_roles);
        }
    }

    public class NamedPipeAuthorizationClientFactory : IAuthorizationClientFactory
    {
        public AuthorizationService CreateClient()
        {
            return new NamedPipeAuthorizationClient();
        }
    }
}