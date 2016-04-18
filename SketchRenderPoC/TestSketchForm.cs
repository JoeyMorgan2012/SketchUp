using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SketchUp;
using SWallTech;

namespace SketchRenderPoC
{
    public partial class TestSketchForm : Form
    {
        private string sectionLetter = "D";
        private int line1Number = 3;
        private int line2Number = 5;
        private string newSectionLetter = "G";
        private PointF breakPoint1 = new PointF(-10, -2);
        private PointF breakPoint2 = new PointF(0, -12);
        private string pointLabel = string.Empty;

        #region Constructor

        private TestSetup ts = new TestSetup();

        public TestSketchForm()

        {
            //TODO: Change this so the parcel data is loaded with the main form, index of 0, and each successive index is one greater, so each "version" is stored in the list.
            InitializeComponent();
            this.ParcelWorkingCopy = ts.TestParcel();
            SketchUpGlobals.SketchSnapshots.Add(ParcelWorkingCopy);
            graphics = pctMain.CreateGraphics();
            BluePen = new Pen(Color.DarkBlue, 3);
            RedPen = new Pen(Color.Red, 2);
            firstTimeLoaded = true;
            RenderSketch();
        }

        private SMParcel GetOriginalParcelFromDb(int record, int dwelling, SketchRepository sr)
        {
            SMParcel parcel = sr.SelectParcelData(record, dwelling);
            parcel.Sections = sr.SelectParcelSections(parcel);
            foreach (SMSection sms in parcel.Sections)
            {
                sms.Lines = sr.SelectSectionLines(sms);
            }
            parcel.IdentifyAttachedToSections();
            return parcel;
        }

        private SMParcel GetSelectedParcelData()
        {
            string dataSource = SketchUp.Properties.Settings.Default.IPAddress;
            string password = SketchUp.Properties.Settings.Default.UserName;
            string userName = SketchUp.Properties.Settings.Default.Password;
            string locality = SketchUpGlobals.LocalityPreFix;
            int record = SketchUpGlobals.Record;
            int dwelling = SketchUpGlobals.Card;

            SketchRepository sr = new SketchRepository(dataSource, userName, password, locality);
            SMParcel parcel = GetOriginalParcelFromDb(record, dwelling, sr);
            return parcel;
        }

        private SketchRepository GetSketchRepository()
        {
            try
            {
                SketchRepository sr = new SketchRepository(SketchUpGlobals.CamraDbConn.DataSource, SketchUpGlobals.CamraDbConn.User, SketchUpGlobals.CamraDbConn.Password, SketchUpGlobals.LocalityPreFix);
                return sr;
            }
            catch (Exception ex)
            {
                string message = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Console.WriteLine(message);
#if DEBUG
                MessageBox.Show(message);
#endif
                throw;
            }
        }

        private void InitializeParcelSnapshots()
        {
            SketchRepository sketchRepo = GetSketchRepository();
            SMParcel baseParcel = GetOriginalParcelFromDb(SketchUpGlobals.Record,
            SketchUpGlobals.Card, sketchRepo);
            baseParcel.SnapShotIndex = 0;
            SketchUpGlobals.SMParcelFromData = baseParcel;
            SMParcel workingCopy = baseParcel;
            workingCopy.SnapShotIndex = 1;
            SketchUpGlobals.SketchSnapshots.Add(workingCopy);
        }

        private void MockGettingCamraData()
        {
            ParcelDataCollection pdc = new ParcelDataCollection(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);
            SketchUpGlobals.CurrentParcel = pdc.GetParcel(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);
        }

        private void RenderSketch()
        {
            SetSketchScale();
            SetSketchOrigin();
            SetScaledStartPoints();
            SetSectionCenterPoints();

            DrawSections();
        }

        #endregion Constructor

        #region control events

        #region toolStripMenuItems

        private void addSectionTsMenu_Click(object sender, EventArgs e)
        {
            TestBreakLine1();
        }

