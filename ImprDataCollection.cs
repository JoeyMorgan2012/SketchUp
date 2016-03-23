using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class ImprDataCollection : List<ImprData>
	{
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

		private ImprDataCollection()
		{
		}

		public bool IsImprCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.ImprisChanged)
						return true;
				}

				return false;
			}
		}

		public ImprDataCollection(SWallTech.CAMRA_Connection fox, int record, int cardnum)
		{
			Record = record;
			Card = cardnum;
			_fox = fox;

			getImpr();
		}

		public void getImpr()
		{
			StringBuilder subImpr = new StringBuilder();
			subImpr.Append(" select iid,irecno,idwell,iseqno,idesc,ilen,iwid,icond,ifill1,itotv,idepr,irate,ifill2,icode,ifill3 ");
			subImpr.Append(String.Format(" from {0}.{1}impr where irecno = {2} and idwell = {3} ", MainForm.localLib, MainForm.localPreFix, Record, Card));
			subImpr.Append("and idesc <> ' ' order by iseqno");

			DataSet Impr = _fox.DBConnection.RunSelectStatement(subImpr.ToString());

			if (Impr.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow row in Impr.Tables[0].Rows)
				{
					var impr = new ImprData()
					{
						iid = row["iid"].ToString().Trim(),

						//lid = Impr.Tables[0].Rows[i]["lid"].ToString(),
						irecno = Record,
						idwell = Card,
						iseqno = Convert.ToInt32(row["iseqno"].ToString()),
						idesc = row["idesc"].ToString().Trim(),
						ilen = Convert.ToDecimal(row["ilen"].ToString()),
						iwid = Convert.ToDecimal(row["iwid"].ToString()),
						icond = row["icond"].ToString().Trim(),
						ifill1 = row["ifill1"].ToString().Trim(),
						itotv = Convert.ToInt32(row["itotv"].ToString()),
						idepr = Convert.ToDecimal(row["idepr"].ToString()),
						irate = Convert.ToDecimal(row["irate"].ToString()),
						ifill2 = row["ifill2"].ToString().Trim(),
						icode = Convert.ToInt32(row["icode"].ToString()),
						ifill3 = row["ifill3"].ToString().Trim(),

						orig_iid = row["iid"].ToString().Trim(),
						orig_idesc = row["idesc"].ToString().Trim(),
						orig_ilen = Convert.ToDecimal(row["ilen"].ToString()),
						orig_iwid = Convert.ToDecimal(row["iwid"].ToString()),
						orig_icond = row["icond"].ToString().Trim(),
						orig_ifill1 = row["ifill1"].ToString().Trim(),
						orig_itotv = Convert.ToInt32(row["itotv"].ToString()),
						orig_idepr = Convert.ToDecimal(row["idepr"].ToString()),
						orig_irate = Convert.ToDecimal(row["irate"].ToString()),
						orig_ifill2 = row["ifill2"].ToString().Trim(),
						orig_icode = Convert.ToInt32(row["icode"].ToString()),
						orig_ifill3 = row["ifill3"].ToString().Trim()
					};

					this.Add(impr);
				}
			}
		}

		public int updatetoImprData()
		{
			int updatedCount = 0;

			// int itemUpdateCount = 0;

			foreach (var item in this)
			{
				if (item.ImprisChanged)
				{
				}
			}

			return updatedCount;
		}
	}
}