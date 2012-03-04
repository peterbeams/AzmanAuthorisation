using System;
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
        private string _stripPrefix;

        public IFindOperations Application(string name)
        {
            this.name = name;
            return this;
        }

        public IClientConfig ScanControllers(AssemblyScanConfig scanning)
        {
            _stripPrefix = scanning.StripPrefix;

            var controllerTypes = from t in scanning.Assembly.GetTypes()
                                  where typeof(Controller).IsAssignableFrom(t)
                                        && t.IsPublic && !t.IsAbstract && !t.IsGenericTypeDefinition && !t.IsGenericType
                                  select t;

            var actionMethods = from m in controllerTypes.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                                where typeof(ActionResult).IsAssignableFrom(m.ReturnType)
                                select m;

            foreach (var m in actionMethods)
            {
                var opName = GetOpName(m, scanning.StripPrefix);

                if (opName.Length > 64)
                {
                    throw new Exception(string.Format("Operation name is too long.  Max length is 64 chars. '{0}'", opName));
                }                

                operations.Add(opName);
            }
            
            return this;
        }

        internal static string GetOpName(MethodInfo m, string stripPrefix)
        {
            var opName = string.Format("{0}.{1}", m.ReflectedType.FullName, m.Name);
            if (m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
            {
                opName = string.Concat(opName, "[POST]");
            }

            opName = opName.Replace(".Areas", string.Empty);
            opName = opName.Replace(".Controllers", string.Empty);
            opName = opName.Replace(stripPrefix, string.Empty);

            if (opName.StartsWith("."))
            {
                opName = opName.Substring(1, opName.Length - 1);
            }

            return opName;
        }

        public void UseNamedPipeClient()
        {
            GlobalFilters.Filters.Add(new AuthorisationFilter(name, _stripPrefix));
            
            //register operations
            var client = new AuthorizationClient();
            client.RegisterOperations(name, operations.ToArray());
        }
    }
}