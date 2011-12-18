using System;
using System.Collections.Generic;

namespace Lockdown
{
    public class Role
    {
        public string Name { get; set; }
        public IEnumerable<Operation> Operations { get; set; }
    }
}