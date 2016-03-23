using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class InteriorImprovementCollection : List<InteriorImprovement>
	{
		private SWallTech.CAMRA_Connection fox = null;
		private DataTable _IITable = new DataTable("InteriorImprovementTable");

		private InteriorImprovementCollection()
		{
		}

		public DataTable InteriorImprovementTable
		{
			get
			{
				return _IITable;
			}
		}

		public InteriorImprovementCollection(SWallTech.CAMRA_Connection _fox)
		{
			fox = _fox;

			_IITable = new DataTable("InteriorImprovementTable");
			_IITable.Columns.Add(new DataColumn("Record", typeof(int)));
			_IITable.Columns.Add(new DataColumn("Card", typeof(int)));
			_IITable.Columns.Add(new DataColumn("SeqNo", typeof(int)));
			_IITable.Columns.Add(new DataColumn("Description", typeof(string)));
			_IITable.Columns.Add(new DataColumn("Quantity", typeof(int)));
			_IITable.Columns.Add(new DataColumn("UnitPrice", typeof(int)));
			_IITable.Columns.Add(new DataColumn("UnitTotal", typeof(int)));
		}

		public DataTable GetImprovement(SWallTech.CAMRA_Connection _fox, int record, int card)
		{
			_IITable.Rows.Clear();
			StringBuilder iisql = new StringBuilder();
			iisql.Append("select urecno,udwell,useqno,udesc,uqty,uprice,utotal ");
			iisql.Append(String.Format("from {0}.{1}bimp where urecno = {2} and udwell = {3} ", MainForm.localLib, MainForm.localPreFix, record, card));
			iisql.Append(" and utotal > 0 ");

			DataSet ds = _fox.DBConnection.RunSelectStatement(iisql.ToString());

			foreach (DataRow IIreader in ds.Tables[0].Rows)
			{
				int seqno = Convert.ToInt32(IIreader["useqno"].ToString());
				InteriorImprovement IIdata = InteriorImprovement.GetImprovement(_fox, record, card, seqno);
				if (IIdata != null)
				{
					DataRow row = _IITable.NewRow();
					row["Record"] = record;
					row["Card"] = card;
					row["SeqNo"] = seqno;
					row["Description"] = IIdata.Decription;
					row["Quantity"] = IIdata.Quantity;
					row["UnitPrice"] = IIdata.UnitPrice;
					row["UnitTotal"] = IIdata.UnitTotal;

					_IITable.Rows.Add(row);

					this.Add(IIdata);
				}
			}

			return _IITable;
		}
	}
}