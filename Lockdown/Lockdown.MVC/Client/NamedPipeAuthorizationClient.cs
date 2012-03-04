using System.ServiceModel;

namespace Lockdown.MVC.Client
{
    public class NamedPipeAuthorizationClient : WCFAuthorizationClient
    {
        private static readonly NetNamedPipeBinding binding = new NetNamedPipeBinding();

        public NamedPipeAuthorizationClient()
            : base(binding, new EndpointAddress("net.pipe://localhost/lockdown.host"))
        {
        }
    }
}