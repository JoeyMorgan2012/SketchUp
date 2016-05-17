using System;
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
using SWallTech;

namespace SketchUp
{
    public partial class TestSketchForm : Form
    {
        #region Constructor

        //private void RenderSketch(PictureBox target)
        //{
        //    SetSketchScale(target);
        //    SetSketchOrigin(target);
        //    SetScaledStartPoints();
        //    SetSectionCenterPoints();
        //    ShowSketchFromBitMap();
        //}

        public TestSketchForm()

        {
            InitializeComponent();
            LocalParcelCopy = ts.TestParcel();
            LocalParcelCopy.SnapShotIndex = 0;
            PictureBox targetContainer = sketchBox;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);

            graphics = targetContainer.CreateGraphics();
            BluePen = new Pen(Color.DarkBlue, 3);
            RedPen = new Pen(Color.Red, 2);
            firstTimeLoaded = true;
            LocalParcelCopy.SnapShotIndex = 1;

            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
        }

        private SMParcel GetOriginalParcelFromDb(int record, int dwelling, SketchRepository sr)
        {
            SMParcel parcel = sr.SelectParcelMasterWithParcel(record, dwelling).Parcel;

            parcel.IdentifyAttachedToSections();
            return parcel;
        }

        private SMParcel GetSelectedParcelData()
        {
            string dataSource = SketchUp.Properties.Settings.Default.IPAddress;
            string password = SketchUp.Properties.Settings.Default.UserName;
            string userName = SketchUp.Properties.Settings.Default.Password;
            string locality = SketchUpGlobals.LocalityPrefix;
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
                SketchRepository sr = new SketchRepository(SketchUpGlobals.CamraDbConn.DataSource, SketchUpGlobals.CamraDbConn.User, SketchUpGlobals.CamraDbConn.Password, SketchUpGlobals.LocalityPrefix);
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

        private int mouseDownX;
        private int mouseDownY;
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

        private void AddNewTestSection()
        {
            //Now add a section and some lines and do not close
            SMSection newSection = new SMSection(LocalParcelCopy);
            newSection.SectionType = "DECK";
            newSection.SectionLetter = LocalParcelCopy.NextSectionLetter;
            SMLine line1 = new SMLine(newSection);
            line1.StartX = 63;
            line1.StartY = 3;
            line1.EndX = 73;
            line1.EndY = 3;
            line1.SectionLetter = newSection.SectionLetter;
            line1.Direction = "E";
            line1.LineNumber = 1;
            line1.XLength = 10;
            line1.YLength = 0;
            line1.ParentParcel = LocalParcelCopy;
            newSection.Lines.Add(line1);

            SMLine line2 = new SMLine(newSection);
            line2.StartX = 73;
            line2.StartY = 3;
            line2.EndX = 73;
            line2.EndY = -7;
            line2.SectionLetter = newSection.SectionLetter;
            line2.Direction = "N";
            line2.LineNumber = 2;
            line2.XLength = 0;
            line2.YLength = 10;
            line2.ParentParcel = LocalParcelCopy;
            newSection.Lines.Add(line2);

            SMLine line3 = new SMLine(newSection);
            line3.StartX = 73;
            line3.StartY = -7;
            line3.EndX = 63;
            line3.EndY = -7;
            line3.SectionLetter = newSection.SectionLetter;
            line3.Direction = "W";
            line3.LineNumber = 3;
            line3.XLength = 10;
            line3.YLength = 0;
            line3.ParentParcel = LocalParcelCopy;
            newSection.Lines.Add(line3);
            SMLine line4 = new SMLine(newSection);
            line4.StartX = 63;
            line4.StartY = -7;
            line4.EndX = 63;
            line4.EndY = 3;
            line4.SectionLetter = newSection.SectionLetter;
            line4.Direction = "S";
            line4.LineNumber = 4;
            line4.XLength = 0;
            line4.YLength = 10;
            line4.ParentParcel = LocalParcelCopy;
            newSection.Lines.Add(line4);

            LocalParcelCopy.Sections.Add(newSection);
            LocalParcelCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;
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

        private void drawSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);

            sms.RenderSketch(true);
            sketchBox.Image = sms.SketchImage;
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
            string message = string.Format("Raw Mouse Location is {0},{1}", MousePosition.X, MousePosition.Y);

            PointF mouseLocation = new PointF(mouseDownX, mouseDownY);
            message += string.Format("   Calculated Position is {0},{1}", mouseDownX, mouseDownY);
            debugInfoLabel.Text = message;
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
                string errMessage = string.Format("No lines contain an available connection point from point {0},{1}", mouseLocation.X, mouseLocation.Y);

                Console.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw new InvalidDataException(errMessage);
            }
            else
            {
                connectionSectionLetters = (from l in connectionLines select l.SectionLetter).ToList();
                if (connectionSectionLetters.Count > 1)
                {
                    AttSectLtr = MultiPointsAvailable(connectionSectionLetters);
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
                MoveCursorToScreenPoint(ScaledJumpPoint);
            }
        }

