using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Lockdown.MVC.Config
{
    public interface IAssemblyScanConfig
    {
        IAssemblyScanConfig DefiningActionsAs(Func<MethodInfo, bool> rule);
    }

}
