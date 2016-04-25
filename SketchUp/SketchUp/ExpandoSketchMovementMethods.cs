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
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                txtLocf = ((distanceD * _currentScale) / 2);

                decimal jup = Convert.ToDecimal(distanceD);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * _currentScale)), StartY);
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * _currentScale)), StartY);
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
                    }
                    g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * _currentScale)), StartY);
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                    }

                    BeginSplitX = BeginSplitX + distanceD;

                    NextStartX = BeginSplitX;
                }

                EndX = StartX + (distanceD * _currentScale);
                EndY = StartY;

                decimal d1 = Math.Round(Convert.ToDecimal(distanceD * _currentScale), 1);

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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                decimal XadjD = 0;
                decimal YadjD = 0;

                if (draw)
                {
                    Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                    Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                    if (startx == 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }

                    if (startx != 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }

                    if (starty == 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    if (starty != 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) + distance);

                    float X1adj = (float)XadjD;

                    if (Xadj != X1adj)
                    {
                        Xadj = X1adj;
                    }

                    YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

                    float Y1adj = (float)YadjD;

                    if (Yadj != Y1adj)
                    {
                        Yadj = Y1adj;
                    }

                    if (NextStartX != (float)XadjD)
                    {
                        NextStartX = (float)XadjD;
                        Xadj = NextStartX;
                    }
                    if (NextStartY != (float)YadjD)
                    {
                        NextStartY = (float)YadjD;
                        Yadj = NextStartY;
                    }
                }

                if (!draw)
                {
                    Xadj = BeginSplitX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = BeginSplitY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
                }
                if (draw)
                {
                    Xadj = NextStartX;

                    //Xadj = startx + distanceD;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = NextStartY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
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
                BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, _isclosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
            }
        }

        public void MoveNorth(float startx, float starty)
        {
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY - (distanceD * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY - txtLocf)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY - (distanceD * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + 5), (StartY - txtLocf)));
                    }
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + 10), (StartY - txtLocf)));
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

                decimal d1 = Math.Round(Convert.ToDecimal(distanceD * _currentScale), 1);

                float EndY2 = StartY - (float)d1;

                EndY = StartY - (distanceD * _currentScale);

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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                decimal XadjD = 0;
                decimal YadjD = 0;

                if (draw)
                {
                    Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                    Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                    if (startx == 0 && startx != Xadj)
                    {
                        Xadj = startx;

                        Xadj = NextStartX;
                    }

                    if (startx != 0 && startx != Xadj)
                    {
                        Xadj = startx;

                        Xadj = NextStartX;
                    }

                    if (starty == 0 && starty != Yadj)
                    {
                        Yadj = starty;

                        Yadj = NextStartY;
                    }

                    if (starty != 0 && starty != Yadj)
                    {
                        Yadj = starty;

                        Yadj = NextStartY;
                    }

                    XadjD = Math.Round(Convert.ToDecimal(Xadj), 1);

                    float X1adj = (float)XadjD;

                    if (Xadj != X1adj)
                    {
                        Xadj = X1adj;
                    }

                    YadjD = (Math.Round(Convert.ToDecimal(Yadj), 1) - distance);

                    float Y1adj = (float)YadjD;

                    if (Yadj != Y1adj)
                    {
                        Yadj = Y1adj;
                    }

                    if (NextStartX != (float)XadjD)
                    {
                        NextStartX = (float)XadjD;
                        Xadj = NextStartX;
                    }
                    if (NextStartY != (float)YadjD)
                    {
                        NextStartY = (float)YadjD;
                        Yadj = NextStartY;
                    }
                }

                if (!draw)
                {
                    Xadj = BeginSplitX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = BeginSplitY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
                }
                if (draw)
                {
                    Xadj = NextStartX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = NextStartY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
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
                BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, _isclosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
            }
        }


        public void MoveNorthEast(float startx, float starty)
        {
#if DEBUG

            //Debugging Code -- remove for production release
            var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
            UtilityMethods.LogMethodCall(fullStack, true);
#endif
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD1.ToString("N1"));

                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
                    }
                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                    }
                }

                EndX = StartX + (Convert.ToInt16(AngD1) * _currentScale);
                EndY = StartY - (Convert.ToInt16(AngD2) * _currentScale);
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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                decimal XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) + distance);

                decimal YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

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
                BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, _isclosing, NextStartX, NextStartY);
            }

            _isAngle = false;
            AngleForm.NorthEast = false;
            AngleForm.NorthWest = false;
            AngleForm.SouthEast = false;
            AngleForm.SouthWest = false;
        }

        public void MoveNorthWest(float startx, float starty)
        {
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD1.ToString("N1"));

                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
                    }
                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                    }
                }

                EndX = StartX - (Convert.ToInt16(AngD1) * _currentScale);
                EndY = StartY - (Convert.ToInt16(AngD2) * _currentScale);
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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                decimal XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) + distance);

                decimal YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

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
                BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, _isclosing, NextStartX, NextStartY);
            }

            _isAngle = false;
            AngleForm.NorthEast = false;
            AngleForm.NorthWest = false;
            AngleForm.SouthEast = false;
            AngleForm.SouthWest = false;
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

        public void MoveSouth(float startx, float starty)
        {

            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY + (distanceD * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY + txtLocf)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY + (distanceD * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + 5), (StartY - txtLocf)));
                    }
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + 10), (StartY - txtLocf)));
                    }

                    BeginSplitY = BeginSplitY + distanceD;

                    NextStartY = BeginSplitY;
                }

                EndX = StartX;

                decimal d1 = Math.Round(Convert.ToDecimal(distanceD * _currentScale), 1);

                float EndY2 = StartY + (float)d1;

                EndY = StartY + (distanceD * _currentScale);

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

                ExpSketchPBox.Image = MainImage;

                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                decimal XadjD = 0;
                decimal YadjD = 0;

                if (draw)
                {
                    Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                    Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                    if (startx == 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }

                    if (startx != 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }
                    if (starty == 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    if (starty != 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    XadjD = Math.Round(Convert.ToDecimal(Xadj), 1);

                    float X1adj = (float)XadjD;

                    if (Xadj != X1adj)
                    {
                        Xadj = X1adj;
                    }

                    YadjD = (Math.Round(Convert.ToDecimal(Yadj), 1) + distance);

                    float Y1adj = (float)YadjD;

                    if (Yadj != Y1adj)
                    {
                        Yadj = Y1adj;
                    }

                    if (NextStartX != (float)XadjD)
                    {
                        NextStartX = (float)XadjD;
                        Xadj = NextStartX;
                    }
                    if (NextStartY != (float)YadjD)
                    {
                        NextStartY = (float)YadjD;
                        Yadj = NextStartY;
                    }
                }

                if (!draw)
                {
                    Xadj = BeginSplitX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = BeginSplitY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
                }
                if (draw)
                {
                    Xadj = NextStartX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    //Yadj = NextStartY + distanceD;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
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
                BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, _isclosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
            }
        }

        public void MoveSouthEast(float startx, float starty)
        {
#if DEBUG

            //Debugging Code -- remove for production release
            var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
            UtilityMethods.LogMethodCall(fullStack, true);
#endif
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD1.ToString("N1"));

                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
                    }
                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                    }
                }

                EndX = StartX + (Convert.ToInt16(AngD1) * _currentScale);
                EndY = StartY + (Convert.ToInt16(AngD2) * _currentScale);
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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                decimal XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) + distance);

                decimal YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

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
                BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, _isclosing, NextStartX, NextStartY);
            }

            _isAngle = false;
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
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD1.ToString("N1"));

                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Pen pen2 = new Pen(Color.Green, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 5)));
                    }
                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                    }
                }

                EndX = StartX - (Convert.ToInt16(AngD1) * _currentScale);
                EndY = StartY + (Convert.ToInt16(AngD2) * _currentScale);
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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);
                //savpic.Add(click, imageToByteArray(_mainimage));

                Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                decimal XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) + distance);

                decimal YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

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
                BuildAddSQLAng(PrevX, PrevY, distanceDX, distanceDY, _direction, distanceD1, lineCnt, _isclosing, NextStartX, NextStartY);
            }

            _isAngle = false;
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
            if (_isKeyValid == true)
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

                _lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (distanceD * _currentScale)), StartY);
                    g.DrawString(_lenString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Arial", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (distanceD * _currentScale)), StartY);
                    if (distance < 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX - txtLocf), (StartY - 5)));
                    }
                    if (distance >= 10)
                    {
                        g.DrawString(_lenString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
                    }

                    BeginSplitX = BeginSplitX - distanceD;

                    NextStartX = BeginSplitX;
                }

                EndX = StartX - (distanceD * _currentScale);
                EndY = StartY;

                decimal d1 = Math.Round(Convert.ToDecimal(distanceD * _currentScale), 1);

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

                ExpSketchPBox.Image = MainImage;
                //click++;
                _StartX.Remove(click);
                _StartY.Remove(click);
                _StartX.Add(click, PrevX);
                _StartY.Add(click, PrevY);

                //savpic.Add(click, imageToByteArray(_mainimage));

                decimal XadjD = 0;
                decimal YadjD = 0;

                if (draw)
                {
                    Xadj = (((ScaleBaseX - PrevX) / _currentScale) * -1);
                    Yadj = (((ScaleBaseY - PrevY) / _currentScale) * -1);

                    if (startx == 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }

                    if (startx != 0 && startx != Xadj)
                    {
                        Xadj = startx;
                    }

                    if (starty == 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    if (starty != 0 && starty != Yadj)
                    {
                        Yadj = starty;
                    }

                    XadjD = (Math.Round(Convert.ToDecimal(Xadj), 1) - distance);
                    ;

                    float X1adj = (float)XadjD;

                    if (Xadj != X1adj && X1adj != startx)
                    {
                        Xadj = startx - distanceD;
                    }

                    YadjD = Math.Round(Convert.ToDecimal(Yadj), 1);

                    float Y1adj = (float)YadjD;

                    if (Yadj != Y1adj)
                    {
                        Yadj = Y1adj;
                    }

                    if (NextStartX != (float)XadjD)
                    {
                        NextStartX = (float)XadjD;
                        Xadj = NextStartX;
                    }
                    if (NextStartY != (float)YadjD)
                    {
                        NextStartY = (float)YadjD;
                        Yadj = NextStartY;
                    }
                }

                if (!draw)
                {
                    Xadj = BeginSplitX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = BeginSplitY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
                }
                if (draw)
                {
                    Xadj = NextStartX;

                    NextStartX = Xadj;
                    XadjD = Convert.ToDecimal(Xadj);

                    Yadj = NextStartY;

                    NextStartY = Yadj;
                    YadjD = Convert.ToDecimal(Yadj);
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
                BuildAddSQL(PrevX, PrevY, distanceD, _direction, lineCnt, _isclosing, NextStartX, NextStartY, PrevStartX, PrevStartY);
            }
        }

        private void HandleEastKeys()
        {
            LastDir = "E";
            if (_isAngle == false)
            {
                MoveEast(NextStartX, NextStartY);
                DistText.Focus();
            }
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        private void HandleNorthKeys()
        {
            _isKeyValid = IsValidDirection("N");

            LastDir = "N";
            if (_isAngle == false)
            {
                MoveNorth(NextStartX, NextStartY);
                DistText.Focus();
            }
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        private void HandleSouthKeys()
        {
            _isKeyValid = IsValidDirection("S");
            LastDir = "S";
            if (_isAngle == false)
            {
                MoveSouth(NextStartX, NextStartY);
                DistText.Focus();
            }
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        private void HandleWestKeys()
        {
            _isKeyValid = IsValidDirection("W");
            LastDir = "W";
            if (_isAngle == false)
            {
                MoveWest(NextStartX, NextStartY);
                DistText.Focus();
            }
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        #endregion Movement Methods
    }
}