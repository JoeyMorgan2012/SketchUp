using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;
namespace SketchManager.Tests
{
	[TestClass]
	public class CalculatedPropertiesTests
	{
	
		TestSetup ts = new TestSetup();
		[TestMethod]
		[TestCategory("Calculated Properties")]
		public void AttachedToSectionTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			Assert.IsNotNull(parcel);
			Assert.IsNotNull(parcel.Sections);
			Assert.IsNotNull(parcel.AllSectionLines);
			Assert.AreEqual(5, parcel.Sections.Count);
			Assert.AreEqual(25, parcel.AllSectionLines.Count);
			SMSection sectionA = (from s in parcel.Sections where s.SectionLetter == "A" select s).FirstOrDefault<SMSection>();
			SMSection sectionB = (from s in parcel.Sections where s.SectionLetter == "B" select s).FirstOrDefault<SMSection>();
			SMSection sectionC = (from s in parcel.Sections where s.SectionLetter == "C" select s).FirstOrDefault<SMSection>();
			SMSection sectionD = (from s in parcel.Sections where s.SectionLetter == "D" select s).FirstOrDefault<SMSection>();
			SMSection sectionF = (from s in parcel.Sections where s.SectionLetter == "F" select s).FirstOrDefault<SMSection>();
			Assert.AreEqual(string.Empty, sectionA.AttachedTo);
			Assert.AreEqual("A", sectionB.AttachedTo);
			Assert.AreEqual("A", sectionC.AttachedTo);
			Assert.AreEqual("A", sectionD.AttachedTo);
			Assert.AreEqual("A", sectionF.AttachedTo);

		}
		[TestMethod]
		[TestCategory("Calculated Properties")]
		public void GetCornersListTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			List<PointF> corners = parcel.CornerPoints;
			Assert.IsNotNull(corners);
			Assert.AreEqual(50, corners.Count);
			foreach (PointF p in corners)
			{
				Console.WriteLine(string.Format("({0},{1})",p.X,p.Y));
			}
		}

		[TestMethod]
		[TestCategory("Calculated Properties")]
		public void GetClosestCornersTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			List<PointF> corners = parcel.CornerPoints;
			Assert.IsNotNull(corners);
			PointF pointA = new PointF();
		}
		[TestMethod]
		[TestCategory("Calculated Properties")]
		public void GetXandYSizeTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			Assert.AreEqual( 77,parcel.SketchXSize);
			Assert.AreEqual(47.5, parcel.SketchYSize);

		}
	}
}
