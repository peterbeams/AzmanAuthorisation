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

            if (authorised)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new HttpForbiddenResult();
        }
    }

    public class HttpForbiddenResult : ContentResult
    {
        public HttpForbiddenResult()
        {
            Content = "403 Forbidden";
            ContentType = "text/plain";
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.StatusCode = 403;
            base.ExecuteResult(context);
        }
    }

}