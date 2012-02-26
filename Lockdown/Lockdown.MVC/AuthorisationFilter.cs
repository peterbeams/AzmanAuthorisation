using System;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using Lockdown.Messages.Commands;

namespace Lockdown.MVC
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
            var result = client.GetAuthorisedOperations(appName);
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

    public class ControllerActionIsNotAuthorizedException : SecurityException
    {
        public ControllerActionIsNotAuthorizedException(string operationName)
            : base(string.Format("Operation '{0}' is not authorized for current user.", operationName))
        {
        }
    }

    public class AuthorisationFilter : ActionFilterAttribute
    {
        private readonly string _appName;

        public AuthorisationFilter(string appName)
        {
            _appName = appName;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var t = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
            var m = t.MethodInfo;

            var opName = ConfigureFluent.GetOpName(m);

            var tokenStore = TokenStore.Current(_appName);
            var authorised = tokenStore.IsAuthorized(opName);

            if (!authorised)
            {
                throw new ControllerActionIsNotAuthorizedException(opName);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}