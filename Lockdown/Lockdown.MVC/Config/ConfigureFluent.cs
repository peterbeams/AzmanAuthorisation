using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
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
        private bool _stripControllerSuffix;

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
            _stripControllerSuffix = scanning.StripControllerSuffix;

            var controllerTypes = from t in scanning.Assembly.GetTypes()
                                  where typeof(Controller).IsAssignableFrom(t)
                                        && t.IsPublic && !t.IsAbstract && !t.IsGenericTypeDefinition && !t.IsGenericType
                                  select t;
            
            var actionMethods = from m in controllerTypes.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                                where scanning.ActionsDefinedAs(m)
                                    select m;    
            
            foreach (var m in actionMethods)
            {
                var opName = GetOpName(m, scanning.StripPrefix, scanning.StripControllerSuffix);

                if (opName.Length > 64)
                {
                    throw new Exception(string.Format("Operation name is too long.  Max length is 64 chars. '{0}'", opName));
                }                

                operations.Add(opName);
            }
            
            return this;
        }

        internal static string GetOpName(MethodInfo m, string stripPrefix, bool stripControllerSuffix)
        {
            var opName = string.Format("{0}.{1}", m.ReflectedType.FullName, m.Name);
            if (m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
            {
                opName = string.Concat(opName, "[POST]");
            }

            opName = opName.Replace(".Areas", string.Empty);
            opName = opName.Replace(".Controllers", string.Empty);

            if (stripControllerSuffix)
            {
                opName = opName.Replace("Controller.", ".");
            }

            if (!string.IsNullOrEmpty(stripPrefix))
            {
                opName = opName.Replace(stripPrefix, string.Empty);
            }

            if (opName.StartsWith("."))
            {
                opName = opName.Substring(1, opName.Length - 1);
            }

            return opName;
        }

        public void UseNamedPipeClient()
        {
            UseClient(new NamedPipeAuthorizationClientFactory());
        }

        public void UseDebugClient()
        {
            UseDebugClient(new string[] {});
        }

        public void UseDebugClient(string[] roles)
        {
            UseClient(new DebugClientFactory(roles));
        }

        private void UseClient(IAuthorizationClientFactory clientFactory)
        {
            GlobalFilters.Filters.Add(new AuthorisationFilter(clientFactory, _tokenFactory, name, _stripPrefix, _stripControllerSuffix));

            ScriptExtensions.ClientFactory = clientFactory;

            var client = clientFactory.CreateClient();
            client.RegisterOperations(name, operations.ToArray());
        }
    }
}