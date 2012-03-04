using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Lockdown.MVC.Client;
using Lockdown.MVC.Filters;

namespace Lockdown.MVC.Config
{
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