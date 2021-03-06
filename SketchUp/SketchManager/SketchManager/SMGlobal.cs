﻿using System;
using System.Drawing;
using System.Text;

namespace SWallTech
{
	public static class SMGlobal
	{
		public enum SnapshotState
		{
			OriginalMainImage,
			InitialEditBase,
			Intermediate,
			CurrentlyEditing,
			PreSaveReview,
			ApprovedForSave
		}

		public const string NorthArrow = "\u2191";
		public const string EastArrow = "\u2192";
		public const string WestArrow = "\u2190";
		public const string SouthArrow = "\u2193";
		public const string NorthEastArrow = "\u2197";
		public const string SouthEastArrow = "\u2198";
		public const string NorthWestArrow = "\u2196";
		public const string SouthWestArrow = "\u2199";

		public static decimal LineLength(PointF startPoint, PointF endPoint)
		{
			double xLength = (double)(endPoint.X - startPoint.X);
			double yLength = (double)(endPoint.Y - startPoint.Y);
			double xLenSquared = (double)Math.Pow(xLength, 2);
			double yLenSquared = (double)Math.Pow(yLength, 2);
			double lengthSquaredTotals = xLenSquared + yLenSquared;
			return (decimal)Math.Sqrt(lengthSquaredTotals);
		}

		public static decimal LargerDecimal(decimal firstNumber, decimal secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return secondNumber;
			}
			else
			{
				return firstNumber;
			}
		}

		public static int LargerInteger(int firstNumber, int secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return secondNumber;
			}
			else
			{
				return firstNumber;
			}
		}

		public static float LargerFloat(float firstNumber, float secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return secondNumber;
			}
			else
			{
				return firstNumber;
			}
		}

		public static decimal LargerDouble(decimal firstNumber, decimal secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return secondNumber;
			}
			else
			{
				return firstNumber;
			}
		}

		public static decimal SmallerDecimal(decimal firstNumber, decimal secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return firstNumber;
			}
			else
			{
				return secondNumber;
			}
		}

		public static int SmallerInteger(int firstNumber, int secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return firstNumber;
			}
			else
			{
				return secondNumber;
			}
		}

		public static float SmallerFloat(float firstNumber, float secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return firstNumber;
			}
			else
			{
				return secondNumber;
			}
		}

		public static decimal SmallerDouble(decimal firstNumber, decimal secondNumber)
		{
			if (secondNumber >= firstNumber)
			{
				return firstNumber;
			}
			else
			{
				return secondNumber;
			}
		}

		public static bool PointIsOnLine(PointF lineStart, PointF lineEnd, PointF checkedPoint)
		{
			bool lineContainsPoint = false;
			bool checkedPointIsBetweenEndpoints = CheckedPointIsBetweenEndpoints(lineStart, lineEnd, checkedPoint);

			decimal mainLineSlope = LineSlope(lineStart, lineEnd);
			decimal checkLineSlope = LineSlope(lineStart, checkedPoint);
			lineContainsPoint = (checkedPointIsBetweenEndpoints && (Math.Abs(mainLineSlope) == Math.Abs(checkLineSlope)));
			return lineContainsPoint;

		}

		private static bool CheckedPointIsBetweenEndpoints(PointF lineStart, PointF lineEnd, PointF checkedPoint)
		{
			bool xInRange = false;
			bool yInRange = false;

			// handles the situation where the endpoint is less than the start point based on line direction or the order in which the paramters are passed in
			if (lineEnd.Y <= lineStart.Y)
			{
				yInRange = checkedPoint.Y >= lineEnd.Y && checkedPoint.Y <= lineStart.Y;
			}
			else
			{
				yInRange = checkedPoint.Y <= lineEnd.Y && checkedPoint.Y >= lineStart.Y;
			}
			if (lineEnd.X <= lineStart.X)
			{
				xInRange = checkedPoint.X >= lineEnd.X && checkedPoint.X <= lineStart.X;
			}
			else
			{
				xInRange = checkedPoint.X <= lineEnd.X && checkedPoint.X >= lineStart.X;
			}
			return xInRange && yInRange;
		}

		private static decimal LineSlope(PointF lineStart, PointF lineEnd)
		{
			decimal rise = (decimal)lineEnd.Y - (decimal)lineStart.Y;
			decimal run = (decimal)lineEnd.X - (decimal)lineStart.X;
			decimal slope = 0.0000M;
			if (run == 0)
			{
				slope = 0.0000M;
			}
			else
			{
				slope = rise / run;
			}
			return slope;
		}
	}
}