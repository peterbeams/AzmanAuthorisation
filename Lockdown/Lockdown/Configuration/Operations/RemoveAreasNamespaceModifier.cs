namespace Lockdown.Configuration.Operations
{
    public class RemoveAreasNamespaceModifier : IModifyOperationName
    {
        public string Apply(string name)
        {
            return name.Replace(".Areas.", ".");
        }
    }
}