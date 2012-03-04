namespace Lockdown.MVC.Config
{
    public interface IConfigApp
    {
        IFindOperations Application(string name);
    }
}