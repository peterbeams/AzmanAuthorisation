namespace Lockdown.Configuration.Operations
{
    public class RemoveControllerFromEndOfTypeNameModifier : IModifyOperationName
    {
        public string Apply(string name)
        {
            return name.Replace("Controller.", ".");
        }
    }
}