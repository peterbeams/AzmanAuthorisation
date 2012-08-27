using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public class RemoveRootNamespaceFromStartOfName : IModifyOperationName
    {
        public string Apply(string rootNamespace, string name, MethodCallExpression expr)
        {
            if (!string.IsNullOrEmpty(rootNamespace) && name.StartsWith(rootNamespace))
            {
                return name.Substring(rootNamespace.Length + 1, name.Length - rootNamespace.Length - 1);
            }

            return name;
        }
    }
}