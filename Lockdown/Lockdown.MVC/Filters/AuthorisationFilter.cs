using Lockdown.MVC.ActionResults;
using Lockdown.MVC.Client;
using Lockdown.MVC.Config;
using Lockdown.MVC.Tokens;
using System.Web.Mvc;
using System.Web.Mvc.Async;

namespace Lockdown.MVC.Filters
{
    public class AuthorisationFilter : ActionFilterAttribute
    {
        private readonly IAuthorizationClientFactory _clientFactory;
        private readonly ITokenFactory _factory;
        private readonly string _appName;
        private readonly string _stripPrefix;
        private readonly bool _stripControllerSuffix;

        public AuthorisationFilter(IAuthorizationClientFactory clientFactory, ITokenFactory factory, string appName, string stripPrefix, bool stripControllerSuffix)
        {
            _clientFactory = clientFactory;
            _factory = factory;
            _appName = appName;
            _stripPrefix = stripPrefix;
            _stripControllerSuffix = stripControllerSuffix;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isAsync = filterContext.ActionDescriptor is TaskAsyncActionDescriptor;

            var m = isAsync
                ? (filterContext.ActionDescriptor as TaskAsyncActionDescriptor).TaskMethodInfo
                : (filterContext.ActionDescriptor as ReflectedActionDescriptor).MethodInfo;


            var opName = ConfigureFluent.GetOpName(m, _stripPrefix, _stripControllerSuffix);

            var tokenStore = OperationStore.Current(_appName, _clientFactory, _factory);
            var authorised = tokenStore.IsAuthorized(opName);

            if (authorised)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            filterContext.Result = new HttpForbiddenResult();
        }
    }
}