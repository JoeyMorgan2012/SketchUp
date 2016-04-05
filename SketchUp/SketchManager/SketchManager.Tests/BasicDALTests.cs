using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;

namespace SketchManager.Tests
{
	[TestClass]
	public class BasicDALTests
	{
		string dataSource = "192.168.176.241";
		string locality = "AUG";
		string password = "CAMRA2";
		string userName = "CAMRA2";
		TestSetup ts = new TestSetup();
				[TestCategory("DAL")]
		[TestMethod]
		public void GetParcelTest()
		{
			/*
				JMRECORD	11787
				JMDWELL		1
				JMSKETCH	Y
				JMSTORY		2
				JMSTORYEX
				JMSCALE		1
				JMTOTSQFT	3429.5
				JMESKETCH

			*/
			SketchRepository sr = new SketchRepository(ts.DataSource, ts.UserName, ts.Password, ts.Locality);
			SMParcel parcel = sr.SelectParcelData(ts.Record,ts.Dwelling);
			Assert.IsNotNull(parcel);
			Assert.AreEqual(1, parcel.Card);
			Assert.AreEqual("Y", parcel.HasSketch);
			Assert.AreEqual(2, parcel.Storeys);
			Assert.AreEqual(string.Empty, parcel.StoreyEx);
			Assert.AreEqual(1M, parcel.Scale);
			Assert.AreEqual(3429.5M, parcel.TotalSqFt);
			Assert.AreEqual(string.Empty, parcel.ExSketch);
		}

		[TestCategory("DAL")]
		[TestMethod]
		public void GetSectionsTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			List<SMSection> sections = parcel.Sections;

			//Verify that the list was created and populated with the 5 sections
			Assert.IsNotNull(sections);
			Assert.AreEqual(5, sections.Count);

			//Get each section to read its values and test against the expected
			SMSection sectionA = (from s in sections where s.SectionLetter == "A" select s).FirstOrDefault<SMSection>();
			SMSection sectionB = (from s in sections where s.SectionLetter == "B" select s).FirstOrDefault<SMSection>();
			SMSection sectionC = (from s in sections where s.SectionLetter == "C" select s).FirstOrDefault<SMSection>();
			SMSection sectionD = (from s in sections where s.SectionLetter == "D" select s).FirstOrDefault<SMSection>();
			SMSection sectionF = (from s in sections where s.SectionLetter == "F" select s).FirstOrDefault<SMSection>();

			//Make sure they are not null, just for GP
			Assert.IsNotNull(sectionA);
			Assert.IsNotNull(sectionB);
			Assert.IsNotNull(sectionC);
			Assert.IsNotNull(sectionD);
			Assert.IsNotNull(sectionF);
			/* Data values
				JSRECORD	11787	11787	11787	11787	11787
				JSDWELL		1		1		1		1		1
				JSSECT		A 		B 		C 		D 		F
				JSTYPE		BASE	GAR 	EPOR	WCP 	POR
				JSSTORY		2		1		1		1		1
				JSDESC
				JSSKETCH	Y		Y		Y		Y		Y
				JSSQFT		2442	360		187.5	280		160
				JS0DEPR
				JSCLASS		C		C		C		C		C
				JSVALUE		0		0		0		0		0
				JSFACTOR	0		0		0		0		0
				JSDEPRC		0		0		0		0		0
				*/

			//Check Section A
			Assert.AreEqual("A", sectionA.SectionLetter);
			Assert.AreEqual("BASE", sectionA.SectionType);
			Assert.AreEqual(2M, sectionA.Storeys);
			Assert.AreEqual(string.Empty, sectionA.Description);
			Assert.AreEqual("Y", sectionA.HasSketch);
			Assert.AreEqual(2442M, sectionA.SqFt);
			Assert.AreEqual(string.Empty, sectionA.ZeroDepr);
			Assert.AreEqual("C", sectionA.SectionClass);
			Assert.AreEqual(0M, sectionA.SectionValue);
			Assert.AreEqual(0M, sectionA.AdjFactor);
			Assert.AreEqual(0M, sectionA.Depreciation);

			//Check Section B
			Assert.AreEqual("B", sectionB.SectionLetter);
			Assert.AreEqual("GAR", sectionB.SectionType);
			Assert.AreEqual(1M, sectionB.Storeys);
			Assert.AreEqual(string.Empty, sectionB.Description);
			Assert.AreEqual("Y", sectionB.HasSketch);
			Assert.AreEqual(360M, sectionB.SqFt);
			Assert.AreEqual(string.Empty, sectionB.ZeroDepr);
			Assert.AreEqual("C", sectionB.SectionClass);
			Assert.AreEqual(0M, sectionB.SectionValue);
			Assert.AreEqual(0M, sectionB.AdjFactor);
			Assert.AreEqual(0M, sectionB.Depreciation);

