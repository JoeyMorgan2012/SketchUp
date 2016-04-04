using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SketchUp
{
	public class LandDataCollection : List<LandData>
	{
		private ParcelData _currentParcel = null;

		public SWallTech.CAMRA_Connection _fox
		{
			get; set;
		}

		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		public int Orig_Sum_Ltotal
		{
			get; set;
		}

		public int sum_SumLtotal
		{
			get; set;
		}

		private DataTable _LTable = new DataTable("LandTable");

		private LandDataCollection()
		{
		}

		public LandDataCollection(ParcelData data, SWallTech.CAMRA_Connection fox)
		{
			_currentParcel = data;
			_fox = fox;
		}

		public DataTable LandTable
		{
			get
			{
				return _LTable;
			}
		}

		public DataTable LandTableShort
		{
			get
			{
				DataTable tb = _LTable.Clone();
				tb.Columns.Remove("Record");
				tb.Columns.Remove("Card");
				tb.Columns.Remove("SeqNo");
				tb.Columns.Remove("TypeCode");
				tb.Columns.Remove("HSCode");
				tb.Columns.Remove("WaterDescription");
				tb.Columns.Remove("SewerDescription");
				tb.Columns.Remove("HSWater");
				tb.Columns.Remove("HSSewer");
				tb.Columns.Remove("UnitDescription");
				tb.Columns.Remove("AcreString");

				foreach (DataRow drow in _LTable.Rows)
				{
					DataRow row = tb.NewRow();

					row["TypeDesc"] = drow["TypeDesc"];
					row["UnitSize"] = drow["UnitSize"];
					row["UnitValue"] = drow["UnitValue"];
					row["PPCode"] = drow["PPCode"];
					row["UnitAdj"] = drow["UnitAdj"];
					row["UnitTotal"] = drow["UnitTotal"];
					row["HSUtility"] = drow["HSUtility"];
					tb.Rows.Add(row);
				}

				return tb;
			}
		}

		public DataTable LandTableExpand
		{
			get
			{
				DataTable etb = _LTable.Clone();
				etb.Columns.Remove("Record");
				etb.Columns.Remove("Card");

				foreach (DataRow drow in _LTable.Rows)
				{
					DataRow row = etb.NewRow();

					row["SeqNo"] = drow["SeqNo"];
					row["TypeCode"] = drow["TypeCode"];
					row["TypeDesc"] = drow["TypeDesc"];
					row["UnitSize"] = drow["UnitSize"];
					row["HSCode"] = drow["HSCode"];
					row["UnitValue"] = drow["UnitValue"];
					row["PPCode"] = drow["PPCode"];
					row["UnitAdj"] = drow["UnitAdj"];
					row["UnitTotal"] = drow["UnitTotal"];
					row["HSUtility"] = drow["HSUtility"];
					row["HSWater"] = drow["HSWater"];
					row["WaterDescription"] = drow["WaterDescription"];
					row["HSSewer"] = drow["HSSewer"];
					row["SewerDescription"] = drow["SewerDescription"];
					row["UnitDescription"] = drow["UnitDescription"];
					row["AcreString"] = drow["AcreString"];
					etb.Rows.Add(row);
				}

				return etb;
			}
		}

		public LandDataCollection(SWallTech.CAMRA_Connection fox)
			: this()
		{
			_LTable = new DataTable("LandTable");
			_LTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_LTable.Columns.Add(new DataColumn("Card", typeof(int)));
			_LTable.Columns.Add(new DataColumn("SeqNo", typeof(int)));
			_LTable.Columns.Add(new DataColumn("TypeCode", typeof(int)));
			_LTable.Columns.Add(new DataColumn("TypeDesc", typeof(string)));
			_LTable.Columns.Add(new DataColumn("UnitSize", typeof(decimal)));
			_LTable.Columns.Add(new DataColumn("HSCode", typeof(string)));
			_LTable.Columns.Add(new DataColumn("UnitValue", typeof(decimal)));
			_LTable.Columns.Add(new DataColumn("PPCode", typeof(string)));
			_LTable.Columns.Add(new DataColumn("UnitAdj", typeof(decimal)));
			_LTable.Columns.Add(new DataColumn("UnitTotal", typeof(int)));
			_LTable.Columns.Add(new DataColumn("HSUtility", typeof(int)));
			_LTable.Columns.Add(new DataColumn("HSWater", typeof(int)));
			_LTable.Columns.Add(new DataColumn("HSSewer", typeof(int)));
			_LTable.Columns.Add(new DataColumn("AcreString", typeof(string)));
		}

		public DataTable GetLand(SWallTech.CAMRA_Connection fox, int record, int card)
		{
			_LTable.Rows.Clear();
			this.Clear();
			StringBuilder lsql = new StringBuilder();
			lsql.Append("select lrecno,ldwell,lseqno,lhs,laccod ");
			lsql.Append(String.Format("from {0}.{1}land where lrecno = {2} and ldwell = {3} order by lseqno", MainForm.localLib, MainForm.localPrefix, record, card));

			DataSet ds = fox.DBConnection.RunSelectStatement(lsql.ToString());
			int wrkSeq = 0;
			string landType = null;
			foreach (DataRow lreader in ds.Tables[0].Rows)
			{
				int seqno = Convert.ToInt32(lreader["lseqno"].ToString());
				if (Convert.ToString(lreader["lhs"]) == "H")
				{
					landType = "Home Site";
				}
				if (Convert.ToString(lreader["lhs"]) != "H")
				{
					landType = CamraSupport.LandTypeCollection.LandDescription(Convert.ToInt32(lreader["laccod"]));
				}

				LandData ldata = LandData.getLand(fox, record, card, seqno);

				if (ldata != null)
				{
					DataRow row = _LTable.NewRow();
					row["Record"] = record;
					row["Card"] = card;
					row["SeqNo"] = ldata.lseqno;
					row["TypeCode"] = ldata.laccod;
					row["TypeDesc"] = ldata.ldescr;
					row["AcreString"] = ldata.lacre;
					row["HSCode"] = ldata.lhs;
					row["UnitSize"] = ldata.lacren;
					row["PPCode"] = ldata.llp;
					row["UnitValue"] = ldata.lvalue;
					row["UnitAdj"] = ldata.ladj;
					row["UnitTotal"] = ldata.ltotal;
					row["HSUtility"] = ldata.lutil;
					row["HSWater"] = ldata.lwater;
					row["HSSewer"] = ldata.lsewer;

					_LTable.Rows.Add(row);

					//this.Add(ldata);
					wrkSeq++;
				}
			}

			return _LTable;
		}

		public LandData GetLand(int recno, int card, int seqno)
		{
			var q = (from p in this
					 where p.lrecno == recno && p.ldwell == card && p.lseqno == seqno
					 select p).SingleOrDefault();

			return q;
		}

		public bool IsLandCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.LandisChanged)
						return true;
				}

				return false;
			}
		}

		public LandDataCollection(SWallTech.CAMRA_Connection fox, int record, int cardnum)
			: this()
		{
			_fox = fox;
			Record = record;
			Card = cardnum;

			GetLand(Record, Card);
		}

		public void GetLand(int _record, int _card)
		{
			StringBuilder subLand = new StringBuilder();
			subLand.Append(" select lid,lrecno,ldwell,lseqno,laccod,lhs,lacre,lvalue,llp,ladj,lacren,ltotal,ldescr,lwater,lsewer,lutil ");
			subLand.Append(String.Format(" from {0}.{1}land where lrecno = {2} and ldwell = {3} ", MainForm.localLib, MainForm.localPrefix, _record, _card));
			subLand.Append(" and laccod > 0 order by lseqno ");

			//subLand.Append(" order by lseqno ");

			DataSet Land = _fox.DBConnection.RunSelectStatement(subLand.ToString());

			foreach (DataRow row in Land.Tables[0].Rows)
			{
				int seqno = Convert.ToInt32(row["lseqno"].ToString());
				var land = LandData.getLand(_fox, _record, _card, seqno);

				land.lid = row["lid"].ToString().Trim();
				land.lseqno = Convert.ToInt32(row["lseqno"].ToString());
				land.laccod = Convert.ToInt32(row["laccod"].ToString());
				land.lhs = row["lhs"].ToString().Trim();
				land.lacre = row["lacre"].ToString().Trim();
				land.lvalue = Convert.ToInt32(row["lvalue"].ToString());
				land.llp = row["llp"].ToString().Trim();
				land.ladj = Convert.ToDecimal(row["ladj"].ToString());
				land.lacren = Convert.ToDecimal(row["lacren"].ToString());
				land.ltotal = Convert.ToInt32(row["ltotal"].ToString());
				land.ldescr = row["ldescr"].ToString().Trim();
				land.lwater = Convert.ToInt32(row["lwater"].ToString());
				land.lsewer = Convert.ToInt32(row["lsewer"].ToString());
				land.lutil = Convert.ToInt32(row["lutil"].ToString());
				land.SetOriginalValues();

				this.Add(land);
			}

			StringBuilder sum_land = new StringBuilder();
			sum_land.Append(String.Format("select sum(ltotal) from {0}.{1}land where lrecno = {2} and ldwell = {3} and laccod > 0 ", MainForm.localLib, MainForm.localPrefix, _record, _card));

			try
			{
				Orig_Sum_Ltotal = Convert.ToInt32(_fox.DBConnection.ExecuteScalar(sum_land.ToString()));
				sum_SumLtotal = Orig_Sum_Ltotal;
			}
			catch (InvalidCastException)
			{
				Orig_Sum_Ltotal = 0;
				sum_SumLtotal = 0;
			}
		}

		public int updatetoLandlData()
		{
			int updatedCount = 0;

			// int itemUpdateCount = 0;

			foreach (var item in this)
			{
				if (item.LandisChanged)
				{
				}
			}

			return updatedCount;
		}
	}
}