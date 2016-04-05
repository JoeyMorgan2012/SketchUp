using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SketchManager;
using System.Drawing;
using SWallTech;

namespace SketchManager.Tests
{
	[TestClass]
	public class MathTests
	{
		PointF origin = new PointF(0, 0);
		PointF x1y1 = new PointF(10, 20);
		PointF x2y2 = new PointF(40, 80);
		PointF x3y3 = new PointF(20, 40); //on line x1Y1=>x2Y2
		PointF x4y4 = new PointF(10, 15); //not on line x1Y1=>x2Y2

		[TestMethod]
		[TestCategory("Math")]
		public void TestPointOnLine()
		{

			Assert.IsTrue(SMGlobal.PointIsOnLine(x1y1, x2y2, x3y3));
			Assert.IsFalse(SMGlobal.PointIsOnLine(x1y1, x2y2, x4y4));
			Assert.IsFalse(SMGlobal.PointIsOnLine(x1y1, x2y2, origin));
		}
	}
}
