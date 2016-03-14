using System;
using System.Data;
using System.IO;
using System.Text;

namespace SketchUp
{
	public class LandData
	{
		public event EventHandler<LandChangedEventArgs> LandChangedEvent;

		public string lid
		{
			get; set;
		}

		public int lrecno
		{
			get; set;
		}

		public int ldwell
		{
			get; set;
		}

		public int lseqno
		{
			get; set;
		}

		public int laccod
		{
			get; set;
		}

		public string lhs
		{
			get; set;
		}

		public string lacre
		{
			get; set;
		}

		public int lvalue
		{
			get; set;
		}

		public string llp
		{
			get; set;
		}

		public decimal ladj
		{
			get; set;
		}

		public decimal lacren
		{
			get; set;
		}

		public int ltotal
		{
			get; set;
		}

		public string ldescr
		{
			get; set;
		}

		public int lwater
		{
			get; set;
		}

		public int lsewer
		{
			get; set;
		}

		public int lutil
		{
			get; set;
		}

		public int sumltotal
		{
			get; set;
		}

		public string orig_lid
		{
			get; set;
		}

		public int orig_lseqno
		{
			get; set;
		}

		public int orig_laccod
		{
			get; set;
		}

		public string orig_lhs
		{
			get; set;
		}

		public string orig_lacre
		{
			get; set;
		}

		public int orig_lvalue
		{
			get; set;
		}

		public string orig_llp
		{
			get; set;
		}

		public decimal orig_ladj
		{
			get; set;
		}

		public decimal orig_lacren
		{
			get; set;
		}

		public int orig_ltotal
		{
			get; set;
		}

		public string orig_ldescr
		{
			get; set;
		}

		public int orig_lwater
		{
			get; set;
		}

		public int orig_lsewer
		{
			get; set;
		}

		public int orig_lutil
		{
			get; set;
		}

		public int orig_sumltotal
		{
			get; set;
		}

		public bool LandisChanged
		{
			get
			{
				return (orig_laccod != laccod
					|| orig_lseqno != lseqno
					|| orig_lhs.Trim() != lhs.Trim()
					|| orig_lacre.Trim() != lacre.Trim()
					|| orig_lvalue != lvalue
					|| orig_llp.Trim() != llp.Trim()
					|| orig_ladj != ladj
					|| orig_lacren != lacren
					|| orig_ltotal != ltotal
					|| orig_ldescr.Trim() != ldescr.Trim()
					|| orig_lwater != lwater
					|| orig_lsewer != lsewer
					|| orig_lutil != lutil);
			}
		}

		public bool IsNewRecord
		{
			get; private set;
		}

		private bool IsDataValid
		{
			get
			{
				return laccod > 0;
			}
		}

		private string _insertSQL = "insert into {0}.{1}land (lid,lrecno,ldwell,lseqno,laccod,lhs,lacre,lvalue,llp,ladj,lacre#,ltotal,ldescr,lwater,lsewer,lutil ) " +
					   " values ('L',{2},{3},{4},{5},'{6}','{7}',{8},'{9}',{10},{11},{12},'{13}',{14},{15},{16} ) ";

		public bool InsertOrUpdate(SWallTech.CAMRA_Connection _fox)
		{
			if (IsDataValid)
			{
				if (IsNewRecord)
				{
					_fox.DBConnection.ExecuteNonSelectStatement(String.Format(_insertSQL, MainForm.localLib, MainForm.localPreFix,
						lrecno, ldwell, lseqno, laccod, lhs, lacre, lvalue, llp, ladj, lacren, ltotal, ldescr, lwater, lsewer, lutil));
				}
				else
				{
					// update logic
				}
				return true;
			}

			return false;
		}

		private LandData()
		{
		}

		public static LandData CreateNewLand(int record, int card, int seqno)
		{
			return CreateNewLand(record, card, seqno, true);
		}

		public void SetOriginalValues()
		{
			orig_lid = lid;
			orig_lseqno = lseqno;
			orig_laccod = laccod;
			orig_lhs = lhs;
			orig_lacre = lacre;
			orig_lvalue = lvalue;
			orig_llp = llp;
			orig_ladj = ladj;
			orig_lacren = lacren;
			orig_ltotal = ltotal;
			orig_ldescr = ldescr;
			orig_lwater = lwater;
			orig_lsewer = lsewer;
			orig_lutil = lutil;
		}

		public static LandData CreateNewLand(int record, int card, int seqno, bool isNewRecord)
		{
			var ld = new LandData()
			{
				IsNewRecord = isNewRecord,
				lid = "L",
				lrecno = record,
				ldwell = card,
				lseqno = seqno,
				laccod = 0,
				lhs = "",
				lacre = "",
				lvalue = 0,
				llp = "",
				ladj = 0,
				lacren = 0,
				ltotal = 0,
				ldescr = "",
				lwater = 0,
				lsewer = 0,
				lutil = 0,
				sumltotal = 0,

				orig_laccod = 0,
				orig_lseqno = 0,
				orig_lhs = "",
				orig_lacre = "",
				orig_lvalue = 0,
				orig_llp = "",
				orig_ladj = 0,
				orig_lacren = 0,
				orig_ltotal = 0,
				orig_ldescr = "",
				orig_lwater = 0,
				orig_lsewer = 0,
				orig_lutil = 0,
				orig_sumltotal = 0,
			};

			if (isNewRecord)
			{
				ld.SetOriginalValues();
			}
			return ld;
		}

