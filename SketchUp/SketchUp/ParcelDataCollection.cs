using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class ParcelDataCollection : List<ParcelData>
	{
		private CAMRA_Connection _db = null;

		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}

		private DataTable _MTable = new DataTable("MasterTable");

		public string LastQueryString
		{
			get; private set;
		}

		public int RecordMaximum
		{
			get; set;
		}

		public List<int> record
		{
			get; set;
		}

		private ParcelDataCollection()
		{
			RecordMaximum = 25;
		}

		public bool IsParcelCollectionChanged
		{
			get
			{
				foreach (var item in this)
				{
					if (item.ParcelIsChanged)
						return true;
				}

				return false;
			}
		}

		public ParcelDataCollection(SWallTech.CAMRA_Connection db, int record, int cardnum)
			: this()
		{
			_db = db;
			Record = record;
			Card = cardnum;

			_MTable = new DataTable("MasterTable");
			_MTable.Columns.Add(new DataColumn("Record", typeof(int)));
			_MTable.Columns.Add(new DataColumn("Card", typeof(int)));
			_MTable.Columns.Add(new DataColumn("Map", typeof(string)));
			_MTable.Columns.Add(new DataColumn("Name", typeof(string)));
			_MTable.Columns.Add(new DataColumn("911Add", typeof(string)));
			_MTable.Columns.Add(new DataColumn("Acres", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Percent_Change", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Occupancy", typeof(string)));
			_MTable.Columns.Add(new DataColumn("Year_Built", typeof(int)));
			_MTable.Columns.Add(new DataColumn("Class", typeof(string)));
			_MTable.Columns.Add(new DataColumn("Factor", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Depreciation", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("No_Depreciation", typeof(string)));
			_MTable.Columns.Add(new DataColumn("FinBrate", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Land_Value", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Sub_Total", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Total_BldgVal", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Other_Imp", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("Sales_Price", typeof(decimal)));
			_MTable.Columns.Add(new DataColumn("YrSold", typeof(Int32)));

			GetParcel(_db, Record, Card);
		}

		public DataTable GetData(List<SearchParameter> parms)
		{
			return this.GetData(parms, false);
		}

		public DataTable GetData(List<SearchParameter> parms, bool loadCollection)
		{
			if (parms.Count == 0)
			{
				throw new ArgumentException("Search parameters not provided.");
			}

			string prefix = _db.LocalityPrefix;
			string localLibrary = _db.Library;

			StringBuilder sqlcount = new StringBuilder();
			sqlcount.Append("select count(*) ");
			sqlcount.Append(String.Format(" from {0}.{1}mast ", MainForm.localLib, MainForm.localPrefix));
			sqlcount.Append(" where moccup < 99");
			StringBuilder whereClause = new StringBuilder();
			foreach (var p in parms)
			{
				whereClause.Append(String.Format(" and {0}", p.WhereClause));
			}

			int count = (int)_db.DBConnection.ExecuteScalar(sqlcount.ToString() + whereClause.ToString());
			if (count > RecordMaximum)
			{
				throw new RecordMaximumExceededException()
				{
					RecordMaximum = this.RecordMaximum,
					QueryCount = count
				};
			}

			StringBuilder sql = new StringBuilder();
			sql.AppendLine("select mrecno, mdwell, mmap, mlnam, macre# as acres, moccup, myrblt");
			sql.AppendLine(", mclass, mfactr, mdeprc, m0depr, mbrate, mtotld, mtsubt, mtotbv, mtotoi, mhse# as strtnbr");
			sql.AppendLine(", mdirct, mstrt, msttyp, cast(((mtotpr - (massl + massb)) / (massl + massb)) as numeric(6,3)) as percChange, myrsld, msellp ");
			sql.AppendLine(String.Format("from {0}.{1}mast", localLibrary, prefix));
			sql.AppendLine(" where moccup < 99");

			sql.Append(whereClause.ToString());

			sql.Append(" order by mmap, mdwell");

			DataTable returnTable = BuildTable(sql.ToString());

			if (loadCollection)
			{
				foreach (DataRow dr in returnTable.Rows)
				{
					int rec = (int)dr["Record"];
					int card = (int)dr["Card"];

					this.Add(GetParcel(_db, rec, card));
				}
			}

			return returnTable;
		}

		//public DataTable GetDataByBatch(string batchName)
		//{
		//    if (batchName.Length != 2)
		//    {
		//        throw new ArgumentException("Batch name cannot be blank.");
		//    }

		//    if (!_db.Localities.GetBatchList(_db.LocalityPrefix).Contains(batchName))
		//    {
		//        throw new ArgumentException("Invalid batch name.");
		//    }

		//    string prefix = _db.LocalityPrefix;

		//    StringBuilder sqlcount = new StringBuilder();
		//    sqlcount.Append("select count(*) from ");
		//    sqlcount.Append(String.Format("(select mrecno from {0}.{1}mast, {0}.{1}bt{2}", _db.Library, prefix, batchName));
		//    sqlcount.Append(" where mrecno = srecno and mdwell = sdwell ");
		//    sqlcount.Append(" and moccup < 99 ");
		//    sqlcount.Append(" group by mrecno, mdwell) query");
		//    int count = (int)_db.DBConnection.ExecuteScalar(sqlcount.ToString());
		//    if (count > RecordMaximum)
		//    {
		//        throw new RecordMaximumExceededException()
		//        {
		//            RecordMaximum = this.RecordMaximum,
		//            QueryCount = count
		//        };
		//    }

		//    StringBuilder sql = new StringBuilder();
		//    sql.AppendLine("select mrecno, mdwell, mmap, mlnam, macre# as acres, moccup, myrblt");
		//    sql.AppendLine(", mclass, mfactr, mdeprc, m0depr, mbrate, mtotld, mtsubt, mtotbv, mtotoi, mhse# as strtnbr");
		//    sql.AppendLine(", mdirct, mstrt, msttyp, cast(((mtotpr - (massl + massb)) / (massl + massb)) as numeric(6,3)) as percChange, myrsld, msellp ");
		//    sql.AppendLine(String.Format("from {0}.{1}mast, {0}.{1}bt{2}", _db.Library, prefix, batchName));
		//    sql.AppendLine(" where mrecno = srecno and mdwell = sdwell ");
		//    sql.AppendLine(" and moccup < 99 ");
		//    sql.AppendLine(" order by mmap, mdwell");

		//    return BuildTable(sql.ToString());

		//}

		private DataTable BuildTable(string sql)
		{
			_MTable.Rows.Clear();
			this.Clear();
			LastQueryString = sql;

			DataSet cmp = _db.DBConnection.RunSelectStatement(sql.ToString());

			if (cmp.Tables[0].Rows.Count > 0)
			{
				string PropAddress = String.Empty;
				for (int i = 0; i < cmp.Tables[0].Rows.Count; i++)
				{
					if (Convert.ToInt32(cmp.Tables[0].Rows[i]["mhse#"].ToString()) != 0)
					{
						PropAddress = String.Format("{0}  {1} {2} {3}",
							Convert.ToInt32(cmp.Tables[0].Rows[i]["mhse#"].ToString().Trim().PadLeft(5, ' ')),
							cmp.Tables[0].Rows[i]["mdirct"].ToString().Trim(),
							cmp.Tables[0].Rows[i]["mstrt"].ToString().Trim(),
							cmp.Tables[0].Rows[i]["msttyp"].ToString().Trim()
							);
					}
					else if (Convert.ToInt32(cmp.Tables[0].Rows[i]["mhse#"].ToString()) == 0)
					{
						PropAddress = String.Format("{0} {1} {2} ",
							cmp.Tables[0].Rows[i]["mdirct"].ToString().Trim(),
							cmp.Tables[0].Rows[i]["mstrt"].ToString().Trim(),
							cmp.Tables[0].Rows[i]["msttyp"].ToString().Trim()
							);
					}

					DataRow row = _MTable.NewRow();
					row["Record"] = Convert.ToInt32(cmp.Tables[0].Rows[i]["mrecno"].ToString());
					row["Card"] = Convert.ToInt32(cmp.Tables[0].Rows[i]["mdwell"].ToString());
					row["Map"] = cmp.Tables[0].Rows[i]["mmap"].ToString().TrimEnd(new char[] { ' ' });
					row["Name"] = cmp.Tables[0].Rows[i]["mlnam"].ToString().Trim();
					row["911Add"] = PropAddress;
					row["Acres"] = cmp.Tables[0].Rows[i]["macre#"].ToString();
					row["Percent_Change"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["percChange"].ToString());
					row["Occupancy"] = Convert.ToInt32(cmp.Tables[0].Rows[i]["moccup"].ToString());
					row["Year_Built"] = Convert.ToInt32(cmp.Tables[0].Rows[i]["myrblt"].ToString());
					row["Class"] = cmp.Tables[0].Rows[i]["mclass"].ToString();
					row["Factor"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mfactr"].ToString());
					row["Depreciation"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mdeprc"].ToString());
					row["No_Depreciation"] = cmp.Tables[0].Rows[i]["m0depr"].ToString();
					row["FinBrate"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mbrate"].ToString());
					row["Land_Value"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mtotld"].ToString());
					row["Sub_Total"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mtsubt"].ToString());
					row["Total_BldgVal"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mtotbv"].ToString());
					row["Other_Imp"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["mtotoi"].ToString());
					row["Sales_Price"] = Convert.ToDecimal(cmp.Tables[0].Rows[i]["msellp"].ToString());
					row["YrSold"] = Convert.ToInt32(cmp.Tables[0].Rows[i]["myrsld"].ToString());

					_MTable.Rows.Add(row);
				}

				if (_MTable.Rows.Count > 0)
				{
					record = new List<int>();
					for (int i = 0; i < _MTable.Rows.Count; i++)
					{
						int _batRec = Convert.ToInt32(_MTable.Rows[i]["Record"].ToString());
						int _batCard = Convert.ToInt32(_MTable.Rows[i]["Card"].ToString());

						if (_batCard == 1)
						{
							record.Add(_batRec);
						}
					}
				}
			}

			//using (IDataReader reader = _db.DBConnection.getDataReader(sql))
			//{
			//    int ord_Record = reader.GetOrdinal("mrecno");
			//    int ord_Dwell = reader.GetOrdinal("mdwell");
			//    int ord_Map = reader.GetOrdinal("mmap");
			//    int ord_Name = reader.GetOrdinal("mlnam");
			//    int ord_House = reader.GetOrdinal("strtnbr");
			//    int ord_Street = reader.GetOrdinal("mstrt");
			//    int ord_Dirct = reader.GetOrdinal("mdirct");
			//    int ord_Sttype = reader.GetOrdinal("msttyp");
			//    int ord_Acres = reader.GetOrdinal("acres");
			//    int ord_Percent_Change = reader.GetOrdinal("percChange");
			//    int ord_Occupancy = reader.GetOrdinal("moccup");
			//    int ord_Year_Built = reader.GetOrdinal("myrblt");
			//    int ord_Class = reader.GetOrdinal("mclass");
			//    int ord_Factor = reader.GetOrdinal("mfactr");
			//    int ord_Depreciation = reader.GetOrdinal("mdeprc");
			//    int ord_No_Depreciation = reader.GetOrdinal("m0depr");
			//    int ord_FinBrate = reader.GetOrdinal("mbrate");
			//    int ord_Land_Value = reader.GetOrdinal("mtotld");
			//    int ord_Sub_Total = reader.GetOrdinal("mtsubt");
			//    int ord_Total_BldgVal = reader.GetOrdinal("mtotbv");
			//    int ord_Other_Imp = reader.GetOrdinal("mtotoi");
			//    int ord_Sales_Price = reader.GetOrdinal("msellp");
			//    int ord_YrSold = reader.GetOrdinal("myrsld");

			//List<RecordCard> usedRecs = new List<RecordCard>();
			//while (reader.Read())
			//{
			//    int recno = reader.GetInt32(ord_Record);
			//    int card = reader.GetInt32(ord_Dwell);

			//    var q = from f in usedRecs
			//            where f.Record == recno && f.Card == card
			//            select f;

			//    string PropAddress = null;
			//    if (q.Count() == 0)
			//    {
			//        usedRecs.Add(new RecordCard() { Record = recno, Card = card });
			//        if (Convert.ToInt32(reader.GetInt32(ord_House)) != 0)
			//        {
			//            PropAddress = String.Format("{0}  {1} {2} {3}",
			//                Convert.ToInt32(reader.GetInt32(ord_House)).ToString().Trim().PadLeft(5, ' '),
			//                reader.GetString(ord_Dirct).ToString().Trim(),
			//                reader.GetString(ord_Street).ToString().Trim(),
			//                reader.GetString(ord_Sttype).ToString().Trim()
			//                );
			//        }
			//        else if (Convert.ToInt32(reader.GetInt32(ord_House)) == 0)
			//        {
			//            PropAddress = String.Format("{0} {1} {2} ",
			//                reader.GetString(ord_Dirct).ToString().Trim(),
			//                reader.GetString(ord_Street).ToString().Trim(),
			//                reader.GetString(ord_Sttype).ToString().Trim()
			//                );

			//        }

			//        DataRow row = _MTable.NewRow();
			//        row["Record"] = reader.GetInt32(ord_Record);
			//        row["Card"] = reader.GetInt32(ord_Dwell);
			//        row["Map"] = reader.GetString(ord_Map).TrimEnd(new char[] { ' ' });
			//        row["Name"] = reader.GetString(ord_Name).Trim();
			//        row["911Add"] = PropAddress;
			//        row["Acres"] = reader.GetDecimal(ord_Acres);
			//        row["Percent_Change"] = reader.GetDecimal(ord_Percent_Change);
			//        row["Occupancy"] = reader.GetInt32(ord_Occupancy);
			//        row["Year_Built"] = reader.GetInt32(ord_Year_Built);
			//        row["Class"] = reader.GetString(ord_Class).Trim();
			//        row["Factor"] = reader.GetDecimal(ord_Factor);
			//        row["Depreciation"] = reader.GetDecimal(ord_Depreciation);
			//        row["No_Depreciation"] = reader.GetString(ord_No_Depreciation).Trim();
			//        row["FinBrate"] = reader.GetDecimal(ord_FinBrate);
			//        row["Land_Value"] = reader.GetInt32(ord_Land_Value);
			//        row["Sub_Total"] = reader.GetInt32(ord_Sub_Total);
			//        row["Total_BldgVal"] = reader.GetInt32(ord_Total_BldgVal);
			//        row["Other_Imp"] = reader.GetInt32(ord_Other_Imp);
			//        row["Sales_Price"] = reader.GetInt32(ord_Sales_Price);
			//        row["YrSold"] = reader.GetInt32(ord_YrSold);

			//        _MTable.Rows.Add(row);

			//        }

			//    }

			//    if (_MTable.Rows.Count > 0)
			//    {
			//        record = new List<int>();
			//        for (int i = 0; i < _MTable.Rows.Count; i++)
			//        {
			//            int _batRec = Convert.ToInt32(_MTable.Rows[i]["Record"].ToString());
			//            int _batCard = Convert.ToInt32(_MTable.Rows[i]["Card"].ToString());

			//            if (_batCard == 1)
			//            {
			//                record.Add(_batRec);
			//            }
			//        }
			//    }
			//}

			return _MTable;
		}

		public void getParcel(SWallTech.CAMRA_Connection db, int record, int card)
		{
			CAMRA_Connection fox = db;
			Record = record;
			Card = card;

			StringBuilder subParcel = new StringBuilder();
			subParcel.Append(" select mrecid,mrecno,mdwell,mmap,mlnam,mfnam,madd1,madd2,mcity,mstate,mzip5,mzip4,macre,mzone,mluse,moccup,mstory,mage,mcond,mclass,mfactr, ");
			subParcel.Append(" mdeprc,mfound,mexwll,mrooft,mroofg,m#dunt,m#room,m#br,m#fbth,m#hbth,mfp2,mltrcd,mheat,mfuel,mac,mfp1,mcdr,mekit,mbastp,mpbtot,msbtot, ");
			subParcel.Append(" mpbfin,msbfin,mbrate,m#flue,mflutp,mgart,mgar#c,mcarpt,mcar#c,mbi#c,mrow,mease,mwater,msewer,mgas,melec,mterrn,mchar,motdes,mgart2,mgar#2, ");
			subParcel.Append(" mdatlg,mdatpr,mintyp,mintyr,minno#,minno2,mdsufx,mwsufx,mpsufx,mimprv,mtotld,mtotoi,mtotpr,massb,macpct,m1frnt,m1dpth,m1area,mmcode,m0depr, ");
			subParcel.Append(" m1um,m2frnt,m2dpth,m2area,mzipbr,mdelay,m2um,mstrt,mdirct,mhse#,mcdmo,mcdda,mcdyr,m1dfac,mrem1,mrem2,mmagcd,mathom,mdesc1,mdesc2,mdesc3,mdesc4, ");
			subParcel.Append(" mfairv,mlgity,mlgiyr,mlgno#,mlgno2,msubdv,msellp,m2dfac,minit,minspd,mswl,mtutil,mnbadj,massl,macsf,mcomm1,mcomm2,mcomm3,macct,mexwl2,mcalc, ");
			subParcel.Append(" mfill4,mtbv,mtbas,mtfbas,mtplum,mtheat,mtac,mtfp,mtfl,mtbi,mttadd,mtsubt,mtotbv,musrid,mbasa,mtota,mpsf,minwll,mfloor,myrblt,mcnst1,mcnst2,masslu, ");
			subParcel.Append(" mmosld,mdasld,myrsld,mtime,mhse#2,m1adj,m2adj,mlgbkc,mlgbk#,mlgpg#,meffag,mpcomp,msttyp,msdirs,m1rate,m2rate,mfuncd,mecond,mnbrhd,muser1,muser2, ");
			subParcel.Append(" mdbook,mdpage,mwbook,mwpage,mdcode,mwcode,mmortc,mfill7,macre#,mgispn,muser3,muser4,mimadj,mcdrdt,mmnud,mmnnud,mss1,mpcode,mpbook,mppage,mss2,massm, ");
			subParcel.Append(" mfill9,mgrntr,mcvmo,mcvda,mcvyr,mprout,mperr,mtbimp,mpuse,mcvexp,metxyr,mqapch,mqafil,mpict,meacre,mprcit,mprsta,mprzp1,mprzp4,mfp#,msfp#,mfl#, ");
			subParcel.Append(" msfl#,mmfl#,miofp#,mstor#,mascom,mhrph#,mhrdat,mhrtim,mhrnam,mhrses,mhidpc,mhidnm,mcamo,mcada,mcayr,moldoc ");
			subParcel.Append(String.Format(" from {0}.{1}mast where mrecno = {2} and mdwell = {3} and moccup < 30 ", MainForm.localLib, MainForm.localPrefix, Record, Card));

			DataSet Parcel = db.DBConnection.RunSelectStatement(subParcel.ToString());

			foreach (DataRow row in Parcel.Tables[0].Rows)
			{
				var parcel = new ParcelData()
				{
					mrecid = row["mrecid"].ToString().Trim(),
					mrecno = Record,
					mdwell = Card,
					mmap = row["mmap"].ToString().Trim(),
					mlnam = row["mlnam"].ToString().Trim(),
					mfnam = row["mfnam"].ToString().Trim(),
					madd1 = row["madd1"].ToString().Trim(),
					madd2 = row["madd2"].ToString().Trim(),
					mcity = row["mcity"].ToString().Trim(),
					mstate = row["mstate"].ToString().Trim(),
					mzip5 = Convert.ToInt32(row["mzip5"].ToString()),
					mzip4 = row["mzip4"].ToString().Trim(),
					macre = row["macre"].ToString().Trim(),
					mzone = row["mzone"].ToString().Trim(),
					mluse = row["mluse"].ToString().Trim(),
					moccup = Convert.ToInt32(row["moccup"].ToString()),
					mstory = row["mstory"].ToString().Trim(),
					mage = Convert.ToInt32(row["mage"].ToString()),
					mcond = row["mcond"].ToString().Trim(),
					mclass = row["mclass"].ToString().Trim(),
					mfactr = Convert.ToDecimal(row["mfactr"].ToString()),
					mdeprc = Convert.ToDecimal(row["mdeprc"].ToString()),
					mfound = Convert.ToInt32(row["mfound"].ToString()),
					mexwll = Convert.ToInt32(row["mexwll"].ToString()),
					mrooft = Convert.ToInt32(row["mrooft"].ToString()),
					mroofg = Convert.ToInt32(row["mroofg"].ToString()),
					mNdunt = Convert.ToInt32(row["m#dunt"].ToString()),
					mNroom = Convert.ToInt32(row["m#room"].ToString()),
					mNbr = Convert.ToInt32(row["m#br"].ToString()),
					mNfbth = Convert.ToInt32(row["m#bth"].ToString()),
					mNhbth = Convert.ToInt32(row["m#hbth"].ToString()),
					mfp2 = row["mfp2"].ToString().Trim(),
					mltrcd = row["mltrcd"].ToString().Trim(),
					mheat = Convert.ToInt32(row["mheat"].ToString()),
					mfuel = Convert.ToInt32(row["mfuel"].ToString()),
					mac = row["mac"].ToString().Trim(),
					mfp1 = row["mfp1"].ToString().Trim(),
					mcdr = row["mcdr"].ToString().Trim(),
					mekit = Convert.ToInt32(row["mekit"].ToString()),
					mbastp = Convert.ToInt32(row["mbastp"].ToString()),
					mpbtot = Convert.ToDecimal(row["mpbtot"].ToString()),
					msbtot = Convert.ToInt32(row["msbtot"].ToString()),
					mpbfin = Convert.ToDecimal(row["mpbfin"].ToString()),
					msbfin = Convert.ToInt32(row["msbfin"].ToString()),
					mbrate = Convert.ToDecimal(row["mbrate"].ToString()),
					mNflue = Convert.ToInt32(row["m_flue"].ToString()),
					mflutp = row["mflutp"].ToString().Trim(),
					mgart = Convert.ToInt32(row["mgart"].ToString()),
					mgarNc = Convert.ToInt32(row["mgar#c"].ToString()),
					mcarpt = Convert.ToInt32(row["mcarpt"].ToString()),
					mcarNc = Convert.ToInt32(row["mcar#c"].ToString()),
					mbiNc = Convert.ToInt32(row["mbi#c"].ToString()),
					mrow = Convert.ToInt32(row["mrow"].ToString()),
					mease = Convert.ToInt32(row["mease"].ToString()),
					mwater = Convert.ToInt32(row["mwater"].ToString()),
					msewer = Convert.ToInt32(row["msewer"].ToString()),
					mgas = row["mgas"].ToString().Trim(),
					melec = row["melec"].ToString().Trim(),
					mterrn = Convert.ToInt32(row["mterrn"].ToString()),
					mchar = Convert.ToInt32(row["mchar"].ToString()),
					motdes = row["motdes"].ToString().Trim(),
					mgart2 = Convert.ToInt32(row["mgart2"].ToString()),
					mgarN2 = Convert.ToInt32(row["mgar#2"].ToString()),
					mdatlg = Convert.ToInt32(row["mdatlg"].ToString()),
					mdatpr = Convert.ToInt32(row["mdatpr"].ToString()),
					mintyp = row["mintyp"].ToString().Trim(),
					mintyr = Convert.ToInt32(row["mintyr"].ToString()),
					minnoN = Convert.ToInt32(row["minno"].ToString()),
					minno2 = Convert.ToInt32(row["minno2"].ToString()),
					mdsufx = row["mdsufx"].ToString().Trim(),
					mwsufx = row["mwsufx"].ToString().Trim(),
					mpsufx = row["mpsufx"].ToString().Trim(),
					mimprv = Convert.ToInt32(row["mimprv"].ToString()),
					mtotld = Convert.ToInt32(row["mtotld"].ToString()),
					mtotoi = Convert.ToInt32(row["mtotoi"].ToString()),
					mtotpr = Convert.ToInt32(row["mtotpr"].ToString()),
					massb = Convert.ToInt32(row["massb"].ToString()),
					macpct = Convert.ToDecimal(row["macpct"].ToString()),
					m1frnt = Convert.ToDecimal(row["m1frnt"].ToString()),
					m1dpth = Convert.ToDecimal(row["m1dpth"].ToString()),
					m1area = Convert.ToInt32(row["m1area"].ToString()),
					mmcode = Convert.ToInt32(row["mmcode"].ToString()),
					m0depr = row["m0depr"].ToString().Trim(),
					m1um = row["m1um"].ToString().Trim(),
					m2frnt = Convert.ToDecimal(row["m2frnt"].ToString()),
					m2dpth = Convert.ToDecimal(row["m2dpth"].ToString()),
					m2area = Convert.ToInt32(row["m2area"].ToString()),
					mzipbr = Convert.ToInt32(row["mzipbr"].ToString()),
					mdelay = row["mdelay"].ToString().Trim(),
					m2um = row["m2um"].ToString().Trim(),
					mstrt = row["mstrt"].ToString().Trim(),
					mdirct = row["mdirct"].ToString().Trim(),
					mhseN = Convert.ToInt32(row["mhse#"].ToString()),
					mcdmo = Convert.ToInt32(row["mcdmo"].ToString()),
					mcdda = Convert.ToInt32(row["mcdda"].ToString()),
					mcdyr = Convert.ToInt32(row["mcdyr"].ToString()),
					m1dfac = Convert.ToDecimal(row["m1dfac"].ToString()),
					mrem1 = row["mrem1"].ToString().Trim(),
					mrem2 = row["mrem2"].ToString().Trim(),
					mmagcd = row["mmagcd"].ToString().Trim(),
					mathom = row["mathom"].ToString().Trim(),
					mdesc1 = row["mdesc1"].ToString().Trim(),
					mdesc2 = row["mdesc2"].ToString().Trim(),
					mdesc3 = row["mdesc3"].ToString().Trim(),
					mdesc4 = row["mdesc4"].ToString().Trim(),
					mfairv = Convert.ToInt32(row["mfairv"].ToString()),
					mlgity = row["mlgity"].ToString().Trim(),
					mlgiyr = Convert.ToInt32(row["mlgiyr"].ToString()),
					mlgnoN = Convert.ToInt32(row["mlgno#"].ToString()),
					mlgno2 = Convert.ToInt32(row["mlgno2"].ToString()),
					msubdv = row["msubdv"].ToString().Trim(),
					msellp = Convert.ToInt32(row["msellp"].ToString()),
					m2dfac = Convert.ToDecimal(row["m2dfac"].ToString()),
					minit = row["minit"].ToString().Trim(),
					minspd = Convert.ToInt32(row["minspd"].ToString()),
					mswl = Convert.ToInt32(row["mswl"].ToString()),
					mtutil = Convert.ToInt32(row["mtutil"].ToString()),
					mnbadj = Convert.ToDecimal(row["mnbadj"].ToString()),
					massl = Convert.ToInt32(row["massl"].ToString()),
					macsf = Convert.ToInt32(row["macsf"].ToString()),
					mcomm1 = row["mcomm1"].ToString().Trim(),
					mcomm2 = row["mcomm2"].ToString().Trim(),
					mcomm3 = row["mcomm3"].ToString().Trim(),
					macct = Convert.ToInt32(row["macct"].ToString()),
					mexwl2 = Convert.ToInt32(row["mexwl2"].ToString()),
					mcalc = row["mcalc"].ToString().Trim(),
					mfill4 = row["mfill4"].ToString().Trim(),
					mtbv = Convert.ToInt32(row["mtbv"].ToString()),
					mtbas = Convert.ToInt32(row["mtbas"].ToString()),
					mtfbas = Convert.ToInt32(row["mtfbas"].ToString()),
					mtplum = Convert.ToInt32(row["mtplum"].ToString()),
					mtheat = Convert.ToInt32(row["mtheat"].ToString()),
					mtac = Convert.ToInt32(row["mtac"].ToString()),
					mtfp = Convert.ToInt32(row["mtfp"].ToString()),
					mtfl = Convert.ToInt32(row["mtfl"].ToString()),
					mtbi = Convert.ToInt32(row["mtbi"].ToString()),
					mttadd = Convert.ToInt32(row["mttadd"].ToString()),
					mtsubt = Convert.ToInt32(row["mtsubt"].ToString()),
					mtotbv = Convert.ToInt32(row["mtotbv"].ToString()),
					musrid = row["musrid"].ToString().Trim(),
					mbasa = Convert.ToDecimal(row["mbasa"].ToString()),
					mtota = Convert.ToDecimal(row["mtota"].ToString()),
					mpsf = Convert.ToDecimal(row["mpsf"].ToString()),
					minwll = row["minwll"].ToString().Trim(),
					mfloor = row["mfloor"].ToString().Trim(),
					myrblt = Convert.ToInt32(row["myrblt"].ToString()),
					mcnst1 = row["mcnst1"].ToString().Trim(),
					mcnst2 = row["mcnst2"].ToString().Trim(),
					masslu = Convert.ToInt32(row["masslu"].ToString()),
					mmosld = Convert.ToInt32(row["mmosld"].ToString()),
					mdasld = Convert.ToInt32(row["mdasld"].ToString()),
					myrsld = Convert.ToInt32(row["myrsld"].ToString()),
					mtime = Convert.ToInt32(row["mtime"].ToString()),
					mhseN2 = row["mhse#2"].ToString().Trim(),
					m1adj = Convert.ToDecimal(row["m1adj"].ToString()),
					m2adj = Convert.ToDecimal(row["m2adj"].ToString()),
					mlgbkc = row["mlgbkc"].ToString().Trim(),
					mlgbkN = row["mlgbk#"].ToString().Trim(),
					mlgpgN = Convert.ToInt32(row["mlgpg#"].ToString()),
					meffag = Convert.ToInt32(row["meffag"].ToString()),
					mpcomp = Convert.ToDecimal(row["mpcomp"].ToString()),
					msttyp = row["msttyp"].ToString().Trim(),
					msdirs = row["msdirs"].ToString().Trim(),
					m1rate = Convert.ToDecimal(row["m1rate"].ToString()),
					m2rate = Convert.ToDecimal(row["m2rate"].ToString()),
					mfuncd = Convert.ToDecimal(row["mfuncd"].ToString()),
					mecond = Convert.ToDecimal(row["mecond"].ToString()),
					mnbrhd = Convert.ToInt32(row["mnbrhd"].ToString()),
					muser1 = row["muser1"].ToString().Trim(),
					muser2 = row["muser2"].ToString().Trim(),
					mdbook = row["mdbook"].ToString().Trim(),
					mdpage = Convert.ToInt32(row["mdpage"].ToString()),
					mwbook = row["mwbook"].ToString().Trim(),
					mwpage = Convert.ToInt32(row["mwpage"].ToString()),
					mdcode = row["mdcode"].ToString().Trim(),
					mwcode = row["mwcode"].ToString().Trim(),
					mmortc = Convert.ToInt32(row["mmortc"].ToString()),
					mfill7 = row["mfill7"].ToString().Trim(),
					macreN = Convert.ToDecimal(row["macre#"].ToString()),
					mgispn = row["mgispn"].ToString().Trim(),
					muser3 = row["muser3"].ToString().Trim(),
					muser4 = row["muser4"].ToString().Trim(),
					mimadj = Convert.ToInt32(row["mimadj"].ToString()),
					mcdrdt = Convert.ToInt32(row["mcdrdt"].ToString()),
					mmnud = Convert.ToInt32(row["mmnud"].ToString()),
					mmnnud = Convert.ToInt32(row["mmnnud"].ToString()),
					mss1 = Convert.ToInt32(row["mss1"].ToString()),
					mpcode = row["mpcode"].ToString().Trim(),
					mpbook = row["mpbook"].ToString().Trim(),
					mppage = Convert.ToInt32(row["mppage"].ToString()),
					mss2 = Convert.ToInt32(row["mss2"].ToString()),
					massm = Convert.ToInt32(row["massm"].ToString()),
					mfill9 = row["mfill9"].ToString().Trim(),
					mgrntr = row["mgrntr"].ToString().Trim(),
					mcvmo = Convert.ToInt32(row["mcvmo"].ToString()),
					mcvda = Convert.ToInt32(row["mcvda"].ToString()),
					mcvyr = Convert.ToInt32(row["mcvyr"].ToString()),
					mprout = row["mprout"].ToString().Trim(),
					mperr = row["mperr"].ToString().Trim(),
					mtbimp = Convert.ToInt32(row["mtbimp"].ToString()),
					mpuse = Convert.ToInt32(row["mpuse"].ToString()),
					mcvexp = row["mcvexp"].ToString().Trim(),
					metxyr = Convert.ToInt32(row["metxyr"].ToString()),
					mqapch = Convert.ToDecimal(row["mqapch"].ToString()),
					mqafil = row["mqafil"].ToString().Trim(),
					mpict = row["mpict"].ToString().Trim(),
					meacre = Convert.ToDecimal(row["meacre"].ToString()),
					mprcit = row["mprcit"].ToString().Trim(),
					mprsta = row["mprsta"].ToString().Trim(),
					mprzp1 = Convert.ToInt32(row["mprzp1"].ToString()),
					mprzp4 = row["mprzp4"].ToString().Trim(),
					mfpN = Convert.ToInt32(row["mfp#"].ToString()),
					msfpN = Convert.ToInt32(row["msfp#"].ToString()),
					mflN = Convert.ToInt32(row["mfl#"].ToString()),
					msflN = Convert.ToInt32(row["msfl#"].ToString()),
					mmflN = Convert.ToInt32(row["mmfl#"].ToString()),
					miofpN = Convert.ToInt32(row["miofp#"].ToString()),
					mstorN = Convert.ToDecimal(row["mstor#"].ToString()),
					mascom = row["mascom"].ToString().Trim(),
					mhrphN = Convert.ToInt32(row["mhrph#"].ToString()),
					mhrdat = Convert.ToInt32(row["mhrdat"].ToString()),
					mhrtim = Convert.ToInt32(row["mhrtim"].ToString()),
					mhrnam = row["mhrnam"].ToString().Trim(),
					mhrses = row["mhrses"].ToString().Trim(),
					mhidpc = row["mhidpc"].ToString().Trim(),
					mhidnm = row["mhidnm"].ToString().Trim(),
					mcamo = Convert.ToInt32(row["mcamo"].ToString()),
					mcada = Convert.ToInt32(row["mcada"].ToString()),
					mcayr = Convert.ToInt32(row["mcayr"].ToString()),
					moldoc = Convert.ToInt32(row["moldoc"].ToString()),

					//sub911Addr = String.Format("{0} {1} {2} {3}",
					//    row["mhse#"].ToString().Trim(),
					//    row["mdirct"].ToString().Trim(),
					//    row["mstrt"].ToString().Trim(),
					//    row["msttyp"].ToString().Trim()),

					orig_mrecid = row["mrecid"].ToString().Trim(),
					orig_mrecno = Record,
					orig_mdwell = Card,
					orig_mmap = row["mmap"].ToString().Trim(),
					orig_mlnam = row["mlnam"].ToString().Trim(),
					orig_mfnam = row["mfnam"].ToString().Trim(),
					orig_madd1 = row["madd1"].ToString().Trim(),
					orig_madd2 = row["madd2"].ToString().Trim(),
					orig_mcity = row["mcity"].ToString().Trim(),
					orig_mstate = row["mstate"].ToString().Trim(),
					orig_mzip5 = Convert.ToInt32(row["mzip5"].ToString()),
					orig_mzip4 = row["mzip4"].ToString().Trim(),
					orig_macre = row["macre"].ToString().Trim(),
					orig_mzone = row["mzone"].ToString().Trim(),
					orig_mluse = row["mluse"].ToString().Trim(),
					orig_moccup = Convert.ToInt32(row["moccup"].ToString()),
					orig_mstory = row["mstory"].ToString().Trim(),
					orig_mage = Convert.ToInt32(row["mage"].ToString()),
					orig_mcond = row["mcond"].ToString().Trim(),
					orig_mclass = row["mclass"].ToString().Trim(),
					orig_mfactr = Convert.ToDecimal(row["mfactr"].ToString()),
					orig_mdeprc = Convert.ToDecimal(row["mdeprc"].ToString()),
					orig_mfound = Convert.ToInt32(row["mfound"].ToString()),
					orig_mexwll = Convert.ToInt32(row["mexwll"].ToString()),
					orig_mrooft = Convert.ToInt32(row["mrooft"].ToString()),
					orig_mroofg = Convert.ToInt32(row["mroofg"].ToString()),
					orig_mNdunt = Convert.ToInt32(row["m#dunt"].ToString()),
					orig_mNroom = Convert.ToInt32(row["m#room"].ToString()),
					orig_mNbr = Convert.ToInt32(row["m#br"].ToString()),
					orig_mNfbth = Convert.ToInt32(row["m#fbth"].ToString()),
					orig_mNhbth = Convert.ToInt32(row["m#hbth"].ToString()),
					orig_mfp2 = row["mfp2"].ToString().Trim(),
					orig_mltrcd = row["mltrcd"].ToString().Trim(),
					orig_mheat = Convert.ToInt32(row["mheat"].ToString()),
					orig_mfuel = Convert.ToInt32(row["mfuel"].ToString()),
					orig_mac = row["mac"].ToString().Trim(),
					orig_mfp1 = row["mfp1"].ToString().Trim(),
					orig_mcdr = row["mcdr"].ToString().Trim(),
					orig_mekit = Convert.ToInt32(row["mekit"].ToString()),
					orig_mbastp = Convert.ToInt32(row["mbastp"].ToString()),
					orig_mpbtot = Convert.ToDecimal(row["mpbtot"].ToString()),
					orig_msbtot = Convert.ToInt32(row["msbtot"].ToString()),
					orig_mpbfin = Convert.ToDecimal(row["mpbfin"].ToString()),
					orig_msbfin = Convert.ToInt32(row["msbfin"].ToString()),
					orig_mbrate = Convert.ToDecimal(row["mbrate"].ToString()),
					orig_mNflue = Convert.ToInt32(row["m_flue"].ToString()),
					orig_mflutp = row["mflutp"].ToString().Trim(),
					orig_mgart = Convert.ToInt32(row["mgart"].ToString()),
					orig_mgarNc = Convert.ToInt32(row["mgar#c"].ToString()),
					orig_mcarpt = Convert.ToInt32(row["mcarpt"].ToString()),
					orig_mcarNc = Convert.ToInt32(row["mcar#c"].ToString()),
					orig_mbiNc = Convert.ToInt32(row["mbi#c"].ToString()),
					orig_mrow = Convert.ToInt32(row["mrow"].ToString()),
					orig_mease = Convert.ToInt32(row["mease"].ToString()),
					orig_mwater = Convert.ToInt32(row["mwater"].ToString()),
					orig_msewer = Convert.ToInt32(row["msewer"].ToString()),
					orig_mgas = row["mgas"].ToString().Trim(),
					orig_melec = row["melec"].ToString().Trim(),
					orig_mterrn = Convert.ToInt32(row["mterrn"].ToString()),
					orig_mchar = Convert.ToInt32(row["mchar"].ToString()),
					orig_motdes = row["motdes"].ToString().Trim(),
					orig_mgart2 = Convert.ToInt32(row["mgart2"].ToString()),
					orig_mgarN2 = Convert.ToInt32(row["mgar#2"].ToString()),
					orig_mdatlg = Convert.ToInt32(row["mdatlg"].ToString()),
					orig_mdatpr = Convert.ToInt32(row["mdatpr"].ToString()),
					orig_mintyp = row["mintyp"].ToString().Trim(),
					orig_mintyr = Convert.ToInt32(row["mintyr"].ToString()),
					orig_minnoN = Convert.ToInt32(row["minno#"].ToString()),
					orig_minno2 = Convert.ToInt32(row["minno2"].ToString()),
					orig_mdsufx = row["mdsufx"].ToString().Trim(),
					orig_mwsufx = row["mwsufx"].ToString().Trim(),
					orig_mpsufx = row["mpsufx"].ToString().Trim(),
					orig_mimprv = Convert.ToInt32(row["mimprv"].ToString()),
					orig_mtotld = Convert.ToInt32(row["mtotld"].ToString()),
					orig_mtotoi = Convert.ToInt32(row["mtotoi"].ToString()),
					orig_mtotpr = Convert.ToInt32(row["mtotpr"].ToString()),
					orig_massb = Convert.ToInt32(row["massb"].ToString()),
					orig_macpct = Convert.ToDecimal(row["macpct"].ToString()),
					orig_m1frnt = Convert.ToDecimal(row["m1frnt"].ToString()),
					orig_m1dpth = Convert.ToDecimal(row["m1dpth"].ToString()),
					orig_m1area = Convert.ToInt32(row["m1area"].ToString()),
					orig_mmcode = Convert.ToInt32(row["mmcode"].ToString()),
					orig_m0depr = row["m0depr"].ToString().Trim(),
					orig_m1um = row["m1um"].ToString().Trim(),
					orig_m2frnt = Convert.ToDecimal(row["m2frnt"].ToString()),
					orig_m2dpth = Convert.ToDecimal(row["m2dpth"].ToString()),
					orig_m2area = Convert.ToInt32(row["m2area"].ToString()),
					orig_mzipbr = Convert.ToInt32(row["mzipbr"].ToString()),
					orig_mdelay = row["mdelay"].ToString().Trim(),
					orig_m2um = row["m2um"].ToString().Trim(),
					orig_mstrt = row["mstrt"].ToString().Trim(),
					orig_mdirct = row["mdirct"].ToString().Trim(),
					orig_mhseN = Convert.ToInt32(row["mhse#"].ToString()),
					orig_mcdmo = Convert.ToInt32(row["mcdmo"].ToString()),
					orig_mcdda = Convert.ToInt32(row["mcdda"].ToString()),
					orig_mcdyr = Convert.ToInt32(row["mcdyr"].ToString()),
					orig_m1dfac = Convert.ToDecimal(row["m1dfac"].ToString()),
					orig_mrem1 = row["mrem1"].ToString().Trim(),
					orig_mrem2 = row["mrem2"].ToString().Trim(),
					orig_mmagcd = row["mmagcd"].ToString().Trim(),
					orig_mathom = row["mathom"].ToString().Trim(),
					orig_mdesc1 = row["mdesc1"].ToString().Trim(),
					orig_mdesc2 = row["mdesc2"].ToString().Trim(),
					orig_mdesc3 = row["mdesc3"].ToString().Trim(),
					orig_mdesc4 = row["mdesc4"].ToString().Trim(),
					orig_mfairv = Convert.ToInt32(row["mfairv"].ToString()),
					orig_mlgity = row["mlgity"].ToString().Trim(),
					orig_mlgiyr = Convert.ToInt32(row["mlgiyr"].ToString()),
					orig_mlgnoN = Convert.ToInt32(row["mlgno#"].ToString()),
					orig_mlgno2 = Convert.ToInt32(row["mlgno2"].ToString()),
					orig_msubdv = row["msubdv"].ToString().Trim(),
					orig_msellp = Convert.ToInt32(row["msellp"].ToString()),
					orig_m2dfac = Convert.ToDecimal(row["m2dfac"].ToString()),
					orig_minit = row["minit"].ToString().Trim(),
					orig_minspd = Convert.ToInt32(row["minspd"].ToString()),
					orig_mswl = Convert.ToInt32(row["mswl"].ToString()),
					orig_mtutil = Convert.ToInt32(row["mtutil"].ToString()),
					orig_mnbadj = Convert.ToDecimal(row["mnbadj"].ToString()),
					orig_massl = Convert.ToInt32(row["massl"].ToString()),
					orig_macsf = Convert.ToInt32(row["macsf"].ToString()),
					orig_mcomm1 = row["mcomm1"].ToString().Trim(),
					orig_mcomm2 = row["mcomm2"].ToString().Trim(),
					orig_mcomm3 = row["mcomm3"].ToString().Trim(),
					orig_macct = Convert.ToInt32(row["macct"].ToString()),
					orig_mexwl2 = Convert.ToInt32(row["mexwl2"].ToString()),
					orig_mcalc = row["mcalc"].ToString().Trim(),
					orig_mfill4 = row["mfill4"].ToString().Trim(),
					orig_mtbv = Convert.ToInt32(row["mtbv"].ToString()),
					orig_mtbas = Convert.ToInt32(row["mtbas"].ToString()),
					orig_mtfbas = Convert.ToInt32(row["mtfbas"].ToString()),
					orig_mtplum = Convert.ToInt32(row["mtplum"].ToString()),
					orig_mtheat = Convert.ToInt32(row["mtheat"].ToString()),
					orig_mtac = Convert.ToInt32(row["mtac"].ToString()),
					orig_mtfp = Convert.ToInt32(row["mtfp"].ToString()),
					orig_mtfl = Convert.ToInt32(row["mtfl"].ToString()),
					orig_mtbi = Convert.ToInt32(row["mtbi"].ToString()),
					orig_mttadd = Convert.ToInt32(row["mttadd"].ToString()),
					orig_mtsubt = Convert.ToInt32(row["mtsubt"].ToString()),
					orig_mtotbv = Convert.ToInt32(row["mtotbv"].ToString()),
					orig_musrid = row["musrid"].ToString().Trim(),
					orig_mbasa = Convert.ToDecimal(row["mbasa"].ToString()),
					orig_mtota = Convert.ToDecimal(row["mtota"].ToString()),
					orig_mpsf = Convert.ToDecimal(row["mpsf"].ToString()),
					orig_minwll = row["minwll"].ToString().Trim(),
					orig_mfloor = row["mfloor"].ToString().Trim(),
					orig_myrblt = Convert.ToInt32(row["myrblt"].ToString()),
					orig_mcnst1 = row["mcnst1"].ToString().Trim(),
					orig_mcnst2 = row["mcnst2"].ToString().Trim(),
					orig_masslu = Convert.ToInt32(row["masslu"].ToString()),
					orig_mmosld = Convert.ToInt32(row["mmosld"].ToString()),
					orig_mdasld = Convert.ToInt32(row["mdasld"].ToString()),
					orig_myrsld = Convert.ToInt32(row["myrsld"].ToString()),
					orig_mtime = Convert.ToInt32(row["mtime"].ToString()),
					orig_mhseN2 = row["mhse#2"].ToString().Trim(),
					orig_m1adj = Convert.ToDecimal(row["m1adj"].ToString()),
					orig_m2adj = Convert.ToDecimal(row["m2adj"].ToString()),
					orig_mlgbkc = row["mlgbkc"].ToString().Trim(),
					orig_mlgbkN = row["mlgbk#"].ToString().Trim(),
					orig_mlgpgN = Convert.ToInt32(row["mlgpg#"].ToString()),
					orig_meffag = Convert.ToInt32(row["meffag"].ToString()),
					orig_mpcomp = Convert.ToDecimal(row["mpcomp"].ToString()),
					orig_msttyp = row["msttyp"].ToString().Trim(),
					orig_msdirs = row["msdirs"].ToString().Trim(),
					orig_m1rate = Convert.ToDecimal(row["m1rate"].ToString()),
					orig_m2rate = Convert.ToDecimal(row["m2rate"].ToString()),
					orig_mfuncd = Convert.ToDecimal(row["mfuncd"].ToString()),
					orig_mecond = Convert.ToDecimal(row["mecond"].ToString()),
					orig_mnbrhd = Convert.ToInt32(row["mnbrhd"].ToString()),
					orig_muser1 = row["muser1"].ToString().Trim(),
					orig_muser2 = row["muser2"].ToString().Trim(),
					orig_mdbook = row["mdbook"].ToString().Trim(),
					orig_mdpage = Convert.ToInt32(row["mdpage"].ToString()),
					orig_mwbook = row["mwbook"].ToString().Trim(),
					orig_mwpage = Convert.ToInt32(row["mwpage"].ToString()),
					orig_mdcode = row["mdcode"].ToString().Trim(),
					orig_mwcode = row["mwcode"].ToString().Trim(),
					orig_mmortc = Convert.ToInt32(row["mmortc"].ToString()),
					orig_mfill7 = row["mfill7"].ToString().Trim(),
					orig_macreN = Convert.ToDecimal(row["macre#"].ToString()),
					orig_mgispn = row["mgispn"].ToString().Trim(),
					orig_muser3 = row["muser3"].ToString().Trim(),
					orig_muser4 = row["muser4"].ToString().Trim(),
					orig_mimadj = Convert.ToInt32(row["mimadj"].ToString()),
					orig_mcdrdt = Convert.ToInt32(row["mcdrdt"].ToString()),
					orig_mmnud = Convert.ToInt32(row["mmnud"].ToString()),
					orig_mmnnud = Convert.ToInt32(row["mmnnud"].ToString()),
					orig_mss1 = Convert.ToInt32(row["mss1"].ToString()),
					orig_mpcode = row["mpcode"].ToString().Trim(),
					orig_mpbook = row["mpbook"].ToString().Trim(),
					orig_mppage = Convert.ToInt32(row["mppage"].ToString()),
					orig_mss2 = Convert.ToInt32(row["mss2"].ToString()),
					orig_massm = Convert.ToInt32(row["massm"].ToString()),
					orig_mfill9 = row["mfill9"].ToString().Trim(),
					orig_mgrntr = row["mgrntr"].ToString().Trim(),
					orig_mcvmo = Convert.ToInt32(row["mcvmo"].ToString()),
					orig_mcvda = Convert.ToInt32(row["mcvda"].ToString()),
					orig_mcvyr = Convert.ToInt32(row["mcvyr"].ToString()),
					orig_mprout = row["mprout"].ToString().Trim(),
					orig_mperr = row["mperr"].ToString().Trim(),
					orig_mtbimp = Convert.ToInt32(row["mtbimp"].ToString()),
					orig_mpuse = Convert.ToInt32(row["mpuse"].ToString()),
					orig_mcvexp = row["mcvexp"].ToString().Trim(),
					orig_metxyr = Convert.ToInt32(row["metxyr"].ToString()),
					orig_mqapch = Convert.ToDecimal(row["mqapch"].ToString()),
					orig_mqafil = row["mqafil"].ToString().Trim(),
					orig_mpict = row["mpict"].ToString().Trim(),
					orig_meacre = Convert.ToDecimal(row["meacre"].ToString()),
					orig_mprcit = row["mprcit"].ToString().Trim(),
					orig_mprsta = row["mprsta"].ToString().Trim(),
					orig_mprzp1 = Convert.ToInt32(row["mprzp1"].ToString()),
					orig_mprzp4 = row["mprzp4"].ToString().Trim(),
					orig_mfpN = Convert.ToInt32(row["mfp#"].ToString()),
					orig_msfpN = Convert.ToInt32(row["msfp#"].ToString()),
					orig_mflN = Convert.ToInt32(row["mfl#"].ToString()),
					orig_msflN = Convert.ToInt32(row["msfl#"].ToString()),
					orig_mmflN = Convert.ToInt32(row["mmfl#"].ToString()),
					orig_miofpN = Convert.ToInt32(row["miofp#"].ToString()),
					orig_mstorN = Convert.ToDecimal(row["mstor#"].ToString()),
					orig_mascom = row["mascom"].ToString().Trim(),
					orig_mhrphN = Convert.ToInt32(row["mhrph#"].ToString()),
					orig_mhrdat = Convert.ToInt32(row["mhrdat"].ToString()),
					orig_mhrtim = Convert.ToInt32(row["mhrtim"].ToString()),
					orig_mhrnam = row["mhrnam"].ToString().Trim(),
					orig_mhrses = row["mhrses"].ToString().Trim(),
					orig_mhidpc = row["mhidpc"].ToString().Trim(),
					orig_mhidnm = row["mhidnm"].ToString().Trim(),
					orig_mcamo = Convert.ToInt32(row["mcamo"].ToString()),
					orig_mcada = Convert.ToInt32(row["mcada"].ToString()),
					orig_mcayr = Convert.ToInt32(row["mcayr"].ToString()),
					orig_moldoc = Convert.ToInt32(row["moldoc"].ToString())
				};

				this.Add(parcel);
			}
		}

		public ParcelData GetParcel(SWallTech.CAMRA_Connection _db, int recno, int card)
		{
			ParcelData data = null;

			var q = from p in this
					where p.Record == recno && p.Card == card
					select p;

			if (q.Count() >= 1)
			{
				data = q.SingleOrDefault();
			}
			else
			{
				data = ParcelData.getParcel(_db,
					recno,
					card);

				//this.Add(data);
			}

			return data;
		}
	}

	public class SearchParameter
	{
		public string FieldName
		{
			get; set;
		}

		public string FieldValue
		{
			get; set;
		}

		public string Comparator
		{
			get; set;
		}

		public virtual string WhereClause
		{
			get
			{
				string where = String.Format(" {0} {1} {2} ",
					FieldName, Comparator, FieldValue);

				return where;
			}
		}
	}

	public class SearchParameterOrList : SearchParameter
	{
		private new string FieldName
		{
			get; set;
		}

		private new string Comparator
		{
			get; set;
		}

		private new string FieldValue
		{
			get; set;
		}

		public List<SearchParameter> SearchParameterList
		{
			get; set;
		}

		public override string WhereClause
		{
			get
			{
				StringBuilder where = new StringBuilder();
				where.Append("( ");
				int count = 0;
				foreach (var parm in this.SearchParameterList)
				{
					if (++count > 1)
					{
						where.Append(" or ");
					}
					where.Append(parm.WhereClause);
				}
				where.Append(" ) ");

				return where.ToString();
			}
		}
	}

	public class SearchParameterAndList : SearchParameter
	{
		private new string FieldName
		{
			get; set;
		}

		private new string Comparator
		{
			get; set;
		}

		private new string FieldValue
		{
			get; set;
		}

		public List<SearchParameter> SearchParameterList
		{
			get; set;
		}

		public override string WhereClause
		{
			get
			{
				StringBuilder where = new StringBuilder();
				where.Append("( ");
				int count = 0;
				foreach (var parm in this.SearchParameterList)
				{
					if (++count > 1)
					{
						where.Append(" and ");
					}
					where.Append(parm.WhereClause);
				}
				where.Append(" ) ");

				return where.ToString();
			}
		}
	}

	public class SearchParameterAndOrList : SearchParameter
	{
		private new string FieldName
		{
			get; set;
		}

		private new string Comparator
		{
			get; set;
		}

		private new string FieldValue
		{
			get; set;
		}

		public List<SearchParameter> SearchParameterList
		{
			get; set;
		}

		public override string WhereClause
		{
			get
			{
				StringBuilder where = new StringBuilder();
				where.Append("( ");
				int count = 0;
				foreach (var parm in this.SearchParameterList)
				{
					if (++count > 1)
					{
						where.Append(" and ");
					}
					if (++count > 2)
					{
						where.Append(" or ");
					}
					if (++count > 3)
					{
						where.Append(" and ");
					}
				}
				where.Append(" )");

				return base.WhereClause;
			}
		}
	}

	public class RecordMaximumExceededException : Exception
	{
		public int RecordMaximum
		{
			get; set;
		}

		public int QueryCount
		{
			get; set;
		}

		public RecordMaximumExceededException()
			: base("Query exceeds maximum number of Records allowed.")
		{
		}

		public RecordMaximumExceededException(string message)
			: base(message)
		{
		}
	}

	internal class RecordCard
	{
		public int Record
		{
			get; set;
		}

		public int Card
		{
			get; set;
		}
	}
}