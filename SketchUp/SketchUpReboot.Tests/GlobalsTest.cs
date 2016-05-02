using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SketchUpReboot;

namespace SketchUpReboot.Tests
{
    [TestClass]
    public class GlobalsTest
    {
        [TestMethod]
        [TestCategory("Reference Data")]
        public void TestSettingsInfo()
        {
            string ip = "192.168.176.241";
            string usr = "CAMRA2";
            string pw = "CAMRA2";
            string loc = "AUG";
            string locDesc = "Augusta County";
            string lib = "NATIVE";
            Assert.AreEqual(ip, GlobalValues.IpAddress);
            Assert.AreEqual(usr, GlobalValues.UserName);
            Assert.AreEqual(pw, GlobalValues.PassWord);
            Assert.AreEqual(lib, GlobalValues.Library);
            Assert.AreEqual(loc, GlobalValues.LocalityPrefix);
            Assert.AreEqual(locDesc, GlobalValues.LocalityName);


        }
        [TestMethod]
        [TestCategory("Reference Data")]
        public void TestSettingsChanges()
        {
            string ip = "192.168.176.241";
            string usr = "CAMRA2";
            string pw = "CAMRA2";
            string loc = "AUG";
            string locDesc = "Augusta County";
            string lib = "NATIVE";
            
            Assert.AreEqual(ip, GlobalValues.IpAddress);
            Assert.AreEqual(usr, GlobalValues.UserName);
            Assert.AreEqual(pw, GlobalValues.PassWord);
            Assert.AreEqual(lib, GlobalValues.Library);
            Assert.AreEqual(loc, GlobalValues.LocalityPrefix);
            Assert.AreEqual(locDesc, GlobalValues.LocalityName);
            GlobalValues.IpAddress = "192.168.176.240";
            GlobalValues.UserName = "Joe";
            GlobalValues.PassWord = "Aardvark";
            GlobalValues.Library = "DaveLib";
            GlobalValues.LocalityPrefix = "ALE";
            Assert.AreNotEqual(ip, GlobalValues.IpAddress);
            Assert.AreNotEqual(usr, GlobalValues.UserName);
            Assert.AreNotEqual(pw, GlobalValues.PassWord);
            Assert.AreNotEqual(lib, GlobalValues.Library);
            Assert.AreNotEqual(loc, GlobalValues.LocalityPrefix);

        }
    }
}
