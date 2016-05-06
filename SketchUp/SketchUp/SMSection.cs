using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public class SMSection
    {
        #region class methods

        public SMLine SelectLineByNumber(int lineNum)
        {
            SMLine line = null;
            if (Lines != null)
            {
                line = (from l in Lines where l.LineNumber == lineNum select l).FirstOrDefault<SMLine>();
            }

            return line;
        }

       

        #endregion class methods

        #region Class Properties

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

        public int Dwelling
        {
            get
            {
                return dwelling;
            }

            set
            {
                dwelling = value;
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
                if (lines==null)
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

        #endregion Class Properties

        #region Constructors

        public SMSection(SMParcel parcel)
        {
            record = parcel.Record;
            dwelling = parcel.Card;
            ParentParcel = parcel;
        }

        #endregion Constructors

        #region Fields
        private bool sectionIsClosed;
        private decimal adjFactor;
        private SMLine anchorLine;
        private string attachedTo;
        private decimal depreciation;
        private string description;
        private int dwelling;
        private bool hasSketch;
        private List<SMLine> lines;
        private int record;
        private bool refreshSection = true;
        private PointF scaledSectionCenter;
        private string sectionClass;
        private string sectionLabel;
        private string sectionLetter;
        private string sectionType;
        private decimal sectionValue;
        private decimal sqFt;
        private decimal storeys;
        private string zeroDepr;

        #endregion Fields

        #region Virtual/navigation properties

        public virtual SMParcel ParentParcel
        {
            get;
            set;
        }

        #endregion Virtual/navigation properties

        private SMLine FindAnchorLine()
        {
            SMLine anchorline = new SMLine(ParentParcel.Record, ParentParcel.Card, "A");
            if (SectionLetter != "A")
            {
                anchorline = (from l in ParentParcel.AllSectionLines where l.AttachedSection == SectionLetter select l).FirstOrDefault<SMLine>();
                if (anchorLine == null)
                {
                    return new SMLine(ParentParcel.Record, ParentParcel.Card, "A");
                }
            }

            return anchorline;
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

        public string SectionLabel
        {
            get
            {
                sectionLabel = string.Format("{0}-{1}", SectionLetter.ToUpper(), SectionType.ToUpper());
                return sectionLabel;
            }

            set
            {
                sectionLabel = value;
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

        private bool CalculateOpenOrClosed()
        {
            bool closed = false;
            if (Lines!=null&&lines.Count>2)
                //Cannot close a shape unless there are at least three lines!
            {

     SMLine firstLine = (from l in Lines orderby l.LineNumber select l).First();
            SMLine lastLine= (from l in Lines orderby l.LineNumber select l).Last();
                if (lastLine.EndX==firstLine.StartX&&lastLine.EndY==firstLine.StartY)
                {
                    closed = true;
                }
            }
            return closed;
       
        }
    }
}