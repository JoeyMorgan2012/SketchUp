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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class ExpandoSketch : Form
    {
        #region "Constructor"

        public ExpandoSketch(string sketchFolder, int sketchRecord, int sketchCard, bool hasSketch, bool hasNewSketch)
        {
            InitializeComponent();
            this.Shown += new System.EventHandler(this.Form_Shown);
            AddSectionContextMenu.Enabled = false;
            WorkingSection = null;
            WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
            if (WorkingParcel.Sections == null || WorkingParcel.Sections.Count == 0)
            {
                EditState = DrawingState.NewSketch;
            }
            else
            {
                EditState = DrawingState.SketchLoaded;

                ShowWorkingCopySketch(sketchFolder, sketchRecord.ToString(), sketchCard.ToString(), hasSketch, hasNewSketch);
            }
            ShowEditStatus();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            EditState = DrawingState.SketchLoaded;
            SetButtonStates();
        }

        #endregion "Constructor"

        #region "Private methods"

        private static string LegalDirectionsMessage(List<string> legalDirections)
        {
            var statusMessage = new StringBuilder();
            statusMessage.Append("Legal Directions: ");
            int counter = 1;
            foreach (string s in legalDirections.Distinct())
            {
                if (counter < legalDirections.Count)
                {
                    statusMessage.AppendFormat("{0}, ", s);
                }
                else
                {
                    statusMessage.AppendFormat("{0}", s);
                }
                counter++;
            }

            return statusMessage.ToString();
        }

        private static Color PenColorForDrawing(DrawingState editingStep)
        {
            Color penColor;
            switch (editingStep)
            {
                case DrawingState.Drawing:
                    penColor = Color.Red;
                    break;

                case DrawingState.SectionAdded:
                case DrawingState.JumpPointSelected:
                    penColor = Color.Teal;
                    break;

                default:
                    penColor = Color.Transparent;
                    break;
            }

            return penColor;
        }

        private void AddJumpLineToSketch(CamraDataEnums.CardinalDirection direction, double xLength, double yLength)
        {
            try
            {
                bool directionIsLegal = LegalMoveDirections.Contains(direction.ToString());
                bool endsOnLine = IsNewSketch;

                var newLine = new DrawOnlyLine(AttachmentSection);

                newLine.LineNumber = AttachmentSection.Lines.Count + 1;
                newLine.Direction = direction.ToString().ToUpper();
                newLine.StartX = DbStartX;
                newLine.StartY = DbStartY;
                newLine.EndX = newLine.StartX + xLength;
                newLine.EndY = newLine.StartY + yLength;
                newLine.XLength = xLength;
                newLine.YLength = yLength;
                PointF projectedEnd = newLine.EndPoint;
                foreach (SMLine l in AttachmentSection.Lines)
                {
                    endsOnLine |= SMGlobal.PointIsOnLine(l.StartPoint, l.EndPoint, projectedEnd);
                }
                if (directionIsLegal && endsOnLine)
                {
                    WorkingParcel.DrawOnlyLines.Add(newLine);
                    //WorkingSection.ParentParcel = WorkingParcel;
                }
                else
                {
                    string title = "Invalid Action";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Information;

                    string statusMessage = $"While positioning your starting point, you must stay on the line attached to the starting corner.\n\n{LegalDirectionsMessage(LegalMoveDirections)}.\n\n If you are ready to begin sketching the section, click the \"Begin\" button.";
                    MessageBox.Show(statusMessage, title, buttons, icon);
                    MoveCursorToLastValidPoint();
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

        private void AddLineToSketch(CamraDataEnums.CardinalDirection direction, double xLength, double yLength)
        {
            try
            {
                var newLine = new SMLine(WorkingSection);
                newLine.LineNumber = WorkingSection.Lines.Count + 1;
                newLine.Direction = direction.ToString().ToUpper();
                newLine.StartX = DbStartX;
                newLine.StartY = DbStartY;
                newLine.EndX = newLine.StartX + xLength;
                newLine.EndY = newLine.StartY + yLength;
                WorkingSection.Lines.Add(newLine);

                //WorkingSection.ParentParcel = WorkingParcel;
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

        private void AddParcelToSnapshots(SMParcel parcel)
        {
            var sr = new SketchRepository(parcel);
            WorkingParcel = sr.AddSketchToSnapshots(parcel);
            ShowEditStatus();
        }

        private void AddSection()
        {
            AddParcelToSnapshots(WorkingParcel);
            GetSectionTypeInfo();
            _deleteMaster = false;
            _isClosed = false;
        }

        private void AdjustLengthDirection(CamraDataEnums.CardinalDirection moveDirection, ref double xLength, ref double ylength)
        {
            switch (moveDirection)
            {
                case CamraDataEnums.CardinalDirection.N:
                case CamraDataEnums.CardinalDirection.NE:
                    ylength *= -1;
                    break;

                case CamraDataEnums.CardinalDirection.SW:
                case CamraDataEnums.CardinalDirection.W:
                    xLength *= -1;
                    break;

                case CamraDataEnums.CardinalDirection.NW:
                    ylength *= -1;
                    xLength *= -1;
                    break;
            }
        }

        private void AttachNewSectionToSketch()
        {
            bool sectionReady = WorkingSection != null && WorkingSection.Lines != null & WorkingSection.SectionIsClosed;
            if (sectionReady)
            {
                SMLine firstLine = (from l in WorkingSection.Lines where l.LineNumber == 1 select l).FirstOrDefault();
                PointF wsStart = firstLine.StartPoint;
                List<SMLine> linesWithStart = (from l in AttachmentSection.Lines where SMGlobal.PointIsOnLine(l.StartPoint, l.EndPoint, wsStart) select l).ToList();

                if (linesWithStart != null && linesWithStart.Count > 1)
                {
                    SMLine anchorLine = (from l in linesWithStart where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
                }
                else
                {
                    SMLine anchorLine = linesWithStart.First();
                    var slm = new SMLineManager();
                    AttachmentSection = slm.SectionWithLineBreak(AttachmentSection, anchorLine.LineNumber, wsStart);
                    anchorLine = (from l in AttachmentSection.Lines where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
                }
            }
        }

        private void AutoCloseSection(SMSection section)
        {
            SMLine firstLine = (from l in section.Lines.OrderBy(n => n.LineNumber) select l).FirstOrDefault();
            SMLine lastLine = (from l in section.Lines.OrderBy(n => n.LineNumber) select l).LastOrDefault();

            PointF closeLineStart = lastLine.EndPoint;
            PointF closeLineEnd = firstLine.StartPoint;
            var xLength = (double)Math.Round(Math.Abs(closeLineStart.X - closeLineEnd.X), 2);
            var yLength = (double)Math.Round(Math.Abs(closeLineStart.Y - closeLineEnd.Y), 2);

            CamraDataEnums.CardinalDirection direction = SMGlobal.CalculateLineDirection(closeLineStart, closeLineEnd);
            AdjustLengthDirection(direction, ref xLength, ref yLength);
            AddLineToSketch(direction, xLength, yLength);

            if (WorkingSection.SectionIsClosed)
            {
                EditState = DrawingState.DoneDrawing;
                RedrawSketch(WorkingParcel, WorkingSection == null ? string.Empty : WorkingSection.SectionLetter);

                //SetButtonStates();
            }
            else
            {
                EditState = DrawingState.Drawing;
                RedrawSketch(WorkingParcel, WorkingSection.SectionLetter);
            }
        }

        private void CompleteDrawingNewSection()
        {
            try
            {
                UnsavedChangesExist = true;
                AttachNewSectionToSketch();
                EditState = DrawingState.DoneDrawing;
                RefreshWorkspace();
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

        private bool ConfirmCarportNumbers()
        {
            bool carportUpdateNeeded = false;
            double carportSize = 0;
            List<SMSection> carportSections = (from s in WorkingParcel.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).ToList();
            if (carportSections != null)
            {
                carportCount = carportSections.Count;
                carportSize = (double)carportSections.Sum(s => s.SqFt);
            }
            string noCpCode = (from c in SketchUpLookups.CarPortTypeCollection where c.Description == "NONE" select c.Code).FirstOrDefault();
            int noCarportCode = 0;
            int.TryParse(noCpCode, out noCarportCode);

            bool codeAndNumbersMismatched = (carportCount > 0 && (ParcelMast.CarportNumCars == 0 || parcelMast.CarportTypeCode == noCarportCode));

            bool carsCountUpdateNeeded = carportCount > 1 && (ParcelMast.CarportTypeCode != 0 || ParcelMast.CarportTypeCode != noCarportCode);
            carportUpdateNeeded = (codeAndNumbersMismatched || carsCountUpdateNeeded);

            if (codeAndNumbersMismatched)
            {
                var missingGarCPForm = new MissingGarageData(parcelMast);
                missingGarCPForm.ShowDialog();
            }

            if (carsCountUpdateNeeded)
            {
                var missCPx = new MissingGarageData(parcelMast);
                missCPx.ShowDialog();

                ParcelMast.CarportNumCars += missCPx.CarportNumCars;
            }
            return carportUpdateNeeded;
        }

        private bool ConfirmGarageNumbers()
        {
            bool garageUpdateNeeded = false;
            int gar1Cars = 0;
            int gar2Cars = 0;
            int garageTotalCars = 0;
            int mastGarageTotal = 0;

            bool garageOk = false;
            SMParcelMast originalParcelMast = SketchUpGlobals.SMParcelFromData.ParcelMast;
            mastGarageTotal = (string.IsNullOrEmpty(originalParcelMast.Garage1NumCars.ToString()) ? 0 : originalParcelMast.Garage1NumCars) + (string.IsNullOrEmpty(originalParcelMast.Garage2NumCars.ToString()) ? 0 : originalParcelMast.Garage2NumCars);
            List<SMSection> garages = (from s in WorkingParcel.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).OrderBy(s => s.SectionLetter).ToList();
            if (garages != null && garages.Count > 0)
            {
                garageCount = garages.Count;
            }
            else
            {
                garageCount = 0;
            }

            if (garageCount > 0)
            {
                if (garageCount == 1 && parcelMast.Garage1TypeCode <= 60 || garageCount == 1 && parcelMast.Garage1TypeCode == 63 || garageCount == 1 && parcelMast.Garage1TypeCode == 64)
                {
                    var missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (missGar.Garage1Code != originalParcelMast.Garage1TypeCode)
                    {
                        parcelMast.Garage1NumCars = missGar.Gar1NumCars;
                        parcelMast.Garage1TypeCode = missGar.Garage1Code;
                    }
                }
                if (garageCount > 1 && parcelMast.Garage2NumCars == 0)
                {
                    var missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    //Start here to migrate to new form.
                    if (missGar.Garage2Code != originalParcelMast.Garage2TypeCode)
                    {
                        parcelMast.Garage2NumCars = missGar.Gar2NumCars;
                        parcelMast.Garage2TypeCode = missGar.Garage2Code;
                    }
                }
                if (garageCount > 2)
                {
                    string message = "There can only be two garages defined for this structure.";
                    MessageBox.Show(message, "Garage Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //TODO: Provide a rollback here.
                }
            }
            return garageOk;
        }

        private DialogResult ConfirmSketchDeletion()
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            MessageBoxIcon icon = MessageBoxIcon.Question;

            string title = string.Empty;
            string warning = string.Empty;

            title = "Make Parcel Vacant?";
            warning = $"This will delete the entire sketch. Do you want to set the parcel's Occupancy to Vacant?\n\nClick \"Yes\" to delete the sketch and make the parcel vacant. Click \"No\" to delete the sketch itself but leave the parcel details unchanged. Click \"Cancel\" to abort this action. NOTE: Once you save your choice cannot be undone--you will have to re-enter structure details and redraw the sketch.";
            icon = MessageBoxIcon.Warning;

            return MessageBox.Show(warning, title, buttons, icon);
        }

        private void DeleteSketch()
        {
            var sr = new SketchRepository(WorkingParcel);
            switch (ConfirmSketchDeletion())
            {
                case DialogResult.Yes:
                    sr.DeleteSketch(WorkingParcel, false);

                    break;

                case DialogResult.No:

                    sr.DeleteSketch(WorkingParcel, false);
                    break;

                default:
                    break;

                    //Do nothing.
            }
        }

        private CamraDataEnums.CardinalDirection DirectionOfKeyEntered(KeyEventArgs e)
        {
            CamraDataEnums.CardinalDirection keyDirection;
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.E:
                case Keys.R:
                    keyDirection = CamraDataEnums.CardinalDirection.E;

                    break;

                case Keys.Left:
                case Keys.L:
                case Keys.W:
                    keyDirection = CamraDataEnums.CardinalDirection.W;

                    break;

                case Keys.Up:
                case Keys.N:
                case Keys.U:
                    keyDirection = CamraDataEnums.CardinalDirection.N;

                    break;

                case Keys.Down:
                case Keys.D:
                case Keys.S:
                    keyDirection = CamraDataEnums.CardinalDirection.S;

                    break;

                default:
                    keyDirection = CamraDataEnums.CardinalDirection.None;

                    break;
            }
            return keyDirection;
        }

        private void DiscardChangesAndExit()
        {
            DisplayStatus("Reverting to saved sketch...");
            RevertToSavedSketch();
            UnsavedChangesExist = false;
            Close();
        }

        private void DisplayStatus(string statusText)
        {
            sketchStatusMessage.Text = statusText;
            if (!UnsavedChangesExist)
            {
                sketchStatusMessage.Image = GreenCheckImage;
            }
            else
            {
                sketchStatusMessage.Image = AsteriskImage;
            }
        }

        private void DistText_KeyDown(object sender, KeyEventArgs e)
        {
            _isKeyValid = false;
            bool IsArrowKey = (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down);
            if (!IsArrowKey)
            {
                HandleNonArrowKeys(e);
            }
            else
            {
                _isKeyValid = true;
                HandleDirectionalKeys(e);
            }
        }

        private void DistText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_isKeyValid == true)
            {
                e.Handled = true;
            }
        }

        private void DistText_KeyUp(object sender, KeyEventArgs e)
        {
            if (_isKeyValid)
            {
                e.Handled = true;
            }
        }

        

        private void DrawSmallRectangleAroundPoint(int jumpXScaled, int jumpYScaled, Color penColor)
        {
            Graphics g = Graphics.FromImage(MainImage);
            var pen1 = new Pen(penColor, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 2, 2);
            g.Save();
        }

        private void EditParcelSections()
        {
            EditState = DrawingState.LoadingEditForm;
            ShowEditStatus();
            ShowEditSections();
            DisplayStatus("Ready.");
            RefreshWorkspace();
        }

        private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = StopClose();
            }
            else
            {
                e.Cancel = false;

                // Close();
            }
        }

        private void ExpandoSketch_Shown(object sender, EventArgs e)
        {
            if (sketchBox.Image == null)
            {
                RefreshWorkspace();
            }
        }

        private void ExpSketchPbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isJumpMode)
            {
                _mouseX = e.X;
                _mouseY = e.Y;
            }
        }

        private void ExpSketchPbox_MouseDown(object sender, MouseEventArgs e)
        {
            _isJumpMode = false;
            if (e.Button == MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;
                _mouseX = e.X;
                _mouseY = e.Y;
            }
        }

        private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                // TODO: Remove if not needed: DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;

                _mouseX = e.X;
                _mouseY = e.Y;
                              
            }
        }

        private void FlipHorizontal()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapshotIndex++;
            CamraDataEnums.CardinalDirection dir;
            EditState = DrawingState.Flipping;
            UnsavedChangesExist = true;
            UseWaitCursor = true;
            ShowEditStatus();
            string newDir = string.Empty;
            foreach (SMLine l in WorkingParcel.AllSectionLines)
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
                l.StartX *= -1;
                l.EndX *= -1;
            }
            Application.DoEvents();
            AddParcelToSnapshots(WorkingParcel);

            var sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch(WorkingSection == null ? string.Empty : WorkingSection.SectionLetter);
            sketchBox.Image = sms.SketchImage;
            UseWaitCursor = false;
        }

        private void FlipVertically()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapshotIndex++;
            UnsavedChangesExist = true;
            EditState = DrawingState.Drawing;
            CamraDataEnums.CardinalDirection dir;
            EditState = DrawingState.Flipping;
            UnsavedChangesExist = true;
            UseWaitCursor = true;
            ShowEditStatus();
            string newDir = string.Empty;
            foreach (SMLine l in WorkingParcel.AllSectionLines)
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
                l.StartY *= -1;
                l.EndY *= -1;
            }
            Application.DoEvents();
            AddParcelToSnapshots(WorkingParcel);
            var sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch(WorkingSection == null ? string.Empty : WorkingSection.SectionLetter);
            sketchBox.Image = sms.SketchImage;
            UseWaitCursor = false;
        }

        private bool GarageDataComplete()
        {
            bool updatesNeeded = false;

            try
            {
                if (WorkingSection != null)
                {
                    updatesNeeded = (SketchUpLookups.CarPortTypes.Contains(WorkingSection.SectionType) || SketchUpLookups.GarageTypes.Contains(WorkingSection.SectionType));
                }

                if (updatesNeeded)
                {
                    var mgd = new MissingGarageData(WorkingParcel.ParcelMast, WorkingSection.SqFt, WorkingSection.SectionType);
                    if (mgd.GarageDataOk && mgd.CarportDataOk)
                    {
                        updatesNeeded = false;
                    }
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
            return !updatesNeeded;

            //SMVehicleStructure svs = new SMVehicleStructure(WorkingParcel);
        }

        private AngleVector GetAngleLine()
        {
            _isAngle = true;
            var angle = new AngleVector();
            double xDistance = 0.00;
            double yDistance = 0.00;
            angle = ParseAngleEntry();
            xDistance = angle.XLength;
            yDistance = angle.YLength;
            return angle;
        }

        private List<string> GetLegalMoveDirections(PointF dbStartPoint, string attachSectionLetter)
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            var legalDirections = new List<string>();
            var startPointLineDirections = new List<string>();
            var endPointLineDirections = new List<string>();

            try
            {
                if (EditState == DrawingState.NewSketch)
                {
                    legalDirections.Add("N");
                    legalDirections.Add("S");
                    legalDirections.Add("E");
                    legalDirections.Add("W");
                }
                else
                {
                    SMSection anchorSection = WorkingParcel.Sections.FirstOrDefault(s => s.SectionLetter == attachSectionLetter);

                    var linesConnectedAtStart = new List<SMLine>();
                    var linesConnectedAtEnd = new List<SMLine>();

                    if (anchorSection != null && anchorSection.Lines != null && anchorSection.Lines.Count > 0)
                    {
                        linesConnectedAtEnd = (from l in anchorSection.Lines select l).Where(p => ((float)p.EndX == dbStartPoint.X && (float)p.EndY == dbStartPoint.Y)).ToList();
                        linesConnectedAtStart = (from l in anchorSection.Lines select l).Where(p => ((float)p.StartX == dbStartPoint.X && (float)p.StartY == dbStartPoint.Y)).ToList();

                        legalDirections.AddRange((from l in linesConnectedAtStart select l.Direction).ToList());
                        legalDirections.AddRange((from l in linesConnectedAtEnd select SMGlobal.ReverseDirection(l.Direction)).ToList());
                    }
                }
                string statusMessage = LegalDirectionsMessage(legalDirections.Distinct().ToList());

                legalDirectionsLabel.Text = statusMessage.ToString();
#if DEBUG || TEST

                Trace.WriteLine($"{DateTime.Now}: Legal Directions Message: {statusMessage}");

#endif

                return legalDirections.Distinct().ToList();
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

        private void GetSectionTypeInfo()
        {
            _addSection = true;

            NewSectionPoints.Clear();
            lineCnt = 0;
            var sectionTypeForm = new SelectSectionTypeDialog(WorkingParcel.ParcelMast, _addSection, lineCnt, IsNewSketch);

            sectionTypeForm.ShowDialog(this);
            if (sectionTypeForm.SectionWasAdded)

            {
                NextSectLtr = WorkingParcel.NextSectionLetter;
                _nextSectType = SelectSectionTypeDialog._nextSectType;
                _nextStoryHeight = SelectSectionTypeDialog.newSectionstories;
                _nextLineCount = SelectSectionTypeDialog._nextLineCount;
                WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
                _hasNewSketch = (NextSectLtr == "A");
                WorkingSection = sectionTypeForm.WorkingSection;
                EditState = DrawingState.SectionAdded;
                SetButtonStates();
                SectionWasAdded = true;
                UnsavedChangesExist = true;
                _isJumpMode = true;
                try
                {
                    FieldText.Text = string.Format("Sect- {0}, {1} sty {2}", NextSectLtr.Trim(), _nextStoryHeight.ToString("N2"), _nextSectType.Trim());
                }
                catch
                {
                }
            }
            else
            {
                EditState = DrawingState.SketchLoaded;
                SectionWasAdded = false;
                UnsavedChangesExist = false;
                SetButtonStates();
            }

            if (sectionTypeForm != null)
            {
                sectionTypeForm.Dispose();
            }

            //SetButtonStates();
        }

        //    int secCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(checkSect.ToString()));
        //    return secCnt;
        //}
        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            CamraDataEnums.CardinalDirection direction;
            direction = DirectionOfKeyEntered(e);
            string distanceText = string.Empty;
            double distanceValueEntered = 0.00;
            double yLength = 0.00;
            double xLength = 0.00;
            var angle = new AngleVector();
            if (!string.IsNullOrEmpty(DistText.Text))
            {
                distanceText = DistText.Text;

                if (distanceText.IndexOf(",") > 0)
                {
                    angle = GetAngleLine();
                    direction = angle.AngledLineDirection;
                    xLength = angle.XLength;
                    yLength = angle.YLength;
                }
                else
                {
                    double.TryParse(distanceText, out distanceValueEntered);
                    angle = ParseNEWSLine(distanceText, direction);
                    direction = angle.AngledLineDirection;
                    xLength = angle.XLength;
                    yLength = angle.YLength;
                }

                AdjustLengthDirection(direction, ref xLength, ref yLength);
                DistText.Text = string.Empty;
                DistText.Focus();

                switch (EditState)
                {
                    case DrawingState.Drawing:
                        DrawSketchLine(direction, yLength, xLength);

                        break;

                    case DrawingState.JumpPointSelected:

                        AddJumpLineToSketch(direction, xLength, yLength);

                        break;

                    default:
                        _isKeyValid = false;

                        break;
                }
               
            }
            SetButtonStates();
            DistText.Text = string.Empty;
            DistText.Focus();
        }

        private void DrawSketchLine(CamraDataEnums.CardinalDirection direction, double yLength, double xLength)
        {
            AddLineToSketch(direction, xLength, yLength);

            //TODO: See if leaving these in place is a better approach. JMM 7-25
            //WorkingParcel.DrawOnlyLines.Clear();

            RedrawSketch(WorkingParcel);
            if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
            {
                SMLine newLine = (from l in WorkingSection.Lines.OrderBy(l => l.LineNumber) select l).LastOrDefault();
                DbStartPoint = newLine.EndPoint;
                dbStartX = newLine.EndX;
                dbStartY = newLine.EndY;
                ScaledStartPoint = newLine.ScaledEndPoint;

                StartOfCurrentLine = ScaledStartPoint;

               
            }
        }

        // SketchUpGlobals.Record, SketchUpGlobals.Card, NextSectLtr));
        private void HandleNonArrowKeys(KeyEventArgs e)
        {
            bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
            if (notNumPad)
            {
                if (e.KeyCode == Keys.Tab)
                {
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
                        UndoLine();
                        _isKeyValid = false;
                    }
                }
            }
        }

        //    lines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());
        //    return lines;
        //}
        //TODO: Remove if unneccessary
        //private int GetSectionsCount()
        //{
        //    var checkSect = new StringBuilder();
        //    checkSect.Append(string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
        //                   SketchUpGlobals.LocalLib,
        //                  SketchUpGlobals.LocalityPrefix,
        private void InitializeDataTablesAndVariables(string sketchFolder, string sketchRecord, string sketchCard, bool hasSketch, bool hasNewSketch)
        {
            IsNewSketch = false;
            _hasNewSketch = hasNewSketch;
            IsNewSketch = hasNewSketch;
            _addSection = false;

            SketchFolder = sketchFolder;
            SketchRecord = sketchRecord;
            SketchCard = sketchCard;
            SketchUpGlobals.HasSketch = hasSketch;
        }

        // crrec, crcard)); getLine.Append("and jldirect <> 'X' ");
        private void JumptoCorner()
        {
            CurrentSecLtr = string.Empty;

            currentAttachmentLine = 0;
            if (IsNewSketch == false)
            {
                var mouseLocation = new PointF(_mouseX, _mouseY);

                JumpConnectionLines = LinesWithClosestEndpoints(mouseLocation);

                if (JumpConnectionLines == null || JumpConnectionLines.Count == 0 || IsNewSketch)
                {
                    SetJumpToCenterForNewSketch();
                }
                else
                {
                    SetJumpPointAtCursor(JumpConnectionLines);
                }
            }
        }

        //    lines = dbConn.DBConnection.RunSelectStatement(getLine);
        //    return lines;
        //}
        //TODO: Remove if unneccessary
        //private DataSet GetSectionLines(int crrec, int crcard)
        //{
        //    DataSet lines;
        //    var getLine = new StringBuilder();
        //    getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
        //    getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
        //    getLine.Append(string.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
        //               SketchUpGlobals.LocalLib,
        //               SketchUpGlobals.LocalityPrefix,
        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isJumpMode || EditState == DrawingState.SectionAdded)
            {
                draw = false;
                IsNewSketch = false;

                JumptoCorner();

                UndoJump = false;
            }
            else if (EditState == DrawingState.NewSketch)
            {
                draw = false;
                IsNewSketch = true;

                JumptoCorner();

                UndoJump = false;
            }
            _isJumpMode = false;
        }

        //private DataSet GetLinesData(int crrec, int crcard)
        //{
        //    DataSet lines;
        //    string getLine = string.Format("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, crrec, crcard);
        private List<SMLine> LinesWithClosestEndpoints(PointF mouseLocation)
        {
            var connectionLines = new List<SMLine>();
            bool parcelHasLines = (WorkingParcel.Sections != null && WorkingParcel.Sections.Count > 0 && WorkingParcel.AllSectionLines != null && WorkingParcel.AllSectionLines.Count > 0);
            if (parcelHasLines)
            {
                foreach (SMLine l in WorkingParcel.AllSectionLines.Where(s => s.SectionLetter != WorkingParcel.LastSectionLetter))
                {
                    l.ComparisonPoint = mouseLocation;
                }

                double shortestDistance = Math.Round((from l in WorkingParcel.AllSectionLines select l.EndPointJumpPointDist).Min(), 1);
                connectionLines = (from l in WorkingParcel.AllSectionLines where Math.Round(l.EndPointJumpPointDist, 1) == shortestDistance select l).ToList();
            }
            return connectionLines;
        }

        private void MoveCursorToLastValidPoint()
        {
            switch (EditState)
            {
                case DrawingState.Drawing:

                    if (WorkingSection != null)
                    {
                        if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
                        {
                            SMLine lastLine = WorkingSection.Lines.OrderByDescending(l => l.LineNumber).FirstOrDefault();
                            PointF lastPoint = lastLine.ScaledEndPoint;
                            MoveCursorToNewPoint(lastPoint);
                        }
                        else if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
                        {
                            DrawOnlyLine lastJumpLine = WorkingParcel.DrawOnlyLines.OrderByDescending(l => l.LineNumber).FirstOrDefault();
                            PointF lastPoint = lastJumpLine.ScaledEndPoint;
                            MoveCursorToNewPoint(lastPoint);
                        }
                        else
                        {
                            MoveCursorToNewPoint(ScaledJumpPoint);
                        }
                    }
                    break;

                case DrawingState.JumpPointSelected:
                    MoveCursorToNewPoint(ScaledJumpPoint);
                    break;

                case DrawingState.NewSketch:
                    var centerPoint = new PointF(sketchBox.Width / 2, sketchBox.Height / 2);
                    MoveCursorToNewPoint(centerPoint);
                    break;
            }
        }

        private void MoveCursorToNewPoint(PointF newPoint)
        {
            Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(newPoint.X);
            int jumpYScaled = Convert.ToInt32(newPoint.Y);

            _isJumpMode = (EditState == DrawingState.JumpPointSelected || EditState == DrawingState.SectionAdded);
            Color penColor = PenColorForDrawing(EditState);
            Cursor.Position = new Point(jumpXScaled, jumpYScaled);
            DrawSmallRectangleAroundPoint(jumpXScaled, jumpYScaled, penColor);

            sketchBox.Image = MainImage;
            sketchBox.Refresh();
            DistText.Focus();
        }

        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multipleSectionsAttachment = string.Empty;

            if (sectionLetterList.Count > 1)
            {
                MultiplePoints = new DataTable();

                var attachmentSectionLetterSelected = new MultiSectionSelection(sectionLetterList);
                attachmentSectionLetterSelected.ShowDialog(this);

                multipleSectionsAttachment = MultiSectionSelection.adjsec;

                MultiplePoints = MultiSectionSelection.MultiplePointsDataTable;

                _hasMultiSection = true;
            }

            return multipleSectionsAttachment;
        }

        private AngleVector ParseAngleEntry()
        {
            var vector = new AngleVector();
            string anglecalls = DistText.Text.Trim();
            double xDist = 0.00;
            double yDist = 0.00;
            int commaCnt = anglecalls.IndexOf(",");

            string xDistText = anglecalls.Substring(0, commaCnt).Trim();
            string yDistText = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();
            double.TryParse(xDistText, out xDist);
            double.TryParse(yDistText, out yDist);

            var angleDialog = new AngleForm();
            angleDialog.ShowDialog();
            CamraDataEnums.CardinalDirection angleDirection = angleDialog.AngleDirection;

            vector.XLength = xDist;
            vector.YLength = yDist;
            vector.AngledLineDirection = angleDirection;
            return vector;
        }

        private AngleVector ParseNEWSLine(string textEntered, CamraDataEnums.CardinalDirection direction)
        {
            double distanceValueEntered = 0.00;
            var angle = new AngleVector();
            double.TryParse(textEntered, out distanceValueEntered);
            switch (direction)
            {
                case CamraDataEnums.CardinalDirection.N:
                case CamraDataEnums.CardinalDirection.S:
                    angle.XLength = 0.00;
                    angle.YLength = distanceValueEntered;
                    break;

                case CamraDataEnums.CardinalDirection.E:
                case CamraDataEnums.CardinalDirection.W:
                    angle.XLength = distanceValueEntered;
                    angle.YLength = 0.00;
                    break;

                case CamraDataEnums.CardinalDirection.None:
                default:
                    angle.XLength = 0.00;
                    angle.YLength = 0.00;

                    break;
            }
            angle.AngledLineDirection = direction;
            return angle;
        }

        private void RedrawSketch(SMParcel parcel, string sectionLetter = "")
        {
            try
            {
                var sketcher = new SMSketcher(parcel, sketchBox);
                sketcher.RenderSketch(sectionLetter);
                sketchBox.Image = sketcher.SketchImage;
                _currentScale = (float)parcel.Scale;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}\n{ex.StackTrace}";
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void RefreshWorkspace()
        {
            try
            {
                WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
                string sectionLetter = WorkingSection == null ? string.Empty : WorkingSection.SectionLetter;
                RedrawSketch(WorkingParcel, sectionLetter);

                SetButtonStates();
                ShowEditStatus();
                progressBar.Visible = false;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void RemoveFirstLineOrSection(SMLine lastLine)
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Starting {MethodBase.GetCurrentMethod().Name}");

#endif
            string message = "This will remove the last line and cancel the editing process. Proceed?";
            string title = "Cancel Sketch Edit?";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
            DialogResult response = MessageBox.Show(message, title, buttons, icon, defButton);
            if (response == DialogResult.Yes)
            {
                DiscardChangesAndExit();
            }

            //SetButtonStates();
        }

        private void RemoveLine(SMLine lastLine)
        {
            string workingSectionLetter = lastLine.SectionLetter;
            WorkingParcel.Sections.FirstOrDefault(s => s.SectionLetter == workingSectionLetter).Lines.Remove(lastLine);

            WorkingSection = WorkingParcel.Sections.FirstOrDefault(s => s.SectionLetter == workingSectionLetter);
            var sms = new SMSketcher(WorkingParcel, sketchBox);
            sms.RenderSketch(WorkingSection.SectionLetter);
            SMLine lastLineDrawn = (from l in WorkingSection.Lines.OrderByDescending(n => n.LineNumber) select l).FirstOrDefault();
            DbStartPoint = lastLineDrawn.EndPoint;
            DbStartX = lastLineDrawn.EndX;
            DbStartY = lastLineDrawn.EndY;
            ScaledStartPoint = SMGlobal.DbPointToScaledPoint(DbStartX, DbStartY, WorkingParcel.Scale, WorkingParcel.SketchOrigin);
            MoveCursorToNewPoint(ScaledStartPoint);
            Application.DoEvents();
            sketchBox.Image = sms.SketchImage;
            Application.DoEvents();
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Sketchbox populated.");
#endif

#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Section {WorkingSection.SectionLetter} now has {WorkingSection.Lines.Count} lines.");
            Trace.WriteLine($"Last line should now be {lastLineDrawn.LineNumber} ({lastLineDrawn.StartX},{lastLineDrawn.StartY} to {lastLineDrawn.EndX},{lastLineDrawn.EndY}; {Math.Abs(lastLineDrawn.LineLength)} ft {lastLineDrawn.Direction}).");
            Trace.WriteLine($"The drawing should now start at screen location  {ScaledStartPoint.X},{ScaledStartPoint.Y}.");
#endif
            EditState = DrawingState.Drawing;
            DistText.Text = string.Empty;
            DistText.Focus();
        }

        private void RestartSectionSketch()
        {
            WorkingSection.Lines.Clear();
            WorkingParcel.DrawOnlyLines.Clear();
            RedrawSketch(WorkingParcel);
            EditState = DrawingState.SectionAdded;
        }

        private void RevertToSavedSketch()
        {
#if DEBUG || TEST
            Trace.WriteLine($"{DateTime.Now}: Running {MethodBase.GetCurrentMethod().Name}");
#endif
            try
            {
                SketchUpGlobals.SketchSnapshots.Clear();

                DisplayStatus("Discarding Changes...");
                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.SMParcelFromData;
                WorkingSection = null;
                UnsavedChangesExist = false;
                Close();
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG||TEST

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void SaveChanges(SMParcel parcel)
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif

            try
            {
                string resultsMessage;
                bool success;
                if (GarageDataComplete() && UnsavedChangesExist)
                {
                    EditState = DrawingState.Saving;
                    progressBar.Visible = true;
                    UpdateProgress(0);
                    parcel.ReorganizeSections();
                    UpdateProgress(10);
                    var sr = new SketchRepository(parcel);
                    UpdateProgress(25);
                    ParcelMast = sr.SaveCurrentParcel(parcel);
                    UpdateProgress(50);
                    WorkingParcel = ParcelMast.Parcel;
                    UpdateProgress(75);

                    success = sr.Success;
                    if (success)
                    {
                        sr.AddSketchToSnapshots(WorkingParcel);
                        UnsavedChangesExist = false;
                        Close();
                    }
                    else
                    {
                        resultsMessage = $"An error occurred while saving Parcel {parcel.Record} Dwelling #{parcel.Card}";
                        MessageBox.Show(resultsMessage.ToString(), "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        EditState = DrawingState.SaveError;
                        progressBar.Visible = false;
                        RefreshWorkspace();
                    }
                    UnsavedChangesExist = !success;
                }

#if DEBUG || TEST

                Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
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

        private void SetButtonStates()
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");

#endif

            try
            {
                bool enableAdd = true;
                bool enableAutoClose = true;
                bool enableBeginDrawing = true;
                bool enableContextMenu = true;
                bool enableDelete = true;
                bool enableDone = true;
                bool enableEdit = true;

                bool enableFlip = true;
                bool enableJumpMenu = true;
                bool enableSave = true;
                bool enableUndo = true;
                bool readyToClose = (WorkingSection != null && WorkingSection.Lines != null && WorkingSection.Lines.Count > 2 && !WorkingSection.SectionIsClosed);
                switch (EditState)
                {
                    case DrawingState.DoneDrawing:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = true;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;

                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = true;
                        enableUndo = false;
                        break;

                    case DrawingState.Flipping:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = true;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;

                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = true;
                        enableUndo = false;
                        break;

                    case DrawingState.Drawing:
                        enableAdd = false;
                        enableAutoClose = readyToClose;
                        enableBeginDrawing = false;
                        enableContextMenu = false;
                        enableDelete = false;
                        enableDone = WorkingSection != null && WorkingSection.SectionIsClosed;
                        enableEdit = false;
                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = false;
                        enableUndo = true;
                        break;

                    case DrawingState.JumpPointSelected:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = true;
                        enableContextMenu = true;
                        enableDelete = false;
                        enableDone = false;
                        enableEdit = false;
                        enableFlip = false;
                        enableJumpMenu = true;
                        enableSave = false;
                        enableUndo = true;
                        break;

                    case DrawingState.LoadingEditForm:
                        enableAdd = true;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = false;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;

                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = true;
                        enableUndo = false;
                        break;

                    case DrawingState.NewSketch:
                        enableAdd = true;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = false;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = false;
                        enableFlip = false;
                        enableJumpMenu = false;
                        enableSave = false;
                        enableUndo = false;
                        break;

                    case DrawingState.SaveError:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = false;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = false;
                        enableFlip = false;
                        enableJumpMenu = false;
                        enableSave = true;
                        enableUndo = false;
                        break;

                    case DrawingState.Saving:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = false;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;
                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = false;
                        enableUndo = false;
                        break;

                    case DrawingState.SectionAdded:
                        enableAdd = false;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = true;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;
                        enableFlip = false;
                        enableJumpMenu = true;
                        enableSave = false;
                        enableUndo = false;
                        break;

                    case DrawingState.SketchLoaded:
                        enableAdd = true;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = true;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;
                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = true;
                        enableUndo = false;
                        break;

                    case DrawingState.SketchSaved:
                        enableAdd = true;
                        enableAutoClose = false;
                        enableBeginDrawing = false;
                        enableContextMenu = true;
                        enableDelete = true;
                        enableDone = false;
                        enableEdit = true;
                        enableFlip = true;
                        enableJumpMenu = false;
                        enableSave = false;
                        enableUndo = false;
                        break;
                }
                btnAddSection.Enabled = enableAdd;
                btnAddSection.Visible = enableAdd;

                btnAutoClose.Enabled = enableAutoClose;
                btnAutoClose.Visible = enableAutoClose;

                btnBegin.Enabled = enableBeginDrawing;
                btnBegin.Visible = enableBeginDrawing;

                btnDone.Enabled = enableDone;
                btnDone.Visible = enableDone;

                btnEditSections.Enabled = enableEdit;
                btnDeleteSketch.Enabled = enableDelete;
                btnSave.Enabled = enableSave;
                btnUndo.Enabled = enableUndo;
                btnUndo.Visible = enableUndo;

                AddSectionContextMenu.Enabled = enableContextMenu;
                cmiJumpToCorner.Enabled = enableJumpMenu;
                cmiFlipH.Enabled = enableFlip;
                cmiFlipV.Enabled = enableFlip;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void SetJumpPointAtCursor(List<SMLine> connectionLines)
        {
            try
            {
                SecLetters = (from l in connectionLines select l.SectionLetter).ToList();
                if (SecLetters.Count > 1)
                {
                    AttSectLtr = MultiPointsAvailable(SecLetters);
                    AttachmentSection = (from s in WorkingParcel.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                    JumpPointLine = (from l in connectionLines where l.SectionLetter == AttSectLtr select l).FirstOrDefault();
                    currentAttachmentLine = JumpPointLine.LineNumber;
                    CurrentSecLtr = AttachmentSection.SectionLetter;
                }
                else
                {
                    AttSectLtr = SecLetters[0];
                    AttachmentSection = (from s in WorkingParcel.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                    JumpPointLine = connectionLines[0];
                    currentAttachmentLine = JumpPointLine.LineNumber;
                    CurrentSecLtr = AttachmentSection.SectionLetter;
                }

                ScaledJumpPoint = JumpPointLine.ScaledEndPoint;
                DbJumpPoint = JumpPointLine.EndPoint;
                ScaledStartPoint = ScaledJumpPoint;
                LegalMoveDirections = GetLegalMoveDirections(DbJumpPoint, AttSectLtr);

                _isJumpMode = true;
                EditState = DrawingState.JumpPointSelected;
                ScaledStartPoint = ScaledJumpPoint;
                DbStartX = Math.Round((double)DbJumpPoint.X, 2);
                DbStartY = Math.Round((double)DbJumpPoint.Y, 2);
                DistText.Focus();
                MoveCursorToNewPoint(ScaledJumpPoint);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void SetJumpToCenterForNewSketch()
        {
            ScaledJumpPoint = new PointF(sketchBox.Width / 2, sketchBox.Height / 2);
            DbJumpPoint = new PointF(0, 0);
            EditState = DrawingState.JumpPointSelected;
            ScaledStartPoint = ScaledJumpPoint;
            DbStartX = Math.Round((double)DbJumpPoint.X, 2);
            DbStartY = Math.Round((double)DbJumpPoint.Y, 2);
            DistText.Focus();
            MoveCursorToNewPoint(ScaledJumpPoint);
            EditState = DrawingState.JumpPointSelected;
        }

        private void ShowEditSections()
        {
            UseWaitCursor = true;
            DisplayStatus("Loading sections information...please wait.");

            var ess = new EditSketchSections(WorkingParcel);
            UseWaitCursor = false;
            ess.BringToFront();
            ess.Show();
            UnsavedChangesExist = ess.UnsavedChangesExist;
        }

        private void ShowEditStatus()
        {
            string message = string.Empty;

            bool showProgress = false;
            Bitmap statusImage = (UnsavedChangesExist ? SaveDrawingImage : GreenCheckImage);
            StatusText = (UnsavedChangesExist ? "*Edited" : "Saved");
            switch (EditState)
            {
                case DrawingState.SketchLoaded:
                case DrawingState.SketchSaved:
                    message = $"Ready to edit the sketch of record {SketchUpGlobals.ParcelWorkingCopy.Record}";

                    showProgress = false;
                    statusImage = EditSectionsImage;
                    UnsavedChangesExist = false;
                    break;

                case DrawingState.NewSketch:
                    StatusText = "New Sketch";
                    message = $"Add a new Sketch for record {WorkingParcel.Record}";
                    showProgress = false;
                    statusImage = GreenCheckImage;
                    UnsavedChangesExist = false;

                    break;

                case DrawingState.Drawing:
                    message = $"Drawing Section {WorkingSection.SectionLabel}";

                    showProgress = false;
                    statusImage = EditSectionsImage;
                    UnsavedChangesExist = true;
                    break;

                case DrawingState.DoneDrawing:

                    message = $"Section added to Sketch. Click \"Save\" to commit this change.";
                    showProgress = false;

                    statusImage = EditSectionsImage;
                    UnsavedChangesExist = true;
                    WorkingSection = null;
                    break;

                case DrawingState.Flipping:
                    message = $"Changing sketch orientation. Click \"Save\" to commit this change.";
                    showProgress = false;

                    statusImage = EditSectionsImage;
                    UnsavedChangesExist = true;
                    break;

                case DrawingState.JumpPointSelected:
                    var directions = new StringBuilder();
                    if (LegalMoveDirections.Count > 0)
                    {
                        int counter = 1;
                        foreach (string s in LegalMoveDirections.Distinct())
                        {
                            if (counter < LegalMoveDirections.Count)
                            {
                                directions.AppendFormat("{0}, ", s);
                            }
                            else
                            {
                                directions.AppendFormat("{0}", s);
                            }
                            counter++;
                        }

                        message = $"Move ({directions.ToString()}) to set start, or click \"Begin\" to start drawing the section here.";
                    }
                    showProgress = false;

                    break;

                case DrawingState.SectionAdded:
                    message = $"Added Section {WorkingSection.SectionLabel}. Right-click to select the nearest corner as a reference point.";
                    showProgress = false;

                    break;

                case DrawingState.Saving:

                    message = $"Saving changes to record {SketchUpGlobals.ParcelWorkingCopy.Record}";
                    showProgress = true;
                    StatusText = "Saving...";
                    break;

                case DrawingState.LoadingEditForm:

                    message = $"Loading the section records...";
                    showProgress = true;

                    break;
            }

            progressBar.Visible = showProgress;
            sketchStatusMessage.Text = StatusText;

            UseWaitCursor = showProgress;
        }

        private void ShowWorkingCopySketch(string sketchFolder, string sketchRecord, string sketchCard, bool hasSketch, bool hasNewSketch)
        {
            try
            {
                InitializeDataTablesAndVariables(sketchFolder, sketchRecord, sketchCard, hasSketch, hasNewSketch);

                WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
                SketchUpGlobals.HasSketch = (WorkingParcel != null && WorkingParcel.AllSectionLines.Count > 0);
                IsNewSketch = !SketchUpGlobals.HasSketch;

                if (SketchUpGlobals.HasSketch)
                {
                    var sketcher = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
                    sketcher.RenderSketch(WorkingSection == null ? string.Empty : WorkingSection.SectionLetter);

                    MainImage = sketcher.SketchImage;
                    _currentScale = (float)WorkingParcel.Scale;
                    ScaleBaseX = sketchBox.Width / (float)WorkingParcel.SketchXSize;

                    ScaleBaseY = sketchBox.Height / (float)WorkingParcel.SketchYSize;
                }
                else
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                }

                if (MainImage == null)
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                    _vacantParcelSketch = true;
                    IsNewSketch = true;
                }
                if (_vacantParcelSketch)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    g.Clear(Color.White);
                    _currentScale = Convert.ToSingle(7.2);
                }

                sketchBox.Image = MainImage;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}";
                Console.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private bool StopClose()
        {
            bool preventClose = UnsavedChangesExist;
            if (preventClose)
            {
                string message = "Do you want to save changes?";
                DialogResult response = MessageBox.Show(message, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                switch (response)
                {
                    case DialogResult.Yes:
                        SaveChanges(WorkingParcel);
                        preventClose = false;
                        break;

                    case DialogResult.No:

                        preventClose = false;
                        DiscardChangesAndExit();

                        break;

                    default:
                        preventClose = UnsavedChangesExist;
                        break;
                }
            }
            return preventClose;
        }

        private void TotalGaragesAndCarports(SMSection s)
        {
            if (SketchUpLookups.GarageTypes.Contains(s.SectionType))
            {
                garageCount++;

                GarSize += s.SqFt;
            }
            if (SketchUpLookups.CarPortTypes.Contains(s.SectionType))
            {
                carportCount++;

                CPSize += (double)s.SqFt;
            }
        }

        private void UndoJumpLine(DrawOnlyLine dol)
        {
            DbStartX = dol.StartX;
            DbStartY = dol.StartY;
            ScaledStartPoint = dol.ScaledStartPoint;
            WorkingParcel.DrawOnlyLines.Remove(dol);

            RedrawSketch(WorkingParcel);
            MoveCursorToNewPoint(ScaledStartPoint);
        }

        private void UndoLine()
        {
            var centerPoint = new PointF(sketchBox.Width / 2, sketchBox.Height / 2);
            switch (EditState)
            {
                case DrawingState.Drawing:

                    if (WorkingSection != null)
                    {
                        if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
                        {
                            SMLine lastLine = WorkingSection.Lines.OrderByDescending(l => l.LineNumber).FirstOrDefault();
                            PointF lastPoint = lastLine.ScaledStartPoint;
                            WorkingSection.Lines.Remove(lastLine);
                            RedrawSketch(WorkingParcel, WorkingSection.SectionLetter);
                            MoveCursorToNewPoint(lastPoint);
                            DistText.Focus();
                        }
                        else if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
                        {
                            EditState = DrawingState.JumpPointSelected;
                            DrawOnlyLine lastJumpLine = WorkingParcel.DrawOnlyLines.OrderByDescending(l => l.LineNumber).FirstOrDefault();
                            PointF lastPoint = lastJumpLine.ScaledStartPoint;
                            WorkingParcel.DrawOnlyLines.Remove(lastJumpLine);
                            RedrawSketch(WorkingParcel, string.Empty);
                            MoveCursorToNewPoint(lastPoint);
                            DistText.Focus();
                        }
                    }
                    break;

                case DrawingState.JumpPointSelected:
                    if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
                    {
                        DrawOnlyLine lastJumpLine = WorkingParcel.DrawOnlyLines.OrderByDescending(l => l.LineNumber).FirstOrDefault();
                        PointF lastPoint = lastJumpLine.ScaledStartPoint;
                        WorkingParcel.DrawOnlyLines.Remove(lastJumpLine);
                        RedrawSketch(WorkingParcel, string.Empty);
                        MoveCursorToNewPoint(lastPoint);
                        DistText.Focus();
                    }
                    else
                    {
                        EditState = DrawingState.SectionAdded;
                        RedrawSketch(WorkingParcel, string.Empty);
                        MoveCursorToNewPoint(centerPoint);

                        btnBegin.Visible = true;
                        btnBegin.Enabled = true;
                        btnBegin.Focus();
                    }

                    break;

                case DrawingState.NewSketch:

                    MoveCursorToNewPoint(centerPoint);
                    break;
            }

            #region Original JMM Code

            //if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
            //{
            //    SMLine lastLine = (from l in WorkingSection.Lines.OrderByDescending(n => n.LineNumber) select l).FirstOrDefault();

            //    if (lastLine.LineNumber == 1)
            //    {
            //        RemoveFirstLineOrSection(lastLine);
            //    }
            //    else
            //    {
            //        RemoveLine(lastLine);
            //    }
            //}
            //else
            //{
            //    if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
            //    {
            //        DrawOnlyLine dol=WorkingParcel.DrawOnlyLines.OrderBy(l=>l.LineNumber).LastOrDefault();
            //        UndoJumpLine(dol);
            //    }
            //    else
            //    {
            //        RestartSectionSketch();
            //    }
            //}

            #endregion Original JMM Code

#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void UpdateCarportCountToZero()
        {
            string codeFromLookup = (from c in SketchUpLookups.CarPortTypeCollection where c.Description.Trim().ToUpper() == "NONE" select c.Code).FirstOrDefault();
            int cpCode = 0;
            int.TryParse(codeFromLookup, out cpCode);
            ParcelMast.CarportTypeCode = cpCode == 0 ? 67 : cpCode;
            ParcelMast.CarportNumCars = 0;
        }

        private void UpdateGarageCountToZero()
        {
            string codeFromLookup = (from c in SketchUpLookups.GarageTypeCollection where c.Description.Trim().ToUpper() == "NONE" select c.Code).FirstOrDefault();
            int garCode = 0;
            int.TryParse(codeFromLookup, out garCode);
            ParcelMast.Garage1TypeCode = (garCode == 0 ? 63 : garCode);
            ParcelMast.Garage1NumCars = 0;
            ParcelMast.Garage2TypeCode = 0;
            ParcelMast.Garage2NumCars = 0;
        }

        private void UpdateProgress(int progress)
        {
            progressBar.Value = progress;
        }

        #endregion "Private methods"

        #region "Properties"

        public List<SMLine> CornerConnectionLines { get; private set; }

        public bool GarageOrCarportAdded { get; set; }

        public List<SMLine> JumpConnectionLines { get; private set; }

        public bool SectionWasAdded { get; private set; }

        public string StatusText { get; private set; }

        #endregion "Properties"

        #region Methods

        private void BeginDrawing()
        {
            EditState = DrawingState.Drawing;
        }

        private void btnAddSection_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void btnAutoClose_Click(object sender, EventArgs e)
        {
            bool readyToClose = (WorkingSection != null && workingSection.Lines.Count > 2 && !WorkingSection.SectionIsClosed);
            if (readyToClose)
            {
                AutoCloseSection(WorkingSection);
                EditState = DrawingState.DoneDrawing;
            }
            else
            {
                EditState = DrawingState.Drawing;
            }
        }

        private void btnBegin_Click(object sender, EventArgs e)
        {
            BeginDrawing();
        }

        private void btnDeleteSketch_Click(object sender, EventArgs e)
        {
            DeleteSketch();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            CompleteDrawingNewSection();
        }

        private void btnEditSections_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            EditParcelSections();
            UseWaitCursor = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveChanges(WorkingParcel);
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            switch (EditState)
            {
                case DrawingState.Drawing:

                    UndoLine();
                    break;

                case DrawingState.DoneDrawing:
                    if (UnsavedChangesExist)
                    {
                        UndoLine();
                    }
                    break;

                case DrawingState.JumpPointSelected:

                    RefreshWorkspace();
                    EditState = DrawingState.SectionAdded;
                    break;

                case DrawingState.SectionAdded:
                    UnsavedChangesExist = false;
                    Close();
                    break;
            }
            SetButtonStates();
        }

        private void cmiFlipH_Click(object sender, EventArgs e)
        {
            FlipHorizontal();
        }

        private void cmiFlipV_Click(object sender, EventArgs e)
        {
            FlipVertically();
        }

        private void ExpandoSketch_Activated(object sender, EventArgs e)
        {
            WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
            RedrawSketch(SketchUpGlobals.ParcelWorkingCopy, WorkingSection != null ? WorkingSection.SectionLetter : string.Empty);
        }

        private void ExpandoSketch_Load(object sender, EventArgs e)
        {
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: Begin {MethodBase.GetCurrentMethod().Name}");
#endif
            RedrawSketch(SketchUpGlobals.ParcelWorkingCopy, string.Empty);
#if DEBUG || TEST

            Trace.WriteLine($"{DateTime.Now}: End {MethodBase.GetCurrentMethod().Name}");
#endif
        }

        private void miBeginDrawing_Click(object sender, EventArgs e)
        {
            EditState = DrawingState.Drawing;
        }

        private void miFlipHorizonal_Click(object sender, EventArgs e)
        {
            FlipHorizontal();
        }

        private void miFlipVertical_Click(object sender, EventArgs e)
        {
            FlipVertically();
        }

        private void miSelectSectionType_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void SaveSketch()
        {
            if (WorkingSection.SectionIsClosed)
            {
                UseWaitCursor = true;
                EditState = DrawingState.Saving;
                ShowEditStatus();
                Application.DoEvents();
                CompleteDrawingNewSection();
                SaveChanges(WorkingParcel);
                EditState = DrawingState.SketchSaved;
                ShowEditStatus();
                Application.DoEvents();
                UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        private void sketchBox_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                RefreshWorkspace();
                Application.DoEvents();
            }
        }

        #endregion Methods
    }
}