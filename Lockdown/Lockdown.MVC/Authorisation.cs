using System;
using Lockdown.MVC.Config;

namespace Lockdown.MVC
{
    public class Authorisation
    {
        public static IConfigApp Configure { get { return new ConfigureFluent(); } }
    }
}
