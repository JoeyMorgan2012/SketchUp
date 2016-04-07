using System;
using System.Data;
using System.Text;

namespace SketchUp
{
	public class ParcelNotes
	{
		public event EventHandler<NoteCollectionChangedEventArgs> NoteCollectionChangedEvent;

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

		public string NLin1
		{
			get; set;
		}

		public string NLin2
		{
			get; set;
		}

		public string NLin3
		{
			get; set;
		}

		public string NLin4
		{
			get; set;
		}

		public string NLin5
		{
			get; set;
		}

		public string NLin6
		{
			get; set;
		}

		public string NLin7
		{
			get; set;
		}

		public string NLin8
		{
			get; set;
		}

		public string NLin9
		{
			get; set;
		}

		public string NLin10
		{
			get; set;
		}

		public string NLin11
		{
			get; set;
		}

		public string NLin12
		{
			get; set;
		}

		public string NLin13
		{
			get; set;
		}

		public string NLin14
		{
			get; set;
		}

		public string NLin15
		{
			get; set;
		}

		public string NLin16
		{
			get; set;
		}

		public string NLin17
		{
			get; set;
		}

		public string NLin18
		{
			get; set;
		}

		public string NLin19
		{
			get; set;
		}

		public string NLin20
		{
			get; set;
		}

		public string NLin21
		{
			get; set;
		}

		public ParcelNotes()
		{
		}

		public static ParcelNotes GetNotes(SWallTech.CAMRA_Connection conn, string prefix, int _rec, int _card, int _seqno)
		{
			var db = conn.DBConnection;

			ParcelNotes NotesCollectionList = null;

			StringBuilder ntSql = new StringBuilder();
			ntSql.Append("select clin1,clin2,clin3,clin4,clin5,clin6,clin7,clin8,clin9,clin10, ");
			ntSql.Append("clin11,clin12,clin13,clin14,clin15,clin16,clin17,clin18,clin19,clin20,clin21 ");
			ntSql.Append(String.Format("from {0}.{1}note where crecno = {2} and cdwell = {3} and cseqno = {4} ",
			   SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, _rec, _card, _seqno));

			DataSet ds = db.RunSelectStatement(ntSql.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				DataRow NTreader = ds.Tables[0].Rows[0];

				NotesCollectionList = new ParcelNotes()
				{
					Record = _rec,
					Card = _card,
					SeqNo = _seqno,
					NLin1 = NTreader["clin1"].ToString().Trim(),
					NLin2 = NTreader["clin2"].ToString().Trim(),
					NLin3 = NTreader["clin3"].ToString().Trim(),
					NLin4 = NTreader["clin4"].ToString().Trim(),
					NLin5 = NTreader["clin5"].ToString().Trim(),
					NLin6 = NTreader["clin6"].ToString().Trim(),
					NLin7 = NTreader["clin7"].ToString().Trim(),
					NLin8 = NTreader["clin8"].ToString().Trim(),
					NLin9 = NTreader["clin9"].ToString().Trim(),
					NLin10 = NTreader["clin10"].ToString().Trim(),
					NLin11 = NTreader["clin11"].ToString().Trim(),
					NLin12 = NTreader["clin12"].ToString().Trim(),
					NLin13 = NTreader["clin13"].ToString().Trim(),
					NLin14 = NTreader["clin14"].ToString().Trim(),
					NLin15 = NTreader["clin15"].ToString().Trim(),
					NLin16 = NTreader["clin16"].ToString().Trim(),
					NLin17 = NTreader["clin17"].ToString().Trim(),
					NLin18 = NTreader["clin18"].ToString().Trim(),
					NLin19 = NTreader["clin19"].ToString().Trim(),
					NLin20 = NTreader["clin20"].ToString().Trim(),
					NLin21 = NTreader["clin21"].ToString().Trim()
				};
			}

			return NotesCollectionList;
		}

		private void FireChangedEvent(string NoteCollectionfile)
		{
			if (NoteCollectionChangedEvent != null)
			{
				NoteCollectionChangedEvent(this,
					new NoteCollectionChangedEventArgs()
					{
						NotesCollectionList = NoteCollectionfile
					});
			}
		}
	}

	public class NoteCollectionChangedEventArgs : EventArgs
	{
		public NoteCollectionChangedEventArgs()
			: base()
		{
		}

		public string NotesCollectionList
		{
			get; set;
		}
	}
}