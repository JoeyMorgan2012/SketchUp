using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SWallTech;

namespace SketchUpReboot
{
    /// <summary>
    /// CommandLineArgumentBase: A class for handling command line arguments
    /// to an application.
    ///
    /// This class can be used as is, but it is recommended that you
    /// inherit a subclass to give the arguments proper variable names
    /// and strong types.
    ///
    /// Default behavior parses string[] of args in the following format:
    ///   /argname:argvalue /argname:argvalue /argname:argvalue ...
    ///
    /// This behavior can be overridden using the
    /// CommandLineArgumentsBase(string[] args, string pattern) constructor.
    /// </summary>
    public class CommandLineArgumentsBase
    {
        public static readonly string DefaultPattern = @"(?<argname>/\w+):(?<argvalue>\S+)";

        public CommandLineArgumentsBase()
        {
            _argumentList = new Dictionary<string, string>();
        }

        public CommandLineArgumentsBase(string[] args)
            : this(args, DefaultPattern)
        {
        }

        public CommandLineArgumentsBase(string[] args, string pattern)
            : this()
        {
            _args = args;
            foreach (string arg in args.Where(arg => arg.Contains("exe") == false))
            {
                Match match = Regex.Match(arg, pattern);

                // If match not found, command line args are improperly formed.
                if (!match.Success)
                {
                    string message = string.Format("Argument {0} did not match.", arg);
                    Logger.Warning(message, MethodBase.GetCurrentMethod().Name);
                    message = "The command line arguments are improperly formed. Default format is /-argname:argvalue and arguments must be separated by commas with no spaces between.";
                    MessageBox.Show(message);
                    throw new ArgumentException(message);
                }
                else
                {
                    // Store command line arg and value
                    string argname = match.Groups["argname"].Value.Substring(1);
                    string argvalue = match.Groups["argvalue"].Value;
                    _argumentList.Add(argname, argvalue);
                }
            }
        }

        private readonly string[] _args;
        private readonly Dictionary<string, string> _argumentList = null;

        public string[] Args
        {
            get
            {
                return _args;
            }
        }

        public bool HasArgument(string argName)
        {
            return _argumentList.ContainsKey(argName);
        }

        public string GetArgument(string argName)
        {
            if (!HasArgument(argName))
                throw new KeyNotFoundException(String.Format("Key {0} not found."));

            return _argumentList[argName];
        }

        public bool HasCommandLineArgs
        {
            get
            {
                if (Args == null)
                    return false;

                return Args.Length > 0;
            }
        }
    }
}