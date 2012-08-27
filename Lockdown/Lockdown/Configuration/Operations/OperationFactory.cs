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
                                    new RemoveRootNamespaceFromStartOfName()
                                };

        public string RootNamespace { get; set; }

        public OperationIdentifier CreateForMethodCall<T>(Expression<Action<T>>  action)
        {
            var lambda = (LambdaExpression) action;
            var expr = (MethodCallExpression) lambda.Body;

            var name = string.Concat(expr.Method.DeclaringType.FullName, ".", expr.Method.Name);

            foreach (var m in _nameModifiers)
            {
                name = m.Apply(RootNamespace, name, expr);
            }
            
            return new OperationIdentifier
                       {
                           Name = name
                       };
        }
    }
}