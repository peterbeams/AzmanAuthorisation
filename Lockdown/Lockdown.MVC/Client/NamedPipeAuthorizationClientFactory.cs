using Lockdown.Messages;

namespace Lockdown.MVC.Client
{
    public class DebugClientFactory : IAuthorizationClientFactory
    {
        public AuthorizationService CreateClient()
        {
            return new DebugClient();
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