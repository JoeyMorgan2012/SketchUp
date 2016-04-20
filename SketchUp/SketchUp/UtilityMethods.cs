using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public static class UtilityMethods
    {
        #region Logging Utilities

        public static void LogMethodCall(StackFrame[] fullStack, bool includeLineNumbers = false)
        {
            StackFrame[] myFrames = fullStack.Where(f => f.GetFileLineNumber() > 0).ToArray<StackFrame>();
            StringBuilder traceOut = new StringBuilder();
            string callerCallee = string.Empty;
            int j = 0;

            int loopCounter = myFrames.Length - 1 >= 1 ? 1 : myFrames.Length - 1;
            if (myFrames.Length >= 1)
            {
                for (int i = loopCounter; i >= 0; i--)
                {
                    var frame = myFrames[i];
                    j = (i == myFrames.Length - 1 ? i : i + 1);

                    var priorFrame = myFrames[j];

                    callerCallee = string.Format("{0}\t{1}\t{2:mm-dd hh:mm:ss:fff}", priorFrame.GetMethod().Name, frame.GetMethod().Name, DateTime.Now);

                    traceOut.AppendLine(string.Format("{0} {1}", Program.lineNo, callerCallee));
                    Program.lineNo++;
                }
                Trace.Write(traceOut.ToString());
                Trace.Flush();
            }
        }

        //public static void LogMethodCall(StackFrame[] fullStack, bool includeLineNumbers = false)
        //{
        //	StackFrame[] myFrames = fullStack.Where(f => f.GetFileLineNumber() > 0).ToArray<StackFrame>();
        //	StringBuilder traceOut = new StringBuilder();
        //	string callerCallee = string.Empty;
        //	int j = 0;

        //	int loopCounter = myFrames.Length - 1 >= 3 ? 3 : myFrames.Length - 1;
        //	if (myFrames.Length>=1)
        //	{
        //	for (int i = loopCounter; i >= 0; i--)
        //	{
        //		var frame = myFrames[i];
        //		j = (i == myFrames.Length - 1 ? i : i + 1);

        //		var priorFrame = myFrames[j];

        //		callerCallee = string.Format("{0}\t{1}\t{2:hh:mm:ss:fff}", priorFrame.GetMethod().Name,frame.GetMethod().Name, DateTime.Now);

        //		traceOut.AppendLine(string.Format("{0} {1}", Program.lineNo, callerCallee));
        //		Program.lineNo++;

        //	}
        //	Trace.WriteLine(traceOut.ToString());
        //	Trace.Flush();
        //	}
        //}
        //TODO: Remove unneccesary logging code when debugging is done.
        //		public static void LogSqlExecutionAttempt(string methodName, string sqlVariable, string sqlStatement)
        //		{
        //#if DEBUG
        //			StringBuilder sb = new StringBuilder();
        //			sb.AppendLine(string.Format("Attempting to execute {0}.", sqlVariable));
        //			sb.AppendLine(string.Format("Sql: {0}", sqlStatement));
        //			Logger.Info(sb.ToString(), methodName);
        //#endif
        //		}
        //		public static void LogSqlExecutionAttempt(string methodName, string sqlVariable, StringBuilder sqlStatement)
        //		{
        //#if DEBUG
        //			StringBuilder sb = new StringBuilder();
        //			sb.AppendLine(string.Format("Attempting to execute {0}.", sqlVariable));
        //			sb.AppendLine(string.Format("Sql: {0}", sqlStatement.ToString()));
        //			Logger.Info(sb.ToString(), methodName);
        //#endif
        //		}
        //		public static void LogSqlExecutionSuccess(string methodName, string sqlVariable, string sqlStatement)
        //		{
        //#if DEBUG
        //			StringBuilder sb = new StringBuilder();
        //			sb.AppendLine(string.Format("{0} executed successfully.", sqlVariable));
        //			sb.AppendLine(string.Format("Sql: {0}", sqlStatement));
        //			Logger.Info(sb.ToString(), methodName);
        //#endif
        //		}
        //		public static void LogSqlExecutionSuccess(string methodName, string sqlVariable, StringBuilder sqlStatement)
        //		{
        //#if DEBUG
        //			StringBuilder sb = new StringBuilder();
        //			sb.AppendLine(string.Format("{0} executed successfully.", sqlVariable));
        //			sb.AppendLine(string.Format("Sql: {0}", sqlStatement.ToString()));
        //			Logger.Info(sb.ToString(), methodName);
        //#endif
        //		}
        public static void LogInfoVerbose(string currentMethod, string callingMethod, bool showMessage = false)
        {
#if DEBUG
            string messageOut = string.Format("{0} instantiated by {1}", currentMethod, callingMethod);
            Console.WriteLine(messageOut);
            Debug.WriteLine(messageOut);
            Logger.Info(messageOut, currentMethod);
            if (showMessage)
            {
                MessageBox.Show(messageOut);
            }
#endif
        }

        public static void LogInfoVerbose(string currentMethod, string callingMethod, string messageIn, bool showMessage = false)
        {
#if DEBUG

            string messageOut = string.Format("{0} instantiated by {1}. Message: {2}", currentMethod, callingMethod, messageIn);
            Console.WriteLine(messageOut);
            Debug.WriteLine(messageOut);
            Logger.Info(messageOut, currentMethod);
            if (showMessage)
            {
                MessageBox.Show(messageOut);
            }
#endif
        }

        public static void LogInfoVerbose(string messageIn, MethodBase currentMethod, bool showMessage = false)
        {
            string messageOut = string.Format("{0}Message: {2}", currentMethod, messageIn);
            Console.WriteLine(messageOut);
            Debug.WriteLine(messageOut);
            Logger.Info(messageOut, currentMethod.Name);
            if (showMessage)
            {
                MessageBox.Show(messageOut);
            }
        }

        #endregion Logging Utilities

        #region String Utilities

        public static string NextLetter(string lastLetter = "A")
        {
            string nextLetter = string.Empty;
            switch (lastLetter)
            {
                case "A":
                    nextLetter = "B";
                    break;

                case "B":
                    nextLetter = "C";
                    break;

                case "C":
                    nextLetter = "D";
                    break;

                case "D":
                    nextLetter = "F";
                    break;

                case "F":
                    nextLetter = "G";
                    break;

                case "G":
                    nextLetter = "H";
                    break;

                case "H":
                    nextLetter = "I";
                    break;

                case "I":
                    nextLetter = "J";
                    break;

                case "J":
                    nextLetter = "K";
                    break;

                case "K":
                    nextLetter = "L";
                    break;

                case "L":
                    nextLetter = "M";
                    break;

                default:
                    nextLetter = string.Empty;
                    break;
            }
            return nextLetter;
        }

        public static string NextLetter(SWallTech.CAMRA_Connection conn, int record, int card)
        {
            var lastLetter = string.Empty;
            string nextLetter = string.Empty;
            string sql = string.Format("SELECT JSSECT FROM \"NATIVE\".\"{0}SECTION\" WHERE JSRECORD = {1} AND JSDWELL = {2} order by jssect desc fetch first row only", conn.LocalityPrefix, record, card);

            DataSet ds = conn.DBConnection.RunSelectStatement(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                lastLetter = dr["JSSECT"].ToString().Trim().ToUpper();
            }
            if (lastLetter != null)
            {
                switch (lastLetter)
                {
                    case "A":
                        nextLetter = "B";
                        break;

                    case "B":
                        nextLetter = "C";
                        break;

                    case "C":
                        nextLetter = "D";
                        break;

                    case "D":
                        nextLetter = "F";
                        break;

                    case "F":
                        nextLetter = "G";
                        break;

                    case "G":
                        nextLetter = "H";
                        break;

                    case "H":
                        nextLetter = "I";
                        break;

                    case "I":
                        nextLetter = "J";
                        break;

                    case "J":
                        nextLetter = "K";
                        break;

                    case "K":
                        nextLetter = "L";
                        break;

                    case "L":
                        nextLetter = "M";
                        break;

                    default:
                        nextLetter = string.Empty;
                        break;
                }
            }
            else
            {
                nextLetter = "A";
            }
            return nextLetter;
        }
        public static string NextLetter(SMParcel parcel)
        {
            var lastLetter = (from s in parcel.Sections orderby s.SectionLetter descending  select s.SectionLetter).FirstOrDefault();
            string nextLetter = string.Empty;
          
            if (lastLetter != null)
            {
                switch (lastLetter)
                {
                    case "A":
                        nextLetter = "B";
                        break;

                    case "B":
                        nextLetter = "C";
                        break;

                    case "C":
                        nextLetter = "D";
                        break;

                    case "D":
                        nextLetter = "F";
                        break;

                    case "F":
                        nextLetter = "G";
                        break;

                    case "G":
                        nextLetter = "H";
                        break;

                    case "H":
                        nextLetter = "I";
                        break;

                    case "I":
                        nextLetter = "J";
                        break;

                    case "J":
                        nextLetter = "K";
                        break;

                    case "K":
                        nextLetter = "L";
                        break;

                    case "L":
                        nextLetter = "M";
                        break;

                    default:
                        nextLetter = string.Empty;
                        break;
                }
            }
            else
            {
                nextLetter = "A";
            }
            return nextLetter;
        }
        #endregion String Utilities
    }
}