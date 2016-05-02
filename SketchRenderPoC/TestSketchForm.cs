﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
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
        #region Constructor

        public TestSketchForm()

        {
            InitializeComponent();
            this.LocalParcelCopy = ts.TestParcel();
            LocalParcelCopy.SnapShotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            graphics = pctMain.CreateGraphics();
            BluePen = new Pen(Color.DarkBlue, 3);
            RedPen = new Pen(Color.Red, 2);
            firstTimeLoaded = true;
            LocalParcelCopy.SnapShotIndex = 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            RenderSketch();
            graphics.Save();
        }

        private SMParcel GetOriginalParcelFromDb(int record, int dwelling, SketchRepository sr)
        {
            SMParcel parcel = sr.SelectParcelAll(record, dwelling);
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
            LocalParcelCopy = baseParcel;
            LocalParcelCopy.SnapShotIndex = 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
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
            ShowSketchFromBitMap();
        }

        private TestSetup ts = new TestSetup();

        #endregion Constructor

        #region control events

        #region toolStripMenuItems

        private static List<SMLine> LinesWithClosestEndpoints(PointF mouseLocation)
        {
            foreach (SMLine l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s => s.SectionLetter != SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter))
            {
                l.ComparisonPoint = mouseLocation;
            }
            decimal shortestDistance = Math.Round((from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines select l.EndPointDistanceFromComparisonPoint).Min(), 2);
            List<SMLine> connectionLines = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where Math.Round(l.EndPointDistanceFromComparisonPoint, 2) == shortestDistance select l).ToList();
            return connectionLines;
        }

        private void addSectionTsMenu_Click(object sender, EventArgs e)
        {
            JumpToNearestCorner();
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

        private void DrawSectionsOntoBitMap(Graphics graphics, bool v)
        {
            ShowSketchFromBitMap();
        }

        private void drawSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSketchFromBitMap();
            SaveImageForVersion();
        }

        private List<string> GetLegalMoveDirections(PointF scaledJumpPoint, string attachSectionLetter)
        {
            List<SMLine> linesWithJumpPoint = (from l in LocalParcelCopy.AllSectionLines where l.SectionLetter == attachSectionLetter && (l.ScaledStartPoint == scaledJumpPoint || l.ScaledEndPoint == scaledJumpPoint) select l).ToList();
            List<string> legalDirections = new List<string>();
            legalDirections.AddRange((from l in LocalParcelCopy.AllSectionLines where l.ScaledStartPoint == scaledJumpPoint && l.SectionLetter == attachSectionLetter select l.Direction).ToList());

            legalDirections.AddRange((from l in linesWithJumpPoint select l.Direction).Distinct().ToList());
            LegalMoveDirections = legalDirections;
            return legalDirections;
        }

        private void JumpToNearestCorner()
        {
            float mouseX = MousePosition.X - SketchOrigin.X;
            float mouseY = MousePosition.Y - SketchOrigin.Y;
            PointF mouseLocation = MousePosition;
            List<string> connectionSectionLetters = new List<string>();
            string AttSectLtr = "A";
            foreach (SMLine l in LocalParcelCopy.AllSectionLines)
            {
                l.ComparisonPoint = mouseLocation;
            }
            decimal shortestDistance = Math.Round((from l in LocalParcelCopy.AllSectionLines select l.EndPointDistanceFromComparisonPoint).Min(), 2);
            List<SMLine> closestLines = (from l in LocalParcelCopy.AllSectionLines where Math.Round(l.EndPointDistanceFromComparisonPoint, 2) == shortestDistance select l).ToList();
            List<SMLine> connectionLines = LinesWithClosestEndpoints(mouseLocation);

            bool sketchHasLineData = (connectionLines.Count > 0);
            if (connectionLines == null || connectionLines.Count == 0)
            {
                string message = string.Format("No lines contain an available connection point from point {0},{1}", mouseLocation.X, mouseLocation.Y);

                Console.WriteLine(message);

#if DEBUG

                MessageBox.Show(message);
#endif
                throw new InvalidDataException(message);
            }
            else
            {
                connectionSectionLetters = (from l in connectionLines select l.SectionLetter).ToList();
                if (connectionSectionLetters.Count > 1)
                {
                    //AttSectLtr = MultiPointsAvailable(SecLetters);
                    AttachmentSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                    JumpPointLine = (from l in connectionLines where l.SectionLetter == AttSectLtr select l).FirstOrDefault();
                }
                else
                {
                    AttSectLtr = connectionSectionLetters[0];
                    AttachmentSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                    JumpPointLine = connectionLines[0];
                }

                ScaledJumpPoint = JumpPointLine.ScaledEndPoint;
                LegalMoveDirections = GetLegalMoveDirections(ScaledJumpPoint, AttSectLtr);
                MoveCursorToJumpPoint(ScaledJumpPoint);
            }
        }

        private void MoveCursorToJumpPoint(PointF closestCornerPoint)
        {
            Color penColor = Color.Teal;
            PointF dbPoint = SMGlobal.ScaledPointToDbPoint((decimal)closestCornerPoint.X, (decimal)closestCornerPoint.Y, LocalParcelCopy.Scale, SketchOrigin);
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(closestCornerPoint.X);
            int jumpYScaled = Convert.ToInt32(closestCornerPoint.Y);
            Cursor.Position = new Point(jumpXScaled, jumpYScaled);

            //penColor = (_undoMode || draw) ? Color.Red : Color.Black;

            Graphics g = Graphics.FromImage(SketchImage);
            Pen pen1 = new Pen(Color.Red, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 1, 1);
            g.Save();

            pctMain.Image = SketchImage;
            string statusMessage = string.Format("Jump is to point {0},{1} which corresponsed to {2},{3} in the database.", closestCornerPoint.X, closestCornerPoint.Y, dbPoint.X, dbPoint.Y);
            statLblStepInfo.Text = statusMessage;

            //DMouseClick();
        }

        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multipleSectionsAttachment = String.Empty;

            if (sectionLetterList.Count > 1)
            {
                MultiplePoints.Clear();

                MultiSectionSelection attachmentSectionLetterSelected = new MultiSectionSelection(sectionLetterList);
                attachmentSectionLetterSelected.ShowDialog(this);

                multipleSectionsAttachment = MultiSectionSelection.adjsec;

                MultiplePoints = MultiSectionSelection.MultiplePointsDataTable;

               // _hasMultiSection = true;
            }

            return multipleSectionsAttachment;
        }

        private void SaveImageForVersion()
        {
            string callingFile = Assembly.GetExecutingAssembly().FullName;
            FileInfo fi = new FileInfo(callingFile);
            string path = fi.FullName.Replace(string.Format(@"\{0}", fi.Name), string.Empty);
            string imageName = string.Format(@"{0}\Snapshot{1}.png", path, SketchUpGlobals.ParcelWorkingCopy.SnapShotIndex);
            fi = new FileInfo(imageName);
            if (fi.Exists)
            {
                fi.Delete();
            }
            sketchImage.Save(imageName);
        }

        private void ShowSketchFromBitMap()
        {
            SketchImage = new Bitmap(pctMain.Width, pctMain.Height);

            graphics = Graphics.FromImage(SketchImage);

            graphics.Clear(Color.White);
            DrawSections();
            pctMain.Image = SketchImage;
        }

        private void tsbGetSketch_Click(object sender, EventArgs e)
        {
            ShowSketch();
        }

        private Bitmap sketchImage;

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
            LocalParcelCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            GreenBrush = Brushes.MidnightBlue;

            RenderSketch();
            statLblStepInfo.Text = string.Format("Breaking line D-{0} at {1:N2},{2:N2}...", line1Number, breakPoint1.X, breakPoint1.Y);
            LabelLinesOffsetNeg(SectionD, line1Number);
        }

        public void TestBreakLine2()
        {
            SMSection SectionD = BreakSectionLine(sectionLetter, line1Number, breakPoint1, newSectionLetter);
            LocalParcelCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            GreenBrush = Brushes.MidnightBlue;
            statLblStepInfo.Text = string.Format("Breaking line D-{0} at {1:N2},{2:N2}...", line2Number, breakPoint2.X, breakPoint2.Y);
            RenderSketch();

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
            LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;

            SMLineManager lm = new SMLineManager();

            SMSection SectionD =
               SketchUpGlobals.ParcelWorkingCopy.SelectSectionByLetter(sectionLetter);
            List<SMLine> sectionlinesD = SectionD.Lines;

            SMParcel parcel = lm.BreakLine(LocalParcelCopy, sectionLetter, lineNumber,
                  breakPoint1, SketchOrigin, newSectionLetter);
            List<SMLine> sectionDwithBokenLine = (from s in
             parcel.Sections.Where(l => l.SectionLetter == sectionLetter)
                                                  select s).FirstOrDefault<SMSection>().Lines;
            SectionD.Lines = sectionDwithBokenLine;
            parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;
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

        public void ShowSketch()
        {
            graphics.Clear(Color.White);
            DrawSections(true);

            //  graphics.Flush();
            // pctMain.BringToFront();
        }

        private void DrawLabel(SMLine line)
        {
            string label = line.LineLabel;

            Font font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);
            graphics.DrawString(label, font, BlackBrush, labelStartPoint);
        }

        private void DrawLabel(SMLine line, bool showEndpoints)
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
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            graphics.DrawLine(BluePen, drawLineStart, drawLineEnd);
            DrawLabel(line);
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("{0}-{1} {2:N2},{3:N2} to {4:N2},{5:N2}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            traceOut.AppendLine(string.Format("Scaled: {0:N2},{1:N2} to {2:N2},{3:N2}", line.ScaledStartPoint.X, line.ScaledStartPoint.Y, line.ScaledEndPoint.X, line.ScaledEndPoint.Y));
            traceOut.AppendLine(string.Format("Origin: {0:N2},{1:N2}, scale {2:N2}", SketchOrigin.X, SketchOrigin.Y, LocalParcelCopy.Scale));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
        }

        private void DrawLine(Graphics gr, SMLine line, Pen pen)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            gr.DrawLine(pen, drawLineStart, drawLineEnd);
            DrawLabel(line);
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("{0}-{1} {2:N2},{3:N2} to {4:N2},{5:N2}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            traceOut.AppendLine(string.Format("Scaled: {0:N2},{1:N2} to {2:N2},{3:N2}", line.ScaledStartPoint.X, line.ScaledStartPoint.Y, line.ScaledEndPoint.X, line.ScaledEndPoint.Y));
            traceOut.AppendLine(string.Format("Origin: {0:N2},{1:N2}, scale {2:N2}", SketchOrigin.X, SketchOrigin.Y, LocalParcelCopy.Scale));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
        }

        private void DrawLine(Graphics gr, SMLine line, Pen pen, bool omitLabel = true)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            graphics.DrawLine(pen, drawLineStart, drawLineEnd);
            if (!omitLabel)
            {
                DrawLabel(line, omitLabel);
            }
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("{0}-{1} {2:N2},{3:N2} to {4:N2},{5:N2}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            traceOut.AppendLine(string.Format("Scaled: {0:N2},{1:N2} to {2:N2},{3:N2}", line.ScaledStartPoint.X, line.ScaledStartPoint.Y, line.ScaledEndPoint.X, line.ScaledEndPoint.Y));
            traceOut.AppendLine(string.Format("Origin: {0:N2},{1:N2}, scale {2:N2}", SketchOrigin.X, SketchOrigin.Y, LocalParcelCopy.Scale));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
        }

        private void DrawLine(SMLine line, bool omitLabel = true)
        {
            //PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            //PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            graphics.DrawLine(BluePen, drawLineStart, drawLineEnd);
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("{0}-{1} {2:N2},{3:N2} to {4:N2},{5:N2}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            traceOut.AppendLine(string.Format("Scaled: {0:N2},{1:N2} to {2:N2},{3:N2}", line.ScaledStartPoint.X, line.ScaledStartPoint.Y, line.ScaledEndPoint.X, line.ScaledEndPoint.Y));
            traceOut.AppendLine(string.Format("Origin: {0:N2},{1:N2}, scale {2:N2}", SketchOrigin.X, SketchOrigin.Y, LocalParcelCopy.Scale));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
            if (!omitLabel)
            {
                DrawLabel(line);
            }
        }

        private void DrawSections()
        {
            if (LocalParcelCopy == null)
            {
                RenderSketch();
            }
            if (LocalParcelCopy.Sections != null)
            {
                foreach (SMSection section in LocalParcelCopy.Sections.OrderBy(l => l.SectionLetter))
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

        private void DrawSections(bool ShowPoints = false)
        {
            if (parcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (LocalParcelCopy.Sections != null)
            {
                foreach (SMSection section in LocalParcelCopy.Sections.OrderBy(l => l.SectionLetter))
                {
                    if (section.Lines != null)
                    {
                        foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
                        {
                            DrawLine(l);
                            if (ShowPoints)
                            {
                                ShowPoint(string.Format("{0}{1}\nbeg\n{2:N1},{3:N1}", l.SectionLetter, l.LineNumber, l.StartX, l.StartY), l.ScaledStartPoint);
                                ShowPoint(string.Format("{0}{1}\nend\n{2:N1},{3:N1}", l.SectionLetter, l.LineNumber, l.EndX, l.EndY), l.EndPoint);
                            }
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
            if (LocalParcelCopy.Sections != null)
            {
                SMSection selectedSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();

                if (selectedSection.Lines != null)
                {
                    foreach (SMLine l in selectedSection.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, false);
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
                comparisons.Add(new SMPointComparer { ComparisonLine = l, ComparisonPoint = referencePoint, SketchOrigin = SketchOrigin, Scale = LocalParcelCopy.Scale });
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
            if (LocalParcelCopy != null && LocalParcelCopy.Sections != null)
            {
                decimal sketchScale = LocalParcelCopy.Scale;
                foreach (SMSection s in LocalParcelCopy.Sections)
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
            foreach (SMSection section in LocalParcelCopy.Sections)
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
            var extraWidth = (pctMain.Width - 20) - (LocalParcelCopy.Scale * LocalParcelCopy.SketchXSize);
            var extraHeight = (pctMain.Height - 20) - (parcelWorkingCopy.Scale * LocalParcelCopy.SketchYSize);
            var paddingX = (extraWidth / 2) + 10;
            var paddingY = (extraHeight / 2) + 10;
            var xLocation = (LocalParcelCopy.OffsetX * LocalParcelCopy.Scale) + paddingX;
            var yLocation = (LocalParcelCopy.OffsetY * LocalParcelCopy.Scale) + paddingY;

            SketchOrigin = new PointF((float)xLocation, (float)yLocation);
#if DEBUG
            Console.WriteLine("Sketch Origin is at {0},{1}", SketchOrigin.X, SketchOrigin.Y);
#endif
        }

        private void SetSketchScale()
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = pctMain.Height - 20;
            int boxWidth = pctMain.Width - 20;
            decimal xScale = Math.Floor(boxWidth / LocalParcelCopy.SketchXSize);
            decimal yScale = Math.Floor(boxHeight / LocalParcelCopy.SketchYSize);
            LocalParcelCopy.Scale = (decimal)SMGlobal.SmallerDouble(xScale, yScale) * 0.75M;
        }

        private void ShowNearestCorners(PointF mouseLocation)
        {
            List<SMPointComparer> pointDistances = PointDistances(mouseLocation, LocalParcelCopy.AllSectionLines);
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

        private void ShowPoint(string pointLabel, PointF pointToLabel)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(BluePen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, new PointF(pointToLabel.X - 16, pointToLabel.Y - 16));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X, pointToLabel.Y - 14), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(BluePen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset, Pen pen)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(pen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        #endregion Private Methods

        #region Properties

        public SMSection AttachmentSection
        {
            get;
            private set;
        }

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

        public SMLine JumpPointLine
        {
            get;
            private set;
        }

        public List<string> LegalMoveDirections
        {
            get;
            private set;
        }

        public SMParcel LocalParcelCopy
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

        public DataTable MultiplePoints
        {
            get;
            private set;
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

        public PointF ScaledJumpPoint
        {
            get;
            private set;
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

        public Bitmap SketchImage
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
                return sketchOrigin;
            }

            set
            {
                sketchOrigin = value;
            }
        }

        public int MouseX
        {
            get
            {
                return _mouseX;
            }

            set
            {
                _mouseX = value;
            }
        }

        public int MouseY
        {
            get
            {
                return _mouseY;
            }

            set
            {
                _mouseY = value;
            }
        }

        private Pen orangePen;

        #endregion Properties

        private void BreakAndRejoinD3()
        {
            //First break the lines
            SMLineManager lm = new SMLineManager();
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", LocalParcelCopy.SnapShotIndex));
            foreach (SMLine line in LocalParcelCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4} Att: {5}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY, line.AttachedSection));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));

            TestBreakLine1();
            statLblStepInfo.Text = "D-3 is now D3 & D4, D4 is now D5.";
            traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", LocalParcelCopy.SnapShotIndex));
            foreach (SMLine line in LocalParcelCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4} Att: {5}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY, line.AttachedSection));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));

            //    CleanUpBrokenLines(lm);
            graphics.Clear(Color.White);
            RenderSketch();
        }

        private void CleanUpBrokenLines(SMLineManager lm)
        {
            List<SMSection> sectionsList = new List<SMSection>();

            //Go through the sections and combine any lines that can be combined.
            foreach (SMSection section in LocalParcelCopy.Sections)
            {
                sectionsList.Add(ReorganizedSection(lm, section));
#if DEBUG

#endif
            }
            LocalParcelCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);

            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", LocalParcelCopy.SnapShotIndex));
            foreach (SMLine line in LocalParcelCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));
            LocalParcelCopy.SnapShotIndex++;
            LocalParcelCopy.Sections = sectionsList;
            SketchUpGlobals.SketchSnapshots.Add(parcelWorkingCopy);
        }

        private void cmenuJump_Click(object sender, EventArgs e)
        {
           
            //PointF jumpPointScaled = MousePosition;
            MoveCursorToJumpPoint(MousePosition);
        }

        private void DeleteSketch()
        {
            DialogResult response = MessageBox.Show("If you delete this sketch, you will have to rebuild it from scratch. This action cannot be undone. Proceed?", "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (response == DialogResult.OK)
            {
                DeleteSketchData(LocalParcelCopy);
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

        private void ShowParcelLineByVersionInDataGrid()
        {
            RenderSketch();
            statLblStepInfo.Text = string.Format("Listing Versions...");

            foreach (SMParcel p in SketchUpGlobals.SketchSnapshots)
            {
                statLblStepInfo.Text = string.Format("Listing Version {0}...", p.SnapShotIndex);
                foreach (SMLine l in p.AllSectionLines)
                {
                    snapshotsDgv.Rows.Add(p.SnapShotIndex, l.SectionLetter, l.LineNumber, string.Format("{0:N2},{1:N2}", l.StartX, l.StartY), string.Format("{0:N2},{1:N2}", l.EndX, l.EndY), l.AttachedSection);
                }
            }
        }

        private void TestSketchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            graphics.Dispose();
        }

        private void toolStripMenuCombinLinesInD_Click(object sender, EventArgs e)
        {
            BreakAndRejoinD3();
        }

        private void tsmAllTests_Click(object sender, EventArgs e)
        {
            TestBreakLine1();

            //    TestBreakLine2();

            ShowParcelLineByVersionInDataGrid();
        }

        private void tsMenuExitForm_Click(object sender, EventArgs e)
        {
            if (graphics != null)
            {
                graphics.Dispose();
            }
            this.Dispose();
            this.Close();
        }

        private void tsmListParcelSnapshots_Click(object sender, EventArgs e)
        {
            ShowParcelLineByVersionInDataGrid();
        }

       // TODO: Remove if not needed:	 private bool _hasMultiSection;
        private PointF breakPoint1 = new PointF(-10, -2);
        private PointF breakPoint2 = new PointF(0, -12);
        private bool firstTimeLoaded = false;
        private int line1Number = 3;
        private int line2Number = 5;
        private string newSectionLetter = "G";
        private string pointLabel = string.Empty;
        private string sectionLetter = "D";
        private int _mouseX;
        private int _mouseY;

        private void pctMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseX = e.X;
                MouseY = e.Y;

                
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
            
                MouseX = e.X;
                MouseY = e.Y;
            
            }
        }
    }
}