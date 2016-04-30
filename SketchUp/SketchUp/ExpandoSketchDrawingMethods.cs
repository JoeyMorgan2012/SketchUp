using System;
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
        #region SketchManagerDrawingMethods

        private void DrawLabel(SMSection section)
        {
            string label = section.SectionLabel;

            Font font = new Font("Segoe UI", 10, FontStyle.Bold, GraphicsUnit.Point);
            int labelLength = (int)section.SectionLabel.Length;

            PointF labelLocation = section.ScaledSectionCenter;

            g.DrawString(label, font, RedBrush, labelLocation);
        }

        private void DrawLabel(SMLine line, bool showEndpoints)
        {
            string label = line.LineLabel;

            Font font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);
            g.DrawString(label, font, BlackBrush, labelStartPoint);
            if (showEndpoints)
            {
                ShowPoint(string.Format("{0}{1}\nbeg\n{2:N1},{3:N1}", line.SectionLetter, line.LineNumber, line.StartX, line.StartY), line.ScaledStartPoint);
                ShowPoint(string.Format("{0}{1}\nend\n{2:N1},{3:N1}", line.SectionLetter, line.LineNumber, line.EndX, line.EndY), line.EndPoint);
            }
        }

        private void DrawLabel(SMLine line)
        {
            string label = line.LineLabel;

            Font font = new Font("Segoe UI", 8, FontStyle.Regular, GraphicsUnit.Point);

            PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);
            g.DrawString(label, font, BlackBrush, labelStartPoint);
        }

        private void DrawLine(SMLine line)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            g.DrawLine(BluePen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, bool omitLabel = true)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            g.DrawLine(BluePen, drawLineStart, drawLineEnd);
            if (!omitLabel)
            {
                DrawLabel(line);
            }
        }

        private void DrawLine(SMLine line, Pen pen)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            g.DrawLine(pen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, Pen pen, bool omitLabel = true)
        {
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

            g.DrawLine(pen, drawLineStart, drawLineEnd);
            if (!omitLabel)
            {
                DrawLabel(line, omitLabel);
            }
        }

        private void DrawParcelLabel()
        {
            Graphics g = Graphics.FromImage(MainImage);
            SolidBrush Lblbrush = new SolidBrush(Color.Black);
            SolidBrush FillBrush = new SolidBrush(Color.White);
            Pen whitePen = new Pen(Color.White, 2);
            Pen blackPen = new Pen(Color.Black, 2);

            Font LbLf = new Font("Segue UI", 10, FontStyle.Bold);
            Font TitleF = new Font("Segue UI", 10, FontStyle.Bold | FontStyle.Underline);
            Font MainTitle = new System.Drawing.Font("Segue UI", 15, FontStyle.Bold | FontStyle.Underline);
            char[] leadzero = new char[] { '0' };

            g.DrawString(SketchUpGlobals.LocalityPreFix, TitleF, Lblbrush, new PointF(10, 10));
            g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            g.DrawString(String.Format("Record # - {0}", SketchUpGlobals.Record.ToString().TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            g.DrawString(String.Format("Card # - {0}", SketchUpGlobals.Card), LbLf, Lblbrush, new PointF(10, 45));

            g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));
        }

        private void DrawSections(bool ShowPoints = false)
        {
            if (SketchUpGlobals.ParcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (SketchUpGlobals.ParcelWorkingCopy.Sections != null)
            {
                foreach (SMSection section in SketchUpGlobals.ParcelWorkingCopy.Sections.OrderBy(l => l.SectionLetter))
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

        private void DrawSection(string sectionLetter)
        {
            if (SketchUpGlobals.ParcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (SketchUpGlobals.ParcelWorkingCopy.Sections != null)
            {
                SMSection selectedSection = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();

                if (selectedSection.Lines != null)
                {
                    foreach (SMLine l in selectedSection.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, false);
                    }
                }
            }
        }
        private void DrawSection(SMSection section)
        {
            if (SketchUpGlobals.ParcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (SketchUpGlobals.ParcelWorkingCopy.Sections != null)
            {
               

                if (section.Lines != null)
                {
                    foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, false);
                    }
                }
            }
        }
        private void DrawSection(SMSection section,Pen pen,bool omitLabel=false)
        {
            if (SketchUpGlobals.ParcelWorkingCopy == null)
            {
                RenderSketch();
            }
            if (SketchUpGlobals.ParcelWorkingCopy.Sections != null)
            {


                if (section.Lines != null)
                {
                    foreach (SMLine l in section.Lines.OrderBy(n => n.LineNumber))
                    {
                        DrawLine(l, pen,omitLabel);
                    }
                }
            }
        }
        private Bitmap RenderSketch()
        {
            try
            {
                Bitmap sketcher = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                SetSketchScale();
                SetSketchOrigin();
                SetScaledStartPoints();
                SetSectionCenterPoints();
                sketcher = ShowSketchFromBitMap();
                return sketcher;
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

        private void ShowPoint(string pointLabel, PointF pointToLabel)
        {
            Graphics g = ExpSketchPBox.CreateGraphics();
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            g.DrawPolygon(BluePen, region);
            g.DrawString(pointLabel, DefaultFont, GreenBrush, new PointF(pointToLabel.X - 16, pointToLabel.Y - 16));
            g.Save();
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X, pointToLabel.Y - 14), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            g.DrawPolygon(BluePen, region);
            g.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        private void ShowPoint(string pointLabel, PointF pointToLabel, SizeF labelOffset, Pen pen)
        {
            PointF[] region = new PointF[] { new PointF(pointToLabel.X - 4, pointToLabel.Y - 4), new PointF(pointToLabel.X - 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y + 4), new PointF(pointToLabel.X + 4, pointToLabel.Y - 4) };
            PolygonF pointPolygon = new PolygonF(region);

            g.DrawPolygon(pen, region);
            g.DrawString(pointLabel, DefaultFont, GreenBrush, PointF.Add(new PointF(pointToLabel.X, pointToLabel.Y + 16), labelOffset));
        }

        private Bitmap ShowSketchFromBitMap()
        {
            try
            {
                Bitmap bmpWorking = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);

                g = Graphics.FromImage(bmpWorking);

                g.Clear(Color.White);
                DrawSections();

                // DrawSectionsOntoBitMap(graphics, true);
                //graphics.Flush();
                return bmpWorking;
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

        #endregion SketchManagerDrawingMethods
    }
}