using System.ServiceModel;

namespace Lockdown.Messages.Commands
{
    [ServiceContract]
    public interface AuthorizationService
    {
        [OperationContract]
        AuthorizedOperations GetAuthorisedOperations(string appName);
        [OperationContract]
        void RegisterOperations(string appName, string[] operationNames);
    }
}