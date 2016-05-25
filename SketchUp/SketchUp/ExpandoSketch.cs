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
    /*
       The ExpandoSketch Form contains all of the sketch-rendering code.
       The original file was over 8,000 lines long, so the class is broken into two
       physical files defining the logical class. The breakdown is:

           ExpandoSketch.cs - All methods not refactored into SketchRepository

           ExpandoSketchFields.cs -  This file contains fields, properties and enums for the ExpandoSketch Form class.
   */

    public partial class ExpandoSketch : Form
    {
#region "Constructor"

        public ExpandoSketch(string sketchFolder, int sketchRecord, int sketchCard, bool hasSketch, bool hasNewSketch)
        {
            // Omitted any steps not needed for SketchUp. JMM 5-9-2016
            InitializeComponent();
            AddSectionContextMenu.Enabled = false;
            WorkingParcel = SketchUpGlobals.ParcelWorkingCopy;
            ShowVersion(WorkingParcel.SnapShotIndex);
            EditState = DrawingState.SketchLoaded;
            ShowWorkingCopySketch(sketchFolder, sketchRecord.ToString(), sketchCard.ToString(), hasSketch, hasNewSketch);
        }

#endregion

#region "Public Methods"

        public void AddParcelToSnapshots(SMParcel parcel)
        {
            SketchRepository sr = new SketchRepository(parcel);
            WorkingParcel = sr.AddSketchToSnapshots(parcel);
            ShowVersion(WorkingParcel.SnapShotIndex);
        }

        //public void AdjustLine(decimal newEndX, decimal newEndY, decimal newDistX, decimal newDistY, decimal EndEndX, decimal EndEndY, decimal finDist)
        //{
        //    StringBuilder adjLine = new StringBuilder();
        //    adjLine.Append(String.Format("update {0}.{1}line set jldirect = '{2}',jlxlen = {3},jlylen = {4},jllinelen = {5}, ",
        //                   SketchUpGlobals.LocalLib,
        //                  SketchUpGlobals.LocalityPrefix,
        //                    CurrentAttDir,
        //                    newDistX,
        //                    newDistY,
        //                    finDist));
        //    adjLine.Append(String.Format("jlpt1x = {0},jlpt1y = {1},jlpt2x = {2},jlpt2y = {3} ",
        //            newEndX, newEndY, EndEndX, EndEndY));
        //    adjLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
        //       SketchUpGlobals.Record, SketchUpGlobals.Card, _savedAttSection, (mylineNo + 1)));

        //    dbConn.DBConnection.ExecuteNonSelectStatement(adjLine.ToString());
        //}


        public void MoveNorthEast(float startx, float starty)
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
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

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
            AngleFormOriginal.NorthEast = false;
            AngleFormOriginal.NorthWest = false;
            AngleFormOriginal.SouthEast = false;
            AngleFormOriginal.SouthWest = false;
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
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

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
            AngleFormOriginal.NorthEast = false;
            AngleFormOriginal.NorthWest = false;
            AngleFormOriginal.SouthEast = false;
            AngleFormOriginal.SouthWest = false;
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

                //_lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                _lenString = String.Format("{0:N1} ft.", distanceD.ToString());
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY + (distanceD * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY + txtLocf)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

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
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

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
            AngleFormOriginal.NorthEast = false;
            AngleFormOriginal.NorthWest = false;
            AngleFormOriginal.SouthEast = false;
            AngleFormOriginal.SouthWest = false;
        }

        public void MoveSouthWest(float startx, float starty)
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
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

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
            AngleFormOriginal.NorthEast = false;
            AngleFormOriginal.NorthWest = false;
            AngleFormOriginal.SouthEast = false;
            AngleFormOriginal.SouthWest = false;
        }

        public void MoveWest(float startx, float starty)
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

                //_lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                _lenString = String.Format("{0:N1} ft.", distanceD.ToString());
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX - (distanceD * _currentScale)), StartY);
                    g.DrawString(_lenString, f, brush, new PointF((StartX - txtLocf), (StartY - 15)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

                sketchBox.Image = MainImage;

                //click++;
                //_StartX.Remove(click);
                //_StartY.Remove(click);
                //_StartX.Add(click, PrevX);
                //_StartY.Add(click, PrevY);

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

        public void RefreshSketch()
        {
            //ExpSketchPBox.Refresh();
            //MainImage = null;
            //float scaleOut = 0.00f;
            //SMParcel parcel = SketchUpGlobals.ParcelWorkingCopy;
            //MainImage = _currentParcel.GetSketchImage(parcel, ExpSketchPBox.Width, ExpSketchPBox.Height,
            //    1000, 572, 400, out scaleOut);
            //DrawingScale = (float)parcel.Scale;
            //_currentScale = DrawingScale;
            //Graphics g = Graphics.FromImage(MainImage);
            //SMSketcher sketcher = new SMSketcher(parcel, ExpSketchPBox);
            //sketcher.RenderSketch(true);
            //MainImage = sketcher.SketchImage;

            //SolidBrush Lblbrush = new SolidBrush(Color.Black);
            //SolidBrush FillBrush = new SolidBrush(Color.White);
            //Pen whitePen = new Pen(Color.White, 2);
            //Pen blackPen = new Pen(Color.Black, 2);

            //Font LbLf = new Font("Segue UI", 10, FontStyle.Bold);
            //Font TitleF = new Font("Segue UI", 10, FontStyle.Bold | FontStyle.Underline);
            //Font MainTitle = new System.Drawing.Font("Segue UI", 15, FontStyle.Bold | FontStyle.Underline);
            //char[] leadzero = new char[] { '0' };

            //g.DrawString(Locality, TitleF, Lblbrush, new PointF(10, 10));
            //g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            //g.DrawString(String.Format("Record # - {0}", SketchRecord.TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            //g.DrawString(String.Format("Card # - {0}", SketchCard), LbLf, Lblbrush, new PointF(10, 45));

            //g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));

            //ExpSketchPBox.Image = MainImage;

            //if (_closeSketch == true)
            //{
            //    Close();
            //}
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        public void setAttPnts()
        {
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
            ShowVersion(WorkingParcel.SnapShotIndex);
            var AttachPoints = (from l in
                  WorkingParcel.AllSectionLines.Where(s =>
                  s.SectionLetter != "A" && s.LineNumber == 1).ToList()
                                select l);
            if (AttachPoints.ToList().Count > 0)
            {
                WorkingParcel.Sections.Where(s => s.SectionLetter ==
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
                WorkingParcel.AllSectionLines.Where(s =>
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
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
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

        #endregion

        #region "Private methods"

        private static string LegalDirectionsMessage(List<string> legalDirections)
        {
            StringBuilder statusMessage = new StringBuilder();
            statusMessage.Append("Legal Directions: ");
            int counter = 1;
            foreach (string s in legalDirections.Distinct())
            {
                if (counter < legalDirections.Count)
                {
                    statusMessage.AppendFormat("{0}, ", s);
                }
                else
                {
                    statusMessage.AppendFormat("{0}", s);
                }
                counter++;
            }

            return statusMessage.ToString();
        }

        //    if (AngleForm.NorthWest == true)
        //    {
        //        MoveNorthWest(NextStartX, NextStartY);
        //    }
        private static Color PenColorForDrawing(DrawingState editingStep)
        {
            Color penColor;
            switch (editingStep)
            {
                case DrawingState.Drawing:
                    penColor = Color.Red;
                    break;

                case DrawingState.SectionAdded:
                case DrawingState.JumpPointSelected:
                    penColor = Color.Teal;
                    break;

                default:
                    penColor = Color.Transparent;
                    break;
            }

            return penColor;
        }

        private void AddJumpLineToSketch(CamraDataEnums.CardinalDirection direction, decimal xLength, decimal yLength, decimal dbStartX, decimal dbStartY)
        {
            try
            {
                if (LegalMoveDirections.Contains(direction.ToString()))
                {
                    DrawOnlyLine newLine = new DrawOnlyLine(AttachmentSection);

                    newLine.LineNumber = AttachmentSection.Lines.Count + 1;
                    newLine.Direction = direction.ToString().ToUpper();
                    newLine.StartX = DbStartX;
                    newLine.StartY = dbStartY;
                    newLine.EndX = newLine.StartX + xLength;
                    newLine.EndY = newLine.StartY + yLength;
                    newLine.XLength = xLength;
                    newLine.YLength = yLength;
                    WorkingParcel.DrawOnlyLines.Add(newLine);
                    WorkingSection.ParentParcel = WorkingParcel;
                }
                else
                {
                    string statusMessage = string.Format("You must move along a line attached to the starting corner. \n\n{0}", LegalDirectionsMessage(LegalMoveDirections));
                    MessageBox.Show(statusMessage);
                }
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

        private void AddJumpTableRow(float jx, float jy, float CurrentScale, SMLine line)
        {
            try
            {
                decimal Distance = 0;

                DataRow row = JumpTable.NewRow();
                row["Record"] = line.Record;
                row["Card"] = line.Dwelling;
                row["Sect"] = line.SectionLetter;
                row["LineNo"] = line.LineNumber;
                row["Direct"] = line.Direction;
                row["XLen"] = line.XLength;
                row["YLen"] = line.YLength;
                row["Length"] = line.LineLength;
                row["Angle"] = line.LineAngle;
                row["XPt1"] = line.StartX;
                row["YPt1"] = line.StartY;
                row["XPt2"] = line.EndX;
                row["YPt2"] = line.EndY;
                row["Attach"] = line.AttachedSection;

                float xPt2 = (float)line.EndX;
                float yPt2 = (float)line.EndY;

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

        private void AddLineToSketch(CamraDataEnums.CardinalDirection direction, decimal xLength, decimal yLength, decimal dbStartX, decimal dbStartY)
        {
            try
            {
                SMLine newLine = new SMLine(WorkingSection);
                newLine.LineNumber = WorkingSection.Lines.Count + 1;
                newLine.Direction = direction.ToString().ToUpper();
                newLine.StartX = DbStartX;
                newLine.StartY = this.dbStartY;
                newLine.EndX = newLine.StartX + xLength;
                newLine.EndY = newLine.StartY + yLength;
                newLine.XLength = xLength;
                newLine.YLength = yLength;
                WorkingSection.Lines.Add(newLine);
                WorkingSection.ParentParcel = WorkingParcel;
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

        private void AddListItemsToJumpTableList(float jx, float jy, decimal CurrentScale, List<SMLine> lines)
        {
            try
            {
                foreach (SMLine l in lines)
                {
                    AddJumpTableRow(jx, jy, (float)CurrentScale, l);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
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
                          SketchUpGlobals.LocalityPrefix,
                       SketchUpGlobals.Record,
                       SketchUpGlobals.Card));

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
                          SketchUpGlobals.LocalityPrefix,
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card));

            try
            {
                baseStory = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(getStory.ToString()));
            }
            catch
            {
            }

            DataSet ds_master = UpdateMasterArea(summedArea);

            //TODO: Refactor into SketchManager
            //if (_deleteMaster == false)
            //{
            //    InsertMasterRecord(summedArea, baseStory, ds_master);
            //}
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

        //private void AddNewLine(int nextLineNumber, PointF dbStart, PointF dbEnd, string direction)
        //{ //TODO: Start here with checking the scaled values. Round them.
        //    SMLine newLine = new SMLine(WorkingSection);
        //    newLine.LineNumber = nextLineNumber;
        //    newLine.Direction = direction;
        //    newLine.StartX = (decimal)dbStart.X;
        //    newLine.StartY = (decimal)dbStart.Y;
        //    newLine.EndX = (decimal)dbEnd.X;
        //    newLine.EndY = (decimal)dbEnd.Y;
        //    newLine.XLength = Math.Round(Math.Abs((decimal)dbStart.X - (decimal)dbEnd.X), 2);
        //    newLine.YLength = Math.Round(Math.Abs((decimal)dbStart.Y - (decimal)dbEnd.Y), 2);
        //    newLine.SectionLetter = WorkingSection.SectionLetter;
        //    newLine.ParentSection = WorkingSection;
        //    newLine.ParentParcel = WorkingSection.ParentParcel;
        //    newLine.SectionLetter = WorkingSection.SectionLetter;
        //    WorkingSection.Lines.Add(newLine);
        //}
        private void AddSectionBtn_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void AddSection()
        {
            GetSectionTypeInfo();
            _deleteMaster = false;
            btnBeginSection.BackColor = Color.Orange;
            btnBeginSection.Text = "Begin";

            _isClosed = false;
        }

      

        private void AddXLine(string sectionLetter)
        {
            SMSection thisSection = (from s in WorkingParcel.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();
            SMLine xLine = new SMLine { Record = thisSection.Record, Dwelling = thisSection.Dwelling, SectionLetter = thisSection.SectionLetter, LineNumber = thisSection.Lines.Count + 1, StartX = 0, StartY = 0, EndX = 0, EndY = 0, ParentParcel = thisSection.ParentParcel, Direction = "X" };
            WorkingParcel.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault<SMSection>().Lines.Add(xLine);
        }

        private void AdjustLengthDirection(CamraDataEnums.CardinalDirection moveDirection, ref decimal xLength, ref decimal ylength)
        {
            switch (moveDirection)
            {
                case CamraDataEnums.CardinalDirection.N:
                case CamraDataEnums.CardinalDirection.NE:
                    ylength *= -1;
                    break;

                case CamraDataEnums.CardinalDirection.SW:
                case CamraDataEnums.CardinalDirection.W:
                    xLength *= -1;
                    break;

                case CamraDataEnums.CardinalDirection.NW:
                    ylength *= -1;
                    xLength *= -1;
                    break;

                default:
                    break;
            }
        }

        private void AttachNewSectionToSketch()
        {
            bool sectionReady = WorkingSection != null && WorkingSection.Lines != null & WorkingSection.SectionIsClosed;
            if (sectionReady)
            {
                //Determine if the origin of the new section requires a line split
                SMLine firstLine = (from l in WorkingSection.Lines where l.LineNumber == 1 select l).FirstOrDefault();
                PointF wsStart = firstLine.StartPoint;
                var linesWithStart = (from l in AttachmentSection.Lines where SMGlobal.PointIsOnLine(l.StartPoint, l.EndPoint, wsStart) select l).ToList();

                if (linesWithStart != null && linesWithStart.Count > 1)
                {
                    // If there is more than one line, the jump point is the start point;
                    // (because it is a corner.) No split needed.
                    SMLine anchorLine = (from l in linesWithStart where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
                }
                else
                {
                    SMLine anchorLine = linesWithStart.First();
                    SMLineManager slm = new SMLineManager();
#if DEBUG
                    DevUtilities du = new DevUtilities();
                    Trace.WriteLine("\nBefore Break: \n");
                    Trace.WriteLine(du.ParcelInfo(WorkingParcel));
#endif

                    AttachmentSection = slm.SectionWithLineBreak(AttachmentSection, anchorLine.LineNumber, wsStart);
                    anchorLine = (from l in AttachmentSection.Lines where l.EndPoint == wsStart select l).FirstOrDefault();
                    anchorLine.AttachedSection = WorkingSection.SectionLetter;
#if DEBUG
                    Trace.WriteLine("\nAfter Break: \n");
                    Trace.WriteLine(du.ParcelInfo(WorkingParcel));
                    Trace.Flush();
#endif
                }
            }
        }

        private void BeginSectionBtn_Click(object sender, EventArgs e)
        {
            EditState = DrawingState.Drawing;
            SetButtonStates(EditState);
        }

        private void BuildAddSQL(float prevX, float prevY, float distD, string direction, int _lineCnt, bool closing, float startx, float starty, float prevstartx, float prevstarty)
        {
            _isclosing = closing;

            pt2X = 0;
            pt2Y = 0;

            float nx1 = NextStartX;

            float ny1 = NextStartY;

            float nx2 = Xadj;

            float ny2 = Yadj;

            float nx2P = XadjP;

            float ny2P = YadjP;

            decimal dste = Convert.ToDecimal(distD);

            Xadj = (((ScaleBaseX - prevX) / _currentScale) * -1);
            Yadj = (((ScaleBaseY - prevY) / _currentScale) * -1);

            NewSectionPoints.Add(new PointF(Xadj, Yadj));

            if (NextStartX != Xadj)
            {
                Xadj = NextStartX;
            }
            if (NextStartY != Yadj)
            {
                Yadj = NextStartY;
            }

            float lengthX = 0;
            float lengthY = 0;
            if (direction == "E")
            {
                if (_isclosing == true)
                {
                    NextStartX = startx;
                }

                Xadj = NextStartX - distD;

                //Xadj = NextStartX;

                lengthX = distD;
                lengthY = 0;

                pt2X = Xadj + distD;
                pt2Y = Yadj;
            }

            if (direction == "W")
            {
                if (_isclosing == true)
                {
                    NextStartX = startx;
                }

                Xadj = NextStartX + distD;

                //Xadj = NextStartX;

                lengthX = distD;
                lengthY = 0;

                pt2X = Xadj - distD;
                pt2Y = Yadj;
            }
            if (direction == "N")
            {
                if (_isclosing == true)
                {
                    NextStartY = starty;
                }

                Yadj = NextStartY + distD;

                //Yadj = NextStartY;

                lengthX = 0;
                lengthY = distD;

                pt2X = Xadj;
                pt2Y = Yadj - distD;
            }
            if (direction == "S")
            {
                if (_isclosing == true)
                {
                    NextStartY = starty;
                }

                Yadj = NextStartY - distD;

                //Yadj = NextStartY;

                lengthX = 0;
                lengthY = distD;

                pt2X = Xadj;
                pt2Y = Yadj + distD;

                //pt2Y = Yadj;
            }

            if (draw)
            {
                if (_lineCnt == 1)
                {
                    if (_hasNewSketch == true)
                    {
                        Xadj = 0;
                        Yadj = 0;
                    }

                    SecBeginX = Xadj;
                    SecBeginY = Yadj;
                }

                decimal Tst1 = Convert.ToDecimal(Xadj);
                decimal Tst2 = Convert.ToDecimal(Yadj);
                decimal Ptx = Convert.ToDecimal(pt2X);
                decimal Pty = Convert.ToDecimal(pt2Y);

                decimal ptyT = Math.Round(Pty, 1);

                var rndTst1 = Math.Round(Tst1, 1);
                var rndTst2 = Math.Round(Tst2, 1);
                var rndPt2X = Math.Round(Ptx, 1);
                var rndPt2Y = Math.Round(Pty, 1);

                if (_hasNewSketch == true && _lineCnt == 1)
                {
                    switch (direction)
                    {
                        case "N":

                            rndTst1 = 0;
                            rndTst2 = 0;
                            rndPt2X = rndTst1;

                            rndPt2Y = rndTst2 - (Convert.ToDecimal(lengthY));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        case "S":

                            rndTst1 = 0;
                            rndTst2 = 0;
                            rndPt2X = rndTst1;

                            rndPt2Y = rndTst2 + (Convert.ToDecimal(lengthY));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        case "E":
                            rndTst1 = 0;
                            rndTst2 = 0;
                            rndPt2Y = rndTst2;

                            rndPt2X = rndTst1 + (Convert.ToDecimal(lengthX));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        case "W":
                            rndTst1 = 0;
                            rndTst2 = 0;
                            rndPt2Y = rndTst2;

                            rndPt2X = rndTst1 - (Convert.ToDecimal(lengthX));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        default:
                            throw new NotImplementedException(string.Format("Invalid line direction: '{0}", direction));
                    }

                    //decimal TprevTst1 = prevTst1;
                    //decimal TprevTst2 = prevTst2;
                    //decimal TprevPt2X = prevPt2X;
                    //decimal TprevPt2Y = prevPt2Y;
                }

                if (_hasNewSketch == true && _lineCnt > 1)
                {
                    //decimal TprevTst1 = prevTst1;
                    //decimal TprevTst2 = prevTst2;
                    //decimal TprevPt2X = prevPt2X;
                    //decimal TprevPt2Y = prevPt2Y;
                    switch (direction)
                    {
                        case "N":
                            rndPt2Y = rndTst2 - (Convert.ToDecimal(lengthY));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        case "S":
                            rndPt2Y = rndTst2 + (Convert.ToDecimal(lengthY));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;

                            break;

                        case "E":
                            rndPt2X = rndTst1 + (Convert.ToDecimal(lengthX));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        case "W":
                            rndPt2X = rndTst1 - (Convert.ToDecimal(lengthX));

                            prevPt2X = rndPt2X;
                            prevPt2Y = rndPt2Y;
                            prevTst1 = rndPt2X;
                            prevTst2 = rndPt2Y;
                            break;

                        default:

                            throw new NotImplementedException(string.Format("Invalid line direction: '{0}", direction));
                    }
                }

                if (_isclosing == true)
                {
                    var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
                    var sectionArea = sectionPolygon.Area;

                    if (_nextStoryHeight < 1.0m)
                    {
                        NextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
                    }
                    if (_nextStoryHeight >= 1.0m)
                    {
                        NextSectArea = Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1);
                    }
                }

                StringBuilder mxline = new StringBuilder();
                mxline.Append(String.Format("select max(jlline#) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                              SketchUpGlobals.LocalLib,
                              SketchUpGlobals.LocalityPrefix,
                               SketchUpGlobals.Record,
                               SketchUpGlobals.Card,
                                   NextSectLtr));

                try
                {
                    _curLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(mxline.ToString()));
                }
                catch
                {
                }

                if (_curLineCnt == 0)
                {
                    _curLineCnt = 0;
                }

                _lineCnt = _curLineCnt;

                lineCnt = _curLineCnt + 1;

                if (lineCnt == 19)
                {
                    MessageBox.Show("Next Line will Max Section Lines", "Line Count Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                if (lineCnt > sketchBoxPaddingTotal)
                {
                    MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                //MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2695", SketchUpGlobals.Record, SketchUpGlobals.Card));

                decimal t1 = rndTst1;
                decimal t2 = rndTst2;

                decimal tX1 = rndPt2X;
                decimal tY1 = rndPt2Y;

                if (lineCnt <= sketchBoxPaddingTotal)
                {
                    StringBuilder addSect = new StringBuilder();
                    addSect.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen, jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix));
                    addSect.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                        SketchUpGlobals.Record, //0
                        SketchUpGlobals.Card, // 1
                        NextSectLtr.Trim(), // 2
                        lineCnt, //3
                        direction.Trim(), //4
                        lengthX, //5
                        lengthY, //6
                        distD, //7
                        0, //8
                        rndTst1,  // 9 jlpt1x  tst1
                        rndTst2,  // 10 jlpt1y  tst2
                        rndPt2X,  // 11 jlpt2x tst1x
                        rndPt2Y,  // 12 jlpt2y tst2y
                        " " //13
                        ));

                    NextStartX = (float)rndPt2X;
                    NextStartY = (float)rndPt2Y;

                    if (_undoLine == false)
                    {
                        if (Math.Abs(lengthX) > 0 || Math.Abs(lengthY) > 0)
                        {
                            try
                            {
                                dbConn.DBConnection.ExecuteNonSelectStatement(addSect.ToString());
                            }
                            catch (Exception ex)
                            {
                                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                                Console.WriteLine(errMessage);
                                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                                MessageBox.Show(errMessage);
#endif
                            }
                        }
                    }
                }

                if (_undoLine == true)
                {
                    _undoLine = false;
                }
            }
        }

        private void BuildAddSQLAng(float prevX, float prevY, decimal distDX, decimal distDY, string direction, decimal length, int _lineCnt, bool closing, float startx, float starty)
        {
            _isclosing = closing;
            LastAngDir = direction;

            pt2X = 0;
            pt2Y = 0;

            Xadj = (((ScaleBaseX - prevX) / _currentScale) * -1);
            Yadj = (((ScaleBaseY - prevY) / _currentScale) * -1);

            NewSectionPoints.Add(new PointF(Xadj, Yadj));

            decimal xw2 = Convert.ToDecimal(Xadj);

            decimal yw2 = Convert.ToDecimal(Yadj);

            decimal XadjD = Math.Round((Convert.ToDecimal(Xadj)), 1);

            decimal YadjD = Math.Round((Convert.ToDecimal(Yadj)), 1);

            Xadj = (float)XadjD;
            Yadj = (float)YadjD;

            if (NextStartX != Xadj)
            {
                Xadj = NextStartX;
            }
            if (NextStartY != Yadj)
            {
                Yadj = NextStartY;
            }

            decimal lengthX = 0;
            decimal lengthY = 0;
            if (direction == "NE")
            {
                lengthX = distDX;
                lengthY = distDY;

                decimal pt2XD = Math.Round((Convert.ToDecimal(Xadj) + distDX), 1);

                decimal pt2YD = Math.Round((Convert.ToDecimal(Yadj) - distDY), 1);

                pt2X = Xadj + Convert.ToInt32(distDX);
                pt2Y = Yadj - Convert.ToInt32(distDY);

                pt2X = (float)pt2XD;
                pt2Y = (float)pt2YD;

                NextStartX = pt2X;
                NextStartY = pt2Y;
            }
            if (direction == "NW")
            {
                lengthX = distDX;
                lengthY = distDY;

                decimal pt2XD = Convert.ToDecimal(Xadj) - distDX;

                decimal pt2YD = Convert.ToDecimal(Yadj) - distDY;

                pt2X = Xadj - Convert.ToInt16(distDX);
                pt2Y = Yadj - Convert.ToInt16(distDY);

                pt2X = (float)pt2XD;
                pt2Y = (float)pt2YD;

                NextStartX = pt2X;
                NextStartY = pt2Y;
            }
            if (direction == "SE")
            {
                lengthX = distDX;
                lengthY = distDY;

                decimal pt2XD = Convert.ToDecimal(Xadj) + distDX;

                decimal pt2YD = Convert.ToDecimal(Yadj) + distDY;

                pt2X = Xadj + Convert.ToInt16(distDX);
                pt2Y = Yadj + Convert.ToInt16(distDY);

                pt2X = (float)pt2XD;
                pt2Y = (float)pt2YD;

                NextStartX = pt2X;
                NextStartY = pt2Y;
            }
            if (direction == "SW")
            {
                lengthX = distDX;
                lengthY = distDY;

                decimal pt2XD = Convert.ToDecimal(Xadj) - distDX;

                decimal pt2YD = Convert.ToDecimal(Yadj) + distDY;

                pt2X = Xadj - Convert.ToInt16(distDX);
                pt2Y = Yadj + Convert.ToInt16(distDY);

                pt2X = (float)pt2XD;
                pt2Y = (float)pt2YD;

                NextStartX = pt2X;
                NextStartY = pt2Y;
            }

            if (draw)
            {
                if (_lineCnt == 1)
                {
                    if (_hasNewSketch == true)
                    {
                        Xadj = 0;
                        Yadj = 0;
                    }

                    SecBeginX = Xadj;
                    SecBeginY = Yadj;
                }

                decimal Tst1 = Convert.ToDecimal(Xadj);
                decimal Tst2 = Convert.ToDecimal(Yadj);
                decimal Ptx = Convert.ToDecimal(pt2X);
                decimal Pty = Convert.ToDecimal(pt2Y);

                var rndTst1 = Math.Round(Tst1, 1);
                var rndTst2 = Math.Round(Tst2, 1);
                var rndPt2X = Math.Round(Ptx, 1);
                var rndPt2Y = Math.Round(Pty, 1);

                if (_hasNewSketch == true && _lineCnt == 1)
                {
                    if (direction == "NE")
                    {
                        rndTst1 = 0;
                        rndTst2 = 0;

                        rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "SE")
                    {
                        rndTst1 = 0;
                        rndTst2 = 0;

                        rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "NW")
                    {
                        rndTst1 = 0;
                        rndTst2 = 0;

                        rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "SW")
                    {
                        rndTst1 = 0;
                        rndTst2 = 0;

                        rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }

                    decimal TprevTst1 = prevTst1;
                    decimal TprevTst2 = prevTst2;
                    decimal TprevPt2X = prevPt2X;
                    decimal TprevPt2Y = prevPt2Y;
                }

                if (_hasNewSketch == true && _lineCnt > 1)
                {
                    decimal TprevTst1 = prevTst1;
                    decimal TprevTst2 = prevTst2;
                    decimal TprevPt2X = prevPt2X;
                    decimal TprevPt2Y = prevPt2Y;

                    if (direction == "NE")
                    {
                        rndTst1 = prevTst1;
                        rndTst2 = prevTst2;

                        rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "SE")
                    {
                        rndTst1 = prevTst1;
                        rndTst2 = prevTst2;

                        rndPt2X = Convert.ToDecimal(rndTst1 + (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "NW")
                    {
                        rndTst1 = prevTst1;
                        rndTst2 = prevTst2;

                        rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 - (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                    if (direction == "SW")
                    {
                        rndTst1 = prevTst1;
                        rndTst2 = prevTst2;

                        rndPt2X = Convert.ToDecimal(rndTst1 - (Convert.ToDecimal(lengthX)));

                        rndPt2Y = Convert.ToDecimal(rndTst2 + (Convert.ToDecimal(lengthY)));

                        prevPt2X = rndPt2X;
                        prevPt2Y = rndPt2Y;
                        prevTst1 = rndPt2X;
                        prevTst2 = rndPt2Y;
                    }
                }

                StringBuilder mxline2 = new StringBuilder();
                mxline2.Append(String.Format("select max(jlline#) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                              SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                SketchUpGlobals.Record,
                                SketchUpGlobals.Card,
                                NextSectLtr));

                try
                {
                    _curLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(mxline2.ToString()));
                }
                catch
                {
                }

                if (_curLineCnt == 0)
                {
                    _curLineCnt = 0;
                }

                _lineCnt = _curLineCnt;

                lineCnt = _curLineCnt + 1;

                if (lineCnt == 19)
                {
                    MessageBox.Show("Next Line will Max Section Lines", "Line Count Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                if (lineCnt > sketchBoxPaddingTotal)
                {
                    MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                if (_isclosing == true)
                {
                    var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
                    var sectionArea = sectionPolygon.Area;

                    if (_nextStoryHeight < 1.0m)
                    {
                        NextSectArea = (Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1) * _nextStoryHeight);
                    }
                    if (_nextStoryHeight >= 1.0m)
                    {
                        NextSectArea = Math.Round(Convert.ToDecimal(sectionPolygon.Area), 1);
                    }
                }

                int checkcnt = lineCnt;

                if (lineCnt <= sketchBoxPaddingTotal)
                {
                    //MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2416", SketchUpGlobals.Record, SketchUpGlobals.Card));

                    StringBuilder addSectAng = new StringBuilder();
                    addSectAng.Append(String.Format("insert into {0}.{1}line ( jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
                                 SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix
                                ));
                    addSectAng.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card,
                        NextSectLtr.Trim(),
                        lineCnt,
                        direction.Trim(),
                        lengthX,
                        lengthY,
                        length,
                        0,
                        rndTst1,  // jlpt1x  tst1 /// 27.0
                        rndTst2,  // jlpt1y  tst2 //// 0
                        rndPt2X,  // jlpt2x tst1x ///// 13.5
                        rndPt2Y,  // jlpt2y tst2y //// 3.0
                        " "));

                    NextStartX = (float)rndPt2X;
                    NextStartY = (float)rndPt2Y;

                    if (_undoLine == false)
                    {
                        if (Math.Abs(lengthX) > 0 || Math.Abs(lengthY) > 0)
                        {
                            dbConn.DBConnection.ExecuteNonSelectStatement(addSectAng.ToString());
                        }
                    }
                }

                if (_undoLine == true)
                {
                    _undoLine = false;
                }
            }

            DistText.Focus();
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                Image returnImage = Image.FromStream(ms);
                return returnImage;
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
                          SketchUpGlobals.LocalityPrefix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card));
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

        private void changeSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void CompleteDrawingNewSection()
        {
            try
            {
                UnsavedChangesExist = true;
                AttachNewSectionToSketch();
                RefreshWorkspace();
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

        private void ComputeSectionArea()
        {
            var sectionPolygon = new PolygonF(NewSectionPoints.ToArray());
            var sectionArea = sectionPolygon.Area;

            calculateNewArea(SketchUpGlobals.Record, SketchUpGlobals.Card, NextSectLtr);

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

        private void ConfirmCarportNumbers()
        {
            if (carportCount > 0)
            {
                if (carportCount > 0 && parcelMast.CarportNumCars == 0 || carportCount > 0 && parcelMast.CarportTypeCode == 67)
                {
                    MissingGarageData missCP = new MissingGarageData(parcelMast, CPSize, "CP");
                    missCP.ShowDialog();
                }

                if (carportCount > 1 && parcelMast.CarportTypeCode != 0 || carportCount > 1 && parcelMast.CarportTypeCode != 67)
                {
                    MissingGarageData missCPx = new MissingGarageData(parcelMast, CPSize, "CP");
                    missCPx.ShowDialog();

                    parcelMast.CarportNumCars += MissingGarageData.CpNbr;
                }
            }
        }

        private void ConfirmGarageNumbers(SMParcel originalParcel)
        {
            if (Garcnt > 0)
            {
                if (Garcnt == 1 && parcelMast.Garage1TypeCode <= 60 || Garcnt == 1 && parcelMast.Garage1TypeCode == 63 || Garcnt == 1 && parcelMast.Garage1TypeCode == 64)
                {
                    MissingGarageData missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (MissingGarageData.GarCode != originalParcel.ParcelMast.Garage1TypeCode)
                    {
                        parcelMast.Garage1NumCars = MissingGarageData.GarNbr;
                        parcelMast.Garage1TypeCode = MissingGarageData.GarCode;
                    }
                }
                if (Garcnt > 1 && parcelMast.Garage1NumCars == 0)
                {
                    MissingGarageData missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    if (MissingGarageData.GarCode != originalParcel.ParcelMast.Garage1TypeCode)
                    {
                        parcelMast.Garage1NumCars = MissingGarageData.GarNbr;
                        parcelMast.Garage1TypeCode = MissingGarageData.GarCode;
                    }
                }
                if (Garcnt > 2)
                {
                    MissingGarageData missGar = new MissingGarageData(parcelMast, GarSize, "GAR");
                    missGar.ShowDialog();

                    parcelMast.Garage2NumCars += MissingGarageData.GarNbr;
                }
            }
        }

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
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
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

        private int CountLines(string thisSection)
        {
            string lineCountSql = string.Format("select count(*) from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card, thisSection);

            int lineCount = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCountSql.ToString()));
            return lineCount;
        }

        private int CountSections()
        {
            try
            {
                string seccnt = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card);

                int secItemCnt = 0;
                Int32.TryParse(dbConn.DBConnection.ExecuteScalar(seccnt).ToString(), out secItemCnt);
                return secItemCnt;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));

                throw;
            }
        }

        private void deleteExistingSketchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //_deleteThisSketch = false;
            //_deleteMaster = true;
            //DialogResult result;
            //result = (MessageBox.Show("Do you REALLY want to Delete this entire Sketch", "Delete Existing Sketch Warning",
            //    MessageBoxButtons.YesNo, MessageBoxIcon.Warning));
            //if (result == DialogResult.Yes)
            //{
            //    DialogResult finalChk;
            //    finalChk = (MessageBox.Show("Are you Sure", "Final Delete Sketch Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

            //    if (finalChk == DialogResult.Yes)
            //    {
            //        StringBuilder delSect = new StringBuilder();
            //        delSect.Append(String.Format("delete from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
            //                  SketchUpGlobals.LocalLib,
            //              SketchUpGlobals.LocalityPreFix,

            //                    //SketchUpGlobals.FcLib,
            //                    //SketchUpGlobals.FcLocalityPrefix,
            //                    SketchUpGlobals.Record,
            //                    SketchUpGlobals.Card));

            //        dbConn.DBConnection.ExecuteNonSelectStatement(delSect.ToString());

            //        StringBuilder delLine = new StringBuilder();
            //        delLine.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
            //                       SketchUpGlobals.LocalLib,
            //              SketchUpGlobals.LocalityPreFix,

            //                       //SketchUpGlobals.FcLib,
            //                       //SketchUpGlobals.FcLocalityPrefix,
            //                       SketchUpGlobals.Record,
            //                       SketchUpGlobals.Card));

            //        dbConn.DBConnection.ExecuteNonSelectStatement(delLine.ToString());

            //        StringBuilder delmaster = new StringBuilder();
            //        delmaster.Append(String.Format("delete from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
            //                  SketchUpGlobals.LocalLib,
            //              SketchUpGlobals.LocalityPreFix,

            //                   //SketchUpGlobals.FcLib,
            //                   //SketchUpGlobals.FcLocalityPrefix,
            //                   SketchUpGlobals.Record,
            //                   SketchUpGlobals.Card));

            //        dbConn.DBConnection.ExecuteNonSelectStatement(delmaster.ToString());
            //    }
            //    if (finalChk == DialogResult.No)
            //    {
            //    }

            //    RefreshEditImageBtn = true;
            //    _deleteThisSketch = true;
            //    _isClosed = true;

            //    DialogResult makeVacant;
            //    makeVacant = (MessageBox.Show("Do you want to clear Master File", "Clear Master File Question",
            //                    MessageBoxButtons.YesNo, MessageBoxIcon.Question));

            //    if (makeVacant == DialogResult.Yes)
            //    {
            //        StringBuilder clrMast2 = new StringBuilder();
            //        clrMast2.Append(String.Format("update {0}.{1}mast set moccup = 15, mstory = ' ', mage = 0, mcond = ' ', mclass = ' ', ",
            //                   SketchUpGlobals.LocalLib,
            //                   SketchUpGlobals.LocalityPreFix

            //                    //SketchUpGlobals.FcLib,
            //                    //SketchUpGlobals.FcLocalityPrefix
            //                    ));
            //        clrMast2.Append(" mfactr = 0, mdeprc = 0, mfound = 0, mexwll = 0, mrooft = 0, mroofg = 0, m#dunt = 0, m#room = 0, m#br = 0, m#fbth = 0, m#hbth = 0 , mswl = 0, ");
            //        clrMast2.Append(" mfp2 = ' ', mheat = 0, mfuel = 0, mac = ' ', mfp1 = ' ', mekit = 0, mbastp = 0, mpbtot = 0, msbtot = 0, mpbfin = 0, msbfin = 0, mbrate = 0, ");
            //        clrMast2.Append(" m#flue = 0, mflutp = ' ', mgart = 0, mgar#c = 0, mcarpt = 0, mcar#c = 0, mbi#c = 0, mgart2 = 0, mgar#2 = 0, macpct = 0, m0depr = ' ',meffag = 0, ");
            //        clrMast2.Append(" mfairv = 0, mexwl2 = 0, mtbv = 0, mtbas = 0, mtfbas = 0, mtplum = 0, mtheat = 0, mtac = 0, mtfp = 0, mtfl = 0 , mtbi = 0 , mttadd = 0 , mnbadj = 0, ");
            //        clrMast2.Append(" mtsubt = 0, mtotbv = 0, mbasa = 0, mtota = 0, mpsf = 0, minwll = ' ', mfloor = ' ', myrblt = 0, mpcomp = 0, mfuncd = 0, mecond = 0, mimadj = 0, ");
            //        clrMast2.Append(" mtbimp = 0, mcvexp = 'Improvement Deleted', mqapch = 0, mqafil = ' ', mfp# = 0, msfp# = 0, mfl#= 0, msfl# = 0, mmfl# = 0, miofp# = 0,mstor# = 0, ");
            //        clrMast2.Append(String.Format(" moldoc = {0}, ", _currentParcel.orig_moccup));
            //        clrMast2.Append(String.Format(" mcvmo = {0}, mcvda = {1}, mcvyr = {2} ",
            //                  SketchUpGlobals.Month,
            //                  SketchUpGlobals.TodayDayNumber,
            //                  SketchUpGlobals.Year

            //                    //MainForm.Month,
            //                    // MainForm.Today,
            //                    // MainForm.Year
            //                    ));
            //        clrMast2.Append(String.Format(" where mrecno = {0} and mdwell = {1} ", SketchUpGlobals.Record, SketchUpGlobals.Card));

            //        dbConn.DBConnection.ExecuteNonSelectStatement(clrMast2.ToString());

            //        if (_currentParcel.GasLogFP > 0)
            //        {
            //            StringBuilder clrGasLg = new StringBuilder();
            //            clrGasLg.Append(String.Format("update {0}.{1}gaslg set gnogas = 0 where grecno = {2} and gdwell = {3} ",
            //               SketchUpGlobals.LocalLib,
            //              SketchUpGlobals.LocalityPreFix,

            //                //SketchUpGlobals.FcLib,
            //                //SketchUpGlobals.FcLocalityPrefix,
            //                SketchUpGlobals.Record,
            //                SketchUpGlobals.Card));

            //            dbConn.DBConnection.ExecuteNonSelectStatement(clrGasLg.ToString());
            //        }
            //    }
            //    if (makeVacant == DialogResult.No)
            //    {
            //    }
            //}
            //if (result == DialogResult.No)
            //{
            //}
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void DeleteLineSection()
        {
            StringBuilder deletelinesect = new StringBuilder();
            deletelinesect.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                           SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            CurrentSecLtr));

            dbConn.DBConnection.ExecuteNonSelectStatement(deletelinesect.ToString());
        }

        private void DeleteSketch()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void DeleteWorkingSectionAndRevert()
        {
            try
            {
                WorkingParcel = SketchUpGlobals.SMParcelFromData;
                SketchUpGlobals.SketchSnapshots.Clear();
                WorkingParcel.SnapShotIndex = 0;
                AddParcelToSnapshots(WorkingParcel);
                WorkingParcel.SnapShotIndex++;
                AddParcelToSnapshots(WorkingParcel);
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

        private CamraDataEnums.CardinalDirection DirectionOfKeyEntered(KeyEventArgs e)
        {
            CamraDataEnums.CardinalDirection keyDirection;
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.E:
                case Keys.R:
                    keyDirection = CamraDataEnums.CardinalDirection.E;

                    break;

                case Keys.Left:
                case Keys.L:
                case Keys.W:
                    keyDirection = CamraDataEnums.CardinalDirection.W;

                    break;

                case Keys.Up:
                case Keys.N:
                case Keys.U:
                    keyDirection = CamraDataEnums.CardinalDirection.N;

                    break;

                case Keys.Down:
                case Keys.D:
                case Keys.S:
                    keyDirection = CamraDataEnums.CardinalDirection.S;

                    break;

                default:
                    keyDirection = CamraDataEnums.CardinalDirection.None;

                    break;
            }
            return keyDirection;
        }

        private void DiscardChangesAndExit()
        {
            SketchUpGlobals.SketchSnapshots.Clear();
            SketchUpGlobals.SMParcelFromData.SnapShotIndex = 0;
            AddParcelToSnapshots(SketchUpGlobals.SMParcelFromData);

            MessageBox.Show("Reverted to database version...", "Changes Discarded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UnsavedChangesExist = false;
        }

        private void DisplayStatus(string statusText)
        {
            sketchStatusMessage.Text = statusText;
        }

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
                _isKeyValid = true;
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
            //if (jumpMode == true)
            //{
            //    _isJumpMode = true;
            //    draw = false;
            //    _mouseX = X;
            //    _mouseY = Y;
            //}
            //else
            //{
            //    _isJumpMode = false;
            //    draw = true;
            //    Graphics g = Graphics.FromImage(MainImage);
            //    Pen pen1 = new Pen(Color.White, 4);
            //    g.DrawRectangle(pen1, X, Y, 1, 1);
            //    g.Save();

            //    ExpSketchPBox.Image = MainImage;

            //click++;
            //savpic.Add(click, imageToByteArray(_mainimage));
            //}
        }

        //    legalDirections.AddRange((from l in WorkingParcel.AllSectionLines where l.ScaledEndPoint == scaledJumpPoint  select ReverseDirection(l.Direction)).ToList());
        //    return legalDirections.Distinct().ToList();
        //}
        //ToDo: Begin here to hook in parcel updates
        private void DrawingDoneBtn_Click(object sender, EventArgs e)
        {
            CompleteDrawingNewSection();
        }

       

        private void EraseSectionFromDrawing(SMSection section)
        {
            WorkingParcel.SnapShotIndex++;
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.Sections.Remove(section);
            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = StopClose();
        }

        private void ExpandoSketch_Shown(object sender, EventArgs e)
        {
        }

        private void ExpandoSketchTools_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
        private void ExpSketchPbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isJumpMode)
            {
                _mouseX = e.X;
                _mouseY = e.Y;
            }
        }

        private void ExpSketchPbox_MouseDown(object sender, MouseEventArgs e)
        {
            _isJumpMode = false;
            if (e.Button == MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;
                _mouseX = e.X;
                _mouseY = e.Y;
                DMouseMove(e.X, e.Y, true);
            }
        }

        private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == MouseButtons.Right)
            {
                _isJumpMode = true;

                _mouseX = e.X;
                _mouseY = e.Y;
                DMouseMove(e.X, e.Y, true);
            }
        }

        private void FlipHorizontal()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapShotIndex++;
            CamraDataEnums.CardinalDirection dir;
            string newDir = string.Empty;
            foreach (SMLine l in WorkingParcel.AllSectionLines)
            {
                dir = SMGlobal.DirectionFromString(l.Direction);

                switch (dir)
                {
                    case CamraDataEnums.CardinalDirection.N:
                        newDir = CamraDataEnums.CardinalDirection.N.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NE:
                        newDir = CamraDataEnums.CardinalDirection.NW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.E:
                        newDir = CamraDataEnums.CardinalDirection.W.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SE:
                        newDir = CamraDataEnums.CardinalDirection.SW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.S:
                        newDir = CamraDataEnums.CardinalDirection.S.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SW:
                        newDir = CamraDataEnums.CardinalDirection.SE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.W:
                        newDir = CamraDataEnums.CardinalDirection.E.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NW:
                        newDir = CamraDataEnums.CardinalDirection.NE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.None:
                        break;

                    default:
                        newDir = string.Empty;
                        break;
                }
                l.Direction = newDir;
                l.StartX = l.StartX * -1M;
                l.EndX = l.EndX * -1M;
            }
            AddParcelToSnapshots(WorkingParcel);

            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private void FlipUpDown()
        {
            AddParcelToSnapshots(WorkingParcel);
            WorkingParcel.SnapShotIndex++;
            CamraDataEnums.CardinalDirection dir;
            string newDir = string.Empty;
            foreach (SMLine l in WorkingParcel.AllSectionLines)
            {
                dir = SMGlobal.DirectionFromString(l.Direction);

                switch (dir)
                {
                    case CamraDataEnums.CardinalDirection.N:
                        newDir = CamraDataEnums.CardinalDirection.S.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NE:
                        newDir = CamraDataEnums.CardinalDirection.SE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.E:
                        newDir = CamraDataEnums.CardinalDirection.E.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SE:
                        newDir = CamraDataEnums.CardinalDirection.NE.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.S:
                        newDir = CamraDataEnums.CardinalDirection.N.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.SW:
                        newDir = CamraDataEnums.CardinalDirection.NW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.W:
                        newDir = CamraDataEnums.CardinalDirection.W.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.NW:
                        newDir = CamraDataEnums.CardinalDirection.SW.ToString();
                        break;

                    case CamraDataEnums.CardinalDirection.None:
                        break;

                    default:
                        newDir = string.Empty;
                        break;
                }
                l.Direction = newDir;
                l.StartY = l.StartY * -1M;
                l.EndY = l.EndY * -1M;
            }
            AddParcelToSnapshots(WorkingParcel);
            SMSketcher sms = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
            sms.RenderSketch();
            sketchBox.Image = sms.SketchImage;
        }

        private AngleVector GetAngleLine(string distance, CamraDataEnums.CardinalDirection direction)
        {
            _isAngle = true;
            AngleVector angle = new AngleVector();
            decimal xDistance = 0.00M;
            decimal yDistance = 0.00M;
            angle = ParseAngleEntry(distance);
            xDistance = angle.XLength;
            yDistance = angle.YLength;
            AdjustLengthDirection(direction, ref xDistance, ref yDistance);
            return angle;
        }

        //    AngD2 = Convert.ToDecimal(D1);
        private List<string> GetLegalMoveDirections(PointF startPoint, string attachSectionLetter)
        {
            List<string> legalDirections = new List<string>();
            List<string> startPointLineDirections = new List<string>();
            List<string> endPointLineDirections = new List<string>();

            try
            {
                List<SMLine> linesWithJumpPoint = (from l in WorkingParcel.AllSectionLines where l.SectionLetter == attachSectionLetter && (l.ScaledStartPoint == startPoint || l.ScaledEndPoint == startPoint) select l).ToList();

                startPointLineDirections.AddRange((from l in linesWithJumpPoint where l.ScaledStartPoint == startPoint select l.Direction).ToList());
                endPointLineDirections.AddRange((from l in linesWithJumpPoint where l.ScaledEndPoint == startPoint && l.SectionLetter == attachSectionLetter select SMGlobal.ReverseDirection(l.Direction)).ToList());
                legalDirections.AddRange(startPointLineDirections.Distinct());
                legalDirections.AddRange(endPointLineDirections.Distinct());

                string statusMessage = LegalDirectionsMessage(legalDirections);

                sketchStatusMessage.Text = statusMessage.ToString();
                return legalDirections.Distinct().ToList();
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

        private DataSet GetLinesData(int crrec, int crcard)
        {
            DataSet lines;
            string getLine = string.Format("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, crrec, crcard);

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
                       SketchUpGlobals.LocalityPrefix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        crrec,
                        crcard));
            getLine.Append("and jldirect <> 'X' ");

            lines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());
            return lines;
        }

        private int GetSectionsCount()
        {
            StringBuilder checkSect = new StringBuilder();
            checkSect.Append(String.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            NextSectLtr));

            int secCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(checkSect.ToString()));
            return secCnt;
        }

        private void GetSectionTypeInfo()
        {
            _addSection = true;

            string nextSectionLetter = WorkingParcel.NextSectionLetter;
            NewSectionPoints.Clear();
            lineCnt = 0;
            SelectSectionTypeDialog sectionTypeForm = new SelectSectionTypeDialog(WorkingParcel.ParcelMast, _addSection, lineCnt, IsNewSketch);

            sectionTypeForm.ShowDialog(this);
            if (sectionTypeForm.SectionWasAdded)

            {
                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
                NextSectLtr = WorkingParcel.NextSectionLetter;
                _nextSectType = SelectSectionTypeDialog._nextSectType;
                _nextStoryHeight = SelectSectionTypeDialog.newSectionStoreys;
                _nextLineCount = SelectSectionTypeDialog._nextLineCount;
                _hasNewSketch = (NextSectLtr == "A");

                AddSectionContextMenu.Enabled = true;
                cmiJumpToCorner.Enabled = true;
                EditState = DrawingState.SectionAdded;
                _isJumpMode = true;
                try
                {
                    FieldText.Text = String.Format("Sect- {0}, {1} sty {2}", NextSectLtr.Trim(), _nextStoryHeight.ToString("N2"), _nextSectType.Trim());
                }
                catch
                {
                }
            }
            else
            {
                AddSectionContextMenu.Enabled = false;
                cmiJumpToCorner.Enabled = false;
                EditState = DrawingState.SketchLoaded;
            }
        }

        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            CamraDataEnums.CardinalDirection direction;
            direction = DirectionOfKeyEntered(e);
            string distanceText = string.Empty;
            decimal distanceValueEntered = 0.00M;
            decimal yLength = 0.00M;
            decimal xLength = 0.00M;
            AngleVector angle = new AngleVector();
            if (!string.IsNullOrEmpty(DistText.Text))
            {
                distanceText = DistText.Text;

                if (distanceText.IndexOf(",") > 0)
                {
                    angle = GetAngleLine(distanceText, direction);
                    xLength = angle.XLength;
                    yLength = angle.YLength;
                }
                else
                {
                    decimal.TryParse(distanceText, out distanceValueEntered);
                    angle = ParseNEWSLine(distanceText, direction);
                    xLength = angle.XLength;
                    yLength = angle.YLength;
                }

                AdjustLengthDirection(direction, ref xLength, ref yLength);
                DistText.Text = string.Empty;
                DistText.Focus();

                switch (EditState)
                {
                    case DrawingState.Drawing:
                        AddLineToSketch(direction, xLength, yLength, DbStartX, DbStartY);
                        WorkingParcel.DrawOnlyLines.Clear();

                        RedrawSketch(WorkingParcel);
                        if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
                        {
                            SMLine newLine = (from l in WorkingSection.Lines.OrderBy(l => l.LineNumber) select l).LastOrDefault();
                            DbStartPoint = newLine.EndPoint;
                            dbStartX = newLine.EndX;
                            dbStartY = newLine.EndY;
                            ScaledStartPoint = newLine.ScaledEndPoint;

                            // TODO: Remove if not needed:
                            StartOfCurrentLine = ScaledStartPoint;
                            btnDrawingDone.Enabled = WorkingSection.SectionIsClosed;
                        }

                        break;

                    case DrawingState.JumpPointSelected:
                        AddJumpLineToSketch(direction, xLength, yLength, DbStartX, DbStartY);
                        RedrawSketch(WorkingParcel);
                        if (WorkingParcel.DrawOnlyLines != null && WorkingParcel.DrawOnlyLines.Count > 0)
                        {
                            DrawOnlyLine newLine = (from l in WorkingParcel.DrawOnlyLines.OrderBy(l => l.LineNumber) select l).LastOrDefault();
                            DbStartPoint = newLine.EndPoint;
                            dbStartX = newLine.EndX;
                            dbStartY = newLine.EndY;
                            ScaledStartPoint = newLine.ScaledEndPoint;

                            // TODO: Remove if not needed:
                            StartOfCurrentLine = ScaledStartPoint;
                        }

                        break;

                    case DrawingState.SectionAdded:
                    case DrawingState.SketchLoaded:

                    default:
                        _isKeyValid = false;
                        break;
                }
            }
        }

        private void HandleNonArrowKeys(KeyEventArgs e)
        {
            bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
            if (notNumPad)
            {
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
            }
        }

        private void InitializeDataTablesAndVariables(string sketchFolder, string sketchRecord, string sketchCard, bool hasSketch, bool hasNewSketch)
        {
            IsNewSketch = false;
            _hasNewSketch = hasNewSketch;
            IsNewSketch = hasNewSketch;
            _addSection = false;

            SketchFolder = sketchFolder;
            SketchRecord = sketchRecord;
            SketchCard = sketchCard;
            SketchUpGlobals.HasSketch = hasSketch;

            // TODO: Remove if not needed:
            //SectionLtrs = ConstructSectionLtrs();
            //AreaTable = ConstructAreaTable();
            //MultiplePoints = ConstructMulPtsTable();
            //AttachmentPointsDataTable = ConstructAttachmentPointsDataTable();
            //AttachPoints = ConstructAttachPointsDataTable();
            //DupAttPoints = ConstructDupAttPointsTable();
            //StrtPts = ConstructStartPointsTable();

            //                case CamraDataEnums.CardinalDirection.None:
            //                    break;
            //    DupAttPoints = ConstructDupAttPointsTable();
        }

        private void InitializeDisplayDataGrid()
        {
            displayDataTable = ConstructDisplayDataTable();

            dgSections.DataSource = displayDataTable;
        }

        private void InsertLine(string CurAttDir, decimal newEndX, decimal newEndY, decimal StartEndX, decimal StartEndY, decimal splitLength)
        {
            StringBuilder insertLine = new StringBuilder();
            insertLine.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen, ",
                      SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
            insertLine.Append("jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ) values ( ");
            insertLine.Append(String.Format(" {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' )", SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr,
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

        private void InsertMasterRecord(decimal summedArea, decimal baseStory, DataSet ds_master)
        {
            if (ds_master.Tables[0].Rows.Count == 0)
            {
                StringBuilder insMaster = new StringBuilder();
                insMaster.Append(String.Format("insert into {0}.{1}master (jmrecord,jmdwell,jmsketch,jmstory,jmstoryex,jmscale,jmtotsqft,jmesketch) ",
                              SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPrefix

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix
                                ));
                insMaster.Append(String.Format("values ({0},{1},'{2}',{3},'{4}',{5},{6},'{7}' ) ",
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            "Y",
                            baseStory,
                            String.Empty,
                            1.00,
                            summedArea,
                            String.Empty));

                dbConn.DBConnection.ExecuteNonSelectStatement(insMaster.ToString());
            }
        }

        private bool IsValidDirection(string moveDirection)
        {
            bool goodDir = (LegalMoveDirections.Contains(moveDirection) || btnBeginSection.Text == "Active" || !checkDirection);
            return goodDir;
        }

        private void JumptoCorner()
        {
            decimal scale = WorkingParcel.Scale;
            PointF origin = WorkingParcel.SketchOrigin;
            CurrentSecLtr = String.Empty;
            // TODO: Remove if not needed:	_newIndex = 0;
            currentAttachmentLine = 0;
            if (IsNewSketch == false)
            {
                PointF mouseLocation = new PointF(_mouseX, _mouseY);

                List<SMLine> connectionLines = LinesWithClosestEndpoints(mouseLocation);
                bool sketchHasLineData = (connectionLines.Count > 0);
                if (connectionLines == null || connectionLines.Count == 0)
                {
                    string message = string.Format("No lines contain an available connection point from point {0},{1}", _mouseX, _mouseY);

                    Console.WriteLine(message);

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
                        AttachmentSection = (from s in WorkingParcel.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = (from l in connectionLines where l.SectionLetter == AttSectLtr select l).FirstOrDefault();
                        currentAttachmentLine = JumpPointLine.LineNumber;
                        CurrentSecLtr = AttachmentSection.SectionLetter;
                    }
                    else
                    {
                        AttSectLtr = SecLetters[0];
                        AttachmentSection = (from s in WorkingParcel.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = connectionLines[0];
                        currentAttachmentLine = JumpPointLine.LineNumber;
                        CurrentSecLtr = AttachmentSection.SectionLetter;
                    }

                    ScaledJumpPoint = JumpPointLine.ScaledEndPoint;
                    DbJumpPoint = JumpPointLine.EndPoint;
                    ScaledStartPoint = ScaledJumpPoint;
                    LegalMoveDirections = GetLegalMoveDirections(ScaledJumpPoint, AttSectLtr);
                    MoveCursorToNewPoint(ScaledJumpPoint);
                    LoadLegacyJumpTable();
                    SetReadyButtonAppearance();
                    _isJumpMode = true;
                    EditState = DrawingState.JumpPointSelected;
                    ScaledStartPoint = ScaledJumpPoint;
                    DbStartX = Math.Round((decimal)DbJumpPoint.X, 2);
                    DbStartY = Math.Round((decimal)DbJumpPoint.Y, 2);

                    DistText.Focus();
                }
            }
        }

        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isJumpMode || EditState == DrawingState.SectionAdded)
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
        //private void AddDbPoints(PointF startPoint, PointF endPoint, decimal xDistance, decimal yDistance, CamraDataEnums.CardinalDirection moveDirection)
        //{
        //    int nextLineNumber;
        //    PointF dbStart;
        //    PointF dbEnd;

        //    dbStart = SMGlobal.ScaledPointToDbPoint((decimal)startPoint.X, (decimal)startPoint.Y, WorkingParcel.Scale, WorkingParcel.SketchOrigin);
        //    dbEnd = SMGlobal.ScaledPointToDbPoint((decimal)startPoint.X, (decimal)startPoint.Y, WorkingParcel.Scale, WorkingParcel.SketchOrigin);

        //    string direction = moveDirection.ToString();
        //    nextLineNumber = WorkingSection.Lines.Count + 1;
        //    AddNewLine(nextLineNumber, dbStart, dbEnd, direction);
        //}
        //                    break;
        private List<SMLine> LinesWithClosestEndpoints(PointF mouseLocation)
        {
            foreach (SMLine l in WorkingParcel.AllSectionLines.Where(s => s.SectionLetter != WorkingParcel.LastSectionLetter))
            {
                l.ComparisonPoint = mouseLocation;
            }

            decimal shortestDistance = Math.Round((from l in WorkingParcel.AllSectionLines select l.EndPointDistanceFromComparisonPoint).Min(), 2);
            List<SMLine> connectionLines = (from l in WorkingParcel.AllSectionLines where Math.Round(l.EndPointDistanceFromComparisonPoint, 2) == shortestDistance select l).ToList();

            return connectionLines;
        }

        //                case CamraDataEnums.CardinalDirection.S:
        //                    endX = ScaledStartPoint.X;
        //                    endY = ScaledStartPoint.Y + (float)scaledDistance;
        //    REJumpTable = ConstructREJumpTable();
        private void LoadAttachmentPointsDataTable()
        {
            AttachmentPointsDataTable.Clear();
            List<SMLine> linesList = (from l in WorkingParcel.AllSectionLines where l.SectionLetter != "A" orderby l.SectionLetter, l.LineNumber select l).ToList();
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

        //                    break;
        //    ConstructJumpTable();
        private void LoadLegacyJumpTable()
        {
            JumpTable = ConstructJumpTable();
            JumpTable.Clear();
            AddListItemsToJumpTableList(ScaledJumpPoint.X, ScaledJumpPoint.Y, WorkingParcel.Scale, WorkingParcel.AllSectionLines);
        }

        //                case CamraDataEnums.CardinalDirection.E:
        //                    endX = ScaledStartPoint.X + (float)scaledDistance;
        //                    endY = ScaledStartPoint.Y;
        //    SectionTable = ConstructSectionTable();
        private void LoadSectionLinesGrid(string sectionLetter)
        {
            displayDataTable.Rows.Clear();
            if (WorkingParcel.SelectSectionByLetter(sectionLetter).Lines != null)
            {
                foreach (SMLine line in WorkingParcel.SelectSectionByLetter(sectionLetter).Lines)
                {
                    DataRow dr = displayDataTable.NewRow();
                    dr["Dir"] = line.Direction.Trim();
                    dr["North"] = line.YLength.ToString();
                    dr["East"] = line.XLength.ToString();
                    dr["Att"] = line.AttachedSection.Trim();
                    displayDataTable.Rows.Add(dr);
                }
            }
        }

        //                    break;
        //    //savpic = new Dictionary<int, byte[]>();
        //    _StartX = new Dictionary<int, float>();
        //    _StartY = new Dictionary<int, float>();
        private void LoadStartPointsDataTable()
        {
            List<SMLine> startLines = (from l in WorkingParcel.AllSectionLines where l.LineNumber == 1 select l).OrderBy(s => s.SectionLetter).ToList();

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

        //        case SketchDrawingState.JumpMoveToBeginPoint:
        //        case SketchDrawingState.JumpPointSelected:
        //            switch (direction)
        //            {
        //                //Regular directions
        //                case CamraDataEnums.CardinalDirection.N:
        //                    endX = ScaledStartPoint.X;
        //                    endY = ScaledStartPoint.Y - (float)scaledDistance;
        //    SketchFolder = sketchFolder;
        //    SketchRecord = sketchRecord;
        //    SketchCard = sketchCard;
        //    SketchUpGlobals.HasSketch = hasSketch;
        //        if (JumpTable.Rows.Count > 0)
        //        {
        //            secltr = FindClosestCorner(CurrentScale, ref curltr, AttSecLtrList);
        //        }
        //    }
        //}
        private int MaximumLineCount()
        {
            int maxLineCnt;
            StringBuilder lineCntx = new StringBuilder();
            lineCntx.Append(String.Format("select max(jlline#) from {0}.{1}line ",
                       SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix
                        ));
            lineCntx.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ",
                SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr));

            maxLineCnt = Convert.ToInt32(dbConn.DBConnection.ExecuteScalar(lineCntx.ToString()));
            return maxLineCnt;
        }

        //        case SketchDrawingState.Drawing:
        //            movementType = MovementMode.Draw;
        //            break;
        //    IsNewSketch = false;
        //    _hasNewSketch = hasNewSketch;
        //    IsNewSketch = hasNewSketch;
        //    _addSection = false;
        private void MeasureAngle()
        {
            throw new NotImplementedException();
        }

        //    string D1 = anglecalls.Substring(0, commaCnt).Trim();
        //
        // TODO: Remove if not needed:
        //private void HandleMovementByKey(CamraDataEnums.CardinalDirection direction, decimal distance)
        //{
        //    decimal scale = WorkingParcel.Scale;
        //    decimal scaledDistance = distance * scale;
        //    MovementMode movementType;
        //    float endX;
        //    float endY;
        //    switch (SketchingState)
        //    {
        //        case SketchDrawingState.BeginPointSelected:
        //    Locality = _locality;
        //        List<string> AttSecLtrList = new List<string>();
        private void MoveCursor()
        {
            Color penColor;
            Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Convert.ToInt32(JumpX) - 50, Convert.ToInt32(JumpY) - 50);
            Cursor.Position = new Point(Convert.ToInt32(JumpX) - 50, Convert.ToInt32(JumpY));
            penColor = (_undoMode || draw) ? Color.Red : Color.Black;

            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(Color.Red, 4);
            g.DrawRectangle(pen1, Convert.ToInt32(JumpX), Convert.ToInt32(JumpY), 1, 1);
            g.Save();

            sketchBox.Image = MainImage;

            //DMouseClick();
        }

        //    _currentSection = new SectionDataCollection(_fox, SketchUpGlobals.Record, SketchUpGlobals.Card);
        private void MoveCursorToNewPoint(PointF newPoint)
        {
            Color penColor;
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(newPoint.X);
            int jumpYScaled = Convert.ToInt32(newPoint.Y);
            Cursor.Position = new Point(jumpXScaled, jumpYScaled);
            penColor = PenColorForDrawing(EditState);
            _isJumpMode = (EditState == DrawingState.JumpPointSelected || EditState == DrawingState.SectionAdded);
            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(penColor, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 1, 1);
            g.Save();

            sketchBox.Image = MainImage;
            sketchBox.Refresh();
        }

        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multipleSectionsAttachment = String.Empty;

            if (sectionLetterList.Count > 1)
            {
                MultiplePoints = new DataTable();

                MultiSectionSelection attachmentSectionLetterSelected = new MultiSectionSelection(sectionLetterList);
                attachmentSectionLetterSelected.ShowDialog(this);

                multipleSectionsAttachment = MultiSectionSelection.adjsec;

                MultiplePoints = MultiSectionSelection.MultiplePointsDataTable;

                _hasMultiSection = true;
            }

            return multipleSectionsAttachment;
        }

        //private void InitializeDataTablesAndVariables(SketchUpParcelData currentParcel, string sketchFolder, string sketchRecord, string sketchCard, string _locality, CAMRA_Connection _fox, SectionDataCollection currentSection, bool hasSketch, bool hasNewSketch)
        //{
        //    checkDirection = false;
        //    _currentParcel = currentParcel;
        //    _currentSection = currentSection;
        //private void NorthDirBtn_Click(object sender, EventArgs e)
        //{
        //    _isKeyValid = true;
        //    MoveNorth(NextStartX, NextStartY);
        //    DistText.Focus();
        //}

        private void NotifyUserOfLegalMoves()
        {
            try
            {
                string legalDirs = string.Empty;
                string message = string.Format("You may only move {0} from this jump point.", legalDirs);
                MessageBox.Show(message, "Illegal direction", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                UndoJump = true;
                RevertToPriorVersion();
                DistText.Text = String.Empty;
                distance = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        //        string secltr = String.Empty;
        //        string curltr = String.Empty;
        private decimal OriginalDistanceX()
        {
            decimal origDistX = 0;

            StringBuilder orgLen = new StringBuilder();
            orgLen.Append(String.Format("select jllinelen from {0}.{1}line where jlrecord = {2} and jldwell = {3} ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card
                        ));
            orgLen.Append(String.Format("and jlsect = '{0}' and jlline# = {1} ",
                CurrentSecLtr, AttSpLineNo));

            origDistX = Convert.ToDecimal(dbConn.DBConnection.ExecuteScalar(orgLen.ToString()));
            return origDistX;
        }

        private AngleVector ParseAngleEntry(string textEntered)
        {
            AngleVector vector = new AngleVector();
            string anglecalls = DistText.Text.Trim();
            decimal xDist = 0.00M;
            decimal yDist = 0.00M;
            int commaCnt = anglecalls.IndexOf(",");

            string xDistText = anglecalls.Substring(0, commaCnt).Trim();
            string yDistText = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();
            decimal.TryParse(xDistText, out xDist);
            decimal.TryParse(yDistText, out yDist);

            AngleForm angleDialog = new AngleForm();
            angleDialog.ShowDialog();
            CamraDataEnums.CardinalDirection angleDirection = angleDialog.AngleDirection;

            vector.XLength = xDist;
            vector.YLength = yDist;
            vector.AngledLineDirection = angleDirection;
            return vector;
        }

        private AngleVector ParseNEWSLine(string textEntered, CamraDataEnums.CardinalDirection direction)
        {
            decimal distanceValueEntered = 0.00M;
            AngleVector angle = new AngleVector();
            decimal.TryParse(textEntered, out distanceValueEntered);
            switch (direction)
            {
                case CamraDataEnums.CardinalDirection.N:
                case CamraDataEnums.CardinalDirection.S:
                    angle.XLength = 0.00M;
                    angle.YLength = distanceValueEntered;
                    break;

                case CamraDataEnums.CardinalDirection.E:
                case CamraDataEnums.CardinalDirection.W:
                    angle.XLength = distanceValueEntered;
                    angle.YLength = 0.00M;
                    break;

                case CamraDataEnums.CardinalDirection.None:
                default:
                    angle.XLength = 0.00M;
                    angle.YLength = 0.00M;

                    break;
            }
            return angle;
        }
        private bool StopClose()
        {
            bool preventClose = UnsavedChangesExist;
            if (preventClose)
            {

      
            string message = "Do you want to save changes?";
            DialogResult response = MessageBox.Show(message, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            switch (response)
            {
                case DialogResult.Yes:
                    SaveCurrentParcelToDatabaseAndExit();
              

                    break;

                case DialogResult.No:
                    DiscardChangesAndExit();
                   
                    break;

                default:

                    break;
            }
            }
            return preventClose;
        }

        // TODO: Remove if not needed:	
        //private void RedrawSection()
        //{
        //    NeedToRedraw = true;

        //    for (int i = 0; i < undoPoints.Rows.Count; i++)
        //    {
        //        float redist = 0;
        //        string undodirect = String.Empty;
        //        undodirect = undoPoints.Rows[i]["Direct"].ToString();

        //        float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

        //        float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

        //        float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

        //        float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

        //        if (undodirect == "N" || undodirect == "S" || undodirect == "E" || undodirect == "W")
        //        {
        //            if (x1 == x2)
        //            {
        //                redist = Math.Abs((y1 - y2) / _currentScale);
        //            }
        //            if (y1 == y2)
        //            {
        //                redist = Math.Abs((x1 - x2) / _currentScale);
        //            }
        //        }
        //        if (undodirect == "NE" || undodirect == "SE" || undodirect == "NW" || undodirect == "SW")
        //        {
        //            float x1f = Math.Abs((x1 - x2) / _currentScale);
        //            float y1f = Math.Abs((y1 - y2) / _currentScale);

        //            distanceD = Convert.ToInt32(Math.Sqrt(x1f + y1f));

        //            decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(x1f + y1f)), 2);

        //            distance = Convert.ToDecimal(distanceD1);

        //            AngD1 = Convert.ToDecimal(x1f);
        //            AngD2 = Convert.ToDecimal(y1f);
        //        }

        //        Graphics g = Graphics.FromImage(MainImage);
        //        g.Save();

        //        StartX = x1;
        //        StartY = y1;

        //        ExpSketchPBox.Image = MainImage;

        //        //click++;
        //        ////savpic.Add(click, imageToByteArray(_mainimage));

        //        if (undoPoints.Rows[i]["Direct"].ToString() == "E")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX + (redist * _currentScale)), StartY);

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            ////savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "N")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, StartX, (StartY - (redist * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "S")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, StartX, (StartY + (redist * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "W")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX - (redist * _currentScale)), StartY);

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "NW")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "NE")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "SW")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //        if (undoPoints.Rows[i]["Direct"].ToString() == "SE")
        //        {
        //            SolidBrush brush = new SolidBrush(Color.Red);
        //            Pen pen1x = new Pen(Color.Red, 2);
        //            Pen pen1w = new Pen(Color.White, 2);
        //            Font f = new Font("Segue UI", 8, FontStyle.Bold);

        //            g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

        //            ExpSketchPBox.Image = MainImage;

        //            //click++;
        //            //savpic.Add(click, imageToByteArray(_mainimage));
        //        }
        //    }

        //    ExpSketchPBox.Image = MainImage;

        //    //click++;
        //    //savpic.Add(click, imageToByteArray(_mainimage));
        //}
        //        private void ReOpenSec()
        //        {
        //            //int rowindex = 0;

        //            //DataSet rolines = null;

        //            //StringBuilder getLine = new StringBuilder();
        //            //getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
        //            //getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
        //            //getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
        //            //           SketchUpGlobals.LocalLib,
        //            //              SketchUpGlobals.LocalityPreFix,

        //            //            //SketchUpGlobals.FcLib,
        //            //            //SketchUpGlobals.FcLocalityPrefix,
        //            //            SketchUpGlobals.Record,
        //            //            SketchUpGlobals.Card,
        //            //            SketchUpGlobals.ReOpenSection));

        //            //rolines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

        //            //int maxsecline = rolines.Tables[0].Rows.Count;
        //            //if (rolines.Tables[0].Rows.Count > 0)
        //            //{
        //            //    REJumpTable.Clear();

        //            //    for (int i = 0; i < rolines.Tables[0].Rows.Count; i++)
        //            //    {
        //            //        decimal Distance = 0;

        //            //        DataRow row = REJumpTable.NewRow();
        //            //        row["Record"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlrecord"].ToString());
        //            //        row["Card"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jldwell"].ToString());
        //            //        row["Sect"] = rolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
        //            //        row["LineNo"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());
        //            //        row["Direct"] = rolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
        //            //        row["XLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlxlen"].ToString());
        //            //        row["YLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlylen"].ToString());
        //            //        row["Length"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jllinelen"].ToString());
        //            //        row["Angle"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlangle"].ToString());
        //            //        row["XPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1x"].ToString());
        //            //        row["YPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1y"].ToString());
        //            //        row["XPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
        //            //        row["YPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
        //            //        row["Attach"] = rolines.Tables[0].Rows[i]["jlattach"].ToString();

        //            //        decimal xpt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
        //            //        decimal ypt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2y"].ToString());

        //            //        float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
        //            //        float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

        //            //        rowindex = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());

        //            //        _StartX.Add(rowindex, xPoint);

        //            //        _StartY.Add(rowindex, yPoint);

        //            //        REJumpTable.Rows.Add(row);
        //            //    }

        //            //    float _JumpXT = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale));

        //            //    float _JumpX = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
        //            //    float _JumpY = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["YPT2"].ToString()) * _currentScale));

        //            //    JumpX = _JumpX;
        //            //    JumpY = _JumpY;

        //            //    GetStartCorner();

        //            //}
        //            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

        //#if DEBUG
        //            MessageBox.Show(message);
        //#else
        //            Console.WriteLine(message);
        //            throw new NotImplementedException();
        //#endif
        //        }



        private void RedrawSketch(SMParcel parcel)
        {
            SMSketcher sketcher = new SMSketcher(parcel, sketchBox);
            sketcher.RenderSketch(WorkingSection.SectionLetter);
            sketchBox.Image = sketcher.SketchImage;
            _currentScale = (float)parcel.Scale;
        }

        private void RefreshWorkspace()
        {
            RedrawSketch(WorkingParcel);
            SetButtonStates(EditState);
        }

        private void RenderCurrentSketch()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        //        // AddListItemsToJumpTableList(jx, jy, CurrentScale, lines);


        private void ReorderSectionsAfterChanges()
        {
            Garcnt = 0;
            GarSize = 0;
            carportCount = 0;
            CPSize = 0;
            SMParcel originalParcel = SketchUpGlobals.SMParcelFromData;
            SMParcelMast parcelMaster = WorkingParcel.ParcelMast;
            var sectionLetterList = (from s in ParcelMast.Parcel.Sections select s.SectionLetter).Distinct().ToList();
            sectionLetterList.Sort();
            foreach (SMSection s in WorkingParcel.Sections)
            {
                TotalGaragesAndCarports(s);
            }

            if (Garcnt == 0)
            {
                UpdateGarageCountToZero();
            }
            if (carportCount == 0)
            {
                UpdateCarportCountToZero();
            }

            ConfirmGarageNumbers(originalParcel);
            ConfirmCarportNumbers();
        }

        private void RevertToPriorVersion()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
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
                FlipHorizontal();
            }
        }

      

        //                AddXLine(thisSection);
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
                Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
                throw;
            }
        }

        //        // PopulateSectionList();
        //        for (int i = 0; i < SecItemCnt; i++)
        //        {
        //            string thisSection = SecLetters[i].ToString();
        //            if (SecItemCnt >= 1)
        //            {
        //                SecLineCnt= CountLines(thisSection);
      
        //    bool sketchHasLineData = lines.Tables[0].Rows.Count > 0;
        //    if (sketchHasLineData)
        //    {
        //        SecItemCnt = CountSections();
        private void SetAddNewPointButton(bool enabled)
        {
            isInAddNewPointMode = enabled;
        }

        private void SetButtonStates(DrawingState sketchState)
        {
            switch (sketchState)
            {
                case DrawingState.Drawing:
                    btnAddSection.Enabled = false;
                    AddSectionContextMenu.Enabled = false;
                    btnDrawingDone.Enabled = WorkingSection.SectionIsClosed;
                    cmiJumpToCorner.Enabled = false;

                    break;

                case DrawingState.SectionAdded:
                case DrawingState.JumpPointSelected:
                    cmiJumpToCorner.Enabled = true;
                    AddSectionContextMenu.Enabled = true;
                    break;

                case DrawingState.SketchLoaded:
                    btnAddSection.Enabled = true;
                    AddSectionContextMenu.Enabled = true;
                    btnDrawingDone.Enabled = false;
                    cmiJumpToCorner.Enabled = false;
                    AddSectionContextMenu.Enabled = false;
                    break;

                default:
                    break;
            }
        }

        private void SetReadyButtonAppearance()
        {
            btnBeginSection.BackColor = Color.PaleTurquoise;
            btnBeginSection.Text = "Begin";
            btnBeginSection.Enabled = ScaledJumpPoint != null;
        }

        private void ShowMessageBox(string s)
        {
            MessageBox.Show(s);
        }

        private void ShowState(DrawingState value)
        {
            string message = string.Empty;
            switch (value)
            {
                case DrawingState.Drawing:
                    message = string.Format("Drawing Section {0}", WorkingSection.SectionLabel);
                    break;

                case DrawingState.SectionEditCompleted:
                    message = string.Format("Click \"Save\" to commit the changes to section {0}.", WorkingSection.SectionLetter);
                    break;

                case DrawingState.DoneDrawing:
                    message = "Section Added to Sketch. Click \"Save\" to commit this change.";
                    break;

                case DrawingState.JumpPointSelected:
                    StringBuilder directions = new StringBuilder();
                    if (LegalMoveDirections.Count > 0)
                    {
                    }
                    int counter = 1;
                    foreach (string s in LegalMoveDirections.Distinct())
                    {
                        if (counter < LegalMoveDirections.Count)
                        {
                            directions.AppendFormat("{0}, ", s);
                        }
                        else
                        {
                            directions.AppendFormat("{0}", s);
                        }
                        counter++;
                    }

                    message = string.Format("Move ({0}) to set start, or click \"Begin\" to start drawing the section here.", directions.ToString());
                    break;

                case DrawingState.SectionAdded:
                    message = string.Format("Added Section {0}. Right-click to select the nearest corner as a reference point.", WorkingSection.SectionLabel);
                    break;

                case DrawingState.SketchLoaded:
                    message = string.Format("Ready to edit the sketch of record {0}", SketchUpGlobals.ParcelWorkingCopy.Record);
                    break;

                default:
                    break;
            }
            DisplayStatus(message);
        }

        private void ShowVersion(int snapShotIndex)
        {
            string versionInfo = string.Format("#{0}", snapShotIndex);
            snapshotIndexLabel.Text = versionInfo;
        }

        //                MessageBox.Show(errMessage);
        //#endif
        //                throw;
        //            }
        //        }
        private void ShowWorkingCopySketch(string sketchFolder, string sketchRecord, string sketchCard, bool hasSketch, bool hasNewSketch)
        {
            try
            {
                InitializeDataTablesAndVariables(sketchFolder, sketchRecord, sketchCard, hasSketch, hasNewSketch);

                InitializeDisplayDataGrid();
                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
                SketchUpGlobals.HasSketch = (SketchUpGlobals.ParcelWorkingCopy != null && WorkingParcel.AllSectionLines.Count > 0);
                IsNewSketch = !SketchUpGlobals.HasSketch;

                //HACK - Easier to repeat than track down the usages at this juncture
                SketchUpGlobals.HasNewSketch = IsNewSketch;
                if (SketchUpGlobals.HasSketch == true)
                {
                    SMSketcher sketcher = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy, sketchBox);
                    sketcher.RenderSketch();

                    MainImage = sketcher.SketchImage;
                    _currentScale = (float)WorkingParcel.Scale;

                    //MainImage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
                    //_currentScale = _scale;
                }
                else
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                }
                ScaleBaseX = sketchBox.Width / (float)WorkingParcel.SketchXSize;

                // ScaleBaseX = BuildingSketcher.basePtX;
                //  ScaleBaseY = BuildingSketcher.basePtY;
                ScaleBaseY = sketchBox.Height / (float)WorkingParcel.SketchYSize;

                if (MainImage == null)
                {
                    MainImage = new Bitmap(sketchBox.Width, sketchBox.Height);
                    _vacantParcelSketch = true;
                    IsNewSketch = true;
                }
                if (_vacantParcelSketch == true)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    g.Clear(Color.White);
                    _currentScale = Convert.ToSingle(7.2);
                }

                sketchBox.Image = MainImage;
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

        private void sortSection()
        {
            //TODO: Refactor into SketchManager
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
            FixSect = new List<string>();

            StringBuilder addFix = new StringBuilder();
            addFix.Append(String.Format("select jlsect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 ",
                      SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                        //SketchUpGlobals.FcLib,
                        //SketchUpGlobals.FcLocalityPrefix,
                        SketchUpGlobals.Record,
                        SketchUpGlobals.Card));

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
                          SketchUpGlobals.LocalityPrefix

                                    //SketchUpGlobals.FcLib,
                                    //SketchUpGlobals.FcLocalityPrefix
                                    ));
                    chkLen.Append(String.Format("where jlrecord = {0} and jldwell = {1} order by jlsect,jlline# ", SketchUpGlobals.Record, SketchUpGlobals.Card));

                    DataSet fixl = dbConn.DBConnection.RunSelectStatement(chkLen.ToString());

                    if (fixl.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < fixl.Tables[0].Rows.Count; i++)
                        {
                            //MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 3177", SketchUpGlobals.Record, SketchUpGlobals.Card));

                            StringBuilder updLine = new StringBuilder();
                            updLine.Append(String.Format("update {0}.{1}line set jlxlen = {2},jlylen = {3},jllinelen = {4} ",
                                           SketchUpGlobals.LocalLib,
                                           SketchUpGlobals.LocalityPrefix,

                                            //SketchUpGlobals.FcLib,
                                            //SketchUpGlobals.FcLocalityPrefix,
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Xlen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["Ylen"].ToString()),
                                            Convert.ToDecimal(fixl.Tables[0].Rows[i]["LineLen"].ToString())));
                            updLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                    SketchUpGlobals.Record,
                                    SketchUpGlobals.Card,
                                    fixl.Tables[0].Rows[i]["jlsect"].ToString(),
                                    Convert.ToInt32(fixl.Tables[0].Rows[i]["jlline#"].ToString())));

                            dbConn.DBConnection.ExecuteNonSelectStatement(updLine.ToString());
                        }
                    }
                }
            }
        }

        //                    //g.DrawRectangle(whitePen, 0, 0, 1000, 572);
        //                    //g.FillRectangle(FillBrush, 0, 0, 1000, 572);
        //                    _currentScale = Convert.ToSingle(7.2);
        //                }
        private void SouthDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveSouth(NextStartX, NextStartY);
            DistText.Focus();
        }

        private void SplitLineIfNeeded()
        {
            try
            {
                if (ScaledStartPoint != ScaledJumpPoint)
                {
                    SMSection attachmentSection = WorkingParcel.SelectSectionByLetter(AttSectLtr);
                    PointF breakPoint = ScaledStartPoint;
                    SMLine breakLine = (from l in attachmentSection.Lines where SMGlobal.PointIsOnLine(l.ScaledStartPoint, l.ScaledEndPoint, breakPoint) select l).FirstOrDefault();
                    SMLineManager lm = new SMLineManager();

                    //  SMParcel newParcel = lm.BreakLine(SketchUpGlobals.ParcelWorkingCopy, AttSectLtr, breakLine.LineNumber, breakPoint, WorkingParcel.SketchOrigin);
                    //  newParcel.SnapShotIndex++;
                    //  SketchUpGlobals.ParcelWorkingCopy = newParcel;
                    AddParcelToSnapshots(WorkingParcel);
                    ShowVersion(WorkingParcel.SnapShotIndex);
                }
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

        //                if (MainImage == null)
        //                {
        //                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
        //                    _vacantParcelSketch = true;
        //                    IsNewSketch = true;
        //                }
        //                if (_vacantParcelSketch == true)
        //                {
        //                    Graphics g = Graphics.FromImage(MainImage);
        //                    g.Clear(Color.White);
        private void TextBtn_Click(object sender, EventArgs e)
        {
            if (FieldText.Text.Trim() != String.Empty)
            {
                Graphics g = Graphics.FromImage(MainImage);
                SolidBrush brush = new SolidBrush(Color.Blue);
                Pen pen1 = new Pen(Color.Red, 2);
                Font f = new Font("Segue UI", 8, FontStyle.Bold);

                g.DrawString(FieldText.Text.Trim(), f, brush, new PointF(_mouseX + 5, _mouseY));

                FieldText.Text = String.Empty;
                FieldText.Focus();

                sketchBox.Image = MainImage;
            }
        }

        private void TotalGaragesAndCarports(SMSection s)
        {
            if (SketchUpLookups.GarageTypes.Contains(s.SectionType))
            {
                Garcnt++;

                GarSize += s.SqFt;
            }
            if (SketchUpLookups.CarPortTypes.Contains(s.SectionType))
            {
                carportCount++;

                CPSize += s.SqFt;
            }
        }

        //                    //MainImage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
        //                    //_currentScale = _scale;
        //                }
        //                else
        //                {
        //                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
        //                }
        //                ScaleBaseX = ExpSketchPBox.Width / (float)WorkingParcel.SketchXSize;
        //               // ScaleBaseX = BuildingSketcher.basePtX;
        //              //  ScaleBaseY = BuildingSketcher.basePtY;
        //                 ScaleBaseY = ExpSketchPBox.Height / (float)WorkingParcel.SketchYSize;
        private void tsbExitSketch_Click(object sender, EventArgs e)
        {
            StopClose();
        }

        //                //HACK - Easier to repeat than track down the usages at this juncture
        //                SketchUpGlobals.HasNewSketch = IsNewSketch;
        //                if (SketchUpGlobals.HasSketch == true)
        //                {
        //                    SMSketcher sketcher = new SMSketcher(SketchUpGlobals.ParcelWorkingCopy,ExpSketchPBox);
        //                    sketcher.RenderSketch();
        //                    WorkingParcel.SetScaleAndOriginForParcel(ExpSketchPBox);
        //                    MainImage = sketcher.SketchImage;
        //                    _currentScale = (float)WorkingParcel.Scale;
        private void UnDoBtn_Click(object sender, EventArgs e)
        {
            switch (EditState)
            {
                case DrawingState.Drawing:
                    UndoLine();
                    break;

                case DrawingState.JumpPointSelected:

                    RedrawSketch(WorkingParcel);

                    break;

                case DrawingState.SectionAdded:
                    break;

                case DrawingState.SketchLoaded:
                    break;

                default:
                    break;
            }
        }

        //                InitializeDisplayDataGrid();
        //                SketchUpGlobals.ParcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
        //                SketchUpGlobals.HasSketch = (SketchUpGlobals.ParcelWorkingCopy != null && WorkingParcel.AllSectionLines.Count > 0);
        //                IsNewSketch = !SketchUpGlobals.HasSketch;
        private void UndoLine()
        {
            SMSketcher sms = new SMSketcher(WorkingParcel, sketchBox);
            sms.RenderSketch(WorkingSection.SectionLetter);
            sketchBox.Image = sms.SketchImage;

            if (WorkingSection.Lines != null && WorkingSection.Lines.Count > 0)
            {
                SMLine lastLine = (from l in WorkingSection.Lines.OrderBy(n => n.LineNumber) select l).LastOrDefault();

                if (lastLine.LineNumber == 1)
                {
                    string message = "This will remove the last line. Do you wish to delete the section as well?\n\nChoose \"Yes\" to revert to the saved version, or \"No\" to start over drawing the section. \n\nChoose \"Cancel\" to leave this line in place.";
                    string title = "Delete Section?";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
                    MessageBoxIcon icon = MessageBoxIcon.Question;
                    MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
                    DialogResult response = MessageBox.Show(message, title, buttons, icon, defButton);
                    switch (response)
                    {
                        case DialogResult.Cancel:

                            break;

                        case DialogResult.Yes:
                            DeleteWorkingSectionAndRevert();

                            break;

                        case DialogResult.No:

                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    WorkingSection.Lines.Remove(lastLine);
                }
                sms = new SMSketcher(WorkingParcel, sketchBox);
                sms.RenderSketch(WorkingParcel.LastSectionLetter);
                sketchBox.Image = sms.SketchImage;
            }
        }

        private void UpdateCarportCountToZero()
        {
            //TODO: make this flexible to update the garage count and square footage per the ParcelMast
            //TODO: Refactor into SketchManager
            StringBuilder zerocp = new StringBuilder();
            zerocp.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
                                    SketchUpGlobals.LocalLib,
                                    SketchUpGlobals.LocalityPrefix,
                                    SketchUpGlobals.Record,
                                    SketchUpGlobals.Card));

            dbConn.DBConnection.ExecuteNonSelectStatement(zerocp.ToString());
        }

        private void UpdateGarageCountToZero()
        {
            //TODO: Refactor into SketchManager
            //TODO: make this flexible to update the garage count and square footage per the ParcelMast
            StringBuilder zeroGarageSql = new StringBuilder();
            zeroGarageSql.Append(String.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0,mgart2 = 0,mgar#2 = 0 where mrecno = {2} and mdwell = {3} ",
                                    SketchUpGlobals.LocalLib,
                                    SketchUpGlobals.LocalityPrefix,
                                    SketchUpGlobals.Record,
                                    SketchUpGlobals.Card));

            dbConn.DBConnection.ExecuteNonSelectStatement(zeroGarageSql.ToString());
        }

        private DataSet UpdateMasterArea(decimal summedArea)
        {
            string checkMaster = string.Format("select * from {0}.{1}master where jmrecord = {2} and jmdwell = {3} ",
                SketchUpGlobals.LocalLib,
                SketchUpGlobals.LocalityPrefix,
                SketchUpGlobals.Record,
                SketchUpGlobals.Card);

            DataSet ds_master = dbConn.DBConnection.RunSelectStatement(checkMaster.ToString());

            if (ds_master.Tables[0].Rows.Count > 0)
            {
                string updateMasterSql = string.Format("update {0}.{1}master set jmtotsqft = {2} where jmrecord = {3} and jmdwell = {4} ",
                               SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPrefix,
                               summedArea,
                               SketchUpGlobals.Record,
                               SketchUpGlobals.Card);

                dbConn.DBConnection.ExecuteNonSelectStatement(updateMasterSql.ToString());
            }

            return ds_master;
        }

        private void viewSectionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

#endregion
        private const int sketchBoxPaddingTotal = 20;

        private void tsmiSaveToDb_Click(object sender, EventArgs e)
        {
            SMParcel parcel = WorkingParcel;
            SaveCurrentParcelToDatabaseAndExit(parcel);
        }

        private void SaveCurrentParcelToDatabaseAndExit(SMParcel parcel)
        {
           
        }

        private void SaveChanges(SMParcel parcel)
        {
            SketchRepository sr = new SketchRepository(parcel);
            ParcelMast = sr.SaveCurrentParcel(parcel);
            WorkingParcel = ParcelMast.Parcel;
        }

        private void miFileAddSection_Click(object sender, EventArgs e)
        {
            AddSection();
        }

        private void miSaveAndContinue_Click(object sender, EventArgs e)
        {
            SaveChanges(WorkingParcel);

            UnsavedChangesExist = false;
            WorkingSection = null;

        }

        private void miSaveAndClose_Click(object sender, EventArgs e)
        {
            SaveChanges(WorkingParcel);

            UnsavedChangesExist = false;
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
    }
}
