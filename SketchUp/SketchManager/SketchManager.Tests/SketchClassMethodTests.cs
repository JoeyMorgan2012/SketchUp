using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;
namespace SketchManager.Tests
{
	[TestClass]
	public class SketchClassMethodTests
	{ TestSetup ts = new TestSetup();
		[TestMethod]
		[TestCategory("Class Method Tests")]
		public void SectionSelectionMethodTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			SMSection sectionA = parcel.SelectSectionByLetter("A");
			SMSection sectionB = parcel.SelectSectionByLetter("B");
			Assert.IsNotNull(sectionA);
			Assert.IsNotNull(sectionB);
		
			SMSection sectionShouldBeNull = parcel.SelectSectionByLetter("E");
			Assert.IsNull(sectionShouldBeNull);

		}
		[TestMethod]
		[TestCategory("Class Method Tests")]
		public void LineSelectionMethodTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			SMLine lineA1= parcel.SelectLineBySectionAndNumber("A", 1);
			Assert.IsNotNull(lineA1);
			SMLine nonExistentLine= parcel.SelectLineBySectionAndNumber("A", 10);
			Assert.IsNull(nonExistentLine);
			nonExistentLine = parcel.SelectLineBySectionAndNumber("G", 5);
			Assert.IsNull(nonExistentLine);

		}
	}
}
