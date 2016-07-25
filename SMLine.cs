using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SketchUp
{
    public class SMLine
    {
        #region "Constructor"

        public SMLine()
        {
        }

        public SMLine(SMSection section)
        {
            Record = section.Record;
            Card = section.Card;
            SectionLetter = section.SectionLetter;
            ParentSection = section;
            ParentParcel = section.ParentParcel;
        }

        #endregion "Constructor"

        #region "Public Methods"

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

                    //labelStartX = ScaledStartPoint.X-(labelLength*2);
                    labelStartX = lineMidPoint.X - ((float)ParentParcel.Scale * (labelLength / 2));
                    labelStartY = lineMidPoint.Y + 10;

                    break;

                case "W": //right to left, label goes above line

                    //labelStartX = (ScaledEndPoint.X) + ((float)(XLength * ParentParcel.Scale) / 2);
                    labelStartX = lineMidPoint.X - ((float)ParentParcel.Scale * (labelLength / 2));
                    labelStartY = lineMidPoint.Y - 20;
                    break;

                case "N": //Bottom to top, label goes to the left
                    labelStartX = lineMidPoint.X - (((float)ParentParcel.Scale * (labelLength)));
                    labelStartY = lineMidPoint.Y - 10;
                    break;

                case "S": //top to bottom, label goes to the right
                    labelStartX = lineMidPoint.X + 10;
                    labelStartY = lineMidPoint.Y - 10;

                    //labelStartX = lineMidPoint.X + sketchOriginPoint.X - labelLength + 20;
                    //labelStartY = lineMidPoint.Y + sketchOriginPoint.Y;
                    break;

                default:
                    break;
            }
            var labelStartPoint = new PointF(labelStartX, labelStartY);
            return labelStartPoint;
        }

        #endregion "Public Methods"

        #region "Private methods"

        private PointF ComputeScaledEndPoint(PointF endPoint, double scale)
        {
            double width = XLength * scale;
            double height = YLength * scale;

            endPoint = new PointF(scaledStartPoint.X + (float)+width, scaledStartPoint.Y + (float)height);

            return endPoint;
        }

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

                    labelText = string.Format("{0:N2}'{1}", LineLength, arrow);

                    //labelText = string.Format("{0} {1}-{2}\n{3},{4} to {5},{6}", arrow, SectionLetter, LineNumber, ScaledStartPoint.X, ScaledStartPoint.Y, ScaledEndPoint.X, ScaledEndPoint.Y);
                    break;

                case "W": //right to left, label goes above line
                case "N": //Bottom to top, label goes to the left

                    labelText = string.Format("{0}{1:N2}'", arrow, LineLength);

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

        #endregion "Private methods"

        #region "Properties"

        public string AttachedSection
        {
            get { return attachedSection ?? string.Empty; }

            set { attachedSection = value; }
        }

        public int Card { get; set; }

        public PointF ComparisonPoint
        {
            get {
                if (comparisonPoint == null)
                {
                    comparisonPoint = ParentParcel.SketchOrigin;
                }
                return comparisonPoint;
            }

            set { comparisonPoint = value; }
        }

        public string Direction
        {
            get { return direction.ToUpper(); }

            set { direction = value.ToUpper(); }
        }

        public PointF EndPoint
        {
            get {
                endPoint = new PointF((float)EndX, (float)EndY);
                return endPoint;
            }

            set { endPoint = value; }
        }

        public double EndPointJumpPointDist
        {
            get {
                endPointJumpPointDist = SMGlobal.LineLength(ComparisonPoint, ScaledEndPoint);
                return endPointJumpPointDist;
            }

            set { endPointJumpPointDist = value; }
        }

        public double EndX { get; set; }

        public double EndY
        {
            get { return endY; }

            set { endY = value; }
        }

        public double LineAngle { get; set; }

        public string LineLabel
        {
            get {
                lineLabel = FormattedLineLabel();
                return lineLabel;
            }

            set { lineLabel = value; }
        }

        public double LineLength
        {
            get {
                lineLength = SMGlobal.LineLength(new PointF((float)StartX, (float)StartY), new PointF((float)EndX, (float)EndY));
                return lineLength;
            }

            set { lineLength = value; }
        }

        public int LineNumber { get; set; }

        public double MinX
        {
            get {
                minX = SMGlobal.SmallerDouble(StartX, EndX);
                return minX;
            }

            set { minX = value; }
        }

        public double MinY
        {
            get {
                minY = SMGlobal.SmallerDouble(StartY, EndY);
                return minY;
            }

            set { minY = value; }
        }

        public virtual SMParcel ParentParcel { get; set; }

        public virtual SMSection ParentSection { get; set; }

        public int Record { get; set; }

        public PointF ScaledEndPoint
        {
            get {
                scaledEndPoint = ComputeScaledEndPoint(EndPoint, ParentParcel.Scale);
                return scaledEndPoint;
            }

            set { scaledEndPoint = value; }
        }

        public PointF ScaledStartPoint
        {
            get { return scaledStartPoint; }

            set { scaledStartPoint = value; }
        }

        public string SectionLetter { get; set; }

        public PointF StartPoint
        {
            get {
                startPoint = new PointF((float)StartX, (float)StartY);
                return startPoint;
            }

            set { startPoint = value; }
        }

        public double StartPointDistanceFromComparisonPoint
        {
            get {
                startPointDistanceFromComparisonPoint = (double)SMGlobal.LineLength(ComparisonPoint, ScaledStartPoint);

                return startPointDistanceFromComparisonPoint;
            }

            set { startPointDistanceFromComparisonPoint = value; }
        }

        public double StartX
        {
            get { return startX; }

            set { startX = value; }
        }

        public double StartY { get; set; }

        public double XLength
        {
            get {
                xLength = (EndX - StartX);
                return xLength;
            }

            set { xLength = value; }
        }

        public double YLength
        {
            get {
                yLength = (EndY - StartY);
                return yLength;
            }

            set { yLength = value; }
        }

        #endregion "Properties"

        #region "Fields"

        private string attachedSection;
        private PointF comparisonPoint;
        private string direction;
        private PointF endPoint;
        private double endPointJumpPointDist;
        private double endY;
        private string lineLabel;
        private double lineLength;
        private PointF midPoint;
        private double minX;
        private double minY;
        private PointF scaledEndPoint;
        private PointF scaledStartPoint;
        private PointF startPoint;
        private double startPointDistanceFromComparisonPoint;
        private double startX;
        private double xLength;
        private double yLength;

        #endregion "Fields"
    }
}