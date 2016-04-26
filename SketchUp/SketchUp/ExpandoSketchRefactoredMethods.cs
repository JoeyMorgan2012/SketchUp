using System;
using System.Collections.Generic;
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
    public partial class ExpandoSketch : Form
    {
        #region Form Events and Menu

        #region May not be needed

        private void AutoClose()
        {
            PromptToSaveOrDiscard();

            //Cursor = Cursors.WaitCursor;

            //savcnt = new List<int>();
            //savpic = curpic;

            //_isClosed = true;

            //string stx = _nextSectLtr;

            //click = curclick;

            //float tx1 = NextStartX;

            //float ty1 = NextStartY;

            ExpSketchPBox.Image = MainImage;

            ////click++;
            ////savpic.Add(click, imageToByteArray(_mainimage));

            //foreach (KeyValuePair<int, byte[]> pair in savpic)
            //{
            //    savcnt.Add(pair.Key);
            //}

            //finalClick = click;

            //_isclosing = true;

            //_addSection = false;

            //computeArea();

            //AddSectionSQL(finalDirect, finalDistanceF);

            //string finalDesc = String.Format("{0}, {1} sf",
            //    FieldText.Text.Trim(),
            //    _nextSectArea.ToString("N1"));

            //FieldText.Text = finalDesc.Trim();

            //ExpSketchPBox.Image = _mainimage;

            //sortSection();

            //setAttPnts();

            //Cursor = Cursors.Default;

            //this.Close();
        }

        #endregion May not be needed

        #region Sketching Methods

        private void SetScaledStartPoints()
        {
            try
            {
                if (SketchUpGlobals.ParcelWorkingCopy != null && SketchUpGlobals.ParcelWorkingCopy.Sections != null)
                {
                    decimal sketchScale = SketchUpGlobals.ParcelWorkingCopy.Scale;
                    foreach (SMSection s in SketchUpGlobals.ParcelWorkingCopy.Sections)
                    {
                        foreach (SMLine line in s.Lines)
                        {
                            var lineStartX = (float)((line.StartX * sketchScale) + (decimal)SketchOrigin.X);
                            var lineStartY = (float)((line.StartY * sketchScale) + (decimal)SketchOrigin.Y);
                            line.ScaledStartPoint = new PointF(lineStartX, lineStartY);
                        }
                    }
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

        private void SetSectionCenterPoints()
        {
            try
            {
                List<PointF> sectionPoints = new List<PointF>();
                foreach (SMSection section in SketchUpGlobals.ParcelWorkingCopy.Sections)
                {
                    sectionPoints = new List<PointF>();
                    foreach (SMLine line in section.Lines)
                    {
                        sectionPoints.Add(line.ScaledStartPoint);
                        sectionPoints.Add(line.ScaledEndPoint);
                    }
                    PolygonF sectionBounds = new PolygonF(sectionPoints.ToArray<PointF>());
                    section.ScaledSectionCenter = PointF.Add(sectionBounds.CenterPointOfBounds, new SizeF(0, -12));
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

        private void SetSketchOrigin()
        {
            //Using the scale and the offsets, determine the point to be considered as "0,0" for the sketch;
            try
            {
                var sketchAreaWidth = ExpSketchPBox.Width - 20;
                var sketchAreaHeight = ExpSketchPBox.Height - 20;

                PointF pictureBoxCorner = ExpSketchPBox.Location;
                var extraWidth = (ExpSketchPBox.Width - 20) - (SketchUpGlobals.ParcelWorkingCopy.Scale * SketchUpGlobals.ParcelWorkingCopy.SketchXSize);
                var extraHeight = (ExpSketchPBox.Height - 20) - (SketchUpGlobals.ParcelWorkingCopy.Scale * SketchUpGlobals.ParcelWorkingCopy.SketchYSize);
                var paddingX = (extraWidth / 2) + 10;
                var paddingY = (extraHeight / 2) + 10;
                var xLocation = (SketchUpGlobals.ParcelWorkingCopy.OffsetX * SketchUpGlobals.ParcelWorkingCopy.Scale) + paddingX;
                var yLocation = (SketchUpGlobals.ParcelWorkingCopy.OffsetY * SketchUpGlobals.ParcelWorkingCopy.Scale) + paddingY;

                SketchOrigin = new PointF((float)xLocation, (float)yLocation);
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

        private void SetSketchScale()
        {
            try
            {
                //Determine the size of the sketch drawing area, which is the picture box less 10 px on a side, so height-20 and width-20. Padding is 10.
                int boxHeight = ExpSketchPBox.Height - 20;
                int boxWidth = ExpSketchPBox.Width - 20;
                //decimal xScale = Math.Floor(boxWidth / SketchUpGlobals.ParcelWorkingCopy.SketchXSize);
                //decimal yScale = Math.Floor(boxHeight / SketchUpGlobals.ParcelWorkingCopy.SketchYSize);
                //SketchUpGlobals.ParcelWorkingCopy.Scale = (decimal)SMGlobal.SmallerDouble(xScale, yScale);

                decimal xScale = (decimal)DrawingScale;
                decimal yScale = Math.Floor(boxHeight / SketchUpGlobals.ParcelWorkingCopy.SketchYSize);
                SketchUpGlobals.ParcelWorkingCopy.Scale = (decimal)SMGlobal.SmallerDouble(xScale, yScale);

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

        #endregion Sketching Methods

        [CodeRefactoringState(ExtractedFrom = "JumpToCorner", ExtractedMethod = true, ChangeDescription = "Adds X-Line to SMLines Collection, not database.", IsToDo = false)]
        private void AddXLine(string sectionLetter)
        {
            SMSection thisSection = (from s in SketchUpGlobals.ParcelWorkingCopy.Sections where s.SectionLetter == sectionLetter select s).FirstOrDefault<SMSection>();
            SMLine xLine = new SMLine { Record = thisSection.Record, Dwelling = thisSection.Dwelling, SectionLetter = thisSection.SectionLetter, LineNumber = thisSection.Lines.Count + 1, StartX = 0, StartY = 0, EndX = 0, EndY = 0, ParentParcel = thisSection.ParentParcel, Direction = "X" };
            SketchUpGlobals.ParcelWorkingCopy.Sections.Where(s => s.SectionLetter == sectionLetter).FirstOrDefault<SMSection>().Lines.Add(xLine);
        }

        //ToDo: Begin here to hook in parcel updates
        private void DoneDrawingBtn_Click(object sender, EventArgs e)
        {
            ReorderParcelStructure();
            RefreshParcelImage();
            SetActiveButtonAppearance();
        }

        private void endSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private string MultiPointsAvailable(List<string> sectionLetterList)
        {
            string multisectatch = String.Empty;

            if (sectionLetterList.Count > 1)
            {
                MulPts.Clear();

                MultiSectionSelection attsecltr = new MultiSectionSelection(sectionLetterList);
                attsecltr.ShowDialog(this);

                multisectatch = MultiSectionSelection.adjsec;

                MulPts = MultiSectionSelection.mulattpts;

                _hasMultiSection = true;
            }

            return multisectatch;
        }

        private void RefreshParcelImage()
        {
            RenderCurrentSketch();
        }

        private void RenderCurrentSketch()
        {
            throw new NotImplementedException();
        }

        private void ReorderParcelStructure()
        {
            throw new NotImplementedException();
        }

        private void RevertToPriorVersion()
        {
            throw new NotImplementedException();
        }

        private void SetActiveButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.Cyan;
            BeginSectionBtn.Text = "Active";
        }

        private void SetReadyButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.PaleTurquoise;
            BeginSectionBtn.Text = "Begin";
        }

        private void tsbExitSketch_Click(object sender, EventArgs e)
        {
            PromptToSaveOrDiscard();
        }

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

        #endregion Form Events and Menu

        #region Misc. Refactored Methods

        private void MoveCursor(PointF jumpPointScaled)
        {
            Color penColor;
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Convert.ToInt32(JumpX) - 50, Convert.ToInt32(JumpY) - 50);

            penColor = (_undoMode || draw) ? Color.Red : Color.Black;
            int jumpXScaled = Convert.ToInt32(jumpPointScaled.X);
            int jumpYScaled = Convert.ToInt32(jumpPointScaled.Y);
            Graphics g = Graphics.FromImage(MainImage);
            Pen pen1 = new Pen(Color.Red, 4);
            g.DrawRectangle(pen1, jumpXScaled, jumpYScaled, 1, 1);
            g.Save();

            ExpSketchPBox.Image = MainImage;

            DMouseClick();
        }

        #endregion Misc. Refactored Methods

        #region refactored SQL inserts and updates

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
                              SketchUpGlobals.LocalityPreFix,
                               _currentParcel.Record,
                               _currentParcel.Card,
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
                if (lineCnt > 20)
                {
                    MessageBox.Show("Section Lines Exceeded", "Critical Line Count", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }

                //MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2695", _currentParcel.Record, _currentParcel.Card));

                decimal t1 = rndTst1;
                decimal t2 = rndTst2;

                decimal tX1 = rndPt2X;
                decimal tY1 = rndPt2Y;

                if (lineCnt <= 20)
                {
                    StringBuilder addSect = new StringBuilder();
                    addSect.Append(String.Format("insert into {0}.{1}line (jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen, jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
                          SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix));
                    addSect.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                        _currentParcel.Record, //0
                        _currentParcel.Card, // 1
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
                    Trace.WriteLine(string.Format("{0}", traceOut.ToString()));
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
                                Trace.WriteLine(errMessage);
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
                          SketchUpGlobals.LocalityPreFix,

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix,
                                _currentParcel.Record,
                                _currentParcel.Card,
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

                if (lineCnt > 20)
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

                if (lineCnt <= 20)
                {
                    //MessageBox.Show(String.Format("Insert into Line Record - {0}, Card - {1} at 2416", _currentParcel.Record, _currentParcel.Card));

                    StringBuilder addSectAng = new StringBuilder();
                    addSectAng.Append(String.Format("insert into {0}.{1}line ( jlrecord,jldwell,jlsect,jlline#,jldirect,jlxlen,jlylen,jllinelen,jlangle,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach) ",
                                 SketchUpGlobals.LocalLib,
                          SketchUpGlobals.LocalityPreFix

                                //SketchUpGlobals.FcLib,
                                //SketchUpGlobals.FcLocalityPrefix
                                ));
                    addSectAng.Append(String.Format(" values ( {0},{1},'{2}',{3},'{4}',{5},{6},{7},{8},{9},{10},{11},{12},'{13}' ) ",
                        _currentParcel.Record,
                        _currentParcel.Card,
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

        #endregion refactored SQL inserts and updates

        #region Save or Discard Changes Refactored

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

        private void SaveCurrentParcelToDatabaseAndExit()
        {
            Reorder();
            MessageBox.Show(
                string.Format("Saving Version {0} with {1} Sections to Database.",
                SketchUpGlobals.ParcelWorkingCopy.SnapShotIndex,
                SketchUpGlobals.ParcelWorkingCopy.Sections.Count));
            this.Close();
        }

        #endregion Save or Discard Changes Refactored

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
            //PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            //PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);
            PointF drawLineStart = new PointF(line.ScaledStartPoint.X, line.ScaledStartPoint.Y);
            PointF drawLineEnd = new PointF(line.ScaledEndPoint.X, line.ScaledEndPoint.Y);
            g.DrawLine(BluePen, drawLineStart, drawLineEnd);
            DrawLabel(line);
        }

        private void DrawLine(SMLine line, bool omitLabel = true)
        {
            //PointF drawLineStart = new PointF(line.ScaledStartPoint.X + SketchOrigin.X, line.ScaledStartPoint.Y + SketchOrigin.Y);
            //PointF drawLineEnd = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y + SketchOrigin.Y);
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

        private void DrawSections(string sectionLetter)
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

        private Bitmap RenderSketch()
        {
            try
            {
                Bitmap sketcher = new Bitmap(ExpSketchPBox.Width, ExpSketchPBox.Height);
                SetSketchScale();
                SetSketchOrigin();
                SetScaledStartPoints();
                SetSectionCenterPoints();
                sketcher=ShowSketchFromBitMap();
                return sketcher;
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
								Trace.WriteLine(errMessage);
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