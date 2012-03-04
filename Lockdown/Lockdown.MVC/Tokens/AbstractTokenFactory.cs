using Lockdown.Messages.Data;

namespace Lockdown.MVC.Tokens
{
    public abstract class AbstractTokenFactory : ITokenFactory
    {
        public abstract UserToken GetCurrent();

        public void Clear()
        {
            OperationStore.Clear();
        }
    }
}