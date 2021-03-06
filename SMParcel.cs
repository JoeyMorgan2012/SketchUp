﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public class SMParcel
    {
        #region "Public Methods"

        public void RemoveSectionFromParcel(SMSection section)
        {
            var sr = new SketchRepository(section.ParentParcel);
            if (section.SectionLetter == "A")
            {
                RemoveSectionA(section);
            }
            else
            { 
                Sections.Remove(section);
            }
            ReorganizeSections();
        }

        public void RemoveSectionFromParcel(string sectionLetter)
        {
            SMSection section = SelectSectionByLetter(sectionLetter);
            RemoveSectionFromParcel(section);
        }

        public void ReorganizeParcelLineConnections()
        {
            try
            {
                foreach (SMLine l in AllSectionLines)
                {
                    l.AttachedSection = string.Empty;
                    List<SMLine> attachedLines = (from al in AllSectionLines where al.SectionLetter != "A" && al.SectionLetter != l.SectionLetter && al.StartPoint == l.EndPoint && al.LineNumber == 1 select al).Distinct().ToList();
                    if (attachedLines != null)
                    {
                        if (attachedLines.Count == 1) //Only one section attaches
                        {
                            l.AttachedSection = attachedLines[0].SectionLetter.ToUpper();
                        }
                        else
                        {
                            for (int i = 1; i < attachedLines.Count; i++)
                            {
                                SMLine dupLine = l;
                                SMSection currentSection = SelectSectionByLetter(l.SectionLetter);
                                int lineNum = (from SMLine line in currentSection.Lines select line.LineNumber).Max() + 1;
                                dupLine.LineNumber = lineNum;
                                dupLine.AttachedSection = attachedLines[i].SectionLetter.ToUpper();
                                currentSection.Lines.Add(dupLine);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        public void ReorganizeSections()
        {
            try
            {
                string sectionLetter = string.Empty;
                List<string> sectionLetters = SketchUpLookups.SectionLetters();
                if (Sections != null)
                {
                    for (int i = 0; i < Sections.Count; i++)
                    {
                        sectionLetter = sectionLetters[i];
                        Sections[i].SectionLetter = sectionLetter;
                        Sections[i].Lines.ForEach(a => a.SectionLetter = sectionLetter);
                        Sections[i].ReorganizeLines();
                    }

                    ReorganizeParcelLineConnections();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
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
                selectedSection = (from s in Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault();
            }
            return selectedSection;
        }

        public void SetScaleAndOriginForParcel(int width, int height, PointF containerCorner)
        {
            SetSketchScale(width, height);
            SetSketchOrigin(width, height, containerCorner);
            SetScaledStartPoints();
        }

        public void SetScaleAndOriginForParcel(PictureBox targetContainer)
        {
            SetSketchScale(targetContainer);
            SetSketchOrigin(targetContainer);
            SetScaledStartPoints();
        }

        private static void RemoveSectionA(SMSection section)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Warning;
            MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
            var sr = new SketchRepository(section.ParentParcel);
            string message = "You are removing the base section! You must have a section \"A\" in place. Removing this section will remove the entire sketch, and you will have to redraw it.\n\nProceed?";
            string title = "Delete Base Section A?";

            DialogResult response = MessageBox.Show(message, title, buttons, icon, defButton);
            switch (response)
            {
                case DialogResult.Yes:
                    buttons = MessageBoxButtons.YesNoCancel;
                    icon = MessageBoxIcon.Question;
                    title = "Make Parcel Vacant?";
                    message = "Do you wish to make this parcel vacant?\nSelect No to delete the sketch and start it over, but leave the parcel information unchanged. Select Cancel to leave the sketch information unchanged.\nSelecting Yes will delete all structure information and make this parcel vacant.";
                    response = MessageBox.Show(message, title, buttons, icon, defButton);
                    switch (response)
                    {
                        case DialogResult.Yes:
                            message = "Are you sure? You will have to re-enter all structure information in CAMRA. Click \"Yes\" to make this parcel vacant, \"No\" to proceed with deleting the sketch, or \"Cancel\" to back out completely.";
                            title = "Confirm Vacant Parcel";
                            icon = MessageBoxIcon.Warning;
                            response = MessageBox.Show(message, title, buttons, icon, defButton);
                            switch (response)
                            {
                                case DialogResult.Yes:

                                    sr.DeleteSketch(section.ParentParcel, true);
                                    break;

                                case DialogResult.No:
                                    sr = new SketchRepository(section.ParentParcel);
                                    sr.DeleteSketch(section.ParentParcel, false);
                                    break;

                                case DialogResult.None:
                                case DialogResult.Cancel:
                                default:

                                    //Do nothing and leave everything as-is
                                    break;
                            }

                            break;

                        case DialogResult.No:

                            sr.DeleteSketch(section.ParentParcel, false);
                            break;

                        case DialogResult.None:
                        case DialogResult.Cancel:
                        default:

                            //Do nothing and leave everything as-is
                            break;
                    }

                    break;

                default:

                    //Do nothing and leave everything as-is
                    break;
            }
        }

        #endregion "Public Methods"

        #region "Private methods"

        public void IdentifyAttachedToSections()
        {
            if (AllSectionLines != null)
            {
                foreach (SMSection sms in Sections)
                {
                    sms.AttachedTo = (from l in AllSectionLines where l.AttachedSection == sms.SectionLetter select l.SectionLetter).FirstOrDefault() ?? string.Empty;
                }
            }
        }

        private List<PointF> AllCorners()
        {
            List<PointF> points = SelectAllPoints();
            return points;
        }

        private double CalculateYSize()
        {
            var yList = new List<double>();

            yList.AddRange((from l in AllSectionLines select l.StartY).ToList());
            yList.AddRange((from l in AllSectionLines select l.EndY).ToList());
            double minY = yList.Min();
            double maxY = yList.Max();
            return Math.Abs(maxY - minY);
        }

        private double CalulateSketchXSize()
        {
            var xList = new List<double>();

            xList.AddRange((from l in AllSectionLines select l.StartX).ToList());
            xList.AddRange((from l in AllSectionLines select l.EndX).ToList());
            double minX = xList.Min();
            double maxX = xList.Max();
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
            var allLines = new List<SMLine>();
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
            var points = new List<PointF>();
            foreach (SMLine l in AllSectionLines.OrderBy(n => n.LineNumber).OrderBy(s => s.SectionLetter))
            {
                points.Add(l.StartPoint);
                points.Add(l.EndPoint);
            }

            return points;
        }

        private void SetScaledStartPoints()
        {
            if (Sections != null && Sections.Count > 0)
            {
                double sketchScale = Scale;
                foreach (DrawOnlyLine d in DrawOnlyLines)
                {
                    d.ScaledStartPoint = SMGlobal.DbPointToScaledPoint(d.StartX, d.StartY, Scale, SketchOrigin);
                }
                foreach (SMSection s in Sections)
                {
                    foreach (SMLine line in s.Lines)
                    {
                        line.ScaledStartPoint = SMGlobal.DbPointToScaledPoint(line.StartX, line.StartY, Scale, SketchOrigin);
                    }
                }
            }
        }

        private void SetSketchOrigin(int width, int height, PointF cornerLocation)
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            if (Sections != null && Sections.Count > 0)
            {
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
            else
            {
                SketchOrigin = new PointF(width / 2, height / 2);
            }
        }

        private void SetSketchOrigin(PictureBox targetContainer)
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            if (Sections != null && Sections.Count > 0)
            {
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
            else
            {
                SketchOrigin = new PointF(targetContainer.Width / 2, targetContainer.Height / 2);
            }
        }

        private void SetSketchScale(int width, int height)
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = height - 20;
            int boxWidth = width - 20;
            double xScale = SketchXSize > 0 ? Math.Floor(boxWidth / SketchXSize) : 7.2;
            double yScale = SketchYSize > 0 ? Math.Floor(boxHeight / SketchYSize) : 7.2;
            Scale = SMGlobal.SmallerDouble(xScale, yScale) * 0.85;
        }

        private void SetSketchScale(PictureBox targetContainer)
        {
            if (Sections != null && Sections.Count > 0)
            {
                int boxHeight = targetContainer.Height - 20;
                int boxWidth = targetContainer.Width - 20;
                double xScale = SketchXSize > 0 ? Math.Floor(boxWidth / SketchXSize) : 7.2;
                double yScale = SketchYSize > 0 ? Math.Floor(boxHeight / SketchYSize) : 7.2;
                Scale = SMGlobal.SmallerDouble(xScale, yScale) * 0.85;
            }
            else
            {
                Scale = 7.2;
            }
        }

        #endregion "Private methods"

        #region "Properties"

        public List<SMLine> AllSectionLines
        {
            get {
                allSectionLines = SelectAllLines();

                return allSectionLines;
            }

            set { allSectionLines = value; }
        }

        public bool AllSectionsClosed
        {
            get {
                allSectionsClosed = checkAllSectionsAreClosed();
                return allSectionsClosed;
            }

            set { allSectionsClosed = value; }
        }

        public int Card
        {
            get { return card; }

            set { card = value; }
        }

        public List<PointF> CornerPoints
        {
            get {
                if (refreshParcel || cornerPoints == null)
                {
                    cornerPoints = AllCorners();
                }

                return cornerPoints;
            }

            set { cornerPoints = value; }
        }

        public List<DrawOnlyLine> DrawOnlyLines
        {
            get {
                if (drawOnlyLines == null)
                {
                    drawOnlyLines = new List<DrawOnlyLine>();
                }
                return drawOnlyLines;
            }

            set { drawOnlyLines = value; }
        }

        public string ExSketch
        {
            get { return exSketch; }

            set { exSketch = value; }
        }

        public string HasSketch
        {
            get { return hasSketch; }

            set { hasSketch = value; }
        }

        public PointF JumpMouseLocation
        {
            get { return jumpMouseLocation; }

            set {
                jumpMouseLocation = value;
                foreach (SMLine l in AllSectionLines)
                {
                    l.ComparisonPoint = jumpMouseLocation;
                }
            }
        }

        public string LastSectionLetter
        {
            get {
                lastSectionLetter = GetLastSectionLetter(Sections);
                return lastSectionLetter;
            }

            set { lastSectionLetter = value; }
        }

        public string NextSectionLetter
        {
            get {
                nextSectionLetter = SketchUp.UtilityMethods.NextLetter(LastSectionLetter);
                return nextSectionLetter;
            }

            set { nextSectionLetter = value; }
        }

        public double OffsetX
        {
            get {
                double minX = (from l in allSectionLines where l.MinX <= 0 select l.MinX).Min();
                offsetX = Math.Abs(minX);
                return offsetX;
            }

            set { offsetX = value; }
        }

        public double OffsetY
        {
            get {
                double minY = (from l in allSectionLines where l.MinY <= 0 select l.MinY).Min();
                offsetY = Math.Abs(minY);
                return offsetY;
            }

            set { offsetY = value; }
        }

        public virtual SMParcelMast ParcelMast
        {
            get { return parcelMast; }

            set { parcelMast = value; }
        }

        public int Record
        {
            get { return record; }

            set { record = value; }
        }

        public double Scale
        {
            get {
                if (scale == 0)
                {
                    scale = SketchUpGlobals.DefaultScale;
                }
                return scale;
            }

            set { scale = value; }
        }

        public List<SMSection> Sections
        {
            get {
                // Avoid NullReferenceExceptions by returning an empty list
                if (sections == null)
                {
                    sections = new List<SMSection>();
                }
                return sections;
            }

            set { sections = value; }
        }

        public Image SketchImage
        {
            get { return sketchImage; }

            set { sketchImage = value; }
        }

        public PointF SketchOrigin
        {
            get {
                if (sketchOrigin == null)
                {
                    sketchOrigin = new PointF(0, 0);
                }
                return sketchOrigin;
            }

            set { sketchOrigin = value; }
        }

        public double SketchXSize
        {
            get {
                sketchXSize = CalulateSketchXSize();

                return sketchXSize;
            }

            set { sketchXSize = value; }
        }

        public double SketchYSize
        {
            get {
                sketchYSize = CalculateYSize();
                return sketchYSize;
            }

            set { sketchYSize = value; }
        }

        public int SnapshotIndex
        {
            get { return snapshotIndex; }

            set { snapshotIndex = value; }
        }

        public double Stories { get; set; }

        public string StoryEx { get; set; }

        public decimal TotalSqFt
        {
            get { return totalSqFt; }

            set { totalSqFt = value; }
        }

        #endregion "Properties"

        #region "Private Fields"

        private List<SMLine> allSectionLines;
        private bool allSectionsClosed;
        private int card;
        private List<PointF> cornerPoints;
        private List<DrawOnlyLine> drawOnlyLines;
        private string exSketch;
        private string hasSketch;
        private PointF jumpMouseLocation;
        private string lastSectionLetter;
        private string nextSectionLetter;
        private double offsetX;
        private double offsetY;
        private SMParcelMast parcelMast;
        private int record;
        private bool refreshParcel = true;
        private double scale;
        private List<SMSection> sections;
        private Image sketchImage;
        private PointF sketchOrigin;
        private double sketchXSize;
        private double sketchYSize;
        private int snapshotIndex;
        private decimal totalSqFt;

        #endregion "Private Fields"
    }
}