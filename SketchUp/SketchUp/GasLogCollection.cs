using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SketchUp
{
	public class GasLogCollection : List<GasLogData>
	{
		private SWallTech.CAMRA_Connection _fox = null;
		private DataTable _GLTable = new DataTable("GasLogTable");

		public SWallTech.CAMRA_Connection DatabaseConnection
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

		public int NbrGasFP
		{
			get; set;
		}

		public int orig_NbrGasFP
		{
			get; set;
		}

		private GasLogCollection()
		{
		}

		public bool IsGasLogCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.GasFPIsChanged)
						return true;
				}

				return false;
			}
		}

		public DataTable GasLogTable
		{
			get
			{
				return _GLTable;
			}
		}

		public DataTable GLTable
		{
			get
			{
				DataTable gltb = _GLTable.Clone();
				foreach (DataRow irow in _GLTable.Rows)
				{
					DataRow row = gltb.NewRow();
					row["Record"] = irow["Record"];
					row["Card"] = irow["Card"];
					row["NbrGasLogFP"] = irow["NbrGasLogFP"];
				}
				return gltb;
			}
		}

		//public GasLogCollection(DBAccessManager fox)
		//    : this()
		//{
		//    _fox = fox;

		//    _GLTable = new DataTable("GasLogTable");
		//    _GLTable.Columns.Add(new DataColumn("Record", typeof(int)));
		//    _GLTable.Columns.Add(new DataColumn("Card", typeof(int)));
		//    _GLTable.Columns.Add(new DataColumn("NbrGasLogFP", typeof(int)));

		//}

		public GasLogCollection(SWallTech.CAMRA_Connection fox, int record, int cardnum)
		{
			_fox = fox;
			DatabaseConnection = fox;
			Record = record;
			Card = cardnum;

			GetGasLog(fox, record, cardnum);
		}

		public DataTable GetGasLog(SWallTech.CAMRA_Connection fox, int record, int card)
		{
			_fox = fox;
			_GLTable.Rows.Clear();
			this.Clear();
			StringBuilder glsql = new StringBuilder();
			glsql.Append("select grecno,gdwell,gnogas ");
			glsql.Append(String.Format(" from {0}.{1}gaslg where grecno = {2} and gdwell = {3} ", MainForm.localLib, MainForm.localPreFix, record, card));

			DataSet ds = _fox.DBConnection.RunSelectStatement(glsql.ToString());
			foreach (DataRow ireader in ds.Tables[0].Rows)
			{
				//GasLogData gaslogdata = GasLogData.GetGasLog(_fox, record, card);
				//if (gaslogdata != null)
				//{
				//    //DataRow row = _GLTable.NewRow();
				//    //row["Record"] = Convert.ToInt32(ireader["grecno"].ToString());
				//    //row["Card"] = Convert.ToInt32(ireader["gdwell"].ToString());
				//    //row["NbrGasLogFP"] = Convert.ToInt32(ireader["gnogas"].ToString());

				//    //_GLTable.Rows.Add(row);
				//    //this.Add(gaslogdata);
				//}

				var gasfp = GasLogData.CreateNewGasLog(record, card, false);

				gasfp.Record = record;
				gasfp.Card = card;
				gasfp.NbrGasFP = Convert.ToInt32(ireader["gnogas"].ToString());
				gasfp.SetOriginalValues();

				this.Add(gasfp);
			}

			return _GLTable;
		}

		public GasLogData GetGasLog(int recno, int card)
		{
			var q = (from p in this
					 where p.Record == recno && p.Card == card
					 select p).SingleOrDefault();

			return q;
		}

		public int updatetoLandlData()
		{
			int updatedCount = 0;

			// int itemUpdateCount = 0;

			foreach (var item in this)
			{
				if (item.GasFPIsChanged)
				{
				}
			}

			return updatedCount;
		}
	}
}