using System;
using System.Linq.Expressions;
using System.Reflection;

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

        public OperationIdentifier Create(MethodInfo method)
        {
            var name = string.Concat(method.DeclaringType.FullName, ".", method.Name);

            foreach (var m in _nameModifiers)
            {
                name = m.Apply(RootNamespace, name, method);
            }

            return new OperationIdentifier
            {
                Name = name
            };
        }

        public OperationIdentifier Create<T>(Expression<Action<T>>  action)
        {
            var lambda = (LambdaExpression) action;
            var expr = (MethodCallExpression) lambda.Body;
            return Create(expr.Method);
        }
    }
}