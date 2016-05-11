using SWallTech;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public class SMParcel
    {
#region "Public Methods"

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

        public void SetScaleAndOriginForParcel(int width, int height, PointF containerCorner)
        {
            SetSketchScale(width,height);
            SetSketchOrigin(width,height,containerCorner);
            SetScaledStartPoints();
        }

        public void SetScaleAndOriginForParcel(PictureBox targetContainer)
        {
            SetSketchScale(targetContainer);
            SetSketchOrigin(targetContainer);
            SetScaledStartPoints();
        }

#endregion

#region "Private methods"

        private List<PointF> AllCorners()
        {
            List<PointF> points = SelectAllPoints();
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

        private bool checkAllSectionsAreClosed()
        {
            bool sectionClosed = true;
            foreach (SMSection s in Sections)
            {
                sectionClosed &= s.SectionIsClosed;
            }
            return sectionClosed;
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

        private List<SMLine> SelectAllLines()
        {
            List<SMLine> allLines = new List<SMLine>();
            if (Sections != null)
            {
                foreach (SMSection sms in Sections.Where(l => l.Lines != null && l.Lines.Count > 0))
                {
                    allLines.AddRange(sms.Lines);
                }
            }
            return allLines;
        }

        private List<PointF> SelectAllPoints()
        {
            List<PointF> points = new List<PointF>();
            foreach (SMLine l in AllSectionLines.OrderBy(n => n.LineNumber).OrderBy(s => s.SectionLetter))
            {
                points.Add(l.StartPoint);
                points.Add(l.EndPoint);
            }

            return points;
        }

        private void SetScaledStartPoints()
        {
            if (Sections != null)
            {
                decimal sketchScale = Scale;
                foreach (SMSection s in Sections)
                {
                    foreach (SMLine line in s.Lines)
                    {
                        line.ScaledStartPoint = SMGlobal.DbPointToScaledPoint(line.StartX, line.StartY, line.ParentParcel.Scale, SketchOrigin);
                    }
                }
            }
        }

        private void SetSketchOrigin(int width, int height, PointF cornerLocation)
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            var sketchAreaWidth = width - 20;
            var sketchAreaHeight = height - 20;

            PointF pictureBoxCorner = cornerLocation;
            var extraWidth = (width - 20) - (Scale * SketchXSize);
            var extraHeight = (height - 20) - (Scale * SketchYSize);
            var paddingX = (extraWidth / 2) + 10;
            var paddingY = (extraHeight / 2) + 10;
            var xLocation = (OffsetX * Scale) + paddingX;
            var yLocation = (OffsetY * Scale) + paddingY;

            SketchOrigin = new PointF((float)xLocation, (float)yLocation);
        }

        private void SetSketchOrigin(PictureBox targetContainer)
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            var sketchAreaWidth = targetContainer.Width - 20;
            var sketchAreaHeight = targetContainer.Height - 20;
            PointF pictureBoxCorner = targetContainer.Location;
            var extraWidth = (targetContainer.Width - 20) - (Scale * SketchXSize);
            var extraHeight = (targetContainer.Height - 20) - (Scale * SketchYSize);
            var paddingX = (extraWidth / 2) + 10;
            var paddingY = (extraHeight / 2) + 10;
            var xLocation = (OffsetX * Scale) + paddingX;
            var yLocation = (OffsetY * Scale) + paddingY;

            SketchOrigin = new PointF((float)xLocation, (float)yLocation);
        }

        private void SetSketchScale(int width, int height)
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = height - 20;
            int boxWidth = width - 20;
            decimal xScale = Math.Floor(boxWidth / SketchXSize);
            decimal yScale = Math.Floor(boxHeight / SketchYSize);
            Scale = SMGlobal.SmallerDouble(xScale, yScale) * 0.85M;
        }

        private void SetSketchScale(PictureBox targetContainer)
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = targetContainer.Height - 20;
            int boxWidth = targetContainer.Width - 20;
            decimal xScale = Math.Floor(boxWidth / SketchXSize);
            decimal yScale = Math.Floor(boxHeight / SketchYSize);
            Scale = SMGlobal.SmallerDouble(xScale, yScale) * 0.85M;
        }

#endregion

        public List<SMLine> AllSectionLines
        {
            get
            {
                allSectionLines = SelectAllLines();

                return allSectionLines;
            }
            set
            {
                allSectionLines = value;
            }
        }

        #region Properties
        public bool AllSectionsClosed
        {
            get
            {
                allSectionsClosed = checkAllSectionsAreClosed();
                return allSectionsClosed;
            }
            set
            {
                allSectionsClosed = value;
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

        public PointF JumpMouseLocation
        {
            get
            {
                return jumpMouseLocation;
            }
            set
            {
                jumpMouseLocation = value;
                foreach (SMLine l in AllSectionLines)
                {
                    l.ComparisonPoint = jumpMouseLocation;
                }
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

        public virtual SMParcelMast ParcelMast
        {
            get
            {
                return parcelMast;
            }
            set
            {
                parcelMast = value;
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
                    scale = SketchUpGlobals.DefaultScale;
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

        public Image SketchImage
        {
            get
            {
                return sketchImage;
            }
            set
            {
                sketchImage = value;
            }
        }

        public PointF SketchOrigin
        {
            get
            {
                if (sketchOrigin == null)
                {
                    sketchOrigin = new PointF(0, 0);
                }
                return sketchOrigin;
            }
            set
            {
                sketchOrigin = value;
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

        #endregion
        private List<SMLine> allSectionLines;
        private bool allSectionsClosed;
        private int card;
        private List<PointF> cornerPoints;
        private string exSketch;
        private string hasSketch;
        private PointF jumpMouseLocation;
        private string lastSectionLetter;
        private string nextSectionLetter;
        private decimal offsetX;
        private decimal offsetY;
        private SMParcelMast parcelMast;
        private int record;
        private bool refreshParcel = true;
        private decimal scale;
        private List<SMSection> sections;
        private Image sketchImage;
        private PointF sketchOrigin;
        private decimal sketchXSize;
        private decimal sketchYSize;
        private int snapShotIndex;
        private string storeyEx;
        private decimal storeys;
        private decimal totalSqFt;
    }
}
