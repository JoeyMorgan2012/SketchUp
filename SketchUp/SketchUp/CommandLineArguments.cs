using System;
using System.Reflection;
using System.Text;

namespace SketchUp
{
    public class CommandLineArguments : CommandLineArgumentsBase
    {
        public CommandLineArguments() : base()
        {
        }

        public string CommandLineArgumentsToString()
        {
            string propertyName = string.Empty;
            string propertyValue = string.Empty;
            StringBuilder sb = new StringBuilder();
            propertyName = "Library";
            propertyValue = Library;
            sb.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));

            propertyName = "Locality";
            propertyValue = Locality;
            sb.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));

            propertyName = "Record";
            propertyValue = Record.ToString();
            sb.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));

            propertyName = "Card";
            propertyValue = Card.ToString();
            sb.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));

            propertyName = "IPAddress";
            propertyValue = IPAddress;
            sb.AppendLine(string.Format("{0}: {1}", propertyName, propertyValue));

            string commandLine = commandLine = sb.ToString();
            return commandLine;
        }

        public CommandLineArguments(string[] args, string pattern) : base(args, pattern)
        {
            ProcessArgs();
        }

        public CommandLineArguments(string[] args)
            : base(args)
        {
            ProcessArgs();
        }

        private void ProcessArgs()
        {
            if (HasArgument("Library"))
            {
                Library = GetArgument("Library").Trim();
            }

            if (HasArgument("Locality"))
            {
                Locality = GetArgument("Locality").Trim();
            }

            if (HasArgument("Record"))
            {
                int record = 0;
                int.TryParse(GetArgument("Record"), out record);

                Record = record;
            }

            if (HasArgument("Card"))
            {
                int card;
                int.TryParse(GetArgument("Card"), out card);

                Card = card;
            }

            if (HasArgument("IPAddress"))
            {
                IPAddress = GetArgument("IPAddress").Trim();
            }
        }

        public string Locality
        {
            get; set;
        }

        public string Library
        {
            get; set;
        }

        public int Record
        {
            get; set;
        }

        public int Card
        {
            get; set;
        }

        public string IPAddress
        {
            get;
            set;
        }
    }
}