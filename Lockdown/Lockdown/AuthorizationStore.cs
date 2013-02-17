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
        //http://msdn.microsoft.com/en-us/library/windows/desktop/ms681382(v=vs.85).aspx
        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_SUCCESS = 0;

        private IAzApplication3 _application;
        private IList<Operation> _operations;
        private IList<Task> _tasks;
        private IAzAuthorizationStore3 _store;

        public AuthorizationStore(string connectionString)
        {
            _store = (IAzAuthorizationStore3)new AzAuthorizationStore();
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

            var members = GetRoleAssignments(role, "default");
            
            return new Role
                       {
                           Name = role.Name,
                           Operations = operations,
                           Tasks = tasks,
                           Members = members
                       };
        }

        private Member[] GetRoleAssignments(IAzTask2 role, string scopeName)
        {
            var members = new List<Member>();
            var list = role.RoleAssignments(scopeName, true);
            var enumerator = list.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    var o = (IAzRoleAssignment)enumerator.Current;

                    foreach (var m in o.Members)
                    {
                        members.Add(new Member { Id = m });
                    }

                    Marshal.FinalReleaseComObject(o);
                }
            }
            finally
            {
                if (enumerator is ICustomAdapter)
                {
                    var adapter = (ICustomAdapter)enumerator;
                    Marshal.ReleaseComObject(adapter.GetUnderlyingObject());
                    Marshal.FinalReleaseComObject(list);
                }
            }

            return members.ToArray();
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
                _application = (IAzApplication3)_store.OpenApplication(appName, null);

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

        public string[] GetAuthroizedOperations(string[] sids)
        {
            var context = _application.InitializeClientContext2("identity");
            var sidArray = sids.Cast<object>().ToArray();
            context.AddStringSids(sidArray);

            var opIds = Operations.Select(o => (object)o.Id).ToArray();
            var scopes = new object[] { "default" };

            var result = (object[])context.AccessCheck("Authz", scopes, opIds, null, null, null, null, null);

            var ops = new List<string>();

            //results will be in same order as opIds array
            for (var i = 0; i < opIds.Length; i++)
            {
                if ((int)result[i] == ERROR_SUCCESS)
                {
                    ops.Add(Operations.Single(o => o.Id == (int)opIds[i]).Name);
                }

                //todo: log out return value
            }

            return ops.ToArray();
        }
    }
}