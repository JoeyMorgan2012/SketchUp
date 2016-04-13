using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SketchUp;
using SWallTech;

namespace SketchRenderPoC
{
    public partial class EditSketchForm : Form
    {
        #region Constructor

        //TODO: Remove debugging code
        private const int recordNumber = 11787;

        //public EditSketchForm(SMParcel workingCopyOfParcel)
        //{
        //	ParcelWorkingCopy = workingCopyOfParcel;
        //	SelectedParcel = workingCopyOfParcel;
        //	InitializeComponent();
        //	graphics = pctMain.CreateGraphics();
        //	BluePen = new Pen(Color.DarkBlue, 3);
        //	RedPen = new Pen(Color.Red, 2);
        //	firstTimeLoaded = true;
        //	InitializeSketch();

        //}
        public EditSketchForm()

        {
            //TODO: Change this so the parcel data is loaded with the main form, index of 0, and each successive index is one greater, so each "version" is stored in the list.

            SelectedParcel = SketchUpGlobals.ParcelWorkingCopy;
            SelectedParcel.SnapShotIndex += 1;
            SketchUpGlobals.SketchSnapshots.Add(SelectedParcel);
            CamraSupport.Init(SketchUpGlobals.CamraDbConn);
            MockGettingCamraData();

            InitializeComponent();
          
            graphics = pctMain.CreateGraphics();
            BluePen = new Pen(Color.DarkBlue, 3);
            RedPen = new Pen(Color.Red, 2);
            firstTimeLoaded = true;
            InitializeSketch();
        }

        private void MockGettingCamraData()
        {
           

                ParcelDataCollection pdc = new ParcelDataCollection(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);
                SketchUpGlobals.CurrentParcel = pdc.GetParcel(SketchUpGlobals.CamraDbConn, SketchUpGlobals.Record, SketchUpGlobals.Card);
            
           

        }

        private void InitializeSketch()
        {
            if (FirstTimeLoaded)
            {
                SetSketchScale();
                SetSketchOrigin();
                SetScaledStartPoints();
                SetSectionCenterPoints();
            }
            DrawSections();
        }

        #endregion Constructor

        #region control events
     private void pctMain_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PointF mouseLocation = new PointF(e.X, e.Y);
            ShowNearestCorners(mouseLocation);
        }

        private void pctMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseLocationLabel.Text = string.Format("({0},{1})", e.X, e.Y);
        }

        #region toolStripMenuItems
        private void deleteSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSketch();
        }

        private void drawSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSketch();
        }
  private void addSectionTsMenu_Click(object sender, EventArgs e)
        {

            SectionAdditionDialog sad = new SectionAdditionDialog(SketchUpGlobals.CamraDbConn, SketchUpGlobals.CurrentParcel, true, 0, true);
            sad.ShowDialog();
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
        private void tsbGetSketch_Click(object sender, EventArgs e)
        {
            ShowSketch();
        }
        #endregion

   

      

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
                InitializeSketch();
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
                InitializeSketch();
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

        private void ShowPoint(string pointLabel, PointF sketchOriginPoint)
        {
            PointF[] region = new PointF[] { new PointF(sketchOriginPoint.X - 4, sketchOriginPoint.Y - 4), new PointF(sketchOriginPoint.X - 4, sketchOriginPoint.Y + 4), new PointF(sketchOriginPoint.X + 4, sketchOriginPoint.Y + 4), new PointF(sketchOriginPoint.X + 4, sketchOriginPoint.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            graphics.DrawPolygon(BluePen, region);
            graphics.DrawString(pointLabel, DefaultFont, GreenBrush, new PointF(sketchOriginPoint.X - 16, sketchOriginPoint.Y - 16));
        }

        public void ShowSketch()
        {
            DrawSections();
            graphics.Flush();
            pctMain.BringToFront();
        }

        #endregion Private Methods

        #region Properties

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
                bluePen = new Pen(Color.DarkBlue, 1);
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
                redPen = new Pen(Color.DarkRed, 1);
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
        private void editSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("{0}", "edit sections");
            MessageBox.Show(message);
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
    }
}