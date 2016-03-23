using System;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class SalesHistory
	{
		public event EventHandler<SalesHistoryEventsArgs> SalesHistoryChangedEvent;

		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		public int SeqNo
		{
			get; set;
		}

		public string Grantor
		{
			get; set;
		}

		public int SalePrice
		{
			get; set;
		}

		public string DateSold
		{
			get; set;
		}

		public int MonthSold
		{
			get; set;
		}

		public int YearSold
		{
			get; set;
		}

		public string Deed_Page
		{
			get; set;
		}

		public string DeedBook
		{
			get; set;
		}

		public int DeedPage
		{
			get; set;
		}

		public string Inst_Nbr
		{
			get; set;
		}

		public int InstYear
		{
			get; set;
		}

		public int InstNumber
		{
			get; set;
		}

		public SalesHistory()
		{
		}

		public static SalesHistory GetSalesHistory(SWallTech.CAMRA_Connection conn, string prefix, int recno, int card, int seqno)
		{
			var db = conn.DBConnection;

			SalesHistory SalesHistoryList = null;

			StringBuilder shsql = new StringBuilder();
			shsql.Append("select fseqno,flnam,fsellp,fyrsld,fdbook,fdpage,fintyp,fintyr,finno# as finno ");
			shsql.Append(String.Format(" from {0}.{1}trnf ", MainForm.localLib, MainForm.localPreFix));
			shsql.Append(String.Format(" where frecno = {0} and fdwell = {1} and fseqno = {2} ",
				recno,
				card,
				seqno));

			//shsql.Append(" and fsellp > 0 ");
			shsql.Append(" order by fseqno desc ");

			DataSet ds = db.RunSelectStatement(shsql.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow SHreader = ds.Tables[0].Rows[0];

				int month = 0;
				int year = 0;
				string instType = String.Empty;
				string instYear = String.Empty;
				string instNbr = String.Empty;
				string deedPage = String.Empty;
				string dateSold = String.Empty;
				string monthstr = String.Empty;
				string yearstr = String.Empty;
				string totalyearstr = Convert.ToString(SHreader["fyrsld"].ToString().Trim());
				if (totalyearstr.Length == 7)
				{
					monthstr = totalyearstr.Substring(0, 1);
					yearstr = totalyearstr.Substring(3, 4);
					month = Convert.ToInt32(monthstr);
					year = Convert.ToInt32(yearstr);
				}
				else if (totalyearstr.Length == 8)
				{
					monthstr = totalyearstr.Substring(0, 2);
					yearstr = totalyearstr.Substring(4, 4);
					month = Convert.ToInt32(monthstr);
					year = Convert.ToInt32(yearstr);
				}

				dateSold = String.Format("{0} / {1}",
					monthstr,
					yearstr);

				if (Convert.ToInt32(SHreader["fdpage"].ToString()) > 0)
				{
					deedPage = String.Format(" DB: {0} / {1}",
						SHreader["fdbook"].ToString().Trim(),
						Convert.ToInt32(SHreader["fdpage"].ToString().Trim()));
				}
				else
				{
					deedPage = "";
				}

				if (Convert.ToInt32(SHreader["finno"].ToString()) > 0)
				{
					instType = SHreader["fintyp"].ToString();
					instYear = Convert.ToString(year);

					instNbr = String.Format("Ins: {0} {1} - {2}",
							instType,
							instYear.Substring(2, 2).Trim(),
							Convert.ToInt32(SHreader["finno"].ToString().Trim()));
				}
				else
				{
					instNbr = "";
				}

				SalesHistoryList = new SalesHistory()
				{
					Record = recno,
					Card = card,
					SeqNo = seqno,
					Grantor = SHreader["flnam"].ToString().Trim(),
					SalePrice = Convert.ToInt32(SHreader["fsellp"].ToString()),
					DateSold = dateSold.ToString().Trim(),
					MonthSold = month,
					YearSold = year,
					Deed_Page = deedPage.ToString().Trim(),
					DeedBook = SHreader["fdbook"].ToString().Trim(),
					DeedPage = Convert.ToInt32(SHreader["fdpage"].ToString().Trim()),
					Inst_Nbr = instNbr.ToString().Trim(),
					InstYear = year,

					//InstYear = Convert.ToInt32(SHreader["fintyr"].ToString().Trim()),
					InstNumber = Convert.ToInt32(SHreader["finno"].ToString().Trim())
				};
			}

			return SalesHistoryList;
		}

		private void FireChangedEvent(string SalesHistoryfile)
		{
			if (SalesHistoryChangedEvent != null)
			{
				SalesHistoryChangedEvent(this,
					new SalesHistoryEventsArgs()
					{
						SalesHistoryList = SalesHistoryfile
					});
			}
		}
	}

	public class SalesHistoryEventsArgs : EventArgs
	{
		public SalesHistoryEventsArgs()
			: base()
		{
		}

		public string SalesHistoryList
		{
			get; set;
		}
	}
}