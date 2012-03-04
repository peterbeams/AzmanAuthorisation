using System.Security;

namespace Lockdown.MVC.Exceptions
{
    public class ControllerActionIsNotAuthorizedException : SecurityException
    {
        public ControllerActionIsNotAuthorizedException(string operationName)
            : base(string.Format("Operation '{0}' is not authorized for current user.", operationName))
        {
        }
    }
}