			//Check Section C
			Assert.AreEqual("C", sectionC.SectionLetter);
			Assert.AreEqual("EPOR", sectionC.SectionType);
			Assert.AreEqual(1, sectionC.Storeys);
			Assert.AreEqual(string.Empty, sectionC.Description);
			Assert.AreEqual("Y", sectionC.HasSketch);
			Assert.AreEqual(187.5M, sectionC.SqFt);
			Assert.AreEqual(string.Empty, sectionC.ZeroDepr);
			Assert.AreEqual("C", sectionC.SectionClass);
			Assert.AreEqual(0M, sectionC.SectionValue);
			Assert.AreEqual(0M, sectionC.AdjFactor);
			Assert.AreEqual(0M, sectionC.Depreciation);

			//Check Section D
			Assert.AreEqual("D", sectionD.SectionLetter);
			Assert.AreEqual("WCP", sectionD.SectionType);
			Assert.AreEqual(1M, sectionD.Storeys);
			Assert.AreEqual(string.Empty, sectionD.Description);
			Assert.AreEqual("Y", sectionD.HasSketch);
			Assert.AreEqual(280M, sectionD.SqFt);
			Assert.AreEqual(string.Empty, sectionD.ZeroDepr);
			Assert.AreEqual("C", sectionD.SectionClass);
			Assert.AreEqual(0M, sectionD.SectionValue);
			Assert.AreEqual(0M, sectionD.AdjFactor);
			Assert.AreEqual(0M, sectionD.Depreciation);

			//Check Section F
			Assert.AreEqual("F", sectionF.SectionLetter);
			Assert.AreEqual("POR", sectionF.SectionType);
			Assert.AreEqual(1M, sectionF.Storeys);
			Assert.AreEqual(string.Empty, sectionF.Description);
			Assert.AreEqual("Y", sectionF.HasSketch);
			Assert.AreEqual(160M, sectionF.SqFt);
			Assert.AreEqual(string.Empty, sectionF.ZeroDepr);
			Assert.AreEqual("C", sectionF.SectionClass);
			Assert.AreEqual(0M, sectionF.SectionValue);
			Assert.AreEqual(0M, sectionF.AdjFactor);
			Assert.AreEqual(0M, sectionF.Depreciation);
		}

