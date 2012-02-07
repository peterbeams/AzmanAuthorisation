using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Lockdown.MVC
{
    public class Authorisation
    {
        public static ConfigureFluent Configure { get { return new ConfigureFluent(); } }
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

    public class ConfigureFluent
    {
        public void ScanForControllers(AssemblyScanConfig scanning)
        {
            var controllerTypes = from t in scanning.Assembly.GetTypes()
                                  where typeof(Controller).IsAssignableFrom(t)
                                        && t.IsPublic
                                  select t;

            var actionMethods = from m in controllerTypes.SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                                where typeof (ActionResult).IsAssignableFrom(m.ReturnType)
                                select m;

            var operations = new List<string>();

            foreach (var m in actionMethods)
            {
                var opName = string.Format("{0}.{1}", m.DeclaringType.FullName, m.Name);
                if (m.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
                {
                    opName = string.Concat(opName, "[POST]");
                }
                operations.Add(opName);
            }

        }
    }
}