		public static LandData getLand(SWallTech.CAMRA_Connection _fox, int record, int card, int seqno)
		{
			LandData _land = null;

			StringBuilder subLand = new StringBuilder();
			subLand.Append(" select lid,lrecno,ldwell,lseqno,laccod,lhs,lacre,lvalue,llp,ladj,lacre#,ltotal,ldescr,lwater,lsewer,lutil ");

			//subLand.Append(String.Format(" from land where lrecno = {0} and ldwell = {1} and lseqno = {2} and laccod > 0 ", record, card,seqno));
			//subLand.Append(String.Format(" from land where lrecno = {0} and ldwell = {1} and laccod > 0 order by lseqno ", record, card));
			subLand.Append(String.Format(" from {0}.{1}land where lrecno = {2} and ldwell = {3} order by lseqno ", MainForm.localLib, MainForm.localPreFix, record, card));

			DataSet Land = _fox.DBConnection.RunSelectStatement(subLand.ToString());

			if (Land.Tables[0].Rows.Count > 0)
			{
				DataRow row = Land.Tables[0].Rows[0];

				_land = new LandData()
				{
					lid = row["lid"].ToString().Trim(),
					lrecno = record,
					ldwell = card,
					lseqno = Convert.ToInt32(row["lseqno"].ToString()),
					laccod = Convert.ToInt32(row["laccod"].ToString()),
					lhs = row["lhs"].ToString().Trim(),
					lacre = row["lacre"].ToString().Trim(),
					lvalue = Convert.ToInt32(row["lvalue"].ToString()),
					llp = row["llp"].ToString().Trim(),
					ladj = Convert.ToDecimal(row["ladj"].ToString()),
					lacren = Convert.ToDecimal(row["lacre#"].ToString()),
					ltotal = Convert.ToInt32(row["ltotal"].ToString()),
					ldescr = row["ldescr"].ToString().Trim(),
					lwater = Convert.ToInt32(row["lwater"].ToString()),
					lsewer = Convert.ToInt32(row["lsewer"].ToString()),
					lutil = Convert.ToInt32(row["lutil"].ToString()),

					orig_lid = row["lid"].ToString().Trim(),
					orig_lseqno = Convert.ToInt32(row["lseqno"].ToString()),
					orig_laccod = Convert.ToInt32(row["laccod"].ToString()),
					orig_lhs = row["lhs"].ToString().Trim(),
					orig_lacre = row["lacre"].ToString().Trim(),
					orig_lvalue = Convert.ToInt32(row["lvalue"].ToString()),
					orig_llp = row["llp"].ToString().Trim(),
					orig_ladj = Convert.ToDecimal(row["ladj"].ToString()),
					orig_lacren = Convert.ToDecimal(row["lacre#"].ToString()),
					orig_ltotal = Convert.ToInt32(row["ltotal"].ToString()),
					orig_ldescr = row["ldescr"].ToString().Trim(),
					orig_lwater = Convert.ToInt32(row["lwater"].ToString()),
					orig_lsewer = Convert.ToInt32(row["lsewer"].ToString()),
					orig_lutil = Convert.ToInt32(row["lutil"].ToString())
				};
			}

			if (_land != null)
			{
				StringBuilder sumLand = new StringBuilder();

				//sumLand.Append(String.Format("select sum(ltotal) from land where lrecno = {0} and ldwell = {1}", record, card));
				sumLand.Append(String.Format("select sum(ltotal) as totalLand from {0}.{1}land where lrecno = {2} and ldwell = {3} and laccod  > 0 ",
					MainForm.localLib, MainForm.localPreFix, record, card));

				_land.orig_sumltotal = 0;
				_land.sumltotal = 0;

				//try
				//{
				_land.orig_sumltotal = Convert.ToInt32(_fox.DBConnection.ExecuteScalar(sumLand.ToString()));

				_land.sumltotal = _land.orig_sumltotal;

				//}
				//catch (InvalidCastException)
				//{
				//    if (_land.orig_sumltotal == null)
				//    {
				//        _land.orig_sumltotal = 0;
				//        _land.sumltotal = 0;
				//    }
				//}
			}
			return _land;
		}

		private void FireChangedEvent(string landfile)
		{
			if (LandChangedEvent != null)
			{
				LandChangedEvent(this,
					new LandChangedEventArgs()
					{
						LandName = landfile
					});
			}
		}
	}

	public class LandChangedEventArgs : EventArgs
	{
		public LandChangedEventArgs()
			: base()
		{
		}

		public string LandName
		{
			get; set;
		}
	}
}