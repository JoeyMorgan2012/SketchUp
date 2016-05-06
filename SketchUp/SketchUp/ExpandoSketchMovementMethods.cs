using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class ExpandoSketch : Form
    {
        #region New or modified movementmethods

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
            PointF labelPoint=startPoint;
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
            if (labelPoint!=startPoint)//TODO: Complete
                //Ignore if I haven't gotten to the placement yet
            {
               
                g.DrawLine(pen, startPoint, endPoint);
                g.DrawString(distance.ToString(), font, brush, labelPoint);
            }
        }

        private void EraseSectionFromDrawing(SMSection workingSection)
        {
            //TODO: Draw everything with a white pen to earase the section and redraw it.
        }

        private List<string> GetLegalMoveDirections(PointF scaledJumpPoint, string attachSectionLetter)
        {
            List<SMLine> linesWithJumpPoint = (from l in LocalParcelCopy.AllSectionLines where l.SectionLetter == attachSectionLetter && (l.ScaledStartPoint == scaledJumpPoint || l.ScaledEndPoint == scaledJumpPoint) select l).ToList();
            List<string> legalDirections = new List<string>();
            legalDirections.AddRange((from l in LocalParcelCopy.AllSectionLines where l.ScaledStartPoint == scaledJumpPoint && l.SectionLetter == attachSectionLetter select l.Direction).ToList());

            legalDirections.AddRange((from l in linesWithJumpPoint select l.Direction).Distinct().ToList());
            LegalMoveDirections = legalDirections;
            return legalDirections;
        }

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
                            endX = ScaledStartOfMovement.X+(float)scaledDistance;
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

        private void HandleNonArrowKeys(KeyEventArgs e)
        {
            bool notNumPad = (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9);
            if (notNumPad)
            {
                #region Not Numberpad

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

                #endregion Not Numberpad
            }
        }

        private void JumptoCorner()
        {
            // float CurrentScale = _currentScale;
            //int crrec = _currentParcel.Record;
            //int crcard = _currentParcel.Card;
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
                    string message = string.Format("No lines contain an available connection point from point {0},{1}", mouseLocation.X, mouseLocation.Y);

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
                    }
                    else
                    {
                        AttSectLtr = SecLetters[0];
                        AttachmentSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == AttSectLtr select s).FirstOrDefault();
                        JumpPointLine = connectionLines[0];
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

        #endregion New or modified movementmethods

        //public void AddEastLineToSection(PointF startPoint, decimal distance)
        //{
        //    SMSection workingSection = (from s in LocalParcelCopy.Sections where s.SectionLetter == s.ParentParcel.LastSectionLetter select s).FirstOrDefault();
        //    if (workingSection!=null)
        //    {
        //        decimal scale = LocalParcelCopy.Scale;
        //        float endPointX = startPoint.X + (float)(distance * LocalParcelCopy.Scale);
        //        float endPointY = startPoint.Y;

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

        //    }
        //}

        #region Original Movement Methods modified

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

        #endregion Original Movement Methods modified
    }
}