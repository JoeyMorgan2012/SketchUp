using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class ParcelNotesCollection : List<ParcelNotes>
	{
		private DBAccessManager _NTdb = null;
		private SWallTech.CAMRA_Connection _conn = null;
		private DataTable _NTTable = new DataTable("ParcelNotesTable");

		private ParcelNotesCollection()
		{
		}

		public DataTable ParcelNotesTable
		{
			get
			{
				return _NTTable;
			}
		}

		public ParcelNotesCollection(SWallTech.CAMRA_Connection conn)
			: this()
		{
			_NTdb = conn.DBConnection;
			_conn = conn;

			_NTTable = new DataTable("ParcelNotesTable");
			_NTTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_NTTable.Columns.Add(new DataColumn("Card", typeof(int)));
			_NTTable.Columns.Add(new DataColumn("SeqNo", typeof(int)));
			_NTTable.Columns.Add(new DataColumn("Line1", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line2", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line3", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line4", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line5", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line6", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line7", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line8", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line9", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line10", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line11", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line12", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line13", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line14", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line15", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line16", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line17", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line18", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line19", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line20", typeof(string)));
			_NTTable.Columns.Add(new DataColumn("Line21", typeof(string)));
		}

		public DataTable GetParcelNotes(string prefix, int recno, int card)
		{
			_NTTable.Rows.Clear();
			this.Clear();

			StringBuilder ntSql = new StringBuilder();
			ntSql.Append("select cseqno,clin1,clin2,clin3,clin4,clin5,clin6,clin7,clin8,clin9,clin10, ");
			ntSql.Append("clin11,clin12,clin13,clin14,clin15,clin16,clin17,clin18,clin19,clin20,clin21 ");
			ntSql.Append(String.Format("from {0}.{1}note where crecno = {2} and cdwell = {3}  ",
				SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, recno, card));
			ntSql.Append("order by cseqno ");

			DataSet ds = _NTdb.RunSelectStatement(ntSql.ToString());

			foreach (DataRow NTreader in ds.Tables[0].Rows)
			{
				int seqno = Convert.ToInt32(NTreader["cseqno"].ToString());
				ParcelNotes NTdata = ParcelNotes.GetNotes(_conn, prefix, recno, card, seqno);

				if (NTdata != null)
				{
					DataRow row = _NTTable.NewRow();
					row["Record"] = recno;
					row["Card"] = card;
					row["SeqNo"] = Convert.ToInt32(NTreader["cseqno"].ToString());
					row["Line1"] = NTreader["clin1"].ToString().Trim();
					row["Line2"] = NTreader["clin2"].ToString().Trim();
					row["Line3"] = NTreader["clin3"].ToString().Trim();
					row["Line4"] = NTreader["clin4"].ToString().Trim();
					row["Line5"] = NTreader["clin5"].ToString().Trim();
					row["Line6"] = NTreader["clin6"].ToString().Trim();
					row["Line7"] = NTreader["clin7"].ToString().Trim();
					row["Line8"] = NTreader["clin8"].ToString().Trim();
					row["Line9"] = NTreader["clin9"].ToString().Trim();
					row["Line10"] = NTreader["clin10"].ToString().Trim();
					row["Line11"] = NTreader["clin11"].ToString().Trim();
					row["Line12"] = NTreader["clin12"].ToString().Trim();
					row["Line13"] = NTreader["clin13"].ToString().Trim();
					row["Line14"] = NTreader["clin14"].ToString().Trim();
					row["Line15"] = NTreader["clin15"].ToString().Trim();
					row["Line16"] = NTreader["clin16"].ToString().Trim();
					row["Line17"] = NTreader["clin17"].ToString().Trim();
					row["Line18"] = NTreader["clin18"].ToString().Trim();
					row["Line19"] = NTreader["clin19"].ToString().Trim();
					row["Line20"] = NTreader["clin20"].ToString().Trim();
					row["Line21"] = NTreader["clin21"].ToString().Trim();

					_NTTable.Rows.Add(row);

					this.Add(NTdata);
				}
			}

			return _NTTable;
		}

		public ParcelNotes GetNotes(int recno, int card, int seqno)
		{
			var q = (from p in this
					 where p.Record == recno && p.Card == card && p.SeqNo == seqno
					 select p).SingleOrDefault();

			return q;
		}
	}
}