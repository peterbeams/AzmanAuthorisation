using System;
using System.ServiceModel;
using Lockdown.Messages;
using Lockdown.Messages.Data;

namespace Lockdown.Host
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AzmanAuthzService : AuthorizationService
    {
        private AuthorizationStore Store { get { return Program.Store; } }

        public AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token)
        {
            try
            {
                Store.UsingApplication(appName);

                var opNames = Store.GetAuthroizedOperations(token.Sids);
                return new AuthorizedOperations { OperationNames = opNames };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            try
            {
                Store.UsingApplication(appName);
                Array.ForEach(operationNames, o => Store.EnsureOperationByName(o));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}