using System;

namespace Lockdown.Configuration.Operations
{
    public class AzmanConstraintViolation : ApplicationException
    {
        public AzmanConstraintViolation(string message)
            : base(message)
        {
        }
    }
}