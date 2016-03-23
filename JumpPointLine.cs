using System;
using System.Drawing;
using System.Text;

namespace SketchUp
{
	public class JumpLinePoint
	{
		public JumpLinePoint(PointF pointToCompare)
		{
			referencePoint = pointToCompare;
		}

		public JumpLinePoint()
		{
			referencePoint = new PointF(0, 0);
		}

		private PointF drawingPoint;
		private PointF labelPoint;
		private PointF referencePoint;
		private PointF beginPoint;
		private PointF endPoint;
		private string lineDirection;
		private string pointSection;
		private double distanceFromReferencePoint;

		public PointF LabelPoint
		{
			get
			{
				labelPoint = new PointF(DrawingPoint.X + 10, DrawingPoint.Y);
				return labelPoint;
			}
		}

		public PointF DrawingPoint
		{
			get
			{
				return drawingPoint;
			}

			set
			{
				drawingPoint = value;
			}
		}

		public PointF ReferencePoint
		{
			get
			{
				return referencePoint;
			}

			set
			{
				referencePoint = value;
			}
		}

		public double DistanceFromReferencePoint
		{
			get
			{
				distanceFromReferencePoint = CalculateDistance();
				return distanceFromReferencePoint;
			}

			set
			{
				distanceFromReferencePoint = value;
			}
		}

		public string PointSection
		{
			get
			{
				return pointSection.ToUpper().Trim();
			}

			set
			{
				pointSection = value;
			}
		}

		public int PointLine
		{
			get
			{
				return pointLine;
			}

			set
			{
				pointLine = value;
			}
		}

		public PointF BeginPoint
		{
			get
			{
				return beginPoint;
			}

			set
			{
				beginPoint = value;
			}
		}

		public PointF EndPoint
		{
			get
			{
				return endPoint;
			}

			set
			{
				endPoint = value;
			}
		}

		public string LineDirection
		{
			get
			{
				return lineDirection;
			}

			set
			{
				lineDirection = value;
			}
		}

		private int pointLine;

		private double CalculateDistance()
		{
			double xDistanceSquared = Math.Pow((DrawingPoint.X - ReferencePoint.X), 2);
			double yDistanceSquared = Math.Pow((DrawingPoint.Y - ReferencePoint.Y), 2);
			double sumOfSquares = xDistanceSquared + yDistanceSquared;
			double distance = Math.Round(Math.Sqrt(sumOfSquares), 1);
			return distance;
		}
	}
}