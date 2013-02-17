using System.Reflection;

namespace Lockdown.MVC.Config
{
    public class AssemblyScanConfig
    {
        private readonly Assembly _assembly;
        private readonly string _stripPrefix;
        private readonly bool _stripControllerSuffix;

        public AssemblyScanConfig(Assembly assembly, string stripPrefix)
            : this(assembly, stripPrefix, false)
        {
        }

        public AssemblyScanConfig(Assembly assembly, string stripPrefix, bool stripControllerSuffix)
        {
            _assembly = assembly;
            _stripPrefix = stripPrefix;
            _stripControllerSuffix = stripControllerSuffix;
        }

        public string StripPrefix
        {
            get { return _stripPrefix; }
        }

        public Assembly Assembly
        {
            get { return _assembly; }
        }

        public bool StripControllerSuffix
        {
            get { return _stripControllerSuffix; }
        }
    }
}