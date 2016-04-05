using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class ValuesTrackingCollection : List<ValuesTracking>
	{
		private DBAccessManager _VTdb = null;
		private SWallTech.CAMRA_Connection _conn = null;
		private DataTable _VTTable = new DataTable("ValuesTrackingTable");

		private ValuesTrackingCollection()
		{
		}

		public DataTable ValuesTrackingTable
		{
			get
			{
				return _VTTable;
			}
		}

		public DataTable ValuesTrackingShortList
		{
			get
			{
				DataTable svt = _VTTable.Clone();
				svt.Columns.Remove("Record");
				svt.Columns.Remove("Card");
				svt.Columns.Remove("SeqNo");
				svt.Columns.Remove("UserID");
				svt.Columns.Remove("ChangeDate");

				foreach (DataRow shrow in _VTTable.Rows)
				{
					DataRow row = svt.NewRow();
					row["Year"] = shrow["Year"];
					row["LandVal"] = shrow["LandVal"];
					row["ImprVal"] = shrow["ImprVal"];
					row["TotalVal"] = shrow["TotalVal"];
					row["Reason"] = shrow["Reason"];

					svt.Rows.Add(row);
				}

				return svt;
			}
		}

		public ValuesTrackingCollection(SWallTech.CAMRA_Connection conn)
			: this()
		{
			_VTdb = conn.DBConnection;
			_conn = conn;

			_VTTable = new DataTable("ValuesTrackingTable");
			_VTTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("Card", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("SeqNo", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("Year", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("LandVal", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("ImprVal", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("TotalVal", typeof(int)));
			_VTTable.Columns.Add(new DataColumn("Reason", typeof(string)));
			_VTTable.Columns.Add(new DataColumn("UserID", typeof(string)));
			_VTTable.Columns.Add(new DataColumn("ChangeDate", typeof(string)));
		}

		public DataTable GetValuesTracking(string prefix, int recno, int card)
		{
			_VTTable.Rows.Clear();
			this.Clear();

			StringBuilder vtSql = new StringBuilder();
			vtSql.Append("select vseqno,vland,vimpr,vtotal,vreasn,vusrid,vchgmo,vchgda,vchgyr,veftxy ");
			vtSql.Append(String.Format("from {0}.{1}valu ", MainForm.localLib, MainForm.localPrefix));
			vtSql.Append(String.Format("where vrecno = {0} and vdwell = {1} ", recno, card));
			vtSql.Append("order by vseqno desc ");

			DataSet ds = _VTdb.RunSelectStatement(vtSql.ToString());

			foreach (DataRow VTreader in ds.Tables[0].Rows)
			{
				DataRow row = _VTTable.NewRow();
				row["Record"] = recno;
				row["Card"] = card;
				row["SeqNo"] = Convert.ToInt32(VTreader["vseqno"].ToString());
				row["Year"] = Convert.ToInt32(VTreader["veftxy"].ToString());
				row["LandVal"] = Convert.ToInt32(VTreader["vland"].ToString());
				row["ImprVal"] = Convert.ToInt32(VTreader["vimpr"].ToString());
				row["TotalVal"] = Convert.ToInt32(VTreader["vtotal"].ToString());
				row["Reason"] = VTreader["vreasn"].ToString().Trim();
				row["UserID"] = VTreader["vusrid"].ToString().Trim();
				row["ChangeDate"] = String.Format("{0}/{1}/{2}",
							VTreader["vchgmo"].ToString().Trim(),
							VTreader["vchgda"].ToString().Trim(),
							VTreader["vchgyr"].ToString().Trim());

				_VTTable.Rows.Add(row);

				var valHist = new ValuesTracking()
				{
					Record = recno,
					Card = card,
					SeqNo = Convert.ToInt32(VTreader["vseqno"].ToString()),
					VLand = Convert.ToInt32(VTreader["vland"].ToString()),
					VImpr = Convert.ToInt32(VTreader["vimpr"].ToString()),
					VTotal = Convert.ToInt32(VTreader["vtotal"].ToString()),
					VReason = VTreader["vreasn"].ToString().Trim(),
					VUser = VTreader["vusrid"].ToString().Trim(),
					VChgDate = String.Format("{0}/{1}/{2}",
							VTreader["vchgmo"].ToString().Trim(),
							VTreader["vchgda"].ToString().Trim(),
							VTreader["vchgyr"].ToString().Trim()),
					VEftxy = Convert.ToInt32(VTreader["veftxy"].ToString())
				};
			}

			return _VTTable;
		}

		public ValuesTracking GetValuesTracking(int recno, int card, int seqno)
		{
			var q = (from p in this
					 where p.Record == recno && p.Card == card && p.SeqNo == seqno
					 select p).SingleOrDefault();

			return q;
		}
	}
}