        private void changeSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "change section");
            MessageBox.Show(message);
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "deleteSection");
            MessageBox.Show(message);
        }

        private void deleteSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSketch();
        }

        private void drawSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSketch();
        }

        private void tsbGetSketch_Click(object sender, EventArgs e)
        {
            ShowSketch();
        }

        #endregion toolStripMenuItems

        private void pctMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PointF mouseLocation = new PointF(e.X, e.Y);
            ShowNearestCorners(mouseLocation);
        }

        private void pctMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseLocationLabel.Text = string.Format("({0},{1})", e.X, e.Y);
        }

        #endregion control events

        #region Debug only

        private void DebugOnlyDisplayResults()
        {
#if DEBUG
            graphics.DrawRectangle(BluePen, SketchOrigin.X, SketchOrigin.Y, 2, 2);
            Brush redBrush = Brushes.Red;
            Font textFont = new Font("Arial", 12);
            graphics.DrawString(string.Format("Origin: {0},{1}", SketchOrigin.X, SketchOrigin.Y), textFont, redBrush, SketchOrigin.X + 10, SketchOrigin.Y + 10);
            graphics.DrawString(string.Format("Scale: {0}", ParcelWorkingCopy.Scale), textFont, redBrush, SketchOrigin.X + 10, SketchOrigin.Y + 20);
#endif
        }

        #endregion Debug only

        #region Fields

        private Brush blackBrush;
        private Brush blueBrush;
        private Pen bluePen;
        private Graphics graphics;
        private GraphicsPath graphicsPath = new GraphicsPath();
        private Brush greenBrush;
        private SMParcel parcelWorkingCopy;
        private Brush redBrush;
        private Pen redPen;
        private SMParcel selectedParcel;
        private PointF sketchOrigin;

        #endregion Fields

        #region Private Methods

        #region Testing code

        //Todo: Eliminate before release

        private static void InitializeGlobals(string dataSource, string password, string userName, string locality, int record, int dwelling)
        {
            SMConnection conn = new SMConnection(dataSource, userName, password, locality);
            SketchUpGlobals.CamraDbConn = conn.DbConn;
            SketchUpGlobals.CamraDbConn.OpenDbConnection();
            SketchUpGlobals.IpAddress = dataSource;

            //TODO: !! Figure out where this should be set and read
            SketchUpGlobals.LocalLib = "NATIVE";
            SketchUpGlobals.LocalityPreFix = locality;
            SketchUpGlobals.FcLib = SketchUpGlobals.LocalLib;
            SketchUpGlobals.FcLocalityPrefix = locality;
            SketchUpGlobals.Record = record;
            SketchUpGlobals.Card = dwelling;
        }

        //private void GetSelectedParcelData()
        //{
        //    string dataSource = SketchUp.Properties.Settings.Default.IPAddress;
        //    string password = SketchUp.Properties.Settings.Default.UserName;
        //    string userName = SketchUp.Properties.Settings.Default.Password;
        //    string locality = "AUG";
        //    int record = 11787;
        //    int dwelling = 1;

        //    SketchRepository sr = new SketchRepository(dataSource, userName, password, locality);
        //    SelectedParcel = GetParcel(record, dwelling, sr);
        //    SelectedParcel.SnapShotIndex = 0;
        //    ParcelWorkingCopy = SelectedParcel;
        //    ParcelWorkingCopy.SnapShotIndex = 1;

        //}

        #endregion Testing code

        #region Tests

        public void TestBreakLine1()
        {
            SMSection SectionD = BreakSectionLine(sectionLetter, line1Number, breakPoint1, newSectionLetter);
            ParcelWorkingCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(ParcelWorkingCopy);
            GreenBrush = Brushes.MidnightBlue;

            RenderSketch();
            statLblStepInfo.Text = "Breaking line D-3...";
            LabelLinesOffsetNeg(SectionD, line1Number);
        }

        public void TestCombinabilityOfLines()
        {
            string sectionLetter = "D";
            int line1Number = 3;
            statLblStepInfo.Text = "Combining Lines D3 & D4 again.";
            SMLineManager lm = new SMLineManager();
            string newSectionLetter = "G";
            PointF breakPoint1 = new PointF(-10, -2);
            PointF breakPoint2 = new PointF(0, -12);
            string pointLabel = string.Empty;
            SMSection SectionD = BreakSectionLine(sectionLetter, line1Number, breakPoint1, newSectionLetter);
            RenderSketch();
            int lastLineNumber = (from l in SectionD.Lines select l.LineNumber).Max();
            foreach (SMLine line in SectionD.Lines)
            {
                if (line.LineNumber < lastLineNumber)
                {
                    SMLine nextLine = (from l in SectionD.Lines where l.LineNumber == line.LineNumber + 1 select l).FirstOrDefault();
                    Console.WriteLine(string.Format("Comparing lines {0} and {1}", line.LineNumber, nextLine.LineNumber));
                    bool canCombine = lm.LinesCanBeCombined(line, nextLine);
                    Console.WriteLine(string.Format("Lines {0} and {1} {2} be combined.", line.LineNumber, nextLine.LineNumber, canCombine ? "can" : "cannot"));
                }
                else
                {
                    SMLine nextLine = (from l in SectionD.Lines where l.LineNumber == 1 select l).FirstOrDefault();
                    Console.WriteLine(string.Format("Comparing lines {0} and {1}", line.LineNumber, nextLine.LineNumber));
                    bool canCombine = lm.LinesCanBeCombined(line, nextLine);
                    Console.WriteLine(string.Format("Lines {0} and {1} {2} be combined.", line.LineNumber, nextLine.LineNumber, canCombine ? "can" : "cannot"));
                }
            }
        }

        private SMSection BreakSectionLine(string sectionLetter, int lineNumber, PointF breakPoint, string newSectionLetter = "")
        {
            ParcelWorkingCopy = ts.TestParcel();
            ParcelWorkingCopy.SnapShotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(ParcelWorkingCopy);
            SMLineManager lm = new SMLineManager();

            SMSection SectionD =
               ParcelWorkingCopy.SelectSectionByLetter(sectionLetter);
            List<SMLine> sectionlinesD = SectionD.Lines;

            SMParcel parcel = lm.BreakLine(ParcelWorkingCopy, sectionLetter, lineNumber,
                  breakPoint1, SketchOrigin, newSectionLetter);
            List<SMLine> sectionDwithBokenLine = (from s in
               SketchUpGlobals.ParcelWorkingCopy.Sections.Where(l => l.SectionLetter == sectionLetter)
                                                  select s).FirstOrDefault<SMSection>().Lines;
            SectionD.Lines = sectionDwithBokenLine;
            int currentSnapShotNumber = (from p in SketchUpGlobals.SketchSnapshots select p.SnapShotIndex).Max();
            parcel.SnapShotIndex = currentSnapShotNumber + 1;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            parcelWorkingCopy = parcel;
            return SectionD;
        }

        private void LabelLinesOffsetNeg(SMSection selectedSection, int startWithLine = 1)
        {
            string pointLabel;
            foreach (SMLine l in selectedSection.Lines.Where(n => n.LineNumber >= startWithLine))
            {
                pointLabel = string.Format("Line {0} start {1},{2}", l.LineNumber, l.StartPoint.X, l.StartPoint.Y);
                ShowPoint(pointLabel, l.ScaledStartPoint, new SizeF(-30, -10), new Pen(Color.MidnightBlue, 1));

                pointLabel = string.Format("Line {0} end {1},{2}", l.LineNumber, l.EndPoint.X, l.EndPoint.Y);
                ShowPoint(pointLabel, l.ScaledEndPoint, new SizeF(-30, -30), new Pen(Color.MidnightBlue, 1));
            }
        }

        private void LabelLinesOffsetPos(SMSection selectedSection, int startWithLine = 1)
        {
            string pointLabel;
            foreach (SMLine l in selectedSection.Lines.Where(n => n.LineNumber >= startWithLine))
            {
                pointLabel = string.Format("Line {0} start {1},{2}", l.LineNumber, l.StartPoint.X, l.StartPoint.Y);
                ShowPoint(pointLabel, l.ScaledStartPoint, new SizeF(12, 10), new Pen(Color.MidnightBlue, 1));

                pointLabel = string.Format("Line {0} end {1},{2}", l.LineNumber, l.EndPoint.X, l.EndPoint.Y);
                ShowPoint(pointLabel, l.ScaledEndPoint, new SizeF(12, 30), new Pen(Color.MidnightBlue, 1));
            }
        }

        #endregion Tests

        private void DrawLabel(SMLine line)
        {
            string label = line.LineLabel;

            Font font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);
            graphics.DrawString(label, font, BlackBrush, labelStartPoint);
        }

        private void DrawLabel(SMSection section)
        {
            string label = section.SectionLabel;

            Font font = new Font("Segoe UI", 10, FontStyle.Bold, GraphicsUnit.Point);
            int labelLength = (int)section.SectionLabel.Length;

            PointF labelLocation = section.ScaledSectionCenter;

            graphics.DrawString(label, font, RedBrush, labelLocation);
        }

        private void DrawLine(SMLine line)
        {
            //PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            //PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            graphics.DrawLine(BluePen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, Pen pen)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            graphics.DrawLine(pen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawSections()
        {
            if (parcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (ParcelWorkingCopy.Sections != null)
            {
                foreach (SMSection section in ParcelWorkingCopy.Sections.OrderBy(l => l.SectionLetter))
                {
                    if (section.Lines != null)
                    {
                        foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
                        {
                            DrawLine(l);
                        }
                    }
                    DrawLabel(section);
                }
            }
        }

        private void DrawSections(string sectionLetter)
        {
            if (parcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (ParcelWorkingCopy.Sections != null)
            {
                SMSection selectedSection = (from s in ParcelWorkingCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();

                if (selectedSection.Lines != null)
                {
                    foreach (SMLine l in selectedSection.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l);
                    }
                }
            }
        }

        private void LabelSection(SMSection section)
        {
        }

        private List<SMPointComparer> PointDistances(PointF referencePoint, List<SMLine> lines)

        {
            List<SMPointComparer> comparisons = new List<SMPointComparer>();
            foreach (SMLine l in lines)
            {
                comparisons.Add(new SMPointComparer { ComparisonLine = l, ComparisonPoint = referencePoint, SketchOrigin = SketchOrigin, Scale = ParcelWorkingCopy.Scale });
            }
            return comparisons;
        }

        private PointF SectionLabelPlacementPoint(SMSection section)
        {
            //Get the origin and its diagonal points
            SMLine firstLine = (from l in section.Lines where l.LineNumber == 1 select l).FirstOrDefault<SMLine>();

            PointF labelStartPoint = new PointF();
            return labelStartPoint;
        }

        private void SetScaledStartPoints()
        {
            if (ParcelWorkingCopy != null && ParcelWorkingCopy.Sections != null)
            {
                decimal sketchScale = ParcelWorkingCopy.Scale;
                foreach (SMSection s in ParcelWorkingCopy.Sections)
                {
                    foreach (SMLine line in s.Lines)
                    {
                        var lineStartX = (float)((line.StartX * sketchScale) + (decimal)SketchOrigin.X);
                        var lineStartY = (float)((line.StartY * sketchScale) + (decimal)SketchOrigin.Y);
                        line.ScaledStartPoint = new PointF(lineStartX, lineStartY);
                    }
                }
            }
        }

        private void SetSectionCenterPoints()
        {
            List<PointF> sectionPoints = new List<PointF>();
            foreach (SMSection section in ParcelWorkingCopy.Sections)
            {
                sectionPoints = new List<PointF>();
                foreach (SMLine line in section.Lines)
                {
                    sectionPoints.Add(line.ScaledStartPoint);
                    sectionPoints.Add(line.ScaledEndPoint);
                }
                PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
                section.ScaledSectionCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(0, -12));
            }
        }

        private void SetSketchOrigin()
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            var sketchAreaWidth = pctMain.Width - 20;
            var sketchAreaHeight = pctMain.Height - 20;

            PointF pictureBoxCorner = pctMain.Location;
            var extraWidth = (pctMain.Width - 20) - (ParcelWorkingCopy.Scale * ParcelWorkingCopy.SketchXSize);
            var extraHeight = (pctMain.Height - 20) - (parcelWorkingCopy.Scale * ParcelWorkingCopy.SketchYSize);
            var paddingX = (extraWidth / 2) + 10;
            var paddingY = (extraHeight / 2) + 10;
            var xLocation = (ParcelWorkingCopy.OffsetX * ParcelWorkingCopy.Scale) + paddingX;
            var yLocation = (ParcelWorkingCopy.OffsetY * ParcelWorkingCopy.Scale) + paddingY;

            SketchOrigin = new PointF((float)xLocation, (float)yLocation);
        }

        private void SetSketchScale()
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = pctMain.Height - 20;
            int boxWidth = pctMain.Width - 20;
            decimal xScale = Math.Floor(boxWidth / ParcelWorkingCopy.SketchXSize);
            decimal yScale = Math.Floor(boxHeight / ParcelWorkingCopy.SketchYSize);
            ParcelWorkingCopy.Scale = (decimal)SMGlobal.SmallerDouble(xScale, yScale);
        }

        private void ShowNearestCorners(PointF mouseLocation)
        {
            List<SMPointComparer> pointDistances = PointDistances(mouseLocation, ParcelWorkingCopy.AllSectionLines);
            decimal closestDistance = (from d in pointDistances select d.EndPointDistance).Min();
            List<SMLine> nearestLines = (from l in pointDistances where l.EndPointDistance == closestDistance select l.ComparisonLine).ToList();
            Brush violetBrush = Brushes.DarkViolet;
            Font font = new Font("Lucida Sans Unicode", 10, FontStyle.Bold, GraphicsUnit.Point);
            foreach (SMLine l in nearestLines)
            {
                PointF location = PointF.Add(l.ScaledEndPoint, new SizeF(SketchOrigin));

                //DrawLine(l, new Pen(Color.DarkGreen, 6));
                graphics.DrawString("*", font, violetBrush, location);
                graphics.DrawEllipse(new Pen(Color.Green), (new RectangleF(location, new SizeF(2, 2))));
            }
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(BluePen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X - 16, pointToLabel.Y - 16), labelOffset));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset, Pen pen)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(pen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X - 16, pointToLabel.Y - 16), labelOffset));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(BluePen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, new PointF(pointToLabel.X - 16, pointToLabel.Y - 16));
        }

        public void ShowSketch()
        {
            DrawSections();
            graphics.Flush();
            pctMain.BringToFront();
        }

        #endregion Private Methods

        #region Properties

        private Pen orangePen;

        public Brush BlackBrush
        {
            get
            {
                blackBrush = Brushes.Black;
                return blackBrush;
            }

            set
            {
                blackBrush = value;
            }
        }

        public Brush BlueBrush
        {
            get
            {
                blueBrush = Brushes.DarkBlue;
                return blueBrush;
            }

            set
            {
                blueBrush = value;
            }
        }

        public Pen BluePen
        {
            get
            {
                if (bluePen == null)
                {
                    bluePen = new Pen(Color.DarkBlue, 1);
                }

                return bluePen;
            }

            set
            {
                bluePen = value;
            }
        }

        public bool FirstTimeLoaded
        {
            get
            {
                return firstTimeLoaded;
            }

            set
            {
                firstTimeLoaded = value;
            }
        }

        public Brush GreenBrush
        {
            get
            {
                greenBrush = Brushes.DarkGreen;
                return greenBrush;
            }

            set
            {
                greenBrush = value;
            }
        }

        public Pen OrangePen
        {
            get
            {
                if (orangePen == null)
                {
                    orangePen = new Pen(Color.DarkOrange, 1);
                }
                return orangePen;
            }

            set
            {
                orangePen = value;
            }
        }

        public SMParcel ParcelWorkingCopy
        {
            get
            {
                return parcelWorkingCopy;
            }

            set
            {
                parcelWorkingCopy = value;
            }
        }

        public Brush RedBrush
        {
            get
            {
                redBrush = Brushes.DarkRed;
                return redBrush;
            }

            set
            {
                redBrush = value;
            }
        }

        public Pen RedPen
        {
            get
            {
                if (redPen == null)
                {
                    redPen = new Pen(Color.Red, 1);
                }
                return redPen;
            }

            set
            {
                redPen = value;
            }
        }

        public SMParcel SelectedParcel
        {
            get
            {
                return selectedParcel;
            }

            set
            {
                selectedParcel = value;
            }
        }

        public PointF SketchOrigin
        {
            get
            {
                return sketchOrigin;
            }

            set
            {
                sketchOrigin = value;
            }
        }

        #endregion Properties

        private void DeleteSketch()
        {
            DialogResult response = MessageBox.Show("If you delete this sketch, you will have to rebuild it from scratch. This action cannot be undone. Proceed?", "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (response == DialogResult.OK)
            {
                DeleteSketchData(ParcelWorkingCopy);
            }
        }

        private void DeleteSketchData(SMParcel parcelWorkingCopy)
        {
            MessageBox.Show("Deleting Sections");
        }

        private void drawingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void editSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestCombinabilityOfLines();
        }

        private void EditSketchForm_Paint(object sender, PaintEventArgs e)
        {
            ShowSketch();
            graphics.Flush();
        }

        private void flipHorizontalMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "Flip Sketch horizontally");
            MessageBox.Show(message);
        }

        private void flipVerticalMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "Flip Sketch vertically");
            MessageBox.Show(message);
        }

        private void tsMenuExitForm_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "Exit Sketch Form");
            MessageBox.Show(message);
        }

        private bool firstTimeLoaded = false;

        private void toolStripMenuCombinLinesInD_Click(object sender, EventArgs e)
        {
            BreakAndRejoinD3();
        }

        private void BreakAndRejoinD3()
        {
            //First break the lines
            SMLineManager lm = new SMLineManager();
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", ParcelWorkingCopy.SnapShotIndex));
            foreach (SMLine line in ParcelWorkingCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4} Att: {5}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY, line.AttachedSection));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));

            TestBreakLine1();
            statLblStepInfo.Text = "D-3 is now D3 & D4, D4 is now D5.";
            traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", ParcelWorkingCopy.SnapShotIndex));
            foreach (SMLine line in ParcelWorkingCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4} Att: {5}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY, line.AttachedSection));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));

            CleanUpBrokenLines(lm);
            graphics.Clear(Color.White);
            RenderSketch();
        }

        private void CleanUpBrokenLines(SMLineManager lm)
        {
            List<SMSection> sectionsList = new List<SMSection>();
          
            //Go through the sections and combine any lines that can be combined.
            foreach (SMSection section in ParcelWorkingCopy.Sections)
            {
                sectionsList.Add(ReorganizedSection(lm, section));
#if DEBUG

#endif
            }
            ParcelWorkingCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(ParcelWorkingCopy);

            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", ParcelWorkingCopy.SnapShotIndex));
            foreach (SMLine line in ParcelWorkingCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));
            ParcelWorkingCopy.SnapShotIndex++;
            ParcelWorkingCopy.Sections = sectionsList;
            SketchUpGlobals.SketchSnapshots.Add(parcelWorkingCopy);
        }

        private SMSection ReorganizedSection(SMLineManager lm, SMSection section)
        {
            SMSection reorganizedSection = section;
            List<SMLine> linesBefore = new List<SMLine>();
            List<SMLine> linesAfter = new List<SMLine>();
            List<SMLine> sectionLines = section.Lines;
            int lastLineNumber = (from l in sectionLines select l.LineNumber).Max();
            for (int i = 0; i < sectionLines.Count - 1; i++)
            {
                SMLine line = sectionLines[i];

                SMLine nextLine = sectionLines[i + 1];
                bool canCombine = lm.LinesCanBeCombined(line, nextLine);
                if (canCombine)
                {
                    linesBefore.Clear();
                    linesAfter.Clear();

                    linesBefore = (from l in sectionLines where l.LineNumber < line.LineNumber select l).ToList();

                    foreach (SMLine l in (from l in sectionLines where l.LineNumber > nextLine.LineNumber select l).ToList())
                    {
                        SMLine renumberedLine = l;
                        renumberedLine.LineNumber -= 1;
                        linesAfter.Add(renumberedLine);
                    }
                    reorganizedSection.Lines.Clear();
                    reorganizedSection.Lines.AddRange(linesBefore);
                    reorganizedSection.Lines.Add(lm.CombinedLines(line, nextLine));
                    reorganizedSection.Lines.AddRange(linesAfter);
                }
            }
            return reorganizedSection;
        }

        private void tsmListParcelSnapshots_Click(object sender, EventArgs e)
        {
            ShowParcelLineByVersionInDataGrid();
        }

        private void ShowParcelLineByVersionInDataGrid()
        {
            foreach (SMParcel p in SketchUpGlobals.SketchSnapshots)
            {
                foreach (SMLine l in p.AllSectionLines)
                {
                    dataGridView1.Rows.Add(p.SnapShotIndex, l.SectionLetter, l.LineNumber, string.Format("{0:N2},{1:N2}", l.StartX, l.StartY), string.Format("{0:N2},{1:N2}", l.EndX, l.EndY), l.AttachedSection);

                }
            }
        }

        private void tsmAllTests_Click(object sender, EventArgs e)
        {
            TestBreakLine1();
            BreakAndRejoinD3();
            ShowParcelLineByVersionInDataGrid();
        }
    }
}