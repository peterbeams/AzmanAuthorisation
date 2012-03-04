using System.Runtime.Serialization;

namespace Lockdown.Messages.Data
{
    [DataContract]
    public class AuthorizedOperations
    {
        [DataMember]
        public string[] OperationNames { get; set; }
    }
}