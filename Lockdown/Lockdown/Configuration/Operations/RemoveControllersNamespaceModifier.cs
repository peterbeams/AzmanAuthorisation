using System.Linq.Expressions;
using System.Reflection;

namespace Lockdown.Configuration.Operations
{
    public class RemoveControllersNamespaceModifier : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodInfo method)
        {
            return name.Replace(".Controllers.", ".");
        }
    }
}