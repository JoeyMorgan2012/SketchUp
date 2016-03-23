using System;
using System.Data;
using System.Text;
using SWallTech;

namespace SketchUp
{
	public class GasLogData
	{
		public event EventHandler<GasLogChangedEventArgs> GasLogChangedEvent;

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

		public bool GasFPIsChanged
		{
			get
			{
				return (orig_NbrGasFP != NbrGasFP);
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
				return NbrGasFP > 0;
			}
		}

		private string _insertSQL = "insert into {0}.{1}gaslg (grecno,gdwell,gnogas) values ({2},{3},{4}) ";

		public bool InsertOrUpdate(DBAccessManager _fox)
		{
			if (IsDataValid)
			{
				if (IsNewRecord)
				{
					_fox.ExecuteNonSelectStatement(String.Format(_insertSQL, MainForm.localLib, MainForm.localPreFix, Record, Card, NbrGasFP));
				}
				else
				{
					// update logic
				}
				return true;
			}

			return false;
		}

		public GasLogData()
		{
		}

		public static GasLogData CreateNewGasLog(int record, int card)
		{
			return CreateNewGasLog(record, card, true);
		}

		public void SetOriginalValues()
		{
			orig_NbrGasFP = NbrGasFP;
		}

		public static GasLogData CreateNewGasLog(int record, int card, bool isNewRecord)
		{
			var gfp = new GasLogData()
			{
				IsNewRecord = isNewRecord,
				Record = record,
				Card = card,
				NbrGasFP = 0,
				orig_NbrGasFP = 0
			};

			if (isNewRecord)
			{
				gfp.SetOriginalValues();
			}
			return gfp;
		}

		public static GasLogData GetGasLog(DBAccessManager fox, int recno, int card)
		{
			GasLogData gaslog = null;

			StringBuilder glsql = new StringBuilder();
			glsql.Append("select grecno,gdwell,gnogas ");
			glsql.Append(String.Format("from {0}.{1}gaslg where grecno = {2} and gdwell = {3} ", MainForm.localLib, MainForm.localPreFix, recno, card));

			DataSet ds = fox.RunSelectStatement(glsql.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow ireader = ds.Tables[0].Rows[0];

				gaslog = new GasLogData()
				{
					Record = recno,
					Card = card,
					NbrGasFP = Convert.ToInt32(ireader["gnogas"].ToString()),
				};
			}
			return gaslog;
		}

		private void FireChangedEvent(string GasLogFile)
		{
			if (GasLogChangedEvent != null)
			{
				GasLogChangedEvent(this,
					new GasLogChangedEventArgs()
					{
						GasLog = GasLogFile
					});
			}
		}
	}

	public class GasLogChangedEventArgs : EventArgs
	{
		public GasLogChangedEventArgs()
			: base()
		{
		}

		public string GasLog
		{
			get; set;
		}
	}
}