		[TestCategory("DAL")]
		[TestMethod]
		public void GetLinesTest()
		{
			SMParcel parcel = ts.TestParcel(ts.DataSource, ts.UserName, ts.Password, ts.Locality, ts.Record, ts.Dwelling);
			SketchRepository sr = new SketchRepository(ts.DataSource, ts.UserName, ts.Password, ts.Locality);
			List<SMSection> sections = parcel.Sections;
			SMSection sectionA = (from s in parcel.Sections where s.SectionLetter == "A" select s).FirstOrDefault<SMSection>();
			SMSection sectionB = (from s in parcel.Sections where s.SectionLetter == "B" select s).FirstOrDefault<SMSection>();
			SMSection sectionC = (from s in parcel.Sections where s.SectionLetter == "C" select s).FirstOrDefault<SMSection>();
			SMSection sectionD = (from s in parcel.Sections where s.SectionLetter == "D" select s).FirstOrDefault<SMSection>();
			SMSection sectionF = (from s in parcel.Sections where s.SectionLetter == "F" select s).FirstOrDefault<SMSection>();

			List<SMLine> linesA = sr.SelectSectionLines(sectionA);
			List<SMLine> linesB = sr.SelectSectionLines(sectionB);
			List<SMLine> linesC = sr.SelectSectionLines(sectionC);
			List<SMLine> linesD = sr.SelectSectionLines(sectionD);
			List<SMLine> linesF = sr.SelectSectionLines(sectionF);
			Assert.IsNotNull(linesA);
			Assert.IsNotNull(linesB);
			Assert.IsNotNull(linesC);
			Assert.IsNotNull(linesD);
			Assert.IsNotNull(linesF);

			//Test Section B Values against data (because B is a small section)
			/*
			Column		1		2		3		4
			JLRECORD	11787	11787	11787	11787
			JLDWELL		1		1		1		1
			JLSECT		B 		B 		B 		B
			JLLINE#		1		2		3		4
			JLDIRECT	E 		S 		W 		N
			JLXLEN		15		0		15		0
			JLYLEN		0		24		0		24
			JLLINELEN	15		24		15		24
			JLANGLE		0		0		0		0
			JLPT1X		48		63		63		48
			JLPT1Y		-21		-21		3		3
			JLPT2X		63		63		48		48
			JLPT2Y		-21		3		3		-21
			JLATTACH
			*/

			SMLine LineB1 = (from l in linesB.ToList<SMLine>() where l.LineNumber == 1 select l).FirstOrDefault<SMLine>();
			SMLine LineB2 = (from l in linesB.ToList<SMLine>() where l.LineNumber == 2 select l).FirstOrDefault<SMLine>();
			SMLine LineB3 = (from l in linesB.ToList<SMLine>() where l.LineNumber == 3 select l).FirstOrDefault<SMLine>();
			SMLine LineB4 = (from l in linesB.ToList<SMLine>() where l.LineNumber == 4 select l).FirstOrDefault<SMLine>();

			//Verify the objects exist
			Assert.IsNotNull(LineB1);
			Assert.IsNotNull(LineB2);
			Assert.IsNotNull(LineB3);
			Assert.IsNotNull(LineB4);

			//Check Line 1 values

			Assert.AreEqual(11787, LineB1.Record);
			Assert.AreEqual(1, LineB1.Dwelling);
			Assert.AreEqual("E", LineB1.Direction);
			Assert.AreEqual(15, LineB1.XLength);
			Assert.AreEqual(0, LineB1.YLength);
			Assert.AreEqual(15, LineB1.LineLength);
			Assert.AreEqual(0, LineB1.LineAngle);
			Assert.AreEqual(48, LineB1.StartX);
			Assert.AreEqual(-21, LineB1.StartY);
			Assert.AreEqual(63, LineB1.EndX);
			Assert.AreEqual(-21, LineB1.EndY);
			Assert.AreEqual(string.Empty, LineB1.AttachedSection);
			
			Assert.AreEqual(new PointF(48, -21), LineB1.StartPoint);
			Assert.AreEqual(new PointF(63, -21), LineB1.EndPoint);

			// Check Line 2 Values
			Assert.AreEqual(11787, LineB2.Record);
			Assert.AreEqual(1, LineB2.Dwelling);
			Assert.AreEqual("S", LineB2.Direction);
			Assert.AreEqual(0, LineB2.XLength);
			Assert.AreEqual(24, LineB2.YLength);
			Assert.AreEqual(24, LineB2.LineLength);
			Assert.AreEqual(0, LineB2.LineAngle);
			Assert.AreEqual(63, LineB2.StartX);
			Assert.AreEqual(-21, LineB2.StartY);
			Assert.AreEqual(63, LineB2.EndX);
			Assert.AreEqual(3, LineB2.EndY);
			Assert.AreEqual(string.Empty, LineB2.AttachedSection);
			
			Assert.AreEqual(new PointF(63 , -21), LineB2.StartPoint);
			Assert.AreEqual(new PointF(63, 3), LineB2.EndPoint);

			// Check Line 3 values
			Assert.AreEqual(11787, LineB3.Record);
			Assert.AreEqual(1, LineB3.Dwelling);
			Assert.AreEqual("W", LineB3.Direction);
			Assert.AreEqual(15, LineB3.XLength);
			Assert.AreEqual(0, LineB3.YLength);
			Assert.AreEqual(15, LineB3.LineLength);
			Assert.AreEqual(0, LineB3.LineAngle);
			Assert.AreEqual(63, LineB3.StartX);
			Assert.AreEqual(3, LineB3.StartY);
			Assert.AreEqual(48, LineB3.EndX);
			Assert.AreEqual(3, LineB3.EndY);
			Assert.AreEqual(string.Empty, LineB3.AttachedSection);
			
			Assert.AreEqual(new PointF(63, 3), LineB3.StartPoint);
			Assert.AreEqual(new PointF(48, 3), LineB3.EndPoint);

			// Check Line 4 values
			Assert.AreEqual(11787, LineB4.Record);
			Assert.AreEqual(1, LineB4.Dwelling);
			Assert.AreEqual("N", LineB4.Direction);
			Assert.AreEqual(0, LineB4.XLength);
			Assert.AreEqual(24, LineB4.YLength);
			Assert.AreEqual(24, LineB4.LineLength);
			Assert.AreEqual(0, LineB4.LineAngle);
			Assert.AreEqual(48, LineB4.StartX);
			Assert.AreEqual(3, LineB4.StartY);
			Assert.AreEqual(48, LineB4.EndX);
			Assert.AreEqual(-21, LineB4.EndY);
			Assert.AreEqual(string.Empty, LineB4.AttachedSection);
			
			Assert.AreEqual(new PointF(48, 3), LineB4.StartPoint);
			Assert.AreEqual(new PointF(48, -21), LineB4.EndPoint);
		}
	}
}