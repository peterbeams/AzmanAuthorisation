using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Lockdown.Messages.Data;
using Lockdown.MVC.Client;

namespace Lockdown.MVC.Tokens
{
    public class OperationStore
    {
        private readonly string[] _authzOps;
        private const string GrantedOperationsSessionKey = "GrantedOperations";

        public string[] Values
        {
            get { return _authzOps; }
        }

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static OperationStore Stored
        {
            get { return Session[GrantedOperationsSessionKey] as OperationStore; }
        }

        public static OperationStore Current(string appName, IAuthorizationClientFactory clientFactory, ITokenFactory factory)
        {
            var t = Stored;
            if (t == null)
            {
                t = Create(appName, clientFactory, factory);
                Session[GrantedOperationsSessionKey] = t;
            }
            return t;
        }

        public static OperationStore Create(string appName, IAuthorizationClientFactory clientFactory, ITokenFactory factory)
        {
            var client = clientFactory.CreateClient();
            var token = factory.GetCurrent();
            var result = client.GetAuthorisedOperations(appName, token);

            return new OperationStore(result);
        }

        public static void Clear()
        {
            Session[GrantedOperationsSessionKey] = null;
        }

        public OperationStore(AuthorizedOperations ops)
        {
            _authzOps = ops.OperationNames;
        }

        public bool IsAuthorized(string operationName)
        {
            return _authzOps.Any(o => o.Equals(operationName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}