        private void MoveCursorToScreenPoint(PointF targetPoint)
        {
            Color penColor = Color.Teal;
            PointF dbPoint = SMGlobal.ScaledPointToDbPoint((decimal)targetPoint.X, (decimal)targetPoint.Y, LocalParcelCopy.Scale, SketchOrigin);
            Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(targetPoint.X);
            int jumpYScaled = Convert.ToInt32(targetPoint.Y);
            Cursor.Position = new Point(jumpXScaled, jumpYScaled);

            //penColor = (_undoMode || draw) ? Color.Red : Color.Black;

            // sketchBox.Image = SketchImage;

            distanceText.Focus();

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
            mouseLocationLabel.Text = string.Format("({0},{1})", e.X, e.Y);
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
            SketchUpGlobals.LocalityPrefix = locality;
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
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch(true);
            sketchBox.Image = sms.SketchImage;

            feedbackStatus.Text = string.Format("Breaking line D-{0} at {1:N2},{2:N2}...", line1Number, breakPoint1.X, breakPoint1.Y);
            LabelLinesOffsetNeg(SectionD, line1Number);
        }

        public void TestBreakLine2()
        {
            SMSection SectionD = BreakSectionLine(sectionLetter, line1Number, breakPoint1, newSectionLetter);
            LocalParcelCopy.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            GreenBrush = Brushes.MidnightBlue;
            feedbackStatus.Text = string.Format("Breaking line D-{0} at {1:N2},{2:N2}...", line2Number, breakPoint2.X, breakPoint2.Y);
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch(true);
            sketchBox.Image = sms.SketchImage;

            LabelLinesOffsetNeg(SectionD, line1Number);
        }

