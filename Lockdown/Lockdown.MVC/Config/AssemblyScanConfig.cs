using System.Reflection;

namespace Lockdown.MVC.Config
{
    public class AssemblyScanConfig
    {
        private readonly Assembly _assembly;

        public AssemblyScanConfig(Assembly assembly)
        {
            _assembly = assembly;
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }
    }
}