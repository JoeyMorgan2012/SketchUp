using System;
using System.Drawing;
using System.Text;

namespace SketchUp
{
    public class SMLine
    {
        #region Class Properties

        public string AttachedSection
        {
            get
            {
                return attachedSection ?? string.Empty;
            }

            set
            {
                attachedSection = value ?? string.Empty;
            }
        }

        public string Direction
        {
            get
            {
                return direction.ToUpper();
            }

            set
            {
                direction = value.ToUpper();
            }
        }

        public int Dwelling
        {
            get
            {
                return dwelling;
            }

            set
            {
                dwelling = value;
            }
        }

        public PointF EndPoint
        {
            get
            {
                endPoint = new PointF((float)EndX, (float)EndY);
                return endPoint;
            }

            set
            {
                endPoint = value;
            }
        }

        public decimal EndX
        {
            get
            {
                return endX;
            }

            set
            {
                endX = value;
            }
        }

        public decimal EndY
        {
            get
            {
                return endY;
            }

            set
            {
                endY = value;
            }
        }

        public decimal LineAngle
        {
            get
            {
                return lineAngle;
            }

            set
            {
                lineAngle = value;
            }
        }

        public decimal LineLength
        {
            get
            {
                lineLength = (decimal)SMGlobal.LineLength(new PointF((float)StartX, (float)StartY), new PointF((float)EndX, (float)EndY));
                return lineLength;
            }

            set
            {
                lineLength = value;
            }
        }

        public int LineNumber
        {
            get
            {
                return lineNumber;
            }

            set
            {
                lineNumber = value;
            }
        }

        public int Record
        {
            get
            {
                return record;
            }

            set
            {
                record = value;
            }
        }

        public string SectionLetter
        {
            get
            {
                return sectionLetter;
            }

            set
            {
                sectionLetter = value;
            }
        }

        public PointF StartPoint
        {
            get
            {
                //float drawingStartX = (float)(StartX + ParentParcel.OffsetX);
                //float drawingStartY = (float)(StartY + ParentParcel.OffsetY);
                startPoint = new PointF((float)StartX, (float)StartY);
                return startPoint;
            }

            set
            {
                startPoint = value;
            }
        }

        public decimal StartX
        {
            get
            {
                return startX;
            }

            set
            {
                startX = value;
            }
        }

        public decimal StartY
        {
            get
            {
                return startY;
            }

            set
            {
                startY = value;
            }
        }

        public decimal XLength
        {
            get
            {
                xLength = (EndX - StartX);
                return xLength;
            }

            set
            {
                xLength = value;
            }
        }

        public decimal YLength
        {
            get
            {
                yLength = (EndY - StartY);
                return yLength;
            }

            set
            {
                yLength = value;
            }
        }

        #endregion Class Properties

        #region Constructors

        public SMLine()
        {
        }

        public SMLine(int recordNumber, int dwellingNumber, string letter)
        {
            record = recordNumber;
            dwelling = dwellingNumber;
            sectionLetter = letter;
        }

        public SMLine(SMSection section)
        {
            record = section.Record;
            dwelling = section.Dwelling;
            sectionLetter = section.SectionLetter;
        }

        #endregion Constructors

        #region Fields

        private string attachedSection;
        private PointF comparisonPoint;
        private string direction;
        private int dwelling;
        private PointF endPoint;
        private decimal endPointDistanceFromComparisonPoint;
        private decimal endX;
        private decimal endY;
        private decimal lineAngle;
        private string lineLabel;
        private decimal lineLength;
        private int lineNumber;
        private PointF midPoint;
        private decimal minX;
        private decimal minY;
        private int record;
        private PointF scaledEndPoint;
        private PointF scaledStartPoint;
        private string sectionLetter;
        private PointF startPoint;
        private decimal startPointDistanceFromComparisonPoint;
        private decimal startX;
        private decimal startY;
        private decimal xLength;
        private decimal yLength;

        #endregion Fields

        #region Virtual/navigation properties

        public virtual SMParcel ParentParcel
        {
            get; set;
        }

        public virtual SMSection ParentSection
        {
            get; set;
        }

        #endregion Virtual/navigation properties

        #region private methods

        private string DirectionArrow()
        {
            string directionArrow = string.Empty;
            switch (direction)
            {
                case "N":
                    directionArrow = SMGlobal.NorthArrow;
                    break;

                case "E":
                    directionArrow = SMGlobal.EastArrow;
                    break;

                case "W":
                    directionArrow = SMGlobal.WestArrow;
                    break;

                case "S":
                    directionArrow = SMGlobal.SouthArrow;
                    break;

                case "NE":
                    directionArrow = SMGlobal.NorthEastArrow;
                    break;

                case "SE":
                    directionArrow = SMGlobal.SouthEastArrow;
                    break;

                case "NW":
                    directionArrow = SMGlobal.NorthWestArrow;
                    break;

                case "SW":
                    directionArrow = SMGlobal.SouthWestArrow;
                    break;

                default:
                    break;
            }

            return directionArrow;
        }