        public void TestCombinabilityOfLines()
        {
            string sectionLetter = "D";
            int line1Number = 3;
            feedbackStatus.Text = "Combining Lines D3 & D4 again.";
            SMLineManager lm = new SMLineManager();
            string newSectionLetter = "G";
            PointF breakPoint1 = new PointF(-10, -2);
            PointF breakPoint2 = new PointF(0, -12);
            string pointLabel = string.Empty;
            SMSection SectionD = BreakSectionLine(sectionLetter, line1Number, breakPoint1, newSectionLetter);
            RenderSketch(sketchBox);
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

        private void RenderSketch(PictureBox targetControl)
        {
            SMSketcher sms = new SMSketcher(LocalParcelCopy, targetControl);
            sms.RenderSketch(true);
            targetControl.Image = sms.SketchImage;
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
            int labelLength = section.SectionLabel.Length;

            PointF labelLocation = section.ScaledSectionCenter;

            graphics.DrawString(label, font, RedBrush, labelLocation);
        }

        private void DrawLine(Graphics gr, SMLine line, Pen pen)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            gr.DrawLine(pen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(Graphics gr, SMLine line, Pen pen, bool omitLabel = true)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            gr.DrawLine(pen, drawLineStart, drawLineEnd);
            if (!omitLabel)
            {
                DrawLabel(line, omitLabel);
            }
        }

        private void DrawLine(SMLine line)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            graphics.DrawLine(BluePen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, bool omitLabel = true)
        {
            //PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            //PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            graphics.DrawLine(BluePen, drawLineStart, drawLineEnd);

            if (!omitLabel)
            {
                DrawLabel(line);
            }
        }

        private void DrawSections()
        {
            if (LocalParcelCopy == null)
            {
                RenderSketch(sketchBox);
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
                RenderSketch(sketchBox);
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
                RenderSketch(sketchBox);
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

        //private void SetSectionCenterPoints(SMParcel parcel)
        //{
        //    List<PointF> sectionPoints = new List<PointF>();
        //    foreach (SMSection section in parcel.Sections)
        //    {
        //        sectionPoints = new List<PointF>();
        //        foreach (SMLine line in section.Lines)
        //        {
        //            sectionPoints.Add(line.ScaledStartPoint);
        //            sectionPoints.Add(line.ScaledEndPoint);
        //        }
        //        PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
        //        PointF exactCenter = sectionBounds.CenterPointOfBounds;

        //        section.ScaledSectionCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(0, -20));
        //    }
        //}

        private void SetSketchScale(PictureBox target)
        {
            //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
            int boxHeight = target.Height - 20;
            int boxWidth = target.Width - 20;
            decimal xScale = Math.Floor(boxWidth / LocalParcelCopy.SketchXSize);
            decimal yScale = Math.Floor(boxHeight / LocalParcelCopy.SketchYSize);
            LocalParcelCopy.Scale = SMGlobal.SmallerDouble(xScale, yScale) * 0.75M;
        }

        private void ShowNearestCorners(PointF mouseLocation)
        {
            List<SMPointComparer> pointDistances = PointDistances(mouseLocation, LocalParcelCopy.AllSectionLines);
            decimal closestDistance = (from d in pointDistances select d.EndPointDistance).Min();
            List<SMLine> nearestLines = (from l in pointDistances where l.EndPointDistance == closestDistance select l.ComparisonLine).ToList();
            Brush violetBrush = Brushes.DarkViolet;
            Font font = new Font("Segoe UI", 8, FontStyle.Bold, GraphicsUnit.Point);
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

        public object DirectionOfMovement
        {
            get;
            private set;
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

        public decimal MovementDistanceScaled
        {
            get;
            private set;
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
            feedbackStatus.Text = "D-3 is now D3 & D4, D4 is now D5.";
            traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Version: {0}", LocalParcelCopy.SnapShotIndex));
            foreach (SMLine line in LocalParcelCopy.AllSectionLines)
            {
                traceOut.AppendLine(string.Format("{0}:{1},{2} to {3},{4} Att: {5}", line.SectionLetter + "-" + line.LineNumber, line.StartX, line.StartY, line.EndX, line.EndY, line.AttachedSection));
            }
            Debug.WriteLine(string.Format("Section {0}", traceOut.ToString()));

            //    CleanUpBrokenLines(lm);
            graphics.Clear(Color.White);
            RenderSketch(sketchBox);
        }

        private void cmenuJump_Click(object sender, EventArgs e)
        {
            JumpToNearestCorner();

            //MoveCursorToJumpPoint(MousePosition);
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

        private void distanceText_KeyDown(object sender, KeyEventArgs e)
        {
            _isKeyValid = false;
            bool IsArrowKey = (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down);
            if (!IsArrowKey)
            {
                HandleNonArrowKeys(e);
            }
            else
            {
                HandleDirectionalKeys(e);
            }
        }

        private void distanceText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_isKeyValid == true)
            {
                e.Handled = true;
            }
        }

        private void distanceText_KeyUp(object sender, KeyEventArgs e)
        {
            if (_isKeyValid == true)
            {
                e.Handled = true;
            }
        }

        private void drawingToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void DrawRedLine(PointF screenStart, PointF screenEnd)
        {
            RedPen = new Pen(Color.Red, 2);
            Graphics g = sketchBox.CreateGraphics();
            g.DrawLine(RedPen, screenStart, screenEnd);
            infoLabel.Text = InfoMessage();
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

        private void FlipSketchHorizonal()
        {
            CamraDataEnums.CardinalDirection dir;
            string newDir=string.Empty;
            foreach (SMLine l in LocalParcelCopy.AllSectionLines)
            {
                dir = SMGlobal.DirectionFromString(l.Direction);
              
                switch (dir)
                {
                    case CamraDataEnums.CardinalDirection.N:
                        newDir = CamraDataEnums.CardinalDirection.N.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NE:
                        newDir = CamraDataEnums.CardinalDirection.NW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.E:
                        newDir = CamraDataEnums.CardinalDirection.W.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SE:
                        newDir = CamraDataEnums.CardinalDirection.SW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.S:
                        newDir = CamraDataEnums.CardinalDirection.S.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SW:
                        newDir = CamraDataEnums.CardinalDirection.SE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.W:
                        newDir = CamraDataEnums.CardinalDirection.E.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NW:
                        newDir = CamraDataEnums.CardinalDirection.NE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.None:
                        break;

                    default:
                        newDir = string.Empty;
                        break;
                }
                l.Direction = newDir;
                l.StartX = l.StartX * -1M;
                l.EndX = l.EndX * -1M;
                
            }
            
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

      

        private void GetPointsFromText()
        {
            float x1 = 0f;
            float y1 = 0f;
            float x2 = 0f;
            float y2 = 0f;
            string dbX1 = x1TextBox.Text;
            string dbY1 = y1TextBox.Text;
            string dbX2 = x2TextBox.Text;
            string dbY2 = y2TextBox.Text;
            float.TryParse(dbX1, out x1);
            float.TryParse(dbY1, out y1);
            float.TryParse(dbX2, out x2);
            float.TryParse(dbY2, out y2);
            dbStart = new PointF(x1, y1);
            dbEnd = new PointF(x2, y2);
        }

        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            string textEntered = string.Empty;
            decimal distanceValue = 0.00M;
            SMGlobal.MoveDirection directionOfMovement;

            textEntered = distanceText.Text;

            if (textEntered.IndexOf(",") > 0)
            {
                _isAngle = true;
                ParseAngleEntry(e, textEntered);
            }
            else
            {
                decimal.TryParse(distanceText.Text, out distanceValue);
                MovementDistanceScaled = distanceValue * LocalParcelCopy.Scale;
                directionOfMovement = SMGlobal.GetDirectionOfKeyEntered(e);
                HandleMovementByKey(directionOfMovement, distanceValue);
            }
        }

        private void HandleDirectionalKeys(KeyEventArgs e, string distanceString)
        {
            decimal distanceValue = 0.00M;
            SMGlobal.MoveDirection directionOfMovement;

            if (distanceString.IndexOf(",") > 0)
            {
                _isAngle = true;
                ParseAngleEntry(e, distanceString);
            }
            else
            {
                decimal.TryParse(distanceText.Text, out distanceValue);
                MovementDistanceScaled = distanceValue * LocalParcelCopy.Scale;
                directionOfMovement = SMGlobal.GetDirectionOfKeyEntered(e);
                HandleMovementByKey(directionOfMovement, distanceValue);
            }
        }

        private void HandleMovementByKey(SMGlobal.MoveDirection directionOfMovement, decimal distanceValue)
        {
        }

        private void HandleNonArrowKeys(KeyEventArgs e)
        {
            bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
            if (notNumPad)
            {
                #region Not Numberpad

                if (e.KeyCode == Keys.Tab)
                {
                    //Ask Dave what should go here, if anything.
                }

                if (e.KeyCode != Keys.Back)
                {
                    _isKeyValid = true;
                }
                bool isNumberKey = (e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D9 || e.KeyCode == Keys.D0);
                bool isPunctuation = (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.OemPeriod);
                {
                    if (isNumberKey || isPunctuation)
                    {
                        _isKeyValid = false;
                    }
                    if (e.KeyCode == Keys.Oemcomma)
                    {
                        _isKeyValid = false;
                        _isAngle = true;
                    }
                    if (e.KeyCode == Keys.Delete)
                    {
                        //UndoLine();
                        _isKeyValid = false;
                    }
                }

                #endregion Not Numberpad
            }
        }

        private void IdentifyCorners()
        {
            List<PointF> endPoints = new List<PointF>();
            int pointNumber = 1;
            foreach (SMLine l in LocalParcelCopy.AllSectionLines)
            {
                string pointLabel = string.Format("{0} ({1})", pointNumber, l.SectionLetter);
                ShowPoint(pointLabel, l.ScaledEndPoint);
                pointNumber++;
            }
        }

        private string InfoMessage()
        {
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("Scale is {0:N2}(x) and {1:N2}(y)", xScale, yScale));
            traceOut.AppendLine(string.Format("Using the {0} scale", xScale > yScale ? "Y" : "X"));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
            return traceOut.ToString();
#endif
        }

        private void ParseAngleEntry(KeyEventArgs e, string textEntered)
        {
        }

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

        private void pictTest1_DoubleClick(object sender, EventArgs e)
        {
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch(false);
            sketchBox.Image = sms.SketchImage;
        }

        private void RemoveLastLineAdded(SMParcel parcel, PictureBox targetContainer)
        {
            SMSketcher sms = new SMSketcher(parcel, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
            SMSection lastSection = parcel.SelectSectionByLetter(parcel.LastSectionLetter);

            int lastLineNumber = (from l in lastSection.Lines select l.LineNumber).Max();

            SMLine lastLine = lastSection.Lines.Where(l => l.LineNumber == lastLineNumber).FirstOrDefault();
            MessageBox.Show(string.Format("I will now Undo line {0}-{1}...", lastSection.SectionLetter, lastLine.LineNumber));
            lastSection.Lines.Remove(lastLine);
            sms = new SMSketcher(parcel, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
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

        private void runTest1_Click(object sender, EventArgs e)
        {
            string sectionLetter = string.Empty;
            string message = string.Empty;

            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch();
            AddNewTestSection();
            DrawSections();
        }

        private void runTest4_Click(object sender, EventArgs e)
        {
            FlipSketchHorizonal();
        }

        private void ShowScaledPoints()
        {
            float xSize = Math.Abs(dbStart.X - dbEnd.X);
            float ySize = Math.Abs(dbStart.Y - dbEnd.Y);

            if (xSize == 0)
            {
                xSize = ySize;
            }

            if (ySize == 0)
            {
                ySize = xSize;
            }
            xScale = (decimal)(sketchBox.Width / xSize);
            yScale = (decimal)(sketchBox.Height / ySize);

            //   scale = SMGlobal.SmallerDecimal(xScale, yScale)*.80M;
            scale = 7.2M;
            float centerX = sketchBox.Left + (sketchBox.Width / 2);
            float centerY = sketchBox.Top + (sketchBox.Height / 2);

            origin = new PointF(centerX, centerY);
            screenStart = SMGlobal.DbPointToScaledPoint((decimal)dbStart.X, (decimal)dbStart.Y, scale, origin);
            screenEnd = SMGlobal.DbPointToScaledPoint((decimal)dbEnd.X, (decimal)dbEnd.Y, scale, origin);
            screenX1TextBox.Text = string.Format("{0:N0}", screenStart.X);
            screenY1TextBox.Text = string.Format("{0:N0}", screenStart.Y);
            screenX2TextBox.Text = string.Format("{0:N0}", screenEnd.X);
            screenY2TextBox.Text = string.Format("{0:N0}", screenEnd.Y);
        }

        private void sketchBox_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownX = e.X;
            mouseDownY = e.Y;
        }

        private void sketchBox_MouseMove(object sender, MouseEventArgs e)
        {
            mouseLocationLabel.Text = string.Format("{0},{1}", e.X, e.Y);
        }

        private void TestSketchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            graphics.Dispose();
        }

        private void toolStripMenuCombinLinesInD_Click(object sender, EventArgs e)
        {
            BreakAndRejoinD3();
        }

        private void tsbDrawRed_Click(object sender, EventArgs e)
        {
            GetPointsFromText();
            ShowScaledPoints();
            DrawRedLine(screenStart, screenEnd);
            infoLabel.Text = InfoMessage();
        }

        private void tsmAllTests_Click(object sender, EventArgs e)
        {
            //TODO: Offset the corner number or combine if there is an overlap.
            //Remove the rectangle and replace with a lighter circle.
            //Identify the legal directions once a corner is chosen, and allow
            //offset of no more than the length of the selected line.

            IdentifyCorners();
        }

        private void tsMenuExitForm_Click(object sender, EventArgs e)
        {
            if (graphics != null)
            {
                graphics.Dispose();
            }
            Dispose();
            Close();
        }

        private void tsmListParcelSnapshots_Click(object sender, EventArgs e)
        {
            RemoveLastLineAdded(LocalParcelCopy, sketchBox);

            DrawSections();
        }

        private bool _isAngle;

        private bool _isKeyValid;

        private int _mouseX;

        private int _mouseY;

        // TODO: Remove if not needed:	 private bool _hasMultiSection;
        private PointF breakPoint1 = new PointF(-10, -2);

        private PointF breakPoint2 = new PointF(0, -12);
        private PointF dbEnd;
        private PointF dbStart;
        private bool firstTimeLoaded = false;
        private int line1Number = 3;
        private int line2Number = 5;
        private string newSectionLetter = "G";
        private PointF origin;
        private string pointLabel = string.Empty;
        private decimal scale;
        private PointF screenEnd;
        private PointF screenStart;
        private string sectionLetter = "D";
        private decimal xScale = 0M;
        private decimal yScale = 0M;

        private void runTest5_Click(object sender, EventArgs e)
        {
            FlipUpDown();
        }
        private void FlipUpDown()
        {
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            LocalParcelCopy.SnapShotIndex++;
            CamraDataEnums.CardinalDirection dir;
            string newDir = string.Empty;
            foreach (SMLine l in LocalParcelCopy.AllSectionLines)
            {
                dir = SMGlobal.DirectionFromString(l.Direction);

                switch (dir)
                {
                    case CamraDataEnums.CardinalDirection.N:
                        newDir = CamraDataEnums.CardinalDirection.S.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NE:
                        newDir = CamraDataEnums.CardinalDirection.SE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.E:
                        newDir = CamraDataEnums.CardinalDirection.E.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SE:
                        newDir = CamraDataEnums.CardinalDirection.NE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.S:
                        newDir = CamraDataEnums.CardinalDirection.N.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SW:
                        newDir = CamraDataEnums.CardinalDirection.NW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.W:
                        newDir = CamraDataEnums.CardinalDirection.W.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NW:
                        newDir = CamraDataEnums.CardinalDirection.SW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.None:
                        break;

                    default:
                        newDir = string.Empty;
                        break;
                }
                l.Direction = newDir;
                l.StartY = l.StartY * -1M;
                l.EndY = l.EndY * -1M;

            }
            SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
            SMSketcher sms = new SMSketcher(LocalParcelCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }
    }
}