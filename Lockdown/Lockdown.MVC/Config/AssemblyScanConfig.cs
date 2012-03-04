using System.Reflection;

namespace Lockdown.MVC.Config
{
    public class AssemblyScanConfig
    {
        private readonly Assembly _assembly;
        private readonly string _stripPrefix;

        public AssemblyScanConfig(Assembly assembly, string stripPrefix)
        {
            _assembly = assembly;
            _stripPrefix = stripPrefix;
        }

        public string StripPrefix
        {
            get { return _stripPrefix; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }
    }
}