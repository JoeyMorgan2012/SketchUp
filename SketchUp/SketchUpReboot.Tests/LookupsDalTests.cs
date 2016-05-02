using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWallTech;
using SketchUpReboot;
using SketchUp;
using System.Collections.Generic;

namespace SketchUpReboot.Tests
{
    [TestClass]
    public class LookupsDalTests
    {
        [TestMethod]
        [TestCategory("Lookups")]
        public void SelectInstalledLocalitiesTest()
        {
            string ip = "192.168.176.241";
            string usr = "CAMRA2";
            string pw = "CAMRA2";
            string loc = "AUG";
            string locDesc = "Augusta County";
            string lib = "NATIVE";
            List<InstalledLocalityRecord> localities = new List<InstalledLocalityRecord>();
            LookupsDalRepo repo = new LookupsDalRepo();
            localities = repo.InstalledLocalitiesList(ip,usr,pw);
            Assert.IsNotNull(localities);
            Assert.AreEqual(64, localities.Count);
            Console.WriteLine("Prefix\t\tDescription\t\tLibrary");
            foreach (InstalledLocalityRecord l in localities)
            {
                Console.WriteLine(string.Format("{0}\t\t{1}\t\t{2}", l.Prefix,l.Description,l.Library));
            }
        }
    }
}
