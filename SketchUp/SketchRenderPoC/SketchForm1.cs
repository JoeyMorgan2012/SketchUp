using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SWallTech;
using System.Linq;
using System.Collections.Generic;

namespace SketchRenderPoC
{
	public partial class SketchForm1 : Form
	{
		private string dataSource = "192.168.176.241";
		private string locality = "AUG";
		private string password = "CAMRA2";
		private string userName = "CAMRA2";
		private SketchMgrParcel selectedParcel;

		public SketchMgrParcel SelectedParcel
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

		public SketchForm1()
		{
			InitializeComponent();
		}

		private void tsbGetSketch_Click(object sender, EventArgs e)
		{
			//TODO: Refactor for DI
			selectedParcel = GetSelectedParcelObjects();
			Pen redPen = new Pen(new SolidBrush(Color.Red), 2);
			Graphics g = pctMainSketch.CreateGraphics();
			PointF boxCorner = new PointF(pctMainSketch.Left, pctMainSketch.Top);
			List<float> xList = new List<float>();
			List<float> yList = new List<float>();
			xList.AddRange((from l in selectedParcel.AllSectionLines select l.StartX).ToList());
			xList.AddRange((from l in selectedParcel.AllSectionLines select l.EndX).ToList());
			yList.AddRange((from l in selectedParcel.AllSectionLines select l.StartY).ToList());
			yList.AddRange((from l in selectedParcel.AllSectionLines select l.EndY).ToList());
			float minX = xList.Min();
			float maxX = xList.Max();
			float minY = yList.Min();
			float maxY = yList.Max();
			float xLength = Math.Abs(maxX - minX);
			float yLength = Math.Abs(maxY - minY);
			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("Corners are ({0},{1}),({2},{3}),({4},{5}),({6},{7})", minX, minY, maxX, minY,maxX,maxY,minX,maxY));
			sb.AppendLine(string.Format("Height is {0} and width is {1}.", yLength, xLength));
			sb.AppendLine(string.Format("sketchBox starts at {0},{1}", boxCorner.X, boxCorner.Y));
				MessageBox.Show(sb.ToString());
			foreach (SketchMgrSection sms in selectedParcel.Sections)
			{
				foreach (SketchMgrLine sml in sms.Lines)
				{
					PointF start = PointF.Add(sml.StartPoint, new Size((int)boxCorner.X,(int)boxCorner.Y));
					PointF end = PointF.Add(sml.EndPoint, new Size((int)boxCorner.X, (int)boxCorner.Y));
					g.DrawLine(redPen, start, end);
				}
			}
			//Point point1 = new Point(10, 10);
			//Point point2 = Point.Add(point1, new Size(250, 250));

			//g.DrawLine(Pens.Red, point1, point2);
		}

		private SketchMgrParcel GetSelectedParcelObjects()
		{
			SketchRepository sr = new SketchRepository(dataSource, userName, password, locality);
			SketchMgrParcel parcel = sr.SelectParcelData(11787, 1);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SketchMgrSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.RefreshParcel = false;
			return parcel;
		}

	

	
	}
}