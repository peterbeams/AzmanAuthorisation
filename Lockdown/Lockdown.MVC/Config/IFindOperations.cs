namespace Lockdown.MVC.Config
{
    public interface IFindOperations
    {
        ITokenFactoryConfig ScanControllers(AssemblyScanConfig scanning);
    }
}