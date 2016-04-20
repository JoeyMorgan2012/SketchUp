using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SketchUp
{
    public class SMParcel
    {
        private List<PointF> AllCorners()
        {
            List<PointF> points = new List<PointF>();
            foreach (SMLine l in AllSectionLines.OrderBy(n => n.LineNumber).OrderBy(s => s.SectionLetter))
            {
                points.Add(l.StartPoint);
                points.Add(l.EndPoint);
            }
            return points;
        }

        private decimal CalculateYSize()
        {
            List<decimal> yList = new List<decimal>();

            yList.AddRange((from l in AllSectionLines select l.StartY).ToList());
            yList.AddRange((from l in AllSectionLines select l.EndY).ToList());
            decimal minY = yList.Min();
            decimal maxY = yList.Max();
            return Math.Abs(maxY - minY);
        }

        private decimal CalulateSketchXSize()
        {
            List<decimal> xList = new List<decimal>();

            xList.AddRange((from l in AllSectionLines select l.StartX).ToList());
            xList.AddRange((from l in AllSectionLines select l.EndX).ToList());
            decimal minX = xList.Min();
            decimal maxX = xList.Max();
            return Math.Abs(maxX - minX);
        }

        public void IdentifyAttachedToSections()
        {
            if (AllSectionLines != null)
            {
                foreach (SMSection sms in Sections)
                {
                    sms.AttachedTo = (from l in AllSectionLines where l.AttachedSection == sms.SectionLetter select l.SectionLetter).FirstOrDefault<string>() ?? string.Empty;
                }
            }
        }

        public SMLine SelectLineBySectionAndNumber(string sectionLetter, int lineNum)
        {
            SMLine selectedLine = null;
            if (Sections != null && Sections.Count > 0)
            {
                SMSection selectedSection = SelectSectionByLetter(sectionLetter);
                if (selectedSection != null)
                {
                    selectedLine = selectedSection.SelectLineByNumber(lineNum);
                }
            }

            return selectedLine;
        }

        public SMSection SelectSectionByLetter(string sectionLetter)
        {
            SMSection selectedSection = null;
            if (Sections != null && Sections.Count > 0)
            {
                selectedSection = (from s in Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();
            }
            return selectedSection;
        }

        public List<SMLine> AllSectionLines
        {
            get
            {
                allSectionLines = new List<SMLine>();
                if (Sections != null)
                {
                    foreach (SMSection sms in Sections)
                    {
                        allSectionLines.AddRange(sms.Lines);
                    }
                }

                return allSectionLines;
            }

            set
            {
                allSectionLines = value;
            }
        }

        public List<PointF> CornerPoints
        {
            get
            {
                if (refreshParcel || cornerPoints == null)
                {
                    cornerPoints = AllCorners();
                }

                return cornerPoints;
            }

            set
            {
                cornerPoints = value;
            }
        }

        public int Card
        {
            get
            {
                return card;
            }

            set
            {
                card = value;
            }
        }

        public string ExSketch
        {
            get
            {
                return exSketch;
            }

            set
            {
                exSketch = value;
            }
        }

        public string HasSketch
        {
            get
            {
                return hasSketch;
            }

            set
            {
                hasSketch = value;
            }
        }

        public decimal OffsetX
        {
            get
            {
                decimal minX = (from l in allSectionLines where l.MinX <= 0 select l.MinX).Min();
                offsetX = Math.Abs(minX);
                return offsetX;
            }

            set
            {
                offsetX = value;
            }
        }

        public decimal OffsetY
        {
            get
            {
                decimal minY = (from l in allSectionLines where l.MinY <= 0 select l.MinY).Min();
                offsetY = Math.Abs(minY);
                return offsetY;
            }

            set
            {
                offsetY = value;
            }
        }

        public int Record
        {
            get
            {
                return record;
            }

            set
            {
                record = value;
            }
        }

        public bool RefreshParcel
        {
            get
            {
                return refreshParcel;
            }

            set
            {
                refreshParcel = value;
            }
        }

        public decimal Scale
        {
            get
            {
                if (scale == 0M)
                {
                    scale = 1.0M;
                }
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public List<SMSection> Sections
        {
            get
            {
                // Avoid NullReferenceExceptions by returning an empty list
                if (sections == null)
                {
                    sections = new List<SMSection>();
                }
                return sections;
            }

            set
            {
                sections = value;
            }
        }

        public decimal SketchXSize
        {
            get
            {
                sketchXSize = CalulateSketchXSize();

                return sketchXSize;
            }

            set
            {
                sketchXSize = value;
            }
        }

        public decimal SketchYSize
        {
            get
            {
                sketchYSize = CalculateYSize();
                return sketchYSize;
            }

            set
            {
                sketchYSize = value;
            }
        }

        public string StoreyEx
        {
            get
            {
                return storeyEx;
            }

            set
            {
                storeyEx = value;
            }
        }

        public decimal Storeys
        {
            get
            {
                return storeys;
            }

            set
            {
                storeys = value;
            }
        }

        public decimal TotalSqFt
        {
            get
            {
                return totalSqFt;
            }

            set
            {
                totalSqFt = value;
            }
        }

        public int SnapShotIndex
        {
            get
            {
                return snapShotIndex;
            }

            set
            {
                snapShotIndex = value;
            }
        }

        public string LastSectionLetter
        {
            get
            {
                lastSectionLetter = GetLastSectionLetter(Sections);
                return lastSectionLetter;
            }

            set
            {
                lastSectionLetter = value;
            }
        }

        private string GetLastSectionLetter(List<SMSection> sections)
        {
            string lastLetter = string.Empty;
            if (sections != null && sections.Count > 0)
            {
                lastLetter = (from s in sections orderby s.SectionLetter descending select s.SectionLetter).FirstOrDefault();
            }
            return lastLetter;
        }

        public string NextSectionLetter
        {
            get
            {
                nextSectionLetter = SketchUp.UtilityMethods.NextLetter(LastSectionLetter);
                return nextSectionLetter;
            }

            set
            {
                nextSectionLetter = value;
            }
        }

     
     
        private string lastSectionLetter;
        private string nextSectionLetter;
        private int snapShotIndex;
        private List<SMLine> allSectionLines;
        private List<PointF> cornerPoints;
        private int card;
        private string exSketch;
        private string hasSketch;
        private decimal offsetX;
        private decimal offsetY;
        private int record;
        private bool refreshParcel = true;
        private decimal scale;
        private List<SMSection> sections;
        private decimal sketchXSize;
        private decimal sketchYSize;
        private string storeyEx;
        private decimal storeys;
        private decimal totalSqFt;
    }
}