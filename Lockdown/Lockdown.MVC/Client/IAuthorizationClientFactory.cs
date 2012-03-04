using Lockdown.Messages;

namespace Lockdown.MVC.Client
{
    public interface IAuthorizationClientFactory
    {
        AuthorizationService CreateClient();
    }
}