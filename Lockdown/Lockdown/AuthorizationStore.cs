using System;
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
            var azOps = _application.Operations;
            var enumerator = azOps.GetEnumerator();
            var ops = new List<Operation>();

            try
            {
                while (enumerator.MoveNext())
                {
                    var o = (IAzOperation2)enumerator.Current;
                    ops.Add(new Operation
                    {
                        Name = o.Name,
                        Id = o.OperationID
                    });

                    Marshal.FinalReleaseComObject(o);
                }
            }
            finally
            {
                var adapter = (ICustomAdapter)enumerator;
                Marshal.ReleaseComObject(adapter.GetUnderlyingObject());
                Marshal.FinalReleaseComObject(azOps);
            }
            
            return ops;
        }

        public IEnumerable<Role> GetRoles()
        {
            var azRoles = _application.Tasks;
            var enumerator = azRoles.GetEnumerator();
            var roles = new List<Role>();

            try
            {
                while (enumerator.MoveNext())
                {
                    var o = (IAzTask2)enumerator.Current;
                    roles.Add(new Role
                    {
                        Name = o.Name
                    });

                    Marshal.FinalReleaseComObject(o);
                }
            }
            finally
            {
                var adapter = (ICustomAdapter)enumerator;
                Marshal.ReleaseComObject(adapter.GetUnderlyingObject());
                Marshal.FinalReleaseComObject(azRoles);
            }

            return roles;
        }

        public IEnumerable<Task> GetTasks()
        {
            return null;
        }
    }
}