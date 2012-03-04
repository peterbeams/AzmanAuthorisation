using System;
using System.Configuration;
using System.Security.Principal;
using System.ServiceModel;
using Lockdown.Messages;
using Lockdown.Messages.Data;

namespace Lockdown.Host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AzmanAuthzService : AuthorizationService
    {
        private AuthorizationStore Store { get { return Program.Store;  } }

        public AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token)
        {
            Store.UsingApplication(appName);

            var opNames = Store.GetAuthroizedOperations(token.Sids);
            return new AuthorizedOperations { OperationNames = opNames };
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            Store.UsingApplication(appName);
            Array.ForEach(operationNames, o => Store.EnsureOperationByName(o));
        }
    }

   public class Program
   {
       internal static AuthorizationStore Store { get; private set; }

       public static void Main(string[] args)
       {
           var connectionString = ConfigurationManager.ConnectionStrings["authzStore"].ConnectionString;
           Store = new AuthorizationStore(connectionString);

           var endPoint = new NetNamedPipeBinding();
           var uris = new [] { new Uri("net.pipe://localhost/lockdown.host") };

           var host = new ServiceHost(typeof (AzmanAuthzService), uris);
           host.AddServiceEndpoint(typeof(AuthorizationService), endPoint, uris[0]);

           host.Open();

           Console.ReadLine();
       }
   }
}
