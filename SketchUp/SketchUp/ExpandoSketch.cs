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
using SWallTech;

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
        public ExpandoSketch(string sketchFolder, int sketchRecord, int sketchCard, bool hasSketch, bool hasNewSketch)
        {
            // Omitted any steps not needed for SketchUp. JMM 5-9-2016
            InitializeComponent();
            AddSectionContextMenu.Enabled = false;

            LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;
            if (LocalParcelCopy.SnapShotIndex == 0)
            {
                LocalParcelCopy.SnapShotIndex++;
                AddParcelToSnapshots(LocalParcelCopy);
            }

            ShowWorkingCopySketch(sketchFolder, sketchRecord.ToString(), sketchCard.ToString(), hasSketch, hasNewSketch);
        }

        #region "Public Methods"

        public static decimal RoundToNearestHalf(decimal value)
        {
            return Math.Round(((value * 2) / 2), 1);
        }

        public void AddNewPoint()
        {
            SaveSketchData();
        }

        public void AdjustLine(decimal newEndX, decimal newEndY, decimal newDistX, decimal newDistY, decimal EndEndX, decimal EndEndY, decimal finDist)
        {
            StringBuilder adjLine = new StringBuilder();
            adjLine.Append(String.Format("update {0}.{1}line set jldirect = '{2}',jlxlen = {3},jlylen = {4},jllinelen = {5}, ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,
                            CurrentAttDir,
                            newDistX,
                            newDistY,
                            finDist));
            adjLine.Append(String.Format("jlpt1x = {0},jlpt1y = {1},jlpt2x = {2},jlpt2y = {3} ",
                    newEndX, newEndY, EndEndX, EndEndY));
            adjLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
               SketchUpGlobals.Record, SketchUpGlobals.Card, _savedAttSection, (mylineNo + 1)));

            dbConn.DBConnection.ExecuteNonSelectStatement(adjLine.ToString());
        }

        public void getSplitLine()
        {
            int Sprowindex = 0;

            ConnectSec = String.Empty;

            if (MultiSectionSelection.adjsec != String.Empty)
            {
                ConnectSec = MultiSectionSelection.adjsec;
            }
            if (MultiSectionSelection.adjsec == String.Empty)
            {
                ConnectSec = _savedAttSection;
            }

            mylineNo = 0;

            //decimal startsplitx1 = startSplitX;
            //decimal startsplity1 = startSplitY;

            if (BeginSplitX != (float)startSplitX)
            {
                NextStartX = (float)startSplitX;
            }
            if (BeginSplitY != (float)startSplitY)
            {
                NextStartY = (float)startSplitY;
            }

            DataSet Sprolines = null;

            StringBuilder getSpLine = new StringBuilder();
            getSpLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            getSpLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            getSpLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                           //SketchUpGlobals.FcLib,
                           //SketchUpGlobals.FcLocalityPrefix,
                           SketchUpGlobals.Record,
                           SketchUpGlobals.Card,
                           ConnectSec));

            Sprolines = dbConn.DBConnection.RunSelectStatement(getSpLine.ToString());

            if (Sprolines.Tables[0].Rows.Count > 0)
            {
                RESpJumpTable.Clear();

                for (int i = 0; i < Sprolines.Tables[0].Rows.Count; i++)
                {
                    DataRow row = RESpJumpTable.NewRow();
                    row["Record"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlrecord"].ToString());
                    row["Card"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jldwell"].ToString());
                    row["Sect"] = Sprolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = Sprolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["XLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["YLen"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlangle"].ToString());
                    row["XPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["YPt1"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["XPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["YPt2"] = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
                    row["Attach"] = Sprolines.Tables[0].Rows[i]["jlattach"].ToString();

                    decimal xpt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2x"].ToString());
                    decimal ypt2 = Convert.ToDecimal(Sprolines.Tables[0].Rows[i]["jlpt2y"].ToString());

                    // TODO: Remove if not needed:	     float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
                    // TODO: Remove if not needed:	       float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

                    Sprowindex = Convert.ToInt32(Sprolines.Tables[0].Rows[i]["jlline#"].ToString());

                    RESpJumpTable.Rows.Add(row);
                }
            }

            if (RESpJumpTable.Rows.Count > 0)
            {
                AttSpLineNo = 0;

                // TODO: Remove if not needed:	bool foundLine = false;

                int RESpJumpTableIndex = 0;

                if (RESpJumpTable.Rows.Count > 0)
                {
                    for (int i = 0; i < RESpJumpTable.Rows.Count; i++)
                    {
                        if (offsetDir == "N" || offsetDir == "S")
                        {
                            XadjR = Convert.ToDecimal(NextStartX);
                            YadjR = Convert.ToDecimal(NextStartY);

                            string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                            decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
                            decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
                            decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
                            decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
                            decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
                            decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
                            int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                            string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();

                            // TODO: Remove if not needed:
                            //decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

                            // TODO: Remove if not needed:
                            //if (offsetDir == "N" && NextStartY != (float)(y2 - distance))
                            //{
                            //    //startSplitY = (y2 - distance);
                            //}

                            //if (offsetDir == "S")
                            //{
                            //    //startSplitY = (y2 + distance);
                            //}

                            if (x2 != startSplitX)
                            {
                                NextStartX = (float)startSplitX;
                            }

                            if (NextStartY != (float)startSplitY)
                            {
                                NextStartY = (float)startSplitY;
                            }

                            // TODO: Remove if not needed:	   int dex1 = x1.ToString().IndexOf(".");
                            // TODO: Remove if not needed:	  int dex2 = x2.ToString().IndexOf(".");

                            if (NextStartX == (float)x2 && (i + 1) < RESpJumpTable.Rows.Count)
                            {
                                if (startSplitY >= y2 && startSplitY <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString()))
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["YLen"].ToString());

                                    begSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt1"].ToString());

                                    begSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt1"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    CurrentSecLtr = atsect;
                                    RemainderLineLength = OrigLineLength - splitLineDist;
                                }
                            }

                            decimal x1x = (x1 + .5m);

                            decimal x1B = (x1 - .5m);

                            decimal x2x = (x2 + .5m);

                            decimal x2B = (x2 - .5m);
                            LineNumberToBreak = lnNbr;
                            if (NextStartY < (float)y1 && NextStartY > (float)y2 && dirsect == "N" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }

                            if (NextStartY > (float)y1 && NextStartY < (float)y2 && dirsect == "S" && NextStartX >= (float)x1B && NextStartX <= (float)x1x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }
                        }
                        if (offsetDir == "E" || offsetDir == "W")
                        {
                            XadjR = Convert.ToDecimal(NextStartX);
                            YadjR = Convert.ToDecimal(NextStartY);

                            string dirsect = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                            decimal x1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["XLen"].ToString());
                            decimal y1Len = Convert.ToDecimal(RESpJumpTable.Rows[i]["YLen"].ToString());
                            decimal x1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt1"].ToString());
                            decimal y1 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt1"].ToString());
                            decimal x2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Xpt2"].ToString());
                            decimal y2 = Convert.ToDecimal(RESpJumpTable.Rows[i]["Ypt2"].ToString());
                            int lnNbr = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                            string atsect = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                            decimal origLen = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());

                            if (y2 != startSplitY)
                            {
                                //NextStartY = (float)startSplitY;
                            }

                            if (x2 != startSplitX)
                            {
                                //NextStartX = (float)startSplitX;
                            }

                            if (startSplitY == y2)
                            {
                                if (startSplitX >= x2 && startSplitX <= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "E")
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    CurrentSecLtr = atsect;

                                    RemainderLineLength = d2 - splitLineDist;
                                }

                                if (startSplitX <= x2 && startSplitX >= Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString()) && offsetDir == "W")
                                {
                                    _savedAttSection = atsect;

                                    OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Xpt2"].ToString());
                                    EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["Ypt2"].ToString());

                                    decimal d2 = Convert.ToDecimal(RESpJumpTable.Rows[i + 1]["XLen"].ToString());

                                    CurrentSecLtr = atsect;

                                    RemainderLineLength = d2 - splitLineDist;
                                }
                            }

                            // TODO: Remove if not needed:	     decimal y1x = (y1 + .5m);

                            decimal y2x = (y2 + .5m);

                            // TODO: Remove if not needed:	      decimal y1B = (y1 - .5m);

                            decimal y2B = (y2 - .5m);

                            decimal x1x = (x1 + .5m);

                            // TODO: Remove if not needed:	       decimal x1B = (x1 - .5m);

                            decimal x2x = (x2 + .5m);

                            decimal x2B = (x2 - .5m);

                            if (NextStartX >= (float)x1 && NextStartX <= (float)x2 && dirsect == "E" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }

                            if (NextStartX < (float)x1 && NextStartX > (float)x2 && dirsect == "W" && NextStartY >= (float)y2B && NextStartY <= (float)y2x)
                            {
                                AttLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineNo = Convert.ToInt32(RESpJumpTable.Rows[i]["LineNo"].ToString());
                                AttSpLineDir = RESpJumpTable.Rows[i]["Direct"].ToString().Trim();
                                OffSetAttSpLineDir = offsetDir;
                                RESpJumpTableIndex = i;

                                // foundLine = true; No usages found
                                NewSectionBeginPointX = startSplitX;
                                NewSectionBeginPointY = startSplitY;
                                adjNewSecX = x1Len;
                                adjNewSecY = y1Len;
                                OrigStartX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt1"].ToString());
                                OrigStartY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt1"].ToString());
                                EndSplitX = Convert.ToDecimal(RESpJumpTable.Rows[i]["XPt2"].ToString());
                                EndSplitY = Convert.ToDecimal(RESpJumpTable.Rows[i]["YPt2"].ToString());
                                OrigLineLength = Convert.ToDecimal(RESpJumpTable.Rows[i]["Length"].ToString());
                                AttSectLtr = RESpJumpTable.Rows[i]["Sect"].ToString().Trim();
                                mylineNo = AttLineNo;
                                break;
                            }
                        }
                    }

                    BeginSplitX = NextStartX;

                    BeginSplitY = NextStartY;

                    if (draw)
                    {
                        if (LastDir == "E")
                        {
                            NextStartX = NextStartX + distanceD;

                            float ty = NextStartY;
                        }
                        if (LastDir == "W")
                        {
                            NextStartX = NextStartX - distanceD;

                            float ty = NextStartY;
                        }

                        if (LastDir == "N")
                        {
                            NextStartY = NextStartY - distanceD;

                            float tx = NextStartX;
                        }
                        if (LastDir == "S")
                        {
                            NextStartY = NextStartY + distanceD;

                            float tx = NextStartX;
                        }
                    }

                    BeginSplitX = NextStartX;
                    BeginSplitY = NextStartY;
                }
            }
        }

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

                //_lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                _lenString = String.Format("{0:N1} ft.", distanceD.ToString());

                txtLocf = ((distanceD * _currentScale) / 2);

                decimal jup = Convert.ToDecimal(distanceD);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, (StartX + (distanceD * _currentScale)), StartY);
                    g.DrawString(_lenString, f, brush, new PointF((StartX + txtLocf), (StartY - 15)));
                }

                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

        public void MoveEastToBegin(PointF startPointScaled, decimal lineLength)
        {
            if (_isKeyValid == true)
            {
                decimal scaledLength = lineLength * LocalParcelCopy.Scale;
                float newX = startPointScaled.X + (float)scaledLength;
                EndOfJumpMovePoint = new PointF(newX, startPointScaled.Y);

                Graphics g = Graphics.FromImage(MainImage);
                SolidBrush brush = new SolidBrush(Color.Red);
                Pen pen1 = new Pen(Color.Red, 2);
                Font f = new Font("Segue UI", 8, FontStyle.Bold);

                g.DrawLine(pen1, startPointScaled, EndOfJumpMovePoint);
                g.DrawString(_lenString, f, brush, new PointF((startPointScaled.X + txtLocf), (startPointScaled.Y - 15)));
            }
        }

        //    }
        //}
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

                //_lenString = String.Format("{0} ft.", distanceD.ToString("N1"));
                _lenString = String.Format("{0:N1} ft.", distanceD.ToString());
                txtLocf = ((distanceD * _currentScale) / 2);

                if (draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1 = new Pen(Color.Red, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1, StartX, StartY, StartX, (StartY - (distanceD * _currentScale)));
                    g.DrawString(_lenString, f, brush, new PointF((StartX + 15), (StartY - txtLocf)));
                }
                if (!draw)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    SolidBrush brush = new SolidBrush(Color.Black);
                    Pen pen1 = new Pen(Color.Cyan, 5);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

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

        //        PointF dbStartPoint = SMGlobal.ScaledPointToDbPoint((decimal)startPoint.X, (decimal)startPoint.Y, scale, SketchOrigin);
        //        PointF endPoint = new PointF(endPointX, endPointY);
        //        decimal dbStartX = (decimal)dbStartPoint.X;
        //        decimal dbStartY = (decimal)dbStartPoint.Y;
        //        decimal dbEndY = dbStartY;
        //        decimal dbEndX = dbStartX + distance;
        //        int nextLineNumber = (from l in workingSection.Lines select l.LineNumber).Max() + 1;
        //        SMLine newLine = new SMLine { Record = workingSection.Record, Dwelling = workingSection.Dwelling, SectionLetter = workingSection.SectionLetter, Direction = "E", XLength = Math.Round((distance / scale), 2), ParentParcel = workingSection.ParentParcel, ParentSection = workingSection, StartX = dbStartX, StartY = dbStartY, EndX = dbEndX, EndY = dbEndY };
        //        workingSection.Lines.Add(newLine);
        //        LocalParcelCopy.SnapShotIndex++;
        //        SketchUpGlobals.SketchSnapshots.Add(LocalParcelCopy);
        //        Graphics g = Graphics.FromImage(MainImage);
        //        SolidBrush brush = new SolidBrush(Color.Black);
        //        Pen pen1 = new Pen(Color.Cyan, 5);
        //        Font f = new Font("Segue UI", 8, FontStyle.Bold);
        //        DrawLine(newLine, pen1, false);
        //        // Do I need to do this? EraseSectionFromDrawing(workingSection);
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

                ExpSketchPBox.Image = MainImage;

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

        //public void AddEastLineToSection(PointF startPoint, decimal distance)
        //{
        //    SMSection workingSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == s.ParentParcel.LastSectionLetter select s).FirstOrDefault();
        //    if (workingSection!=null)
        //    {
        //        decimal scale = LocalParcelCopy.Scale;
        //        float endPointX = startPoint.X + (float)(distance * LocalParcelCopy.Scale);
        //        float endPointY = startPoint.Y;
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

                ExpSketchPBox.Image = MainImage;

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

                ExpSketchPBox.Image = MainImage;

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

                ExpSketchPBox.Image = MainImage;

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

                ExpSketchPBox.Image = MainImage;

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

                ExpSketchPBox.Image = MainImage;

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

        public void MoveWestToBegin(PointF startPointScaled, decimal lineLength)
        {
            if (_isKeyValid == true)
            {
                decimal sketchScale = LocalParcelCopy.Scale;
                PointF origin = LocalParcelCopy.SketchOrigin;
                DbLineLengthX = lineLength;
                ScaledStartOfMovement = startPointScaled;
                MovementDistanceScaled = lineLength * sketchScale;
                float newX = startPointScaled.X - (float)movementDistanceScaled;
                ScaledEndOfMovement = new PointF(newX, startPointScaled.Y);
                DbMovementStartPoint = SMGlobal.ScaledPointToDbPoint((decimal)ScaledStartOfMovement.X, (decimal)ScaledStartOfMovement.Y, sketchScale, origin);
                Graphics g = Graphics.FromImage(MainImage);
                SolidBrush brush = new SolidBrush(Color.Red);
                Pen pen1 = new Pen(Color.Red, 2);
                Font f = new Font("Segue UI", 8, FontStyle.Bold);

                g.DrawLine(pen1, startPointScaled, ScaledEndOfMovement);

                g.DrawString(_lenString, f, brush, new PointF((ScaledStartOfMovement.X + txtLocf), (ScaledStartOfMovement.Y - 15)));
                g.Save();
                ExpSketchPBox.Refresh();
                ScaledStartOfMovement = ScaledEndOfMovement;

                DistText.Focus();
                BeginSectionBtn.Enabled = true;
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
            SMParcel newCopy = SketchUpGlobals.ParcelWorkingCopy;
            newCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(newCopy);
            var AttachPoints = (from l in
                  SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s =>
                  s.SectionLetter != "A" && s.LineNumber == 1).ToList()
                                select l);
            if (AttachPoints.ToList().Count > 0)
            {
                newCopy.Sections.Where(s => s.SectionLetter ==
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
                SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s =>
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
            newCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(newCopy);
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

        public void SplitLine()
        {
            int ln = _savedAttLine;
            string asec = _savedAttSection;
            string newSec = NextSectLtr;
            string tstdir = CurrentAttDir;
            decimal begx = OrigStartX;
            decimal begy = OrigStartY;
            decimal OldbegX = startSplitX;
            decimal OldbegY = startSplitY;
            decimal OldbegX1 = OrigStartX;
            decimal OldbegY1 = OrigStartY;
            float oldx = endOldSecX;
            float oldy = endOldSecY;
            string splitLineDir = offsetDir;
            string CurAttDir = String.Empty;
            string NextAttDir = String.Empty;
            decimal OrigY = OrigStartY;
            decimal newEndX = 0;
            decimal newEndY = 0;
            decimal newEndX1 = EndSplitX;
            decimal newEndY1 = EndSplitY;

            //TODO: See if setting this to absolute value will correct the problem.
            decimal finalNewLen = (OrigLineLength - splitLineDist);

            float mySx = BeginSplitX;

            float mySy = BeginSplitY;

            RemainderLineLength = finalNewLen;

            JumpTable = new DataTable();

            int chekattline = TempAttSplineNo;

            _savedAttLine = AttSpLineNo;
            if (AttSpLineNo == 0)
            {
                _savedAttLine = TempAttSplineNo;
                AttSpLineNo = TempAttSplineNo;
            }

            if (MultiSectionSelection.adjsec != String.Empty)
            {
                _savedAttSection = MultiSectionSelection.adjsec;
            }

            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
                           SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            _savedAttSection));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = SketchUpGlobals.Record;
                    row["Card"] = SketchUpGlobals.Card;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    SectionTable.Rows.Add(row);
                }
            }

            CurrentAttDir = offsetDir;

            // TODO: Remove if not needed:	string tstOffset = offsetDir;

            CurAttDir = CurrentAttDir;

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                string getNCurDir = string.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline#= {5} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix, SketchUpGlobals.Record, SketchUpGlobals.Card, _savedAttSection, AttSpLineNo);

                CurrentAttDir = dbConn.DBConnection.ExecuteScalar(getNCurDir.ToString()).ToString();
            }

            _savedAttLine = AttSpLineNo;

            if (_savedAttLine == 0)
            {
            }

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                StringBuilder nxtAttDir = new StringBuilder();
                nxtAttDir.Append(String.Format("select jldirect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' and jlline# = {5} ",
                               SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                SketchUpGlobals.Record,
                                SketchUpGlobals.Card,
                                CurrentSecLtr,
                                _savedAttLine));

                NextAttDir = dbConn.DBConnection.ExecuteScalar(nxtAttDir.ToString()).ToString();
            }

            decimal myoriglen = 0;
            mylineNo = 0;

            if (CurAttDir.Trim() == offsetDir.Trim())
            {
                StringBuilder getoriglen = new StringBuilder();
                getoriglen.Append(String.Format("select jllinelen,jlline# from {0}.{1}line where jlrecord = {2} and jldwell = {3}  and jlsect = '{4}' and jlline# = {5}",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            CurrentSecLtr,
                            _savedAttLine));
                getoriglen.Append(String.Format(" and jldirect = '{0}'", offsetDir));

                DataSet myds = dbConn.DBConnection.RunSelectStatement(getoriglen.ToString());

                if (myds.Tables[0].Rows.Count > 0)
                {
                    myoriglen = Convert.ToDecimal(myds.Tables[0].Rows[0]["jllinelen"].ToString());
                    mylineNo = Convert.ToInt32(myds.Tables[0].Rows[0]["jlline#"].ToString());
                }
            }

            int maxLineCnt = 0;

            decimal origEndx = 0;
            decimal origEndy = 0;
            decimal origDist = 0;
            decimal newDistX = 0;
            decimal newDistY = 0;
            decimal StartEndX = 0;
            decimal StartEndY = 0;
            decimal EndEndX = 0;
            decimal EndEndY = 0;
            decimal origLen = 0;
            decimal origXlen = 0;
            decimal origYlen = 0;
            decimal NewSplitDist = 0;

            StringBuilder getOrigEnds = new StringBuilder();
            getOrigEnds.Append(String.Format("select jldirect,jlsect,jlline#,jlxlen,jlylen,jllinelen,jlpt1x,jlpt1y,jlpt2x,jlpt2y from {0}.{1}line ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix
                            ));
            getOrigEnds.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr, mylineNo));

            DataSet orgLin = dbConn.DBConnection.RunSelectStatement(getOrigEnds.ToString());

            if (orgLin.Tables[0].Rows.Count > 0)
            {
                origEndx = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
                origEndy = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

                endOldSecX = Convert.ToSingle(origEndx);
                endOldSecY = Convert.ToSingle(origEndy);

                StartEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1x"].ToString());
                StartEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt1y"].ToString());

                EndEndX = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2x"].ToString());
                EndEndY = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlpt2y"].ToString());

                CurrentAttDir = orgLin.Tables[0].Rows[0]["jldirect"].ToString().Trim();
                origLen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

                origXlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlxlen"].ToString());
                origYlen = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jlylen"].ToString());

                origDist = Convert.ToDecimal(orgLin.Tables[0].Rows[0]["jllinelen"].ToString());

                NewSplitLIneDist = origLen - splitLineDist;
            }

            NewSplitDist = Math.Abs(myoriglen - splitLineDist);

            decimal origDistX = OriginalDistanceX();

            if (origDistX != myoriglen)
            {
                origDistX = myoriglen;
            }

            maxLineCnt = MaximumLineCount();

            if (maxLineCnt > 0 && mylineNo <= maxLineCnt)
            {
                decimal ttue = splitLineDist;
                StringBuilder incrLine = new StringBuilder();
                incrLine.Append(String.Format("update {0}.{1}line set jlline# = jlline#+1 ",
                          SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
                incrLine.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# > {3} ", SketchUpGlobals.Record, SketchUpGlobals.Card, CurrentSecLtr, _savedAttLine));

                //Ask Dave why this is not excecuting. Can this whole section be removed?
                //fox.DBConnection.ExecuteNonSelectStatement(incrLine.ToString());

                if (_savedAttLine > 0 && RemainderLineLength != 0)
                {
                    for (int i = _savedAttLine - 1; i < SectionTable.Rows.Count; i++)
                    {
                        int tske = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());

                        SectionTable.Rows[i]["LineNo"] = tske + 1;
                    }
                }

                DeleteLineSection();

                for (int i = 0; i < SectionTable.Rows.Count; i++)
                {
                    StringBuilder instLine = new StringBuilder();
                    instLine.Append(String.Format("insert into {0}.{1}line ",
                                  SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
                    instLine.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                                        Convert.ToInt32(SectionTable.Rows[i]["Record"].ToString()),
                                        Convert.ToInt32(SectionTable.Rows[i]["Card"].ToString()),
                                        SectionTable.Rows[i]["Sect"].ToString().Trim(),
                                        Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()),
                                        SectionTable.Rows[i]["Direct"].ToString().Trim(),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Xlen"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Ylen"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["Angle"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString()),
                                        Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString()),
                                        SectionTable.Rows[i]["Attach"].ToString().Trim()));

                    decimal td1 = Convert.ToDecimal(SectionTable.Rows[i]["Length"].ToString());

                    decimal td2 = Convert.ToDecimal(SectionTable.Rows[i]["XPt1"].ToString());

                    decimal td3 = Convert.ToDecimal(SectionTable.Rows[i]["YPt1"].ToString());

                    decimal td4 = Convert.ToDecimal(SectionTable.Rows[i]["XPt2"].ToString());

                    decimal td5 = Convert.ToDecimal(SectionTable.Rows[i]["YPt2"].ToString());

                    if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) <= 20)
                    {
                        dbConn.DBConnection.ExecuteNonSelectStatement(instLine.ToString());
                    }
                    if (Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString()) > 20)
                    {
                        MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    }
                }
            }

            decimal xlen = 0;
            decimal ylen = 0;

            if (offsetDir == "N" || offsetDir == "S")  // offsetDir
            {
                xlen = 0;
                ylen = splitLineDist;
                newDistX = 0;
                newDistY = origDist - splitLineDist;
                adjNewSecX = 0;
                adjNewSecY = splitLineDist;
            }
            if (offsetDir == "E" || offsetDir == "W")   // offsetDir
            {
                xlen = splitLineDist;
                ylen = 0;
                newDistX = origDist - splitLineDist;
                newDistY = 0;
                adjNewSecY = 0;
                adjNewSecX = splitLineDist;
            }

            decimal x1t = adjNewSecX;
            decimal y1t = adjNewSecY;

            decimal splitLength = splitLineDist;

            if (OrigLineLength != (RemainderLineLength + splitLineDist))
            {
                OrigLineLength = (RemainderLineLength + splitLineDist);
            }

            if (CurAttDir.Trim() == offsetDir.Trim())
            {
                decimal utp = splitLength;

                decimal ttue = splitLineDist;
                StringBuilder fixOrigLine = new StringBuilder();
                fixOrigLine.Append(String.Format("update {0}.{1}line ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix
                            ));
                fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
                                        adjNewSecX,
                                        adjNewSecY,
                                        splitLength,
                                        NewSectionBeginPointX,
                                        NewSectionBeginPointY));
                fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                SketchUpGlobals.Record,
                                SketchUpGlobals.Card,
                                CurrentSecLtr,
                                _savedAttLine));

                //Ask Dave why this is not being called. Can it be removed?
                //fox.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
            }

            if (CurAttDir.Trim() != offsetDir.Trim())
            {
                if (offsetDir == "N" || offsetDir == "S")
                {
                    RemainderLineLength = OrigLineLength - adjNewSecY;
                }
                if (offsetDir == "W" || offsetDir == "E")
                {
                    adjNewSecX = RemainderLineLength;
                }
                adjNewSecY = RemainderLineLength;

                if (RemainderLineLength > 0)
                {
                    FixOrigLine();
                }
            }

            if (offsetDir == "N" || offsetDir == "S")
            {
                adjOldSecX = 0;
                adjOldSecY = RemainderLineLength;
            }

            if (offsetDir == "E" || offsetDir == "W")
            {
                adjOldSecY = 0;
                adjOldSecX = RemainderLineLength;
            }

            int newLineNbr = _savedAttLine + 1;

            if (CurAttDir.Trim() != splitLineDir.Trim())
            {
                splitLineDir = CurAttDir;
            }

            decimal endNewSecX = adjNewSecX;
            switch (AttSpLineDir)
            {
                case "E":
                    newEndX = (OrigStartX + splitLineDist);
                    newEndY = OrigStartY;
                    break;

                case "W":
                    endNewSecX = adjNewSecX * -1;
                    newEndX = (OrigStartX - splitLineDist);
                    newEndY = OrigStartY;
                    break;

                case "N":
                    newEndY = (OrigStartY - splitLineDist);
                    newEndX = OrigStartX;
                    break;

                case "S":

                    EndSplitY = EndSplitY * -1;
                    newEndY = (OrigStartY + splitLineDist);
                    newEndX = OrigStartX;
                    break;

                default:
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. AttSpLineDir not in NEWS. ", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
                    break;
            }

            if (RemainderLineLength > 0)
            {
                InsertLine(CurAttDir, newEndX, newEndY, StartEndX, StartEndY, splitLength);
            }

            decimal jjel = OrigLineLength;

            decimal finEndX = 0;
            decimal finEndY = 0;
            decimal finDist = 0;
            switch (CurrentAttDir)
            {
                case "N":

                    finDist = (OrigStartY - splitLineDist);

                    finDist = newDistY;
                    break;

                case "S":

                    finDist = (OrigStartY + splitLineDist);
                    finDist = newDistY;
                    break;

                case "E":

                    finDist = (OrigStartX + splitLineDist);

                    finDist = newDistX;
                    break;

                case "W":

                    finDist = (OrigStartX - splitLineDist);
                    finDist = newDistX;
                    break;

                default:
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}. CurrentAttDir not in NEWS.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name));
                    break;
            }
            AdjustLine(newEndX, newEndY, newDistX, newDistY, EndEndX, EndEndY, finDist);
        }

        #endregion "Public Methods"

        #region "Private methods"

        private static List<SMLine> LinesWithClosestEndpoints(PointF mouseLocation)
        {
            foreach (SMLine l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines.Where(s => s.SectionLetter != SketchUpGlobals.ParcelWorkingCopy.LastSectionLetter))
            {
                l.ComparisonPoint = mouseLocation;
            }
            decimal shortestDistance = Math.Round((from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines select l.EndPointDistanceFromComparisonPoint).Min(), 2);
            List<SMLine> connectionLines = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where Math.Round(l.EndPointDistanceFromComparisonPoint, 2) == shortestDistance select l).ToList();
            return connectionLines;
        }

        //    if (AngleForm.NorthWest == true)
        //    {
        //        MoveNorthWest(NextStartX, NextStartY);
        //    }
        private static Color PenColorForDrawing(MovementMode movementType)
        {
            Color penColor;
            switch (movementType)
            {
                case MovementMode.Draw:
                    penColor = Color.Teal;
                    break;

                case MovementMode.Erase:
                    penColor = Color.White;
                    break;

                case MovementMode.Jump:
                case MovementMode.MoveDrawRed:
                    penColor = Color.Red;

                    break;

                case MovementMode.MoveNoLine:
                case MovementMode.NoMovement:

                default:
                    penColor = Color.Transparent;
                    break;
            }

            return penColor;
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

        private void AddParcelToSnapshots(SMParcel localParcelCopy)
        {
            if (localParcelCopy != null)
            {
                SketchUpGlobals.SketchSnapshots.Add(localParcelCopy);
            }
        }

        private void AddSectionBtn_Click(object sender, EventArgs e)
        {
            GetSectionTypeInfo();
            AddNewPoint();
            _deleteMaster = false;

            BeginSectionBtn.BackColor = Color.Orange;
            BeginSectionBtn.Text = "Begin";

            _isClosed = false;
        }

        private void addSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetSectionTypeInfo();
        }

        private void AddXLine(string sectionLetter)
        {
            SMSection thisSection = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();
            SMLine xLine = new SMLine { Record = thisSection.Record, Dwelling = thisSection.Dwelling, SectionLetter = thisSection.SectionLetter, LineNumber = thisSection.Lines.Count + 1, StartX = 0, StartY = 0, EndX = 0, EndY = 0, ParentParcel = thisSection.ParentParcel, Direction = "X" };
            SketchUpGlobals.ParcelWorkingCopy.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault<SMSection>().Lines.Add(xLine);
        }

        private void AutoClose()
        {
            PromptToSaveOrDiscard();

            ExpSketchPBox.Image = MainImage;
        }

        private void beginPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void BeginSectionBtn_Click(object sender, EventArgs e)
        {
            SketchingState = SketchDrawingState.BeginPointSelected;

            if (_addSection == false)
            {
                MessageBox.Show("Must select additon type ", "Missing Addition warning");
            }
            if (_addSection == true)
            {
                Xadj = (((ScaleBaseX - _mouseX) / _currentScale) * -1);
                Yadj = (((ScaleBaseY - _mouseY) / _currentScale) * -1);

                offsetDir = LastDir;

                if (Xadj != NextStartX)
                {
                    Xadj = NextStartX;
                }

                if (Yadj != NextStartY)
                {
                    Yadj = NextStartY;
                }

                NewSectionBeginPointX = Math.Round(Convert.ToDecimal(Xadj), 1);
                NewSectionBeginPointY = Math.Round(Convert.ToDecimal(Yadj), 1);

                if (_hasNewSketch == true)
                {
                    Xadj = 0;
                    Yadj = 0;
                }

                if (_hasNewSketch == true)
                {
                    AttSectLtr = "A";
                }
                if (AttSectLtr == String.Empty)
                {
                    AttSectLtr = JumpTable.Rows[0]["Sect"].ToString().Trim();
                }

                AttSpLineDir = offsetDir;

                splitLineDist = distance;

                startSplitX = NewSectionBeginPointX;
                startSplitY = NewSectionBeginPointY;

                getSplitLine();

                int _attLineNo = AttSpLineNo;

                draw = true;

                lineCnt = 0;

                //DMouseClick();
            }
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
#if DEBUG
                    StringBuilder traceOut = new StringBuilder();
                    traceOut.AppendLine(string.Format("Section Adding SQL: {0}", addSect.ToString()));
                    Console.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
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

        private void ClearXLinesFromSections()
        {
            if (draw != false)
            {
                StringBuilder delXdir = new StringBuilder();
                delXdir.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jldirect = 'X'",
                               SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                SketchUpGlobals.Record,
                                SketchUpGlobals.Card));

                dbConn.DBConnection.ExecuteNonSelectStatement(delXdir.ToString());
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

        private void DeleteSection()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void deleteSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSection();

            RefreshSketch();
        }

        private void DiscardChangesAndExit()
        {
            SketchUpGlobals.SketchSnapshots.Clear();
            SketchUpGlobals.SMParcelFromData.SnapShotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(SketchUpGlobals.SMParcelFromData);

            MessageBox.Show(
                string.Format("Reverting to Version {0} with {1} Sections.",
                SketchUpGlobals.SMParcelFromData.SnapShotIndex,
                SketchUpGlobals.SMParcelFromData.Sections.Count));

            this.Close();
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
            if (jumpMode == true)
            {
                _isJumpMode = true;
                draw = false;
                _mouseX = X;
                _mouseY = Y;
            }
            else
            {
                _isJumpMode = false;
                draw = true;
                Graphics g = Graphics.FromImage(MainImage);
                Pen pen1 = new Pen(Color.White, 4);
                g.DrawRectangle(pen1, X, Y, 1, 1);
                g.Save();

                ExpSketchPBox.Image = MainImage;

                //click++;
                //savpic.Add(click, imageToByteArray(_mainimage));
            }
        }

        //    legalDirections.AddRange((from l in LocalParcelCopy.AllSectionLines where l.ScaledEndPoint == scaledJumpPoint  select ReverseDirection(l.Direction)).ToList());
        //    return legalDirections.Distinct().ToList();
        //}
        //ToDo: Begin here to hook in parcel updates
        private void DoneDrawingBtn_Click(object sender, EventArgs e)
        {
            ReorderParcelStructure();
            RefreshParcelImage();
            SetActiveButtonAppearance();
            jumpToolStripMenuItem.Enabled = false;
            AddSectionContextMenu.Enabled = false;
        }

        //    if (_isKeyValid == false)
        //    {
        //        _isKeyValid = true;
        //    }
        private void DrawLineOnSketch()
        {
            decimal scale = LocalParcelCopy.Scale;

            switch (SketchingState)
            {
                case SketchDrawingState.BeginPointSelected:
                    break;

                case SketchDrawingState.Drawing:
                    break;

                case SketchDrawingState.JumpMoveToBeginPoint:
                    break;

                case SketchDrawingState.JumpPointSelected:
                    break;

                case SketchDrawingState.UndoLastLine:

                    break;

                default:
                    break;
            }
        }

        //    AngleForm angleDialog = new AngleForm();
        //    angleDialog.ShowDialog();
        private void DrawTealLine(PointF startPoint, PointF endPoint, decimal distance, decimal scaledDistance)
        {
            Pen pen = new Pen(Color.Teal, 2);
            Font font = new Font("Segoe UI", 7);
            Graphics g = Graphics.FromImage(MainImage);
            decimal scale = LocalParcelCopy.Scale;
            g.DrawLine(pen, startPoint, endPoint);
            Brush brush = Brushes.Black;
            float labelX;
            float labelY;
            PointF labelPoint = startPoint;
            switch (DirectionOfMovement)
            {
                case MoveDirections.N:

                    labelX = startPoint.X - (float)(10 * scale);
                    labelY = startPoint.Y - (Math.Abs(startPoint.Y - endPoint.Y) / 2f);
                    labelPoint = new PointF(labelX, labelY);
                    break;

                case MoveDirections.E:

                    break;

                case MoveDirections.S:
                    labelX = startPoint.X - (float)(10 * scale);
                    labelY = endPoint.Y + (Math.Abs(endPoint.Y - startPoint.Y) / 2f);
                    labelPoint = new PointF(labelX, labelY);
                    break;

                case MoveDirections.W:
                    break;

                case MoveDirections.None:
                    break;

                default:
                    break;
            }
            if (labelPoint != startPoint)//TODO: Complete

            //Ignore if I haven't gotten to the placement yet
            {
                g.DrawLine(pen, startPoint, endPoint);
                g.DrawString(distance.ToString(), font, brush, labelPoint);
            }
        }

        private void EastDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveEast(NextStartX, NextStartY);
            DistText.Focus();
        }

        //private List<string> GetLegalMoveDirections(PointF scaledJumpPoint)
        //{
        //      List<string> legalDirections = new List<string>();
        //    legalDirections.AddRange((from l in LocalParcelCopy.AllSectionLines where l.ScaledStartPoint == scaledJumpPoint select l.Direction).ToList());
        private void endSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        //    AngD1 = Convert.ToDecimal(D2);
        private void EraseSectionFromDrawing(SMSection workingSection)
        {
            //TODO: Draw everything with a white pen to earase the section and redraw it.
        }

        private void ExpandoSketch_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClearXLinesFromSections();

            //All database updates will happen with an explicit save command, not as part of another operation. JMM 5-9-2016
            //AddMaster();
        }

        private void ExpandoSketch_Shown(object sender, EventArgs e)
        {
        }

        private void ExpSketchPbox_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isJumpMode)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                Graphics g = Graphics.FromImage(MainImage);
                Pen pen1 = new Pen(Color.Red, 4);
                g.DrawRectangle(pen1, e.X, e.Y, 1, 1);
                g.Save();

                //DMouseClick();
            }
        }

        private void ExpSketchPbox_MouseDown(object sender, MouseEventArgs e)
        {
            _isJumpMode = false;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _mouseX = e.X;
                _mouseY = e.Y;

                DMouseMove(e.X, e.Y, false);
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                _isJumpMode = true;
                _mouseX = e.X;
                _mouseY = e.Y;
                DMouseMove(e.X, e.Y, true);
            }
        }

        private void ExpSketchPbox_MouseMove(object sender, MouseEventArgs e)
        {
            string statusText = string.Format("{0},{1}", e.X, e.Y);
            SetMainStatusText(statusText);
        }

        private void ExpSketchPbox_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private string FindClosestCorner(float CurrentScale, ref string curltr, List<string> AttSecLtrList)
        {
            string secltr;
            decimal dist1 = 0;
            decimal dist1x = 0;
            decimal dist2 = 0;
            decimal distX = 0;
            int rowindex = 0;

            //was called dv--renamed for clarity
            DataView SortedJumpTableDataView = new DataView(JumpTable);
            SortedJumpTableDataView.Sort = "Dist ASC";

            BeginSplitX = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString()));
            BeginSplitY = (float)(Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString()));

            NextStartX = BeginSplitX;
            NextStartY = BeginSplitY;

            for (int i = 0; i < SortedJumpTableDataView.Count; i++)
            {
                dist1 = Convert.ToDecimal(JumpTable.Rows[i]["Dist"].ToString());
                dist1x = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());

                if (i == 0)
                {
                    dist2 = dist1;
                    rowindex = i;
                }

                if (dist1 <= dist2 && i > 0)
                {
                    dist2 = dist1;
                    rowindex = i;
                }
            }

            distX = Convert.ToDecimal(SortedJumpTableDataView[0]["Dist"].ToString());

            secltr = SortedJumpTableDataView[0]["Sect"].ToString();
            AttSecLtrList.Add(secltr);
            int cntsec = 0;

            for (int i = 1; i < SortedJumpTableDataView.Count; i++)
            {
                decimal distx2 = Convert.ToDecimal(SortedJumpTableDataView[i]["Dist"].ToString());
                curltr = SortedJumpTableDataView[i]["Sect"].ToString();

                if (distx2 == distX)
                {
                    cntsec++;
                    AttSecLtrList.Add(curltr);
                }
            }

            /* Joey's attempt to simplify the determination of the closest points and populate the multi-attach section if there are more than one.

            List<PointWithComparisons> possibleAttachmentPoints = ClosestPoints(JumpTable, new PointF(JumpX, JumpY));
            if (possibleAttachmentPoints.Count > 1)
            {
                AttSecLtrList.Clear();
                foreach (PointWithComparisons p in possibleAttachmentPoints)
                {
                    AttSecLtrList.Add(p.PointLabel);
                }
            }
            else
            {
                secltr = possibleAttachmentPoints.FirstOrDefault<PointWithComparisons>().PointLabel;
            }

    End Joey's alternative Code */

            string multisectatch = MultiPointsAvailable(AttSecLtrList);

            SaveJumpPointsAndOldSectionEndPoints(CurrentScale, rowindex, SortedJumpTableDataView);

            string _CurrentSecLtr = JumpTable.Rows[rowindex]["Sect"].ToString();

            //Ask Dave why this is set here if it is set differently below
            //  Rube Goldberg code. Value is set again below, so I commented this one out.
            //	CurrentSecLtr = SortedJumpTableDataView[0]["Sect"].ToString();

            int savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

            _savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());
            Console.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(JumpTable.Rows[rowindex][LineNo]={0}", _savedAttLine));
            Console.WriteLine(string.Format("************ ({0} is not subsequently used.******** ", _savedAttLine));
            Console.WriteLine(string.Format("_savedAttLine = Convert.ToInt32(SortedJumpTableDataView[0][LineNo]={0}", _savedAttLine));

            //Ask Dave why this is set here if it is set differently above
            //Ask Dave why sometimes the rowindex of the JumpTable is used and othertimes the row[0] of the Sorted Jump Table
            CurrentSecLtr = _CurrentSecLtr;

            if (AttSecLtrList.Count > 1)
            {
                CurrentSecLtr = multisectatch;
            }

            string priorDirection = JumpTable.Rows[rowindex]["Direct"].ToString();

            string savedAttSection = JumpTable.Rows[rowindex]["Sect"].ToString();
            int _CurrentAttLine = Convert.ToInt32(JumpTable.Rows[rowindex]["LineNo"].ToString());

            startSplitX = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt1"].ToString());
            startSplitY = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt1"].ToString());
            Console.WriteLine(string.Format("Start split point: {0},{1}", startSplitX, startSplitY));
            /* More Rube Goldberg code. These values are set, but then they are not used anywhere.
             -JMM
                        decimal tsplit2 = Convert.ToDecimal(SortedJumpTableDataView[0]["XPt2"].ToString());
                        decimal tsplit3 = Convert.ToDecimal(SortedJumpTableDataView[0]["YPt2"].ToString());
            */
            _priorDirection = SortedJumpTableDataView[0]["Direct"].ToString();
            _savedAttSection = SortedJumpTableDataView[0]["Sect"].ToString();
            currentAttachmentLine = Convert.ToInt32(SortedJumpTableDataView[0]["LineNo"].ToString());

            //TODO: Find the last moved direction and the direction of the Current AttLine. If they are not the same call undo and return to main screen.
            _mouseX = Convert.ToInt32(JumpX);
            _mouseY = Convert.ToInt32(JumpY);
            Console.WriteLine(string.Format("Mouse moved to {0},{1}", JumpX, JumpY));
            Console.WriteLine(string.Format("Section attachment is {0} Line {1}, _priorDirection is {2}", _savedAttSection, currentAttachmentLine, _priorDirection));

            //LegalMoveDirections = AttachLineDirection(_savedAttSection, currentAttachmentLine);
            MoveCursor();
            return secltr;
        }

        private void FixOrigLine()
        {
            StringBuilder fixOrigLine = new StringBuilder();
            fixOrigLine.Append(String.Format("update {0}.{1}line ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPrefix));
            fixOrigLine.Append(String.Format("set jlxlen = {0},jlylen = {1}, jllinelen = {2}, jlpt2x = {3}, jlpt2y = {4} ",
                                    adjNewSecX,
                                    adjNewSecY,
                                    RemainderLineLength,
                                    NewSectionBeginPointX,
                                    NewSectionBeginPointY));
            fixOrigLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card,
                            CurrentSecLtr,
                            _savedAttLine));

            dbConn.DBConnection.ExecuteNonSelectStatement(fixOrigLine.ToString());
        }

        private void FlipLeftRight()
        {
            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = SketchUpGlobals.Record;
                    row["Card"] = SketchUpGlobals.Card;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

                    string mytestdir = row["Direct"].ToString();

                    if (mytestdir == "E")
                    {
                        row["Direct"] = "W";
                    }
                    if (mytestdir == "NE")
                    {
                        row["Direct"] = "NW";
                    }
                    if (mytestdir == "SE")
                    {
                        row["Direct"] = "SW";
                    }
                    if (mytestdir == "W")
                    {
                        row["Direct"] = "E";
                    }
                    if (mytestdir == "NW")
                    {
                        row["Direct"] = "NE";
                    }
                    if (mytestdir == "SW")
                    {
                        row["Direct"] = "SE";
                    }

                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) != 0)
                    {
                        row["Xpt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString()) * -1);
                    }
                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) != 0)
                    {
                        row["Xpt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString()) * -1);
                    }

                    SectionTable.Rows.Add(row);
                }
            }

            for (int i = 0; i < SectionTable.Rows.Count; i++)
            {
                string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
                int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
                string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
                decimal fXpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt1"].ToString());
                decimal fXpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Xpt2"].ToString());

                StringBuilder flipit = new StringBuilder();
                flipit.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1x = {3}, jlpt2x = {4} ",
                               SketchUpGlobals.LocalLib,
                               SketchUpGlobals.LocalityPrefix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                fdirect,
                                fXpt1,
                                fXpt2));
                flipit.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                SketchUpGlobals.Record, SketchUpGlobals.Card, fsect, flineno));

                dbConn.DBConnection.ExecuteNonSelectStatement(flipit.ToString());
            }

            _closeSketch = true;

            RefreshSketch();
        }

        private void FlipUpDown()
        {
            StringBuilder sectable = new StringBuilder();
            sectable.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach ");
            sectable.Append(String.Format(" from {0}.{1}line where jlrecord = {2} and jldwell = {3}  ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPrefix,

                            //SketchUpGlobals.FcLib,
                            //SketchUpGlobals.FcLocalityPrefix,
                            SketchUpGlobals.Record,
                            SketchUpGlobals.Card));

            DataSet scl = dbConn.DBConnection.RunSelectStatement(sectable.ToString());

            if (scl.Tables[0].Rows.Count > 0)
            {
                SectionTable.Clear();

                for (int i = 0; i < scl.Tables[0].Rows.Count; i++)
                {
                    DataRow row = SectionTable.NewRow();
                    row["Record"] = SketchUpGlobals.Record;
                    row["Card"] = SketchUpGlobals.Card;
                    row["Sect"] = scl.Tables[0].Rows[i]["jlsect"].ToString().Trim();
                    row["LineNo"] = Convert.ToInt32(scl.Tables[0].Rows[i]["jlline#"].ToString());
                    row["Direct"] = scl.Tables[0].Rows[i]["jldirect"].ToString().Trim();
                    row["Xlen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlxlen"].ToString());
                    row["Ylen"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlylen"].ToString());
                    row["Length"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jllinelen"].ToString());
                    row["Angle"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlangle"].ToString());
                    row["Xpt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1x"].ToString());
                    row["Ypt1"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString());
                    row["Xpt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2x"].ToString());
                    row["Ypt2"] = Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString());
                    row["Attach"] = scl.Tables[0].Rows[i]["jlattach"].ToString().Trim();

                    string testd = scl.Tables[0].Rows[i]["jldirect"].ToString();

                    string mytestdir = row["Direct"].ToString();

                    if (mytestdir == "N")
                    {
                        row["Direct"] = "S";
                    }
                    if (mytestdir == "NE")
                    {
                        row["Direct"] = "SE";
                    }
                    if (mytestdir == "NW")
                    {
                        row["Direct"] = "SW";
                    }

                    if (mytestdir == "S")
                    {
                        row["Direct"] = "N";
                    }
                    if (mytestdir == "SE")
                    {
                        row["Direct"] = "NE";
                    }
                    if (mytestdir == "SW")
                    {
                        row["Direct"] = "NW";
                    }

                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) != 0)
                    {
                        row["Ypt1"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt1y"].ToString()) * -1);
                    }
                    if (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) != 0)
                    {
                        row["Ypt2"] = (Convert.ToDecimal(scl.Tables[0].Rows[i]["jlpt2y"].ToString()) * -1);
                    }

                    SectionTable.Rows.Add(row);
                }
            }

            for (int i = 0; i < SectionTable.Rows.Count; i++)
            {
                string fsect = SectionTable.Rows[i]["Sect"].ToString().Trim();
                int flineno = Convert.ToInt32(SectionTable.Rows[i]["LineNo"].ToString());
                string fdirect = SectionTable.Rows[i]["Direct"].ToString().Trim();
                decimal fYpt1 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt1"].ToString());
                decimal fYpt2 = Convert.ToDecimal(SectionTable.Rows[i]["Ypt2"].ToString());

                StringBuilder flipitFB = new StringBuilder();
                flipitFB.Append(String.Format("update {0}.{1}line set jldirect = '{2}', jlpt1y = {3}, jlpt2y = {4} ",
                                   SketchUpGlobals.LocalLib,
                                   SketchUpGlobals.LocalityPrefix,

                                     //SketchUpGlobals.FcLib,
                                     //SketchUpGlobals.FcLocalityPrefix,
                                     fdirect,
                                     fYpt1,
                                     fYpt2));
                flipitFB.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' and jlline# = {3} ",
                                SketchUpGlobals.Record, SketchUpGlobals.Card, fsect, flineno));

                dbConn.DBConnection.ExecuteNonSelectStatement(flipitFB.ToString());
            }

            _closeSketch = true;

            RefreshSketch();
        }

        //    AngD2 = Convert.ToDecimal(D1);
        private List<string> GetLegalMoveDirections(PointF scaledJumpPoint, string attachSectionLetter)
        {
            List<SMLine> linesWithJumpPoint = (from l in LocalParcelCopy.AllSectionLines where l.SectionLetter == attachSectionLetter && (l.ScaledStartPoint == scaledJumpPoint || l.ScaledEndPoint == scaledJumpPoint) select l).ToList();
            List<string> legalDirections = new List<string>();
            legalDirections.AddRange((from l in LocalParcelCopy.AllSectionLines where l.ScaledStartPoint == scaledJumpPoint && l.SectionLetter == attachSectionLetter select l.Direction).ToList());

            legalDirections.AddRange((from l in linesWithJumpPoint select l.Direction).Distinct().ToList());
            LegalMoveDirections = legalDirections;
            return legalDirections;
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

            string nextSectionLetter = LocalParcelCopy.NextSectionLetter;
            NewSectionPoints.Clear();
            lineCnt = 0;
            SelectSectionTypeDialog sectionTypeForm = new SelectSectionTypeDialog(LocalParcelCopy.ParcelMast, _addSection, lineCnt, IsNewSketch);

            sectionTypeForm.ShowDialog(this);

            //Ensure they did not just cancel out by checking that there is a new version of the parcel
            if (LocalParcelCopy.LastSectionLetter == nextSectionLetter)
            {
                NextSectLtr = SelectSectionTypeDialog._nextSectLtr;
                _nextSectType = SelectSectionTypeDialog._nextSectType;
                _nextStoryHeight = SelectSectionTypeDialog.newSectionStoreys;
                _nextLineCount = SelectSectionTypeDialog._nextLineCount;
                _hasNewSketch = (NextSectLtr == "A");

                AddSectionContextMenu.Enabled = true;
                jumpToolStripMenuItem.Enabled = true;
                SketchingState = SketchDrawingState.SectionAdded;
                _isJumpMode = true;
                try
                {
                    FieldText.Text = String.Format("Sect- {0}, {1} sty {2}", NextSectLtr.Trim(), _nextStoryHeight.ToString("N2"), _nextSectType.Trim());
                }
                catch
                {
                }
            }
        }

        private void GetStartCorner()
        {
            _undoMode = true;

            undoPoints.Clear();

            int rowindex = 0;

            for (int i = 0; i < REJumpTable.Rows.Count; i++)
            {
                DataRow row = undoPoints.NewRow();

                float _JumpX1 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt1"].ToString()) * _currentScale)); //  change XPt1 to XPt2
                float _JumpY1 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT1"].ToString()) * _currentScale));
                float _JumpX2 = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[i]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
                float _JumpY2 = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[i]["YPT2"].ToString()) * _currentScale));

                JumpX = _JumpX1;
                JumpY = _JumpY1;
                float JumpX2 = _JumpX2;
                float JumpY2 = _JumpY2;

                int _mouseX1 = Convert.ToInt32(JumpX);
                int _mouseY1 = Convert.ToInt32(JumpY);
                int _mouseX2 = Convert.ToInt32(JumpX2);
                int _mouseY2 = Convert.ToInt32(JumpY2);

                row["Direct"] = REJumpTable.Rows[i]["Direct"].ToString();
                row["X1pt"] = _mouseX1;
                row["Y1pt"] = _mouseY1;
                row["X2pt"] = _mouseX2;
                row["Y2pt"] = _mouseY2;

                undoPoints.Rows.Add(row);
            }

            RedrawSection();
        }

        //    string D2 = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();
        private void HandleDirectionalKeys(KeyEventArgs e)
        {
            string textEntered = string.Empty;
            decimal distanceValue = 0.00M;

            if (!string.IsNullOrEmpty(DistText.Text))
            {
                textEntered = DistText.Text;

                if (textEntered.IndexOf(",") > 0)
                {
                    _isAngle = true;
                    ParseAngleEntry(e, textEntered);
                }
                else
                {
                    decimal.TryParse(DistText.Text, out distanceValue);
                    MovementDistanceScaled = distanceValue * LocalParcelCopy.Scale;
                    SetDirectionOfKeyEntered(e);
                    HandleMovementByKey(DirectionOfMovement, distanceValue);
                }
            }
        }

        private void HandleEastKeys()
        {
            LastDir = "E";
            decimal distanceValue = 0;
            decimal.TryParse(DistText.Text, out distanceValue);
            if (_isAngle == false)
            {
                if (_isJumpMode || SketchingState == SketchDrawingState.JumpMoveToBeginPoint || SketchingState == SketchDrawingState.JumpPointSelected)
                {
                    MoveEastToBegin(ScaledJumpPoint, distanceValue);
                }
                else if (draw || SketchingState == SketchDrawingState.BeginPointSelected)
                {
                    ScaledBeginPoint = ScaledJumpPoint;

                    //TODO: Handle this with the addition of the line and the drawing of it using the LocalParcelCopy
                    //     AddEastLineToSection(ScaledBeginPoint, distanceValue);
                }
                DistText.Text = string.Empty;
                DistText.Focus();
            }
            if (_isAngle == true)
            {
                MeasureAngle();
            }
        }

        //    string D1 = anglecalls.Substring(0, commaCnt).Trim();
        private void HandleMovementByKey(MoveDirections direction, decimal distance)
        {
            decimal scale = LocalParcelCopy.Scale;
            decimal scaledDistance = distance * scale;
            MovementMode movementType;
            float endX;
            float endY;
            switch (SketchingState)
            {
                case SketchDrawingState.BeginPointSelected:

                case SketchDrawingState.Drawing:
                    movementType = MovementMode.Draw;
                    break;

                case SketchDrawingState.JumpMoveToBeginPoint:
                case SketchDrawingState.JumpPointSelected:
                    switch (direction)
                    {
                        //Regular directions
                        case MoveDirections.N:
                            endX = ScaledStartOfMovement.X;
                            endY = ScaledStartOfMovement.Y - (float)scaledDistance;

                            break;

                        case MoveDirections.E:
                            endX = ScaledStartOfMovement.X + (float)scaledDistance;
                            endY = ScaledStartOfMovement.Y;

                            break;

                        case MoveDirections.S:
                            endX = ScaledStartOfMovement.X;
                            endY = ScaledStartOfMovement.Y + (float)scaledDistance;

                            break;

                        case MoveDirections.W:
                            endX = ScaledStartOfMovement.X - (float)scaledDistance;
                            endY = ScaledStartOfMovement.Y;

                            break;

                        //TODO: Handle angles
                        //case MoveDirections.NE:
                        //    break;

                        //case MoveDirections.SE:
                        //    break;

                        //case MoveDirections.SW:
                        //    break;

                        //case MoveDirections.NW:
                        //    break;

                        //case MoveDirections.None:
                        //    break;

                        default:
                            endX = ScaledStartOfMovement.X;
                            endY = ScaledStartOfMovement.Y;
                            break;
                    }
                    ScaledEndOfMovement = new PointF(endX, endY);
                    DrawTealLine(ScaledStartOfMovement, ScaledEndOfMovement, distance, scaledDistance);
                    movementType = MovementMode.MoveDrawRed;
                    break;

                default:
                    movementType = MovementMode.MoveNoLine;
                    break;
            }

            PointF start = ScaledStartOfMovement;
        }

        //    int commaCnt = anglecalls.IndexOf(",");
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

        private void HandleNorthKeys()
        {
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
            LastDir = "W";
            if (_isJumpMode || SketchingState == SketchDrawingState.JumpMoveToBeginPoint || SketchingState == SketchDrawingState.JumpPointSelected)
            {
                decimal distanceValue = 0;
                decimal.TryParse(DistText.Text, out distanceValue);
                MoveWestToBegin(ScaledJumpPoint, distanceValue);
            }
            else if (draw)
            {
                MoveEast(NextStartX, NextStartY);
            }
            DistText.Text = string.Empty;
            DistText.Focus();
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream dh = new MemoryStream();
            imageIn.Save(dh, System.Drawing.Imaging.ImageFormat.Jpeg);
            return dh.ToArray();
        }

        //    StrtPts = ConstructStartPointsTable();
        //}
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

            //SectionLtrs = ConstructSectionLtrs();
            //AreaTable = ConstructAreaTable();
            //MultiplePoints = ConstructMulPtsTable();
            //AttachmentPointsDataTable = ConstructAttachmentPointsDataTable();
            //AttachPoints = ConstructAttachPointsDataTable();
            //DupAttPoints = ConstructDupAttPointsTable();
            //StrtPts = ConstructStartPointsTable();
        }

        //    DupAttPoints = ConstructDupAttPointsTable();
        private void InitializeDisplayDataGrid()
        {
            displayDataTable = ConstructDisplayDataTable();

            dgSections.DataSource = displayDataTable;
        }

        //    AttachPoints = ConstructAttachPointsDataTable();
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

        //    AttachmentPointsDataTable = ConstructAttachmentPointsDataTable();
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

        //    undoPoints = ConstructUndoPointsTable();
        //    sortDist = ConstructSortDistanceTable();
        private bool IsMovementAllowed(MoveDirections direction)
        {
            bool isAllowed = (displayDataTable.Rows.Count > 0);

            if (isAllowed && isInAddNewPointMode)
            {
                isAllowed = false;
            }

            if (isAllowed && "".Equals(DistText.Text))
            {
                ShowMessageBox("Please indicate a length");
                isAllowed = false;
            }

            if (isAllowed)
            {
                string dir = displayDataTable.Rows[dgSections.CurrentRow.Index]["Dir"].ToString();
                if (!dir.Equals(direction.ToString()))
                {
                    ShowMessageBox("Illegal Direction");
                    isAllowed = false;
                    RefreshSketch();
                }
            }

            if (isAllowed)
            {
                decimal len = 0M;
                switch (direction)
                {
                    case MoveDirections.N:
                    case MoveDirections.S:
                        len = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["North"].ToString());
                        break;

                    case MoveDirections.E:
                    case MoveDirections.W:
                        len = Convert.ToDecimal(displayDataTable.Rows[dgSections.CurrentRow.Index]["East"].ToString());
                        break;

                    default:
                        break;
                }

                if (len == 0M)
                {
                    isAllowed = false;
                }
                else if (len <= Convert.ToDecimal(DistText.Text))
                {
                    ShowMessageBox("Illegal Distance");
                    isAllowed = false;
                    RefreshSketch();
                }
            }

            return isAllowed;
        }

        //    MultiplePoints = ConstructMulPtsTable();
        private bool IsValidDirection(string moveDirection)
        {
            bool goodDir = (LegalMoveDirections.Contains(moveDirection) || BeginSectionBtn.Text == "Active" || !checkDirection);
            return goodDir;
        }

        //    AreaTable = ConstructAreaTable();
        //public void MeasureAngle()
        //{
        //    string anglecalls = DistText.Text.Trim();
        private void JumptoCorner()
        {
            // float CurrentScale = _currentScale;
            //int crrec = SketchUpGlobals.Record;
            //int crcard = SketchUpGlobals.Card;
            decimal scale = LocalParcelCopy.Scale;
            PointF origin = LocalParcelCopy.SketchOrigin;
            CurrentSecLtr = String.Empty;
            _newIndex = 0;
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
                        AttachmentSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = (from l in connectionLines where l.SectionLetter == AttSectLtr select l).FirstOrDefault();
                        currentAttachmentLine = JumpPointLine.LineNumber;
                        CurrentSecLtr = AttachmentSection.SectionLetter;
                    }
                    else
                    {
                        AttSectLtr = SecLetters[0];
                        AttachmentSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = connectionLines[0];
                        currentAttachmentLine = JumpPointLine.LineNumber;
                        CurrentSecLtr = AttachmentSection.SectionLetter;
                    }

                    ScaledJumpPoint = JumpPointLine.ScaledEndPoint;
                    LegalMoveDirections = GetLegalMoveDirections(ScaledJumpPoint, AttSectLtr);
                    MoveCursorToNewPoint(ScaledJumpPoint, MovementMode.Jump);
                    LoadLegacyJumpTable();
                    SetReadyButtonAppearance();
                    _isJumpMode = true;
                    SketchingState = SketchDrawingState.JumpPointSelected;
                    ScaledBeginPoint = ScaledJumpPoint;
                    DbMovementStartPoint = SMGlobal.ScaledPointToDbPoint((decimal)ScaledBeginPoint.X, (decimal)ScaledBeginPoint.Y, scale, origin);

                    DistText.Focus();
                }
            }
        }

        //    RESpJumpTable = ConstructRESpJumpTable();
        //    SectionLtrs = ConstructSectionLtrs();
        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_isJumpMode || SketchingState == SketchDrawingState.SectionAdded)
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

        //    REJumpTable = ConstructREJumpTable();
        private void LoadAttachmentPointsDataTable()
        {
            AttachmentPointsDataTable.Clear();
            List<SMLine> linesList = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where l.SectionLetter != "A" orderby l.SectionLetter, l.LineNumber select l).ToList();
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

        //    ConstructJumpTable();
        private void LoadLegacyJumpTable()
        {
            JumpTable = ConstructJumpTable();
            JumpTable.Clear();
            AddListItemsToJumpTableList(ScaledJumpPoint.X, ScaledJumpPoint.Y, LocalParcelCopy.Scale, LocalParcelCopy.AllSectionLines);
        }

        //    SectionTable = ConstructSectionTable();
        private void LoadSectionLinesGrid(string sectionLetter)
        {
            displayDataTable.Rows.Clear();
            if (LocalParcelCopy.SelectSectionByLetter(sectionLetter).Lines != null)
            {
                foreach (SMLine line in LocalParcelCopy.SelectSectionByLetter(sectionLetter).Lines)
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

        //    //savpic = new Dictionary<int, byte[]>();
        //    _StartX = new Dictionary<int, float>();
        //    _StartY = new Dictionary<int, float>();
        private void LoadStartPointsDataTable()
        {
            List<SMLine> startLines = (from l in SketchUpGlobals.ParcelWorkingCopy.AllSectionLines where l.LineNumber == 1 select l).OrderBy(s => s.SectionLetter).ToList();

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

        //    IsNewSketch = false;
        //    _hasNewSketch = hasNewSketch;
        //    IsNewSketch = hasNewSketch;
        //    _addSection = false;
        private void MeasureAngle()
        {
            throw new NotImplementedException();
        }

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

            ExpSketchPBox.Image = MainImage;

            //DMouseClick();
        }

        //    _currentSection = new SectionDataCollection(_fox, SketchUpGlobals.Record, SketchUpGlobals.Card);
        private void MoveCursorToNewPoint(PointF newPoint, MovementMode movementType)
        {
            Color penColor;
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int jumpXScaled = Convert.ToInt32(newPoint.X);
            int jumpYScaled = Convert.ToInt32(newPoint.Y);
            Cursor.Position = new Point(jumpXScaled, jumpYScaled);
            penColor = PenColorForDrawing(movementType);
            _isJumpMode = (SketchingState == SketchDrawingState.JumpPointSelected || SketchingState == SketchDrawingState.JumpMoveToBeginPoint);
            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(penColor, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 1, 1);
            g.Save();

            ExpSketchPBox.Image = MainImage;
            ExpSketchPBox.Refresh();
        }

        //    dbConn = _fox;
        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multipleSectionsAttachment = String.Empty;

            if (sectionLetterList.Count > 1)
            {
                MultiplePoints.Clear();

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
        private void NorthDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveNorth(NextStartX, NextStartY);
            DistText.Focus();
        }

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

        private void ParseAngleEntry(KeyEventArgs e, string textEntered)
        {
            string anglecalls = DistText.Text.Trim();

            int commaCnt = anglecalls.IndexOf(",");

            string D1 = anglecalls.Substring(0, commaCnt).Trim();

            string D2 = anglecalls.PadRight(25, ' ').Substring(commaCnt + 1, 10).Trim();

            AngD2 = Convert.ToDecimal(D1);

            AngD1 = Convert.ToDecimal(D2);

            AngleForm angleDialog = new AngleForm();
            angleDialog.ShowDialog();
            MoveDirections angleDirection = angleDialog.AngleDirection;
            if (_isKeyValid == false)
            {
                _isKeyValid = true;
            }
            switch (angleDirection)
            {
                case MoveDirections.NE:
                    MoveNorthEast(ScaledStartOfMovement.X, ScaledStartOfMovement.Y);
                    break;

                case MoveDirections.SE:
                    MoveSouthEast(ScaledStartOfMovement.X, ScaledStartOfMovement.Y);
                    break;

                case MoveDirections.SW:
                    MoveSouthWest(ScaledStartOfMovement.X, ScaledStartOfMovement.Y);
                    break;

                case MoveDirections.NW:
                    MoveNorthWest(ScaledStartOfMovement.X, ScaledStartOfMovement.Y);
                    break;

                case MoveDirections.None:

                default:
                    break;
            }
        }

        private void PromptToSaveOrDiscard()
        {
            string message = "Do you want to save changes?";
            DialogResult response = MessageBox.Show(message, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            switch (response)
            {
                case DialogResult.Cancel:
                case DialogResult.None:

                    // Do we need to do anything here?
                    break;

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

        private void RedrawSection()
        {
            NeedToRedraw = true;

            for (int i = 0; i < undoPoints.Rows.Count; i++)
            {
                float redist = 0;
                string undodirect = String.Empty;
                undodirect = undoPoints.Rows[i]["Direct"].ToString();

                float x1 = Convert.ToSingle(undoPoints.Rows[i]["X1pt"].ToString());

                float y1 = Convert.ToSingle(undoPoints.Rows[i]["Y1pt"].ToString());

                float x2 = Convert.ToSingle(undoPoints.Rows[i]["X2pt"].ToString());

                float y2 = Convert.ToSingle(undoPoints.Rows[i]["Y2pt"].ToString());

                if (undodirect == "N" || undodirect == "S" || undodirect == "E" || undodirect == "W")
                {
                    if (x1 == x2)
                    {
                        redist = Math.Abs((y1 - y2) / _currentScale);
                    }
                    if (y1 == y2)
                    {
                        redist = Math.Abs((x1 - x2) / _currentScale);
                    }
                }
                if (undodirect == "NE" || undodirect == "SE" || undodirect == "NW" || undodirect == "SW")
                {
                    float x1f = Math.Abs((x1 - x2) / _currentScale);
                    float y1f = Math.Abs((y1 - y2) / _currentScale);

                    distanceD = Convert.ToInt32(Math.Sqrt(x1f + y1f));

                    decimal distanceD1 = Math.Round(Convert.ToDecimal(Math.Sqrt(x1f + y1f)), 2);

                    distance = Convert.ToDecimal(distanceD1);

                    AngD1 = Convert.ToDecimal(x1f);
                    AngD2 = Convert.ToDecimal(y1f);
                }

                Graphics g = Graphics.FromImage(MainImage);
                g.Save();

                StartX = x1;
                StartY = y1;

                ExpSketchPBox.Image = MainImage;

                //click++;
                ////savpic.Add(click, imageToByteArray(_mainimage));

                if (undoPoints.Rows[i]["Direct"].ToString() == "E")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (redist * _currentScale)), StartY);

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    ////savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "N")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, StartX, (StartY - (redist * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "S")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, StartX, (StartY + (redist * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "W")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (redist * _currentScale)), StartY);

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "NW")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "NE")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY - (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "SW")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX - (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
                if (undoPoints.Rows[i]["Direct"].ToString() == "SE")
                {
                    SolidBrush brush = new SolidBrush(Color.Red);
                    Pen pen1x = new Pen(Color.Red, 2);
                    Pen pen1w = new Pen(Color.White, 2);
                    Font f = new Font("Segue UI", 8, FontStyle.Bold);

                    g.DrawLine(pen1x, StartX, StartY, (StartX + (Convert.ToInt16(AngD1) * _currentScale)), (StartY + (Convert.ToInt16(AngD2) * _currentScale)));

                    ExpSketchPBox.Image = MainImage;

                    //click++;
                    //savpic.Add(click, imageToByteArray(_mainimage));
                }
            }

            ExpSketchPBox.Image = MainImage;

            //click++;
            //savpic.Add(click, imageToByteArray(_mainimage));
        }

        private void RefreshParcelImage()
        {
            RenderCurrentSketch();
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
        private void ReOpenSec()
        {
            //int rowindex = 0;

            //DataSet rolines = null;

            //StringBuilder getLine = new StringBuilder();
            //getLine.Append("select jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle, ");
            //getLine.Append("jlpt1x,jlpt1y,jlpt2x,jlpt2Y,jlattach ");
            //getLine.Append(String.Format("from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}' ",
            //           SketchUpGlobals.LocalLib,
            //              SketchUpGlobals.LocalityPreFix,

            //            //SketchUpGlobals.FcLib,
            //            //SketchUpGlobals.FcLocalityPrefix,
            //            SketchUpGlobals.Record,
            //            SketchUpGlobals.Card,
            //            SketchUpGlobals.ReOpenSection));

            //rolines = dbConn.DBConnection.RunSelectStatement(getLine.ToString());

            //int maxsecline = rolines.Tables[0].Rows.Count;
            //if (rolines.Tables[0].Rows.Count > 0)
            //{
            //    REJumpTable.Clear();

            //    for (int i = 0; i < rolines.Tables[0].Rows.Count; i++)
            //    {
            //        decimal Distance = 0;

            //        DataRow row = REJumpTable.NewRow();
            //        row["Record"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlrecord"].ToString());
            //        row["Card"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jldwell"].ToString());
            //        row["Sect"] = rolines.Tables[0].Rows[i]["jlsect"].ToString().Trim();
            //        row["LineNo"] = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());
            //        row["Direct"] = rolines.Tables[0].Rows[i]["jldirect"].ToString().Trim();
            //        row["XLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlxlen"].ToString());
            //        row["YLen"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlylen"].ToString());
            //        row["Length"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jllinelen"].ToString());
            //        row["Angle"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlangle"].ToString());
            //        row["XPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1x"].ToString());
            //        row["YPt1"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt1y"].ToString());
            //        row["XPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
            //        row["YPt2"] = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2Y"].ToString());
            //        row["Attach"] = rolines.Tables[0].Rows[i]["jlattach"].ToString();

            //        decimal xpt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2x"].ToString());
            //        decimal ypt2 = Convert.ToDecimal(rolines.Tables[0].Rows[i]["jlpt2y"].ToString());

            //        float xPoint = (ScaleBaseX + (Convert.ToSingle(xpt2) * _currentScale));
            //        float yPoint = (ScaleBaseY + (Convert.ToSingle(ypt2) * _currentScale));

            //        rowindex = Convert.ToInt32(rolines.Tables[0].Rows[i]["jlline#"].ToString());

            //        _StartX.Add(rowindex, xPoint);

            //        _StartY.Add(rowindex, yPoint);

            //        REJumpTable.Rows.Add(row);
            //    }

            //    float _JumpXT = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale));

            //    float _JumpX = (ScaleBaseX + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["XPt2"].ToString()) * _currentScale)); //  change XPt1 to XPt2
            //    float _JumpY = (ScaleBaseY + (Convert.ToSingle(REJumpTable.Rows[rowindex - 1]["YPT2"].ToString()) * _currentScale));

            //    JumpX = _JumpX;
            //    JumpY = _JumpY;

            //    GetStartCorner();

            //}
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void ReorderParcelStructure()
        {
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void ReorderSectionsAfterChanges()
        {
            Garcnt = 0;
            GarSize = 0;
            carportCount = 0;
            CPSize = 0;
            SMParcel originalParcel = SketchUpGlobals.SMParcelFromData;
            SMParcelMast parcelMaster = SketchUpGlobals.ParcelWorkingCopy.ParcelMast;
            var sectionLetterList = (from s in parcelMast.Parcel.Sections select s.SectionLetter).Distinct().ToList();
            sectionLetterList.Sort();
            foreach (SMSection s in SketchUpGlobals.ParcelWorkingCopy.Sections)
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

            ReorderParcelStructure();
        }

        private string ReverseDirection(string direction)
        {
            string reverseDirection = direction;
            switch (direction)
            {
                case "E":
                    {
                        reverseDirection = "W";
                        break;
                    }
                case "NE":
                    {
                        reverseDirection = "NW";
                        break;
                    }
                case "SE":
                    {
                        reverseDirection = "SW";
                        break;
                    }
                case "W":
                    {
                        reverseDirection = "E";
                        break;
                    }
                case "NW":
                    {
                        reverseDirection = "NE";
                        break;
                    }
                case "SW":
                    {
                        reverseDirection = "SE";
                        break;
                    }

                case "S":
                    {
                        reverseDirection = "N";
                        break;
                    }
                case "N":
                    {
                        reverseDirection = "S";
                        break;
                    }

                default:
                    Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.\n{2} is not a valid direction value.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, direction));
                    break;
            }

            return reverseDirection;
        }

        private void RevertToPriorVersion()
        {
#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("{0}", ""));
            Console.WriteLine(string.Format("{0}", traceOut.ToString()));
            Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
#endif
            throw new NotImplementedException();
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
                FlipLeftRight();
            }
        }

        private void SaveCurrentParcelToDatabaseAndExit()
        {
            ReorderSectionsAfterChanges();
            MessageBox.Show(
                string.Format("Saving Version {0} with {1} Sections to Database.",
                SketchUpGlobals.ParcelWorkingCopy.SnapShotIndex,
                SketchUpGlobals.ParcelWorkingCopy.Sections.Count));
            throw new NotImplementedException();

            //this.Close();
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
        private void SaveSketchData()
        {
            //if (isInAddNewPointMode)
            //{
            //    if (isLastLine)
            //    {
            //        section.SectionLines.TrimExcess();
            //        int lastLine = section.SectionLines.Count;
            //        int lastRow = displayDataTable.Rows.Count - 1;

            //        var prevLine = section.SectionLines[lastLine];
            //        prevLine.YLength = Convert.ToDecimal(displayDataTable.Rows[lastRow]["North"].ToString());
            //        prevLine.XLength = Convert.ToDecimal(displayDataTable.Rows[lastRow]["East"].ToString());
            //        prevLine.Point1X = Convert.ToDecimal(unadj_pts[lastRow].X);
            //        prevLine.Point1Y = Convert.ToDecimal(unadj_pts[lastRow].Y);
            //        prevLine.Point2X = Convert.ToDecimal(unadj_pts[0].X);
            //        prevLine.Point2Y = Convert.ToDecimal(unadj_pts[0].Y);
            //        prevLine.Update();

            //        section.SectionLines[lastLine].IncrementLineNumber();

            //        var newLine = new BuildingLine();
            //        newLine.Record = section.Record;
            //        newLine.Card = section.Card;
            //        newLine.SectionLetter = section.SectionLetter;
            //        newLine.LineNumber = lastLine;
            //        newLine.Directional = displayDataTable.Rows[lastRow - 1]["Dir"].ToString();
            //        newLine.YLength = Convert.ToDecimal(displayDataTable.Rows[lastRow - 1]["North"].ToString());
            //        newLine.XLength = Convert.ToDecimal(displayDataTable.Rows[lastRow - 1]["East"].ToString());
            //        newLine.Point1X = Convert.ToDecimal(unadj_pts[lastRow - 1].X);
            //        newLine.Point1Y = Convert.ToDecimal(unadj_pts[lastRow - 1].Y);
            //        newLine.Point2X = Convert.ToDecimal(unadj_pts[lastRow].X);
            //        newLine.Point2Y = Convert.ToDecimal(unadj_pts[lastRow].Y);
            //        newLine.Insert();
            //    }
            //    else
            //    {
            //        var prevLine = section.SectionLines[NewPointIndex];
            //        prevLine.YLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex]["North"].ToString());
            //        prevLine.XLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex]["East"].ToString());
            //        prevLine.Point1X = Convert.ToDecimal(unadj_pts[NewPointIndex].X);
            //        prevLine.Point1Y = Convert.ToDecimal(unadj_pts[NewPointIndex].Y);
            //        prevLine.Point2X = Convert.ToDecimal(unadj_pts[NewPointIndex + 1].X);
            //        prevLine.Point2Y = Convert.ToDecimal(unadj_pts[NewPointIndex + 1].Y);
            //        prevLine.Update();

            //        section.IncrementAllLines(NewPointIndex);

            //        var newLine = new BuildingLine();
            //        newLine.Record = section.Record;
            //        newLine.Card = section.Card;
            //        newLine.SectionLetter = section.SectionLetter;
            //        newLine.LineNumber = NewPointIndex;
            //        newLine.Directional = displayDataTable.Rows[NewPointIndex - 1]["Dir"].ToString();
            //        newLine.YLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex - 1]["North"].ToString());
            //        newLine.XLength = Convert.ToDecimal(displayDataTable.Rows[NewPointIndex - 1]["East"].ToString());
            //        newLine.Point1X = Convert.ToDecimal(unadj_pts[NewPointIndex - 1].X);
            //        newLine.Point1Y = Convert.ToDecimal(unadj_pts[NewPointIndex - 1].Y);
            //        newLine.Point2X = Convert.ToDecimal(unadj_pts[NewPointIndex].X);
            //        newLine.Point2Y = Convert.ToDecimal(unadj_pts[NewPointIndex].Y);
            //        newLine.Insert();
            //    }

            //    SetAddNewPointButton(false);
            //}
            string message = string.Format("Need to implement {0}.{1}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name);

#if DEBUG
            MessageBox.Show(message);
#else
            Console.WriteLine(message);
            throw new NotImplementedException();
#endif
        }

        private void SetActiveButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.Cyan;
            BeginSectionBtn.Text = "Active";
            BeginSectionBtn.Enabled = false;
        }

        //    bool sketchHasLineData = lines.Tables[0].Rows.Count > 0;
        //    if (sketchHasLineData)
        //    {
        //        SecItemCnt = CountSections();
        private void SetAddNewPointButton(bool enabled)
        {
            isInAddNewPointMode = enabled;
        }

        private void SetDirectionOfKeyEntered(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                case Keys.E:
                case Keys.R:
                    DirectionOfMovement = MoveDirections.E;

                    break;

                case Keys.Left:
                case Keys.L:
                case Keys.W:
                    DirectionOfMovement = MoveDirections.W;

                    break;

                case Keys.Up:
                case Keys.N:
                case Keys.U:
                    DirectionOfMovement = MoveDirections.N;

                    break;

                case Keys.Down:
                case Keys.D:
                case Keys.S:
                    DirectionOfMovement = MoveDirections.S;

                    break;

                default:
                    DirectionOfMovement = MoveDirections.None;

                    break;
            }
        }

        private void SetMainStatusText(string statusText)
        {
            sketchStatusMain.Text = statusText;
        }

        private void SetReadyButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.PaleTurquoise;
            BeginSectionBtn.Text = "Begin";
            BeginSectionBtn.Enabled = ScaledJumpPoint != null;
        }

        private void ShowMessageBox(string s)
        {
            MessageBox.Show(s);
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
                LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;
                SketchUpGlobals.HasSketch = (LocalParcelCopy != null && LocalParcelCopy.AllSectionLines.Count > 0);
                IsNewSketch = !SketchUpGlobals.HasSketch;

                //HACK - Easier to repeat than track down the usages at this juncture
                SketchUpGlobals.HasNewSketch = IsNewSketch;
                if (SketchUpGlobals.HasSketch == true)
                {
                    SMSketcher sketcher = new SMSketcher(LocalParcelCopy, ExpSketchPBox);
                    sketcher.RenderSketch();

                    MainImage = sketcher.SketchImage;
                    _currentScale = (float)LocalParcelCopy.Scale;

                    //MainImage = currentParcel.GetSketchImage(ExpSketchPBox.Width, ExpSketchPBox.Height, 1000, 572, 400, out _scale);
                    //_currentScale = _scale;
                }
                else
                {
                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                }
                ScaleBaseX = ExpSketchPBox.Width / (float)LocalParcelCopy.SketchXSize;

                // ScaleBaseX = BuildingSketcher.basePtX;
                //  ScaleBaseY = BuildingSketcher.basePtY;
                ScaleBaseY = ExpSketchPBox.Height / (float)LocalParcelCopy.SketchYSize;

                if (MainImage == null)
                {
                    MainImage = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                    _vacantParcelSketch = true;
                    IsNewSketch = true;
                }
                if (_vacantParcelSketch == true)
                {
                    Graphics g = Graphics.FromImage(MainImage);
                    g.Clear(Color.White);
                    _currentScale = Convert.ToSingle(7.2);
                }

                ExpSketchPBox.Image = MainImage;
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

        //                ExpSketchPBox.Image = MainImage;
        //            }
        //            catch (Exception ex)
        //            {
        //                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
        //                Console.WriteLine(errMessage);
        //                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
        //#if DEBUG
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

                ExpSketchPBox.Image = MainImage;

                //click++;
                //savpic.Add(click, imageToByteArray(_mainimage));
            }
        }

        private void TotalGaragesAndCarports(SMSection s)
        {
            if (SketchUpCamraSupport.GarageTypes.Contains(s.SectionType))
            {
                Garcnt++;

                GarSize += s.SqFt;
            }
            if (SketchUpCamraSupport.CarPortTypes.Contains(s.SectionType))
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
        //                ScaleBaseX = ExpSketchPBox.Width / (float)LocalParcelCopy.SketchXSize;
        //               // ScaleBaseX = BuildingSketcher.basePtX;
        //              //  ScaleBaseY = BuildingSketcher.basePtY;
        //                 ScaleBaseY = ExpSketchPBox.Height / (float)LocalParcelCopy.SketchYSize;
        private void tsbExitSketch_Click(object sender, EventArgs e)
        {
            PromptToSaveOrDiscard();
        }

        //                //HACK - Easier to repeat than track down the usages at this juncture
        //                SketchUpGlobals.HasNewSketch = IsNewSketch;
        //                if (SketchUpGlobals.HasSketch == true)
        //                {
        //                    SMSketcher sketcher = new SMSketcher(LocalParcelCopy,ExpSketchPBox);
        //                    sketcher.RenderSketch();
        //                    LocalParcelCopy.SetScaleAndOriginForParcel(ExpSketchPBox);
        //                    MainImage = sketcher.SketchImage;
        //                    _currentScale = (float)LocalParcelCopy.Scale;
        private void UnDoBtn_Click(object sender, EventArgs e)
        {
            RevertToPriorVersion();
        }

        //                InitializeDisplayDataGrid();
        //                LocalParcelCopy = SketchUpGlobals.ParcelWorkingCopy;
        //                SketchUpGlobals.HasSketch = (LocalParcelCopy != null && LocalParcelCopy.AllSectionLines.Count > 0);
        //                IsNewSketch = !SketchUpGlobals.HasSketch;
        private void UndoLine()
        {
            SMParcel parcel = SketchUpGlobals.ParcelWorkingCopy;
            string workingSectionLetter = NextSectLtr;
            SMSection workingSection = (from s in parcel.Sections where s.SectionLetter == workingSectionLetter select s).FirstOrDefault();
            int lastLineNumber = (from l in workingSection.Lines select l.LineNumber).Max();
            parcel.Sections.Remove(parcel.Sections.Where(l => l.SectionLetter == workingSectionLetter).FirstOrDefault());
            workingSection.Lines.Remove(workingSection.Lines.Where(n => n.LineNumber == lastLineNumber).FirstOrDefault());
            parcel.Sections.Add(workingSection);
            parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(parcel);
            RenderCurrentSketch();
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

        //    fox.DBConnection.ExecuteNonSelectStatement(fixLine.ToString());
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

        private void WestDirBtn_Click(object sender, EventArgs e)
        {
            _isKeyValid = true;
            MoveWest(NextStartX, NextStartY);
            DistText.Focus();
        }

        #endregion "Private methods"

        private const int sketchBoxPaddingTotal = 20;
    }
}