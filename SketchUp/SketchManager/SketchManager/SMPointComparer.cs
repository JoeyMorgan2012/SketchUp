using System;
using System.Collections.Generic;
using System.Linq;

using System.Drawing;
using System.Text;

namespace SWallTech
{
	public class SMPointComparer
	{
		SMLine comparisonLine;
		PointF comparisonPoint;
		decimal endPointDistance;
		decimal scale;

		public SMLine ComparisonLine
		{
			get
			{
				return comparisonLine;
			}

			set
			{
				comparisonLine = value;
			}
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
		PointF sketchOrigin;
		decimal sketchScale;
		public decimal EndPointDistance
		{
			get
			{
				endPointDistance = CalculatedScaledDistance(ComparisonLine,ComparisonPoint);
#if DEBUG
				 Console.WriteLine(string.Format("Scaled End point {0},{1} is {2} from {3},{4}",ComparisonLine.ScaledEndPoint.X,ComparisonLine.ScaledEndPoint.Y,endPointDistance,ComparisonPoint.X,ComparisonPoint.Y ));
#endif
				return endPointDistance;
			}

			set
			{
				endPointDistance = value;
			}
		}

		public decimal Scale
		{
			get
			{
				scale = comparisonLine.ParentParcel.Scale;
				return scale;
			}

			set
			{
				scale = value;
			}
		}

		public PointF SketchOrigin
		{
			get
			{
				return sketchOrigin;
			}

			set
			{
				sketchOrigin = value;
			}
		}

		public decimal SketchScale
		{
			get
			{
				return sketchScale;
			}

			set
			{
				sketchScale = value;
			}
		}

		private decimal CalculatedScaledDistance(SMLine line, PointF point)
		{

			//TODO: Determine how to adjust for centering effect
			//Attempt 1: Adjusting the endPoint
			PointF endpoint = new PointF(line.ScaledEndPoint.X + SketchOrigin.X, line.ScaledEndPoint.Y+SketchOrigin.Y);
#if DEBUG
			Console.WriteLine(string.Format("Line Scaled Endpoint {0},{1} - adjusted endpoint {2},{3} - origin {4},{5} {6}-{7})", line.ScaledEndPoint.X, line.ScaledEndPoint.Y, endpoint.X, endpoint.Y, SketchOrigin.X,SketchOrigin.Y, line.SectionLetter, line.LineNumber));
#endif

			decimal distance = SMGlobal.LineLength(point, endpoint);
			return distance;

		}
	}
}
