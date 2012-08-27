namespace Lockdown.Configuration.Operations
{
    public class RemoveControllersNamespaceModifier : IModifyOperationName
    {
        public string Apply(string name)
        {
            return name.Replace(".Controllers.", ".");
        }
    }
}