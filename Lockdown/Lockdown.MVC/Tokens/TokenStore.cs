using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Lockdown.Messages.Data;
using Lockdown.MVC.Client;

namespace Lockdown.MVC.Tokens
{
    public class TokenStore
    {
        private readonly string[] _authzOps;

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static TokenStore Current(string appName, IAuthorizationClientFactory clientFactory, ITokenFactory factory)
        {

            var t = Session["TokenStore"] as TokenStore;
            if (t == null)
            {
                t = Create(appName, clientFactory, factory);
                Session["TokenStore"] = t;
            }
            return t;
        }

        public static TokenStore Create(string appName, IAuthorizationClientFactory clientFactory, ITokenFactory factory)
        {
            var client = clientFactory.CreateClient();
            var token = factory.GetCurrent();
            var result = client.GetAuthorisedOperations(appName, token);

            return new TokenStore(result);
        }

        public TokenStore(AuthorizedOperations ops)
        {
            _authzOps = ops.OperationNames;
        }

        public bool IsAuthorized(string operationName)
        {
            return _authzOps.Any(o => o.Equals(operationName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}