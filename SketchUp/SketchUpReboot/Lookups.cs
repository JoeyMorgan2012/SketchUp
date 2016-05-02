using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SketchUpReboot
{
    public static class Lookups
    {
        #region Fields
        static bool refreshLookups;
        static List<InstalledLocalityRecord> installedLocalities;
        #endregion

        #region Properties
        public static List<InstalledLocalityRecord> InstalledLocalities
        {
            get
            {
                if (installedLocalities==null||RefreshLookups)
                {
                    installedLocalities = new List<InstalledLocalityRecord>();
                    LookupsDalRepo ldr = new LookupsDalRepo();
                    string dataSource = GlobalValues.IpAddress;
                    string userName = GlobalValues.UserName;
                    string pw = GlobalValues.PassWord;
                    installedLocalities = ldr.InstalledLocalitiesList(dataSource, userName, pw);
                }
                return installedLocalities;
            }

            set
            {
                installedLocalities = value;
            }
        }

        public static bool RefreshLookups
        {
            get
            {

                return refreshLookups;
            }

            set
            {
                refreshLookups = value;
            }
        } 
        #endregion
    }
}
