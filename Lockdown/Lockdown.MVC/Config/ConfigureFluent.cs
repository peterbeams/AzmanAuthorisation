using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Lockdown.Configuration.Operations;
using Lockdown.MVC.Client;
using Lockdown.MVC.Filters;
using Lockdown.MVC.Tokens;

namespace Lockdown.MVC.Config
{
    public class ConfigureFluent : IFindOperations, IClientConfig, IConfigApp, ITokenFactoryConfig
    {
        private List<string> operations = new List<string>();
        private string name;
        private string _stripPrefix;
        private ITokenFactory _tokenFactory;

        public IFindOperations Application(string name)
        {
            this.name = name;
            ScriptExtensions.AppName = name;
            return this;
        }

        public IClientConfig UseTokenFactory<T>() where T : ITokenFactory, new()
        {
            _tokenFactory = new T();
            ScriptExtensions.TokenFactory = _tokenFactory;
            return this;
        }

        public IClientConfig UseTokenFactory(Func<ITokenFactory> createFactoryFunction)
        {
            _tokenFactory = createFactoryFunction.Invoke();
            ScriptExtensions.TokenFactory = _tokenFactory;
            return this;
        }

        public ITokenFactoryConfig ScanControllers(AssemblyScanConfig scanning)
        {
            _stripPrefix = scanning.StripPrefix;

            var controllerTypes = from t in scanning.Assembly.GetTypes()
                                  where typeof(Controller).IsAssignableFrom(t)
                                        && t.IsPublic && !t.IsAbstract && !t.IsGenericTypeDefinition && !t.IsGenericType
                                  select t;

            var actionMethods = from m in controllerTypes.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                                where typeof(ActionResult).IsAssignableFrom(m.ReturnType)
                                select m;

            var operationFactory = new OperationIdentifierFactory
                                       {
                                           RootNamespace = scanning.StripPrefix
                                       };

            foreach (var m in actionMethods)
            {
                var opName = operationFactory.Create(m).Name;

                operations.Add(opName);
            }
            
            return this;
        }

        public void UseNamedPipeClient()
        {
            UseClient(new NamedPipeAuthorizationClientFactory());
        }

        public void UseDebugClient()
        {
            UseClient(new DebugClientFactory());
        }

        private void UseClient(IAuthorizationClientFactory clientFactory)
        {
            GlobalFilters.Filters.Add(new AuthorisationFilter(clientFactory, _tokenFactory, name, _stripPrefix));

            ScriptExtensions.ClientFactory = clientFactory;

            var client = clientFactory.CreateClient();
            client.RegisterOperations(name, operations.ToArray());
        }
    }
}