        private string FormattedLineLabel()
        {
            string labelText = string.Empty;
            string arrow = DirectionArrow();
            switch (Direction)
            {
                case "E": //Left to right, label goes below line
                case "S": //top to bottom, label goes to the right

                    labelText = string.Format("{0}-{1} {2:F2}' {3}", SectionLetter, LineNumber, LineLength, arrow);

                    //labelText = string.Format("{0} {1}-{2}\n{3},{4} to {5},{6}", arrow, SectionLetter, LineNumber, ScaledStartPoint.X, ScaledStartPoint.Y, ScaledEndPoint.X, ScaledEndPoint.Y);
                    break;

                case "W": //right to left, label goes above line
                case "N": //Bottom to top, label goes to the left

                    labelText = string.Format(" {3} {0}-{1} {2:F2}'", SectionLetter, LineNumber, LineLength, arrow);

                    //labelText = string.Format(" {1}-{2}\n{3},{4} to {5},{6} {0}", arrow, SectionLetter, LineNumber, ScaledStartPoint.X, ScaledStartPoint.Y, ScaledEndPoint.X, ScaledEndPoint.Y);
                    break;

                default:
                    break;
            }

            return labelText;
        }

        private PointF MidPointOfLine(PointF sketchOriginPoint)
        {
            float xMidPoint = ScaledStartPoint.X + ((ScaledEndPoint.X - ScaledStartPoint.X) / 2);
            float yMidPoint = ScaledStartPoint.Y + ((ScaledEndPoint.Y - ScaledStartPoint.Y) / 2);
            midPoint = new PointF(xMidPoint, yMidPoint);
            return midPoint;
        }

        #endregion private methods

        #region public methods

        public PointF LineLabelPlacementPoint(PointF sketchOriginPoint)
        {
            float lineHalfWidth = Math.Abs((ScaledEndPoint.X - ScaledStartPoint.X) / 2);
            float lineHalfHeight = Math.Abs((ScaledEndPoint.Y - ScaledStartPoint.Y) / 2);
            float labelStartX = ScaledStartPoint.X;
            float labelStartY = ScaledStartPoint.Y;
            float labelLength = LineLabel.Length;
            PointF lineMidPoint = MidPointOfLine(sketchOriginPoint);
            switch (Direction)
            {
                case "E": //Left to right, label goes below line
                    labelStartX = ScaledStartPoint.X;
                    labelStartY = lineMidPoint.Y + 10;

                    break;

                case "W": //right to left, label goes above line
                    labelStartX = (ScaledEndPoint.X) - labelLength - ((float)(XLength * ParentParcel.Scale) / 2);
                    labelStartY = lineMidPoint.Y - 20;
                    break;

                case "N": //Bottom to top, label goes to the left
                    labelStartX = lineMidPoint.X + labelLength - 20;
                    labelStartY = lineMidPoint.Y;
                    break;

                case "S": //top to bottom, label goes to the right
                    labelStartX = lineMidPoint.X - labelLength + 20;
                    labelStartY = lineMidPoint.Y;

                    //labelStartX = lineMidPoint.X + sketchOriginPoint.X - labelLength + 20;
                    //labelStartY = lineMidPoint.Y + sketchOriginPoint.Y;
                    break;

                default:
                    break;
            }
            PointF labelStartPoint = new PointF(labelStartX, labelStartY);
            return labelStartPoint;
        }

        public PointF ComparisonPoint
        {
            get
            {
                return comparisonPoint;
            }

            set
            {
                comparisonPoint = value;
            }
        }

        public decimal EndPointDistanceFromComparisonPoint
        {
            get
            {
                endPointDistanceFromComparisonPoint = (decimal)SMGlobal.LineLength(ComparisonPoint, ScaledEndPoint);
                return endPointDistanceFromComparisonPoint;
            }

            set
            {
                endPointDistanceFromComparisonPoint = value;
            }
        }

        public string LineLabel
        {
            get
            {
                lineLabel = FormattedLineLabel();
                return lineLabel;
            }

            set
            {
                lineLabel = value;
            }
        }

        public decimal MinX
        {
            get
            {
                minX = SMGlobal.SmallerDecimal(StartX, EndX);
                return minX;
            }

            set
            {
                minX = value;
            }
        }

        public decimal MinY
        {
            get
            {
                minY = SMGlobal.SmallerDecimal(StartY, EndY);
                return minY;
            }

            set
            {
                minY = value;
            }
        }

        public PointF ScaledEndPoint
        {
            get
            {
                scaledEndPoint = ComputeScaledEndPoint(EndPoint, ParentParcel.Scale);
                return scaledEndPoint;
            }

            set
            {
                scaledEndPoint = value;
            }
        }

        private PointF ComputeScaledEndPoint(PointF endPoint, decimal scale)
        {
            decimal width = XLength * scale;
            decimal height = YLength * scale;

            endPoint = new PointF(scaledStartPoint.X + (float)+width, scaledStartPoint.Y + (float)height);

            return endPoint;
        }

        public PointF ScaledStartPoint
        {
            get
            {
                return scaledStartPoint;
            }

            set
            {
                scaledStartPoint = value;
            }
        }

        public decimal StartPointDistanceFromComparisonPoint
        {
            get
            {
                startPointDistanceFromComparisonPoint = (decimal)SMGlobal.LineLength(ComparisonPoint, ScaledStartPoint);

                return startPointDistanceFromComparisonPoint;
            }

            set
            {
                startPointDistanceFromComparisonPoint = value;
            }
        }

        #endregion public methods
    }
}