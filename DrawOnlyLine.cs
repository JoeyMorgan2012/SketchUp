using System;
using System.Drawing;
using System.Text;

namespace SketchUp
{

    public class DrawOnlyLine
    {
        #region Class Properties


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

        }

        public double EndX { get; set; }

        public double EndY { get; set; }


        public double LineLength
        {
            get {
                lineLength = SMGlobal.LineLength(new PointF((float)StartX, (float)StartY), new PointF((float)EndX, (float)EndY));
                return lineLength;
            }

        }

        public int LineNumber { get; set; }

        public int Record { get; set; }

        public string SectionLetter { get; set; }


        public double StartX { get; set; }

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

        #endregion Class Properties

        #region Constructors

        public DrawOnlyLine(SMSection section)
        {
            Record = section.Record;
            card = section.Card;
            SectionLetter = section.SectionLetter;
            ParentSection = section;
            ParentParcel = section.ParentParcel;
        }

        #endregion Constructors

        #region Fields

        private int card;

        private string direction;
        private PointF endPoint;

        private string lineLabel;
        private double lineLength;
        private PointF midPoint;
        private PointF scaledEndPoint;
        private double xLength;
        private double yLength;

        #endregion Fields

        #region Virtual/navigation properties

        public virtual SMParcel ParentParcel { get; set; }

        public virtual SMSection ParentSection { get; set; }

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

        private PointF MidPointOfLine()
        {
            float xMidPoint = ScaledStartPoint.X + ((ScaledEndPoint.X - ScaledStartPoint.X) / 2);
            float yMidPoint = ScaledStartPoint.Y + ((ScaledEndPoint.Y - ScaledStartPoint.Y) / 2);
            midPoint = new PointF(xMidPoint, yMidPoint);
            return midPoint;
        }

        #endregion private methods

        #region public methods

        public PointF LineLabelPlacementPoint()
        {

            float labelStartX = ScaledStartPoint.X;
            float labelStartY = ScaledStartPoint.Y;
            float labelLength = LineLabel.Length;
            PointF lineMidPoint = MidPointOfLine();
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


        public string LineLabel
        {
            get {
                lineLabel = FormattedLineLabel();
                return lineLabel;
            }


        }




        public PointF ScaledEndPoint
        {
            get {
                scaledEndPoint = ComputeScaledEndPoint(EndPoint, ParentParcel.Scale);
                return scaledEndPoint;
            }


        }

        public PointF ScaledStartPoint { get; set; }

        private PointF ComputeScaledEndPoint(PointF endPoint, double scale)
        {
            double width = XLength * scale;
            double height = YLength * scale;

            endPoint = new PointF(ScaledStartPoint.X + (float)+width, ScaledStartPoint.Y + (float)height);

            return endPoint;
        }
        #endregion public methods
    }
}
