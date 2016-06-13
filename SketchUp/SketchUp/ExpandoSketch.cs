using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class ExpandoSketch : Form
    {
        public bool GarageOrCarportAdded
        {
            get
            {
                return garageOrCarportAdded;
            }

            set
            {
                garageOrCarportAdded = value;
            }
        }

        private bool garageOrCarportAdded;

        #region "Constructor"

        public ExpandoSketch(string sketchFolder, int sketchRecord, int sketchCard, bool hasSketch, bool hasNewSketch)
        {
            InitializeComponent();
            AddSectionContextMenu.Enabled = false;
            WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
            ShowVersion(WorkingParcel.SnapShotIndex);
            EditState = DrawingState.SketchLoaded;
            ShowWorkingCopySketch(sketchFolder, sketchRecord.ToString(), sketchCard.ToString(), hasSketch, hasNewSketch);
        }

        #endregion "Constructor"

        #region "Private methods"

        private static string LegalDirectionsMessage(List<string> legalDirections)
        {
            StringBuilder statusMessage = new StringBuilder();
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

        private void AddJumpLineToSketch(CamraDataEnums.CardinalDirection direction, decimal xLength, decimal yLength, decimal dbStartX, decimal dbStartY)
        {
            try
            {
                if (LegalMoveDirections.Contains(direction.ToString()))
                {
                    DrawOnlyLine newLine = new DrawOnlyLine(AttachmentSection);

                    newLine.LineNumber = AttachmentSection.Lines.Count + 1;
                    newLine.Direction = direction.ToString().ToUpper();
                    newLine.StartX = DbStartX;
                    newLine.StartY = dbStartY;
                    newLine.EndX = newLine.StartX + xLength;
                    newLine.EndY = newLine.StartY + yLength;
                    newLine.XLength = xLength;
                    newLine.YLength = yLength;
                    WorkingParcel.DrawOnlyLines.Add(newLine);
                    WorkingSection.ParentParcel = WorkingParcel;
                }
                else
                {
                    string statusMessage = string.Format("You must move along a line attached to the starting corner. \n\n{0}", LegalDirectionsMessage(LegalMoveDirections));
                    MessageBox.Show(statusMessage);
                }
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void AddJumpTableRow(float jx, float jy, float CurrentScale, SMLine line)
        {
            try
            {
                decimal Distance = 0;

                DataRow row = JumpTable.NewRow();
                row["Record"] = line.Record;
                row["Card"] = line.Card;
                row["Sect"] = line.SectionLetter;
                row["LineNo"] = line.LineNumber;
                row["Direct"] = line.Direction;
                row["XLen"] = line.XLength;
                row["YLen"] = line.YLength;
                row["Length"] = line.LineLength;
                row["Angle"] = line.LineAngle;
                row["XPt1"] = line.StartX;
                row["YPt1"] = line.StartY;
                row["XPt2"] = line.EndX;
                row["YPt2"] = line.EndY;
                row["Attach"] = line.AttachedSection;

                float xPt2 = (float)line.EndX;
                float yPt2 = (float)line.EndY;

                float xPoint = (ScaleBaseX + (Convert.ToSingle(xPt2) * CurrentScale));
                float yPoint = (ScaleBaseY + (Convert.ToSingle(yPt2) * CurrentScale));

                float xDiff = (jx - xPoint);
                float yDiff = (jy - yPoint);

                double xDiffSquared = Math.Pow(Convert.ToDouble(xDiff), 2);
                double yDiffSquared = Math.Pow(Convert.ToDouble(yDiff), 2);

                Distance = Convert.ToDecimal(Math.Sqrt(Math.Pow(Convert.ToDouble(xDiff), 2) + Math.Pow(Convert.ToDouble(yDiff), 2)));

                row["Dist"] = Distance;

                JumpTable.Rows.Add(row);
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Console.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void AddLineToSketch(CamraDataEnums.CardinalDirection direction, decimal xLength, decimal yLength, decimal dbStartX, decimal dbStartY)
        {
            try
            {
                SMLine newLine = new SMLine(WorkingSection);
                newLine.LineNumber = WorkingSection.Lines.Count + 1;
                newLine.Direction = direction.ToString().ToUpper();
                newLine.StartX = DbStartX;
                newLine.StartY = DbStartY;
                newLine.EndX = newLine.StartX + xLength;
                newLine.EndY = newLine.StartY + yLength;
                WorkingSection.Lines.Add(newLine);
                WorkingSection.ParentParcel = WorkingParcel;
                SetButtonStates(EditState);
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void AddListItemsToJumpTableList(float jx, float jy, decimal CurrentScale, List<SMLine> lines)
        {
            try
            {
                foreach (SMLine l in lines)
                {
                    AddJumpTableRow(jx, jy, (float)CurrentScale, l);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private void AddMoveToImage()
        {
            try
            {
                AddMoveToImage();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddParcelToSnapshots(SMParcel parcel)
        {
            SketchRepository sr = new SketchRepository(parcel);
            WorkingParcel = sr.AddSketchToSnapshots(parcel);
            ShowVersion(WorkingParcel.SnapShotIndex);
        }

        private void AddSection()
        {
            GetSectionTypeInfo();
            _deleteMaster = false;
            _isClosed = false;
            SetButtonStates(EditState);
        }

        private void AddSectionBtn_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void AdjustLengthDirection(CamraDataEnums.CardinalDirection moveDirection, ref decimal xLength, ref decimal ylength)
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

                default:
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
                var linesWithStart = (from l in AttachmentSection.Lines where SMGlobal.PointIsOnLine(l.StartPoint, l.EndPoint, wsStart) select l).ToList();

                if (linesWithStart != null && linesWithStart.Count > 1)
                {
                    SMLine anchorLine = (from l in linesWithStart where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
                }
                else
                {
                    SMLine anchorLine = linesWithStart.First();
                    SMLineManager slm = new SMLineManager();
#if DEBUG
                    DevUtilities du = new DevUtilities();
                    Trace.WriteLine("\nBefore Break: \n");
                    Trace.WriteLine(du.ParcelInfo(WorkingParcel));
#endif

                    AttachmentSection = slm.SectionWithLineBreak(AttachmentSection, anchorLine.LineNumber, wsStart);
                    anchorLine = (from l in AttachmentSection.Lines where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
#if DEBUG
                    Trace.WriteLine("\nAfter Break: \n");
                    Trace.WriteLine(du.ParcelInfo(WorkingParcel));
                    Trace.Flush();
#endif
                }
            }
        }

        private void AutoCloseSection(SMSection section)
        {
            SMLine firstLine = (from l in section.Lines.OrderBy(n => n.LineNumber) select l).FirstOrDefault();
            SMLine lastLine = (from l in section.Lines.OrderBy(n => n.LineNumber) select l).LastOrDefault();

            PointF closeLineStart = lastLine.EndPoint;
            PointF closeLineEnd = firstLine.StartPoint;
            decimal xLength = (decimal)Math.Round(Math.Abs(closeLineStart.X - closeLineEnd.X), 2);
            decimal yLength = (decimal)Math.Round(Math.Abs(closeLineStart.Y - closeLineEnd.Y), 2);

            CamraDataEnums.CardinalDirection direction = SMGlobal.CalculateLineDirection(closeLineStart, closeLineEnd);
            AdjustLengthDirection(direction, ref xLength, ref yLength);
            AddLineToSketch(direction, xLength, yLength, (decimal)closeLineStart.X, (decimal)closeLineStart.Y);
            RedrawSketch(WorkingParcel);
            if (WorkingSection.SectionIsClosed)
            {
                EditState = DrawingState.DoneDrawing;
            }
            else
            {
                EditState = DrawingState.Drawing;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            switch (editState)
            {
                case DrawingState.DoneDrawing:
                    if (GarageOrCarportAdded)
                    {
                        MessageBox.Show("Will check for missing garage data");
                    }
                    SaveChanges(WorkingParcel);
                    EditState = DrawingState.SketchSaved;
                    break;

                case DrawingState.Drawing:
                    if (WorkingSection.SectionIsClosed)
                    {
                        CompleteDrawingNewSection();
                        EditState = DrawingState.DoneDrawing;
                    }
                    else
                    {
                        AutoCloseSection(WorkingSection);
                    }

                    break;

                case DrawingState.JumpPointSelected:
                case DrawingState.SectionAdded:
                    EditState = DrawingState.Drawing;
                    UnsavedChangesExist = true;
                    break;

                case DrawingState.SectionDeleted:
                    UnsavedChangesExist = true;
                    SaveChanges(WorkingParcel);
                    break;

                case DrawingState.SectionEditCompleted:
                    UnsavedChangesExist = true;
                    SaveChanges(WorkingParcel);
                    break;

                case DrawingState.SectionEditStarted:
                    UnsavedChangesExist = true;

                    break;

                case DrawingState.SketchLoaded:
                    AddSection();
                    break;

                case DrawingState.SketchSaved:
                    UnsavedChangesExist = false;

                    break;

                default:
                    break;
            }

            SetButtonStates(EditState);
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Console.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private void calculateNewArea(int record, int card, string nextsec)
        {
            StringBuilder getLine = new StringBuilder();
            getLine.Append("select jlpt1x,jlpt1y,jlpt2x,jlpt2Y ");
            getLine.Append(string.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card));
            getLine.Append(string.Format("and jlsect = '{0}' ", nextsec));

            DataSet arealines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

            AreaTable.Clear();

            for (int i = 0; i < arealines.Tables[0].Rows.Count; i++)
            {
                DataRow row = AreaTable.NewRow();
                row["XPt1"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt1x"].ToString());
                row["YPt1"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt1y"].ToString());
                row["XPt2"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt2x"].ToString());
                row["YPt2"] = Convert.ToDecimal(arealines.Tables[0].Rows[i]["jlpt2Y"].ToString());

                AreaTable.Rows.Add(row);
            }

            decimal sumareacalc = 0;
            decimal x1 = 0;
            decimal y2 = 0;
            decimal y1 = 0;
            decimal x2 = 0;

            for (int i = 0; i < AreaTable.Rows.Count; i++)
            {
                x1 = Convert.ToDecimal(AreaTable.Rows[i]["XPt1"].ToString());

                if ((i + 1) == AreaTable.Rows.Count)
                {
                    y2 = Convert.ToDecimal(AreaTable.Rows[0]["YPt1"].ToString());
                }
                if (i < AreaTable.Rows.Count && (i + 1) != AreaTable.Rows.Count)
                {
                    y2 = Convert.ToDecimal(AreaTable.Rows[i + 1]["YPt1"].ToString());
                }

                sumareacalc = sumareacalc + (x1 * y2);
            }

            for (int i = 0; i < AreaTable.Rows.Count; i++)
            {
                y1 = Convert.ToDecimal(AreaTable.Rows[i]["YPt1"].ToString());

                if ((i + 1) == AreaTable.Rows.Count)
                {
                    x2 = Convert.ToDecimal(AreaTable.Rows[0]["XPt1"].ToString());
                }

                if (i < AreaTable.Rows.Count && (i + 1) < AreaTable.Rows.Count)
                {
                    x2 = Convert.ToDecimal(AreaTable.Rows[i + 1]["XPt1"].ToString());
                }

                sumareacalc = sumareacalc - (y1 * x2);
            }

            _calcNextSectArea = Math.Round(Convert.ToDecimal((sumareacalc / 2.0m)), 1);

            if (_calcNextSectArea < 0)
            {
                _calcNextSectArea = (_calcNextSectArea * -1);
            }
        }

        private void changeSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void cmiAddClosingLine_Click(object sender, EventArgs e)
        {
            AutoCloseSection(WorkingSection);
        }

        private void cmiBeginDrawing_Click(object sender, EventArgs e)
        {
            EditState = DrawingState.Drawing;
            SetButtonStates(EditState);
        }

        private void CompleteDrawingNewSection()
        {
            try
            {
                UnsavedChangesExist = true;
                AttachNewSectionToSketch();
                RefreshWorkspace();
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void ComputeSectionArea()
        {
            var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
            var sectionArea = sectionPolygon.Area;

            calculateNewArea(SketchUpGlobals.Record, SketchUpGlobals.Card, NextSectLtr);

            if (_nextStoryHeight < 1.0m)
            {
                _nextStoryHeight = 1;
            }
            if (_nextStoryHeight >= 1.0m)
            {
                NextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
            }

            NextSectArea = (Math.Round((_calcNextSectArea * _nextStoryHeight), 1));
        }

        private bool ConfirmCarportNumbers()
        {
            bool carportUpdateNeeded = false;
            decimal carportSize = 0;
            var carportSections = (from s in WorkingParcel.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).ToList();
            if (carportSections != null)
            {
                carportCount = carportSections.Count();
                carportSize = carportSections.Sum(s => s.SqFt);
            }
            string noCpCode = (from c in SketchUpLookups.CarPortTypeCollection where c.Description == "NONE" select c.Code).FirstOrDefault();
            int noCarportCode = 0;
            int.TryParse(noCpCode, out noCarportCode);

            bool codeAndNumbersMismatched = (carportCount > 0 && (ParcelMast.CarportNumCars == 0 || parcelMast.CarportTypeCode == noCarportCode));

            bool carsCountUpdateNeeded = carportCount > 1 && (ParcelMast.CarportTypeCode != 0 || ParcelMast.CarportTypeCode != noCarportCode);
            carportUpdateNeeded = (codeAndNumbersMismatched || carsCountUpdateNeeded);
         
            if (codeAndNumbersMismatched)
            {
                MissingGarageData missingGarCPForm = new MissingGarageData(parcelMast);
                missingGarCPForm.ShowDialog();
            }

            if (carsCountUpdateNeeded)
            {
                MissingGarageData missCPx = new MissingGarageData(parcelMast);
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
                    MissingGarageData missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (missGar.Garage1Code != originalParcelMast.Garage1TypeCode)
                    {
                        parcelMast.Garage1NumCars = missGar.Gar1NumCars;
                        parcelMast.Garage1TypeCode = missGar.Garage1Code;
                    }
                }
                if (garageCount > 1 && parcelMast.Garage2NumCars == 0)
                {
                    MissingGarageData missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
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

        private DataTable ConstructAreaTable()
        {
            DataTable at = new DataTable();
            at.Columns.Add("XPt1", typeof(decimal));
            at.Columns.Add("YPt1", typeof(decimal));
            at.Columns.Add("XPt2", typeof(decimal));
            at.Columns.Add("YPt2", typeof(decimal));
            return at;
        }

        private DataTable ConstructAttachmentPointsDataTable()
        {
            DataTable atpt = new DataTable();
            atpt.Columns.Add("Sect", typeof(string));
            atpt.Columns.Add("X1", typeof(decimal));
            atpt.Columns.Add("Y1", typeof(decimal));
            atpt.Columns.Add("X2", typeof(decimal));
            atpt.Columns.Add("Y2", typeof(decimal));
            return atpt;
        }

        private DataTable ConstructAttachPointsDataTable()
        {
            DataTable atpt = new DataTable();
            atpt.Columns.Add("RecNo", typeof(int));
            atpt.Columns.Add("CardNo", typeof(int));
            atpt.Columns.Add("Sect", typeof(string));
            atpt.Columns.Add("Direct", typeof(string));
            atpt.Columns.Add("Xpt1", typeof(decimal));
            atpt.Columns.Add("Ypt1", typeof(decimal));
            atpt.Columns.Add("Xpt2", typeof(decimal));
            atpt.Columns.Add("Ypt2", typeof(decimal));
            atpt.Columns.Add("Attch", typeof(string));
            return atpt;
        }

        private DataTable ConstructDisplayDataTable()
        {
            DataTable displayDT = new DataTable();

            DataColumn col_sect = new DataColumn("Dir", Type.GetType("System.String"));
            displayDT.Columns.Add(col_sect);
            DataColumn col_desc = new DataColumn("North", Type.GetType("System.Decimal"));
            displayDT.Columns.Add(col_desc);
            DataColumn col_sqft = new DataColumn("East", Type.GetType("System.Decimal"));
            displayDT.Columns.Add(col_sqft);
            DataColumn col_att = new DataColumn("Att", Type.GetType("System.String"));
            displayDT.Columns.Add(col_att);

            DataGridTableStyle style = new DataGridTableStyle();
            DataGridTextBoxColumn SectColumn = new DataGridTextBoxColumn();
            SectColumn.MappingName = "Dir";
            SectColumn.HeaderText = "Dir";
            SectColumn.Width = 30;
            style.GridColumnStyles.Add(SectColumn);

            DataGridTextBoxColumn DescColumn = new DataGridTextBoxColumn();
            DescColumn.MappingName = "North";
            DescColumn.HeaderText = "North";
            DescColumn.Width = 50;
            style.GridColumnStyles.Add(DescColumn);

            DataGridTextBoxColumn SqftColumn = new DataGridTextBoxColumn();
            SqftColumn.MappingName = "East";
            SqftColumn.HeaderText = "East";
            SqftColumn.Width = 50;
            style.GridColumnStyles.Add(SqftColumn);

            DataGridTextBoxColumn AttColumn = new DataGridTextBoxColumn();
            AttColumn.MappingName = "Att";
            AttColumn.HeaderText = "Att";
            AttColumn.Width = 30;
            style.GridColumnStyles.Add(AttColumn);
            return displayDT;
        }

        private DataTable ConstructDupAttPointsTable()
        {
            DataTable dupAtPt = new DataTable();
            dupAtPt.Columns.Add("RecNo", typeof(int));
            dupAtPt.Columns.Add("CardNo", typeof(int));
            dupAtPt.Columns.Add("Sect", typeof(string));
            dupAtPt.Columns.Add("LineNo", typeof(int));
            dupAtPt.Columns.Add("Direct", typeof(string));
            dupAtPt.Columns.Add("Xpt1", typeof(decimal));
            dupAtPt.Columns.Add("Ypt1", typeof(decimal));
            dupAtPt.Columns.Add("Xpt2", typeof(decimal));
            dupAtPt.Columns.Add("Ypt2", typeof(decimal));
            dupAtPt.Columns.Add("Attch", typeof(string));
            dupAtPt.Columns.Add("Index", typeof(int));
            return dupAtPt;
        }

        private DataTable ConstructJumpTable()
        {
            DataTable jt = new DataTable();
            jt.Columns.Add("Record", typeof(int));
            jt.Columns.Add("Card", typeof(int));
            jt.Columns.Add("Sect", typeof(string));
            jt.Columns.Add("LineNo", typeof(int));
            jt.Columns.Add("Direct", typeof(string));
            jt.Columns.Add("XLen", typeof(decimal));
            jt.Columns.Add("YLen", typeof(decimal));
            jt.Columns.Add("Length", typeof(decimal));
            jt.Columns.Add("Angle", typeof(decimal));
            jt.Columns.Add("XPt1", typeof(decimal));
            jt.Columns.Add("YPt1", typeof(decimal));
            jt.Columns.Add("XPt2", typeof(decimal));
            jt.Columns.Add("YPt2", typeof(decimal));
            jt.Columns.Add("Attach", typeof(string));
            jt.Columns.Add("Dist", typeof(decimal));
            return jt;
        }

        private DataTable ConstructMulPtsTable()
        {
            DataTable mp = new DataTable();
            mp.Columns.Add("Sect", typeof(string));
            mp.Columns.Add("Line", typeof(int));
            mp.Columns.Add("X1", typeof(decimal));
            mp.Columns.Add("Y1", typeof(decimal));
            mp.Columns.Add("X2", typeof(decimal));
            mp.Columns.Add("Y2", typeof(decimal));
            return mp;
        }

        private DataTable ConstructREJumpTable()
        {
            DataTable ret = new DataTable();
            try
            {
                ret.Columns.Add("Record", typeof(int));
                ret.Columns.Add("Card", typeof(int));
                ret.Columns.Add("Sect", typeof(string));
                ret.Columns.Add("LineNo", typeof(int));
                ret.Columns.Add("Direct", typeof(string));
                ret.Columns.Add("XLen", typeof(decimal));
                ret.Columns.Add("YLen", typeof(decimal));
                ret.Columns.Add("Length", typeof(decimal));
                ret.Columns.Add("Angle", typeof(decimal));
                ret.Columns.Add("XPt1", typeof(decimal));
                ret.Columns.Add("YPt1", typeof(decimal));
                ret.Columns.Add("XPt2", typeof(decimal));
                ret.Columns.Add("YPt2", typeof(decimal));
                ret.Columns.Add("Attach", typeof(string));
                ret.Columns.Add("Dist", typeof(decimal));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
            return ret;
        }

        private DataTable ConstructRESpJumpTable()
        {
            DataTable resp = new DataTable();
            resp.Columns.Add("Record", typeof(int));
            resp.Columns.Add("Card", typeof(int));
            resp.Columns.Add("Sect", typeof(string));
            resp.Columns.Add("LineNo", typeof(int));
            resp.Columns.Add("Direct", typeof(string));
            resp.Columns.Add("XLen", typeof(decimal));
            resp.Columns.Add("YLen", typeof(decimal));
            resp.Columns.Add("Length", typeof(decimal));
            resp.Columns.Add("Angle", typeof(decimal));
            resp.Columns.Add("XPt1", typeof(decimal));
            resp.Columns.Add("YPt1", typeof(decimal));
            resp.Columns.Add("XPt2", typeof(decimal));
            resp.Columns.Add("YPt2", typeof(decimal));
            resp.Columns.Add("Attach", typeof(string));
            resp.Columns.Add("Dist", typeof(decimal));
            return resp;
        }

        private DataTable ConstructSectionLtrs()
        {
            DataTable sl = new DataTable();
            sl.Columns.Add("RecNo", typeof(int));
            sl.Columns.Add("CardNo", typeof(int));
            sl.Columns.Add("CurSecLtr", typeof(string));
            sl.Columns.Add("NewSecLtr", typeof(string));
            sl.Columns.Add("NewType", typeof(string));
            sl.Columns.Add("SectSize", typeof(decimal));
            return sl;
        }

        private DataTable ConstructSectionTable()
        {
            DataTable st = new DataTable();

            st.Columns.Add("Record", typeof(int));
            st.Columns.Add("Card", typeof(int));
            st.Columns.Add("Sect", typeof(string));
            st.Columns.Add("LineNo", typeof(int));
            st.Columns.Add("Direct", typeof(string));
            st.Columns.Add("XLen", typeof(decimal));
            st.Columns.Add("YLen", typeof(decimal));
            st.Columns.Add("Length", typeof(decimal));
            st.Columns.Add("Angle", typeof(decimal));
            st.Columns.Add("XPt1", typeof(decimal));
            st.Columns.Add("YPt1", typeof(decimal));
            st.Columns.Add("XPt2", typeof(decimal));
            st.Columns.Add("YPt2", typeof(decimal));
            st.Columns.Add("Attach", typeof(string));
            return st;
        }

        private DataTable ConstructSortDistanceTable()
        {
            sortDist = new DataTable();
            sortDist.Columns.Add("Sect", typeof(string));
            sortDist.Columns.Add("Line", typeof(int));
            sortDist.Columns.Add("Direct", typeof(string));
            sortDist.Columns.Add("Xdist", typeof(decimal));
            sortDist.Columns.Add("Ydist", typeof(decimal));
            sortDist.Columns.Add("Length", typeof(decimal));
            return sortDist;
        }

        private DataTable ConstructStartPointsTable()
        {
            DataTable startPts = new DataTable();
            startPts.Columns.Add("Sect", typeof(string));
            startPts.Columns.Add("Sx1", typeof(decimal));
            startPts.Columns.Add("Sy1", typeof(decimal));
            return startPts;
        }

        private DataTable ConstructUndoPointsTable()
        {
            undoPoints = new DataTable();
            undoPoints.Columns.Add("Direct", typeof(string));
            undoPoints.Columns.Add("X1pt", typeof(int));
            undoPoints.Columns.Add("Y1pt", typeof(int));
            undoPoints.Columns.Add("X2pt", typeof(int));
            undoPoints.Columns.Add("Y2pt", typeof(int));
            return undoPoints;
        }

        private int CountLines(string thisSection)
        {
            string lineCountSql = string.Format("select count(*) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card, thisSection);

            int lineCount = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCountSql.ToString()));
            return lineCount;
        }

        private int CountSections()
        {
            try
            {
                string seccnt = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card);

                int secItemCnt = 0;
                Int32.TryParse(dbConn.DBConnection.ExecuteScalar(seccnt).ToString(), out secItemCnt);
                return secItemCnt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));

                throw;
            }
        }

        private void deleteExistingSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void DeleteLineSection()
        {
            StringBuilder deletelinesect = new StringBuilder();
            deletelinesect.Append(string.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                           SketchUpGlobals.LocalityPrefix,

                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            CurrentSecLtr));

            dbConn.DBConnection.ExecuteNonSelectStatement(deletelinesect.ToString());
        }

        private void DeleteSketch()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
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
            Close();
        }

        private void DisplayStatus(string statusText)
        {
            sketchStatusMessage.Text = statusText;
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
            if (_isKeyValid == true)
            {
                e.Handled = true;
            }
        }

        private void DistText_Leave(object sender, EventArgs e)
        {
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        private void DMouseClick()
        {
            StartX = _mouseX;
            StartY = _mouseY;
            BaseX = _mouseX;
            BaseY = _mouseY;
            PrevX = _mouseX;
            PrevY = _mouseY;
            EndX = 0;
            EndY = 0;
            txtLoc = 0;
            txtX = 0;
            txtY = 0;
            _lenString = string.Empty;
            LastDir = string.Empty;

            if (_undoMode == true)
            {
                string reopestr = SketchUpGlobals.ReOpenSection;

                UndoLine();
            }
            if (_undoMode == false)
            {
                DistText.Focus();
            }
        }

        private void DMouseMove(int X, int Y, bool jumpMode)
        {
        }

        //ToDo: Begin here to hook in parcel updates
        private void DrawingDoneBtn_Click(object sender, EventArgs e)
        {
            CompleteDrawingNewSection();
        }

        private void EditParcelSections()
        {
            throw new NotImplementedException();
        }

        private void EraseSectionFromDrawing(SMSection section)
        {
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.Sections.Remove(section);
            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = StopClose();
        }

        private void ExpandoSketch_Shown(object sender, EventArgs e)
        {
        }

        private void ExpandoSketchTools_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
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

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;
                _mouseX = e.X;
                _mouseY = e.Y;

                // TODO: Remove if not needed:	   DMouseMove(e.X, e.Y, true);
            }
        }

        private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;

                _mouseX = e.X;
                _mouseY = e.Y;
                DMouseMove(e.X, e.Y, true);
            }
        }

        private void FlipHorizontal()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapShotIndex++;
            CamraDataEnums.CardinalDirection dir;
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
                l.StartX = l.StartX * -1M;
                l.EndX = l.EndX * -1M;
            }
            AddParcelToSnapshots(WorkingParcel);

            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private void FlipUpDown()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapShotIndex++;
            CamraDataEnums.CardinalDirection dir;
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
                l.StartY = l.StartY * -1M;
                l.EndY = l.EndY * -1M;
            }
            AddParcelToSnapshots(WorkingParcel);
            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private AngleVector GetAngleLine(string distance, CamraDataEnums.CardinalDirection direction)
        {
            _isAngle = true;
            AngleVector angle = new AngleVector();
            decimal xDistance = 0.00M;
            decimal yDistance = 0.00M;
            angle = ParseAngleEntry(distance);
            xDistance = angle.XLength;
            yDistance = angle.YLength;
            return angle;
        }

        private List<string> GetLegalMoveDirections(PointF startPoint, string attachSectionLetter)
        {
            List<string> legalDirections = new List<string>();
            List<string> startPointLineDirections = new List<string>();
            List<string> endPointLineDirections = new List<string>();

            try
            {
                List<SMLine> linesWithJumpPoint = (from l in WorkingParcel.AllSectionLines where l.SectionLetter == attachSectionLetter && (l.ScaledStartPoint == startPoint || l.ScaledEndPoint == startPoint) select l).ToList();

                startPointLineDirections.AddRange((from l in linesWithJumpPoint where l.ScaledStartPoint == startPoint select l.Direction).ToList());
                endPointLineDirections.AddRange((from l in linesWithJumpPoint where l.ScaledEndPoint == startPoint && l.SectionLetter == attachSectionLetter select SMGlobal.ReverseDirection(l.Direction)).ToList());
                legalDirections.AddRange(startPointLineDirections.Distinct());
                legalDirections.AddRange(endPointLineDirections.Distinct());

                string statusMessage = LegalDirectionsMessage(legalDirections);

                sketchStatusMessage.Text = statusMessage.ToString();
                return legalDirections.Distinct().ToList();
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private DataSet GetLinesData(int crrec, int crcard)
        {
            DataSet lines;
            string getLine = string.Format("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, crrec, crcard);

            lines = dbConn.DBConnection.RunSelectStatement(getLine);
            return lines;
        }

        private DataSet GetSectionLines(int crrec, int crcard)
        {
            DataSet lines;
            StringBuilder getLine = new StringBuilder();
            getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            getLine.Append(string.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                       SketchUpGlobals.LocalLib,
                       SketchUpGlobals.LocalityPrefix,

                        crrec,
                        crcard));
            getLine.Append("and jldirect <> 'X' ");

            lines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());
            return lines;
        }

        private int GetSectionsCount()
        {
            StringBuilder checkSect = new StringBuilder();
            checkSect.Append(string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            NextSectLtr));

            int secCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(checkSect.ToString()));
            return secCnt;
        }

        private void GetSectionTypeInfo()
        {
            _addSection = true;

            string nextSectionLetter = WorkingParcel.NextSectionLetter;
            NewSectionPoints.Clear();
            lineCnt = 0;
            SelectSectionTypeDialog sectionTypeForm = new SelectSectionTypeDialog(WorkingParcel.ParcelMast, _addSection, lineCnt, IsNewSketch);

            sectionTypeForm.ShowDialog(this);
            if (sectionTypeForm.SectionWasAdded)

            {
                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
                NextSectLtr = WorkingParcel.NextSectionLetter;
                _nextSectType = SelectSectionTypeDialog._nextSectType;
                _nextStoryHeight = SelectSectionTypeDialog.newSectionStoreys;
                _nextLineCount = SelectSectionTypeDialog._nextLineCount;
                _hasNewSketch = (NextSectLtr == "A");

                AddSectionContextMenu.Enabled = true;
                cmiJumpToCorner.Enabled = true;
                EditState = DrawingState.SectionAdded;
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
                AddSectionContextMenu.Enabled = false;
                cmiJumpToCorner.Enabled = false;
                EditState = DrawingState.SketchLoaded;
                UnsavedChangesExist = sectionTypeForm.SectionWasAdded;
            }
        }

        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            CamraDataEnums.CardinalDirection direction;
            direction = DirectionOfKeyEntered(e);
            string distanceText = string.Empty;
            decimal distanceValueEntered = 0.00M;
            decimal yLength = 0.00M;
            decimal xLength = 0.00M;
            AngleVector angle = new AngleVector();
            if (!string.IsNullOrEmpty(DistText.Text))
            {
                distanceText = DistText.Text;

                if (distanceText.IndexOf(",") > 0)
                {
                    angle = GetAngleLine(distanceText, direction);
                    direction = angle.AngledLineDirection;
                    xLength = angle.XLength;
                    yLength = angle.YLength;
                }
                else
                {
                    decimal.TryParse(distanceText, out distanceValueEntered);
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
                        AddLineToSketch(direction, xLength, yLength, DbStartX, DbStartY);
                        WorkingParcel.DrawOnlyLines.Clear();

                        RedrawSketch(WorkingParcel);
                        if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
                        {
                            SMLine newLine = (from l in WorkingSection.Lines.OrderBy(l => l.LineNumber) select l).LastOrDefault();
                            DbStartPoint = newLine.EndPoint;
                            dbStartX = newLine.EndX;
                            dbStartY = newLine.EndY;
                            ScaledStartPoint = newLine.ScaledEndPoint;

                            // TODO: Remove if not needed:
                            StartOfCurrentLine = ScaledStartPoint;
                            SetButtonStates(EditState);
                        }

                        break;

                    case DrawingState.JumpPointSelected:
                        AddJumpLineToSketch(direction, xLength, yLength, DbStartX, DbStartY);
                        RedrawSketch(WorkingParcel);
                        if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
                        {
                            DrawOnlyLine newLine = (from l in WorkingParcel.DrawOnlyLines.OrderBy(l => l.LineNumber) select l).LastOrDefault();
                            DbStartPoint = newLine.EndPoint;
                            dbStartX = newLine.EndX;
                            dbStartY = newLine.EndY;
                            ScaledStartPoint = newLine.ScaledEndPoint;
                        }

                        break;

                    case DrawingState.SectionAdded:
                    case DrawingState.SketchLoaded:

                    default:
                        _isKeyValid = false;
                        break;
                }
            }
        }

        private void HandleNonArrowKeys(KeyEventArgs e)
        {
            bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
            if (notNumPad)
            {
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
                        UndoLine();
                        _isKeyValid = false;
                    }
                }
            }
        }

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

            // TODO: Remove if not needed:
        }

        private void InitializeDisplayDataGrid()
        {
            displayDataTable = ConstructDisplayDataTable();

            dgSections.DataSource = displayDataTable;
        }

        private void InsertLine(string CurAttDir, decimal newEndX, decimal newEndY, decimal StartEndX, decimal StartEndY, decimal splitLength)
        {
            StringBuilder insertLine = new StringBuilder();
            insertLine.Append(string.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen, ",
                      SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
            insertLine.Append("jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ) values ( ");
            insertLine.Append(string.Format(" {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' )", SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr,
                mylineNo,
                CurAttDir,
                Math.Abs(adjNewSecX),
                Math.Abs(adjNewSecY),
                Math.Abs(splitLength),
                0,
                StartEndX,
                StartEndY,
                newEndX,
                newEndY,
                NextSectLtr
                ));

            dbConn.DBConnection.ExecuteNonSelectStatement(insertLine.ToString());
        }

        private void InsertMasterRecord(decimal summedArea, decimal baseStory, DataSet ds_master)
        {
            if (ds_master.Tables[0].Rows.Count == 0)
            {
                StringBuilder insMaster = new StringBuilder();
                insMaster.Append(string.Format("insert into {0}.{1}master (jmrecord,jmdwell,jmsketch,jmstory,jmstoryex,jmscale,jmtotsqft,jmesketch) ",
                              SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPrefix

                                ));
                insMaster.Append(string.Format("values ({0},{1},'{2}',{3},'{4}',{5},{6},'{7}' ) ",
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            "Y",
                            baseStory,
                            string.Empty,
                            1.00,
                            summedArea,
                            string.Empty));

                dbConn.DBConnection.ExecuteNonSelectStatement(insMaster.ToString());
            }
        }

        private bool IsValidDirection(string moveDirection)
        {
            bool goodDir = (LegalMoveDirections.Contains(moveDirection) || btnAdd.Text == "Active" || !checkDirection);
            return goodDir;
        }

        private void JumptoCorner()
        {
            decimal scale = WorkingParcel.Scale;
            PointF origin = WorkingParcel.SketchOrigin;
            CurrentSecLtr = string.Empty;

            // TODO: Remove if not needed:	_newIndex = 0;
            currentAttachmentLine = 0;
            if (IsNewSketch == false)
            {
                PointF mouseLocation = new PointF(_mouseX, _mouseY);

                List<SMLine> connectionLines = LinesWithClosestEndpoints(mouseLocation);
                bool sketchHasLineData = (connectionLines.Count > 0);
                if (connectionLines == null || connectionLines.Count == 0)
                {
                    string message = string.Format("No lines contain an available connection point from point {0},{1}", _mouseX, _mouseY);

                    Console.WriteLine(message);

#if DEBUG

                    MessageBox.Show(message);
#endif
                    throw new InvalidDataException(message);
                }
                else
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
                    LegalMoveDirections = GetLegalMoveDirections(ScaledJumpPoint, AttSectLtr);
                    SetReadyButtonAppearance();
                    _isJumpMode = true;
                    EditState = DrawingState.JumpPointSelected;
                    ScaledStartPoint = ScaledJumpPoint;
                    DbStartX = Math.Round((decimal)DbJumpPoint.X, 2);
                    DbStartY = Math.Round((decimal)DbJumpPoint.Y, 2);
                    DistText.Focus();
                    MoveCursorToNewPoint(ScaledJumpPoint);
                }
            }
        }

        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isJumpMode || EditState == DrawingState.SectionAdded)
            {
                int jx = _mouseX;
                int jy = _mouseY;

                float _scaleBaseX = ScaleBaseX;
                float _scaleBaseY = ScaleBaseY;
                float CurrentScale = _currentScale;

                draw = false;
                IsNewSketch = false;

                JumptoCorner();

                UndoJump = false;
            }
            _isJumpMode = false;
        }

        private List<SMLine> LinesWithClosestEndpoints(PointF mouseLocation)
        {
            foreach (SMLine l in WorkingParcel.AllSectionLines.Where(s => s.SectionLetter != WorkingParcel.LastSectionLetter))
            {
                l.ComparisonPoint = mouseLocation;
            }

            decimal shortestDistance = Math.Round((from l in WorkingParcel.AllSectionLines select l.EndPointDistanceFromComparisonPoint).Min(), 2);
            List<SMLine> connectionLines = (from l in WorkingParcel.AllSectionLines where Math.Round(l.EndPointDistanceFromComparisonPoint, 2) == shortestDistance select l).ToList();

            return connectionLines;
        }

        private void LoadSectionLinesGrid(string sectionLetter)
        {
            displayDataTable.Rows.Clear();
            if (WorkingParcel.SelectSectionByLetter(sectionLetter).Lines != null)
            {
                foreach (SMLine line in WorkingParcel.SelectSectionByLetter(sectionLetter).Lines)
                {
                    DataRow dr = displayDataTable.NewRow();
                    dr["Dir"] = line.Direction.Trim();
                    dr["North"] = line.YLength.ToString();
                    dr["East"] = line.XLength.ToString();
                    dr["Att"] = line.AttachedSection.Trim();
                    displayDataTable.Rows.Add(dr);
                }
            }
        }

        private void LoadStartPointsDataTable()
        {
            List<SMLine> startLines = (from l in WorkingParcel.AllSectionLines where l.LineNumber == 1 select l).OrderBy(s => s.SectionLetter).ToList();

            StrtPts.Clear();

            foreach (SMLine l in startLines)
            {
                DataRow row = StrtPts.NewRow();
                row["Sect"] = l.SectionLetter;
                row["Sx1"] = l.StartX;
                row["Sy1"] = l.StartY;

                StrtPts.Rows.Add(row);
            }
        }

        private int MaximumLineCount()
        {
            int maxLineCnt;
            StringBuilder lineCntx = new StringBuilder();
            lineCntx.Append(string.Format("select max(jlline#) from {0}.{1}line ",
                       SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                        ));
            lineCntx.Append(string.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ",
                SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr));

            maxLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCntx.ToString()));
            return maxLineCnt;
        }

        private void MeasureAngle()
        {
            throw new NotImplementedException();
        }

        private void miFileAddSection_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void miFileCloseNoSave_Click(object sender, EventArgs e)
        {
            DiscardChangesAndExit();
            Close();
        }

        private void miFileEditSection_Click(object sender, EventArgs e)
        {
            EditParcelSections();
        }

        private void miSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveChanges(WorkingParcel);
            WorkingSection = null;
        }

        private void miSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveChanges(WorkingParcel);
            WorkingSection = null;
        }
        
        private void MoveCursorToNewPoint(PointF newPoint)
        {
            Color penColor;
            Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(newPoint.X);
            int jumpYScaled = Convert.ToInt32(newPoint.Y);

            Cursor.Position = new Point(jumpXScaled, jumpYScaled);
            penColor = PenColorForDrawing(EditState);
            _isJumpMode = (EditState == DrawingState.JumpPointSelected || EditState == DrawingState.SectionAdded);
            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(penColor, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 1, 1);
            g.Save();

            sketchBox.Image = MainImage;
            sketchBox.Refresh();
        }

        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multipleSectionsAttachment = string.Empty;

            if (sectionLetterList.Count > 1)
            {
                MultiplePoints = new DataTable();

                MultiSectionSelection attachmentSectionLetterSelected = new MultiSectionSelection(sectionLetterList);
                attachmentSectionLetterSelected.ShowDialog(this);

                multipleSectionsAttachment = MultiSectionSelection.adjsec;

                MultiplePoints = MultiSectionSelection.MultiplePointsDataTable;

                _hasMultiSection = true;
            }

            return multipleSectionsAttachment;
        }

        private void NotifyUserOfLegalMoves()
        {
            try
            {
                string legalDirs = string.Empty;
                string message = string.Format("You may only move {0} from this jump point.", legalDirs);
                MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                UndoJump = true;
                RevertToPriorVersion();
                DistText.Text = string.Empty;
                distance = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private decimal OriginalDistanceX()
        {
            decimal origDistX = 0;

            StringBuilder orgLen = new StringBuilder();
            orgLen.Append(string.Format("select jllinelen from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card
                        ));
            orgLen.Append(string.Format("and jlsect = '{0}' and jlline# = {1} ",
                CurrentSecLtr, AttSpLineNo));

            origDistX = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(orgLen.ToString()));
            return origDistX;
        }

        private AngleVector ParseAngleEntry(string textEntered)
        {
            AngleVector vector = new AngleVector();
            string anglecalls = DistText.Text.Trim();
            decimal xDist = 0.00M;
            decimal yDist = 0.00M;
            int commaCnt = anglecalls.IndexOf(",");

            string xDistText = anglecalls.Substring(0, commaCnt).Trim();
            string yDistText = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();
            decimal.TryParse(xDistText, out xDist);
            decimal.TryParse(yDistText, out yDist);

            AngleForm angleDialog = new AngleForm();
            angleDialog.ShowDialog();
            CamraDataEnums.CardinalDirection angleDirection = angleDialog.AngleDirection;

            vector.XLength = xDist;
            vector.YLength = yDist;
            vector.AngledLineDirection = angleDirection;
            return vector;
        }

        private AngleVector ParseNEWSLine(string textEntered, CamraDataEnums.CardinalDirection direction)
        {
            decimal distanceValueEntered = 0.00M;
            AngleVector angle = new AngleVector();
            decimal.TryParse(textEntered, out distanceValueEntered);
            switch (direction)
            {
                case CamraDataEnums.CardinalDirection.N:
                case CamraDataEnums.CardinalDirection.S:
                    angle.XLength = 0.00M;
                    angle.YLength = distanceValueEntered;
                    break;

                case CamraDataEnums.CardinalDirection.E:
                case CamraDataEnums.CardinalDirection.W:
                    angle.XLength = distanceValueEntered;
                    angle.YLength = 0.00M;
                    break;

                case CamraDataEnums.CardinalDirection.None:
                default:
                    angle.XLength = 0.00M;
                    angle.YLength = 0.00M;

                    break;
            }
            angle.AngledLineDirection = direction;
            return angle;
        }

        private void RedrawSketch(SMParcel parcel,string sectionLetter="")
        {
            SMSketcher sketcher = new SMSketcher(parcel, sketchBox);
            sketcher.RenderSketch(sectionLetter);
            sketchBox.Image = sketcher.SketchImage;
            _currentScale = (float)parcel.Scale;
        }

        private void RefreshWorkspace()
        {
            RedrawSketch(WorkingParcel);
            SetButtonStates(EditState);
            DisplayStatus("Ready");
        }

        private void RevertToPriorVersion()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void RevertToSavedSketch()
        {
            try
            {
                WorkingParcel = SketchUpGlobals.SMParcelFromData;
                SketchUpGlobals.SketchSnapshots.Clear();
                WorkingParcel.SnapShotIndex = 0;
                AddParcelToSnapshots(WorkingParcel);
                WorkingParcel.SnapShotIndex++;
                AddParcelToSnapshots(WorkingParcel);
                EditState = DrawingState.SketchLoaded;
                UnsavedChangesExist = false;
                RefreshWorkspace();
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }

        private void rotateSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FlipSketch fskt = new FlipSketch();
            fskt.ShowDialog();
            if (FlipSketch.FrontBack == true)
            {
                FlipUpDown();
            }
            if (FlipSketch.RightLeft == true)
            {
                FlipHorizontal();
            }
        }

        private bool SaveChanges(SMParcel parcel)
        {
            try
            {
                if (UnsavedChangesExist)
                {
                    if (GarageDataComplete())
                    {
                  
                    parcel.ReorganizeSections();
                    SketchRepository sr = new SketchRepository(parcel);
                    ParcelMast = sr.SaveCurrentParcel(parcel);
                    WorkingParcel = ParcelMast.Parcel;
                    RedrawSketch(WorkingParcel);
                    UnsavedChangesExist = false;
                }
                }
                return true;
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                return false;

                throw;
            }
        }

        private void SaveJumpPointsAndOldSectionEndPoints(float CurrentScale, int rowindex, DataView SortedJumpTableDataView)
        {
            try
            {
                float _JumpX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt2"].ToString()) * CurrentScale));
                float _JumpY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPT2"].ToString()) * CurrentScale));

                JumpX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));
                JumpY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));

                float _endOldSecX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt1"].ToString()) * CurrentScale));
                float _endOldSecY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPt1"].ToString()) * CurrentScale));
                endOldSecX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));
                endOldSecY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private void SetAddNewPointButton(bool enabled)
        {
            isInAddNewPointMode = enabled;
        }

        private void SetButtonStates(DrawingState sketchState)
        {
            bool enableAdd = false;
            bool enableUndo = false;
            bool enableContextMenu = false;
            bool enableJumpMenu = false;
            switch (sketchState)
            {
                case DrawingState.Drawing:
                    enableAdd = true;
                    enableContextMenu = true;
                    enableJumpMenu = false;
                    enableUndo = (WorkingSection != null && WorkingSection.Lines != null && WorkingSection.Lines.Count > 0);

                    if (WorkingSection.SectionIsClosed)
                    {
                        btnAdd.Text = "Done Drawing";
                        btnAdd.Image = DoneSketchingImage;
                    }
                    else
                    {
                        btnAdd.Text = "Auto-Close";
                        btnAdd.Image = CloseSectionImage;
                        enableAdd = WorkingSection.Lines.Count >= 2;
                    }

                    break;

                case DrawingState.SectionAdded:
                    btnAdd.Text = "Begin";
                    btnAdd.Image = JumpToCornerImage;
                    enableAdd = false;
                    enableContextMenu = true;
                    enableJumpMenu = true;
                    break;

                case DrawingState.JumpPointSelected:
                    enableJumpMenu = false;
                    enableContextMenu = true;
                    btnAdd.Text = "Begin";
                    btnAdd.Image = BeginDrawingImage;
                    enableAdd = true;
                    break;

                case DrawingState.DoneDrawing:
                    enableAdd = true;
                    enableContextMenu = true;
                    btnAdd.Text = "Save";
                    btnAdd.Image = SaveAndCloseImage;
                    enableJumpMenu = false;

                    break;

                case DrawingState.SketchLoaded:
                case DrawingState.SketchSaved:
                default:

                    enableAdd = true;
                    enableContextMenu = !UnsavedChangesExist;
                    btnAdd.Text = "Add Section";
                    btnAdd.Image = AddImage;
                    enableJumpMenu = false;

                    break;
            }
            btnAdd.Enabled = enableAdd;
            AddSectionContextMenu.Enabled = enableContextMenu;
            cmiJumpToCorner.Enabled = enableJumpMenu;
            btnUnDo.Enabled = enableUndo;
        }

        private void SetReadyButtonAppearance()
        {
            btnAdd.BackColor = Color.PaleTurquoise;
            btnAdd.Text = "Begin";
            btnAdd.Enabled = ScaledJumpPoint != null;
        }

        private void ShowMessageBox(string s)
        {
            MessageBox.Show(s);
        }

        private void ShowMultipleAttachmentSectionsForm()
        {
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
            ShowVersion(WorkingParcel.SnapShotIndex);
            var AttachPoints = (from l in
                  WorkingParcel.AllSectionLines.Where(s =>
                  s.SectionLetter != "A" && s.LineNumber == 1).ToList()
                                select l);
            if (AttachPoints.ToList().Count > 0)
            {
                WorkingParcel.Sections.Where(s => s.SectionLetter ==
                    NextSectLtr.Trim()).FirstOrDefault().Lines.Where(a =>
                    a.AttachedSection ==
                    NextSectLtr).FirstOrDefault().AttachedSection = " ";
            }

            if (MultiSectionSelection.adjsec == string.Empty)
            {
                ConnectSec = "A";
            }

            if (MultiSectionSelection.adjsec != string.Empty)
            {
                ConnectSec = MultiSectionSelection.adjsec;
            }

            foreach (SMLine l in
                WorkingParcel.AllSectionLines.Where(s =>
                s.SectionLetter != "A").ToList())
            {
                int record = l.Record;
                int card = l.Card;
                string curSect = l.SectionLetter;
                decimal X1 = l.StartX;
                decimal Y1 = l.StartY;
                decimal X2 = l.EndX;
                decimal Y2 = l.EndY;

                if (curSect == NextSectLtr)
                {
                    ConnectSec = CurrentSecLtr;
                }
                l.AttachedSection = curSect;
                if (l.EndX == X1 && l.EndY == Y1 && l.SectionLetter !=
                    curSect && l.SectionLetter == ConnectSec &&
                    l.AttachedSection.Trim() == string.Empty)
                {
                    l.AttachedSection = curSect;
                }
            }
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
        }

        private void ShowState(DrawingState value)
        {
            string message = string.Empty;
            switch (value)
            {
                case DrawingState.Drawing:
                    message = string.Format("Drawing Section {0}", WorkingSection.SectionLabel);
                    break;

                case DrawingState.SectionEditCompleted:
                    message = string.Format("Click \"Save\" to commit the changes to section {0}.", WorkingSection.SectionLetter);
                    break;

                case DrawingState.DoneDrawing:
                    message = "Section Added to Sketch. Click \"Save\" to commit this change.";
                    break;

                case DrawingState.JumpPointSelected:
                    StringBuilder directions = new StringBuilder();
                    if (LegalMoveDirections.Count > 0)
                    {
                    }
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

                    message = string.Format("Move ({0}) to set start, or click \"Begin\" to start drawing the section here.", directions.ToString());
                    break;

                case DrawingState.SectionAdded:
                    message = string.Format("Added Section {0}. Right-click to select the nearest corner as a reference point.", WorkingSection.SectionLabel);
                    break;

                case DrawingState.SketchLoaded:
                    message = string.Format("Ready to edit the sketch of record {0}", SketchUpGlobals.ParcelWorkingCopy.Record);
                    break;

                default:
                    break;
            }
            DisplayStatus(message);
        }

        private void ShowVersion(int snapShotIndex)
        {
            string versionInfo = string.Format("#{0}", snapShotIndex);
            snapshotIndexLabel.Text = versionInfo;
        }

        private void ShowWorkingCopySketch(string sketchFolder, string sketchRecord, string sketchCard, bool hasSketch, bool hasNewSketch)
        {
            try
            {
                InitializeDataTablesAndVariables(sketchFolder, sketchRecord, sketchCard, hasSketch, hasNewSketch);

                InitializeDisplayDataGrid();
                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
                SketchUpGlobals.HasSketch = (SketchUpGlobals.ParcelWorkingCopy != null && WorkingParcel.AllSectionLines.Count > 0);
                IsNewSketch = !SketchUpGlobals.HasSketch;

                //HACK - Easier to repeat than track down the usages at this juncture
                SketchUpGlobals.HasNewSketch = IsNewSketch;
                if (SketchUpGlobals.HasSketch == true)
                {
                    SMSketcher sketcher = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
                    sketcher.RenderSketch();

                    MainImage = sketcher.SketchImage;
                    _currentScale = (float)WorkingParcel.Scale;
                }
                else
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                }
                ScaleBaseX = sketchBox.Width / (float)WorkingParcel.SketchXSize;

                ScaleBaseY = sketchBox.Height / (float)WorkingParcel.SketchYSize;

                if (MainImage == null)
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                    _vacantParcelSketch = true;
                    IsNewSketch = true;
                }
                if (_vacantParcelSketch == true)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    g.Clear(Color.White);
                    _currentScale = Convert.ToSingle(7.2);
                }

                sketchBox.Image = MainImage;
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Console.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private void sortSection()
        {
            //TODO: Refactor into SketchManager
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
            FixSect = new List<string>();

            StringBuilder addFix = new StringBuilder();
            addFix.Append(string.Format("select jlsect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card));

            DataSet fs = dbConn.DBConnection.RunSelectStatement(addFix.ToString());

            if (fs.Tables[0].Rows.Count > 0)
            {
                FixSect.Clear();

                for (int i = 0; i < fs.Tables[0].Rows.Count; i++)
                {
                    FixSect.Add(fs.Tables[0].Rows[i]["jlsect"].ToString());
                }

                if (FixSect.Count > 0)
                {
                    sortDist.Clear();

                    StringBuilder chkLen = new StringBuilder();
                    chkLen.Append("select jlsect,jlline#,jldirect, case when jldirect in ( 'N','S') then abs(jlpt1y-jlpt2y) when jldirect in ( 'E','W') then abs(jlpt1x-jlpt2x) ");
                    chkLen.Append("when jldirect in ( 'NE','SE','NW','SW') then sqrt(abs(jlpt1y-jlpt2y)*abs(jlpt1y-jlpt2y)+abs(jlpt1x-jlpt2x)*abs(jlpt1x-jlpt2x)) ");
                    chkLen.Append(string.Format("end as LineLen, abs(jlpt1x-jlpt2x) as Xlen, abs(jlpt1y-jlpt2y) as Ylen from {0}.{1}line ",
                                  SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                                    ));
                    chkLen.Append(string.Format("where jlrecord = {0} and jldwell = {1} order by jlsect,jlline# ", SketchUpGlobals.Record, SketchUpGlobals.Card));

                    DataSet fixl = dbConn.DBConnection.RunSelectStatement(chkLen.ToString());

                    if (fixl.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < fixl.Tables[0].Rows.Count; i++)
                        {
                            StringBuilder updLine = new StringBuilder();
                            updLine.Append(string.Format("update {0}.{1}line set jlxlen = {2},jlylen = {3},jllinelen = {4} ",
                                           SketchUpGlobals.LocalLib,
                                           SketchUpGlobals.LocalityPrefix,

                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Xlen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Ylen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["LineLen"].ToString())));
                            updLine.Append(string.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                    SketchUpGlobals.Record,
                                    SketchUpGlobals.Card,
                                    fixl.Tables[0].Rows[i]["jlsect"].ToString(),
                                    Convert.ToInt32(fixl.Tables[0].Rows[i]["jlline#"].ToString())));

                            dbConn.DBConnection.ExecuteNonSelectStatement(updLine.ToString());
                        }
                    }
                }
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

                        break;

                    case DialogResult.No:
                        DiscardChangesAndExit();

                        break;

                    default:

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

                CPSize += s.SqFt;
            }
        }

        private void UnDoBtn_Click(object sender, EventArgs e)
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

                    break;

                case DrawingState.SectionAdded:
                    FormattableString message = $"Do you wish to undo the addition of {WorkingSection.SectionLabel}?";
                    string title = "Undo Section Addition?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    MessageBoxIcon icon = MessageBoxIcon.Question;
                    MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
                    DialogResult response = MessageBox.Show(message.ToString(), title, buttons, icon, defButton);
                    switch (response)
                    {
                        case DialogResult.Yes:
                            RevertToSavedSketch();
                            break;

                        default:
                            break;
                    }
                    break;

                case DrawingState.SketchLoaded:

                default:
                    break;
            }
        }

        private void UndoLine()
        {
            SMSketcher sms = new SMSketcher(WorkingParcel, sketchBox);
            sms.RenderSketch(WorkingSection.SectionLetter);
            sketchBox.Image = sms.SketchImage;

            if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
            {
                SMLine lastLine = (from l in WorkingSection.Lines.OrderBy(n => n.LineNumber) select l).LastOrDefault();

                if (lastLine.LineNumber == 1)
                {
                    string message = "This will remove the last line. Do you wish to delete the section as well?\n\nChoose \"Yes\" to revert to the saved version, or \"No\" to start over drawing the section. \n\nChoose \"Cancel\" to leave this line in place.";
                    string title = "Delete Section?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    MessageBoxIcon icon = MessageBoxIcon.Question;
                    MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
                    DialogResult response = MessageBox.Show(message, title, buttons, icon, defButton);
                    switch (response)
                    {
                        case DialogResult.Cancel:

                            break;

                        case DialogResult.Yes:
                            RevertToSavedSketch();
                            EditState = DrawingState.SketchLoaded;
                            break;

                        case DialogResult.No:
                            DbStartPoint = lastLine.StartPoint;
                            ScaledStartPoint = lastLine.ScaledStartPoint;
                            MoveCursorToNewPoint(ScaledStartPoint);
                            ScaledJumpPoint = ScaledStartPoint;
                            WorkingSection.Lines.Remove(lastLine);
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    DbStartPoint = lastLine.StartPoint;
                    ScaledStartPoint = lastLine.ScaledStartPoint;
                    ScaledJumpPoint = ScaledStartPoint;
                    MoveCursorToNewPoint(ScaledStartPoint);
                    WorkingSection.Lines.Remove(lastLine);
                }
                sms = new SMSketcher(WorkingParcel, sketchBox);
                sms.RenderSketch(WorkingParcel.LastSectionLetter);
                sketchBox.Image = sms.SketchImage;
            }
        }

        private void UpdateCarportCountToZero()
        {
            string codeFromLookup = (from c in SketchUpLookups.CarPortTypeCollection where c.Description.Trim().ToUpper() == "NONE" select c.Code).FirstOrDefault();
            int cpCode = 0;
            int.TryParse(codeFromLookup, out cpCode);
            ParcelMast.CarportTypeCode = cpCode == 0 ? 67 : cpCode;
            ParcelMast.CarportNumCars = 0;

            ////TODO: make this flexible to update the garage count and square footage per the ParcelMast
            ////TODO: Refactor into SketchManager
            //StringBuilder zerocp = new StringBuilder();
            //zerocp.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
            //                        SketchUpGlobals.LocalLib,
            //                        SketchUpGlobals.LocalityPrefix,
            //                        SketchUpGlobals.Record,
            //                        SketchUpGlobals.Card));

            //dbConn.DBConnection.ExecuteNonSelectStatement(zerocp.ToString());
        }

        private bool GarageDataComplete()
        {
            bool updatesNeeded = (SketchUpLookups.CarPortTypes.Contains(WorkingSection.SectionType) || SketchUpLookups.GarageTypes.Contains(WorkingSection.SectionType));

            if (updatesNeeded)
            {
                MissingGarageData mgd = new MissingGarageData(WorkingParcel.ParcelMast, WorkingSection.SqFt, WorkingSection.SectionType);
                if (mgd.GarageDataOk&&mgd.CarportDataOk)
                {
                    updatesNeeded = false;
                }
            }
            return !updatesNeeded;
            //SMVehicleStructure svs = new SMVehicleStructure(WorkingParcel);
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

        //private DataSet UpdateMasterArea(decimal summedArea)
        //{
        //    string checkMaster = string.Format("select * from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
        //        SketchUpGlobals.LocalLib,
        //        SketchUpGlobals.LocalityPrefix,
        //        SketchUpGlobals.Record,
        //        SketchUpGlobals.Card);

        //    DataSet ds_master = dbConn.DBConnection.RunSelectStatement(checkMaster.ToString());

        //    if (ds_master.Tables[0].Rows.Count > 0)
        //    {
        //        string updateMasterSql = string.Format("update {0}.{1}master set jmtotsqft = {2} where jmrecord = {3} and jmdwell = {4} ",
        //                       SketchUpGlobals.LocalLib,
        //                       SketchUpGlobals.LocalityPrefix,
        //                       summedArea,
        //                       SketchUpGlobals.Record,
        //                       SketchUpGlobals.Card);

        //        dbConn.DBConnection.ExecuteNonSelectStatement(updateMasterSql.ToString());
        //    }

        //    return ds_master;
        //}

        #endregion "Private methods"

        private void btnEditSections_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DisplayStatus("Loading sections information...please wait.");
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
            EditSketchSections ess = new EditSketchSections(WorkingParcel);
            ess.ShowDialog();
            RefreshWorkspace();
        }
    }
}