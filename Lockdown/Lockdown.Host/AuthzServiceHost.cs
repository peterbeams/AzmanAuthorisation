using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Description;
using Lockdown.Messages;

namespace Lockdown.Host
{
    public class AuthzServiceHost
    {
        private ServiceHost host;
        internal static AuthorizationStore Store { get; private set; }

        public AuthzServiceHost()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["authzStore"].ConnectionString;
            Store = new AuthorizationStore(connectionString);

            var endPoint = new NetNamedPipeBinding();
            var uris = new[] { new Uri("net.pipe://localhost/lockdown.host") };

            host = new ServiceHost(typeof(AzmanAuthzService), uris);

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true });

            host.AddServiceEndpoint(typeof(AuthorizationService), endPoint, uris[0]);
        }

        public void Start()
        {
            host.Open();
        }

        public void Stop()
        {
            host.Close();
        }
    }
}