using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;

namespace SketchManager.Tests
{
	[TestClass]
	public class ParcelConnectionInfoTests
	{
		string dataSource = "192.168.176.241";
		string locality = "AUG";
		string password = "CAMRA2";
		string userName = "CAMRA2";

		string expected_library = "NATIVE";
		string expected_lineTable = "NATIVE.AUGMASTER";
		string expected_masterTable = "NATIVE.AUGMASTER";
		string expected_password = "CAMRA2";
		string expected_sectionTable = "NATIVE.AUGMASTER";
		string expected_userName = "CAMRA2";
		string actual_library = string.Empty;
		string actual_dataSource = string.Empty;
		string actual_userName = string.Empty;
		string actual_password = string.Empty;
		string actual_masterTable = string.Empty;
		string actual_sectionTable = string.Empty;
		string actual_lineTable = string.Empty;


		[TestCategory("Parcel Connection Tests")]
		[TestMethod]
		public void ParcelConnectionCreationWithParametersTest()
		{
			SMConnection pci = new SMConnection(dataSource, userName, password, locality);

			Assert.IsNotNull(pci);
		}
		[TestCategory("Parcel Connection Tests")]
		[TestMethod]
		public void ParcelReturnsLibraryTest()
		{
			SMConnection pci = new SMConnection(dataSource, userName, password, locality);
			Assert.IsNotNull(pci);
			Assert.AreEqual(expected_userName, pci.UserName);
			Assert.AreEqual(expected_library, pci.Library);
		}
	}
}