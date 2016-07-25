/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public class SMSketcher
    {
        #region "Constructor"

        public SMSketcher(SMParcel parcel, Graphics graphics)
        {
            InitializeVariables(parcel, graphics);
        }

        public SMSketcher(SMParcel parcel, PictureBox container)
        {
            SketchImage = new Bitmap(container.Width, container.Height);
            InitializeVariables(parcel, container);
        }

        public SMSketcher(SMParcel parcel, PictureBox container, Bitmap backingImage)
        {
            SketchImage = backingImage;
            InitializeVariables(parcel, container);
        }

        #endregion "Constructor"

        #region "Public Methods"

        public void DrawParcelLabel()
        {
            var Lblbrush = new SolidBrush(Color.Black);
            var FillBrush = new SolidBrush(Color.White);
            var whitePen = new Pen(Color.White, 2);
            var blackPen = new Pen(Color.Black, 2);

            var LbLf = new Font("Segue UI", 10, FontStyle.Bold);
            var TitleF = new Font("Segue UI", 10, FontStyle.Bold | FontStyle.Underline);
            var MainTitle = new System.Drawing.Font("Segue UI", 15, FontStyle.Bold | FontStyle.Underline);
            var leadzero = new char[] { '0' };

            GraphicsHandle.DrawString(SketchUpGlobals.LocalityPrefix, TitleF, Lblbrush, new PointF(10, 10));
            GraphicsHandle.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            GraphicsHandle.DrawString(string.Format("Record # - {0}", SketchUpGlobals.Record.ToString().TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            GraphicsHandle.DrawString(string.Format("Card # - {0}", SketchUpGlobals.Card), LbLf, Lblbrush, new PointF(10, 45));

            GraphicsHandle.DrawString(string.Format("Scale - {0}", LocalParcelCopy.Scale), LbLf, Lblbrush, new PointF(10, 70));
        }

        public void DrawRectangleAtPoint(Pen pen, float x, float y, int width, int height)
        {
            GraphicsHandle.DrawRectangle(pen, x, y, width, height);
        }

        public void RenderSketch()
        {
            try
            {
                LocalParcelCopy.SetScaleAndOriginForParcel(ImageContainer);
                DrawSections();
                DrawScreenOnlyLines();
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());

#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        public void RenderSketch(string redSectionLetter = "")
        {
            try
            {
                LocalParcelCopy.SetScaleAndOriginForParcel(ImageContainer);
                DrawSections(redSectionLetter);
                DrawScreenOnlyLines();
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());

#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        public void RenderSketchSection(string sectionLetter, bool showMeasurements = true)
        {
            try
            {
                LocalParcelCopy.SetScaleAndOriginForParcel(ImageContainer);
                DrawSection(sectionLetter);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#if DEBUG
                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        public void RenderSketchSection(string sectionLetter, Pen pen, bool showMeasurements = true)
        {
            try
            {
                LocalParcelCopy.SetScaleAndOriginForParcel(ImageContainer);
                DrawSection(sectionLetter, pen);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());

#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        private void RemoveLastLineAdded(SMParcel parcel, PictureBox targetContainer, string sectionLetter)
        {
            RenderSketch();
            string message = string.Empty;
            targetContainer.Image = SketchImage;
            SMSection workingSection = parcel.SelectSectionByLetter(sectionLetter);
            if (workingSection != null && workingSection.Lines != null && workingSection.Lines.Count > 0)
            {
                int lastLineNumber = (from l in workingSection.Lines select l.LineNumber).Max();

                SMLine lastLine = workingSection.Lines.Where(l => l.LineNumber == lastLineNumber).FirstOrDefault();
                message = string.Format("I will now Undo line {0}-{1}...", workingSection.SectionLetter, lastLine.LineNumber);
                MessageBox.Show(message);
                workingSection.Lines.Remove(lastLine);

                RenderSketch(sectionLetter);
                targetContainer.Image = SketchImage;
            }
            else
            {
                message = string.Format("There are no more lines pending for section {0}. Do you want to remove this section?", sectionLetter);

                DialogResult response = MessageBox.Show(message, "Remove Section Entirely?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.Yes)
                {
                    parcel.SnapshotIndex++;
                    SketchUpGlobals.SketchSnapshots.Add(parcel);
                    parcel.Sections.Remove(workingSection);
                    RenderSketch();
                }
            }
        }

        #endregion "Public Methods"

        #region "Private methods"

        private void AdjustLine(SMParcel parcel, string sectionLetter, int lineNumber, double newStartX, double newStartY, double newEndX, double newEndY)
        {
            SMLine selectedLine = (from l in parcel.AllSectionLines where l.SectionLetter == sectionLetter && l.LineNumber == lineNumber select l).FirstOrDefault();
            if (selectedLine != null)
            {
                selectedLine.StartX = newStartX;
                selectedLine.StartY = newStartY;
                selectedLine.EndX = newEndX;
                selectedLine.EndY = newEndY;
                SetScaledStartPoint(selectedLine);
            }
        }

        private void DrawLabel(DrawOnlyLine line)
        {
            string label = line.LineLabel;

            var font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint();

            GraphicsHandle.DrawString(label, font, BlackBrush, labelStartPoint);
        }

        private void DrawLabel(SMLine line)
        {
            string label = line.LineLabel;

            var font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);

            GraphicsHandle.DrawString(label, font, BlackBrush, labelStartPoint);
        }

        private void DrawLabel(SMLine line, bool showEndpoints)
        {
            string label = line.LineLabel;

            var font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);

            GraphicsHandle.DrawString(label, font, BlackBrush, labelStartPoint);
            if (showEndpoints)
            {
                ShowPoint(string.Format("{0}{1}\nbeg\n{2:N1},{3:N1}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY), line.ScaledStartPoint);
                ShowPoint(string.Format("{0}{1}\nend\n{2:N1},{3:N1}", line.SectionLetter, line.LineNumber, line.EndX, line.EndY), line.EndPoint);
            }
        }

        private void DrawLine(SMLine line)
        {
            var drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            var drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);

            GraphicsHandle.DrawLine(SketchPen, line.ScaledStartPoint, line.ScaledEndPoint);

            DrawLabel(line);
        }

        private void DrawLine(SMLine line, bool omitLabel = true)
        {
            GraphicsHandle.DrawLine(SketchPen, line.ScaledStartPoint, line.ScaledEndPoint);
            DrawLabel(line);
            if (!omitLabel)
            {
                DrawLabel(line);
            }
        }

        private void DrawLine(SMLine line, Pen pen)
        {
            var drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            var drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            GraphicsHandle.DrawLine(pen, line.ScaledStartPoint, line.ScaledEndPoint);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, Pen pen, bool omitLabel = true)
        {
            var drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            var drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            GraphicsHandle.DrawLine(pen, line.ScaledStartPoint, line.ScaledEndPoint);

            if (!omitLabel)
            {
                DrawLabel(line, omitLabel);
            }
        }

        private void DrawScreenOnlyLine(DrawOnlyLine line, Pen pen)
        {
            var drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            var drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);

            GraphicsHandle.DrawLine(pen, line.ScaledStartPoint, line.ScaledEndPoint);
            DrawRectangleAtPoint(pen, line.ScaledStartPoint.X, line.ScaledStartPoint.Y, 1, 1);
            DrawRectangleAtPoint(pen, line.ScaledEndPoint.X, line.ScaledEndPoint.Y, 1, 1);
            DrawLabel(line);
        }

        private void DrawScreenOnlyLines()
        {
            if (LocalParcelCopy.DrawOnlyLines != null && LocalParcelCopy.DrawOnlyLines.Count > 0)
            {
                foreach (DrawOnlyLine l in LocalParcelCopy.DrawOnlyLines.OrderBy(n => n.LineNumber))
                {
                    DrawScreenOnlyLine(l, new Pen(Color.Cyan, 3));
                }
            }
        }

        private void DrawSection(string sectionLetter)
        {
            if (LocalParcelCopy.Sections != null)
            {
                SMSection selectedSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();

                if (selectedSection.Lines != null)
                {
                    foreach (SMLine l in selectedSection.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, new Pen(Color.Blue), false);
                    }
                }
            }
        }

        private void DrawSection(string sectionLetter, Pen pen)
        {
            if (LocalParcelCopy.Sections != null)
            {
                SMSection selectedSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();

                if (selectedSection.Lines != null)
                {
                    foreach (SMLine l in selectedSection.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, pen, false);
                    }
                }
            }
        }

        private void DrawSectionLabel(SMSection section)
        {
            FormattableString label = section.SectionLabel;
            int fontSize = SketchImage.Height > 300 ? 9 : 7;
            var font = new Font("Segoe UI", fontSize, FontStyle.Regular, GraphicsUnit.Point);
            var labelLength = (int)section.SectionLabel.ToString().Length;

            PointF labelLocation = section.ScaledSectionCenter;

            GraphicsHandle.DrawString(label.ToString(), font, RedBrush, labelLocation);
        }

        private void DrawSections(string redSectionLetter = "")
        {
            var pen = new Pen(Color.Blue, 2);
            if (LocalParcelCopy != null && LocalParcelCopy.Sections != null)
            {
                foreach (SMSection section in LocalParcelCopy.Sections.OrderBy(l => l.SectionLetter).Distinct())
                {
                    if (section.SectionLetter == redSectionLetter || !section.SectionIsClosed)
                    {
                        pen = new Pen(Color.Red, 2);
                    }
                    else
                    {
                        pen = new Pen(Color.DarkBlue, 2);
                    }
                    if (section.Lines != null)
                    {
                        foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber).Distinct())
                        {
                            DrawLine(l, pen, false);
                        }
                    }

                    DrawSectionLabel(section);
                }
            }
        }

        private void InitializeVariables(SMParcel parcel, Graphics graphics)
        {
            LocalParcelCopy = parcel;
            var width = (int)graphics.ClipBounds.Width;
            var height = (int)graphics.ClipBounds.Height;
            SketchImage = new Bitmap(width, height);

            GraphicsHandle = graphics;
            GraphicsHandle.Clear(Color.White);
            GraphicsHandle.PageUnit = GraphicsUnit.Display;
        }

        private void InitializeVariables(SMParcel parcel, PictureBox container)
        {
            LocalParcelCopy = parcel;
            SketchImage = new Bitmap(container.Width, container.Height);
            imageContainer = container;

            GraphicsHandle = Graphics.FromImage(SketchImage);
            GraphicsHandle.Clear(Color.White);
            GraphicsHandle.PageUnit = GraphicsUnit.Display;
        }

        private void InitializeVariables(SMParcel parcel, PictureBox container, Bitmap backingImage)
        {
            LocalParcelCopy = parcel;
            SketchImage = backingImage;
            imageContainer = container;

            GraphicsHandle = Graphics.FromImage(SketchImage);
            GraphicsHandle.Clear(Color.White);
            GraphicsHandle.PageUnit = GraphicsUnit.Display;
        }

        private void SetScaledStartPoint(SMLine line)
        {
            try
            {
                line.ScaledStartPoint = SMGlobal.DbPointToScaledPoint(line.StartX, line.StartY, line.ParentParcel.Scale, SketchOrigin);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());

#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        private void SetScaledStartPoint(DrawOnlyLine line)
        {
            try
            {
                line.ScaledStartPoint = SMGlobal.DbPointToScaledPoint(line.StartX, line.StartY, line.ParentParcel.Scale, SketchOrigin);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
                Debug.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        private void SetScaledStartPoints()
        {
            if (LocalParcelCopy != null && LocalParcelCopy.Sections != null)
            {
                double sketchScale = LocalParcelCopy.Scale;
                foreach (SMSection s in LocalParcelCopy.Sections)
                {
                    foreach (SMLine line in s.Lines)
                    {
                        PointF p = SMGlobal.DbPointToScaledPoint(line.StartX, line.StartY, line.ParentParcel.Scale, SketchOrigin);

                        line.ScaledStartPoint = p;
                    }
                    foreach (DrawOnlyLine dol in LocalParcelCopy.DrawOnlyLines)
                    {
                        PointF p = SMGlobal.DbPointToScaledPoint(dol.StartX, dol.StartY, dol.ParentParcel.Scale, SketchOrigin);

                        dol.ScaledStartPoint = p;
                    }
                }
            }
        }

        // private void SetSectionCenterPoints(SMParcel parcel) { List<PointF> sectionPoints = new
        // List<PointF>(); foreach (SMSection section in parcel.Sections) { sectionPoints = new
        // List<PointF>(); foreach (SMLine line in section.Lines) {
        // sectionPoints.Add(line.ScaledStartPoint); sectionPoints.Add(line.ScaledEndPoint); }
        // PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
        // section.ScaledSectionCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(0,
        // -12)); } }

        //        private void SetSectionCenterPoints()
        //        {
        //            try
        //            {
        //                List<PointF> sectionPoints = new List<PointF>();
        //                foreach (SMSection section in LocalParcelCopy.Sections)
        //                {
        //                    sectionPoints = new List<PointF>();
        //                    foreach (SMLine line in section.Lines)
        //                    {
        //                        sectionPoints.Add(line.ScaledStartPoint);
        //                        sectionPoints.Add(line.ScaledEndPoint);
        //                    }
        //                    PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
        //                    section.ScaledSectionCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(0, -12));
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                string errMessage=$"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
        //                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
        //                Debug.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
        //#if DEBUG

        //                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
        //#endif
        //                throw;
        //            }
        //        }

        private void SetSketchOrigin()
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            try
            {
                int sketchAreaWidth = ImageContainer.Width - sketchBoxPaddingTotal;
                int sketchAreaHeight = ImageContainer.Height - sketchBoxPaddingTotal;

                PointF pictureBoxCorner = ImageContainer.Location;
                var extraWidth = (ImageContainer.Width - sketchBoxPaddingTotal) - (LocalParcelCopy.Scale * LocalParcelCopy.SketchXSize);
                var extraHeight = (ImageContainer.Height - sketchBoxPaddingTotal) - (LocalParcelCopy.Scale * LocalParcelCopy.SketchYSize);
                var paddingX = (extraWidth / 2) + 10;
                var paddingY = (extraHeight / 2) + 10;
                var xLocation = (LocalParcelCopy.OffsetX * LocalParcelCopy.Scale) + paddingX;
                var yLocation = (1.00 * LocalParcelCopy.OffsetY * LocalParcelCopy.Scale) + paddingY;

                SketchOrigin = new PointF((float)xLocation, (float)yLocation);
                LocalParcelCopy.SketchOrigin = SketchOrigin;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());

#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        private void SetSketchScale()
        {
            try
            {
                //Determine the size of the sketch drawing area, which is the picture box less 30 px on a side, so height-sketchBoxPadding and width-sketchBoxPaddingTotal. Padding is sketchBoxPaddingTotal/2.
                int boxHeight = ImageContainer.Height - sketchBoxPaddingTotal;
                int boxWidth = ImageContainer.Width - sketchBoxPaddingTotal;
                double xScale = Math.Round(Math.Floor(boxWidth / LocalParcelCopy.SketchXSize), 1);
                double yScale = Math.Round(Math.Floor(boxHeight / LocalParcelCopy.SketchYSize), 1);

                // Allow 15% for the titles and to draw.
                LocalParcelCopy.Scale = Math.Round(0.85 * SMGlobal.SmallerDouble(xScale, yScale), 1);
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Console.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
                Debug.WriteLine($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#if DEBUG

                MessageBox.Show($"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString());
#endif
                throw;
            }
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel)
        {
            var region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            var pointPolygon = new PolygonF(region);

            GraphicsHandle.DrawPolygon(SketchPen, region);
            GraphicsHandle.DrawString(pointLabel, sketchFont, GreenBrush, new PointF(pointToLabel.X - 16, pointToLabel.Y - 16));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset)
        {
            var region = new PointF[] { new PointF(pointToLabel.X, pointToLabel.Y - 14), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            var pointPolygon = new PolygonF(region);

            GraphicsHandle.DrawPolygon(SketchPen, region);
            GraphicsHandle.DrawString(pointLabel, sketchFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset, Pen pen)
        {
            var region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            var pointPolygon = new PolygonF(region);

            GraphicsHandle.DrawPolygon(pen, region);

            GraphicsHandle.DrawString(pointLabel, sketchFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        #endregion "Private methods"

        #region "Properties"

        public static Graphics GraphicsHandle
        {
            get { return graphicsHandle; }

            set { graphicsHandle = value; }
        }

        public Brush BlackBrush
        {
            get {
                blackBrush = Brushes.Black;
                return blackBrush;
            }

            set {
                blackBrush = value;
            }
        }

        public Brush GreenBrush
        {
            get {
                greenBrush = Brushes.DarkGreen;
                return greenBrush;
            }

            set {
                greenBrush = value;
            }
        }

        public PictureBox ImageContainer
        {
            get { return imageContainer; }

            set { imageContainer = value; }
        }

        public SMParcel LocalParcelCopy { get; set; }

        public Color PenColor
        {
            get {
                if (penColor == null)
                {
                    penColor = Color.Blue;
                }
                return penColor;
            }

            set {
                penColor = value;
            }
        }

        public Brush RedBrush
        {
            get {
                redBrush = Brushes.Red;
                return redBrush;
            }

            set {
                redBrush = value;
            }
        }

        public Font SketchFont
        {
            get {
                if (sketchFont == null)
                {
                    sketchFont = new Font("Segoe UI", 5);
                }
                return sketchFont;
            }

            set {
                sketchFont = value;
            }
        }

        public Image SketchImage
        {
            get { return sketchImage; }

            set { sketchImage = value; }
        }

        public PointF SketchOrigin
        {
            get { return sketchOrigin; }

            set { sketchOrigin = value; }
        }

        public Pen SketchPen
        {
            get {
                if (sketchPen == null)
                {
                    sketchPen = new Pen(PenColor, 2);
                }
                return sketchPen;
            }

            set {
                sketchPen = value;
            }
        }

        #endregion "Properties"

        #region Fields

        private const int sketchBoxPaddingTotal = 20;
        private static Graphics graphicsHandle;
        private Brush blackBrush;
        private Brush greenBrush;
        private PictureBox imageContainer;
        private Color penColor;
        private Brush redBrush;
        private Font sketchFont;
        private Image sketchImage;
        private PointF sketchOrigin;
        private Pen sketchPen;

        #endregion Fields
    }
}