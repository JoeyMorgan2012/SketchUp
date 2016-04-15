using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public static class Extensions
    {
        public static Image GetImage(this string ImagePath)
        {
            Image bmp = null;

            if (ImagePath != null && ImagePath != String.Empty)
            {
                using (Stream s = new FileStream(ImagePath, FileMode.Open, FileAccess.Read))
                {
                    bmp = Image.FromStream(s);
                }
            }

            return bmp;
        }

        public static double AsDouble(this decimal d)
        {
            return Decimal.ToDouble(d);
        }

        public static string IntStringWithCommas(this decimal d)
        {
            return Convert.ToInt32(d).ToString("N0");
        }

        public static string StringWithCommas(this int i)
        {
            return i.ToString("N0");
        }

        //string SectionLineDirectionalString(this List<BuildingLine> lines)
        //{
        //    StringBuilder s = new StringBuilder();
        //    foreach (var line in lines)
        //    {
        //        switch (line.LineDirect)
        //        {
        //            case "N":
        //            case "S":
        //                s.Append(String.Format(" {0} {1}", line.LineDirect, line.YLength.ToString("N1")));
        //                break;

        //            case "E":
        //            case "W":
        //                s.Append(String.Format(" {0} {1}", line.LineDirect, line.XLength.ToString("N1")));
        //                break;

        //            case "NW":
        //            case "NE":
        //            case "SW":
        //            case "SE":
        //                s.Append(String.Format(" {0} ({1},{2})",
        //                    line.LineDirect,
        //                    line.XLength.ToString("N1"),
        //                    line.YLength.ToString("N1")));
        //                break;

        //            default:
        //                break;
        //        }
        //        if (!String.IsNullOrEmpty(line.AttachPT))
        //        {
        //            s.Append(String.Format("({0})", line.AttachPT));
        //        }
        //    }

        //    return s.ToString();
        //}

        public static string ToISeriesTimestampString(this DateTime dt)
        {
            var s = String.Empty;
            var format = "{0}-{1}-{2}-{3}.{4}.{5}.{6}";

            return String.Format(format,
                dt.Year,
                dt.Month.ToString().PadLeft(2, '0'),
                dt.Day.ToString().PadLeft(2, '0'),
                dt.Hour.ToString().PadLeft(2, '0'),
                dt.Minute.ToString().PadLeft(2, '0'),
                dt.Second.ToString().PadLeft(2, '0'),
                dt.Millisecond.ToString().PadLeft(6, '0'));
        }

        public static string RoundHundredsToString(this decimal d)
        {
            return RoundHundredsToString(Convert.ToInt64(d));
        }

        public static string RoundHundredsToString(this decimal d, string format)
        {
            return RoundHundredsToString(Convert.ToInt64(d), format);
        }

        public static string RoundHundredsToString(this int i)
        {
            return RoundHundredsToString(Convert.ToInt64(i));
        }

        public static string RoundHundredsToString(this int i, string format)
        {
            return RoundHundredsToString(Convert.ToInt64(i), format);
        }

        public static string RoundHundredsToString(this long l)
        {
            return RoundHundredsToString(l, "N0");
        }

        public static string RoundHundredsToString(this long l, string format)
        {
            return l.RoundToBase(100).ToString(format);
        }

        public static decimal ReverseAndRoundToZero(this decimal d)
        {
            return Decimal.Round(d * -1M, 0);
        }
    }
}