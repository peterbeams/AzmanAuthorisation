using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;
using AZROLESLib;

namespace Lockdown
{
    public class AuthorizationStore
    {
        private IAzApplication _application;
        private IList<Operation> _operations;
        private IList<Task> _tasks;
        private AzAuthorizationStore _store;

        public AuthorizationStore(string connectionString)
        {
            _store = new AzAuthorizationStore();
            _store.Initialize(0, connectionString, null);
        }

        public IEnumerable<Operation> Operations
        {
            get { return _operations; }
        }

        public IEnumerable<Task> Tasks
        {
            get { return _tasks; }
        }

        private IList<Operation> GetOperations()
        {
            return GetEntityListFromAzmanEnumerator<IAzOperation2, Operation>(() => _application.Operations, o => true, o => new Operation
                                                              {
                                                                  Name = o.Name,
                                                                  Id = o.OperationID
                                                              }).ToList();
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

        private IList<Task> GetTasks()
        {
            return GetEntityListFromAzmanEnumerator<IAzTask2, Task>(() => _application.Tasks, o => o.IsRoleDefinition != 1, o => new Task
                                                                                                    {
                                                                                                        Name = o.Name
                                                                                                    }).ToList();
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

        public void UsingApplication(string appName)
        {
            if (_application == null)
            {
                _application = _store.OpenApplication(appName, null);

                _operations = GetOperations();
                _tasks = GetTasks();
            }
            else
            {
                if (!_application.Name.Equals(appName))
                {
                    throw new Exception("Store is already using another applications");
                }
            }
        }

        public void EnsureOperationByName(string operationName)
        {
            if (!Operations.Any(o => o.Name.Equals(operationName, StringComparison.InvariantCultureIgnoreCase)))
            {
                AddOperation(operationName, NextOperationId());
            }
        }

        private int NextOperationId()
        {
            if (_operations.Count == 0)
            {
                return 1;
            }

            return _operations.Max(o => o.Id) + 1;
        }

        private void AddOperation(string operationName, int id)
        {
            var op = _application.CreateOperation(operationName);
            op.OperationID = id;
            op.Submit();

            _operations.Add(new Operation { Id = id, Name = operationName });
        }

        public string[] GetAuthroizedOperations(WindowsIdentity windowsIdentity)
        {
            var context = _application.InitializeClientContextFromToken((ulong)windowsIdentity.Token);
            
            var opIds = Operations.Select(o => o.Id).ToArray();

            var result = (object[])context.AccessCheck(string.Empty, "default", opIds);

            foreach (var r in result)
            {
                Console.WriteLine(r);
            }

            return null;
        }
    }
}