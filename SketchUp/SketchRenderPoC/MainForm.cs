using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchRenderPoC
{
	public partial class MainForm : Form
	{
		#region Constructor

		public MainForm()
		{
			InitializeComponent();
			graphics = pctMain.CreateGraphics();
			bluePen = new Pen(Color.DarkBlue, 3);

			redPen = new Pen(Color.Red, 2);
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

		private void tsb2DSketch_Click(object sender, EventArgs e)
		{
			//ShowSketch2D();
			CheckFonts();
		}

		private void tsbGetSketch_Click(object sender, EventArgs e)
		{
			ShowSketch();
		}

		#endregion control events

		#region Debug only

		private void DebugOnlyDisplayResults()
		{
#if DEBUG
			graphics.DrawRectangle(bluePen, SketchOrigin.X, SketchOrigin.Y, 2, 2);
			Brush redBrush = Brushes.Red;
			Font textFont = new Font("Arial", 12);
			graphics.DrawString(string.Format("Origin: {0},{1}", SketchOrigin.X, SketchOrigin.Y), textFont, redBrush, SketchOrigin.X + 10, SketchOrigin.Y + 10);
			graphics.DrawString(string.Format("Scale: {0}", ParcelWorkingCopy.Scale), textFont, redBrush, SketchOrigin.X + 10, SketchOrigin.Y + 20);
#endif
		}

		#endregion Debug only

		#region Fields

		private Brush blueBrush = Brushes.Blue;
		private Pen bluePen;
		private Graphics graphics;
		private GraphicsPath graphicsPath = new GraphicsPath();
		private SMParcel parcelWorkingCopy;
		private Pen redPen = new Pen(Color.Red, 1);
		private SMParcel selectedParcel;
		private PointF sketchOrigin;

		#endregion Fields

		#region Properties

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

		#region Private Methods

		private void CheckFonts()
		{
			string[] fontNames = {  "Arial",
				"Tahoma",
				"Microsoft Sans Serif",
				"Arial Unicode MS",
				"Lucida Sans Unicode",
				"MS Sans Serif",
				SystemFonts.DefaultFont.FontFamily.Name,
				SystemFonts.MessageBoxFont.FontFamily.Name };

			int yOffset = 10;
			string message;
			foreach (string f in fontNames)
				using (Font fnt = new Font(f, 12))
				{
					message = string.Format("{0} N: {1} E: {2} W: {3} S: {4} NE: {5} SE: {6} NW: {7} SW: {8}", fnt.FontFamily.ToString(), SMGlobal.NorthArrow, SMGlobal.EastArrow, SMGlobal.WestArrow, SMGlobal.SouthArrow, SMGlobal.NorthEastArrow, SMGlobal.SouthEastArrow, SMGlobal.NorthWestArrow, SMGlobal.SouthWestArrow);
					graphics.DrawString(message, fnt, SystemBrushes.ControlText, new PointF(10, yOffset));
					yOffset += 30;
					TextRenderer.DrawText(graphics, message, fnt, new Point(10, yOffset), SystemColors.ControlText);
					yOffset += 30;
				}
		}

		private void DrawLabel(SMLine line)
		{
			string label = line.LineLabel;
			Brush redBrush = Brushes.Red;
			Font font = new Font("Lucida Sans Unicode", 10, FontStyle.Bold, GraphicsUnit.Point);

			PointF labelStartPoint = line.LineLabelPlacementPoint(SketchOrigin);
			graphics.DrawString(label, font, redBrush, labelStartPoint);
		}

		private void DrawLine(SMLine line)
		{
			PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
			PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);

			graphics.DrawLine(bluePen, drawLineStart, drawLineEnd);
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
			if (ParcelWorkingCopy.Sections != null)
			{
				foreach (SMSection sms in ParcelWorkingCopy.Sections.OrderBy(l => l.SectionLetter))
				{
					if (sms.Lines != null)
					{
						foreach (SMLine l in sms.Lines.OrderBy(n => n.LineNumber))
						{
							DrawLine(l);
						}
					}
				}
			}
		}

		private SMParcel GetParcel(int dwelling, int record, SketchRepository sr)
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

		private void GetSelectedParcelData()
		{
			string dataSource = "192.168.176.241";
			int dwelling = 1;
			string locality = "AUG";
			string password = "CAMRA2";
			int record = 11787;
			string userName = "CAMRA2";
			SketchRepository sr = new SketchRepository(dataSource, userName, password, locality);
			SelectedParcel = GetParcel(dwelling, record, sr);
			ParcelWorkingCopy = SelectedParcel;
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

		private void SetSketchOrigin()
		{
			//Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
			var sketchAreaWidth = pctMain.Width - 20;
			var sketchAreaHeight = pctMain.Height - 20;

			PointF pictureBoxCorner = pctMain.Location;
			var paddingX = ((sketchAreaWidth - (ParcelWorkingCopy.SketchXSize * ParcelWorkingCopy.Scale)) / 2);
			var paddingY = ((sketchAreaHeight - (ParcelWorkingCopy.SketchYSize * ParcelWorkingCopy.Scale)) / 2);
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
			if (ParcelWorkingCopy != null && ParcelWorkingCopy.Sections != null)
			{
				foreach (SMSection s in ParcelWorkingCopy.Sections)
				{
					foreach (SMLine line in s.Lines)
					{
						var lineStartX = (float)((decimal)line.StartX * line.ParentParcel.Scale + (decimal)SketchOrigin.X);
						var lineStartY = (float)((decimal)line.StartY * line.ParentParcel.Scale + (decimal)SketchOrigin.Y);
						var lineEndX = (float)((decimal)lineStartX + ((decimal)line.XLength * line.ParentParcel.Scale));
						var lineEndY = (float)((decimal)lineStartY + ((decimal)line.YLength * line.ParentParcel.Scale));

						line.ScaledStartPoint = new PointF(lineStartX, lineStartY);
						line.ScaledEndPoint = new PointF(lineEndX, lineEndY);
					}
				}
			}
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

		private void ShowSketch()
		{
			GetSelectedParcelData();

			SetSketchScale();
			SetSketchOrigin();

			//DebugOnlyDisplayResults();
			DrawSections();
		}

		#endregion Private Methods
	}
}