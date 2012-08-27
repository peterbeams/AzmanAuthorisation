using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace Lockdown.Configuration.Operations
{
    public class AppendPostToNameForHttpPostMethods : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodInfo method)
        {
            if (method.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
            {
                return string.Concat(name, "[POST]");
            }

            return name;
        }
    }
}