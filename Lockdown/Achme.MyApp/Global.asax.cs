using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Lockdown.Messages.Data;
using Lockdown.MVC;
using Lockdown.MVC.Config;
using Lockdown.MVC.Tokens;
using NServiceBus;

namespace Achme.MyApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private const string AppName = "AchmeApp";

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        
        protected void Application_Start()
        {
            Authorisation.Configure
                .Application(AppName)
                .ScanControllers(In.AssemblyContaining<MvcApplication>("Achme.MyApp").DefiningActionsAs(m => true))                
                .UseTokenFactory<TokenFactory>()
                .UseDebugClient();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }

    public class TokenFactory : ITokenFactory
    {
        public UserToken GetCurrent()
        {
            return new UserToken();
        }

        public void Clear()
        {
            
        }
    }
}