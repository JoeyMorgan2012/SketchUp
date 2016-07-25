using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    internal static class Program
    {
        #region Fields

        public static string StartupErrorMessage { get; private set; }

        public static string[] args;
        public static CommandLineArguments commandLineArgs;
        public static int lineNo = 0;
        public static int progress = 0;

        #endregion Fields

        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        // [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CreateTraceLogs();
#if DEBUG||TEST
            Trace.WriteLine($"Logging started {DateTime.Now}.");
            Trace.WriteLine($"User: {Environment.UserName}");
            Trace.WriteLine("-----------------------------------------");
#endif

            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    FormatNetworkDeployedArguments();
                }
                else
                {
                    args = Environment.GetCommandLineArgs();
                }

                InitializeCommandLineArgsClass();

                var controller = new SingleInstanceController();
                controller.Run(args);
            }
            catch (Exception ex)
            {
                string message = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Console.WriteLine(message);
#if DEBUG||TEST
                MessageBox.Show(ex.Message);
#endif
            }
        }

        private static void CreateTraceLogs()
        {
            // Remove the original default trace listener.
            Trace.Listeners.RemoveAt(0);

            // Create and add a new default trace listener.
            DefaultTraceListener defaultListener;
            defaultListener = new DefaultTraceListener();
            Trace.Listeners.Add(defaultListener);
            string logFile = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\SketchUp Trace_{DateTime.Now.Month}-{DateTime.Now.Day}.log";
            //string traceFile = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\SketchUp Trace_{DateTime.Now.Month}-{DateTime.Now.Day}.txt";
            if (File.Exists(logFile))
            {
                File.Delete(logFile);
            }
            //if (File.Exists(traceFile))
            //{
            //    File.Delete(traceFile);
            //}
            //var traceOut = new TextWriterTraceListener(traceFile);
            //Trace.Listeners.Add(traceOut);
            defaultListener.LogFileName = logFile;
            var consoleListener = new ConsoleTraceListener();
            Trace.Listeners.Add(consoleListener);
            Trace.IndentLevel = 1;
            Trace.IndentSize = 2;
        }

        #endregion Methods

        #region Main Program Methods

        public static string[] GetArguments()
        {
            var commandLineArgs = new List<string>();
            string startupUrl = string.Empty;

            if (ApplicationDeployment.IsNetworkDeployed &&
                ApplicationDeployment.CurrentDeployment.ActivationUri != null)
            {
                startupUrl = FormatClickOnceCommandLines(commandLineArgs);
            }
            else
            {
                commandLineArgs = Environment.GetCommandLineArgs().ToList();
            }

            // Also tack on any activation args at the back
            AddActivationArgsIfPresent(commandLineArgs, startupUrl);

            return commandLineArgs.ToArray();
        }

        private static void AddActivationArgsIfPresent(List<string> commandLineArgs, string startupUrl)
        {
            var activationArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments;
            if (activationArgs != null && activationArgs.ActivationData.Length > 0)
            {
                commandLineArgs.AddRange(activationArgs.ActivationData.Where(d => d != startupUrl).Select((s, i) => string.Format("-in{1}:\"{0}\"", s, i == 0 ? string.Empty : i.ToString())));
            }
        }

        private static string FormatClickOnceCommandLines(List<string> commandLineArgs)
        {
            string startupUrl;

            // Add the EXE name at the front
            commandLineArgs.Add(Environment.GetCommandLineArgs()[0]);

            // Get the query portion of the URI, also decode out any escaped sequences
            startupUrl = ApplicationDeployment.CurrentDeployment.ActivationUri.ToString();
            var query = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
            if (!string.IsNullOrEmpty(query) && query.StartsWith("?", StringComparison.CurrentCulture))
            {
                // Split by the ampersands, a append a "-" for use with splitting functions
                string[] arguments = query.Substring(1).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(a => string.Format("-{0}", HttpUtility.UrlDecode(a))).ToArray();

                // Now add the parsed argument components
                commandLineArgs.AddRange(arguments);
            }

            return startupUrl;
        }

        private static void FormatNetworkDeployedArguments()
        {
            if (args == null || args.Length == 0)
            {
                string[] inputArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;

                if (inputArgs != null && inputArgs.Length > 0)
                {
                    var splitChar = new char();
                    splitChar = Convert.ToChar(",");
                    args = inputArgs[0].Split(splitChar);
                }
            }
            else
            {
                string[] inputArgs = Program.args;
            }
        }

        private static void InitializeCommandLineArgsClass()
        {
            //Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
            ;

            if (args == null || args.Length < 2)
            {
                commandLineArgs = new CommandLineArguments();
            }
            else
            {
                try
                {
                    commandLineArgs = new CommandLineArguments(args, @"(?<argname>-\w+):(?<argvalue>\S+)");
                }
                catch (ArgumentException aex)
                {
                    commandLineArgs = new CommandLineArguments();

                    MessageBox.Show(aex.Message);
                }
            }

            //Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
        }

        #endregion Main Program Methods

        #region Utility methods

        private static void ParseArgs(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                commandLineArgs = new CommandLineArguments();
            }
            else
            {
                try
                {
                    commandLineArgs = new CommandLineArguments(args, @"(?<argname>-\w+):(?<argvalue>\S+)");
                }
                catch (ArgumentException aex)
                {
                    StartupErrorMessage = "Invalid parameters passed to application.Message: " + aex.Message;
                    commandLineArgs = new CommandLineArguments();
                }
            }
        }

        #endregion Utility methods
    }
}