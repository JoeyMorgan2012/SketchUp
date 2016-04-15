using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
    public class SalesHistoryCollection : List<SalesHistory>
    {
        private DBAccessManager _SHdb = null;
        private SWallTech.CAMRA_Connection _conn = null;
        private DataTable _SHTable = new DataTable("SalesHistoryTable");

        private SalesHistoryCollection()
        {
        }

        public DataTable SalesHistoryTable
        {
            get
            {
                return _SHTable;
            }
        }

        public DataTable SalesHistoryList
        {
            get
            {
                DataTable shb = _SHTable.Clone();
                shb.Columns.Remove("Record");
                shb.Columns.Remove("Card");
                shb.Columns.Remove("SeqNo");
                shb.Columns.Remove("MonthSold");
                shb.Columns.Remove("YearSold");
                shb.Columns.Remove("DeedBook");
                shb.Columns.Remove("DeedPage");
                shb.Columns.Remove("InstYear");
                shb.Columns.Remove("InstNumber");

                foreach (DataRow shrow in _SHTable.Rows)
                {
                    DataRow row = shb.NewRow();

                    row["Grantor"] = shrow["Grantor"];
                    row["SalesPrice"] = shrow["SalesPrice"];
                    row["DateSold"] = shrow["DateSold"];
                    row["Deed_Page"] = shrow["Deed_Page"];
                    row["Inst_Nbr"] = shrow["Inst_Nbr"];

                    shb.Rows.Add(row);
                }
                return shb;
            }
        }

        public SalesHistoryCollection(SWallTech.CAMRA_Connection conn)
            : this()
        {
            _SHdb = conn.DBConnection;
            _conn = conn;

            _SHTable = new DataTable("SalesHistoryTable");
            _SHTable.Columns.Add(new DataColumn("Record", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("Card", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("SeqNo", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("Grantor", typeof(string)));
            _SHTable.Columns.Add(new DataColumn("SalesPrice", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("DateSold", typeof(string)));
            _SHTable.Columns.Add(new DataColumn("MonthSold", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("YearSold", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("Deed_Page", typeof(string)));
            _SHTable.Columns.Add(new DataColumn("DeedBook", typeof(string)));
            _SHTable.Columns.Add(new DataColumn("DeedPage", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("Inst_Nbr", typeof(string)));
            _SHTable.Columns.Add(new DataColumn("InstYear", typeof(int)));
            _SHTable.Columns.Add(new DataColumn("InstNumber", typeof(int)));
        }

        public DataTable GetSalesHistory(string prefix, int record, int card)
        {
            _SHTable.Rows.Clear();
            this.Clear();

            StringBuilder shsql = new StringBuilder();
            shsql.Append("select fseqno,flnam,fsellp,fyrsld,fdbook,fdpage,fintyp,fintyr,finno# as finno ");
            shsql.Append(String.Format(" from {0}.{1}trnf ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix));
            shsql.Append(String.Format(" where frecno = {0} and fdwell = {1} ",
                record,
                card));

            //shsql.Append(" and fsellp > 0 ");
            shsql.Append(" order by fseqno desc");

            DataSet ds = _SHdb.RunSelectStatement(shsql.ToString());

            foreach (DataRow SHreader in ds.Tables[0].Rows)
            {
                int month = 0;
                int year = 0;
                string instType = String.Empty;
                string instYear = String.Empty;
                string instNbr = String.Empty;
                string dateSold = String.Empty;
                string deedPage = String.Empty;
                string monthstr = String.Empty;
                string yearstr = String.Empty;
                string totalyearstr = String.Empty;

                if (Convert.ToInt32(SHreader["fyrsld"].ToString()) > 0)
                {
                    totalyearstr = SHreader["fyrsld"].ToString().Trim();
                }
                if (Convert.ToInt32(SHreader["fyrsld"].ToString()) == 0 && Convert.ToInt32(SHreader["fintyr"].ToString()) > 0)
                {
                    totalyearstr = SHreader["fintyr"].ToString().Trim();
                }

                if (totalyearstr.Length == 7)
                {
                    monthstr = totalyearstr.Substring(0, 1);
                    yearstr = totalyearstr.Substring(3, 4);
                    month = Convert.ToInt32(monthstr);
                    year = Convert.ToInt32(yearstr);
                }
                if (totalyearstr.Length == 8)
                {
                    monthstr = totalyearstr.Substring(0, 2);
                    yearstr = totalyearstr.Substring(4, 4);
                    month = Convert.ToInt32(monthstr);
                    year = Convert.ToInt32(yearstr);
                }
                if (totalyearstr.Length == 4)
                {
                    monthstr = String.Empty;
                    yearstr = totalyearstr;
                    month = 0;
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

                if (Convert.ToInt32(SHreader["finno"].ToString()) > 0 && year > 0)
                {
                    instType = SHreader["fintyp"].ToString();
                    instYear = year.ToString();
                    instNbr = String.Format("Ins: {0} {1} - {2}",
                            instType,
                            instYear.Substring(2, 2).Trim(),
                            Convert.ToInt32(SHreader["finno"].ToString().Trim()));
                }
                else
                {
                    instNbr = "";
                }

                int seqno = Convert.ToInt32(SHreader["fseqno"].ToString());

                DataRow row = _SHTable.NewRow();
                row["Record"] = record;
                row["Card"] = card;
                row["SeqNo"] = seqno;
                row["Grantor"] = SHreader["flnam"].ToString().Trim();
                row["SalesPrice"] = Convert.ToInt32(SHreader["fsellp"].ToString().Trim());
                row["DateSold"] = dateSold.ToString().Trim();
                row["MonthSold"] = month;
                row["YearSold"] = year;
                row["Deed_Page"] = deedPage.ToString().Trim();
                row["DeedBook"] = SHreader["fdbook"].ToString();
                row["DeedPage"] = Convert.ToInt32(SHreader["fdpage"].ToString().Trim());

                //row["InstYear"] = year;
                row["InstYear"] = Convert.ToInt32(SHreader["fintyr"].ToString().Trim());
                row["Inst_Nbr"] = instNbr.ToString().Trim();
                row["InstNumber"] = Convert.ToInt32(SHreader["finno"].ToString().Trim());

                var sale = new SalesHistory()
                {
                    Record = record,
                    Card = card,
                    SeqNo = seqno,
                    Grantor = SHreader["flnam"].ToString().Trim(),
                    SalePrice = Convert.ToInt32(SHreader["fsellp"].ToString().Trim()),
                    DateSold = dateSold.ToString().Trim(),
                    MonthSold = month,
                    YearSold = year,
                    Deed_Page = deedPage.ToString().Trim(),
                    DeedBook = SHreader["fdbook"].ToString(),
                    DeedPage = Convert.ToInt32(SHreader["fdpage"].ToString().Trim()),
                    Inst_Nbr = instNbr.ToString().Trim(),

                    //InstYear = year,
                    InstYear = Convert.ToInt32(SHreader["fintyr"].ToString().Trim()),
                    InstNumber = Convert.ToInt32(SHreader["finno"].ToString().Trim())
                };

                //if (CamraSupport.SaleYearCutOff <= year)
                //{
                _SHTable.Rows.Add(row);

                this.Add(sale);

                //}
            }

            return _SHTable;
        }

        public SalesHistory GetSalesHistory(int recno, int card, int seqno)
        {
            var q = (from p in this
                     where p.Record == recno && p.Card == card && p.SeqNo == seqno
                     select p).SingleOrDefault();

            return q;
        }
    }
}