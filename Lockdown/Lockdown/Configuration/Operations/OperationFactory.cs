using System;
using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public class OperationFactory
    {
        private readonly IModifyOperationName[] _nameModifiers = new IModifyOperationName[]
                                {
                                    new RemoveAreasNamespaceModifier(),
                                    new RemoveControllersNamespaceModifier(),
                                    new RemoveControllerFromEndOfTypeNameModifier(), 
                                    new AppendPostToNameForHttpPostMethods(), 
                                };

        public OperationIdentifier CreateForMethodCall<T>(Expression<Action<T>>  action)
        {
            var lambda = (LambdaExpression) action;
            var expr = (MethodCallExpression) lambda.Body;

            var name = string.Concat(expr.Method.DeclaringType.FullName, ".", expr.Method.Name);

            foreach (var m in _nameModifiers)
            {
                name = m.Apply(name, expr);
            }
            
            return new OperationIdentifier
                       {
                           Name = name
                       };
        }
    }
}