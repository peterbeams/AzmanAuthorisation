using System.ServiceProcess;

namespace Lockdown.Host
{
    public class WindowsServiceHost : ServiceBase
    {
        private AuthzServiceHost _host;

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            _host = new AuthzServiceHost();
            _host.Start();
        }

        protected override void OnStop()
        {
            _host.Stop();
            base.OnStop();
        }
    }
}