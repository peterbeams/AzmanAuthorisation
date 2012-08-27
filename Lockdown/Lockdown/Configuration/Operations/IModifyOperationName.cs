using System.Linq.Expressions;
using System.Reflection;

namespace Lockdown.Configuration.Operations
{
    public interface IModifyOperationName
    {
        string Apply(string rootNamespace, string name, MethodInfo method);
    }
}