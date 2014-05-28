using System;
using Lockdown.Messages;
using Lockdown.Messages.Data;

namespace Lockdown.MVC.Client
{
    public class DebugClient : AuthorizationService
    {
        private readonly string[] _roles;
        private static string[] _operationNames;

        public DebugClient(string[] roles)
        {
            _roles = roles;
        }

        public AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token)
        {
            return new AuthorizedOperations { OperationNames = _operationNames };
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            _operationNames = operationNames;
        }

        public string[] GetRoles(string appName, UserToken token)
        {
            return _roles;
        }
    }
}