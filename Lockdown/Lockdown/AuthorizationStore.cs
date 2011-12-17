using System;
using System.Collections.Generic;

namespace Lockdown
{
    public class Operation
    {
        public string Name { get; set; }
        public int Id { get; set; }
    }

    public class AuthorizationStore
    {
        public AuthorizationStore(string connectionString)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Operation> GetOperations()
        {
            return null;
        }
    }
}