using System;
using System.Linq;
using System.Text;

namespace SketchUpReboot
{
    public static class GlobalValues
    {
        #region Properties

        public static string DescTableName
        {
            get
            {
                descTableName = string.Format("{0}.{1}DESC", Library, LocalityPrefix);
                return descTableName;
            }

            set
            {
                descTableName = value;
            }
        }

        public static string GasLogTableName
        {
            get
            {
                gasLogTableName = string.Format("{0}.{1}GASLG", Library, LocalityPrefix);
                return gasLogTableName;
            }

            set
            {
                gasLogTableName = value;
            }
        }

        public static string ImprovementTableName
        {
            get
            {
                improvementTableName = string.Format("{0}.{1}IMP", Library, LocalityPrefix);
                return improvementTableName;
            }

            set
            {
                improvementTableName = value;
            }
        }

        public static string InteriorImprovementTableName
        {
            get
            {
                interiorImprovementTableName = string.Format("{0}.{1}BIMP", Library, LocalityPrefix);
                return interiorImprovementTableName;
            }

            set
            {
                interiorImprovementTableName = value;
            }
        }

        public static string IpAddress
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.IPAddress))
                {
                    ipAddress = Properties.Settings.Default.IPAddress;
                }
                return ipAddress;
            }

            set
            {
                Properties.Settings.Default.IPAddress = value;
                ipAddress = value;
            }
        }

        public static string LandTableName
        {
            get
            {
                landTableName = string.Format("{0}.{1}LAND", Library, LocalityPrefix);
                return landTableName;
            }

            set
            {
                landTableName = value;
            }
        }

        public static string Library
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Library))
                {
                    library = Properties.Settings.Default.Library;
                }
                else
                {
                    library = "NATIVE";
                }
                return library;
            }

            set
            {
                Properties.Settings.Default.Library = value;
                library = value;
            }
        }

        public static string LineTableName
        {
            get
            {
                lineTableName = string.Format("{0}.{1}LINE", Library, LocalityPrefix);
                return lineTableName;
            }

            set
            {
                lineTableName = value;
            }
        }

        public static string LocalityName
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.LocalityName))
                {
                    localityName = Properties.Settings.Default.LocalityName;
                }
                return localityName;
            }

            set
            {
                Properties.Settings.Default.LocalityName = value;
                localityName = value;
            }
        }

        public static string LocalityPrefix
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Locality))
                {
                    localityPrefix = Properties.Settings.Default.Locality;
                }
                return localityPrefix.ToUpper();
            }

            set
            {
                Properties.Settings.Default.Locality = value;
                localityPrefix = value;
            }
        }

        public static string MasterTableName
        {
            get
            {
                masterTableName = string.Format("{0}.{1}MASTER", Library, LocalityPrefix);
                return masterTableName;
            }

            set
            {
                masterTableName = value;
            }
        }

        public static string MastTableName
        {
            get
            {
                mastTableName = string.Format("{0}.{1}MAST", Library, LocalityPrefix);
                return mastTableName;
            }

            set
            {
                mastTableName = value;
            }
        }

        public static string PassWord
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.Password))
                {
                    passWord = Properties.Settings.Default.Password;
                }
                else
                {
                    passWord = "CAMRA2";
                }

                return passWord;
            }

            set
            {
                Properties.Settings.Default.Password = value;
                passWord = value;
            }
        }

        public static string Rate1TableName
        {
            get
            {
                rate1TableName = string.Format("{0}.{1}RAT1", Library, LocalityPrefix);
                return rate1TableName;
            }

            set
            {
                rate1TableName = value;
            }
        }

        public static string Rate2TableName
        {
            get
            {
                rate2TableName = string.Format("{0}.{1}RAT2", Library, LocalityPrefix);
                return rate2TableName;
            }

            set
            {
                rate2TableName = value;
            }
        }

        public static string SectionTableName
        {
            get
            {
                sectionTableName = string.Format("{0}.{1}SECTION", Library, LocalityPrefix);
                return sectionTableName;
            }

            set
            {
                sectionTableName = value;
            }
        }

        public static string StabTableName
        {
            get
            {
                stabTableName = string.Format("{0}.{1}STAB", Library, LocalityPrefix);
                return stabTableName;
            }

            set
            {
                stabTableName = value;
            }
        }

        public static string SysTableName
        {
            get
            {
                sysTableName = string.Format("{0}.{1}SYS", Library, LocalityPrefix);
                return sysTableName;
            }

            set
            {
                sysTableName = value;
            }
        }

        public static string UserName
        {
            get
            {
                if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
                {
                    userName = Properties.Settings.Default.UserName;
                }
                else
                {
                    userName = "CAMRA2";
                }

                return userName;
            }

            set
            {
                Properties.Settings.Default.UserName = value;
                userName = value;
            }
        }

        #endregion Properties

        #region property-backing fields

        private static string descTableName;
        private static string gasLogTableName;
        private static string improvementTableName;
        private static string interiorImprovementTableName;
        private static string ipAddress;
        private static string landTableName;
        private static string library;
        private static string lineTableName;
        private static string localityName;
        private static string localityPrefix;
        private static string masterTableName;
        private static string mastTableName;
        private static string passWord;
        private static string rate1TableName;
        private static string rate2TableName;
        private static string sectionTableName;
        private static string stabTableName;
        private static string sysTableName;
        private static string userName;

        #endregion property-backing fields
    }
}