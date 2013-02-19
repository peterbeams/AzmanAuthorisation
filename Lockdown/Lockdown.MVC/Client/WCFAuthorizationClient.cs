using System.ServiceModel;
using System.ServiceModel.Channels;
using Lockdown.Messages;
using Lockdown.Messages.Data;

namespace Lockdown.MVC.Client
{
    public abstract class WCFAuthorizationClient : ClientBase<AuthorizationService>, AuthorizationService
    {
        protected WCFAuthorizationClient(Binding binding, EndpointAddress address)
            : base(binding, address)
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

        public string[] GetRoles(string appName, UserToken token)
        {
            return Channel.GetRoles(appName, token);
        }
    }
}