using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public partial class SketchLines : Form
	{
		private DataTable Lines = null;

		//   DBAccessManager _fox = null;
		private CAMRA_Connection conn = null;

		public SketchLines(SWallTech.CAMRA_Connection _conn, int _record, int _card, string _section)
		{
			InitializeComponent();

			conn = _conn;

			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (this.Width + 50),
				Screen.PrimaryScreen.WorkingArea.Height - (this.Height + 50));

			Lines = new DataTable();
			Lines.Columns.Add("Section", typeof(string));
			Lines.Columns.Add("Direct", typeof(string));
			Lines.Columns.Add("Length", typeof(string));
			Lines.Columns.Add("X-Len", typeof(decimal));
			Lines.Columns.Add("Y-Len", typeof(decimal));

			StringBuilder getLine = new StringBuilder();
			getLine.Append(String.Format("select jlsect,jldirect,jllinelen,jlxlen,jlylen from {0}.{1}line ", MainForm.FClib, MainForm.FCprefix));
			getLine.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ", _record, _card, _section));

			DataSet ds = conn.DBConnection.RunSelectStatement(getLine.ToString());

			//StringBuilder getLine2 = new StringBuilder();
			//getLine2.Append("select jlsect,jldirect,jllinelen,jlxlen,jlylen from addline ");
			//getLine2.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlsect = '{2}' ", _record, _card, _section));

			//DataSet ds2 = fox.RunSelectStatement(getLine2.ToString());

			if (ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					DataRow row = Lines.NewRow();
					row["Section"] = ds.Tables[0].Rows[i]["jlsect"].ToString();
					row["Direct"] = ds.Tables[0].Rows[i]["jldirect"].ToString();
					string dir = ds.Tables[0].Rows[i]["jldirect"].ToString().TrimEnd();
					if (dir == "N" || dir == "S")
					{
						row["Length"] = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["jlylen"].ToString()), 1).ToString("N1");
					}
					if (dir == "E" || dir == "W")
					{
						row["Length"] = Math.Round(Convert.ToDecimal(ds.Tables[0].Rows[i]["jlxlen"].ToString()), 1).ToString("N1");
					}
					else
					{
						//row["Length"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jllinelen"].ToString());
					}

					string td = ds.Tables[0].Rows[i]["jldirect"].ToString().Trim();

					if (ds.Tables[0].Rows[i]["jldirect"].ToString().Trim().Length == 2)
					{
						row["X-Len"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jlxlen"].ToString());
						row["Y-Len"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jlylen"].ToString());
					}
					else
					{
						row["X-Len"] = 0;
						row["Y-Len"] = 0;
					}

					Lines.Rows.Add(row);
				}
			}

			//if (ds2.Tables[0].Rows.Count > 0)
			//{
			//    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
			//    {
			//        DataRow row = Lines.NewRow();
			//        row["Section"] = ds2.Tables[0].Rows[i]["jlsect"].ToString();
			//        row["Direct"] = ds2.Tables[0].Rows[i]["jldirect"].ToString();
			//        string dir = ds2.Tables[0].Rows[i]["jldirect"].ToString().TrimEnd();
			//        if (dir == "N" || dir == "S")
			//        {
			//            row["Length"] = Math.Round(Convert.ToDecimal(ds2.Tables[0].Rows[i]["jlylen"].ToString()), 1).ToString("N1");
			//        }
			//        if (dir == "E" || dir == "W")
			//        {
			//            row["Length"] = Math.Round(Convert.ToDecimal(ds2.Tables[0].Rows[i]["jlxlen"].ToString()), 1).ToString("N1");
			//        }
			//        else
			//        {
			//            //row["Length"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jllinelen"].ToString());
			//        }

			//        string td = ds2.Tables[0].Rows[i]["jldirect"].ToString().Trim();

			//        if (ds2.Tables[0].Rows[i]["jldirect"].ToString().Trim().Length == 2)
			//        {
			//            row["X-Len"] = Convert.ToDecimal(ds2.Tables[0].Rows[i]["jlxlen"].ToString());
			//            row["Y-Len"] = Convert.ToDecimal(ds2.Tables[0].Rows[i]["jlylen"].ToString());
			//        }
			//        else
			//        {
			//            row["X-Len"] = 0;
			//            row["Y-Len"] = 0;
			//        }

			//        Lines.Rows.Add(row);
			//    }

			//}

			if (Lines.Rows.Count > 0)
			{
				LineDGView.DataSource = Lines;
			}

			DataGridViewColumn sectCol1 = LineDGView.Columns[0];
			DataGridViewColumn directCol2 = LineDGView.Columns[1];
			DataGridViewColumn lenCol3 = LineDGView.Columns[2];
			DataGridViewColumn xlenCol4 = LineDGView.Columns[3];
			DataGridViewColumn ylenCol5 = LineDGView.Columns[4];
			sectCol1.Width = 25;
			directCol2.Width = 35;
			lenCol3.Width = 75;
			xlenCol4.Width = 75;
			ylenCol5.Width = 75;
		}
	}
}