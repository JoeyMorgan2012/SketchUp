using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	internal static class Program
	{
		#region Fields

		public static string[] args;
		public static CommandLineArguments commandLineArgs;
		public static int progress = 0;

		public static int lineNo = 0;

		public static string StartupErrorMessage
		{
			get; private set;
		}

		#endregion Fields

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//  [STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(false).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif

			try
			{
#if DEBUG

				//Debugging Code -- remove for production release
				fullStack = new System.Diagnostics.StackTrace(false).GetFrames();
				UtilityMethods.LogMethodCall(fullStack, true);
#endif

				if (ApplicationDeployment.IsNetworkDeployed)
				{
					FormatNetworkDeployedArguments();
				}
				else
				{
					args = Environment.GetCommandLineArgs();
				}

				InitializeCommandLineArgsClass();

				SingleInstanceController controller = new SingleInstanceController();
				controller.Run(args);
			}
			catch (Exception ex)
			{
#if DEBUG

				MessageBox.Show(ex.Message);
#endif
				Logger.Error(ex, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name));
				throw;
			}
		}

		#region Main Program Methods

		private static void InitializeCommandLineArgsClass()
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
					commandLineArgs = new CommandLineArguments();

					MessageBox.Show(aex.Message);
				}
			}
		}

		private static void FormatNetworkDeployedArguments()
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (args == null || args.Length == 0)
			{
				string[] inputArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;

				if (inputArgs != null && inputArgs.Length > 0)
				{
					char splitChar = new char();
					splitChar = Convert.ToChar(",");
					args = inputArgs[0].Split(splitChar);
				}
			}
			else
			{
				string[] inputArgs = Program.args;
			}
		}

		public static string[] GetArguments()
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			var commandLineArgs = new List<string>();
			string startupUrl = String.Empty;

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
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			var activationArgs = AppDomain.CurrentDomain.SetupInformation.ActivationArguments;
			if (activationArgs != null && activationArgs.ActivationData.Length > 0)
			{
				commandLineArgs.AddRange(activationArgs.ActivationData.Where(d => d != startupUrl).Select((s, i) => String.Format("-in{1}:\"{0}\"", s, i == 0 ? String.Empty : i.ToString())));
			}
		}

		private static string FormatClickOnceCommandLines(List<string> commandLineArgs)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			string startupUrl;

			// Add the EXE name at the front
			commandLineArgs.Add(Environment.GetCommandLineArgs()[0]);

			// Get the query portion of the URI, also decode out any escaped sequences
			startupUrl = ApplicationDeployment.CurrentDeployment.ActivationUri.ToString();
			var query = ApplicationDeployment.CurrentDeployment.ActivationUri.Query;
			if (!string.IsNullOrEmpty(query) && query.StartsWith("?", StringComparison.CurrentCulture))
			{
				// Split by the ampersands, a append a "-" for use with splitting functions
				string[] arguments = query.Substring(1).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries).Select(a => String.Format("-{0}", HttpUtility.UrlDecode(a))).ToArray();

				// Now add the parsed argument components
				commandLineArgs.AddRange(arguments);
			}

			return startupUrl;
		}

		#endregion Main Program Methods

		#region Utility methods

		private static void ParseArgs(string[] args)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
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