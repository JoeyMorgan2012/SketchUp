using SWallTech;
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

namespace SketchUp
{
    //Refactored several stringbuilders to strings and extracted many long code runs into separate methods. JMM Feb 2016
    // Refactored adding a section to use an in-memory parcel representation and only write to the DB when changes are saved.
    public partial class ExpandoSketch : Form
    {
        #region Constructor

        public ExpandoSketch(ParcelData currentParcel, string sketchFolder,
      string sketchRecord, string sketchCard, string _locality, SWallTech.CAMRA_Connection _fox,
      SectionDataCollection currentSection, bool hasSketch, Image sketchImage, bool hasNewSketch)
        {
            InitializeComponent();
            ShowWorkingCopySketch(currentParcel, sketchFolder, sketchRecord, sketchCard, _locality, _fox, currentSection, hasSketch, hasNewSketch);

            //click++;
            ////savpic.Add(click, imageToByteArray(_mainimage));
        }

        private void DrawParcelLabel()
        {
            Graphics g = Graphics.FromImage(MainImage);
            SolidBrush Lblbrush = new SolidBrush(Color.Black);
            SolidBrush FillBrush = new SolidBrush(Color.White);
            Pen whitePen = new Pen(Color.White, 2);
            Pen blackPen = new Pen(Color.Black, 2);

            Font LbLf = new Font("Arial", 10, FontStyle.Bold);
            Font TitleF = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline);
            Font MainTitle = new System.Drawing.Font("Arial", 15, FontStyle.Bold | FontStyle.Underline);
            char[] leadzero = new char[] { '0' };

            g.DrawString(SketchUpGlobals.LocalityPreFix, TitleF, Lblbrush, new PointF(10, 10));
       //HACK     g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            g.DrawString(String.Format("Record # - {0}", SketchUpGlobals.Record.ToString().TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            g.DrawString(String.Format("Card # - {0}", SketchUpGlobals.Card), LbLf, Lblbrush, new PointF(10, 45));

