using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public class RemoveControllerFromEndOfTypeNameModifier : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodCallExpression expr)
        {
            return name.Replace("Controller.", ".");
        }
    }
}