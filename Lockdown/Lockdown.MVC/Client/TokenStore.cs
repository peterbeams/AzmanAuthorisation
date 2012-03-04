using System;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using Lockdown.Messages.Commands;

namespace Lockdown.MVC.Client
{
    public class TokenStore
    {
        private readonly string[] _authzOps;

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static TokenStore Current(string appName)
        {

            var t = Session["TokenStore"] as TokenStore;
            if (t == null)
            {
                t = Create(appName);
                Session["TokenStore"] = t;
            }
            return t;
        }

        public static TokenStore Create(string appName)
        {
            var client = new AuthorizationClient();

            var sids = new[]
                           {
                               "S-1-5-21-4001081062-1862719798-2782911012-1000",
                               "S-1-5-21-4001081062-1862719798-2782911012-1001",
                               "S-1-5-32-544",
                               "S-1-5-32-545",
                               "S-1-5-4",
                               "S-1-2-1",
                               "S-1-5-11",
                               "S-1-5-15",
                               "S-1-2-0",
                               "S-1-5-64-10",
                               "S-1-16-8192",
                               "S-1-1-0"
                           };
            var token = new UserToken { Sids = sids };

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