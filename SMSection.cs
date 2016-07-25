/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/

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
        #region Constructor

        public SMSection(SMParcel parcel)
        {
            Record = parcel.Record;
            Card = parcel.Card;
            ParentParcel = parcel;
        }

        #endregion Constructor

        #region Class Methods

        #region Public methods

        public void ReorganizeLines()
        {
            List<SMLine> sectionLines = Lines.OrderBy(l => l.LineNumber).ToList();
            for (int i = 0; i < sectionLines.Count; i++)
            {
                sectionLines[i].LineNumber = i + 1;
            }
        }

        public SMLine SelectLineByNumber(int lineNum)
        {
            SMLine line = null;
            if (Lines != null)
            {
                line = (from l in Lines where l.LineNumber == lineNum select l).FirstOrDefault<SMLine>();
            }

            return line;
        }

        #endregion Public methods

        #region Private methods

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
            var areaPoints = new List<PointF>();
            decimal area = sqFt;
            foreach (SMLine l in Lines.OrderBy(n => n.LineNumber))
            {
                areaPoints.Add(l.StartPoint);
                areaPoints.Add(l.EndPoint);
            }
            var sectionPolygon = new PolygonF(areaPoints.ToArray<PointF>());
            area = Math.Round(sectionPolygon.Area * (decimal)StoriesValue, 2);
            return area;
        }

        private PointF ComputeLabelLocation()
        {
            var sectionPoints = new List<PointF>();
            foreach (SMLine line in Lines)
            {
                sectionPoints.Add(line.ScaledStartPoint);
                sectionPoints.Add(line.ScaledEndPoint);
            }
            var sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
            PointF exactCenter = sectionBounds.CenterPointOfBounds;
            float labelSize = SectionType.Length;
            PointF adjustedCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(-labelSize, -20));
            return adjustedCenter;
        }

        private SMLine FindAnchorLine()
        {
            var anchorline = new SMLine(this);
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
        #endregion Private methods

        #endregion Class Methods

        #region Properties

        public double AdjFactor { get; set; }

        #endregion Properties

        #region Properties

        #region Computed Properties

        public SMLine AnchorLine
        {
            get {
                anchorLine = FindAnchorLine();
                return anchorLine;
            }

            set { anchorLine = value; }
        }

        public FormattableString AreaLabel
        {
            get {
                if (SectionIsClosed && SqFt > 0)
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

            set { areaLabel = value; }
        }

        public int DepreciatedValue
        {
            get {
                double adjPercent = 1 + AdjFactor;
                double deprPercent = 1 - Depreciation;
                var baseAmt = (double)(SqFt * Rate);

                double adjustedBase = baseAmt * adjPercent;
                int deprAmt = Convert.ToInt32(baseAmt * adjPercent * deprPercent);
                return HasZeroDepreciation ? 0 : Convert.ToInt32(adjustedBase -
                        deprAmt);
            }
        }

        private decimal rate;

        #endregion Computed Properties

        #region Auto-implemented properties

        public string AttachedTo { get; set; }

        public int Card { get; set; }

        public double Depreciation { get; set; }

        public string Description { get; set; }

        public List<SMLine> Lines
        {
            get {
                if (lines == null)
                {
                    lines = new List<SMLine>();
                }
                return lines;
            }

            set { lines = value; }
        }

        public virtual SMParcel ParentParcel { get; set; }

        public int Record { get; set; }

        public bool RefreshSection { get; set; } = true;

        public PointF ScaledSectionCenter
        {
            get {
                scaledSectionCenter = ComputeLabelLocation();
                return scaledSectionCenter;
            }

            set { scaledSectionCenter = value; }
        }

        public string SectionClass { get; set; }

        public bool SectionIsClosed
        {
            get {
                sectionIsClosed = CalculateOpenOrClosed();
                return sectionIsClosed;
            }

            set { sectionIsClosed = value; }
        }

        public FormattableString SectionLabel
        {
            get {
                sectionLabel = $"{SectionLetter.ToUpper()}-{SectionType.ToUpper()}{AreaLabel}";
                return sectionLabel;
            }

            set { sectionLabel = value; }
        }

        public string SectionType { get; set; }

        public int SectionValue
        {
            get {
                sectionValue = ComputeSectionValue();

                return sectionValue;
            }

            set { sectionValue = value; }
        }

        public decimal SqFt
        {
            get {
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

            set { sqFt = value; }
        }

        private int ComputeSectionValue()
        {
            int sectionVal = 0;
            var baseValue = (double)(Rate * SqFt);
            double factor = 1.00 + AdjFactor;
            double deprFactor = 1.00 - Depreciation;
            switch (ParentParcel.ParcelMast.OccupancyType)
            {
                case CamraDataEnums.OccupancyType.Commercial:
                case CamraDataEnums.OccupancyType.TaxExempt:
                    if (HasZeroDepreciation)
                    {
                        rate = Convert.ToInt32(baseValue * factor);
                    }
                    else
                    {
                        rate = Convert.ToInt32(baseValue * factor * deprFactor);
                    }
                    break;

                default:
                    sectionVal = Convert.ToInt32(Rate * SqFt);
                    break;
            }
            return sectionVal;
        }
        public int ComputedSectionValue(CamraDataEnums.OccupancyType occType, string sectionType, string sectionClass, double newFactor, double newDepr, bool has0Depr)
        {
            int sectionVal = 0;
            decimal baseRate = Rates.RateForSection(occType, sectionType, sectionClass);
            int baseValue = Convert.ToInt32(baseRate * SqFt);
            double factor = 1.00 + newFactor;
            double deprFactor = 1.00 - newDepr;
            switch (occType)
            {
                case CamraDataEnums.OccupancyType.Commercial:
                case CamraDataEnums.OccupancyType.TaxExempt:
                    if (has0Depr)
                    {
                        rate = Convert.ToInt32(baseValue * factor);
                    }
                    else
                    {
                        rate = Convert.ToInt32(baseValue * factor * deprFactor);
                    }
                    break;

                default:
                    sectionVal = Convert.ToInt32(baseRate * SqFt);
                    break;
            }
            return sectionVal;
        }

        private string hasSketch;

        #endregion Auto-implemented properties

        public bool DeleteSection { get; set; } = false;

        public string HasSketch
        {
            get {
                if (string.IsNullOrEmpty(hasSketch))
                {
                    hasSketch = "Y";
                }
                return hasSketch;
            }

            set { hasSketch = value; }
        }

        public bool HasZeroDepreciation => ZeroDepr == "Y";

        public decimal Rate
        {
            get {
                if (!SectionBeingEdited)
                {
                    rate = Rates.RateForSection(this);
                }

                return rate;
            }

            set { rate = value; }
        }

        public bool SectionBeingEdited { get; set; } = false;

        public string SectionLetter { get; set; }

        public string StoriesText
        {
            get {
                if (string.IsNullOrEmpty(storiesText))
                {
                    storiesText = StoriesValue.ToString("N2");
                }
                return storiesText;
            }

            set { storiesText = value; }
        }

        public double StoriesValue { get; set; }

        public string ZeroDepr { get; set; }

        #endregion Properties

        #region Fields

        private SMLine anchorLine;
        private FormattableString areaLabel;
        private List<SMLine> lines;
        private PointF scaledSectionCenter;
        private bool sectionIsClosed;
        private FormattableString sectionLabel;
        private int sectionValue;
        private decimal sqFt;
        private string storiesText;

        #endregion Fields
    }
}