using System.Runtime.Serialization;

namespace Lockdown.Messages.Commands
{
    [DataContract]
    public class AuthorizedOperations
    {
        [DataMember]
        public string[] OperationNames { get; set; }
    }
}