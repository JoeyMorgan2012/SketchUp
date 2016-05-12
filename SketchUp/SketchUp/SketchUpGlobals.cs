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
        public static CAMRA_Connection CamraDbConn
        {
            get
            {
                return camraDbConn;
            }

            set
            {
                camraDbConn = value;
            }
        }

        public static int Card
        {
            get
            {
                return card;
            }

            set
            {
                card = value;
            }
        }

        public static int Checker
        {
            get
            {
                return checker;
            }

            set
            {
                checker = value;
            }
        }

        public static Image CurrentSketchImage
        {
            get
            {
                return currentSketchImage;
            }

            set
            {
                currentSketchImage = value;
            }
        }

        public static DBAccessManager DbAccessMgr
        {
            get
            {
                return dbAccessMgr;
            }

            set
            {
                dbAccessMgr = value;
            }
        }

        public static decimal DefaultScale
        {
            get
            {
                return defaultScale;
            }

            set
            {
                defaultScale = value;
            }
        }

        public static int FcCard
        {
            get
            {
                return fcCard;
            }

            set
            {
                fcCard = value;
            }
        }

        public static string FcIpAddress
        {
            get
            {
                return fcIpAddress;
            }

            set
            {
                fcIpAddress = value;
            }
        }

        public static string FcLib
        {
            get
            {
                return fcLib;
            }

            set
            {
                fcLib = value;
            }
        }

        public static string FcLocalityPrefix
        {
            get
            {
                return fcLocalityPrefix;
            }

            set
            {
                fcLocalityPrefix = value;
            }
        }

        public static int FcRecord
        {
            get
            {
                return fcRecord;
            }

            set
            {
                fcRecord = value;
            }
        }

        public static bool HasNewSketch
        {
            get
            {
                return hasNewSketch;
            }

            set
            {
                hasNewSketch = value;
            }
        }

        public static bool HasSketch
        {
            get
            {
                return hasSketch;
            }

            set
            {
                hasSketch = value;
            }
        }

        public static int InitalCard
        {
            get
            {
                return initalCard;
            }

            set
            {
                initalCard = value;
            }
        }

        public static int InitalRecord
        {
            get
            {
                return initalRecord;
            }

            set
            {
                initalRecord = value;
            }
        }

        public static string IpAddress
        {
            get
            {
                return ipAddress;
            }

            set
            {
                ipAddress = value;
            }
        }

        public static string LocalityDescription
        {
            get
            {
                return localityDescription;
            }

            set
            {
                localityDescription = value;
            }
        }

        public static string LocalityPrefix
        {
            get
            {
                return localityPreFix;
            }

            set
            {
                localityPreFix = value;
            }
        }

        public static string LocalLib
        {
            get
            {
                if (string.IsNullOrEmpty(localLib))
                {
                    localLib = "NATIVE";
                }
                return localLib;
            }

            set
            {
                localLib = value;
            }
        }

        public static bool MainFormIsClosed
        {
            get
            {
                return mainFormIsClosed;
            }

            set
            {
                mainFormIsClosed = value;
            }
        }

        public static bool MainFormIsMinimized
        {
            get
            {
                return mainFormIsMinimized;
            }

            set
            {
                mainFormIsMinimized = value;
            }
        }

        public static int Month
        {
            get
            {
                return month;
            }

            set
            {
                month = value;
            }
        }

        public static SMParcelMast ParcelMast
        {
            get
            {
                if (parcelMast == null)
                {
                    parcelMast = SketchMgrRepo.SelectParcelMasterWithParcel(record, card);
                }
                return parcelMast;
            }

            set
            {
                parcelMast = value;
            }
        }

        public static SMParcel ParcelWorkingCopy
        {
            get
            {
                if (SketchSnapshots != null && SketchSnapshots.Count > 0)
                {
                    int lastIndex = (from p in SketchSnapshots select p.SnapShotIndex).Max();
                    parcelWorkingCopy = (from p in SketchSnapshots where p.SnapShotIndex == lastIndex select p).FirstOrDefault<SMParcel>();
                }
                else
                {
                    parcelWorkingCopy = ParcelMast.Parcel;
                    parcelWorkingCopy.SnapShotIndex = 0;
                    SketchSnapshots.Add(parcelWorkingCopy);
                }

                return parcelWorkingCopy;
            }

            set
            {
                parcelWorkingCopy = value;
            }
        }

        public static int Record
        {
            get
            {
                return record;
            }

            set
            {
                record = value;
            }
        }

        public static string ReOpenSection
        {
            get
            {
                return reOpenSection;
            }

            set
            {
                reOpenSection = value;
            }
        }

        public static string SketchFolder
        {
            get
            {
                return sketchFolder;
            }

            set
            {
                sketchFolder = value;
            }
        }

        public static Image SketchImage
        {
            get
            {
                return sketchImage;
            }

            set
            {
                sketchImage = value;
            }
        }

        public static SketchRepository SketchMgrRepo
        {
            get
            {
                if (sketchMgrRepo == null)
                {
                    sketchMgrRepo = new SketchRepository(IpAddress, "CAMRA2", "CAMRA2", LocalityPrefix);
                }
                return sketchMgrRepo;
            }

            set
            {
                sketchMgrRepo = value;
            }
        }

        public static List<SMParcel> SketchSnapshots
        {
            get
            {
                if (sketchSnapshots == null)
                {
                    sketchSnapshots = new List<SMParcel>();
                }
                return sketchSnapshots;
            }

            set
            {
                sketchSnapshots = value;
            }
        }

        public static SMParcel SMParcelFromData
        {
            get
            {
                currentSMParcel = SketchMgrRepo.SelectParcelMasterWithParcel(record, card).Parcel;
                return currentSMParcel;
            }

            set
            {
                currentSMParcel = value;
            }
        }

        private static string _selectedPath = String.Empty;
        private static string _selectedPicPath = String.Empty;
        private static string _selectedSktPath = String.Empty;
        private static CAMRA_Connection camraDbConn = null;
        private static int card = 0;
        private static int checker = 0;
        private static Image currentSketchImage;
        private static SMParcel currentSMParcel;
        private static DBAccessManager dbAccessMgr = null;
        private static decimal defaultScale;
        private static int fcCard = 0;
        private static string fcIpAddress = String.Empty;
        private static string fcLib = String.Empty;
        private static string fcLocalityPrefix = String.Empty;
        private static int fcRecord = 0;
        private static bool hasNewSketch = false;
        private static bool hasSketch = false;
        private static int initalCard = 0;
        private static int initalRecord = 0;
        private static string ipAddress = String.Empty;
        private static string localityDescription = string.Empty;
        private static string localityPreFix = String.Empty;
        private static string localLib = String.Empty;
        private static bool mainFormIsClosed = false;
        private static bool mainFormIsMinimized = false;
        private static int month;
        private static SMParcelMast parcelMast;
        private static SMParcel parcelWorkingCopy;
        private static int record = 0;
        private static string reOpenSection;
        private static string sketchFolder;
        private static Image sketchImage;
        private static SketchRepository sketchMgrRepo;
        private static List<SMParcel> sketchSnapshots;
    }
}