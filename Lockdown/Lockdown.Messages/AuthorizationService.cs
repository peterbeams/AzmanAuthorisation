using System.ServiceModel;
using Lockdown.Messages.Data;

namespace Lockdown.Messages
{
    [ServiceContract]
    public interface AuthorizationService
    {
        [OperationContract]
        AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token);

        [OperationContract]
        void RegisterOperations(string appName, string[] operationNames);
    }
}