using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public interface IModifyOperationName
    {
        string Apply(string rootNamespace, string name, MethodCallExpression expr);
    }
}