using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public class SMSection
    {
        #region "Constructor"

        public SMSection(SMParcel parcel)
        {
            record = parcel.Record;
            card = parcel.Card;
            ParentParcel = parcel;
        }

        #endregion "Constructor"

        #region "Public Methods"

        public SMLine SelectLineByNumber(int lineNum)
        {
            SMLine line = null;
            if (Lines != null)
            {
                line = (from l in Lines where l.LineNumber == lineNum select l).FirstOrDefault<SMLine>();
            }

            return line;
        }

        #endregion "Public Methods"

        #region "Private methods"

        private bool CalculateOpenOrClosed()
        {
            bool closed = false;
            if (Lines != null && lines.Count > 2)

            //Cannot close a shape unless there are at least three lines!
            {
                SMLine firstLine = (from l in Lines orderby l.LineNumber select l).First();
                SMLine lastLine = (from l in Lines orderby l.LineNumber select l).Last();
                if (lastLine.EndX == firstLine.StartX && lastLine.EndY == firstLine.StartY)
                {
                    closed = true;
                }
            }
            return closed;
        }

        private decimal CalculateSectionArea()
        {
            List<PointF> areaPoints = new List<PointF>();
            decimal area = sqFt;
            foreach (SMLine l in Lines.OrderBy(n => n.LineNumber))
            {
                areaPoints.Add(l.StartPoint);
                areaPoints.Add(l.EndPoint);
            }
            PolygonF sectionPolygon = new PolygonF(areaPoints.ToArray<PointF>());
            area = Math.Round(sectionPolygon.Area*Storeys, 2);
            return area;
        }

        private PointF ComputeLabelLocation()
        {
            List<PointF> sectionPoints = new List<PointF>();
            foreach (SMLine line in Lines)
            {
                sectionPoints.Add(line.ScaledStartPoint);
                sectionPoints.Add(line.ScaledEndPoint);
            }
            PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
            PointF exactCenter = sectionBounds.CenterPointOfBounds;
            float labelSize = SectionType.Length;
            PointF adjustedCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(-labelSize, -20));
            return adjustedCenter;
        }

        private SMLine FindAnchorLine()
        {
            SMLine anchorline = new SMLine(this);
            if (SectionLetter != "A")
            {
                anchorline = (from l in ParentParcel.AllSectionLines where l.AttachedSection == SectionLetter select l).FirstOrDefault<SMLine>();
            }
            else
            {
                anchorline = new SMLine(this);
            }
            if (anchorLine == null)
            {
                anchorline = new SMLine(this);
            }
            return anchorline;
        }

        #endregion "Private methods"

        #region "Properties"

        public decimal AdjFactor
        {
            get
            {
                return adjFactor;
            }

            set
            {
                adjFactor = value;
            }
        }

        public SMLine AnchorLine
        {
            get
            {
                anchorLine = FindAnchorLine();
                return anchorLine;
            }

            set
            {
                anchorLine = value;
            }
        }

        public FormattableString AreaLabel
        {
            get
            {
                if (SectionIsClosed&&SqFt>0)
                {
  string area = SqFt.ToString();
                areaLabel = $"\n{area} ft²";
                }
                else
                {
                    string area = string.Empty;
                    areaLabel = $"{area}";
                }
               
                return areaLabel;
            }

            set
            {
                areaLabel = value;
            }
        }

        public string AttachedTo
        {
            get
            {
                return attachedTo;
            }

            set
            {
                attachedTo = value;
            }
        }

        public decimal Depreciation
        {
            get
            {
                return depreciation;
            }

            set
            {
                depreciation = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
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

        public bool HasSketch
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

        public List<SMLine> Lines
        {
            get
            {
                if (lines == null)
                {
                    lines = new List<SMLine>();
                }
                return lines;
            }

            set
            {
                lines = value;
            }
        }

        public virtual SMParcel ParentParcel
        {
            get;
            set;
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

        public bool RefreshSection
        {
            get
            {
                return refreshSection;
            }

            set
            {
                refreshSection = value;
            }
        }

        public PointF ScaledSectionCenter
        {
            get
            {
                scaledSectionCenter = ComputeLabelLocation();
                return scaledSectionCenter;
            }

            set
            {
                scaledSectionCenter = value;
            }
        }

        public string SectionClass
        {
            get
            {
                return sectionClass;
            }

            set
            {
                sectionClass = value;
            }
        }

        public bool SectionIsClosed
        {
            get
            {
                sectionIsClosed = CalculateOpenOrClosed();
                return sectionIsClosed;
            }

            set
            {
                sectionIsClosed = value;
            }
        }

        public FormattableString SectionLabel
        {
            get
            {
                sectionLabel = $"{SectionLetter.ToUpper()}-{SectionType.ToUpper()}{AreaLabel}";
                return sectionLabel;
            }

            set
            {
                sectionLabel = value;
            }
        }

        public string SectionLetter
        {
            get
            {
                return sectionLetter;
            }

            set
            {
                sectionLetter = value;
            }
        }

        public string SectionType
        {
            get
            {
                return sectionType;
            }

            set
            {
                sectionType = value;
            }
        }

        public decimal SectionValue
        {
            get
            {
                return sectionValue;
            }

            set
            {
                sectionValue = value;
            }
        }

        public decimal SqFt
        {
            get
            {
                if (SectionIsClosed)
                {
                    sqFt = CalculateSectionArea();
                }
                else
                {
                    sqFt = 0.00M;
                }
                return sqFt;
            }

            set
            {
                sqFt = value;
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

        public string ZeroDepr
        {
            get
            {
                return zeroDepr;
            }

            set
            {
                zeroDepr = value;
            }
        }

   

        #endregion "Properties"

        #region "Private Fields"
      
        private decimal adjFactor;
        private SMLine anchorLine;
        private FormattableString areaLabel;
        private string attachedTo;
        private decimal depreciation;
        private string description;
        private int card;
        private bool hasSketch;
        private List<SMLine> lines;
        private int record;
        private bool refreshSection = true;
        private PointF scaledSectionCenter;
        private string sectionClass;
        private bool sectionIsClosed;
        private FormattableString sectionLabel;
        private string sectionLetter;
        private string sectionType;
        private decimal sectionValue;
        private decimal sqFt;
        private decimal storeys;
        private string zeroDepr;

        #endregion "Private Fields"
    }
}