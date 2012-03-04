using System;
using System.ServiceProcess;
using System.Threading;
using log4net;
using log4net.Appender;
using log4net.Config;

namespace Lockdown.Host
{
    public class Program
    {
        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof (Program)); }
        }

        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (args[0].Equals("/install", StringComparison.InvariantCultureIgnoreCase))
                {
                    Console.WriteLine("Running Service Installer");

                    
                }

                if (args[0].Equals("/service", StringComparison.InvariantCultureIgnoreCase))
                {
                    var services = new ServiceBase[] { new WindowsServiceHost() };
                    ServiceBase.Run(services);
                    return;
                }
            }

            LoggingConfig.ConfigureConsole();

            Log.Info("Running Lockdown Host.");
            Log.Info("To install host as windows service run with /install argument.");

            var host = new AuthzServiceHost();
            host.Start();

            Log.Info("Host Started.");

            do
            {
                Thread.Sleep(60000);
            } while (true);
        }
    }
}