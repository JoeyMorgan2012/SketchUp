using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public static class SketchUpGlobals
    {
        #region Properties

        public static CAMRA_Connection CamraDbConn
        {
            get {
                if (camraDbConn == null)
                {
                    camraDbConn = new CAMRA_Connection(IpAddress, "CAMRA2", "CAMRA2");
                    camraDbConn.LocalityPrefix = LocalityPrefix;
                }
                return camraDbConn;
            }

            set { camraDbConn = value; }
        }

        public static int Card { get; set; } = 0;

        public static int Checker { get; set; } = 0;

        public static Image CurrentSketchImage { get; set; }

        public static DBAccessManager DbAccessMgr { get; set; } = null;

        public static double DefaultScale { get; set; }

        public static int FcCard { get; set; } = 0;

        public static string FcIpAddress { get; set; } = string.Empty;

        public static string FcLib { get; set; } = string.Empty;

        public static string FcLocalityPrefix { get; set; } = string.Empty;

        public static int FcRecord { get; set; } = 0;

        public static bool HasNewSketch { get; set; } = false;

        public static bool HasSketch { get; set; } = false;

        public static int InitalCard { get; set; } = 0;

        public static int InitalRecord { get; set; } = 0;

        public static string IpAddress { get; set; } = string.Empty;

        public static string LocalityDescription { get; set; } = string.Empty;

        public static string LocalityPrefix { get; set; } = string.Empty;

        public static string LocalLib
        {
            get {
                if (string.IsNullOrEmpty(localLib))
                {
                    localLib = "NATIVE";
                }
                return localLib;
            }

            set { localLib = value; }
        }

        public static bool MainFormIsClosed { get; set; } = false;

        public static bool MainFormIsMinimized { get; set; } = false;

        public static int Month { get; set; }

        public static SMParcelMast ParcelMast
        {
            get {
                if (parcelMast == null)
                {
                    parcelMast = SketchMgrRepo.SelectParcelMasterWithParcel(Record, Card);
                    SMParcelFromData = parcelMast.Parcel;
                }
                return parcelMast;
            }

            set { parcelMast = value; }
        }

        public static SMParcel ParcelWorkingCopy
        {
            get {
                if (SketchSnapshots != null && SketchSnapshots.Count > 0)
                {
                    parcelWorkingCopy = (from p in SketchSnapshots.OrderByDescending(i => i.SnapshotIndex) select p).FirstOrDefault();
                }
                else
                {
                    ParcelMast = SketchMgrRepo.SelectParcelMasterWithParcel(Record, Card);
                    parcelWorkingCopy = ParcelMast.Parcel;
                    parcelWorkingCopy.SnapshotIndex = 0;
                    SketchSnapshots.Add(parcelWorkingCopy);
                }

                return parcelWorkingCopy;
            }

            set { parcelWorkingCopy = value; }
        }

        public static string Password { get; set; } = "CAMRA2";

        public static int Record { get; set; } = 0;

        public static string ReOpenSection { get; set; }

        public static string SketchFolder { get; set; }

        public static Image SketchImage { get; set; }

        public static SketchRepository SketchMgrRepo
        {
            get {
                if (sketchMgrRepo == null)
                {
                    sketchMgrRepo = new SketchRepository(IpAddress, "CAMRA2", "CAMRA2", LocalityPrefix);
                }
                return sketchMgrRepo;
            }

            set { sketchMgrRepo = value; }
        }

        public static List<SMParcel> SketchSnapshots
        {
            get {
                if (sketchSnapshots == null)
                {
                    sketchSnapshots = new List<SMParcel>();
                }
                return sketchSnapshots;
            }

            set { sketchSnapshots = value; }
        }

        public static SMParcel SMParcelFromData
        {
            get {
                if (smParcelFromData == null)
                {
                    smParcelFromData = SketchMgrRepo.SelectParcelMasterWithParcel(Record, Card).Parcel;
                }

                return smParcelFromData;
            }

            set { smParcelFromData = value; }
        }

        public static string UserName { get; set; } = "CAMRA2";

        #endregion Properties

        #region Fields

        private static string _selectedPath = string.Empty;
        private static string _selectedPicPath = string.Empty;
        private static string _selectedSktPath = string.Empty;
        private static CAMRA_Connection camraDbConn = null;
        private static string localLib = string.Empty;
        private static SMParcelMast parcelMast;
        private static SMParcel parcelWorkingCopy;
        private static SketchRepository sketchMgrRepo;
        private static List<SMParcel> sketchSnapshots;
        private static SMParcel smParcelFromData;

        #endregion Fields
    }
}