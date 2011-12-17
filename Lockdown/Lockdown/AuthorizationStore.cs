using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AZROLESLib;

namespace Lockdown
{
    public class Operation
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

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
    }
}