            g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));
        }

        private void InitializeDataTablesAndVariables(ParcelData currentParcel, string sketchFolder, string sketchRecord, string sketchCard, string _locality, CAMRA_Connection _fox, SectionDataCollection currentSection, bool hasSketch, bool hasNewSketch)
        {
            checkDirection = false;
            _currentParcel = currentParcel;
            _currentSection = currentSection;

            dbConn = _fox;

            _currentSection = new SectionDataCollection(_fox, _currentParcel.Record, _currentParcel.Card);

            Locality = _locality;

            IsNewSketch = false;
            _hasNewSketch = hasNewSketch;
            IsNewSketch = hasNewSketch;
            _addSection = false;
            click = 0;
            SketchFolder = sketchFolder;
            SketchRecord = sketchRecord;
            SketchCard = sketchCard;
            SketchUpGlobals.HasSketch = hasSketch;

            //savpic = new Dictionary<int, byte[]>();
            _StartX = new Dictionary<int, float>();
            _StartY = new Dictionary<int, float>();

            SectionTable = ConstructSectionTable();

            ConstructJumpTable();

            REJumpTable = ConstructREJumpTable();

            RESpJumpTable = ConstructRESpJumpTable();
            SectionLtrs = ConstructSectionLtrs();

            AreaTable = ConstructAreaTable();

            MulPts = ConstructMulPtsTable();

            undoPoints = ConstructUndoPointsTable();
            sortDist = ConstructSortDistanceTable();

            AttachmentPointsDataTable = ConstructAttachmentPointsDataTable();

            //AttachPoints = ConstructAttachPointsDataTable();

            DupAttPoints = ConstructDupAttPointsTable();

            StrtPts = ConstructStartPointsTable();
        }

        private void InitializeDisplayDataGrid()
        {
            displayDataTable = ConstructDisplayDataTable();

            dgSections.DataSource = displayDataTable;
        }

        private void ShowWorkingCopySketch(ParcelData currentParcel, string sketchFolder, string sketchRecord, string sketchCard, string _locality, CAMRA_Connection _fox, SectionDataCollection currentSection, bool hasSketch, bool hasNewSketch)
        {
            try
            {
                InitializeDataTablesAndVariables(currentParcel, sketchFolder, sketchRecord, sketchCard, _locality, _fox, currentSection, hasSketch, hasNewSketch);

                InitializeDisplayDataGrid();

                SketchUpGlobals.HasSketch = (SketchUpGlobals.ParcelWorkingCopy != null && SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Count > 0);
                IsNewSketch = !SketchUpGlobals.HasSketch;
                //HACK - Easier to repeat than track down the usages at this juncture
                SketchUpGlobals.HasNewSketch = IsNewSketch;
                if (SketchUpGlobals.HasSketch == true)
                {
                    MainImage = RenderSketch();
                    _currentScale = (float)SketchUpGlobals.ParcelWorkingCopy.Scale;

                    //MainImage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
                    //_currentScale = _scale;
                }
                else
                {
                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                }

                ScaleBaseX = BuildingSketcher.basePtX;
                ScaleBaseY = BuildingSketcher.basePtY;

                if (MainImage == null)
                {
                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                    _vacantParcelSketch = true;
                    IsNewSketch = true;
                }
                if (_vacantParcelSketch == true)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    g.Clear(Color.White);

                    //g.DrawRectangle(whitePen, 0, 0, 1000, 572);
                    //g.FillRectangle(FillBrush, 0, 0, 1000, 572);
                    _currentScale = Convert.ToSingle(7.2);
                }

                DrawParcelLabel();

                ExpSketchPBox.Image = MainImage;
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

        #endregion Constructor

        #region DataTable Construction Refactored

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
                Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
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

        #endregion DataTable Construction Refactored

        #region General Class Methods

        #region Original Code for RedrawSection

        private void RedrawSection()
        {
            NeedToRedraw = true;

            for (int i = 0; i < undoPoints.Rows.Count; i++)
            {
                float redist = 0;
                string undodirect = String.Empty;
                undodirect = undoPoints.Rows[i]["Direct"].ToString();

                float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

                float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

                float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

                float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

                if (undodirect == "N" || undodirect == "S" || undodirect == "E" || undodirect == "W")
                {
                    if (x1 == x2)
                    {
                        redist = Math.Abs((y1 - y2) / _currentScale);
                    }
                    if (y1 == y2)
                    {
                        redist = Math.Abs((x1 - x2) / _currentScale);
                    }
                }
                if (undodirect == "NE" || undodirect == "SE" || undodirect == "NW" || undodirect == "SW")
                {
                    float x1f = Math.Abs((x1 - x2) / _currentScale);
                    float y1f = Math.Abs((y1 - y2) / _currentScale);

                    distanceD = Convert.ToInt32(Math.Sqrt(x1f + y1f));

                    decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(x1f + y1f)), 2);

                    distance = Convert.ToDecimal(distanceD1);

                    AngD1 = Convert.ToDecimal(x1f);
                    AngD2 = Convert.ToDecimal(y1f);
                }

                Graphics g = Graphics.FromImage(MainImage);
                g.Save();

                StartX = x1;
                StartY = y1;

                ExpSketchPBox.Image = MainImage;

                //click++;
                ////savpic.Add(click, imageToByteArray(_mainimage));

                if (undoPoints.Rows[i]["Direct"].ToString() == "E")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (redist * _currentScale)), StartY);

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    ////savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "N")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, StartX, (StartY - (redist * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "S")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, StartX, (StartY + (redist * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "W")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (redist * _currentScale)), StartY);

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "NW")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "NE")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "SW")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "SE")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
            }

            ExpSketchPBox.Image = MainImage;

            //click++;
            //savpic.Add(click, imageToByteArray(_mainimage));
        }

        #endregion Original Code for RedrawSection

        private void AdjustLine(decimal newEndX, decimal newEndY, decimal newDistX, decimal newDistY, decimal EndEndX, decimal EndEndY, decimal finDist)
        {
            StringBuilder adjLine = new StringBuilder();
            adjLine.Append(String.Format("update {0}.{1}line set jldirect = '{2}',jlxlen = {3},jlylen = {4},jllinelen = {5}, ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            CurrentAttDir,
                            newDistX,
                            newDistY,
                            finDist));
            adjLine.Append(String.Format("jlpt1x = {0},jlpt1y = {1},jlpt2x = {2},jlpt2y = {3} ",
                    newEndX, newEndY, EndEndX, EndEndY));
            adjLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                _currentParcel.mrecno, _currentParcel.mdwell, _savedAttSection, (mylineNo + 1)));

            dbConn.DBConnection.ExecuteNonSelectStatement(adjLine.ToString());
        }

        private string AttachLineDirection(string attachSection, int attachLineNumber)
        {
            //Find the line that begins where the line in the saved section ends.
            string lineDirection = string.Empty;
            decimal lastLineEndX = 0M;
            decimal lastLineEndY = 0M;
            decimal nextLineStartX = 0M;
            decimal nextLineStartY = 0M;
            string checkRowSection = string.Empty;
            int checkRowLine = 0;
            for (int i = 0; i < JumpTable.Rows.Count; i++)
            {
                DataRow checkRow = JumpTable.Rows[i];
                checkRowSection = checkRow["Sect"].ToString().Trim();
                Int32.TryParse(checkRow["LineNo"].ToString(), out checkRowLine);
                if (checkRowSection == attachSection && checkRowLine == attachLineNumber) // this is the row whose END points are the start point of the line with the legal direction
                {
                    decimal.TryParse(checkRow["XPt2"].ToString(), out lastLineEndX);
                    decimal.TryParse(checkRow["YPt2"].ToString(), out lastLineEndY);
                }
            }

            // Now get the line that starts with those end point, in the same section.
            for (int i = 0; i < JumpTable.Rows.Count; i++)
            {
                DataRow checkRow = JumpTable.Rows[i];
                checkRowSection = checkRow["Sect"].ToString().Trim();
                decimal.TryParse(checkRow["XPt1"].ToString(), out nextLineStartX);
                decimal.TryParse(checkRow["YPt1"].ToString(), out nextLineStartY);

                if (checkRowSection == attachSection && nextLineStartX == lastLineEndX && nextLineStartY == lastLineEndY) // this is the row whose direction we need
                {
                    lineDirection = checkRow["Direct"].ToString().Trim();
                }
            }
            return lineDirection;
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void CalculateClosure(float _distX, float _distY)
        {
            float ewDist = (SecBeginX - _distX);
            float nsDist = (SecBeginY - _distY);

            string closeX = String.Empty;
            string closeY = String.Empty;

            _openForm = true;

            if (ewDist > 0)
            {
                closeY = "E";
            }
            if (ewDist < 0)
            {
                closeY = "W";
            }
            if (nsDist < 0)
            {
                closeX = "N";
            }
            if (nsDist > 0)
            {
                closeX = "S";
            }

            if (Math.Round(Convert.ToDecimal(ewDist), 1) == 0 && Math.Round(Convert.ToDecimal(nsDist), 1) == 0)
            {
                _openForm = false;

                decimal EWdist = Math.Round(Convert.ToDecimal(ewDist), 1);
                decimal NSdist = Math.Round(Convert.ToDecimal(nsDist), 1);

                ShowDistanceForm(closeY, EWdist, closeX, NSdist, _openForm);
            }

            if (Math.Round(Convert.ToDecimal(ewDist), 1) != 0 || Math.Round(Convert.ToDecimal(nsDist), 1) != 0)
            {
                _openForm = true;

                decimal EWdist = Math.Round(Convert.ToDecimal(ewDist), 1);
                decimal NSdist = Math.Round(Convert.ToDecimal(nsDist), 1);

                ShowDistanceForm(closeY, EWdist, closeX, NSdist, _openForm);
            }
        }

        private void calculateNewArea(int record, int card, string nextsec)
        {
            StringBuilder getLine = new StringBuilder();
            getLine.Append("select jlpt1x,jlpt1y,jlpt2x,jlpt2Y ");
            getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        _currentParcel.mrecno,
                        _currentParcel.mdwell));
            getLine.Append(String.Format("and jlsect = '{0}' ", nextsec));

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

        private void ClrX()
        {
            if (draw != false)
            {
                StringBuilder delXdir = new StringBuilder();
                delXdir.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X'",
                               SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                _currentParcel.Record,
                                _currentParcel.Card));

                dbConn.DBConnection.ExecuteNonSelectStatement(delXdir.ToString());
            }
        }

        private void computeArea()
        {
            var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
            var sectionArea = sectionPolygon.Area;

            calculateNewArea(_currentParcel.Record, _currentParcel.Card, NextSectLtr);

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

        private void CountLines(string thisSection)
        {
            string curlincnt = string.Format("select count(*) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, _currentParcel.Record, _currentParcel.Card, thisSection);

            SecLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(curlincnt.ToString()));
        }

        private int CountSections()
        {
            try
            {
                string seccnt = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, _currentParcel.Record, _currentParcel.Card);

                int secItemCnt = 0;
                Int32.TryParse(dbConn.DBConnection.ExecuteScalar(seccnt).ToString(), out secItemCnt);
                return secItemCnt;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));

                throw;
            }
        }

        private void DeleteLineSection()
        {
            StringBuilder deletelinesect = new StringBuilder();
            deletelinesect.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                           SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.mrecno,
                            _currentParcel.mdwell,
                            CurrentSecLtr));

            dbConn.DBConnection.ExecuteNonSelectStatement(deletelinesect.ToString());
        }

        private void DeleteSection()
        {
            SketchSection sksect = new SketchSection(_currentParcel, dbConn, _currentSection);
            sksect.ShowDialog(this);

            _currentSection = new SectionDataCollection(dbConn, _currentParcel.Record, _currentParcel.Card);
        }

        private string FindClosestCorner(float CurrentScale, ref string curltr, List<string> AttSecLtrList)
        {
            string secltr;
            decimal dist1 = 0;
            decimal dist1x = 0;
            decimal dist2 = 0;
            decimal distX = 0;
            int rowindex = 0;

            //was called dv--renamed for clarity
            DataView SortedJumpTableDataView = new DataView(JumpTable);
            SortedJumpTableDataView.Sort = "Dist ASC";

            BeginSplitX = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString()));
            BeginSplitY = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString()));

            NextStartX = BeginSplitX;
            NextStartY = BeginSplitY;

            for (int i = 0; i < SortedJumpTableDataView.Count; i++)
            {
                dist1 = Convert.ToDecimal(JumpTable.Rows[i]["Dist"].ToString());
                dist1x = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());

                if (i == 0)
                {
                    dist2 = dist1;
                    rowindex = i;
                }

                if (dist1 <= dist2 && i > 0)
                {
                    dist2 = dist1;
                    rowindex = i;
                }
            }

            distX = Convert.ToDecimal(SortedJumpTableDataView[0]["Dist"].ToString());

            secltr = SortedJumpTableDataView[0]["Sect"].ToString();
            AttSecLtrList.Add(secltr);
            int cntsec = 0;

            for (int i = 1; i < SortedJumpTableDataView.Count; i++)
            {
                decimal distx2 = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());
                curltr = SortedJumpTableDataView[i]["Sect"].ToString();

                if (distx2 == distX)
                {
                    cntsec++;
                    AttSecLtrList.Add(curltr);
                }
            }

            /* Joey's attempt to simplify the determination of the closest points and populate the multi-attach section if there are more than one.

            List<PointWithComparisons> possibleAttachmentPoints = ClosestPoints(JumpTable, new PointF(JumpX, JumpY));
            if (possibleAttachmentPoints.Count > 1)
            {
                AttSecLtrList.Clear();
                foreach (PointWithComparisons p in possibleAttachmentPoints)
                {
                    AttSecLtrList.Add(p.PointLabel);
                }
            }
            else
            {
                secltr = possibleAttachmentPoints.FirstOrDefault<PointWithComparisons>().PointLabel;
            }

    End Joey's alternative Code */

            string multisectatch = MultiPointsAvailable(AttSecLtrList);

            SaveJumpPointsAndOldSectionEndPoints(CurrentScale, rowindex, SortedJumpTableDataView);

            string _CurrentSecLtr = JumpTable.Rows[rowindex]["Sect"].ToString();

            //Ask Dave why this is set here if it is set differently below
            //  Rube Goldberg code. Value is set again below, so I commented this one out.
            //	CurrentSecLtr = SortedJumpTableDataView[0]["Sect"].ToString();

            int savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

            _savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());
            Trace.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex][LineNo]={0}", _savedAttLine));
            Trace.WriteLine(string.Format("************ ({0} is not subsequently used.******** ", _savedAttLine));
            Trace.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0][LineNo]={0}", _savedAttLine));

            //Ask Dave why this is set here if it is set differently above
            //Ask Dave why sometimes the rowindex of the JumpTable is used and othertimes the row[0] of the Sorted Jump Table
            CurrentSecLtr = _CurrentSecLtr;

            if (AttSecLtrList.Count > 1)
            {
                CurrentSecLtr = multisectatch;
            }

            string priorDirection = JumpTable.Rows[rowindex]["Direct"].ToString();

            string savedAttSection = JumpTable.Rows[rowindex]["Sect"].ToString();
            int _CurrentAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

            startSplitX = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt1"].ToString());
            startSplitY = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt1"].ToString());
            Trace.WriteLine(string.Format("Start split point: {0},{1}", startSplitX, startSplitY));
            /* More Rube Goldberg code. These values are set, but then they are not used anywhere.
             -JMM
                        decimal tsplit2 = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString());
                        decimal tsplit3 = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString());
            */
            _priorDirection = SortedJumpTableDataView[0]["Direct"].ToString();
            _savedAttSection = SortedJumpTableDataView[0]["Sect"].ToString();
            currentAttachmentLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());

            //TODO: Find the last moved direction and the direction of the Current AttLine. If they are not the same call undo and return to main screen.
            _mouseX = Convert.ToInt32(JumpX);
            _mouseY = Convert.ToInt32(JumpY);
            Trace.WriteLine(string.Format("Mouse moved to {0},{1}", JumpX, JumpY));
            Trace.WriteLine(string.Format("Section attachment is {0} Line {1}, _priorDirection is {2}", _savedAttSection, currentAttachmentLine, _priorDirection));
            legalMoveDirection = AttachLineDirection(_savedAttSection, currentAttachmentLine);
            MoveCursor();
            return secltr;
        }

        public void findends()
        {
            delStartX = 0;
            delStartY = 0;
            LastDir = String.Empty;

            StringBuilder cntLine = new StringBuilder();
            cntLine.Append(String.Format("select max(jlline#) from {0}.{1}line ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            cntLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ", _currentParcel.mrecno, _currentParcel.mdwell, NextSectLtr));

            try
            {
                int jlinecnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(cntLine.ToString()));

                StringBuilder curLine = new StringBuilder();
                curLine.Append(String.Format("select jldirect,jlpt1x,jlpt1y from {0}.{1}line ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
                curLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                _currentParcel.mrecno,
                                _currentParcel.mdwell,
                                NextSectLtr,
                                jlinecnt));
                DataSet dsl = dbConn.DBConnection.RunSelectStatement(curLine.ToString());

                if (dsl.Tables[0].Rows.Count > 0)
                {
                    LastDir = dsl.Tables[0].Rows[0]["jldirect"].ToString();

                    delStartX = (float)(Convert.ToDecimal(dsl.Tables[0].Rows[0]["jlpt1x"].ToString()));
                    delStartY = (float)(Convert.ToDecimal(dsl.Tables[0].Rows[0]["jlpt1y"].ToString()));
                }
            }
            catch
            {
            }
        }

        private void FixOrigLine()
        {
            StringBuilder fixOrigLine = new StringBuilder();
            fixOrigLine.Append(String.Format("update {0}.{1}line ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
                                    adjNewSecX,
                                    adjNewSecY,
                                    RemainderLineLength,
                                    NewSectionBeginPointX,
                                    NewSectionBeginPointY));
            fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                            _currentParcel.mrecno,
                            _currentParcel.mdwell,
                            CurrentSecLtr,
                            _savedAttLine));

            dbConn.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
        }

        private void FlipLeftRight()
        {
            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.Record,
                            _currentParcel.Card));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = _currentParcel.mrecno;
                    row["Card"] = _currentParcel.mdwell;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

                    string mytestdir = row["Direct"].ToString();

                    if (mytestdir == "E")
                    {
                        row["Direct"] = "W";
                    }
                    if (mytestdir == "NE")
                    {
                        row["Direct"] = "NW";
                    }
                    if (mytestdir == "SE")
                    {
                        row["Direct"] = "SW";
                    }
                    if (mytestdir == "W")
                    {
                        row["Direct"] = "E";
                    }
                    if (mytestdir == "NW")
                    {
                        row["Direct"] = "NE";
                    }
                    if (mytestdir == "SW")
                    {
                        row["Direct"] = "SE";
                    }

                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) != 0)
                    {
                        row["Xpt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) * -1);
                    }
                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) != 0)
                    {
                        row["Xpt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) * -1);
                    }

                    SectionTable.Rows.Add(row);
                }
            }

            for (int i = 0; i < SectionTable.Rows.Count; i++)
            {
                string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
                int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
                string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
                decimal fXpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt1"].ToString());
                decimal fXpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt2"].ToString());

                StringBuilder flipit = new StringBuilder();
                flipit.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1x = {3}, jlpt2x = {4} ",
                               SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPreFix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                fdirect,
                                fXpt1,
                                fXpt2));
                flipit.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                _currentParcel.mrecno, _currentParcel.mdwell, fsect, flineno));

                dbConn.DBConnection.ExecuteNonSelectStatement(flipit.ToString());
            }

            _closeSketch = true;

            RefreshSketch();
        }

        private void FlipUpDown()
        {
            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.Record,
                            _currentParcel.Card));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = _currentParcel.mrecno;
                    row["Card"] = _currentParcel.mdwell;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

                    string mytestdir = row["Direct"].ToString();

                    if (mytestdir == "N")
                    {
                        row["Direct"] = "S";
                    }
                    if (mytestdir == "NE")
                    {
                        row["Direct"] = "SE";
                    }
                    if (mytestdir == "NW")
                    {
                        row["Direct"] = "SW";
                    }

                    if (mytestdir == "S")
                    {
                        row["Direct"] = "N";
                    }
                    if (mytestdir == "SE")
                    {
                        row["Direct"] = "NE";
                    }
                    if (mytestdir == "SW")
                    {
                        row["Direct"] = "NW";
                    }

                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) != 0)
                    {
                        row["Ypt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) * -1);
                    }
                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) != 0)
                    {
                        row["Ypt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) * -1);
                    }

                    SectionTable.Rows.Add(row);
                }
            }

            for (int i = 0; i < SectionTable.Rows.Count; i++)
            {
                string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
                int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
                string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
                decimal fYpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt1"].ToString());
                decimal fYpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt2"].ToString());

                StringBuilder flipitFB = new StringBuilder();
                flipitFB.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1y = {3}, jlpt2y = {4} ",
                                   SketchUpGlobals.LocalLib,
                                   SketchUpGlobals.LocalityPreFix,

                                     //SketchUpGlobals.FcLib,
                                     //SketchUpGlobals.FcLocalityPrefix,
                                     fdirect,
                                     fYpt1,
                                     fYpt2));
                flipitFB.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                _currentParcel.mrecno, _currentParcel.mdwell, fsect, flineno));

                dbConn.DBConnection.ExecuteNonSelectStatement(flipitFB.ToString());
            }

            _closeSketch = true;

            RefreshSketch();
        }

        private DataSet GetLinesData(int crrec, int crcard)
        {
            DataSet lines;
            string getLine = string.Format("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, crrec, crcard);

            lines = dbConn.DBConnection.RunSelectStatement(getLine);
            return lines;
        }

        private DataSet GetSectionLines(int crrec, int crcard)
        {
            DataSet lines;
            StringBuilder getLine = new StringBuilder();
            getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                       SketchUpGlobals.LocalLib,
                       SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        crrec,
                        crcard));
            getLine.Append("and jldirect <> 'X' ");

            lines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());
            return lines;
        }

        public void getSplitLine()
        {
            int Sprowindex = 0;

            ConnectSec = String.Empty;

            if (MultiSectionSelection.adjsec != String.Empty)
            {
                ConnectSec = MultiSectionSelection.adjsec;
            }
            if (MultiSectionSelection.adjsec == String.Empty)
            {
                ConnectSec = _savedAttSection;
            }

            mylineNo = 0;

            decimal startsplitx1 = startSplitX;
            decimal startsplity1 = startSplitY;

            if (BeginSplitX != (float)startSplitX)
            {
                NextStartX = (float)startSplitX;
            }
            if (BeginSplitY != (float)startSplitY)
            {
                NextStartY = (float)startSplitY;
            }

            DataSet Sprolines = null;

            StringBuilder getSpLine = new StringBuilder();
            getSpLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            getSpLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            getSpLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                           //SketchUpGlobals.FcLib,
                           //SketchUpGlobals.FcLocalityPrefix,
                           _currentParcel.mrecno,
                           _currentParcel.mdwell,
                           ConnectSec));

            Sprolines = dbConn.DBConnection.RunSelectStatement(getSpLine.ToString());

            int maxsecline = Sprolines.Tables[0].Rows.Count;
            if (Sprolines.Tables[0].Rows.Count > 0)
            {
                RESpJumpTable.Clear();

                for (int i = 0; i < Sprolines.Tables[0].Rows.Count; i++)
                {
                    DataRow row = RESpJumpTable.NewRow();
                    row["Record"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlrecord"].ToString());
                    row["Card"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jldwell"].ToString());
                    row["Sect"] = Sprolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = Sprolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["XLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["YLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlangle"].ToString());
                    row["XPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["YPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["XPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["YPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
                    row["Attach"] = Sprolines.Tables[0].Rows[i]["jlattach"].ToString();

                    decimal xpt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    decimal ypt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2y"].ToString());

                    float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
                    float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

                    Sprowindex = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());

                    RESpJumpTable.Rows.Add(row);
                }
            }

            if (RESpJumpTable.Rows.Count > 0)
            {
                AttSpLineNo = 0;

                bool foundLine = false;

                int RESpJumpTableIndex = 0;

                if (RESpJumpTable.Rows.Count > 0)
                {
                    for (int i = 0; i < RESpJumpTable.Rows.Count; i++)
                    {
                        if (offsetDir == "N" || offsetDir == "S")
                        {
                            // TODO: Remove if not needed:	float txtsx = NextStartX;
                            // TODO: Remove if not needed:	float txtsy = NextStartY;
                            // TODO: Remove if not needed:	decimal tstxadjr = XadjR;

                            XadjR = Convert.ToDecimal(NextStartX);
                            YadjR = Convert.ToDecimal(NextStartY);

                            decimal testdit = distance;

                            string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                            decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
                            decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
                            decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
                            decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
                            decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
                            decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
                            int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                            string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                            decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

                            decimal testX = startSplitX;
                            decimal testY = startSplitY;

                            decimal check = (y2 - distance);

                            if (offsetDir == "N" && NextStartY != (float)(y2 - distance))
                            {
                                //startSplitY = (y2 - distance);
                            }

                            if (offsetDir == "S")
                            {
                                //startSplitY = (y2 + distance);
                            }

                            if (x2 != startSplitX)
                            {
                                NextStartX = (float)startSplitX;
                            }

                            if (NextStartY != (float)startSplitY)
                            {
                                NextStartY = (float)startSplitY;
                            }

                            int dex1 = x1.ToString().IndexOf(".");
                            int dex2 = x2.ToString().IndexOf(".");

                            int xxt = i;

                            if (NextStartX == (float)x2 && (i + 1) < RESpJumpTable.Rows.Count)
                            {
                                int newLineNbr = Convert.ToInt32(RESpJumpTable.Rows[i + 1]["LineNo"].ToString());

                                if (startSplitY >= y2 && startSplitY <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString()))
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["YLen"].ToString());

                                    begSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt1"].ToString());

                                    begSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt1"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    CurrentSecLtr = atsect;
                                    RemainderLineLength = OrigLineLength - splitLineDist;
                                }
                            }

                            decimal x1x = (x1 + .5m);

                            decimal x1B = (x1 - .5m);

                            decimal x2x = (x2 + .5m);

                            decimal x2B = (x2 - .5m);
                            LineNumberToBreak = lnNbr;
                            if (NextStartY < (float)y1 && NextStartY > (float)y2 && dirsect == "N" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }

                            if (NextStartY > (float)y1 && NextStartY < (float)y2 && dirsect == "S" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }
                        }
                        if (offsetDir == "E" || offsetDir == "W")
                        {
                            float txtsx = NextStartX;

                            float txtsy = NextStartY;

                            decimal tstyadjr = YadjR;

                            XadjR = Convert.ToDecimal(NextStartX);
                            YadjR = Convert.ToDecimal(NextStartY);

                            string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                            decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
                            decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
                            decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
                            decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
                            decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
                            decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
                            int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                            string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                            decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

                            decimal TestX = startSplitX;
                            decimal TestY = startSplitY;

                            if (y2 != startSplitY)
                            {
                                //NextStartY = (float)startSplitY;
                            }

                            if (x2 != startSplitX)
                            {
                                //NextStartX = (float)startSplitX;
                            }

                            int dey1 = y1.ToString().IndexOf(".");
                            int dey2 = y2.ToString().IndexOf(".");

                            decimal mytest = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());

                            if (startSplitY == y2)
                            {
                                if (startSplitX >= x2 && startSplitX <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "E")
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    CurrentSecLtr = atsect;

                                    RemainderLineLength = d2 - splitLineDist;
                                }

                                if (startSplitX <= x2 && startSplitX >= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "W")
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    CurrentSecLtr = atsect;

                                    RemainderLineLength = d2 - splitLineDist;
                                }
                            }

                            decimal y1x = (y1 + .5m);

                            decimal y2x = (y2 + .5m);

                            decimal y1B = (y1 - .5m);

                            decimal y2B = (y2 - .5m);

                            decimal x1x = (x1 + .5m);

                            decimal x1B = (x1 - .5m);

                            decimal x2x = (x2 + .5m);

                            decimal x2B = (x2 - .5m);

                            if (NextStartX >= (float)x1 && NextStartX <= (float)x2 && dirsect == "E" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }

                            if (NextStartX < (float)x1 && NextStartX > (float)x2 && dirsect == "W" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }
                        }
                    }

                    BeginSplitX = NextStartX;

                    BeginSplitY = NextStartY;

                    if (draw)
                    {
                        if (LastDir == "E")
                        {
                            NextStartX = NextStartX + distanceD;

                            float ty = NextStartY;
                        }
                        if (LastDir == "W")
                        {
                            NextStartX = NextStartX - distanceD;

                            float ty = NextStartY;
                        }

                        if (LastDir == "N")
                        {
                            NextStartY = NextStartY - distanceD;

                            float tx = NextStartX;
                        }
                        if (LastDir == "S")
                        {
                            NextStartY = NextStartY + distanceD;

                            float tx = NextStartX;
                        }
                    }

                    BeginSplitX = NextStartX;
                    BeginSplitY = NextStartY;
                }
            }
        }

        private void GetStartCorner()
        {
            _undoMode = true;

            undoPoints.Clear();

            int rowindex = 0;

            for (int i = 0; i < REJumpTable.Rows.Count; i++)
            {
                DataRow row = undoPoints.NewRow();

                float _JumpX1 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt1"].ToString()) * _currentScale)); //  change XPt1 to XPt2
                float _JumpY1 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT1"].ToString()) * _currentScale));
                float _JumpX2 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
                float _JumpY2 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT2"].ToString()) * _currentScale));

                JumpX = _JumpX1;
                JumpY = _JumpY1;
                float JumpX2 = _JumpX2;
                float JumpY2 = _JumpY2;

                int _mouseX1 = Convert.ToInt32(JumpX);
                int _mouseY1 = Convert.ToInt32(JumpY);
                int _mouseX2 = Convert.ToInt32(JumpX2);
                int _mouseY2 = Convert.ToInt32(JumpY2);

                row["Direct"] = REJumpTable.Rows[i]["Direct"].ToString();
                row["X1pt"] = _mouseX1;
                row["Y1pt"] = _mouseY1;
                row["X2pt"] = _mouseX2;
                row["Y2pt"] = _mouseY2;

                undoPoints.Rows.Add(row);
            }

            RedrawSection();
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream dh = new MemoryStream();
            imageIn.Save(dh, System.Drawing.Imaging.ImageFormat.Jpeg);
            return dh.ToArray();
        }

        private void InsertLine(string CurAttDir, decimal newEndX, decimal newEndY, decimal StartEndX, decimal StartEndY, decimal splitLength)
        {
            StringBuilder insertLine = new StringBuilder();
            insertLine.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen, ",
                      SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            insertLine.Append("jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ) values ( ");
            insertLine.Append(String.Format(" {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' )", _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr,
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

        private bool IsMovementAllowed(MoveDirections direction)
        {
            bool isAllowed = (displayDataTable.Rows.Count > 0);

            if (isAllowed && isInAddNewPointMode)
            {
                isAllowed = false;
            }

            if (isAllowed && "".Equals(DistText.Text))
            {
                ShowMessageBox("Please indicate a length");
                isAllowed = false;
            }

            if (isAllowed)
            {
                string dir = displayDataTable.Rows[dgSections.CurrentRow.Index]["Dir"].ToString();
                if (!dir.Equals(direction.ToString()))
                {
                    ShowMessageBox("Illegal Direction");
                    isAllowed = false;
                    RefreshSketch();
                }
            }

            if (isAllowed)
            {
                decimal len = 0M;
                switch (direction)
                {
                    case MoveDirections.N:
                    case MoveDirections.S:
                        len = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["North"].ToString());
                        break;

                    case MoveDirections.E:
                    case MoveDirections.W:
                        len = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["East"].ToString());
                        break;

                    default:
                        break;
                }

                if (len == 0M)
                {
                    isAllowed = false;
                }
                else if (len <= Convert.ToDecimal(DistText.Text))
                {
                    ShowMessageBox("Illegal Distance");
                    isAllowed = false;
                    RefreshSketch();
                }
            }

            return isAllowed;
        }

        private bool IsValidDirection(string moveDirection)
        {
            bool goodDir = (moveDirection == legalMoveDirection || BeginSectionBtn.Text == "Active" || !checkDirection);
            return goodDir;
        }

        private void JumptoCorner()
        {
            float txtx = NextStartX;
            float txty = NextStartY;
            float jx = _mouseX;
            float jy = _mouseY;
            float _scaleBaseX = ScaleBaseX;
            float _scaleBaseY = ScaleBaseY;
            float CurrentScale = _currentScale;
            int crrec = _currentParcel.Record;
            int crcard = _currentParcel.Card;

            CurrentSecLtr = String.Empty;
            _newIndex = 0;
            currentAttachmentLine = 0;
            if (IsNewSketch == false)
            {
                PointF mouseLocation = new PointF(_mouseX,_mouseY);

                foreach (SMLine l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s => s.SectionLetter != SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter))
                {
                    l.ComparisonPoint = mouseLocation;
                }
                int shortestDistance = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines select (int)l.EndPointDistanceFromComparisonPoint).Min();
                List<SMLine> connectionLines = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where (int)l.EndPointDistanceFromComparisonPoint == shortestDistance select l).ToList();
                bool sketchHasLineData = connectionLines.Count > 0;
                if (connectionLines == null || connectionLines.Count == 0)
                {
                    string message = string.Format("No lines contain an available connection point from point {0},{1}", mouseLocation.X, mouseLocation.Y);

                    Trace.WriteLine(message);

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
                        AttachmentSection = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = (from l in connectionLines where l.SectionLetter == AttSectLtr select l).FirstOrDefault();
                    }
                    else
                    {
                        AttSectLtr = SecLetters[0];
                        AttachmentSection = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = connectionLines[0];
                    }
                    ScaledJumpPoint = JumpPointLine.ScaledEndPoint;
                    MoveCursor(scaledJumpPoint);
                }
            }
        }

        private void JumptoCornerOriginal()
        {
            float txtx = NextStartX;
            float txty = NextStartY;
            float jx = _mouseX;
            float jy = _mouseY;
            float _scaleBaseX = ScaleBaseX;
            float _scaleBaseY = ScaleBaseY;
            float CurrentScale = _currentScale;
            int crrec = _currentParcel.Record;
            int crcard = _currentParcel.Card;

            CurrentSecLtr = String.Empty;
            _newIndex = 0;
            currentAttachmentLine = 0;

            DataSet lines = null;
            if (IsNewSketch == false)
            {
                lines = GetLinesData(crrec, crcard);
            }

            bool sketchHasLineData = lines.Tables[0].Rows.Count > 0;
            if (sketchHasLineData)
            {
                SecItemCnt = CountSections();

                // PopulateSectionList();
                for (int i = 0; i < SecItemCnt; i++)
                {
                    string thisSection = SecLetters[i].ToString();
                    if (SecItemCnt >= 1)
                    {
                        CountLines(thisSection);

                        AddXLine(thisSection);

                        lines = GetSectionLines(crrec, crcard);
                    }
                }
                JumpTable = ConstructJumpTable();
                JumpTable.Clear();

                AddListItemsToJumpTableList(jx, jy, CurrentScale, lines);

                string secltr = String.Empty;
                string curltr = String.Empty;

                List<string> AttSecLtrList = new List<string>();

                if (JumpTable.Rows.Count > 0)
                {
                    secltr = FindClosestCorner(CurrentScale, ref curltr, AttSecLtrList);
                }
            }
        }

        private void LoadSection()
        {
            displayDataTable.Rows.Clear();
            if (section.SectionLines != null)
            {
                foreach (var line in section.SectionLines)
                {
                    DataRow dr = displayDataTable.NewRow();
                    dr["Dir"] = line.Directional.Trim();
                    dr["North"] = line.YLength.ToString();
                    dr["East"] = line.XLength.ToString();
                    dr["Att"] = line.Attachment.Trim();
                    displayDataTable.Rows.Add(dr);
                }
            }
        }

        private int MaximumLineCount()
        {
            int maxLineCnt;
            StringBuilder lineCntx = new StringBuilder();
            lineCntx.Append(String.Format("select max(jlline#) from {0}.{1}line ",
                       SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix
                        ));
            lineCntx.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ",
                _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr));

            maxLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCntx.ToString()));
            return maxLineCnt;
        }

        public void MeasureAngle()
        {
            string anglecalls = DistText.Text.Trim();

            int commaCnt = anglecalls.IndexOf(",");

            string D1 = anglecalls.Substring(0, commaCnt).Trim();

            string D2 = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();

            AngD2 = Convert.ToDecimal(D1);

            AngD1 = Convert.ToDecimal(D2);

            AngleForm angleDialog = new AngleForm();
            angleDialog.ShowDialog();

            if (_isKeyValid == false)
            {
                _isKeyValid = true;
            }

            if (AngleForm.NorthWest == true)
            {
                MoveNorthWest(NextStartX, NextStartY);
            }

            if (AngleForm.NorthEast == true)
            {
                MoveNorthEast(NextStartX, NextStartY);
            }
            if (AngleForm.SouthWest == true)
            {
                MoveSouthWest(NextStartX, NextStartY);
            }
            if (AngleForm.SouthEast == true)
            {
                MoveSouthEast(NextStartX, NextStartY);
            }
        }

        private void MoveCursor()
        {
            Color penColor;
            Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Convert.ToInt32(JumpX) - 50, Convert.ToInt32(JumpY) - 50);

            penColor = (_undoMode || draw) ? Color.Red : Color.Black;

            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(Color.Red, 4);
            g.DrawRectangle(pen1, Convert.ToInt32(JumpX), Convert.ToInt32(JumpY), 1, 1);
            g.Save();

            ExpSketchPBox.Image = MainImage;

            DMouseClick();
        }

        private decimal OriginalDistanceX()
        {
            decimal origDistX = 0;

            StringBuilder orgLen = new StringBuilder();
            orgLen.Append(String.Format("select jllinelen from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        _currentParcel.mrecno,
                        _currentParcel.mdwell
                        ));
            orgLen.Append(String.Format("and jlsect = '{0}' and jlline# = {1} ",
                CurrentSecLtr, AttSpLineNo));

            origDistX = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(orgLen.ToString()));
            return origDistX;
        }

        private void PerformMoveLength(MoveDirections direction)
        {
            if (IsMovementAllowed(direction))
            {
                int cr = dgSections.CurrentRow.Index;
                try
                {
                    pts = section.SectionPoints;
                    Point[] adjPts = new Point[section.SectionPoints.Length + 1];

                    decimal length = Convert.ToDecimal(DistText.Text);

                    DataRow dr = displayDataTable.NewRow();
                    string dir = "";

                    for (int i = 0; i < pts.Length; i++)
                    {
                        if ((i < dgSections.CurrentRow.Index) ||
                            (i == dgSections.CurrentRow.Index && dgSections.CurrentRow.Index != pts.Length - 1))
                        {
                            adjPts[i] = pts[i];
                        }
                        else if ((i == dgSections.CurrentRow.Index + 1) ||
                            (i == dgSections.CurrentRow.Index && dgSections.CurrentRow.Index == pts.Length - 1))
                        {
                            isLastLine = (i == dgSections.CurrentRow.Index && dgSections.CurrentRow.Index == pts.Length - 1);
                            bool isLineAdded = false;
                            switch (direction)
                            {
                                case MoveDirections.NW:
                                    dir = "NW";
                                    break;

                                case MoveDirections.NE:
                                    dir = "NE";
                                    break;

                                case MoveDirections.SE:
                                    dir = "SE";
                                    break;

                                case MoveDirections.SW:
                                    dir = "SW";
                                    break;

                                case MoveDirections.N:
                                    dir = "N";
                                    dr["North"] = length.ToString();
                                    dr["East"] = "0.0";
                                    decimal old = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["North"]);
                                    old -= length;
                                    displayDataTable.Rows[dgSections.CurrentRow.Index]["North"] = old.ToString();
                                    int adjLength = 0 - Convert.ToInt32(length);
                                    if (isLastLine)
                                    {
                                        adjPts[i] = pts[i];
                                        adjPts[i + 1] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
                                    }
                                    else
                                    {
                                        adjPts[i] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
                                        adjPts[i + 1] = pts[i];
                                    }
                                    isLineAdded = true;
                                    break;

                                case MoveDirections.S:
                                    dir = "S";
                                    dr["North"] = length.ToString();
                                    dr["East"] = "0.0";
                                    old = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["North"]);
                                    old -= length;
                                    displayDataTable.Rows[dgSections.CurrentRow.Index]["North"] = old.ToString();
                                    adjLength = Convert.ToInt32(length);
                                    if (isLastLine)
                                    {
                                        adjPts[i] = pts[i];
                                        adjPts[i + 1] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
                                    }
                                    else
                                    {
                                        adjPts[i] = new Point(pts[i].X, pts[i - 1].Y + adjLength);
                                        adjPts[i + 1] = pts[i];
                                    }
                                    isLineAdded = true;
                                    break;

                                case MoveDirections.E:
                                    dir = "E";
                                    dr["North"] = "0.0";
                                    dr["East"] = length.ToString();
                                    old = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["East"]);
                                    old -= length;
                                    displayDataTable.Rows[dgSections.CurrentRow.Index]["East"] = old.ToString();
                                    adjLength = Convert.ToInt32(length);
                                    if (isLastLine)
                                    {
                                        adjPts[i] = pts[i];
                                        adjPts[i + 1] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
                                    }
                                    else
                                    {
                                        adjPts[i] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
                                        adjPts[i + 1] = pts[i];
                                    }
                                    isLineAdded = true;
                                    break;

                                case MoveDirections.W:
                                    dir = "W";
                                    dr["North"] = "0.0";
                                    dr["East"] = length.ToString();
                                    old = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["East"]);
                                    old -= length;
                                    displayDataTable.Rows[dgSections.CurrentRow.Index]["East"] = old.ToString();
                                    adjLength = 0 - Convert.ToInt32(length);
                                    if (isLastLine)
                                    {
                                        adjPts[i] = pts[i];
                                        adjPts[i + 1] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
                                    }
                                    else
                                    {
                                        adjPts[i] = new Point(pts[i - 1].X + adjLength, pts[i].Y);
                                        adjPts[i + 1] = pts[i];
                                    }
                                    isLineAdded = true;
                                    break;

                                default:
                                    break;
                            }
                            if (isLineAdded)
                            {
                                SetAddNewPointButton(true);
                                if (isLastLine)
                                {
                                    NewPointIndex = ++i;
                                }
                                else
                                {
                                    NewPointIndex = i;
                                }
                            }
                        }
                        else
                        {
                            adjPts[i + 1] = pts[i];
                        }
                    }

                    unadj_pts = adjPts;

                    dr["Dir"] = dir;
                    dr["Att"] = "";

                    displayDataTable.Rows.InsertAt(dr, dgSections.CurrentRow.Index);

                    DistText.Text = "";
                    DrawSketch(dgSections.CurrentRow.Index);
                }
                catch (System.FormatException)
                {
                    ShowMessageBox("Invalid Length");
                    RefreshSketch();
                }
            }
        }

        public void RefreshSketch()
        {
            ExpSketchPBox.Refresh();
            MainImage = null;
            float scaleOut = 0.00f;
            SMParcel parcel = SketchUpGlobals.ParcelWorkingCopy;
            MainImage = _currentParcel.GetSketchImage(parcel,ExpSketchPBox.Width, ExpSketchPBox.Height,
                1000, 572, 400, out scaleOut);
            DrawingScale = scaleOut;
            _currentScale = DrawingScale;
            RenderSketch();

            Graphics g = Graphics.FromImage(MainImage);
            SolidBrush Lblbrush = new SolidBrush(Color.Black);
            SolidBrush FillBrush = new SolidBrush(Color.White);
            Pen whitePen = new Pen(Color.White, 2);
            Pen blackPen = new Pen(Color.Black, 2);

            Font LbLf = new Font("Arial", 10, FontStyle.Bold);
            Font TitleF = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline);
            Font MainTitle = new System.Drawing.Font("Arial", 15, FontStyle.Bold | FontStyle.Underline);
            char[] leadzero = new char[] { '0' };

            g.DrawString(Locality, TitleF, Lblbrush, new PointF(10, 10));
            g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            g.DrawString(String.Format("Record # - {0}", SketchRecord.TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            g.DrawString(String.Format("Card # - {0}", SketchCard), LbLf, Lblbrush, new PointF(10, 45));

            g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));

            ExpSketchPBox.Image = MainImage;

            if (_closeSketch == true)
            {
                Close();
            }
        }

        private void ReOpenSec()
        {
            int rowindex = 0;

            DataSet rolines = null;

            StringBuilder getLine = new StringBuilder();
            getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                       SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        _currentParcel.mrecno,
                        _currentParcel.mdwell,
                        SketchUpGlobals.ReOpenSection));

            rolines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

            int maxsecline = rolines.Tables[0].Rows.Count;
            if (rolines.Tables[0].Rows.Count > 0)
            {
                REJumpTable.Clear();

                for (int i = 0; i < rolines.Tables[0].Rows.Count; i++)
                {
                    decimal Distance = 0;

                    DataRow row = REJumpTable.NewRow();
                    row["Record"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlrecord"].ToString());
                    row["Card"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jldwell"].ToString());
                    row["Sect"] = rolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = rolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["XLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["YLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlangle"].ToString());
                    row["XPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["YPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["XPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["YPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
                    row["Attach"] = rolines.Tables[0].Rows[i]["jlattach"].ToString();

                    decimal xpt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    decimal ypt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2y"].ToString());

                    float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
                    float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

                    rowindex = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());

                    _StartX.Add(rowindex, xPoint);

                    _StartY.Add(rowindex, yPoint);

                    REJumpTable.Rows.Add(row);
                }

                float _JumpXT = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale));

                float _JumpX = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
                float _JumpY = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["YPT2"].ToString()) * _currentScale));

                JumpX = _JumpX;
                JumpY = _JumpY;

                GetStartCorner();
            }
        }

        private void Reorder()
        {
            Garcnt = 0;
            GarSize = 0;
            CPcnt = 0;
            CPSize = 0;

            int tg = _currentParcel.mgart;

            int tg2 = _currentParcel.mgart2;

            int tc = _currentParcel.mcarpt;

            StringBuilder getSect = new StringBuilder();
            getSect.Append(String.Format("select jsrecord,jsdwell,jssect,jstype,jssqft from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
                              SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.Record, _currentParcel.Card));
            getSect.Append(" order by jssect ");

            DataSet ds = dbConn.DBConnection.RunSelectStatement(getSect.ToString());

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionLtrs.NewRow();
                    row["RecNo"] = _currentParcel.Record;
                    row["CardNo"] = _currentParcel.Card;

                    //row["CurSecLtr"] = ds.Tables[0].Rows[i]["jssect"].ToString();
                    //row["NewSecLtr"] = Letters[i].ToString();
                    row["CurSecLtr"] = CurrentSecLtr;
                    row["NewSecLtr"] = NextSectLtr;
                    row["NewType"] = ds.Tables[0].Rows[i]["jstype"].ToString();
                    row["SectSize"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());

                    SectionLtrs.Rows.Add(row);

                    if (CamraSupport.GarageTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
                    {
                        Garcnt++;

                        GarSize = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
                    }
                    if (CamraSupport.CarPortTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
                    {
                        CPcnt++;

                        CPSize = CPSize + Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
                    }
                }
            }

            if (Garcnt == 0)
            {
                StringBuilder zerogar = new StringBuilder();
                zerogar.Append(String.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0,mgart2 = 0,mgar#2 = 0 where mrecno = {2} and mdwell = {3} ",
                                        SketchUpGlobals.LocalLib,
                                        SketchUpGlobals.LocalityPreFix,
                                        _currentParcel.mrecno,
                                        _currentParcel.mdwell));

                dbConn.DBConnection.ExecuteNonSelectStatement(zerogar.ToString());

                ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
            }
            if (CPcnt == 0)
            {
                StringBuilder zerocp = new StringBuilder();
                zerocp.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
                                        SketchUpGlobals.LocalLib,
                                        SketchUpGlobals.LocalityPreFix,
                                        _currentParcel.mrecno,
                                        _currentParcel.mdwell));

                dbConn.DBConnection.ExecuteNonSelectStatement(zerocp.ToString());

                ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
            }

            if (Garcnt > 0)
            {
                if (Garcnt == 1 && _currentParcel.mgart <= 60 || Garcnt == 1 && _currentParcel.mgart == 63 || Garcnt == 1 && _currentParcel.mgart == 64)
                {
                    MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (MissingGarageData.GarCode != _currentParcel.orig_mgart)
                    {
                        StringBuilder fixCp = new StringBuilder();
                        fixCp.Append(String.Format("update {0}.{1}mast set mgart = {2},mgar#c = {3},mgart2 = 0,mgar#2 = 0 ",
                          SketchUpGlobals.LocalLib,
                              SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            MissingGarageData.GarCode,
                            MissingGarageData.GarNbr));
                        fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

                        dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

                        ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
                    }
                }
                if (Garcnt > 1 && _currentParcel.mgart2 == 0)
                {
                    MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (MissingGarageData.GarCode != _currentParcel.orig_mgart2)
                    {
                        StringBuilder fixCp = new StringBuilder();
                        fixCp.Append(String.Format("update {0}.{1}mast set mgart2 = {2},mgar#2 = {3} ",
                          SketchUpGlobals.LocalLib,
                              SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            MissingGarageData.GarCode,
                            MissingGarageData.GarNbr));
                        fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

                        dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

                        ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
                    }
                }
                if (Garcnt > 2)
                {
                    MissingGarageData missGar = new MissingGarageData(dbConn, _currentParcel, GarSize, "GAR");
                    missGar.ShowDialog();

                    int newgarcnt = _currentParcel.mgarN2 + MissingGarageData.GarNbr;

                    StringBuilder addcp = new StringBuilder();
                    addcp.Append(String.Format("update {0}.{1}mast set mgar#2 = {2} where mrecno = {3} and mdwell = {4} ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix,
                            newgarcnt,
                            _currentParcel.mrecno,
                            _currentParcel.mdwell));

                    dbConn.DBConnection.ExecuteNonSelectStatement(addcp.ToString());

                    ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
                }
            }
            if (CPcnt > 0)
            {
                if (CPcnt > 0 && _currentParcel.mcarpt == 0 || CPcnt > 0 && _currentParcel.mcarpt == 67)
                {
                    MissingGarageData missCP = new MissingGarageData(dbConn, _currentParcel, CPSize, "CP");
                    missCP.ShowDialog();

                    if (MissingGarageData.CPCode != _currentParcel.orig_mcarpt)
                    {
                        StringBuilder fixCp = new StringBuilder();
                        fixCp.Append(String.Format("update {0}.{1}mast set mcarpt = {2},mcar#c = {3} ",
                           SketchUpGlobals.LocalLib,
                              SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            MissingGarageData.CPCode,
                            MissingGarageData.CpNbr));
                        fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

                        dbConn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

                        ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
                    }
                }

                if (CPcnt > 1 && _currentParcel.mcarpt != 0 || CPcnt > 1 && _currentParcel.mcarpt != 67)
                {
                    MissingGarageData missCPx = new MissingGarageData(dbConn, _currentParcel, CPSize, "CP");
                    missCPx.ShowDialog();

                    int newcpcnt = _currentParcel.mcarNc + MissingGarageData.CpNbr;

                    StringBuilder addcp = new StringBuilder();
                    addcp.Append(String.Format("update {0}.{1}mast set mcar#c = {2} where mrecno = {3} and mdwell = {4} ",
                            SketchUpGlobals.LocalLib,
                            SketchUpGlobals.LocalityPreFix,
                            newcpcnt,
                            _currentParcel.mrecno,
                            _currentParcel.mdwell));

                    dbConn.DBConnection.ExecuteNonSelectStatement(addcp.ToString());

                    ParcelData.getParcel(dbConn, _currentParcel.mrecno, _currentParcel.mdwell);
                }
            }

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                StringBuilder fixSect = new StringBuilder();
                fixSect.Append(String.Format("update {0}.{1}section set jssect = '{2}' where jsrecord = {3} and jsdwell = {4} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix,
                    SectionLtrs.Rows[j]["NewSecLtr"].ToString().Trim(), _currentParcel.Record, _currentParcel.Card));
                fixSect.Append(String.Format(" and jssect = '{0}' ", SectionLtrs.Rows[j]["CurSecLtr"].ToString().Trim()));

                //fox.DBConnection.ExecuteNonSelectStatement(fixSect.ToString());
            }

            string newLineLtr = String.Empty;
            string oldLineLtr = String.Empty;
            for (int k = 0; k < SectionLtrs.Rows.Count; k++)
            {
                //newLineLtr = SectionLtrs.Rows[k]["NewSecLtr"].ToString().Trim();

                //oldLineLtr = SectionLtrs.Rows[k]["CurSecLtr"].ToString().Trim();

                newLineLtr = NextSectLtr.Trim();

                oldLineLtr = CurrentSecLtr.Trim();

                //upDlineLtr(newLineLtr, oldLineLtr);
            }
        }

        private void SaveJumpPointsAndOldSectionEndPoints(float CurrentScale, int rowindex, DataView SortedJumpTableDataView)
        {
            try
            {
                float _JumpX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt2"].ToString()) * CurrentScale)); //  change XPt1 to XPt2
                float _JumpY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPT2"].ToString()) * CurrentScale));

                JumpX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));  //  change XPt1 to XPt2
                JumpY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));

                float _endOldSecX = (ScaleBaseX + (Convert.ToSingle(JumpTable.Rows[rowindex]["XPt1"].ToString()) * CurrentScale));//   change XPt2 to XPt1
                float _endOldSecY = (ScaleBaseY + (Convert.ToSingle(JumpTable.Rows[rowindex]["YPt1"].ToString()) * CurrentScale)); // ScaleBaseY was ScaleBaseX ??

                endOldSecX = (ScaleBaseX + (Convert.ToSingle(SortedJumpTableDataView[0]["XPt2"].ToString()) * CurrentScale));//   change XPt2 to XPt1
                endOldSecY = (ScaleBaseY + (Convert.ToSingle(SortedJumpTableDataView[0]["YPt2"].ToString()) * CurrentScale));  // ScaleBaseY was ScaleBaseX ??
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private void SaveSketchData()
        {
            if (isInAddNewPointMode)
            {
                if (isLastLine)
                {
                    section.SectionLines.TrimExcess();
                    int lastLine = section.SectionLines.Count;
                    int lastRow = displayDataTable.Rows.Count - 1;

                    var prevLine = section.SectionLines[lastLine];
                    prevLine.YLength = Convert.ToDecimal(displayDataTable.Rows[lastRow]["North"].ToString());
                    prevLine.XLength = Convert.ToDecimal(displayDataTable.Rows[lastRow]["East"].ToString());
                    prevLine.Point1X = Convert.ToDecimal(unadj_pts[lastRow].X);
                    prevLine.Point1Y = Convert.ToDecimal(unadj_pts[lastRow].Y);
                    prevLine.Point2X = Convert.ToDecimal(unadj_pts[0].X);
                    prevLine.Point2Y = Convert.ToDecimal(unadj_pts[0].Y);
                    prevLine.Update();

                    section.SectionLines[lastLine].IncrementLineNumber();

                    var newLine = new BuildingLine();
                    newLine.Record = section.Record;
                    newLine.Card = section.Card;
                    newLine.SectionLetter = section.SectionLetter;
                    newLine.LineNumber = lastLine;
                    newLine.Directional = displayDataTable.Rows[lastRow - 1]["Dir"].ToString();
                    newLine.YLength = Convert.ToDecimal(displayDataTable.Rows[lastRow - 1]["North"].ToString());
                    newLine.XLength = Convert.ToDecimal(displayDataTable.Rows[lastRow - 1]["East"].ToString());
                    newLine.Point1X = Convert.ToDecimal(unadj_pts[lastRow - 1].X);
                    newLine.Point1Y = Convert.ToDecimal(unadj_pts[lastRow - 1].Y);
                    newLine.Point2X = Convert.ToDecimal(unadj_pts[lastRow].X);
                    newLine.Point2Y = Convert.ToDecimal(unadj_pts[lastRow].Y);
                    newLine.Insert();
                }
                else
                {
                    var prevLine = section.SectionLines[NewPointIndex];
                    prevLine.YLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex]["North"].ToString());
                    prevLine.XLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex]["East"].ToString());
                    prevLine.Point1X = Convert.ToDecimal(unadj_pts[NewPointIndex].X);
                    prevLine.Point1Y = Convert.ToDecimal(unadj_pts[NewPointIndex].Y);
                    prevLine.Point2X = Convert.ToDecimal(unadj_pts[NewPointIndex + 1].X);
                    prevLine.Point2Y = Convert.ToDecimal(unadj_pts[NewPointIndex + 1].Y);
                    prevLine.Update();

                    section.IncrementAllLines(NewPointIndex);

                    var newLine = new BuildingLine();
                    newLine.Record = section.Record;
                    newLine.Card = section.Card;
                    newLine.SectionLetter = section.SectionLetter;
                    newLine.LineNumber = NewPointIndex;
                    newLine.Directional = displayDataTable.Rows[NewPointIndex - 1]["Dir"].ToString();
                    newLine.YLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex - 1]["North"].ToString());
                    newLine.XLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex - 1]["East"].ToString());
                    newLine.Point1X = Convert.ToDecimal(unadj_pts[NewPointIndex - 1].X);
                    newLine.Point1Y = Convert.ToDecimal(unadj_pts[NewPointIndex - 1].Y);
                    newLine.Point2X = Convert.ToDecimal(unadj_pts[NewPointIndex].X);
                    newLine.Point2Y = Convert.ToDecimal(unadj_pts[NewPointIndex].Y);
                    newLine.Insert();
                }

                SetAddNewPointButton(false);
            }
        }

        private void SetAddNewPointButton(bool enabled)
        {
            isInAddNewPointMode = enabled;
        }

        public void setAttPnts()
        {
            SMParcel newCopy = SketchUpGlobals.ParcelWorkingCopy;
            newCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(newCopy);
            var AttachPoints = (from l in
                  SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s =>
                  s.SectionLetter != "A" && s.LineNumber == 1).ToList()
                                select l);

            //{
            //    DataRow row = AttachPoints.NewRow();
            //    row["RecNo"] = l.Record;
            //    row["CardNo"] = l.Dwelling;
            //    row["Sect"] = l.SectionLetter;
            //    row["Direct"] = l.Direction;
            //    row["Xpt1"] = l.StartX.ToString();
            //    row["Ypt1"] = l.StartY.ToString();
            //    row["Xpt2"] = l.EndX.ToString();
            //    row["Ypt2"] = l.StartY.ToString();
            //    row["Attch"] = l.AttachedSection;

            //    AttachPoints.Rows.Add(row);
            //}
            if (AttachPoints.ToList().Count > 0)
            {
                newCopy.Sections.Where(s => s.SectionLetter ==
                    NextSectLtr.Trim()).FirstOrDefault().Lines.Where(a =>
                    a.AttachedSection ==
                    NextSectLtr).FirstOrDefault().AttachedSection = " ";
            }

            if (MultiSectionSelection.adjsec == String.Empty)
            {
                ConnectSec = "A";
            }

            if (MultiSectionSelection.adjsec != String.Empty)
            {
                ConnectSec = MultiSectionSelection.adjsec;
            }

            foreach (SMLine l in
                SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s =>
                s.SectionLetter != "A").ToList())
            {
                int record = l.Record;
                int card = l.Dwelling;
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
            newCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(newCopy);
        }

        public void ShowDistanceForm(string _closeY, decimal _ewDist, string _closeX, decimal _nsDist, bool openForm)
        {
            if (openForm == true)
            {
                SketchDistance sketDist = new SketchDistance(_closeY, _ewDist, _closeX, _nsDist);
                sketDist.Show(this);

                sketDist.Close();
            }
        }

        private void ShowMessageBox(string s)
        {
            MessageBox.Show(s);
        }

        //private void upDlineLtr(string newLtr, string old)
        //{
        //    StringBuilder fixLine = new StringBuilder();
        //    fixLine.Append(String.Format("update {0}.{1}line set jlsect = '{2}' where jlrecord = {3} and jldwell = {4} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix,
        //         newLtr, _currentParcel.Record, _currentParcel.Card));
        //    fixLine.Append(String.Format(" and jlsect = '{0}' ", old));

        //    fox.DBConnection.ExecuteNonSelectStatement(fixLine.ToString());

        //}

        private void sortSection()
        {
            FixSect = new List<string>();

            StringBuilder addFix = new StringBuilder();
            addFix.Append(String.Format("select jlsect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        _currentParcel.Record,
                        _currentParcel.Card));

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
                    chkLen.Append(String.Format("end as LineLen, abs(jlpt1x-jlpt2x) as Xlen, abs(jlpt1y-jlpt2y) as Ylen from {0}.{1}line ",
                                  SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix

                                    //SketchUpGlobals.FcLib,
                                    //SketchUpGlobals.FcLocalityPrefix
                                    ));
                    chkLen.Append(String.Format("where jlrecord = {0} and jldwell = {1} order by jlsect,jlline# ", _currentParcel.Record, _currentParcel.Card));

                    DataSet fixl = dbConn.DBConnection.RunSelectStatement(chkLen.ToString());

                    if (fixl.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < fixl.Tables[0].Rows.Count; i++)
                        {
                            //MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 3177", _currentParcel.Record, _currentParcel.Card));

                            StringBuilder updLine = new StringBuilder();
                            updLine.Append(String.Format("update {0}.{1}line set jlxlen = {2},jlylen = {3},jllinelen = {4} ",
                                           SketchUpGlobals.LocalLib,
                                           SketchUpGlobals.LocalityPreFix,

                                            //SketchUpGlobals.FcLib,
                                            //SketchUpGlobals.FcLocalityPrefix,
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Xlen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Ylen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["LineLen"].ToString())));
                            updLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                    _currentParcel.Record,
                                    _currentParcel.Card,
                                    fixl.Tables[0].Rows[i]["jlsect"].ToString(),
                                    Convert.ToInt32(fixl.Tables[0].Rows[i]["jlline#"].ToString())));

                            dbConn.DBConnection.ExecuteNonSelectStatement(updLine.ToString());
                        }
                    }
                }
            }
        }

        public void SplitLine()
        {
            int ln = _savedAttLine;
            string asec = _savedAttSection;
            string newSec = NextSectLtr;
            string tstdir = CurrentAttDir;
            decimal begx = OrigStartX;
            decimal begy = OrigStartY;
            decimal OldbegX = startSplitX;
            decimal OldbegY = startSplitY;
            decimal OldbegX1 = OrigStartX;
            decimal OldbegY1 = OrigStartY;
            float oldx = endOldSecX;
            float oldy = endOldSecY;
            string splitLineDir = offsetDir;
            string CurAttDir = String.Empty;
            string NextAttDir = String.Empty;
            decimal OrigY = OrigStartY;
            decimal newEndX = 0;
            decimal newEndY = 0;
            decimal newEndX1 = EndSplitX;
            decimal newEndY1 = EndSplitY;

            //TODO: See if setting this to absolute value will correct the problem.
            decimal finalNewLen = (OrigLineLength - splitLineDist);

            float mySx = BeginSplitX;

            float mySy = BeginSplitY;

            RemainderLineLength = finalNewLen;

            JumpTable = new DataTable();

            int chekattline = TempAttSplineNo;

            _savedAttLine = AttSpLineNo;
            if (AttSpLineNo == 0)
            {
                _savedAttLine = TempAttSplineNo;
                AttSpLineNo = TempAttSplineNo;
            }

            if (MultiSectionSelection.adjsec != String.Empty)
            {
                _savedAttSection = MultiSectionSelection.adjsec;
            }

            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.Record,
                            _currentParcel.Card,
                            _savedAttSection));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = _currentParcel.mrecno;
                    row["Card"] = _currentParcel.mdwell;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    SectionTable.Rows.Add(row);
                }
            }

            CurrentAttDir = offsetDir;

            // TODO: Remove if not needed:	string tstOffset = offsetDir;

            CurAttDir = CurrentAttDir;

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                string getNCurDir = string.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline#= {5} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, _currentParcel.Record, _currentParcel.Card, _savedAttSection, AttSpLineNo);

                CurrentAttDir = dbConn.DBConnection.ExecuteScalar(getNCurDir.ToString()).ToString();
            }

            _savedAttLine = AttSpLineNo;

            if (_savedAttLine == 0)
            {
            }

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                StringBuilder nxtAttDir = new StringBuilder();
                nxtAttDir.Append(String.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline# = {5} ",
                               SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                _currentParcel.mrecno,
                                _currentParcel.mdwell,
                                CurrentSecLtr,
                                _savedAttLine));

                NextAttDir = dbConn.DBConnection.ExecuteScalar(nxtAttDir.ToString()).ToString();
            }

            decimal myoriglen = 0;
            mylineNo = 0;

            if (CurAttDir.Trim() == offsetDir.Trim())
            {
                StringBuilder getoriglen = new StringBuilder();
                getoriglen.Append(String.Format("select jllinelen,jlline# from {0}.{1}line where jlrecord = {2} and jldwell = {3}  and jlsect = '{4}' and jlline# = {5}",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.mrecno,
                            _currentParcel.mdwell,
                            CurrentSecLtr,
                            _savedAttLine));
                getoriglen.Append(String.Format(" and jldirect = '{0}'", offsetDir));

                DataSet myds = dbConn.DBConnection.RunSelectStatement(getoriglen.ToString());

                if (myds.Tables[0].Rows.Count > 0)
                {
                    myoriglen = Convert.ToDecimal(myds.Tables[0].Rows[0]["jllinelen"].ToString());
                    mylineNo = Convert.ToInt32(myds.Tables[0].Rows[0]["jlline#"].ToString());
                }
            }

            int maxLineCnt = 0;

            decimal origEndx = 0;
            decimal origEndy = 0;
            decimal origDist = 0;
            decimal newDistX = 0;
            decimal newDistY = 0;
            decimal StartEndX = 0;
            decimal StartEndY = 0;
            decimal EndEndX = 0;
            decimal EndEndY = 0;
            decimal origLen = 0;
            decimal origXlen = 0;
            decimal origYlen = 0;
            decimal NewSplitDist = 0;

            StringBuilder getOrigEnds = new StringBuilder();
            getOrigEnds.Append(String.Format("select jldirect,jlsect,jlline#,jlxlen,jlylen,jllinelen,jlpt1x,jlpt1y,jlpt2x,jlpt2y from {0}.{1}line ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix
                            ));
            getOrigEnds.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr, mylineNo));

            DataSet orgLin = dbConn.DBConnection.RunSelectStatement(getOrigEnds.ToString());

            if (orgLin.Tables[0].Rows.Count > 0)
            {
                origEndx = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
                origEndy = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

                endOldSecX = Convert.ToSingle(origEndx);
                endOldSecY = Convert.ToSingle(origEndy);

                StartEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1x"].ToString());
                StartEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1y"].ToString());

                EndEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
                EndEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

                CurrentAttDir = orgLin.Tables[0].Rows[0]["jldirect"].ToString().Trim();
                origLen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

                origXlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlxlen"].ToString());
                origYlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlylen"].ToString());

                origDist = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

                NewSplitLIneDist = origLen - splitLineDist;
            }

            NewSplitDist = Math.Abs(myoriglen - splitLineDist);

            decimal origDistX = OriginalDistanceX();

            if (origDistX != myoriglen)
            {
                origDistX = myoriglen;
            }

            maxLineCnt = MaximumLineCount();

            if (maxLineCnt > 0 && mylineNo <= maxLineCnt)
            {
                decimal ttue = splitLineDist;
                StringBuilder incrLine = new StringBuilder();
                incrLine.Append(String.Format("update {0}.{1}line set jlline# = jlline#+1 ",
                          SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
                incrLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# > {3} ", _currentParcel.mrecno, _currentParcel.mdwell, CurrentSecLtr, _savedAttLine));

                //Ask Dave why this is not excecuting. Can this whole section be removed?
                //fox.DBConnection.ExecuteNonSelectStatement(incrLine.ToString());

                if (_savedAttLine > 0 && RemainderLineLength != 0)
                {
                    for (int i = _savedAttLine - 1; i < SectionTable.Rows.Count; i++)
                    {
                        int tske = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());

                        SectionTable.Rows[i]["LineNo"] = tske + 1;
                    }
                }

                DeleteLineSection();

                for (int i = 0; i < SectionTable.Rows.Count; i++)
                {
                    StringBuilder instLine = new StringBuilder();
                    instLine.Append(String.Format("insert into {0}.{1}line ",
                                  SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
                    instLine.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                                        Convert.ToInt32(SectionTable.Rows[i]["Record"].ToString()),
                                        Convert.ToInt32(SectionTable.Rows[i]["Card"].ToString()),
                                        SectionTable.Rows[i]["Sect"].ToString().Trim(),
                                        Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()),
                                        SectionTable.Rows[i]["Direct"].ToString().Trim(),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Xlen"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Ylen"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Angle"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString()),
                                        SectionTable.Rows[i]["Attach"].ToString().Trim()));

                    decimal td1 = Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString());

                    decimal td2 = Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString());

                    decimal td3 = Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString());

                    decimal td4 = Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString());

                    decimal td5 = Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString());

                    if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) <= 20)
                    {
                        dbConn.DBConnection.ExecuteNonSelectStatement(instLine.ToString());
                    }
                    if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) > 20)
                    {
                        MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    }
                }
            }

            decimal xlen = 0;
            decimal ylen = 0;

            if (offsetDir == "N" || offsetDir == "S")  // offsetDir
            {
                xlen = 0;
                ylen = splitLineDist;
                newDistX = 0;
                newDistY = origDist - splitLineDist;
                adjNewSecX = 0;
                adjNewSecY = splitLineDist;
            }
            if (offsetDir == "E" || offsetDir == "W")   // offsetDir
            {
                xlen = splitLineDist;
                ylen = 0;
                newDistX = origDist - splitLineDist;
                newDistY = 0;
                adjNewSecY = 0;
                adjNewSecX = splitLineDist;
            }

            decimal x1t = adjNewSecX;
            decimal y1t = adjNewSecY;

            decimal splitLength = splitLineDist;

            if (OrigLineLength != (RemainderLineLength + splitLineDist))
            {
                OrigLineLength = (RemainderLineLength + splitLineDist);
            }

            if (CurAttDir.Trim() == offsetDir.Trim())
            {
                decimal utp = splitLength;

                decimal ttue = splitLineDist;
                StringBuilder fixOrigLine = new StringBuilder();
                fixOrigLine.Append(String.Format("update {0}.{1}line ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix
                            ));
                fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
                                        adjNewSecX,
                                        adjNewSecY,
                                        splitLength,
                                        NewSectionBeginPointX,
                                        NewSectionBeginPointY));
                fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                _currentParcel.mrecno,
                                _currentParcel.mdwell,
                                CurrentSecLtr,
                                _savedAttLine));

                //Ask Dave why this is not being called. Can it be removed?
                //fox.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
            }

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                if (offsetDir == "N" || offsetDir == "S")
                {
                    RemainderLineLength = OrigLineLength - adjNewSecY;
                }
                if (offsetDir == "W" || offsetDir == "E")
                {
                    adjNewSecX = RemainderLineLength;
                }
                adjNewSecY = RemainderLineLength;

                if (RemainderLineLength > 0)
                {
                    FixOrigLine();
                }
            }

            if (offsetDir == "N" || offsetDir == "S")
            {
                adjOldSecX = 0;
                adjOldSecY = RemainderLineLength;
            }

            if (offsetDir == "E" || offsetDir == "W")
            {
                adjOldSecY = 0;
                adjOldSecX = RemainderLineLength;
            }

            int newLineNbr = _savedAttLine + 1;

            if (CurAttDir.Trim() != splitLineDir.Trim())
            {
                splitLineDir = CurAttDir;
            }

            decimal endNewSecX = adjNewSecX;
            switch (AttSpLineDir)
            {
                case "E":
                    newEndX = (OrigStartX + splitLineDist);
                    newEndY = OrigStartY;
                    break;

                case "W":
                    endNewSecX = adjNewSecX * -1;
                    newEndX = (OrigStartX - splitLineDist);
                    newEndY = OrigStartY;
                    break;

                case "N":
                    newEndY = (OrigStartY - splitLineDist);
                    newEndX = OrigStartX;
                    break;

                case "S":

                    EndSplitY = EndSplitY * -1;
                    newEndY = (OrigStartY + splitLineDist);
                    newEndX = OrigStartX;
                    break;

                default:
                    Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. AttSpLineDir not in NEWS. ", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
                    break;
            }

            if (RemainderLineLength > 0)
            {
                InsertLine(CurAttDir, newEndX, newEndY, StartEndX, StartEndY, splitLength);
            }

            decimal jjel = OrigLineLength;

            decimal finEndX = 0;
            decimal finEndY = 0;
            decimal finDist = 0;
            switch (CurrentAttDir)
            {
                case "N":

                    finDist = (OrigStartY - splitLineDist);

                    finDist = newDistY;
                    break;

                case "S":

                    finDist = (OrigStartY + splitLineDist);
                    finDist = newDistY;
                    break;

                case "E":

                    finDist = (OrigStartX + splitLineDist);

                    finDist = newDistX;
                    break;

                case "W":

                    finDist = (OrigStartX - splitLineDist);
                    finDist = newDistX;
                    break;

                default:
                    Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. CurrentAttDir not in NEWS.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
                    break;
            }
            AdjustLine(newEndX, newEndY, newDistX, newDistY, EndEndX, EndEndY, finDist);
        }

        #endregion General Class Methods

        #region Refactored out from constructor

        #region Methods Checked and left as original with SM refactor

        #endregion Methods Checked and left as original with SM refactor

        #region Methods refactored to use SketchManager

        public void AddNewSection()
        {
            //Section is now added at the close of the AddSection form
        }

        #endregion Methods refactored to use SketchManager

        private void AddJumpTableRow(float jx, float jy, float CurrentScale, DataSet lines, int i)
        {
            decimal Distance = 0;

            DataRow row = JumpTable.NewRow();
            row["Record"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jlrecord"].ToString());
            row["Card"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jldwell"].ToString());
            row["Sect"] = lines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
            row["LineNo"] = Convert.ToInt32(lines.Tables[0].Rows[i]["jlline#"].ToString());
            row["Direct"] = lines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
            row["XLen"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlxlen"].ToString());
            row["YLen"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlylen"].ToString());
            row["Length"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jllinelen"].ToString());
            row["Angle"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlangle"].ToString());
            row["XPt1"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt1x"].ToString());
            row["YPt1"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt1y"].ToString());
            row["XPt2"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2x"].ToString());
            row["YPt2"] = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2Y"].ToString());
            row["Attach"] = lines.Tables[0].Rows[i]["jlattach"].ToString();

            decimal xPt2 = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2x"].ToString());
            decimal yPt2 = Convert.ToDecimal(lines.Tables[0].Rows[i]["jlpt2y"].ToString());

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

        private void AddListItemsToJumpTableList(float jx, float jy, float CurrentScale, DataSet lines)
        {
            try
            {
                for (int i = 0; i < lines.Tables[0].Rows.Count; i++)
                {
                    AddJumpTableRow(jx, jy, CurrentScale, lines, i);
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        private void AddMaster()
        {
            decimal summedArea = 0;
            decimal baseStory = 0;

            StringBuilder sumArea = new StringBuilder();
            sumArea.Append(String.Format("select sum(jssqft) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                       //SketchUpGlobals.FcLib,
                       //SketchUpGlobals.FcLocalityPrefix,
                       _currentParcel.Record,
                       _currentParcel.Card));

            try
            {
                summedArea = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(sumArea.ToString()));
            }
            catch
            {
            }

            StringBuilder getStory = new StringBuilder();
            getStory.Append(String.Format("select jsstory from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = 'A'  ",
                       SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        _currentParcel.Record,
                        _currentParcel.Card));

            try
            {
                baseStory = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(getStory.ToString()));
            }
            catch
            {
            }

            DataSet ds_master = UpdateMasterArea(summedArea);

            if (_deleteMaster == false)
            {
                InsertMasterRecord(summedArea, baseStory, ds_master);
            }
        }

        public void AddNewPoint()
        {
            SaveSketchData();
        }

        private void AddSections()
        {
            _addSection = true;

            string nextSectionLetter = SketchUpGlobals.ParcelWorkingCopy.NextSectionLetter;
            NewSectionPoints.Clear();
            lineCnt = 0;
            SectionTypes sktype = new SectionTypes(dbConn, _currentParcel, _addSection, lineCnt, IsNewSketch);

            sktype.ShowDialog(this);

            //Ensure they did not just cancel out by checking that there is a new version of the parcel
            if (SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter == nextSectionLetter)
            {
                NextSectLtr = SectionTypes._nextSectLtr;
                _nextSectType = SectionTypes._nextSectType;
                _nextStoryHeight = SectionTypes._nextSectStory;
                _nextLineCount = SectionTypes._nextLineCount;
                _hasNewSketch = (NextSectLtr == "A");

                AddSectionContextMenu.Enabled = true;
                jumpToolStripMenuItem.Enabled = true;

                try
                {
                    FieldText.Text = String.Format("Sect- {0}, {1} sty {2}", NextSectLtr.Trim(), _nextStoryHeight.ToString("N2"), _nextSectType.Trim());
                }
                catch
                {
                }
            }
        }

        private void AddSectionSQL(string dirct, float dist)
        {
            int secCnt = GetSectionsCount();

            if (NextSectLtr != String.Empty)
            {
                AddNewSection();
            }

            _currentSection = new SectionDataCollection(dbConn, _currentParcel.Record, _currentParcel.Card);

            LoadAttachmentPointsDataTable();

            LoadStartPointsDataTable();

            //TODO: Look at this stepping through
            SetNewSectionAttachmentPoint();

            TempAttSplineNo = _savedAttLine;

            decimal sptline = splitLineDist;

            if (splitLineDist > 0)
            {
                SplitLine();
            }
        }

        private int GetSectionsCount()
        {
            StringBuilder checkSect = new StringBuilder();
            checkSect.Append(String.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.Record,
                            _currentParcel.Card,
                            NextSectLtr));

            int secCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(checkSect.ToString()));
            return secCnt;
        }

        private void InsertMasterRecord(decimal summedArea, decimal baseStory, DataSet ds_master)
        {
            if (ds_master.Tables[0].Rows.Count == 0)
            {
                StringBuilder insMaster = new StringBuilder();
                insMaster.Append(String.Format("insert into {0}.{1}master (jmrecord,jmdwell,jmsketch,jmstory,jmstoryex,jmscale,jmtotsqft,jmesketch) ",
                              SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPreFix

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix
                                ));
                insMaster.Append(String.Format("values ({0},{1},'{2}',{3},'{4}',{5},{6},'{7}' ) ",
                            _currentParcel.Record,
                            _currentParcel.Card,
                            "Y",
                            baseStory,
                            String.Empty,
                            1.00,
                            summedArea,
                            String.Empty));

                dbConn.DBConnection.ExecuteNonSelectStatement(insMaster.ToString());
            }
        }

        private void LoadAttachmentPointsDataTable()
        {
            AttachmentPointsDataTable.Clear();
            List<SMLine> linesList = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where l.SectionLetter != "A" orderby l.SectionLetter, l.LineNumber select l).ToList();
            foreach (SMLine l in linesList)
            {
                DataRow row = AttachmentPointsDataTable.NewRow();
                row["Sect"] = l.SectionLetter;
                row["X1"] = l.StartX;
                row["Y1"] = l.StartY;
                row["X2"] = l.EndX;
                row["Y2"] = l.EndY;

                AttachmentPointsDataTable.Rows.Add(row);
            }
        }

        private void LoadStartPointsDataTable()
        {
            List<SMLine> startLines = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where l.LineNumber == 1 select l).OrderBy(s => s.SectionLetter).ToList();

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

        private void SetNewSectionAttachmentPoint()
        {
            //TODO: Intercept update here. Find where new lines are inserted
            string updateSQL = String.Format("update {0}.{1}line set JLATTACH = '{2}' where JLRECORD = {3} and JLDWELL = {4} and JLSECT = '{5}' and JLLINE# = {6}",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPreFix,
                NextSectLtr.Trim(),
                _currentParcel.Record,
                _currentParcel.Card,
                CurrentSecLtr,
                _savedAttLine);

            dbConn.DBConnection.ExecuteNonSelectStatement(updateSQL);
        }

        private DataSet UpdateMasterArea(decimal summedArea)
        {
            string checkMaster = string.Format("select * from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPreFix,
                _currentParcel.Record,
                _currentParcel.Card);

            DataSet ds_master = dbConn.DBConnection.RunSelectStatement(checkMaster.ToString());

            if (ds_master.Tables[0].Rows.Count > 0)
            {
                string updateMasterSql = string.Format("update {0}.{1}master set jmtotsqft = {2} where jmrecord = {3} and jmdwell = {4} ",
                               SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPreFix,
                               summedArea,
                               _currentParcel.Record,
                               _currentParcel.Card);

                dbConn.DBConnection.ExecuteNonSelectStatement(updateMasterSql.ToString());
            }

            return ds_master;
        }

        #endregion Refactored out from constructor

        #region User Actions Response Methods

        #region Key Press event handlers

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

        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            string legalDirectionName = string.Empty;
            switch (legalMoveDirection)
            {
                case "E":
                    legalDirectionName = "East";
                    break;

                case "S":
                    legalDirectionName = "South";
                    break;

                case "W":
                    legalDirectionName = "West";
                    break;

                case "N":
                    legalDirectionName = "North";
                    break;

                default:
                    legalDirectionName = "in a clockwise direction, relative to the anchoring section";
                    break;
            }

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.R || e.KeyCode == Keys.E)
            {
                _isKeyValid = IsValidDirection("E");
                if (_isKeyValid)
                {
                    HandleEastKeys();
                }
                else
                {
                    string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
                    MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    UndoJump = true;
                    RevertToPriorVersion();
                }
            }
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.L || e.KeyCode == Keys.W)
            {
                _isKeyValid = IsValidDirection("W");
                if (_isKeyValid)
                {
                    HandleWestKeys();
                }
                else
                {
                    string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
                    MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    UndoJump = true;
                    RevertToPriorVersion();
                }
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.U || e.KeyCode == Keys.N)
            {
                _isKeyValid = IsValidDirection("N");
                if (_isKeyValid)
                {
                    HandleNorthKeys();
                }
                else
                {
                    string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
                    MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    UndoJump = true;
                    RevertToPriorVersion();
                }
            }
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.D || e.KeyCode == Keys.S)
            {
                _isKeyValid = IsValidDirection("S");
                if (_isKeyValid)
                {
                    HandleSouthKeys();
                }
                else
                {
                    NotifyUserOfLegalMove(legalDirectionName);
                }
            }
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
                        UndoLine();
                        _isKeyValid = false;
                    }
                }

                #endregion Not Numberpad
            }
        }

        private void NotifyUserOfLegalMove(string legalDirectionName)
        {
            try
            {
                string message = string.Format("You may only move {0} from this jump point.", legalDirectionName);
                MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                UndoJump = true;
                RevertToPriorVersion();
                DistText.Text = String.Empty;
                distance = 0;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        #endregion Key Press event handlers

        private void AddSectionBtn_Click(object sender, EventArgs e)
        {
            AddSections();
            AddNewPoint();
            _deleteMaster = false;

            BeginSectionBtn.BackColor = Color.Orange;
            BeginSectionBtn.Text = "Begin";

            _isClosed = false;
        }

        private void addSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSections();
        }

        private void angleToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void beginPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw = true;

            DMouseClick();
        }

        private void BeginSectionBtn_Click(object sender, EventArgs e)
        {
            SetActiveButtonAppearance();
            float xxx = NextStartX;
            float yyy = NextStartY;

            float tsx = BeginSplitX;
            float tsy = BeginSplitY;

            float tstdist = distanceD;

            string tstdirect = LastDir;

            if (_addSection == false)
            {
                MessageBox.Show("Must select additon type ", "Missing Addition warning");
            }
            if (_addSection == true)
            {
                Xadj = (((ScaleBaseX - _mouseX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - _mouseY) / _currentScale) * -1);

                offsetDir = LastDir;

                //if (NextStartX != 0 || Xadj != 0)
                //{
                //    Xadj = NextStartX;
                //}

                if (Xadj != NextStartX)
                {
                    Xadj = NextStartX;
                }

                //if (NextStartY != 0 || Yadj != 0)
                //{
                //    NextStartY = Yadj;
                //}

                if (Yadj != NextStartY)
                {
                    Yadj = NextStartY;
                }

                if (offsetDir == "E")
                {
                    NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                    NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);
                }

                if (offsetDir == "W")
                {
                    NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                    NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);
                }

                if (offsetDir == "N")
                {
                    NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                    NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);
                }

                if (offsetDir == "S")
                {
                    NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                    NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);
                }
                if (offsetDir == String.Empty)
                {
                    NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                    NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);
                }

                if (_hasNewSketch == true)
                {
                    Xadj = 0;
                    Yadj = 0;
                }

                if (_hasNewSketch == true)
                {
                    AttSectLtr = "A";
                }
                if (AttSectLtr == String.Empty)
                {
                    AttSectLtr = JumpTable.Rows[0]["Sect"].ToString().Trim();
                }

                AttSpLineDir = offsetDir;

                splitLineDist = distance;

                startSplitX = NewSectionBeginPointX;
                startSplitY = NewSectionBeginPointY;

                getSplitLine();

                int _attLineNo = AttSpLineNo;

                draw = true;

                lineCnt = 0;
                DMouseClick();
            }
        }

        private void changeSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SketchSection sysect = new SketchSection(_currentParcel, dbConn, _currentSection);
            sysect.ShowDialog(this);

            ExpSketchPBox.Image = MainImage;
        }

        private void deleteExistingSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _deleteThisSketch = false;
            _deleteMaster = true;
            DialogResult result;
            result = (MessageBox.Show("Do you REALLY want to Delete this entire Sketch", "Delete Existing Sketch Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
            if (result == DialogResult.Yes)
            {
                DialogResult finalChk;
                finalChk = (MessageBox.Show("Are you Sure", "Final Delete Sketch Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

                if (finalChk == DialogResult.Yes)
                {
                    StringBuilder delSect = new StringBuilder();
                    delSect.Append(String.Format("delete from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
                              SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                _currentParcel.Record,
                                _currentParcel.Card));

                    dbConn.DBConnection.ExecuteNonSelectStatement(delSect.ToString());

                    StringBuilder delLine = new StringBuilder();
                    delLine.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                                   SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                                   //SketchUpGlobals.FcLib,
                                   //SketchUpGlobals.FcLocalityPrefix,
                                   _currentParcel.Record,
                                   _currentParcel.Card));

                    dbConn.DBConnection.ExecuteNonSelectStatement(delLine.ToString());

                    StringBuilder delmaster = new StringBuilder();
                    delmaster.Append(String.Format("delete from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
                              SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                               //SketchUpGlobals.FcLib,
                               //SketchUpGlobals.FcLocalityPrefix,
                               _currentParcel.Record,
                               _currentParcel.Card));

                    dbConn.DBConnection.ExecuteNonSelectStatement(delmaster.ToString());
                }
                if (finalChk == DialogResult.No)
                {
                }

                RefreshEditImageBtn = true;
                _deleteThisSketch = true;
                _isClosed = true;

                DialogResult makeVacant;
                makeVacant = (MessageBox.Show("Do you want to clear Master File", "Clear Master File Question",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question));

                if (makeVacant == DialogResult.Yes)
                {
                    StringBuilder clrMast2 = new StringBuilder();
                    clrMast2.Append(String.Format("update {0}.{1}mast set moccup = 15, mstory = ' ', mage = 0, mcond = ' ', mclass = ' ', ",
                               SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPreFix

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix
                                ));
                    clrMast2.Append(" mfactr = 0, mdeprc = 0, mfound = 0, mexwll = 0, mrooft = 0, mroofg = 0, m#dunt = 0, m#room = 0, m#br = 0, m#fbth = 0, m#hbth = 0 , mswl = 0, ");
                    clrMast2.Append(" mfp2 = ' ', mheat = 0, mfuel = 0, mac = ' ', mfp1 = ' ', mekit = 0, mbastp = 0, mpbtot = 0, msbtot = 0, mpbfin = 0, msbfin = 0, mbrate = 0, ");
                    clrMast2.Append(" m#flue = 0, mflutp = ' ', mgart = 0, mgar#c = 0, mcarpt = 0, mcar#c = 0, mbi#c = 0, mgart2 = 0, mgar#2 = 0, macpct = 0, m0depr = ' ',meffag = 0, ");
                    clrMast2.Append(" mfairv = 0, mexwl2 = 0, mtbv = 0, mtbas = 0, mtfbas = 0, mtplum = 0, mtheat = 0, mtac = 0, mtfp = 0, mtfl = 0 , mtbi = 0 , mttadd = 0 , mnbadj = 0, ");
                    clrMast2.Append(" mtsubt = 0, mtotbv = 0, mbasa = 0, mtota = 0, mpsf = 0, minwll = ' ', mfloor = ' ', myrblt = 0, mpcomp = 0, mfuncd = 0, mecond = 0, mimadj = 0, ");
                    clrMast2.Append(" mtbimp = 0, mcvexp = 'Improvement Deleted', mqapch = 0, mqafil = ' ', mfp# = 0, msfp# = 0, mfl#= 0, msfl# = 0, mmfl# = 0, miofp# = 0,mstor# = 0, ");
                    clrMast2.Append(String.Format(" moldoc = {0}, ", _currentParcel.orig_moccup));
                    clrMast2.Append(String.Format(" mcvmo = {0}, mcvda = {1}, mcvyr = {2} ",
                              SketchUpGlobals.Month,
                              SketchUpGlobals.TodayDayNumber,
                              SketchUpGlobals.Year

                                //MainForm.Month,
                                // MainForm.Today,
                                // MainForm.Year
                                ));
                    clrMast2.Append(String.Format(" where mrecno = {0} and mdwell = {1} ", _currentParcel.Record, _currentParcel.Card));

                    dbConn.DBConnection.ExecuteNonSelectStatement(clrMast2.ToString());

                    if (_currentParcel.GasLogFP > 0)
                    {
                        StringBuilder clrGasLg = new StringBuilder();
                        clrGasLg.Append(String.Format("update {0}.{1}gaslg set gnogas = 0 where grecno = {2} and gdwell = {3} ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            _currentParcel.Record,
                            _currentParcel.Card));

                        dbConn.DBConnection.ExecuteNonSelectStatement(clrGasLg.ToString());
                    }
                }
                if (makeVacant == DialogResult.No)
                {
                }
            }
            if (result == DialogResult.No)
            {
            }
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSection();

            RefreshSketch();
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
            _lenString = String.Empty;
            LastDir = String.Empty;

            // TODO: Remove if not needed:	int tclick = click;

            try
            {
                _StartX.Add(click, StartX);

                _StartY.Add(click, StartY);
            }
            catch
            {
            }

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
            if (jumpMode == false)
            {
                _isJumpMode = false;
                draw = true;
                Graphics g = Graphics.FromImage(MainImage);
                Pen pen1 = new Pen(Color.White, 4);
                g.DrawRectangle(pen1, X, Y, 1, 1);
                g.Save();

                ExpSketchPBox.Image = MainImage;

                //click++;
                //savpic.Add(click, imageToByteArray(_mainimage));
            }
            if (jumpMode == true)
            {
                _isJumpMode = true;
                draw = true;
                _mouseX = X;
                _mouseY = Y;
            }
        }

        public void DrawSketch(int selectedPoint)
        {
        }

        private void EastDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveEast(NextStartX, NextStartY);
            DistText.Focus();
        }

        private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClrX();
            AddMaster();
        }

        private void ExpSketchPbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isJumpMode)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                Graphics g = Graphics.FromImage(MainImage);
                Pen pen1 = new Pen(Color.Red, 4);
                g.DrawRectangle(pen1, e.X, e.Y, 1, 1);
                g.Save();

                DMouseClick();
            }
        }

        private void ExpSketchPbox_MouseDown(object sender, MouseEventArgs e)
        {
            _isJumpMode = false;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _isJumpMode = true;
                _mouseX = e.X;
                _mouseY = e.Y;
                DMouseMove(e.X, e.Y, true);
            }
        }

        private void ExpSketchPbox_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw && !_isJumpMode)
            {
                Graphics g = Graphics.FromImage(MainImage);
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillRectangle(brush, e.X, e.Y, StandardDrawWidthAndHeight, StandardDrawWidthAndHeight);
                g.Save();

                ExpSketchPBox.Image = MainImage;
            }
        }

        private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isJumpMode)
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

        private void NorthDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveNorth(NextStartX, NextStartY);
            DistText.Focus();
        }

        private void rotateSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //DialogResult result;
            //result = (MessageBox.Show("Do you want to Flip Sketch Left to Right", "Flip Sketch Warning",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            //if (result == DialogResult.Yes)
            //{
            //    FlipLeftRight();
            //}
            //if (result == DialogResult.No)
            //{
            //    DialogResult result2;
            //    result2 = (MessageBox.Show("Do you want to Flip Sketch Front to Back", "Flip Sketch Warning",
            //        MessageBoxButtons.YesNo, MessageBoxIcon.Question));
            //    if (result2 == DialogResult.Yes)
            //    {
            //        FlipUpDown();
            //    }

            //    if (result2 == DialogResult.No)
            //    {
            //    }
            //}

            FlipSketch fskt = new FlipSketch();
            fskt.ShowDialog();
            if (FlipSketch.FrontBack == true)
            {
                FlipUpDown();
            }
            if (FlipSketch.RightLeft == true)
            {
                FlipLeftRight();
            }
        }

        private void SouthDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveSouth(NextStartX, NextStartY);
            DistText.Focus();
        }

        private void TextBtn_Click(object sender, EventArgs e)
        {
            if (FieldText.Text.Trim() != String.Empty)
            {
                Graphics g = Graphics.FromImage(MainImage);
                SolidBrush brush = new SolidBrush(Color.Blue);
                Pen pen1 = new Pen(Color.Red, 2);
                Font f = new Font("Arial", 8, FontStyle.Bold);

                g.DrawString(FieldText.Text.Trim(), f, brush, new PointF(_mouseX + 5, _mouseY));

                FieldText.Text = String.Empty;
                FieldText.Focus();

                ExpSketchPBox.Image = MainImage;

                //click++;
                //savpic.Add(click, imageToByteArray(_mainimage));
            }
        }

        private void UnDoBtn_Click(object sender, EventArgs e)
        {
            RevertToPriorVersion();
        }

        private void viewSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SketchSection sksect = new SketchSection(_currentParcel, dbConn, _currentSection);
            sksect.ShowDialog(this);

            _reOpenSec = false;

            if (SketchUpGlobals.ReOpenSection != String.Empty)
            {
                _reOpenSec = true;

                ReOpenSec();
            }
        }

        private void WestDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveWest(NextStartX, NextStartY);
            DistText.Focus();
        }

        #endregion User Actions Response Methods
    }
}
