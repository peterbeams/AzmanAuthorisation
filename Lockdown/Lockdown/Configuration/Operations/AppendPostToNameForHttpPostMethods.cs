using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Lockdown.Configuration.Operations
{
    public class AppendPostToNameForHttpPostMethods : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodCallExpression expr)
        {
            if (expr.Method.GetCustomAttributes(typeof(HttpPostAttribute), true).Any())
            {
                return string.Concat(name, "[POST]");
            }

            return name;
        }
    }
}