using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public interface IModifyOperationName
    {
        string Apply(string name, MethodCallExpression expr);
    }
}