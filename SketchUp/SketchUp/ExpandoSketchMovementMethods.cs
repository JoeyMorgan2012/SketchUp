using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
	public partial class ExpandoSketch : Form
	{
		#region Movement Methods

		public void MoveEast(float startx, float starty)
		{
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				float nx1 = NextStartX;

				float ny1 = NextStartY;

				float bx1 = BeginSplitX;

				float by1 = BeginSplitY;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				float.TryParse(DistText.Text, out distanceD);

				distance = Convert.ToDecimal(distanceD);

				lengthLabelString = String.Format("{0} ft.", distanceD.ToString("N1"));
				txtLocf = ((distanceD * currentScale) / 2);

				decimal jup = Convert.ToDecimal(distanceD);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * currentScale)), StartY);
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
				}

				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * currentScale)), StartY);
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
					}
					g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * currentScale)), StartY);
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
					}

					BeginSplitX = BeginSplitX + distanceD;

					NextStartX = BeginSplitX;
				}

				EndX = StartX + (distanceD * currentScale);
				EndY = StartY;

				decimal d1 = Math.Round(Convert.ToDecimal(distanceD * currentScale), 1);

				float EndX2 = StartX + (float)d1;

				txtX = (_mouseX + txtLocf);
				txtY = (_mouseY - 15);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				decimal XadjD = 0;
				decimal YadjD = 0;

				if (draw)
				{
					xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
					yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

					if (startx == 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}

					if (startx != 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}

					if (starty == 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					if (starty != 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) + distance);

					float X1adj = (float)XadjD;

					if (xAdjustment != X1adj)
					{
						xAdjustment = X1adj;
					}

					YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

					float Y1adj = (float)YadjD;

					if (yAdjustment != Y1adj)
					{
						yAdjustment = Y1adj;
					}

					if (NextStartX != (float)XadjD)
					{
						NextStartX = (float)XadjD;
						xAdjustment = NextStartX;
					}
					if (NextStartY != (float)YadjD)
					{
						NextStartY = (float)YadjD;
						yAdjustment = NextStartY;
					}
				}

				if (!draw)
				{
					xAdjustment = BeginSplitX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = BeginSplitY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}
				if (draw)
				{
					xAdjustment = NextStartX;

					//Xadj = startx + distanceD;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = NextStartY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}

				PrevStartX = NextStartX - distanceD;
				PrevStartY = NextStartY;

				XadjP = PrevStartX;
				YadjP = PrevStartY;

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (Math.Abs(YadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString())) &&
							Math.Abs(YadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString())) &&
							Math.Abs(XadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString())) &&
							Math.Abs(XadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "E";
				lineCnt++;
				BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, isClosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
			}
		}

		public void MoveNorth(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				float nx1 = NextStartX;

				float ny1 = NextStartY;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				float.TryParse(DistText.Text, out distanceD);

				distance = Convert.ToDecimal(distanceD);

				lengthLabelString = String.Format("{0} ft.", distanceD.ToString("N1"));
				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, StartX, (StartY - (distanceD * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 15), (StartY - txtLocf)));
				}
				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, StartX, (StartY - (distanceD * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 5), (StartY - txtLocf)));
					}
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 10), (StartY - txtLocf)));
					}

					if (startx == 0 && starty == 0)
					{
						NextStartX = startx;
						NextStartY = starty - distanceD;
					}

					BeginSplitY = BeginSplitY - distanceD;

					NextStartY = BeginSplitY;
				}

				EndX = StartX;

				decimal d1 = Math.Round(Convert.ToDecimal(distanceD * currentScale), 1);

				float EndY2 = StartY - (float)d1;

				EndY = StartY - (distanceD * currentScale);

				EndY = EndY2;

				txtX = (StartX + 15);
				txtY = (StartY - txtLocf);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				decimal XadjD = 0;
				decimal YadjD = 0;

				if (draw)
				{
					xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
					yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

					if (startx == 0 && startx != xAdjustment)
					{
						xAdjustment = startx;

						xAdjustment = NextStartX;
					}

					if (startx != 0 && startx != xAdjustment)
					{
						xAdjustment = startx;

						xAdjustment = NextStartX;
					}

					if (starty == 0 && starty != yAdjustment)
					{
						yAdjustment = starty;

						yAdjustment = NextStartY;
					}

					if (starty != 0 && starty != yAdjustment)
					{
						yAdjustment = starty;

						yAdjustment = NextStartY;
					}

					XadjD = Math.Round(Convert.ToDecimal(xAdjustment), 1);

					float X1adj = (float)XadjD;

					if (xAdjustment != X1adj)
					{
						xAdjustment = X1adj;
					}

					YadjD = (Math.Round(Convert.ToDecimal(yAdjustment), 1) - distance);

					float Y1adj = (float)YadjD;

					if (yAdjustment != Y1adj)
					{
						yAdjustment = Y1adj;
					}

					if (NextStartX != (float)XadjD)
					{
						NextStartX = (float)XadjD;
						xAdjustment = NextStartX;
					}
					if (NextStartY != (float)YadjD)
					{
						NextStartY = (float)YadjD;
						yAdjustment = NextStartY;
					}
				}

				if (!draw)
				{
					xAdjustment = BeginSplitX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = BeginSplitY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}
				if (draw)
				{
					xAdjustment = NextStartX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = NextStartY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}

				PrevStartX = NextStartX;
				PrevStartY = NextStartY + distanceD;

				XadjP = PrevStartX;
				YadjP = PrevStartY;

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (Math.Abs(YadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString())) &&
							Math.Abs(YadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString())) &&
							Math.Abs(XadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString())) &&
							Math.Abs(XadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "N";
				lineCnt++;
				BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, isClosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
			}
		}

		public void MoveNorthEast(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				string teset = DistText.Text.Trim();

				double D12 = Math.Pow(Convert.ToDouble(AngD1), 2);
				double D22 = Math.Pow(Convert.ToDouble(AngD2), 2);

				decimal D12d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD1), 2));

				decimal D22d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD2), 2));

				distanceD = (Convert.ToInt32(Math.Sqrt(D12 + D22)));

				decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(D12 + D22)), 3);

				distance = Convert.ToDecimal(distanceD1);

				decimal distanceDX = Convert.ToDecimal(AngD1);
				decimal distanceDY = Convert.ToDecimal(AngD2);
				distanceDXF = (float)distanceDX;
				distanceDYF = (float)distanceDY;

				lengthLabelString = String.Format("{0} ft.", distanceD1.ToString("N1"));

				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
				}

				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
					}
					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
					}
				}

				EndX = StartX + (Convert.ToInt16(AngD1) * currentScale);
				EndY = StartY - (Convert.ToInt16(AngD2) * currentScale);
				txtX = (_mouseX + txtLocf);
				txtY = (_mouseY - 15);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
				yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

				decimal XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) + distance);

				decimal YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (XadjD >= Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString()) && XadjD <= Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())
							&& YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString()) && YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString()))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "NE";
				lineCnt++;
				BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, isClosing, NextStartX, NextStartY);
			}

			isAngle = false;
			AngleForm.NorthEast = false;
			AngleForm.NorthWest = false;
			AngleForm.SouthEast = false;
			AngleForm.SouthWest = false;
		}

		public void MoveNorthWest(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				double D12 = Math.Pow(Convert.ToDouble(AngD1), 2);
				double D22 = Math.Pow(Convert.ToDouble(AngD2), 2);

				decimal D12d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD1), 2));

				decimal D22d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD2), 2));

				distanceD = Convert.ToInt32(Math.Sqrt(D12 + D22));

				decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(D12 + D22)), 2);

				distance = Convert.ToDecimal(distanceD1);

				decimal distanceDX = Convert.ToDecimal(AngD1);
				decimal distanceDY = Convert.ToDecimal(AngD2);
				distanceDXF = (float)distanceDX;
				distanceDYF = (float)distanceDY;

				lengthLabelString = String.Format("{0} ft.", distanceD1.ToString("N1"));

				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
				}

				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
					}
					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY - (Convert.ToInt16(AngD2) * currentScale)));
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
					}
				}

				EndX = StartX - (Convert.ToInt16(AngD1) * currentScale);
				EndY = StartY - (Convert.ToInt16(AngD2) * currentScale);
				txtX = (_mouseX + txtLocf);
				txtY = (_mouseY - 15);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
				yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

				decimal XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) + distance);

				decimal YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (XadjD >= Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString()) && XadjD <= Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())
							&& YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString()) && YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString()))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "NW";
				lineCnt++;
				BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, isClosing, NextStartX, NextStartY);
			}

			isAngle = false;
			AngleForm.NorthEast = false;
			AngleForm.NorthWest = false;
			AngleForm.SouthEast = false;
			AngleForm.SouthWest = false;
		}

		public void MoveSouth(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				float nx1 = NextStartX;

				float ny1 = NextStartY;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				float.TryParse(DistText.Text, out distanceD);

				distance = Convert.ToDecimal(distanceD);

				lengthLabelString = String.Format("{0} ft.", distanceD.ToString("N1"));
				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, StartX, (StartY + (distanceD * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 15), (StartY + txtLocf)));
				}
				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, StartX, (StartY + (distanceD * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 5), (StartY - txtLocf)));
					}
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + 10), (StartY - txtLocf)));
					}

					BeginSplitY = BeginSplitY + distanceD;

					NextStartY = BeginSplitY;
				}

				EndX = StartX;

				decimal d1 = Math.Round(Convert.ToDecimal(distanceD * currentScale), 1);

				float EndY2 = StartY + (float)d1;

				EndY = StartY + (distanceD * currentScale);

				EndY = EndY2;

				txtX = (StartX + 15);
				txtY = (StartY + txtLocf);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;

				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				decimal XadjD = 0;
				decimal YadjD = 0;

				if (draw)
				{
					xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
					yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

					if (startx == 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}

					if (startx != 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}
					if (starty == 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					if (starty != 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					XadjD = Math.Round(Convert.ToDecimal(xAdjustment), 1);

					float X1adj = (float)XadjD;

					if (xAdjustment != X1adj)
					{
						xAdjustment = X1adj;
					}

					YadjD = (Math.Round(Convert.ToDecimal(yAdjustment), 1) + distance);

					float Y1adj = (float)YadjD;

					if (yAdjustment != Y1adj)
					{
						yAdjustment = Y1adj;
					}

					if (NextStartX != (float)XadjD)
					{
						NextStartX = (float)XadjD;
						xAdjustment = NextStartX;
					}
					if (NextStartY != (float)YadjD)
					{
						NextStartY = (float)YadjD;
						yAdjustment = NextStartY;
					}
				}

				if (!draw)
				{
					xAdjustment = BeginSplitX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = BeginSplitY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}
				if (draw)
				{
					xAdjustment = NextStartX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					//Yadj = NextStartY + distanceD;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}

				PrevStartX = NextStartX;
				PrevStartY = NextStartY - distanceD;

				XadjP = PrevStartX;
				YadjP = PrevStartY;

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (Math.Abs(YadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString())) &&
							Math.Abs(YadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString())) &&
							Math.Abs(XadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString())) &&
							Math.Abs(XadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "S";
				lineCnt++;
				BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, isClosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
			}
		}

		public void MoveSouthEast(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				string teset = DistText.Text.Trim();

				double D12 = Math.Pow(Convert.ToDouble(AngD1), 2);
				double D22 = Math.Pow(Convert.ToDouble(AngD2), 2);

				decimal D12d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD1), 2));

				decimal D22d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD2), 2));

				distanceD = Convert.ToInt32(Math.Sqrt(D12 + D22));

				decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(D12 + D22)), 2);

				distance = Convert.ToDecimal(distanceD1);

				decimal distanceDX = Convert.ToDecimal(AngD1);
				decimal distanceDY = Convert.ToDecimal(AngD2);
				distanceDXF = (float)distanceDX;
				distanceDYF = (float)distanceDY;

				lengthLabelString = String.Format("{0} ft.", distanceD1.ToString("N1"));

				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
				}

				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
					}
					g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
					}
				}

				EndX = StartX + (Convert.ToInt16(AngD1) * currentScale);
				EndY = StartY + (Convert.ToInt16(AngD2) * currentScale);
				txtX = (_mouseX + txtLocf);
				txtY = (_mouseY - 15);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
				yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

				decimal XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) + distance);

				decimal YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (XadjD >= Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString()) && XadjD <= Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())
							&& YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString()) && YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString()))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "SE";
				lineCnt++;
				BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, isClosing, NextStartX, NextStartY);
			}

			isAngle = false;
			AngleForm.NorthEast = false;
			AngleForm.NorthWest = false;
			AngleForm.SouthEast = false;
			AngleForm.SouthWest = false;
		}

		public void MoveSouthWest(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				string teset = DistText.Text.Trim();

				double D12 = Math.Pow(Convert.ToDouble(AngD1), 2);
				double D22 = Math.Pow(Convert.ToDouble(AngD2), 2);

				decimal D12d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD1), 2));

				decimal D22d = Convert.ToDecimal(Math.Pow(Convert.ToDouble(AngD2), 2));

				distanceD = Convert.ToInt32(Math.Sqrt(D12 + D22));

				decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(D12 + D22)), 2);

				distance = Convert.ToDecimal(distanceD1);

				decimal distanceDX = Convert.ToDecimal(AngD1);
				decimal distanceDY = Convert.ToDecimal(AngD2);
				distanceDXF = (float)distanceDX;
				distanceDYF = (float)distanceDY;

				lengthLabelString = String.Format("{0} ft.", distanceD1.ToString("N1"));

				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Pen pen2 = new Pen(Color.Green, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
				}

				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
					}
					g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * currentScale)), (StartY + (Convert.ToInt16(AngD2) * currentScale)));
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
					}
				}

				EndX = StartX - (Convert.ToInt16(AngD1) * currentScale);
				EndY = StartY + (Convert.ToInt16(AngD2) * currentScale);
				txtX = (_mouseX + txtLocf);
				txtY = (_mouseY - 15);

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);
				savpic.Add(click, imageToByteArray(_mainimage));

				xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
				yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

				decimal XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) + distance);

				decimal YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (XadjD >= Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString()) && XadjD <= Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())
							&& YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString()) && YadjD == Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString()))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "SW";
				lineCnt++;
				BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, isClosing, NextStartX, NextStartY);
			}

			isAngle = false;
			AngleForm.NorthEast = false;
			AngleForm.NorthWest = false;
			AngleForm.SouthEast = false;
			AngleForm.SouthWest = false;
		}

		public void MoveWest(float startx, float starty)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
			if (isValidKey == true)
			{
				StrxD = 0;
				StryD = 0;
				EndxD = 0;
				EndyD = 0;
				midLine = 0;
				midDirect = String.Empty;
				midSection = String.Empty;

				float nx1 = NextStartX;

				float ny1 = NextStartY;

				distanceD = 0;
				distanceDXF = 0;
				distanceDYF = 0;

				float.TryParse(DistText.Text, out distanceD);

				distance = Convert.ToDecimal(distanceD);

				lengthLabelString = String.Format("{0} ft.", distanceD.ToString("N1"));
				txtLocf = ((distanceD * currentScale) / 2);

				if (draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Red);
					Pen pen1 = new Pen(Color.Red, 2);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (distanceD * currentScale)), StartY);
					g.DrawString(lengthLabelString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
				}
				if (!draw)
				{
					Graphics g = Graphics.FromImage(_mainimage);
					SolidBrush brush = new SolidBrush(Color.Black);
					Pen pen1 = new Pen(Color.Cyan, 5);
					Font f = new Font("Arial", 8, FontStyle.Bold);

					g.DrawLine(pen1, StartX, StartY, (StartX - (distanceD * currentScale)), StartY);
					if (distance < 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX - txtLocf), (StartY - 5)));
					}
					if (distance >= 10)
					{
						g.DrawString(lengthLabelString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
					}

					BeginSplitX = BeginSplitX - distanceD;

					NextStartX = BeginSplitX;
				}

				EndX = StartX - (distanceD * currentScale);
				EndY = StartY;

				decimal d1 = Math.Round(Convert.ToDecimal(distanceD * currentScale), 1);

				float EndX2 = StartX - (float)d1;

				txtX = (StartX - txtLocf);
				txtY = (StartY - 15);

				EndX = EndX2;

				PrevX = StartX;
				PrevY = StartY;

				StartX = EndX;
				StartY = EndY;

				_mouseX = Convert.ToInt32(EndX);
				_mouseY = Convert.ToInt32(EndY);

				DistText.Text = String.Empty;

				DistText.Focus();

				ExpSketchPBox.Image = _mainimage;
				click++;
				startPointX.Remove(click);
				startPointY.Remove(click);
				startPointX.Add(click, PrevX);
				startPointY.Add(click, PrevY);

				savpic.Add(click, imageToByteArray(_mainimage));

				decimal XadjD = 0;
				decimal YadjD = 0;

				if (draw)
				{
					xAdjustment = (((ScaleBaseX - PrevX) / currentScale) * -1);
					yAdjustment = (((ScaleBaseY - PrevY) / currentScale) * -1);

					if (startx == 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}

					if (startx != 0 && startx != xAdjustment)
					{
						xAdjustment = startx;
					}

					if (starty == 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					if (starty != 0 && starty != yAdjustment)
					{
						yAdjustment = starty;
					}

					XadjD = (Math.Round(Convert.ToDecimal(xAdjustment), 1) - distance);
					;

					float X1adj = (float)XadjD;

					if (xAdjustment != X1adj && X1adj != startx)
					{
						xAdjustment = startx - distanceD;
					}

					YadjD = Math.Round(Convert.ToDecimal(yAdjustment), 1);

					float Y1adj = (float)YadjD;

					if (yAdjustment != Y1adj)
					{
						yAdjustment = Y1adj;
					}

					if (NextStartX != (float)XadjD)
					{
						NextStartX = (float)XadjD;
						xAdjustment = NextStartX;
					}
					if (NextStartY != (float)YadjD)
					{
						NextStartY = (float)YadjD;
						yAdjustment = NextStartY;
					}
				}

				if (!draw)
				{
					xAdjustment = BeginSplitX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = BeginSplitY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}
				if (draw)
				{
					xAdjustment = NextStartX;

					NextStartX = xAdjustment;
					XadjD = Convert.ToDecimal(xAdjustment);

					yAdjustment = NextStartY;

					NextStartY = yAdjustment;
					YadjD = Convert.ToDecimal(yAdjustment);
				}

				PrevStartX = NextStartX + distanceD;
				PrevStartY = NextStartY;

				XadjP = PrevStartX;
				YadjP = PrevStartY;

				if (JumpTable.Rows.Count > 0)
				{
					for (int i = 0; i < JumpTable.Rows.Count; i++)
					{
						if (Math.Abs(YadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString())) &&
							Math.Abs(YadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString())) &&
							Math.Abs(XadjD) >= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString())) &&
							Math.Abs(XadjD) <= Math.Abs(Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString())))
						{
							StrxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt1"].ToString());
							StryD = Convert.ToDecimal(JumpTable.Rows[i]["YPt1"].ToString());
							EndxD = Convert.ToDecimal(JumpTable.Rows[i]["XPt2"].ToString());
							EndyD = Convert.ToDecimal(JumpTable.Rows[i]["YPt2"].ToString());

							midSection = JumpTable.Rows[i]["Sect"].ToString();
							midLine = Convert.ToInt32(JumpTable.Rows[i]["LineNo"].ToString());
							midDirect = JumpTable.Rows[i]["Direct"].ToString();
							break;
						}
					}
				}

				string _direction = "W";
				lineCnt++;
				BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, isClosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
			}
		}

		private void HandleEastKeys()
		{
			lastLineDirection = "E";
			if (isAngle == false)
			{
				MoveEast(NextStartX, NextStartY);
				DistText.Focus();
			}
			if (isAngle == true)
			{
				MeasureAngle();
			}
		}

		private void HandleNorthKeys()
		{
			isValidKey = IsValidDirection("N");

			lastLineDirection = "N";
			if (isAngle == false)
			{
				MoveNorth(NextStartX, NextStartY);
				DistText.Focus();
			}
			if (isAngle == true)
			{
				MeasureAngle();
			}
		}

		private void HandleSouthKeys()
		{
			isValidKey = IsValidDirection("S");
			lastLineDirection = "S";
			if (isAngle == false)
			{
				MoveSouth(NextStartX, NextStartY);
				DistText.Focus();
			}
			if (isAngle == true)
			{
				MeasureAngle();
			}
		}

		private void HandleWestKeys()
		{
			isValidKey = IsValidDirection("W");
			lastLineDirection = "W";
			if (isAngle == false)
			{
				MoveWest(NextStartX, NextStartY);
				DistText.Focus();
			}
			if (isAngle == true)
			{
				MeasureAngle();
			}
		}

		#endregion Movement Methods
	}
}