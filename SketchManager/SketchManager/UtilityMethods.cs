using System;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace SWallTech
{
    public static class UtilityMethods
    {
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

        #endregion String Utilities
    }
}