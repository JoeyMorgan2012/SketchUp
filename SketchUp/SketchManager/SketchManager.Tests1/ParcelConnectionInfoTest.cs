// <copyright file="ParcelConnectionInfoTest.cs" company="Stonewall Technologies">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;

namespace SWallTech.Tests
{
    /// <summary>This class contains parameterized unit tests for ParcelConnectionInfo</summary>
    [PexClass(typeof(ParcelConnectionInfo))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ParcelConnectionInfoTest
    {
        /// <summary>Test stub for .ctor(String, String, String, String)</summary>
        [PexMethod]
        public ParcelConnectionInfo ConstructorTest(
            string DataSourceIPAddress,
            string UserId,
            string UserPassword,
            string LocalityPrefix
        )
        {
            ParcelConnectionInfo target = new ParcelConnectionInfo
                                              (DataSourceIPAddress, UserId, UserPassword, LocalityPrefix);
            return target;
            // TODO: add assertions to method ParcelConnectionInfoTest.ConstructorTest(String, String, String, String)
        }

        /// <summary>Test stub for get_DbConn()</summary>
        [PexMethod]
        public CAMRA_Connection DbConnGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            CAMRA_Connection result = target.DbConn;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.DbConnGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_DbConn(CAMRA_Connection)</summary>
        [PexMethod]
        public void DbConnSetTest(
            [PexAssumeUnderTest]ParcelConnectionInfo target,
            CAMRA_Connection value
        )
        {
            target.DbConn = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.DbConnSetTest(ParcelConnectionInfo, CAMRA_Connection)
        }

        /// <summary>Test stub for get_IpAddress()</summary>
        [PexMethod]
        public string IpAddressGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.IpAddress;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.IpAddressGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_IpAddress(String)</summary>
        [PexMethod]
        public void IpAddressSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.IpAddress = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.IpAddressSetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_Library()</summary>
        [PexMethod]
        public string LibraryGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.Library;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.LibraryGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for get_LineTable()</summary>
        [PexMethod]
        public string LineTableGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.LineTable;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.LineTableGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_LineTable(String)</summary>
        [PexMethod]
        public void LineTableSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.LineTable = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.LineTableSetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_Locality()</summary>
        [PexMethod]
        public string LocalityGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.Locality;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.LocalityGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_Locality(String)</summary>
        [PexMethod]
        public void LocalitySetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.Locality = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.LocalitySetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_MasterTable()</summary>
        [PexMethod]
        public string MasterTableGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.MasterTable;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.MasterTableGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_MasterTable(String)</summary>
        [PexMethod]
        public void MasterTableSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.MasterTable = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.MasterTableSetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_Password()</summary>
        [PexMethod]
        public string PasswordGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.Password;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.PasswordGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_Password(String)</summary>
        [PexMethod]
        public void PasswordSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.Password = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.PasswordSetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_SectionTable()</summary>
        [PexMethod]
        public string SectionTableGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.SectionTable;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.SectionTableGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_SectionTable(String)</summary>
        [PexMethod]
        public void SectionTableSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.SectionTable = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.SectionTableSetTest(ParcelConnectionInfo, String)
        }

        /// <summary>Test stub for get_UserName()</summary>
        [PexMethod]
        public string UserNameGetTest([PexAssumeUnderTest]ParcelConnectionInfo target)
        {
            string result = target.UserName;
            return result;
            // TODO: add assertions to method ParcelConnectionInfoTest.UserNameGetTest(ParcelConnectionInfo)
        }

        /// <summary>Test stub for set_UserName(String)</summary>
        [PexMethod]
        public void UserNameSetTest([PexAssumeUnderTest]ParcelConnectionInfo target, string value)
        {
            target.UserName = value;
            // TODO: add assertions to method ParcelConnectionInfoTest.UserNameSetTest(ParcelConnectionInfo, String)
        }
    }
}
