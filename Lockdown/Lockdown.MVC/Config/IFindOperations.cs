namespace Lockdown.MVC.Config
{
    public interface IFindOperations
    {
        IClientConfig ScanControllers(AssemblyScanConfig scanning);
    }
}