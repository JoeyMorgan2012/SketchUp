using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public class BuildingSketcher
	{
		#region Constructors

		//Refactored stringbuilders to strings and extracted long code runs into separate methods. JMM Feb 2016
		public BuildingSketcher(BuildingSectionCollection sections)
		{
			Sections = sections;
		}

		private BuildingSketcher()
		{

		}

		#endregion Constructors

		public enum SupportedTypes
		{
			BMP,
			GIF,
			JPG,
			PNG,
			TIF,
			WMF
		}

		public static float basePtX;

		public static float basePtY;

		public BuildingSectionCollection Sections;

		private const int SizeInPixelsDefault = 400;

		private BuildingSectionCollection _sections;

		private int _sizeInPixels = 400;

		private Bitmap _sketch;

		private SupportedTypes _type = SupportedTypes.JPG;

		#region Property values

		public int Card
		{
			get
			{
				return Sections.Card;
			}
		}

		public int Record
		{
			get
			{
				return Sections.Record;
			}
		}

		public int SizeInPixels
		{
			get
			{
				return _sizeInPixels;
			}
		}

		public SupportedTypes Type
		{
			get
			{
				return _type;
			}

			set
			{
				_type = value;
			}
		}

		#endregion Property values

		public Bitmap DrawSketch()
		{

			try
			{
				return DrawSketch(SizeInPixelsDefault);
			}
			catch (Exception ex)
			{
#if DEBUG

				MessageBox.Show(ex.Message);
#endif
				Logger.Error(ex, string.Format("{0}.{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().Name));
				throw;
			}
		}

		public Bitmap DrawSketch(int sizeInPixels)
		{
		
			float scale = 1.0f;

			return DrawSketch(sizeInPixels, sizeInPixels, sizeInPixels, sizeInPixels, sizeInPixels, out scale);
		}

		public Bitmap DrawSketch(int bitmapWidth, int bitmapHeight, int sizeXinPixels, int sizeYinPixels, int sizeInPixels, out float scaleOut)
		{

			scaleOut = 1.0f;
			if (Sections == null || Sections.Count == 0)
			{
				return null;
			}
			else
			{
				#region Setup

				Pen pen = new Pen(Color.Black, 1.0f);
				Pen base_pen = new Pen(Color.Black, 2.0f);
				Brush brush = new SolidBrush(Color.Black);

				float em = EmSizeSetting(sizeInPixels);
				Font font = new Font(FontFamily.GenericSansSerif, em);

				List<string> noLabelTypes = NoLabelTypesList();

				SortedList<string, SectionInfo> sections = new SortedList<string, SectionInfo>();

				decimal MINX = 0;
				decimal MAXX = 0;
				decimal MINY = 0;
				decimal MAXY = 0;

				#endregion Setup

				#region Loop through sections

				foreach (BuildingSection section in Sections)
				{
					SectionInfo sectInfo = new SectionInfo();
					sectInfo.SECT = section.SectionLetter;
					sectInfo.TYPE = section.SectionType;
					sectInfo.STORY = section.StoryHeight;
					sectInfo.SQFT = section.SquareFootage;
					List<LineInfo> LINELIST = new List<LineInfo>();
					List<PointF> pts = new List<PointF>();

					if (section.HasSketch && section.SectionLines != null && section.SectionLines.Count > 0)
					{
						DrawSketchIfDataExists(ref MINX, ref MAXX, ref MINY, ref MAXY, section, LINELIST, pts);
					}
					sectInfo.POINTS = pts.ToArray();
					sectInfo.LINELIST = LINELIST.ToArray();
					sections.Add(sectInfo.SECT, sectInfo);
				}

				#endregion Loop through sections

				// DRAW SKETCH

				#region Draw Sketch

				scaleOut = CreateSketchFromLines(bitmapWidth, bitmapHeight, sizeXinPixels, sizeYinPixels, sizeInPixels, scaleOut, pen, base_pen, brush, font, noLabelTypes, sections, MINX, MAXX, MINY, MAXY);

				#endregion Draw Sketch

				return _sketch;
			}
		}

		private static void CenterLabelInPolygon(Brush brush, Font font, Graphics g, SectionInfo sect, PolygonF poly, ref string TYPELABEL, ref SizeF sz)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (sz.Width >= poly.Bounds.Width)
			{
				TYPELABEL = sect.SECT.Trim();
				sz = g.MeasureString(TYPELABEL, font);

				//drawDimensions = false;
			}

			if (sz.Height < poly.Bounds.Height)
			{
				PointF tp = poly.CenterPoint;
				tp.X -= sz.Width / 2;
				tp.Y -= sz.Height / 2;

				if ("A".Equals(sect.SECT.Trim()))
				{
					sz = DrawSectionA(brush, font, g, sect, poly, TYPELABEL, sz, tp);
				}
				else
				{
					g.DrawString(TYPELABEL, font, brush, tp);
				}
			}
		}

		private static SizeF DrawLinesWithDimensions(Brush brush, Font font, Graphics g, PolygonF poly, SizeF sz, LineInfo line)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			SizeF dimSz = g.MeasureString(line.LENGTH.ToString(), font);

			if ("N".Equals(line.DIRECTION) || "S".Equals(line.DIRECTION))
			{
				if ((sz.Width + (dimSz.Width * 2) + 4) < poly.Bounds.Width)
				{
					PointF dimPt = new PointF();
					dimPt.X = line.LINE_CENTER_POINT.X + 2;
					dimPt.Y = line.LINE_CENTER_POINT.Y;

					dimPt.Y -= dimSz.Height / 2;
					if (!poly.Contains(dimPt))
					{
						dimPt.X -= dimSz.Width + 2;
					}
					g.DrawString(line.LENGTH.ToString(), font, brush, dimPt);
				}
			}
			else if ("E".Equals(line.DIRECTION) || "W".Equals(line.DIRECTION))
			{
				if ((sz.Height + (dimSz.Height * 2) + 4) < poly.Bounds.Height)
				{
					PointF dimPt = new PointF();

					//var dimPt = new PointF
					//{
					//X = line.LINE_CENTER_POINT.X,
					//Y = line.LINE_CENTER_POINT.Y + 2
					//};

					dimPt.X = line.LINE_CENTER_POINT.X;
					dimPt.Y = line.LINE_CENTER_POINT.Y + 2;

					dimPt.X -= dimSz.Width / 2;
					if (!poly.Contains(dimPt))
					{
						dimPt.Y -= dimSz.Height + 2;
					}
					g.DrawString(line.LENGTH.ToString(), font, brush, dimPt);
				}
			}

			return sz;
		}

		private static SizeF DrawSectionA(Brush brush, Font font, Graphics g, SectionInfo sect, PolygonF poly, string TYPELABEL, SizeF sz, PointF tp)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			tp.Y -= sz.Height;
			sz = g.MeasureString(sect.SQFT.ToString(), font);
			PointF sq = new PointF();
			sq.X = poly.CenterPoint.X;
			sq.X -= sz.Width / 2;
			sq.Y = tp.Y + sz.Height + 2;

			sz = g.MeasureString(sect.STORY.ToString(), font);
			PointF st = new PointF();
			st.X = poly.CenterPoint.X;
			st.X -= sz.Width / 2;
			st.Y = tp.Y + sz.Height + sz.Height + 4;

			g.DrawString(TYPELABEL, font, brush, tp);
			g.DrawString(sect.SQFT.ToString(), font, brush, sq);
			g.DrawString(sect.STORY.ToString(), font, brush, st);
			return sz;
		}

		private static void DrawSketchIfDataExists(ref decimal MINX, ref decimal MAXX, ref decimal MINY, ref decimal MAXY, BuildingSection section, List<LineInfo> LINELIST, List<PointF> pts)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif

			foreach (BuildingLine line in section.SectionLines)
			{
				//int seq = Convert.ToInt32(_lineReader.GetDecimal(ord_JLLINE));
				decimal x = line.Point1X;
				decimal y = line.Point1Y;
				decimal x2 = line.Point2X;
				decimal y2 = line.Point2Y;

				PointF pt = new PointF(Convert.ToSingle(x), Convert.ToSingle(y));

				if (x < MINX)
				{
					MINX = x;
				}
				if (x > MAXX)
				{
					MAXX = x;
				}
				if (y < MINY)
				{
					MINY = y;
				}
				if (y > MAXY)
				{
					MAXY = y;
				}

				string dir = line.Directional;
				if ("N".Equals(dir) || "S".Equals(dir) || "E".Equals(dir) || "W".Equals(dir))
				{
					LineInfo lineInfo = new LineInfo();
					lineInfo.DIRECTION = dir;
					lineInfo.LINE_CENTER_POINT = new PointF();
					TranslateToDirection(line, ref pt, dir, ref lineInfo);
					LINELIST.Add(lineInfo);
				}
				pts.Add(pt);
			}
		}

		private static float EmSizeSetting(int sizeInPixels)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			float em = 8;
			if (sizeInPixels < 300)
			{
				em = 6;
			}
			else if (sizeInPixels > 500)
			{
				em = 10;
			}

			return em;
		}

		private static List<string> NoLabelTypesList()
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			List<string> noLabelTypes = new List<string>();
			noLabelTypes.Add("OH");
			noLabelTypes.Add("LAG");
			noLabelTypes.Add("ATTC");
			noLabelTypes.Add("BSMT");
			noLabelTypes.Add("FBMT");
			return noLabelTypes;
		}

		private static void ScaleAndAdjustCenterpoint(float scale, float TOTAL_X_ADJ, float TOTAL_Y_ADJ, SectionInfo sect, int j)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			sect.LINELIST[j].LINE_CENTER_POINT.X *= scale;
			sect.LINELIST[j].LINE_CENTER_POINT.X += TOTAL_X_ADJ;
			sect.LINELIST[j].LINE_CENTER_POINT.Y *= scale;
			sect.LINELIST[j].LINE_CENTER_POINT.Y += TOTAL_Y_ADJ;
		}

		private static int ScaleAndAdjustLine(float scale, float TOTAL_X_ADJ, float TOTAL_Y_ADJ, SectionInfo sect, int linCnt, int i)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			linCnt++;
			sect.POINTS[i].X *= scale;
			sect.POINTS[i].X += TOTAL_X_ADJ;
			sect.POINTS[i].Y *= scale;
			sect.POINTS[i].Y += TOTAL_Y_ADJ;
			return linCnt;
		}

		private static void TranslateToDirection(BuildingLine line, ref PointF pt, string dir, ref LineInfo lineInfo)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			switch (dir)
			{
				case "N":
					lineInfo.LENGTH = line.YLength;
					lineInfo.LINE_CENTER_POINT.X = pt.X;
					lineInfo.LINE_CENTER_POINT.Y = pt.Y - Convert.ToSingle(lineInfo.LENGTH / 2);
					break;

				case "S":
					lineInfo.LENGTH = line.YLength;
					lineInfo.LINE_CENTER_POINT.X = pt.X;
					lineInfo.LINE_CENTER_POINT.Y = pt.Y + Convert.ToSingle(lineInfo.LENGTH / 2);
					break;

				case "E":
					lineInfo.LENGTH = line.XLength;
					lineInfo.LINE_CENTER_POINT.X = pt.X + Convert.ToSingle(lineInfo.LENGTH / 2);
					lineInfo.LINE_CENTER_POINT.Y = pt.Y;
					break;

				case "W":
					lineInfo.LENGTH = line.XLength;
					lineInfo.LINE_CENTER_POINT.X = pt.X - Convert.ToSingle(lineInfo.LENGTH / 2);
					lineInfo.LINE_CENTER_POINT.Y = pt.Y;
					break;

				default:
					break;
			}
		}

		private float CreateSketchFromLines(int bitmapWidth, int bitmapHeight, int sizeXinPixels, int sizeYinPixels, int sizeInPixels, float scaleOut, Pen pen, Pen base_pen, Brush brush, Font font, List<string> noLabelTypes, SortedList<string, SectionInfo> sections, decimal MINX, decimal MAXX, decimal MINY, decimal MAXY)
		{

			if (sections.Values.Count > 0)
			{
				_sketch = new Bitmap(bitmapWidth, bitmapHeight, PixelFormat.Format32bppPArgb);

				Graphics g = Graphics.FromImage(_sketch);
				g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, bitmapWidth, bitmapHeight));  // bitmapWidth set at 1000, Height at 600
				int buffer = 20;
				int sizeNoBuffer = sizeInPixels - (buffer * 2);

				// Calculate scale and translation
				float XDIFF = Convert.ToSingle(Math.Abs(MAXX - MINX));
				float YDIFF = Convert.ToSingle(Math.Abs(MAXY - MINY));
				float XOFFSET = Convert.ToSingle(Math.Abs(MINX));
				float YOFFSET = Convert.ToSingle(Math.Abs(MINY));
				float scale = 1.0f;

				if (XDIFF >= YDIFF)
				{
					scale = Convert.ToSingle(sizeNoBuffer) / XDIFF;
				}
				else
				{
					scale = Convert.ToSingle(sizeNoBuffer) / YDIFF;
				}
				scaleOut = scale;

				float XDIFF_ADJ = XDIFF *= scale;
				float YDIFF_ADJ = YDIFF *= scale;
				float CENTERX_ADJ = (sizeInPixels - XDIFF_ADJ) / 2;
				float CENTERY_ADJ = (sizeInPixels - YDIFF_ADJ) / 2;

				float TOTAL_X_ADJ = (XOFFSET * scale) + CENTERX_ADJ;
				float TOTAL_Y_ADJ = (YOFFSET * scale) + CENTERY_ADJ;

				if (sizeXinPixels > sizeInPixels)
				{
					TOTAL_X_ADJ += ((sizeXinPixels - sizeInPixels) / 2);
					TOTAL_Y_ADJ += ((sizeYinPixels - sizeInPixels) / 2);
				}

				int secCnt = 0;

				#region sections.Values foreach loop

				foreach (SectionInfo sect in sections.Values)
				{
					secCnt = DrawTheSection(pen, base_pen, brush, font, noLabelTypes, sections, g, scale, TOTAL_X_ADJ, TOTAL_Y_ADJ, secCnt, sect);
				}

				#endregion sections.Values foreach loop
			}

			return scaleOut;
		}

		private bool DrawPolygonIfPresent(Pen pen, Pen base_pen, Graphics g, SectionInfo sect, bool isSketchDrawn)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			//var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			//UtilityMethods.LogMethodCall(fullStack, true);
