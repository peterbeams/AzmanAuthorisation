using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace Lockdown.Host
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller1;
        private ServiceInstaller serviceInstaller1;

        public ProjectInstaller(string serviceName)
        {
            InitializeComponent(serviceName);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent(string serviceName)
        {
            serviceProcessInstaller1 = new ServiceProcessInstaller();
            serviceInstaller1 = new ServiceInstaller();
            
            serviceProcessInstaller1.Password = null;
            serviceProcessInstaller1.Username = null;
            
            serviceInstaller1.ServiceName = serviceName;
            
            Installers.AddRange(new Installer[] { serviceProcessInstaller1, serviceInstaller1 });
        } 
    }
}
