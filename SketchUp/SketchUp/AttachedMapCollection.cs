using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class AttachedMapCollection : List<string>
	{
		private AttachedMapCollection()
		{
		}

		public static AttachedMapCollection Factory(ParcelData parcel)
		{
			AttachedMapCollection attachedMap = new AttachedMapCollection();

			StringBuilder amsql = new StringBuilder();
			amsql.Append("select atmap# as attached ");
			amsql.Append(String.Format("from {0}.{1}amap ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
			amsql.Append(String.Format("where atpmap = '{0}' ", parcel.mmap));

			DataSet ds = parcel.Connection.DBConnection.RunSelectStatement(amsql.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds.Tables[0].Rows)
				{
					string att = row["attached"].ToString().TrimEnd(new char[] { ' ' });
					attachedMap.Add(att);
				}
			}
			return attachedMap;
		}
	}
}