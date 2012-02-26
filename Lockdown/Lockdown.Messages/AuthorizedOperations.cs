using System.Runtime.Serialization;

namespace Lockdown.Messages.Commands
{
    [DataContract]
    public class AuthorizedOperations
    {
        public string[] OperationNames { get; set; }
    }
}