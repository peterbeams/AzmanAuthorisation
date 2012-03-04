using System.Runtime.Serialization;

namespace Lockdown.Messages.Data
{
    [DataContract]
    public class UserToken
    {
        [DataMember]
        public string[] Sids { get; set; }
    }
}