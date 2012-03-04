using System.ServiceModel;
using Lockdown.Messages;
using Lockdown.Messages.Data;

namespace Lockdown.MVC.Client
{
    public class AuthorizationClient : ClientBase<AuthorizationService>, AuthorizationService
    {
        private static readonly NetNamedPipeBinding binding = new NetNamedPipeBinding();
        
        public AuthorizationClient()
            : base(binding, new EndpointAddress("net.pipe://localhost/lockdown.host"))
        {
        }

        public AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token)
        {
            return Channel.GetAuthorisedOperations(appName, token);
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            Channel.RegisterOperations(appName, operationNames);
        }
    }
}