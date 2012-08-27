using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Lockdown.Configuration.Operations
{
    public class OperationIdentifierFactory
    {
        private readonly IModifyOperationName[] _nameModifiers = new IModifyOperationName[]
                                {
                                    new RemoveAreasNamespaceModifier(),
                                    new RemoveControllersNamespaceModifier(),
                                    new RemoveControllerFromEndOfTypeNameModifier(), 
                                    new AppendPostToNameForHttpPostMethods(), 
                                    new RemoveRootNamespaceFromStartOfName()
                                };

        private readonly IConstrainOperationIdentifiers[] _constraints = new IConstrainOperationIdentifiers[]
                                                                             {
                                                                                 new OperationNamesAreLimitedLength(64)
                                                                             };

        public string RootNamespace { get; set; }

        public OperationIdentifier Create(MethodInfo method)
        {
            var name = string.Concat(method.DeclaringType.FullName, ".", method.Name);

            name = ApplyModifers(method, name);
            CheckConstraints(name);

            return new OperationIdentifier
            {
                Name = name
            };
        }

        private string ApplyModifers(MethodInfo method, string name)
        {
            foreach (var m in _nameModifiers)
            {
                name = m.Apply(RootNamespace, name, method);
            }
            return name;
        }

        private void CheckConstraints(string name)
        {
            foreach (var c in _constraints)
            {
                c.Check(name);
            }
        }

        public OperationIdentifier Create<T>(Expression<Action<T>>  action)
        {
            var lambda = (LambdaExpression) action;
            var expr = (MethodCallExpression) lambda.Body;
            return Create(expr.Method);
        }
    }
}