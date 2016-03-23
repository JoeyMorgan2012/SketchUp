using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class AdminDataCollection : List<AdminData>
	{
		public DBAccessManager _fox
		{
			get; set;
		}

		public SWallTech.CAMRA_Connection _conn
		{
			get; set;
		}

		public static string locality
		{
			get; set;
		}

		private AdminDataCollection()
		{
		}

		public bool IsAdminDataCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.AdminDataIsChanged)
						return true;
				}

				return false;
			}
		}

		public AdminDataCollection(SWallTech.CAMRA_Connection conn, string _lib, string _prefix)
		{
			_conn = conn;

			getAdminData(_lib, _prefix);
		}

		public void getAdminData(string lib, string prefix)
		{
			StringBuilder adminSql = new StringBuilder();
			adminSql.Append("select alocality,astate,adatadrive,acamfolder,acamprefix,auserid,ausrinit,agispath,agislayer, ");
			adminSql.Append(String.Format(" agisfield,acamfld,asidlayer from {0}.{1}admin ", lib, prefix));

			DataSet ds_admin = _conn.DBConnection.RunSelectStatement(adminSql.ToString());

			if (ds_admin.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in ds_admin.Tables[0].Rows)
				{
					var item = new AdminData()
					{
						Loc_Locality = row["alocality"].ToString().Trim(),
						Loc_State = row["astate"].ToString().Trim(),
						Loc_DataDrive = row["adatadrive"].ToString().Trim(),
						Loc_CamFolder = row["acamfolder"].ToString().Trim(),
						Loc_CamPreFix = row["acamprefix"].ToString().Trim(),
						Loc_UserId = row["auserid"].ToString().Trim(),
						Loc_UserInit = row["ausrinit"].ToString().Trim(),
						Loc_GisPath = row["agispath"].ToString().Trim(),
						Loc_GisLayer = row["agislayer"].ToString().Trim(),
						Loc_GisField = row["agisfield"].ToString().Trim(),
						Loc_CamField = row["acamfld"].ToString().Trim(),
						Loc_SidLayer = row["asidlayer"].ToString().Trim(),

						orig_Loc_Locality = row["alocality"].ToString().Trim(),
						orig_Loc_State = row["astate"].ToString().Trim(),
						orig_Loc_DataDrive = row["adatadrive"].ToString().Trim(),
						orig_Loc_CamFolder = row["acamfolder"].ToString().Trim(),
						orig_Loc_CamPreFix = row["acamprefix"].ToString().Trim(),
						orig_Loc_UserId = row["auserid"].ToString().Trim(),
						orig_Loc_UserInit = row["ausrinit"].ToString().Trim(),
						orig_Loc_GisPath = row["agispath"].ToString().Trim(),
						orig_Loc_GisLayer = row["agislayer"].ToString().Trim(),
						orig_Loc_GisField = row["agisfield"].ToString().Trim(),
						orig_Loc_CamField = row["acamfld"].ToString().Trim(),
						orig_Loc_SidLayer = row["asidlayer"].ToString().Trim(),
					};

					this.Add(item);
				}
			}
		}

		public int updatetoAdminData()
		{
			int updatedCount = 0;

			foreach (var item in this)
			{
				if (item.AdminDataIsChanged)
				{
				}
			}

			return updatedCount;
		}
	}
}