using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;

using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public partial class ExpandoSketch : Form
	{
		#region refactored methods

		public static decimal RoundToNearestHalf(decimal value)
		{
			return Math.Round(((value * 2) / 2), 1);
		}

		private string ReverseDirection(string direction)
		{
#if DEBUG

			//Debugging Code -- remove for production release
			var fullStack = new System.Diagnostics.StackTrace(true).GetFrames();
			UtilityMethods.LogMethodCall(fullStack, true);
#endif
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
				default:
					Console.WriteLine(string.Format("Error occurred in {0}, in procedure {1}.\n{2} is not a valid direction value.", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, direction));
					break;
			}

			return reverseDirection;
		}

		#endregion refactored methods

		#region Linq calculation of jump point attachment points

		public List<JumpLinePoint> ClosestPoints(DataTable jumpTable, PointF jumpPoint)
		{
			List<JumpLinePoint> endPoints = new List<JumpLinePoint>();

			float endPointX = 0f;
			float endPointY = 0f;
			string pointLabel;
			for (int i = 0; i < JumpTable.Rows.Count; i++)
			{
				DataRow dr = JumpTable.Rows[i];
				pointLabel = dr["sect"].ToString().Trim();
				float.TryParse(dr["XPt2"].ToString(), out endPointX);
				float.TryParse(dr["YPt2"].ToString(), out endPointY);
				JumpLinePoint endPoint = new JumpLinePoint { PointSection = pointLabel, DrawingPoint = new PointF(endPointX, endPointY), ReferencePoint = jumpPoint };
				endPoints.Add(endPoint);
			}

			// Conversion to decimal eliminates the floating point numbers comparison issue. Wouldn't want to use this hack sending a man to Mars, but it works for out purposes. --JMM
			decimal shortestDistance = (decimal)(from e in endPoints select e.DistanceFromReferencePoint).Min();
			List<JumpLinePoint> closestPoints = (from ep in endPoints where (decimal)ep.DistanceFromReferencePoint == (decimal)shortestDistance select ep).ToList<JumpLinePoint>();
			return closestPoints;
		}

		#endregion Linq calculation of jump point attachment points
	}
}