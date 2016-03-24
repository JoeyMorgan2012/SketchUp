using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchRenderPoC
{
	public partial class SketchForm1 : Form
	{
		public SketchForm1()
		{
			InitializeComponent();
		}

		private void DrawSection(Pen redPen, Graphics g, SMSection sms)
		{
			string message = string.Empty;
			foreach (SMLine sml in (sms.Lines).OrderBy(l => l.LineNumber))
			{
				//g.DrawLine(redPen, new PointF(sml.StartX,sml.StartY),new PointF (sml.EndX,sml.EndY));
				message = string.Format("{0}-{1}: ({2},{3}) - ({4},{5}). {6} x {7}", sml.SectionLetter, sml.LineNumber, sml.StartX, sml.StartY, sml.EndX, sml.EndY, sml.XLength, sml.YLength);

				Console.WriteLine(message);
				g.Clip.GetBounds(g);
				g.DrawLine(redPen, sml.StartPoint, sml.EndPoint);
			}
		}

		private SMParcel GetSelectedParcelObjects()
		{
			SketchRepository sr = new SketchRepository(dataSource, userName, password, locality);
			SMParcel parcel = sr.SelectParcelData(11787, 1);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.RefreshParcel = false;
			return parcel;
		}

		private void SetDrawingScale(SMParcel parcel)
		{
			double smallestDimension = SMGlobal.SmallerDouble(pctMainSketch.Width, pctMainSketch.Height);
			double smallestSketchDimensionDrawingArea = smallestDimension - 20;
			parcel.Scale = (decimal)SMGlobal.LargerDouble(parcel.SketchXSize, parcel.SketchYsize) == (decimal)parcel.SketchXSize ? (decimal)(pctMainSketch.Width - 20 / parcel.SketchXSize) : (decimal)(pctMainSketch.Height - 20 / parcel.SketchYsize);

			SetParcelOffsets(parcel);
		}

		private void SetParcelOffsets(SMParcel parcel)
		{
			parcel.OffsetX = pctMainSketch.Left + 10;

			parcel.OffsetY = pctMainSketch.Top + 10;
		}

		private void tsbGetSketch_Click(object sender, EventArgs e)
		{
			//TODO: Refactor for DI
			SelectedParcel = GetSelectedParcelObjects();
			Pen redPen = new Pen(new SolidBrush(Color.Red), 2);

			Graphics g = pctMainSketch.CreateGraphics();

			PointF boxCorner = new PointF(pctMainSketch.Left, pctMainSketch.Top);
			SetDrawingScale(SelectedParcel);
			SetParcelOffsets(SelectedParcel);
			foreach (SMSection sms in selectedParcel.Sections)
			{
				DrawSection(redPen, g, sms);
			}

			//g.ClipBounds.Offset(pctMainSketch.Location);
			g.TranslateTransform(pctMainSketch.Left + 10, pctMainSketch.Top - 10);

			//Point point1 = new Point(10, 10);
			//Point point2 = Point.Add(point1, new Size(250, 250));

			//g.DrawLine(Pens.Red, point1, point2);
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

		private string dataSource = "192.168.176.241";
		private string locality = "AUG";
		private string password = "CAMRA2";
		private SMParcel selectedParcel;
		private string userName = "CAMRA2";
	}
}