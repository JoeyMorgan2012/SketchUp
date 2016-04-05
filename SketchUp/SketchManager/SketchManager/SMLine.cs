using System;
using System.Drawing;
using System.Text;

namespace SWallTech
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
				//SizeF lineLengths = new SizeF(, (float)(YLength * ParentParcel.Scale));
				
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
				decimal drawingStartX = StartX;
				decimal drawingStartY = StartY;
				startPoint = new PointF((float)drawingStartX, (float)drawingStartY);
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
				xLength = Math.Abs(EndX - StartX);
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
				yLength = Math.Abs(EndY - StartY);
				return yLength;
			}
			set
			{
				yLength = value;
			}
		}

		#endregion Class Properties

		#region Constructors

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
						  //labelText = string.Format("{0}{1:F2}", arrow, LineLength);
					labelText = string.Format("{0}{1:F2} {2}-{3}", arrow, LineLength,SectionLetter,LineNumber);
					break;

				case "W": //right to left, label goes above line
				case "N": //Bottom to top, label goes to the left
						  //labelText = string.Format("{0:F2}{1}", LineLength, arrow);
					labelText = string.Format(" {2}-{3} {0:F2}{1}", LineLength, arrow, SectionLetter,LineNumber);
					break;

				default:
					break;
			}

			return labelText;
		}



		private PointF MidPointOfLine(PointF sketchOriginPoint)
		{
			float xMidPoint =ScaledStartPoint.X + (ScaledEndPoint.X - ScaledStartPoint.X) / 2;
			float yMidPoint = ScaledStartPoint.Y + (ScaledEndPoint.Y - ScaledStartPoint.Y) / 2;

			return new PointF(xMidPoint, yMidPoint);
		}
		#endregion

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
					labelStartX = ScaledStartPoint.X + sketchOriginPoint.X;
					labelStartY = lineMidPoint.Y + 10 + sketchOriginPoint.Y;
					break;

				case "W": //right to left, label goes above line
					labelStartX = (ScaledEndPoint.X + sketchOriginPoint.X) - (float)(XLength * ParentParcel.Scale);
					labelStartY = lineMidPoint.Y - 20 + sketchOriginPoint.Y;
					break;

				case "N": //Bottom to top, label goes to the left
					labelStartX = lineMidPoint.X + sketchOriginPoint.X - labelLength - 20;
					labelStartY = lineMidPoint.Y + sketchOriginPoint.Y;

					break;

				case "S": //top to bottom, label goes to the right
					labelStartX = lineMidPoint.X + sketchOriginPoint.X - labelLength + 20;
					labelStartY = lineMidPoint.Y + sketchOriginPoint.Y;
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
				return scaledEndPoint;
			}
			set
			{
				scaledEndPoint = value;
			}
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
		#endregion
	}
}
