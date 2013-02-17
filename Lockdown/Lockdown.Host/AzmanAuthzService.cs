using System;
using System.Linq;
using System.ServiceModel;
using Lockdown.Messages;
using Lockdown.Messages.Data;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace Lockdown.Host
{
    /// <summary>
    /// Handles logging configuration for the lite profile.
    /// </summary>
    public class LoggingConfig
    {
        public static void ConfigureConsole()
        {
            var appender = new ColoredConsoleAppender
                               {
                                   Threshold = Level.Info,
                                   Layout = new PatternLayout("%timestamp [%thread] %level %logger %ndc - %message%newline"),
                                   Name = "Default",
                                   Target = ColoredConsoleAppender.ConsoleOut
                               };
            ConfigColours(appender);
            appender.ActivateOptions();
            BasicConfigurator.Configure(appender);
        }

        public static void ConfigColours(ColoredConsoleAppender a)
        {
            a.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Debug,
                    ForeColor = ColoredConsoleAppender.Colors.White
                });
            a.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Info,
                    ForeColor = ColoredConsoleAppender.Colors.Green
                });
            a.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Warn,
                    ForeColor = ColoredConsoleAppender.Colors.Yellow | ColoredConsoleAppender.Colors.HighIntensity
                });
            a.AddMapping(
                new ColoredConsoleAppender.LevelColors
                {
                    Level = Level.Error,
                    ForeColor = ColoredConsoleAppender.Colors.Red | ColoredConsoleAppender.Colors.HighIntensity
                });
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class AzmanAuthzService : AuthorizationService
    {
        private AuthorizationStore Store { get { return AuthzServiceHost.Store; } }

        private ILog Log
        {
            get { return LogManager.GetLogger(typeof (AzmanAuthzService)); }
        }

        public AuthorizedOperations GetAuthorisedOperations(string appName, UserToken token)
        {
            Log.Info("Call made to GetAuthorisedOperations");

            try
            {
                Store.UsingApplication(appName);

                var opNames = Store.GetAuthroizedOperations(token.Sids);
                return new AuthorizedOperations { OperationNames = opNames };
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public void RegisterOperations(string appName, string[] operationNames)
        {
            Log.Info("Call made to RegisterOperations");

            try
            {
                Store.UsingApplication(appName);
                Array.ForEach(operationNames, o => Store.EnsureOperationByName(o));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }

        public string[] GetRoles(string appName, UserToken token)
        {
            Log.Info("Call made to GetRoles");

            try
            {
                Store.UsingApplication(appName);

                var opNames = Store.GetRoles().Where(r => r.Members.Any(m => token.Sids.Contains(m.Id)) );
                return opNames.Select(r => r.Name).ToArray();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}