namespace Lockdown.MVC.Config
{
    public class In
    {
        public static AssemblyScanConfig AssemblyContaining<T>(string stripPrefix, bool stripControllerSuffix)
        {
            return new AssemblyScanConfig(typeof(T).Assembly, stripPrefix, stripControllerSuffix);
        }

        public static AssemblyScanConfig AssemblyContaining<T>(string stripPrefix)
        {
            return new AssemblyScanConfig(typeof(T).Assembly, stripPrefix);
        }

        public static AssemblyScanConfig AssemblyContaining<T>()
        {
            return new AssemblyScanConfig(typeof(T).Assembly, string.Empty);
        }
    }
}