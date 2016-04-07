using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public partial class MultiSectionSelection : Form
	{
		#region Constructors

		public MultiSectionSelection(List<string> sections, int record, int dwell, CAMRA_Connection fox)
		{
			InitializeComponent();

			DataSet atpts = MultiSelectAttachmentPoints(record, dwell, fox);

			if (atpts.Tables[0].Rows.Count > 0)
			{
				mulattpts.Clear();

				for (int i = 0; i < atpts.Tables[0].Rows.Count; i++)
				{
					DataRow row = mulattpts.NewRow();
					row["Sect"] = atpts.Tables[0].Rows[i]["jlsect"].ToString();
					row["Line"] = Convert.ToInt32(atpts.Tables[0].Rows[i]["jlline#"].ToString());
					row["X1"] = Convert.ToDecimal(atpts.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Y1"] = Convert.ToDecimal(atpts.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["X2"] = Convert.ToDecimal(atpts.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Y2"] = Convert.ToDecimal(atpts.Tables[0].Rows[i]["jlpt2y"].ToString());

					mulattpts.Rows.Add(row);
				}
			}

			SecLetterCbox.DataSource = sections;

			//attsec = sections;

			//SecLetterCbox.DataSource = attsec;

			SecLetterCbox.SelectedIndex = 0;
			adjsec = SecLetterCbox.SelectedItem.ToString();
		}

		#endregion Constructors

		#region Refactored and Class Methods

		private DataSet MultiSelectAttachmentPoints(int record, int dwell, CAMRA_Connection fox)
		{
			_fox = fox;

			mulattpts = new DataTable();
			mulattpts.Columns.Add("Sect", typeof(string));
			mulattpts.Columns.Add("Line", typeof(int));
			mulattpts.Columns.Add("X1", typeof(decimal));
			mulattpts.Columns.Add("Y1", typeof(decimal));
			mulattpts.Columns.Add("X2", typeof(decimal));
			mulattpts.Columns.Add("Y2", typeof(decimal));

			DataSet atpts = null;

			StringBuilder getpts = new StringBuilder();
			getpts.Append(String.Format("select distinct jlsect,jlline#,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach from {0}.{1}line ",
						SketchUpGlobals.LocalLib,
						SketchUpGlobals.LocalityPreFix));
			getpts.Append(String.Format("where jlrecord = {0} and jldwell = {1} and jlattach <> ' ' ",
							record, dwell));

			atpts = fox.DBConnection.RunSelectStatement(getpts.ToString());
			return atpts;
		}

		#endregion Refactored and Class Methods

		#region Event and Control Methods

		private void MultiSectionSelection_FormClosing(object sender, FormClosingEventArgs e)
		{
			adjsec = SecLetterCbox.SelectedItem.ToString();
		}

		private void SecLetterCbox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SecLetterCbox.SelectedIndex > 0)
			{
				adjsec = SecLetterCbox.SelectedItem.ToString();
			}
		}

		#endregion Event and Control Methods

		#region Fields

		public static string adjsec = String.Empty;
		public static List<string> attsec = new List<string>();
		public static DataTable mulattpts = null;

		private CAMRA_Connection _fox = null;

		#endregion Fields
	}
}