#endif
			try
			{
				if (sect.POINTS != null && sect.POINTS.Count() >= 3)
				{
					if ("A".Equals(sect.SECT.Trim()))
					{
						g.DrawPolygon(base_pen, sect.POINTS);
					}
					else
					{
						g.DrawPolygon(pen, sect.POINTS);
					}
					isSketchDrawn = true;
				}
			}
			catch (ArgumentException aex)
			{
				_sketch = null;
				throw new SketchNotDrawnException(Record, Card, sect.SECT, aex);
			}
			catch (Exception ex)
			{
				_sketch = null;
				throw new SketchNotDrawnException(Record, Card, sect.SECT, ex);
			}

			return isSketchDrawn;
		}

		private int DrawTheSection(Pen pen, Pen base_pen, Brush brush, Font font, List<string> noLabelTypes, SortedList<string, SectionInfo> sections, Graphics g, float scale, float TOTAL_X_ADJ, float TOTAL_Y_ADJ, int secCnt, SectionInfo sect)
		{
			secCnt++;
			int linCnt = 0;
			for (int i = 0; i < sect.POINTS.Length; i++)
			{
				linCnt = ScaleAndAdjustLine(scale, TOTAL_X_ADJ, TOTAL_Y_ADJ, sect, linCnt, i);
			}

			for (int j = 0; j < sect.LINELIST.Length; j++)
			{
				ScaleAndAdjustCenterpoint(scale, TOTAL_X_ADJ, TOTAL_Y_ADJ, sect, j);
			}

			bool isSketchDrawn = false;
			isSketchDrawn = DrawPolygonIfPresent(pen, base_pen, g, sect, isSketchDrawn);

			PolygonF poly = new PolygonF(sect.POINTS);
			bool drawDimensions = true;

			string TYPELABEL = sect.SECT + "- " + sect.TYPE;
			SizeF sz = g.MeasureString(TYPELABEL, font);

			if (!noLabelTypes.Contains(sect.TYPE))
			{
				// place LABEL in center of Polygon
				if (poly.Contains(poly.CenterPoint))
				{
					CenterLabelInPolygon(brush, font, g, sect, poly, ref TYPELABEL, ref sz);
				}

				if (drawDimensions)
				{
					foreach (LineInfo line in sect.LINELIST)
					{
						sz = DrawLinesWithDimensions(brush, font, g, poly, sz, line);
					}
				}

				try
				{
					basePtX = sections.Values[0].POINTS[0].X;
					basePtY = sections.Values[0].POINTS[0].Y;
				}
				catch
				{
				}

				if (SketchCreatedEvent != null)
				{
					SketchCreatedEvent(this, new SketchCreatedEventArgs(_sketch, Record, Card));
				}
			}

			return secCnt;
		}

		public event SketchCreatedEventHandler SketchCreatedEvent;

		public struct LineInfo
		{
			public string DIRECTION;
			public decimal LENGTH;
			public PointF LINE_CENTER_POINT;
		}

		public struct SectionInfo
		{
			public LineInfo[] LINELIST;
			public PointF[] POINTS;
			public string SECT;
			public decimal SQFT;
			public decimal STORY;
			public string TYPE;
		}
	}

	public delegate void SketchCreatedEventHandler(object sender, SketchCreatedEventArgs e);
}