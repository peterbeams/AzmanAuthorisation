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
        private Func<MethodInfo, bool> _actionDefinition;
        
        public AssemblyScanConfig(Assembly assembly, string stripPrefix)
            : this(assembly, stripPrefix, false)
        {
        }

        public AssemblyScanConfig(Assembly assembly, string stripPrefix, bool stripControllerSuffix)
        {
            _assembly = assembly;
            _stripPrefix = stripPrefix;
            _stripControllerSuffix = stripControllerSuffix;
            _actionDefinition = (m) => typeof(ActionResult).IsAssignableFrom(m.ReturnType);
        }

        public AssemblyScanConfig DefiningActionsAs(Func<MethodInfo, bool> rule)
        {
            _actionDefinition = rule;
            return this;
        }

        internal Func<MethodInfo, bool> ActionsDefinedAs
        {
            get { return _actionDefinition; }
        }

        internal string StripPrefix
        {
            get { return _stripPrefix; }
        }

        internal Assembly Assembly
        {
            get { return _assembly; }
        }

        internal bool StripControllerSuffix
        {
            get { return _stripControllerSuffix; }
        }
    }
}