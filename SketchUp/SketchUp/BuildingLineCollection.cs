using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SWallTech;

namespace SketchUp
{
    //Refactored stringbuilders to strings and extracted long code runs into separate methods. JMM Feb 2016
    public class BuildingLineCollection : List<BuildingLine>
    {
        public BuildingLineCollection(int record, int card, string sectionLetter)
            : this()
        {
            Record = record;
            Card = card;
            SectionLetter = sectionLetter;
        }

        private BuildingLineCollection()
        {
        }

        public int Card
        {
            get; set;
        }

        public int Record
        {
            get; set;
        }

        public string SectionLetter
        {
            get; set;
        }

        public static BuildingLineCollection GetLines(SWallTech.CAMRA_Connection fox, int Record, int Dwell)
        {
            BuildingLineCollection sectionLines = new BuildingLineCollection();

            string blsql = string.Format("select jlsect, jlline# as jlline, jldirect, jlxlen, jlylen, jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} order by jlsect, jlline ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, Record, Dwell);
            DataSet ds = fox.DBConnection.RunSelectStatement(blsql);

            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow blreader in ds.Tables[0].Rows)
                {
                    sectionLines.Add(new BuildingLine()
                    {
                        Record = Record,
                        Card = Dwell,
                        SectionLetter = blreader["jlsect"].ToString().Trim(),
                        LineNumber = Convert.ToInt32(blreader["jlline"].ToString()),
                        Directional = blreader["jldirect"].ToString().Trim(),
                        XLength = Convert.ToDecimal(blreader["jlxlen"].ToString()),
                        YLength = Convert.ToDecimal(blreader["jlylen"].ToString()),
                        Attachment = blreader["jlattach"].ToString().Trim()
                    });
                }
            }

            return sectionLines;
        }

        public Dictionary<string, List<BuildingLine>> GetAllBuildingSections()
        {
            Dictionary<string, List<BuildingLine>> allLines = new Dictionary<string, List<BuildingLine>>();
            foreach (string letter in GetSectionLetters())
            {
                allLines.Add(letter, GetLinesForSection(letter));
            }

            return allLines;
        }

        public List<BuildingLine> GetLinesForSection(string sectionLetter)
        {
            var q = from l in this
                    where l.SectionLetter == sectionLetter
                    select l;
            return q.ToList();
        }

        public List<string> GetSectionLetters()
        {
            var q = from l in this
                    orderby l.SectionLetter
                    group l.SectionLetter by l.SectionLetter
                        into s
                    select new
                    {
                        SectionLetter = s.Key
                    };

            List<string> lets = new List<string>();
            foreach (var i in q)
            {
                lets.Add(i.SectionLetter);
            }
            return lets;
        }

        public string SectionLineDirectionalString()
        {
            //Program.stp.Restart();
            var s = new StringBuilder();
            foreach (BuildingLine line in this)
            {
                switch (line.Directional)
                {
                    case "N":
                    case "S":
                        s.Append(String.Format(" {0} {1}", line.Directional, line.YLength.ToString("N1")));
                        break;

                    case "E":
                    case "W":
                        s.Append(String.Format(" {0} {1}", line.Directional, line.XLength.ToString("N1")));
                        break;

                    case "NW":
                    case "NE":
                    case "SW":
                    case "SE":
                        s.Append(String.Format(" {0} ({1},{2})",
                            line.Directional,
                            line.YLength.ToString("N1"),
                            line.XLength.ToString("N1")));
                        break;

                    default:
                        break;
                }
                if (!String.IsNullOrEmpty(line.Attachment))
                {
                    s.Append(String.Format("({0})", line.Attachment));
                }
            }
            return s.ToString();
        }
    }
}