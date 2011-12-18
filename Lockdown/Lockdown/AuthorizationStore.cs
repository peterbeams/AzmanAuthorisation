using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AZROLESLib;

namespace Lockdown
{
    public class AuthorizationStore
    {
        private readonly IAzApplication _application;

        public AuthorizationStore(string connectionString)
        {
            var store = new AzAuthorizationStore();
            store.Initialize(0, connectionString, null);
            _application = store.OpenApplication("MyApp", null);
        }

        public IEnumerable<Operation> GetOperations()
        {
            return GetEntityListFromAzmanEnumerator<IAzOperation2, Operation>(() => _application.Operations, o => true, o => new Operation
                                                              {
                                                                  Name = o.Name,
                                                                  Id = o.OperationID
                                                              });
        }

        public IEnumerable<Role> GetRoles()
        {
            return GetEntityListFromAzmanEnumerator<IAzTask2, Role>(() => _application.Tasks, o => o.IsRoleDefinition == 1, CreateRole);
        }

        private Role CreateRole(IAzTask2 o)
        {
            var opNames = new List<string>();
            foreach (var s in o.Operations)
            {
                opNames.Add(s);
            }

            var operations = GetOperations();
            operations = operations.Where(op => opNames.Any(opName => opName == op.Name));

            return new Role
                       {
                           Name = o.Name,
                           Operations = operations
                       };
        }

        public IEnumerable<Task> GetTasks()
        {
            return GetEntityListFromAzmanEnumerator<IAzTask2, Task>(() => _application.Tasks, o => o.IsRoleDefinition != 1, o => new Task
                                                                                                    {
                                                                                                        Name = o.Name
                                                                                                    });
        }

        public IEnumerable<TEntity> GetEntityListFromAzmanEnumerator<TAzItem, TEntity>(Func<IEnumerable> azmanListFunc, Func<TAzItem, bool> includeAny, Func<TAzItem, TEntity> createEntity)
        {
            var azmanList = azmanListFunc.Invoke();
            var enumerator = azmanList.GetEnumerator();
            var entityList = new List<TEntity>();

            try
            {
                while (enumerator.MoveNext())
                {
                    var o = (TAzItem)enumerator.Current;

                    if (!includeAny.Invoke(o))
                    {
                        continue;
                    }

                    entityList.Add(createEntity.Invoke(o));
                    Marshal.FinalReleaseComObject(o);
                }
            }
            finally
            {
                if (enumerator is ICustomAdapter)
                {
                    var adapter = (ICustomAdapter)enumerator;
                    Marshal.ReleaseComObject(adapter.GetUnderlyingObject());
                    Marshal.FinalReleaseComObject(azmanList);
                }
            }

            return entityList;
        }
    }
}