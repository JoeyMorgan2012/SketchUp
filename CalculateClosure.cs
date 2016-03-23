using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.IO;
using SWallTech;

namespace SketchUp
{
    public partial class CalculateClosure
    {
	//	CAMRA_Connection _conn = null;
        ParcelData _currentParcel = null;

        public static int EWdist = 0;
        public static int NSdist = 0;


        public CalculateClosure()
        {
        }

        public static CalculateClosure getClosure(SWallTech.CAMRA_Connection conn, string section, int record, int dwell)
        {
            CalculateClosure _closure = null;

            var db = conn.DBConnection;

            EWdist = 0;
            NSdist = 0;

            return _closure;
        }
    }

     
}
