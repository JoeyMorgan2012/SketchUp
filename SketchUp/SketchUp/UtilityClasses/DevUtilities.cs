using System;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class DevUtilities
    {
        #region "Public Methods"

        public string DescribeAll()
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();
            details.AppendLine(string.Format("{0}", string.Empty));
            description = details.ToString();
            return description;
        }

        public string DescribeLine(DrawOnlyLine drawLine)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();

            //  Section\tLine#\tStart Point\tEnd Point\tX-Length\tY-Length\tDirection\tAttached
            details.AppendLine(string.Format("{0}\t{1}\t({2:N2},{3:N2})\t({4:N2},{5:N2})\t{6:N2}\t\t{7:N2}\t\t{8}\t\t{9}", drawLine.SectionLetter, drawLine.LineNumber, drawLine.StartX, drawLine.StartY, drawLine.EndX, drawLine.EndY, drawLine.XLength, drawLine.YLength, drawLine.Direction, drawLine.AttachedSection));
            description = details.ToString();
            return description;
        }

        public string DescribeLine(SMLine sketchLine)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();

            //  Section\tLine#\tStart Point\tEnd Point\tX-Length\tY-Length\tDirection\tAttached
            details.AppendLine(string.Format("{0}\t{1}\t({2:N2},{3:N2})\t({4:N2},{5:N2})\t{6:N2}\t\t{7:N2}\t\t{8}\t\t{9}", sketchLine.SectionLetter, sketchLine.LineNumber, sketchLine.StartX, sketchLine.StartY, sketchLine.EndX, sketchLine.EndY, sketchLine.XLength, sketchLine.YLength, sketchLine.Direction, sketchLine.AttachedSection));
            description = details.ToString();
            return description;
        }

        public string DescribeMast(SMParcelMast mast)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();
            details.AppendLine(string.Format("{0}", string.Empty));
            description = details.ToString();
            return description;
        }

        public string DescribeParcel(SMParcel parcel)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();
            details.AppendLine(string.Format("{0}", string.Empty));
            description = details.ToString();
            return description;
        }

        #region Section

        public string DescribeSection(SMSection section, bool includeLines = false)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();
            details.AppendLine(string.Format("{0}\t\t{1:N2}\t\t{2:N2}\t\t{3}\t\t{4}", section.SectionLabel, section.Storeys, section.SqFt, section.Lines.Count, section.AttachedTo));
            if (includeLines && section.Lines != null && section.Lines.Count > 0)
            {
                details.AppendLine(LinesHeader());
                foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
                {
                    details.AppendLine(DescribeLine(l));
                }
            }
            description = details.ToString();
            return description;
        }

        public string SectionHeader()
        {
            string header = "Section\t\tStoreys\tSqFt\t\tLine Count\tAttachedTo\n______________________________________________________";

            return header;
        }

        #endregion Section

        public string MastHeader()
        {
            string header = string.Empty;

            return header;
        }

        public string ParcelHeader()
        {
            string header = string.Empty;

            return header;
        }

        #region Lines

        public string DescribeSectionLines(SMSection section)
        {
            string description = string.Empty;
            StringBuilder details = new StringBuilder();

            foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
            {
                //  Section\tLine#\tStart Point\tEnd Point\tX-Length\tY-Length\tDirection\tAttached
                details.AppendLine(string.Format("{0}\t{1}\t{2:N2},{3:N2}\t{4:N2},{5:N2}\t{6:N2}\t{7:N2}\t{8}\t{9}", l.SectionLetter, l.LineNumber, l.StartX, l.StartY, l.EndX, l.EndY, l.XLength, l.YLength, l.Direction, l.AttachedSection));
            }
            description = details.ToString();
            return description;
        }

        public string LinesHeader()
        {
            string header = "Section\tLine#\tStart Point\tEnd Point\tX-Length\tY-Length\tDirection\tAttached";

            return header;
        }

        #endregion Lines

        #region Capture Info

        public string ParcelInfo(SMParcel parcel)
        {
            StringBuilder output = new StringBuilder();
            DevUtilities du = new DevUtilities();
            output.AppendLine(string.Format("Parcel Info for {0} record {1}, read {2:mm-dd-yyyy} at {3:hh:mm:ss}", SketchUpGlobals.LocalityDescription, parcel.Record, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
            output.AppendLine(du.SectionHeader());
            foreach (SMSection s in parcel.Sections)
            {
                output.AppendLine(du.DescribeSection(s, true));
            }
            Console.Write(output.ToString());
            return output.ToString();
        }

        #endregion Capture Info

        #endregion "Public Methods"
    }
}