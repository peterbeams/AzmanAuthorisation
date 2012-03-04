using System.ServiceModel;

namespace Lockdown.Messages.Commands
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