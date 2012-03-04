using System.Runtime.Serialization;

namespace Lockdown.Messages.Commands
{
    [DataContract]
    public class UserToken
    {
        [DataMember]
        public string[] Sids { get; set; }
    }
}