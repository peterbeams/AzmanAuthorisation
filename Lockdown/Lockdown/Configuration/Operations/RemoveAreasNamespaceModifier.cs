using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public class RemoveAreasNamespaceModifier : IModifyOperationName
    {
        public string Apply(string name, MethodCallExpression expr)
        {
            return name.Replace(".Areas.", ".");
        }
    }
}