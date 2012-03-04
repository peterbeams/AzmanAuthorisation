namespace Lockdown.MVC.Config
{
    public class In
    {
        public static AssemblyScanConfig AssemlbyContaining<T>()
        {
            return new AssemblyScanConfig(typeof(T).Assembly);
        }
    }
}