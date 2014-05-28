using System;
using System.Reflection;
using System.Web.Mvc;

namespace Lockdown.MVC.Config
{
    public class AssemblyScanConfig
    {
        private readonly Assembly _assembly;
        private readonly string _stripPrefix;
        private readonly bool _stripControllerSuffix;
        private Func<MethodInfo, bool> _rule;


        public AssemblyScanConfig(Assembly assembly, string stripPrefix)
            : this(assembly, stripPrefix, false)
        {
        }

        public AssemblyScanConfig(Assembly assembly, string stripPrefix, bool stripControllerSuffix)
        {
            _assembly = assembly;
            _stripPrefix = stripPrefix;
            _stripControllerSuffix = stripControllerSuffix;
            _rule = (m) => typeof (ActionResult).IsAssignableFrom(m.ReturnType);
        }

        public AssemblyScanConfig Using(Func<MethodInfo, bool> rule)
        {
            _rule = rule;
            return this;
        }

        public Func<MethodInfo, bool> MethodRequiresFiltering
        {
            get { return _rule; }
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