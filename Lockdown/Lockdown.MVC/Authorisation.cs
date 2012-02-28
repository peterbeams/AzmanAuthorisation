using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web.Mvc;
using Lockdown.Messages.Commands;

namespace Lockdown.MVC
{
    public class AuthorizationClient : ClientBase<AuthorizationService>, AuthorizationService
    {
        private static readonly NetNamedPipeBinding binding = new NetNamedPipeBinding();
        
        public AuthorizationClient()
            : base(binding, new EndpointAddress("net.pipe://localhost/lockdown.host"))
        {
        }

        public AuthorizedOperations GetAuthorisedOperations(string appName, string userName, string domain)
        {
            return Channel.GetAuthorisedOperations(appName, userName, domain);
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            Channel.RegisterOperations(appName, operationNames);
        }
    }

    public class Authorisation
    {
        public static IConfigApp Configure { get { return new ConfigureFluent(); } }
    }

    public class AssemblyScanConfig
    {
        private readonly Assembly _assembly;

        public AssemblyScanConfig(Assembly assembly)
        {
            _assembly = assembly;
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }
    }

    public class In
    {
        public static AssemblyScanConfig AssemlbyContaining<T>()
        {
            return new AssemblyScanConfig(typeof(T).Assembly);
        }
    }

    public interface IConfigApp
    {
        IFindOperations Application(string name);
    }

    public interface IFindOperations
    {
        IClientConfig ScanControllers(AssemblyScanConfig scanning);
    }

    public interface IClientConfig
    {
        void UseNamedPipeClient();
    }

    public class ConfigureFluent : IFindOperations, IClientConfig, IConfigApp
    {
        private List<string> operations = new List<string>();
        private string name;

        public IFindOperations Application(string name)
        {
            this.name = name;
            return this;
        }

        public IClientConfig ScanControllers(AssemblyScanConfig scanning)
        {
            var controllerTypes = from t in scanning.Assembly.GetTypes()
                                  where typeof(Controller).IsAssignableFrom(t)
                                        && t.IsPublic
                                  select t;

            var actionMethods = from m in controllerTypes.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                                where typeof(ActionResult).IsAssignableFrom(m.ReturnType)
                                select m;

            foreach (var m in actionMethods)
            {
                var opName = GetOpName(m);
                operations.Add(opName);
            }

            return this;
        }

        internal static string GetOpName(MethodInfo m)
        {
            var opName = string.Format("{0}.{1}", m.DeclaringType.FullName, m.Name);
            if (m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
            {
                opName = string.Concat(opName, "[POST]");
            }
            return opName;
        }

        public void UseNamedPipeClient()
        {
            GlobalFilters.Filters.Add(new AuthorisationFilter(name));
            
            //register operations
            var client = new AuthorizationClient();
            client.RegisterOperations(name, operations.ToArray());
        }
    }
}
