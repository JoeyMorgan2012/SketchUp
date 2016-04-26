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

        public static ParcelData CurrentParcel
        {
            get
            {
                return currentParcel;
            }

            set
            {
                currentParcel = value;
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

        public static SMParcel SMParcelFromData
        {
            get
            {
                return currentSMParcel;
            }

            set
            {
                currentSMParcel = value;
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

        public static string LocalityPreFix
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
                    parcelWorkingCopy = SketchMgrRepo.SelectParcelAll(Record, Card);
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
                if (sketchMgrRepo==null)
                {
                    sketchMgrRepo = new SketchRepository(IpAddress, "CAMRA2", "CAMRA2", LocalityPreFix);
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

        public static SectionDataCollection SubSections
        {
            get
            {
                return _subSections;
            }

            set
            {
                _subSections = value;
            }
        }

        public static DateTime Today
        {
            get
            {
                return today;
            }

            set
            {
                today = value;
            }
        }

        public static int TodayDayNumber
        {
            get
            {
                return todayDayNumber;
            }

            set
            {
                todayDayNumber = value;
            }
        }

        public static int Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }

        private static CAMRA_Connection camraDbConn = null;
        private static int card = 0;
        private static int checker = 0;
        private static ParcelData currentParcel = null;
        private static Image currentSketchImage;
        private static SMParcel currentSMParcel;
        private static DBAccessManager dbAccessMgr = null;
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
        private static SMParcel parcelWorkingCopy;
        private static int record = 0;
        private static string reOpenSection;
        private static string sketchFolder;
        private static Image sketchImage;
        private static SketchRepository sketchMgrRepo;
        private static List<SMParcel> sketchSnapshots;

        private static DateTime today;
        private static int todayDayNumber;
        private static int year;
        private static string _selectedPath = String.Empty;
        private static string _selectedPicPath = String.Empty;
        private static string _selectedSktPath = String.Empty;
        private static SectionDataCollection _subSections = null;
    }
}