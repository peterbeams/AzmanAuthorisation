using System.Linq.Expressions;
using System.Reflection;

namespace Lockdown.Configuration.Operations
{
    public class RemoveAreasNamespaceModifier : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodInfo method)
        {
            return name.Replace(".Areas.", ".");
        }
    }
}