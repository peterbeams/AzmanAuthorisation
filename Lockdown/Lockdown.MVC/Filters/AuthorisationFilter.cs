﻿using System.Web.Mvc;
using Lockdown.Configuration.Operations;
using Lockdown.MVC.ActionResults;
using Lockdown.MVC.Client;
using Lockdown.MVC.Config;
using Lockdown.MVC.Tokens;

namespace Lockdown.MVC.Filters
{
    public class AuthorisationFilter : ActionFilterAttribute
    {
        private readonly IAuthorizationClientFactory _clientFactory;
        private readonly ITokenFactory _factory;
        private readonly string _appName;
        private readonly string _stripPrefix;

        public AuthorisationFilter(IAuthorizationClientFactory clientFactory, ITokenFactory factory, string appName, string stripPrefix)
        {
            _clientFactory = clientFactory;
            _factory = factory;
            _appName = appName;
            _stripPrefix = stripPrefix;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var t = (ReflectedActionDescriptor)filterContext.ActionDescriptor;
            var m = t.MethodInfo;

            var operationFactory = new OperationIdentifierFactory
                                       {
                                           RootNamespace = _stripPrefix
                                       };

            var opName = operationFactory.Create(m).Name;

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