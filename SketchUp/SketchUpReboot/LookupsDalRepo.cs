using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUpReboot
{
    public class LookupsDalRepo
    {
        public List<InstalledLocalityRecord> InstalledLocalitiesList(string dataSource, string userName, string passWord)
        {
            List<InstalledLocalityRecord> locs = new List<InstalledLocalityRecord>();
            try
            {
             
                DBAccessManager dbAccessMgr = new DBAccessManager(DBAccessManager.DatabaseTypes.iSeriesDB2, dataSource, null, userName, passWord, null);
                string locSelectSql = "select PREFIX,DESC30,LIBRARY from  camfilib.cafpsysctl  order by DESC30";
                DbDataReader locsDr = (DbDataReader)dbAccessMgr.getDataReader(locSelectSql);
                if (locsDr.HasRows)
                {
                    while (locsDr.Read())
                    {

                        locs.Add(new InstalledLocalityRecord { Prefix = locsDr["PREFIX"].ToString().Trim(), Description = locsDr["DESC30"].ToString().Trim(), Library = locsDr["LIBRARY"].ToString().Trim() });
                    }
                }
                return locs;
            }
            catch (Exception ex)
            {
                string errMessage = string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message);
                Trace.WriteLine(errMessage);
                Debug.WriteLine(string.Format("Error occurred in {0}, in procedure {1}: {2}", MethodBase.GetCurrentMethod().Module, MethodBase.GetCurrentMethod().Name, ex.Message));
#if DEBUG

                MessageBox.Show(errMessage);
#endif

                throw;
            }
        }
    }
}

