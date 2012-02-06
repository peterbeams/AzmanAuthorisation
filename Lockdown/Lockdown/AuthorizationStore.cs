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
        private IEnumerable<Operation> _operations;
        private IEnumerable<Task> _tasks;

        public AuthorizationStore(string connectionString)
        {
            var store = new AzAuthorizationStore();
            store.Initialize(0, connectionString, null);
            _application = store.OpenApplication("MyApp", null);

            _operations = GetOperations();
            _tasks = GetTasks();
        }

        public IEnumerable<Operation> Operations
        {
            get { return _operations; }
        }

        public IEnumerable<Task> Tasks
        {
            get { return _tasks; }
        }

        private IEnumerable<Operation> GetOperations()
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

        private Role CreateRole(IAzTask2 role)
        {
            var opNames = new List<string>();
            foreach (var s in role.Operations)
            {
                opNames.Add(s);
            }

            var operations = Operations.Where(op => opNames.Any(opName => opName == op.Name));

            var taskNames = new List<string>();
            foreach (var s in role.Tasks)
            {
                taskNames.Add(s);
            }

            var tasks = Tasks.Where(t => taskNames.Any(taskName => taskName == t.Name));

            return new Role
                       {
                           Name = role.Name,
                           Operations = operations,
                           Tasks = tasks
                       };
        }

        private IEnumerable<Task> GetTasks()
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

        public static void Create(string connectionString)
        {
            var store = new AzAuthorizationStore();
            store.Initialize(1, connectionString, null);

            store.Submit();
            
            var app = store.CreateApplication("MyApp");
            app.Submit();
        }
    }
}