using System;
using System.Linq.Expressions;

namespace Lockdown.Configuration.Operations
{
    public class OperationFactory
    {
        public IModifyOperationName[] nameModifiers;

        public OperationFactory()
        {
            nameModifiers = new IModifyOperationName[]
                                {
                                    new RemoveAreasNamespaceModifier(),
                                    new RemoveControllersNamespaceModifier(),
                                    new RemoveControllerFromEndOfTypeNameModifier(), 
                                };
        }

        public OperationIdentifier CreateForMethodCall<T>(Expression<Action<T>>  action)
        {
            var lambda = (LambdaExpression) action;
            var expr = (MethodCallExpression) lambda.Body;

            var name = string.Concat(expr.Method.DeclaringType.FullName, ".", expr.Method.Name);

            foreach (var m in nameModifiers)
            {
                name = m.Apply(name);
            }
            
            return new OperationIdentifier
                       {
                           Name = name
                       };
        }
    }
}