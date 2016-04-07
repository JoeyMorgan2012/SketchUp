using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public partial class SketchSection : Form
	{
		//Refactored stringbuilders to strings and extracted long code runs into separate methods. JMM Feb 2016

		#region Constructor

		public SketchSection(ParcelData currentparcel, CAMRA_Connection _conn, SectionDataCollection currentSection)
		{
			InitializeComponent();

			conn = _conn;

			rowselected = false;

			_currentParcel = currentparcel;
			_currentSection = currentSection;

			while (_isVacant == false)
			{
				if (CamraSupport.VacantOccupancies.Contains(_currentParcel.moccup))
				{
					DialogResult result;
					result = MessageBox.Show("Vacant Occupancy - Continue ?", "Vacant Occupancy Warning", MessageBoxButtons.YesNo);

					if (result == DialogResult.Yes)
					{
						_isVacant = false;
						_currentParcel.moccup = 10;
					}
					else if (result == DialogResult.No)
					{
						_isVacant = true;
						break;
					}
				}
				InitializeComboBoxes(currentparcel);

				Rectangle r = Screen.PrimaryScreen.WorkingArea;
				this.StartPosition = FormStartPosition.CenterScreen;
				this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (this.Width + 50),
					Screen.PrimaryScreen.WorkingArea.Height - (this.Height + 50));

				CreateSectionsDataTable();

				CreateSectionLettersDataTable();

				CreateAttPointsDataTable();

				CreateAttachPointsDataTable();

				CreateLinesDataTable();

				CreateDupAttPointsDataTable();

				if (currentSection.Count > 0)
				{
					AddRowsToSections(currentSection);
				}

				if (Sections.Rows.Count > 0)
				{
					TotalUpSectionValues();
				}

				_isVacant = true;
			}
		}

		#endregion Constructor

		#region Properties

		public static List<string> Letters = new List<string>() { "A", "B", "C", "D", "F", "G", "H", "I", "J", "K", "L", "M" };

		public static string OriginalUnitType = String.Empty;

		public static int subTotal;

		public static int sumBaseValue;

		public static int sumFinalFullValue;
		public static int sumDepreciation;

		public static int sumFactoredValue;

		public static int sumFinalValue;

		public static decimal sumSize;

		public bool _sectDelete = false;

		public int CPcnt = 0;

		public decimal CPSize = 0;

		public int Garcnt = 0;

		public decimal GarSize = 0;

		public int ncp = 0;

		public int ngar = 0;

		public bool rowselected = false;

		public bool typeChange = false;

		public string UCpType = String.Empty;

		public string UGarType = String.Empty;

		private ParcelData _currentParcel = null;

		private SectionDataCollection _currentSection = null;

		private decimal _defDepr = 0;

		private string _Djsect = String.Empty;

		private int _fullvalue = 0;

		private bool _isVacant = false;

		private string _j0depr = String.Empty;

		private string _jclass = String.Empty;

		private decimal _jdeprec = 0;

		private decimal _jfactor = 0;

		private string _jsect = String.Empty;

		private decimal _jssqft = 0;

		private decimal _jstory = 0;

		private string _jstype = String.Empty;

		private int _value = 0;

		private bool _isValid = false;
		private bool alowNoDep = false;

		private DataTable AttachPoints = null;

		private List<string> attList = null;

		private DataTable AttPts = null;

		private int Card = 0;

		private bool _isLoading = false;
		private bool commSection = false;

		private List<string> ComTypes = null;

		private DBAccessManager fox = null;
		private CAMRA_Connection conn = null;

		private List<int> CPCodes = null;

		private List<String> CPTypes = null;

		private int CRindex = 0;

		private List<string> cursectltr = null;

		private DataTable DupAttPoints = null;

		private List<int> GarCodes = null;

		private List<String> GarTypes = null;

		private int index = 0;

		private DataTable Lines = null;

		private string newClass = String.Empty;

		private decimal newDeprec = 0;

		private decimal newFactor = 0;

		private string NewZeroDep = String.Empty;

		private decimal Orig_Sect_sf = 0;

		private decimal OriginalUnitRate = 0;

		private int RateA = 0;

		private int RateB = 0;

		private int RateC = 0;

		private int RateD = 0;

		private decimal Ratepsf = 0;

		private SectionData _sect = null;
		private int Record = 0;

		private bool resSection = false;

		private List<string> ResTypes = null;

		private List<string> secLtrs = null;

		private DataTable SectionLtrs = null;

		private DataTable Sections = null;

		private decimal tempFactor = 0;

		private string typDesc = String.Empty;

		private decimal UnitRate = 0;

		public static string reOpenSecLtr
		{
			get; set;
		}

		public DataGridViewRow SelectSectionDataRow
		{
			get
			{
				if (SectDGView.SelectedRows.Count > 0)
				{
					return SectDGView.SelectedRows[0];
				}
				else
				{
					return null;
				}
			}
		}

		#endregion Properties

		#region Form Control Event Methods

		private void DeleteSectBtn_Click(object sender, EventArgs e)
		{
			DeleteSection();
		}

		private void ReOpenSectionBtn_Click(object sender, EventArgs e)
		{
			int curIndex = SectDGView.CurrentRow.Index;
			if (rowselected == false)
			{
				MessageBox.Show("Must Select Section to Re-Open ");
			}
			if (rowselected == true)
			{
				for (int i = 0; i < SectDGView.CurrentRow.Cells.Count; i++)
				{
					SectDGView.CurrentRow.Cells[i].Style.BackColor = Color.HotPink;
				}

				reOpenSecLtr = String.Empty;

				reOpenSecLtr = SectDGView.CurrentRow.Cells["Section"].Value.ToString().Trim();

				SketchUpGlobals.ReOpenSection = reOpenSecLtr;

				this.Close();
			}
		}

		private void UpdateSectionBtn_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			int curIndex = SectDGView.CurrentRow.Index;

			if (rowselected == false)
			{
				MessageBox.Show("Must Select Row to Update & make Change");
				return;
			}
			else

			{
				for (int i = 0; i < SectDGView.CurrentRow.Cells.Count; i++)
				{
					SectDGView.CurrentRow.Cells[i].Style.BackColor = Color.Salmon;
				}
				string TypeText = String.Empty;

				string TypeDesc = String.Empty;
				int substart = 0;
				string upDateSection = String.Empty;
				string upDateSectionLtr = String.Empty;
				decimal newStory = 0;
				decimal newSize = 0;
				string newClass = String.Empty;
				string new0Depr = String.Empty;
				decimal newFactor = 0;
				decimal newDeprec = 0;

				upDateSectionLtr = SectDGView.CurrentRow.Cells["Section"].Value.ToString();

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					if (ResidentialSectionCbox.SelectedIndex > 0)
					{
						if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
						{
							substart = ResidentialSectionCbox.SelectedItem.ToString().IndexOf("-", StringComparison.CurrentCulture);

							TypeText = ResidentialSectionCbox.SelectedItem.ToString().Substring(0, (substart - 1)).Trim();

							TypeDesc = ResidentialSectionCbox.SelectedItem.ToString().Substring(substart + 1).Trim();

							upDateSection = TypeText;

							SectDGView.CurrentRow.Cells["Type"].Value = upDateSection.Trim();
							SectDGView.CurrentRow.Cells["Desc"].Value = TypeDesc.Trim();
						}
					}
					if (ResidentialSectionCbox.SelectedIndex == 0)
					{
						upDateSectionLtr = SectDGView.CurrentRow.Cells["Section"].Value.ToString().Trim();
						upDateSection = SectDGView.CurrentRow.Cells["Type"].Value.ToString().Trim();
						TypeDesc = SectDGView.CurrentRow.Cells["Desc"].Value.ToString().Trim();
					}

					UpdateGAR_CP();
				}

				if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
				{
					if (CommercialSectionCbox.SelectedIndex > 0)
					{
						if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
						{
							substart = CommercialSectionCbox.SelectedItem.ToString().IndexOf("-", StringComparison.CurrentCulture);

							TypeText = CommercialSectionCbox.SelectedItem.ToString().Substring(0, (substart - 1)).Trim();

							TypeDesc = CommercialSectionCbox.SelectedItem.ToString().Substring(substart + 1).Trim();
							upDateSection = TypeText;

							SectDGView.CurrentRow.Cells["Type"].Value = upDateSection.Trim();
							SectDGView.CurrentRow.Cells["Desc"].Value = TypeDesc.Trim();
						}
					}
					if (CommercialSectionCbox.SelectedIndex == 0)
					{
						upDateSectionLtr = SectDGView.CurrentRow.Cells["Section"].Value.ToString().Trim();
						upDateSection = SectDGView.CurrentRow.Cells["Type"].Value.ToString().Trim();
						TypeDesc = SectDGView.CurrentRow.Cells["Desc"].Value.ToString().Trim();
					}
				}

				string curSecLtr = SectDGView.CurrentRow.Cells["Section"].Value.ToString().Trim();

				newStory = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Story"].Value.ToString());

				if (newStory == _currentParcel.orig_mstorN && upDateSectionLtr == "A")
				{
					string NewSLSF = String.Empty;

					if (_currentParcel.orig_mstory != string.Empty && newStory == 1)
					{
						NewSLSF = _currentParcel.orig_mstory;
					}
					StringBuilder fixstory = new StringBuilder();
					fixstory.Append(String.Format("update {0}.{1}mast set mstor# = {2},mstory = '{3}' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									newStory,
									NewSLSF));
					fixstory.Append(String.Format("where mrecno = {0} and mdwell = {1} ",
									_currentParcel.mrecno,
									_currentParcel.mdwell));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixstory", fixstory);
					conn.DBConnection.ExecuteNonSelectStatement(fixstory.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixstory", fixstory);
					StringBuilder fixsecstry = new StringBuilder();
					fixsecstry.Append(String.Format("update {0}.{1}section set jsstory = {2},jstype = '{3}' where jsrecord = {4} and jsdwell = {5} and jssect = 'A' ",
								SketchUpGlobals.FcLib,
								SketchUpGlobals.FcLocalityPrefix,
								newStory,
								upDateSection,
								_currentParcel.mrecno,
								_currentParcel.mdwell));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
					conn.DBConnection.ExecuteNonSelectStatement(fixsecstry.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);

					StringBuilder fixSketMaster = new StringBuilder();
					fixSketMaster.Append(String.Format("update {0}.{1}master set jmstory = {2},jmtotsqft = {3} ",
								SketchUpGlobals.FcLib,
								SketchUpGlobals.FcLocalityPrefix,
								newStory,
								SketchSection.sumSize));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixSketMaster", fixSketMaster);
					conn.DBConnection.ExecuteNonSelectStatement(fixSketMaster.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixSketMaster", fixSketMaster);
				}
				if (upDateSectionLtr != "A")
				{
					StringBuilder fixsecstry = new StringBuilder();
					fixsecstry.Append(String.Format("update {0}.{1}section set jsstory = {2},jstype = '{3}' where jsrecord = {4} and jsdwell = {5} and jssect = '{6}' ",
								SketchUpGlobals.FcLib,
								SketchUpGlobals.FcLocalityPrefix,
								newStory,
								upDateSection,
								_currentParcel.mrecno,
								_currentParcel.mdwell,
								upDateSectionLtr.Trim()));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
					conn.DBConnection.ExecuteNonSelectStatement(fixsecstry.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
				}

				if (newStory != _currentParcel.orig_mstorN && upDateSectionLtr == "A")
				{
					DialogResult storyresult;
					storyresult = MessageBox.Show("Do You Want to change Master Stories ?", "Master Story Difference", MessageBoxButtons.YesNo);

					if (storyresult == DialogResult.Yes)
					{
						_currentParcel.mstorN = newStory;
						string NewSLSF = String.Empty;

						if (_currentParcel.orig_mstory != string.Empty && newStory == 1)
						{
							NewSLSF = _currentParcel.orig_mstory;
						}
						StringBuilder fixstory = new StringBuilder();
						fixstory.Append(String.Format("update {0}.{1}mast set mstor# = {2},mstory = '{3}' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									newStory,
									NewSLSF));
						fixstory.Append(String.Format("where mrecno = {0} and mdwell = {1} ",
									_currentParcel.mrecno,
									_currentParcel.mdwell));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixstory", fixstory);
						conn.DBConnection.ExecuteNonSelectStatement(fixstory.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixstory", fixstory);
						StringBuilder fixsecstry = new StringBuilder();
						fixsecstry.Append(String.Format("update {0}.{1}section set jsstory = {2} where jsrecord = {3} and jsdwell = {4} and jssect = 'A' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									newStory,
									_currentParcel.mrecno,
									_currentParcel.mdwell));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
						conn.DBConnection.ExecuteNonSelectStatement(fixsecstry.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);

						StringBuilder fixSketMaster = new StringBuilder();
						fixSketMaster.Append(String.Format("update {0}.{1}master set jmstory = {2},jmtotsqft = {3} ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									newStory,
									SketchSection.sumSize));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixSketMaster", fixSketMaster);
						conn.DBConnection.ExecuteNonSelectStatement(fixSketMaster.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixSketMaster", fixSketMaster);
					}
					if (storyresult == DialogResult.No)
					{
						StringBuilder fixsecstry = new StringBuilder();
						fixsecstry.Append(String.Format("update {0}.{1}section set jsstory = {2} where jsrecord = {3} and jsdwell = {4} and jssect = 'A' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									_currentParcel.orig_mstorN,
									_currentParcel.mrecno,
									_currentParcel.mdwell));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
						conn.DBConnection.ExecuteNonSelectStatement(fixsecstry.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixsecstry", fixsecstry);
					}
				}

				newSize = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Size"].Value.ToString());
				newClass = SectDGView.CurrentRow.Cells["Class"].Value.ToString();
				new0Depr = SectDGView.CurrentRow.Cells["0Depr"].Value.ToString();

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup) && Convert.ToDecimal(SectDGView.CurrentRow.Cells["Factor"].Value.ToString()) != 0)
				{
					MessageBox.Show("Section Factor for Residential Must Be Zero!");

					SectDGView.CurrentRow.Cells["Factor"].Value = 0;
					newFactor = 0;
				}
				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup) && Convert.ToDecimal(SectDGView.CurrentRow.Cells["Deprec"].Value.ToString()) != 0)
				{
					MessageBox.Show("Section Depreciation for Residential Must Be Zero!");

					SectDGView.CurrentRow.Cells["Deprec"].Value = 0;
					newDeprec = 0;
				}

				if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
				{
					newFactor = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Factor"].Value.ToString());
					newDeprec = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Deprec"].Value.ToString());

					StringBuilder chgSec = new StringBuilder();
					chgSec.Append(String.Format("update {0}.{1}section set jstype = '{2}', jsstory = {3}, jssqft = {4},js0depr = '{5}', ",
								SketchUpGlobals.FcLib,
								SketchUpGlobals.FcLocalityPrefix,
								upDateSection.Trim(),
								newStory,
								newSize,
								new0Depr.Trim()));
					chgSec.Append(String.Format("jsclass = '{0}', jsfactor = {1}, jsdeprc = {2} ",
								newClass,
								newFactor,
								newDeprec));
					chgSec.Append(String.Format("where jsrecord = {0} and jsdwell = {1} and jssect = '{2}' ",
									_currentParcel.Record,
									_currentParcel.Card,
									curSecLtr.Trim()));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "chgSec", chgSec);
					conn.DBConnection.ExecuteNonSelectStatement(chgSec.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "chgSec", chgSec);
				}
			}

			Cursor = Cursors.Default;

			Reorder();
		}

		#endregion Form Control Event Methods

		#region Refactored Methods

		public void setAttPnts()
		{
			string attPnts = string.Format("select jlrecord,jldwell,jlsect,jldirect,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 and jlsect <> 'A' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.Record, _currentParcel.Card);
			DataSet ap = conn.DBConnection.RunSelectStatement(attPnts);

			if (ap.Tables[0].Rows.Count > 0)
			{
				AttachPoints.Clear();
				for (int i = 0; i < ap.Tables[0].Rows.Count; i++)
				{
					DataRow row = AttachPoints.NewRow();
					row["RecNo"] = _currentParcel.Record;
					row["CardNo"] = _currentParcel.Card;
					row["Sect"] = ap.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["Direct"] = ap.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xpt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2y"].ToString());
					row["Attch"] = ap.Tables[0].Rows[i]["jlattach"].ToString().Trim();

					AttachPoints.Rows.Add(row);
				}
			}

			if (AttachPoints.Rows.Count > 0)
			{
				DeleteAttachmentPoints();

				AddAttachmentPoints();
			}

			string DupattPnts = string.Format("select jlrecord,jldwell,jlsect,jldirect,jlline#,jlpt1x,jlpt1y,jlpt2x,jlpt2y,jlattach from {0}.{1}line  where jlrecord = {2} and jldwell = {3} and jlattach <> ' ' order by jlattach,jlsect ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.Record, _currentParcel.Card);

			DataSet Dupap = conn.DBConnection.RunSelectStatement(DupattPnts);

			if (Dupap.Tables[0].Rows.Count > 0)
			{
				DuplicateAttachementPoints(Dupap);
			}

			attList = new List<string>();
			attList.Clear();

			string sortAtt = string.Format("select jlattach from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlattach <> ' ' ",
						 SketchUpGlobals.FcLib,
						 SketchUpGlobals.FcLocalityPrefix,
						 _currentParcel.Record,
						 _currentParcel.Card);

			DataSet sa = conn.DBConnection.RunSelectStatement(sortAtt.ToString());

			if (sa.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < sa.Tables[0].Rows.Count; i++)
				{
					attList.Add(sa.Tables[0].Rows[i]["jlattach"].ToString().Trim());
				}
			}

			string delAtt = string.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} ",
						 SketchUpGlobals.FcLib,
						 SketchUpGlobals.FcLocalityPrefix,
						 _currentParcel.Record,
						 _currentParcel.Card);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "delAtt", delAtt);
			conn.DBConnection.ExecuteNonSelectStatement(delAtt.ToString());

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "delAtt", delAtt);
			if (AttPts.Rows.Count > 0)
			{
				for (int i = 0; i < AttPts.Rows.Count; i++)
				{
					int record = _currentParcel.Record;
					int card = _currentParcel.Card;
					string curSection = AttPts.Rows[i]["Sect"].ToString().Trim();
					decimal X1 = Convert.ToDecimal(AttPts.Rows[i]["X1"].ToString());
					decimal Y1 = Convert.ToDecimal(AttPts.Rows[i]["Y1"].ToString());
					decimal X2 = Convert.ToDecimal(AttPts.Rows[i]["X2"].ToString());
					decimal Y2 = Convert.ToDecimal(AttPts.Rows[i]["Y2"].ToString());

					// MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} atg 4492", _currentParcel.Record, _currentParcel.Card));

					StringBuilder addAttPnt1 = new StringBuilder();
					addAttPnt1.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									curSection));
					addAttPnt1.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt1x = {2} and jlpt1y = {3} ",
									record,
									card,
									X2,
									Y2));
					addAttPnt1.Append(String.Format(" and jlpt2x = {0} and jlpt2y = {1} ", X1, Y1));
					addAttPnt1.Append(String.Format(" and jlsect = '{0}' ", curSection));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addAttPnt1", addAttPnt1);
					conn.DBConnection.ExecuteNonSelectStatement(addAttPnt1.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addAttPnt1", addAttPnt1);
				}
			}

			if (DupAttPoints.Rows.Count > 0)
			{
				/// check out points in AttachPoints

				for (int i = 0; i < DupAttPoints.Rows.Count; i++)
				{
					int record = _currentParcel.Record;
					int card = _currentParcel.Card;
					string curSect = DupAttPoints.Rows[i]["Attch"].ToString().Trim();
					string curSection = DupAttPoints.Rows[i]["Sect"].ToString().Trim();
					decimal X1 = Convert.ToDecimal(DupAttPoints.Rows[i]["Xpt1"].ToString());
					decimal Y1 = Convert.ToDecimal(DupAttPoints.Rows[i]["Ypt1"].ToString());
					decimal X2 = Convert.ToDecimal(DupAttPoints.Rows[i]["Xpt2"].ToString());
					decimal Y2 = Convert.ToDecimal(DupAttPoints.Rows[i]["Ypt2"].ToString());

					if (Convert.ToInt32(DupAttPoints.Rows[i]["Index"].ToString()) == 1)
					{
						//MessageBox.Show(String.Format("Updating Line Record - {0}, Card - {1} at 4527", _currentParcel.Record, _currentParcel.Card));

						StringBuilder addAttPnt = new StringBuilder();
						addAttPnt.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ",
										SketchUpGlobals.FcLib,
										SketchUpGlobals.FcLocalityPrefix,
										curSect));
						addAttPnt.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt1x = {2} and jlpt1y = {3} ", record, card, X1, Y1));
						addAttPnt.Append(String.Format(" and jlpt2x = {0} and jlpt2y = {1} ", X2, Y2));

						//addAttPnt.Append(String.Format(" and jlsect <> '{0}' and jlsect = '{1}' ", curSect, CurrentSecLtr));
						addAttPnt.Append(String.Format(" and jlsect = '{0}' ", curSection));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
						conn.DBConnection.ExecuteNonSelectStatement(addAttPnt.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
					}
				}
			}
		}

		private void _getDefaultDepr(decimal secDepr, string NoDepr)
		{
			if (secDepr == 0 && NoDepr != "Y")
			{
				int baseAge = 0;
				if (_currentParcel.meffag == 0)
				{
					baseAge = _currentParcel.mage;
				}
				if (_currentParcel.meffag != 0)
				{
					baseAge = _currentParcel.meffag;
				}
				if (_currentParcel.mcond == "G")
				{
					_defDepr = Decimal.Round((Convert.ToDecimal(baseAge) * CamraSupport.DefDepCondG), 2);
				}
				if (_currentParcel.mcond == "A")
				{
					_defDepr = Decimal.Round((Convert.ToDecimal(baseAge) * CamraSupport.DefDepCondA), 2);
				}
				if (_currentParcel.mcond == "F")
				{
					_defDepr = Decimal.Round((Convert.ToDecimal(baseAge) * CamraSupport.DefDepCondF), 2);
				}
				if (_currentParcel.mcond == "P")
				{
					_defDepr = Decimal.Round((Convert.ToDecimal(baseAge) * CamraSupport.DefDepCondP), 2);
				}
			}
			if (secDepr == 0 && NoDepr == "Y")
			{
				_defDepr = 0;
			}

			if (_defDepr > 0.65m)
			{
				_defDepr = 0.65m;
			}
		}

		private void _getUnitRate(string type, string _class)
		{
			if (rowselected == true)
			{
				CRindex = SectDGView.CurrentRow.Index;

				OriginalUnitRate = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Rate"].Value.ToString());
			}

			GetRatesFromTables(type);

			SetResOrCommValues(type);

			ApplyRates(_class);
		}

		private void _getUnitValue(string NoDepr, decimal _size, decimal _factor, decimal _deprc)
		{
			if (NoDepr == "Y")
			{
				_value = Convert.ToInt32((_size * UnitRate) * (1 + _factor));
				_fullvalue = _value;
			}
			else
			{
				_value = Convert.ToInt32((_size * UnitRate) * (1 + _factor) * (1 - _deprc));
				_fullvalue = Convert.ToInt32((_size * UnitRate) * (1 + _factor));
			}
		}

		private void AddAttachmentPoints()
		{
			for (int i = 0; i < AttachPoints.Rows.Count; i++)
			{
				int record = Convert.ToInt32(AttachPoints.Rows[i]["RecNo"].ToString());
				int card = Convert.ToInt32(AttachPoints.Rows[i]["CardNo"].ToString());
				string curSect = AttachPoints.Rows[i]["Sect"].ToString().Trim();
				decimal X1 = Convert.ToDecimal(AttachPoints.Rows[i]["Xpt1"].ToString());
				decimal Y1 = Convert.ToDecimal(AttachPoints.Rows[i]["Ypt1"].ToString());

				string addAttPnt = string.Format("update {0}.{1}line set jlattach = '{2}' where jlrecord = {3} and jldwell = {4} and jlpt2x = {5} and jlpt2y = {6}  and jlsect <> '{7}' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, curSect, record, card, X1, Y1, curSect);

				//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
				conn.DBConnection.ExecuteNonSelectStatement(addAttPnt);

				//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
			}
		}

		private void AddNewTypeToCommercialTypes(string newType, out string newDesc, out int thisSect, string curSect, out int cx)
		{
			cx = ComTypes.IndexOf(newType);

			thisSect = ComTypes.IndexOf(curSect.Trim());

			CommercialSectionCbox.SelectedIndex = thisSect + 1;

			newDesc = CamraSupport.CommercialSectionTypeCollection[cx]._commSectionDescription.ToString().Trim();
		}

		private void AddNewTypeToResidentialTypes(string newType, out string newDesc, out int thisSect, string curSect, out int rx)
		{
			rx = ResTypes.IndexOf(newType);

			thisSect = ResTypes.IndexOf(curSect.Trim());

			ResidentialSectionCbox.SelectedIndex = thisSect + 1;

			newDesc = CamraSupport.ResidentialSectionTypeCollection[rx]._resSectionDescription.ToString().Trim();
		}

		private void AddRowsToSections(SectionDataCollection currentSection)
		{
			for (int i = 0; i < currentSection.Count; i++)
			{
				string getDesc = string.Format("select rclar,rclbr,rclcr,rcldr,rrpsf,rdesc from {0}.{1}rat1 where rid in ( 'C','P') and rsecto = '{2}' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, currentSection[i].jstype.ToString().Trim());

				DataSet rates = conn.DBConnection.RunSelectStatement(getDesc);
				_getUnitRate(currentSection[i].jstype, currentSection[i].jsclass);

				DataRow row = Sections.NewRow();
				row["Section"] = currentSection[i].jssect.ToString();
				row["Type"] = currentSection[i].jstype.ToString();
				row["Desc"] = typDesc.Trim();
				row["Story"] = Convert.ToDecimal(currentSection[i].jsstory.ToString());
				row["Size"] = Convert.ToDecimal(currentSection[i].jssqft.ToString());
				row["0Depr"] = currentSection[i].js0depr.ToString();
				row["Class"] = currentSection[i].jsclass.ToString();
				row["Factor"] = Convert.ToDecimal(currentSection[i].jsfactor.ToString());
				row["Deprec"] = Convert.ToDecimal(currentSection[i].jsdeprc.ToString());
				row["Rate"] = UnitRate;

				_getUnitValue(currentSection[i].js0depr, currentSection[i].jssqft, currentSection[i].jsfactor, currentSection[i].jsdeprc);

				row["Value"] = _value;
				row["NewValue"] = _fullvalue;

				Sections.Rows.Add(row);
			}
		}

		private void AdjustAttachments()
		{
			StringBuilder delAtt = new StringBuilder();
			delAtt.Append(String.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} ",
							SketchUpGlobals.FcLib,
							SketchUpGlobals.FcLocalityPrefix,
							_currentParcel.Record,
							_currentParcel.Card));

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "delAtt", delAtt);
			conn.DBConnection.ExecuteNonSelectStatement(delAtt.ToString());

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "delAtt", delAtt);
			StringBuilder attPnts = new StringBuilder();
			attPnts.Append(String.Format("select jlrecord,jldwell,jlsect,jldirect,jlpt1x,jlpt1y,jlpt2x,jlpt2y from {0}.{1}line ",
								SketchUpGlobals.FcLib,
								SketchUpGlobals.FcLocalityPrefix));
			attPnts.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlline# = 1 ",
								_currentParcel.Record,
								_currentParcel.Card));

			DataSet ap = conn.DBConnection.RunSelectStatement(attPnts.ToString());

			if (ap.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ap.Tables[0].Rows.Count; i++)
				{
					DataRow row = AttachPoints.NewRow();
					row["RecNo"] = _currentParcel.Record;
					row["CardNo"] = _currentParcel.Card;
					row["Sect"] = ap.Tables[0].Rows[i]["jlsect"].ToString().Trim();
					row["Direct"] = ap.Tables[0].Rows[i]["jldirect"].ToString().Trim();
					row["Xpt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1x"].ToString());
					row["Ypt1"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt1y"].ToString());
					row["Xpt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2x"].ToString());
					row["Ypt2"] = Convert.ToDecimal(ap.Tables[0].Rows[i]["jlpt2y"].ToString());

					AttachPoints.Rows.Add(row);
				}
			}

			if (AttachPoints.Rows.Count > 0)
			{
				for (int i = 0; i < AttachPoints.Rows.Count; i++)
				{
					int record = Convert.ToInt32(AttachPoints.Rows[i]["RecNo"].ToString());
					int card = Convert.ToInt32(AttachPoints.Rows[i]["CardNo"].ToString());
					string curSect = AttachPoints.Rows[i]["Sect"].ToString().Trim();
					decimal X1 = Convert.ToDecimal(AttachPoints.Rows[i]["Xpt1"].ToString());
					decimal Y1 = Convert.ToDecimal(AttachPoints.Rows[i]["Ypt1"].ToString());

					StringBuilder addAttPnt = new StringBuilder();
					addAttPnt.Append(String.Format("update {0}.{1}line set jlattach = '{2}' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix, curSect));
					addAttPnt.Append(String.Format(" where jlrecord = {0} and jldwell = {1} and jlpt2x = {2} and jlpt2y = {3} ",
									record,
									card,
									X1,
									Y1));
					addAttPnt.Append(String.Format(" and jlsect <> '{0}' ", curSect));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
					conn.DBConnection.ExecuteNonSelectStatement(addAttPnt.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addAttPnt", addAttPnt);
				}
			}
		}

		private void ApplyRates(string _class)
		{
			if (_class == "A" && commSection == true)
			{
				UnitRate = Convert.ToDecimal(RateA);
			}
			if (_class == "B" && commSection == true)
			{
				UnitRate = Convert.ToDecimal(RateB);
			}
			if (_class == "C" && commSection == true)
			{
				UnitRate = Convert.ToDecimal(RateC);
			}
			if (_class == "D" && commSection == true)
			{
				UnitRate = Convert.ToDecimal(RateD);
			}
			if (resSection == true)
			{
				UnitRate = Ratepsf;
			}
			if (resSection == true && UnitRate == 0)
			{
				UnitRate = _currentParcel.mpsf;
			}
		}

		private void CountCP(string garcptype)
		{
			ncp = 0;

			string cntCP = string.Format("select count(*) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jstype = '{4}' ",
							SketchUpGlobals.LocalLib,
							SketchUpGlobals.LocalityPreFix,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							garcptype.Trim());

			ncp = Convert.ToInt32(conn.DBConnection.ExecuteScalar(cntCP));

			if (ncp == 1)
			{
				try
				{
					if (CPTypes.Contains(OriginalUnitType.Trim()))
					{
						string fixCP = string.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
							 SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell);

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCP", fixCP);
						conn.DBConnection.ExecuteNonSelectStatement(fixCP);

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCP", fixCP);
						_currentParcel.mcarpt = 67;
						_currentParcel.mcarNc = 0;
					}
				}
				catch
				{
				}
			}
		}

		private void CountGar(string garcptype)
		{
			ngar = 1;

			string cntgar = string.Format("select mgart2 from {0}.{1}mast where mrecno = {2} and mdwell = {3} ",
									SketchUpGlobals.LocalLib,
									SketchUpGlobals.LocalityPreFix,
									_currentParcel.mrecno,
									_currentParcel.mdwell);

			int secgar = Convert.ToInt32(conn.DBConnection.ExecuteScalar(cntgar));

			if (secgar != 64 && secgar > 0)
			{
				ngar = 2;
			}

			if (ngar == 1)
			{
				try
				{
					if (GarTypes.Contains(OriginalUnitType.Trim()))
					{
						string fixGar = string.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0 where mrecno = {2} and mdwell = {3} ",
							SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell);

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixGar (ngar == 1)", fixGar);
						conn.DBConnection.ExecuteNonSelectStatement(fixGar.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixGar", fixGar);
						_currentParcel.mgart = 63;
						_currentParcel.mgarNc = 0;
					}
				}
				catch
				{
				}
			}
			if (ngar == 2)
			{
				string fixGar = string.Format("update {0}.{1}mast set mgart2 = 0, mgar#2 = 0 where mrecno = {2} and mdwell = {3} ",
					SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell);

				//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixGar (ngar == 2)", fixGar);
				conn.DBConnection.ExecuteNonSelectStatement(fixGar.ToString());

				//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixGar", fixGar);
				_currentParcel.mgart2 = 0;
				_currentParcel.mgarN2 = 0;
			}
		}

		private void CreateAttachPointsDataTable()
		{
			AttachPoints = new DataTable();
			AttachPoints.Columns.Add("RecNo", typeof(int));
			AttachPoints.Columns.Add("CardNo", typeof(int));
			AttachPoints.Columns.Add("Sect", typeof(string));
			AttachPoints.Columns.Add("Direct", typeof(string));
			AttachPoints.Columns.Add("Xpt1", typeof(decimal));
			AttachPoints.Columns.Add("Ypt1", typeof(decimal));
			AttachPoints.Columns.Add("Xpt2", typeof(decimal));
			AttachPoints.Columns.Add("Ypt2", typeof(decimal));
			AttachPoints.Columns.Add("Attch", typeof(string));
		}

		private void CreateAttPointsDataTable()
		{
			AttPts = new DataTable();
			AttPts.Columns.Add("Sect", typeof(string));
			AttPts.Columns.Add("X1", typeof(decimal));
			AttPts.Columns.Add("Y1", typeof(decimal));
			AttPts.Columns.Add("X2", typeof(decimal));
			AttPts.Columns.Add("Y2", typeof(decimal));
		}

		private void CreateDupAttPointsDataTable()
		{
			DupAttPoints = new DataTable();
			DupAttPoints.Columns.Add("RecNo", typeof(int));
			DupAttPoints.Columns.Add("CardNo", typeof(int));
			DupAttPoints.Columns.Add("Sect", typeof(string));
			DupAttPoints.Columns.Add("LineNo", typeof(int));
			DupAttPoints.Columns.Add("Direct", typeof(string));
			DupAttPoints.Columns.Add("Xpt1", typeof(decimal));
			DupAttPoints.Columns.Add("Ypt1", typeof(decimal));
			DupAttPoints.Columns.Add("Xpt2", typeof(decimal));
			DupAttPoints.Columns.Add("Ypt2", typeof(decimal));
			DupAttPoints.Columns.Add("Attch", typeof(string));
			DupAttPoints.Columns.Add("Index", typeof(int));
		}

		private void CreateLinesDataTable()
		{
			Lines = new DataTable();
			Lines.Columns.Add("RecNo", typeof(int));
			Lines.Columns.Add("CardNo", typeof(int));
			Lines.Columns.Add("Sect", typeof(string));
			Lines.Columns.Add("LineNo", typeof(int));
			Lines.Columns.Add("Direct", typeof(string));
			Lines.Columns.Add("Xlen", typeof(decimal));
			Lines.Columns.Add("Ylen", typeof(decimal));
			Lines.Columns.Add("Length", typeof(decimal));
			Lines.Columns.Add("Angle", typeof(decimal));
			Lines.Columns.Add("Xpt1", typeof(decimal));
			Lines.Columns.Add("Ypt1", typeof(decimal));
			Lines.Columns.Add("Xpt2", typeof(decimal));
			Lines.Columns.Add("Ypt2", typeof(decimal));
			Lines.Columns.Add("Attach", typeof(string));
		}

		private void CreateSectionLettersDataTable()
		{
			SectionLtrs = new DataTable();
			SectionLtrs.Columns.Add("RecNo", typeof(int));
			SectionLtrs.Columns.Add("CardNo", typeof(int));
			SectionLtrs.Columns.Add("CurSecLtr", typeof(string));
			SectionLtrs.Columns.Add("NewSecLtr", typeof(string));
			SectionLtrs.Columns.Add("NewType", typeof(string));
			SectionLtrs.Columns.Add("SectSize", typeof(decimal));
		}

		private void CreateSectionsDataTable()
		{
			Sections = new DataTable();
			Sections.Columns.Add("Section", typeof(string));
			Sections.Columns.Add("Type", typeof(string));
			Sections.Columns.Add("Desc", typeof(string));
			Sections.Columns.Add("Story", typeof(decimal));
			Sections.Columns.Add("Size", typeof(decimal));
			Sections.Columns.Add("0Depr", typeof(string));
			Sections.Columns.Add("Class", typeof(string));
			Sections.Columns.Add("Factor", typeof(decimal));
			Sections.Columns.Add("Deprec", typeof(decimal));
			Sections.Columns.Add("Rate", typeof(decimal));
			Sections.Columns.Add("Value", typeof(int));
			Sections.Columns.Add("NewValue", typeof(int));
		}

		private void DeleteAttachmentPoints()
		{
			string delapts = string.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} and jlattach <> ' ' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "delapts", delapts);
			conn.DBConnection.ExecuteNonSelectStatement(delapts.ToString());

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "delapts", delapts);
		}

		private void DeleteSection()
		{
			int curIndex = SectDGView.CurrentRow.Index;

			_sectDelete = true;

			string dType = SectDGView.Rows[curIndex].Cells["Type"].Value.ToString();

			for (int i = 0; i < SectDGView.CurrentRow.Cells.Count; i++)
			{
				SectDGView.CurrentRow.Cells[i].Style.BackColor = Color.Salmon;
			}

			string garcp = string.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' and rrpsf <> 0 ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix);

			try
			{
				DataSet ds = conn.DBConnection.RunSelectStatement(garcp);

				if (ds.Tables[0].Rows.Count > 0)
				{
					GarTypes = new List<string>();
					for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
					{
						string sect = ds.Tables[0].Rows[i]["rsecto"].ToString().Trim();

						GarTypes.Add(sect);
					}
				}
			}
			catch
			{
			}

			string cptype = string.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rrpsf <> 0 ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix);

			try
			{
				DataSet ds1 = conn.DBConnection.RunSelectStatement(cptype);

				if (ds1.Tables[0].Rows.Count > 0)
				{
					CPTypes = new List<string>();
					for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
					{
						string sect = ds1.Tables[0].Rows[i]["rsecto"].ToString().Trim();

						CPTypes.Add(sect);
					}
				}
			}
			catch
			{
			}
			string garcode = string.Format("select ttelem from {0}.{1}stab where ttid = 'GAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix);

			try
			{
				DataSet gc = conn.DBConnection.RunSelectStatement(garcode);

				if (gc.Tables[0].Rows.Count > 0)
				{
					GarCodes = new List<int>();
					for (int i = 0; i < gc.Tables[0].Rows.Count; i++)
					{
						int gcode = Convert.ToInt32(gc.Tables[0].Rows[i]["ttelem"].ToString());

						GarCodes.Add(gcode);
					}
				}
			}
			catch
			{
			}

			string cpcode = string.Format("select ttelem from {0}.{1}stab where ttid = 'CAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix);

			try
			{
				DataSet cp = conn.DBConnection.RunSelectStatement(cpcode);

				if (cp.Tables[0].Rows.Count > 0)
				{
					CPCodes = new List<int>();
					for (int i = 0; i < cp.Tables[0].Rows.Count; i++)
					{
						int cpcodeX = Convert.ToInt32(cp.Tables[0].Rows[i]["ttelem"].ToString());

						CPCodes.Add(cpcodeX);
					}
				}
			}
			catch
			{
			}

			try
			{
				if (GarTypes.Contains(dType))
				{
					//TODO: See if this can be refactored later to pass in the mgart and mgarNc values and use a single method for garages.
					string fixGar = string.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0 where mrecno = {2} and mdwell = {3} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell);

					conn.DBConnection.ExecuteNonSelectStatement(fixGar);

					_currentParcel.mgart = 63;
					_currentParcel.mgarNc = 0;
				}
			}
			catch
			{
			}
			try
			{
				if (CPTypes.Contains(dType))
				{
					//TODO: Look for refacroting opportunity by passing in mcarpt and mcarNc
					StringBuilder fixCP = new StringBuilder();
					fixCP.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
						 SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCP", fixCP);
					conn.DBConnection.ExecuteNonSelectStatement(fixCP.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCP", fixCP);
					_currentParcel.mcarpt = 67;
					_currentParcel.mcarNc = 0;
				}
			}
			catch
			{
			}

			_Djsect = SectDGView.Rows[curIndex].Cells["Section"].Value.ToString();

			if (_Djsect == String.Empty)
			{
				_Djsect = "X";
			}

			if (_Djsect != "X")
			{
				DialogResult DelSect;
				DelSect = (MessageBox.Show(String.Format("Do you want to Delect Section - '{0}'", _Djsect), "Delete Section Warning",
					MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

				if (DelSect == DialogResult.No)
				{
					Close();
				}
				if (DelSect == DialogResult.Yes)
				{
					StringBuilder chkeaccess = new StringBuilder();

					chkeaccess.Append(String.Format("select mlnam from {0}.{1}mast where mrecno = {2} and mdwell = {3} ",
									SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell));

					string testename = conn.DBConnection.ExecuteScalar(chkeaccess.ToString()).ToString().Trim();

					StringBuilder delLine = new StringBuilder();
					delLine.Append(String.Format("delete from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlsect = '{4}'",
									 SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.Record, _currentParcel.Card, _Djsect));

					conn.DBConnection.ExecuteNonSelectStatement(delLine.ToString());

					StringBuilder delSect = new StringBuilder();
					delSect.Append(String.Format("delete from {0}.{1}section where jsrecord = {2} and jsdwell = {3} and jssect = '{4}'",
									SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.Record, _currentParcel.Card, _Djsect));

					conn.DBConnection.ExecuteNonSelectStatement(delSect.ToString());

					StringBuilder delAttc = new StringBuilder();
					delAttc.Append(String.Format("update {0}.{1}line set jlattach = ' ' where jlrecord = {2} and jldwell = {3} and jlattach = '{4}' ",
									SketchUpGlobals.FcLib,
									SketchUpGlobals.FcLocalityPrefix,
									_currentParcel.Record,
									_currentParcel.Card,
									_Djsect));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "delAttc", delAttc);
					conn.DBConnection.ExecuteNonSelectStatement(delAttc.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "delAttc", delAttc);
					Reorder();

					ResetSect();

					Close();
				}
			}

			_sectDelete = false;
		}

		private void DuplicateAttachementPoints(DataSet Dupap)
		{
			DupAttPoints.Clear();

			for (int j = 0; j < Dupap.Tables[0].Rows.Count; j++)
			{
				DataRow row = DupAttPoints.NewRow();
				row["RecNo"] = _currentParcel.Record;
				row["CardNo"] = _currentParcel.Card;
				row["Sect"] = Dupap.Tables[0].Rows[j]["jlsect"].ToString().Trim();
				row["LineNo"] = Convert.ToInt32(Dupap.Tables[0].Rows[j]["jlline#"].ToString());
				row["Direct"] = Dupap.Tables[0].Rows[j]["jldirect"].ToString().Trim();
				row["Xpt1"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt1x"].ToString());
				row["Ypt1"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt1y"].ToString());
				row["Xpt2"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt2x"].ToString());
				row["Ypt2"] = Convert.ToDecimal(Dupap.Tables[0].Rows[j]["jlpt2y"].ToString());
				row["Attch"] = Dupap.Tables[0].Rows[j]["jlattach"].ToString().Trim();
				row["Index"] = 1;

				DupAttPoints.Rows.Add(row);
			}
			string dupAtt = String.Empty;
			string priorAtt = String.Empty;
			for (int h = 0; h < DupAttPoints.Rows.Count; h++)
			{
				dupAtt = DupAttPoints.Rows[h]["Attch"].ToString();

				if (h > 0)
				{
					priorAtt = DupAttPoints.Rows[h - 1]["Attch"].ToString();
				}

				if (dupAtt == DupAttPoints.Rows[h]["Attch"].ToString())
				{
					if (h > 0 && dupAtt == priorAtt)
					{
						DupAttPoints.Rows[h]["Index"] = 2;
						dupAtt = DupAttPoints.Rows[h]["Attch"].ToString();
					}
				}
			}
		}

		private void fixLine(string lltr, string lcltr)
		{
			string fixl = string.Format("update {0}.{1}line set jlsect = '{2}' where jlrecord = {3} and jldwell = {4} and jlsect = '{5}' ",
							SketchUpGlobals.LocalLib,
							SketchUpGlobals.LocalityPreFix,
							lltr,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							lcltr);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixl", fixl);
			conn.DBConnection.ExecuteNonSelectStatement(fixl);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixl", fixl);
			string fixatt = string.Format("update {0}.{1}line set jlattach = '{2}' where jlrecord = {3} and jldwell = {4} and jlattach = '{5}' ",
							SketchUpGlobals.LocalLib,
							SketchUpGlobals.LocalityPreFix,
							lltr,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							lcltr);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixatt", fixatt);
			conn.DBConnection.ExecuteNonSelectStatement(fixatt);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixatt", fixatt);
		}

		private void fixSection(string ltr, string cltr)
		{
			string fixsect = string.Format("update {0}.{1}section set jssect = '{2}' where jsrecord = {3} and jsdwell = {4} and jssect = '{5}' ",
							SketchUpGlobals.LocalLib,
							SketchUpGlobals.LocalityPreFix,
							ltr,
							_currentParcel.mrecno,
							_currentParcel.mdwell,
							cltr);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixsect", fixsect);
			conn.DBConnection.ExecuteNonSelectStatement(fixsect);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixsect", fixsect);
			fixLine(ltr, cltr);
		}

		private void FixSections(DataSet ds)
		{
			for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
			{
				string fixSect = string.Format("update {0}.{1}section set jssect = '{2}' where jsrecord = {3} and jsdwell = {4} and jssect = '{5}'", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, SectionLtrs.Rows[j]["NewSecLtr"].ToString().Trim(), _currentParcel.Record, _currentParcel.Card, SectionLtrs.Rows[j]["CurSecLtr"].ToString().Trim());

				//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixSect", fixSect);
				conn.DBConnection.ExecuteNonSelectStatement(fixSect);

				//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixSect", fixSect);
			}
			string newLineLtr = String.Empty;
			string oldLineLtr = String.Empty;
			for (int k = 0; k < SectionLtrs.Rows.Count; k++)
			{
				newLineLtr = SectionLtrs.Rows[k]["NewSecLtr"].ToString().Trim();

				oldLineLtr = SectionLtrs.Rows[k]["CurSecLtr"].ToString().Trim();

				upDlineLtr(newLineLtr, oldLineLtr);
			}
		}

		private void GetMisingGarageData()
		{
			MissingGarageData missGar = new MissingGarageData(conn, _currentParcel, GarSize, "GAR");
			missGar.ShowDialog();

			int newgarcnt = _currentParcel.mgarN2 + MissingGarageData.GarNbr;

			string addcp = string.Format("update {0}.{1}mast set mgar#2 = {2} where mrecno = {3} and mdwell = {4} ",
					SketchUpGlobals.LocalLib,
					SketchUpGlobals.LocalityPreFix,
					newgarcnt,
					_currentParcel.mrecno,
					_currentParcel.mdwell);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addcp", addcp);
			conn.DBConnection.ExecuteNonSelectStatement(addcp);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addcp", addcp);
			ParcelData.getParcel(conn, _currentParcel.mrecno, _currentParcel.mdwell);
		}

		private void GetMissingFirstGarageData()
		{
			MissingGarageData missGar = new MissingGarageData(conn, _currentParcel, GarSize, "GAR");
			missGar.ShowDialog();

			if (MissingGarageData.GarCode != _currentParcel.orig_mgart)
			{
				string fixCp = string.Format("update {0}.{1}mast set mgart = {2},mgar#c = {3} where mrecno = {4} and mdwell = {5} ",
				 SketchUpGlobals.LocalLib,
					 SketchUpGlobals.LocalityPreFix,
					MissingGarageData.GarCode,
					MissingGarageData.GarNbr,
					_currentParcel.mrecno,
					_currentParcel.mdwell);

				//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
				conn.DBConnection.ExecuteNonSelectStatement(fixCp);

				//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
				ParcelData.getParcel(conn, _currentParcel.mrecno, _currentParcel.mdwell);
			}
		}

		private void GetRatesFromTables(string type)
		{
			string getDesc = string.Format("select rclar,rclbr,rclcr,rcldr,rrpsf,rdesc from {0}.{1}rat1 where rid in ( 'C','P') and rsecto = '{2}'", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix
			, type.Trim());

			DataSet rates = conn.DBConnection.RunSelectStatement(getDesc);

			if (rates.Tables[0].Rows.Count > 0)
			{
				for (int p = 0; p < rates.Tables[0].Rows.Count; p++)
				{
					typDesc = rates.Tables[0].Rows[p]["rdesc"].ToString().Trim();
					RateA = Convert.ToInt32(rates.Tables[0].Rows[p]["rclar"].ToString());
					RateB = Convert.ToInt32(rates.Tables[0].Rows[p]["rclbr"].ToString());
					RateC = Convert.ToInt32(rates.Tables[0].Rows[p]["rclcr"].ToString());
					RateD = Convert.ToInt32(rates.Tables[0].Rows[p]["rcldr"].ToString());
					Ratepsf = Convert.ToDecimal(rates.Tables[0].Rows[p]["rrpsf"].ToString());
				}

				if (typDesc.Trim() == String.Empty || typDesc == null)
				{
					typDesc = "U/K";
				}
			}
		}

		private void InitializeComboBoxes(ParcelData currentparcel)
		{
			ResTypes = new List<string>();
			ComTypes = new List<string>();

			int ressectcnt = CamraSupport.ResidentialSectionTypeCollection.Count;
			int comsectcnt = CamraSupport.CommercialSectionTypeCollection.Count;

			if (ressectcnt > 0)
			{
				PopulateResidentialSectionsComboBox(ressectcnt);
			}

			if (comsectcnt > 0)
			{
				PopulateCommercialSectionComboBox(comsectcnt);
			}

			ResidentialSectionCbox.SelectedIndex = 0;
			CommercialSectionCbox.SelectedIndex = 0;

			Record = currentparcel.Record;
			Card = currentparcel.Card;
		}

		private void PopulateCommercialSectionComboBox(int comsectcnt)
		{
			for (int i = 0; i < comsectcnt; i++)
			{
				string ComSectTypes = String.Empty;
				if (i == 0)
				{
					ComSectTypes = "<Commercial Sections>";
				}
				if (i > 0)
				{
					ComSectTypes = String.Format("{0} - {1}",
						CamraSupport.CommercialSectionTypeCollection[i - 1]._commSectionType.ToString().Trim(),
						CamraSupport.CommercialSectionTypeCollection[i - 1]._commSectionDescription.ToString().Trim());

					ComTypes.Add(CamraSupport.CommercialSectionTypeCollection[i - 1]._commSectionType.ToString().Trim());
				}
				CommercialSectionCbox.Items.Add(ComSectTypes.Trim());
			}
		}

		private void PopulateResidentialSectionsComboBox(int ressectcnt)
		{
			for (int i = 0; i < ressectcnt; i++)
			{
				string ResSectTypes = String.Empty;
				if (i == 0)
				{
					ResSectTypes = "<Residential Sections>";
				}
				if (i > 0)
				{
					ResSectTypes = String.Format("{0} - {1}",
						CamraSupport.ResidentialSectionTypeCollection[i - 1]._resSectionType.ToString().Trim(),
						CamraSupport.ResidentialSectionTypeCollection[i - 1]._resSectionDescription.ToString().Trim());

					ResTypes.Add(CamraSupport.ResidentialSectionTypeCollection[i - 1]._resSectionType.ToString().Trim());
				}
				ResidentialSectionCbox.Items.Add(ResSectTypes.Trim());
			}
		}

		private void PreventResidentialChangeNewCOmmercialClass()
		{
			MessageBox.Show("Residential Change not Allowed");

			CRindex = SectDGView.CurrentRow.Index;
			SectDGView.CurrentRow.Cells["Class"].Style.BackColor = Color.White;
			SectDGView.CurrentRow.Cells["Class"].Value = _currentSection[CRindex].orig_jsclass.ToUpper();
			SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
		}

		private void PreventResidentialChangeZeroDepr()
		{
			MessageBox.Show("Residential Change not Allowed");

			CRindex = SectDGView.CurrentRow.Index;

			SectDGView.CurrentRow.Cells["0Depr"].Style.BackColor = Color.White;
			SectDGView.CurrentRow.Cells["0Depr"].Value = _currentSection[CRindex].orig_js0depr.ToUpper();
			SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
		}

		private void ProcessChangedSectDGViewCell(DataGridViewCellEventArgs e)
		{
			if (SectDGView.Columns[e.ColumnIndex].Name == "Rate")
			{
				#region Stuff Commented Out by Joel - 1

				//TODO: Figure out what this is designed to do.
				//if (!CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				//{
				// //CRindex = SectDGView.CurrentRow.Index;

				// ////MessageBox.Show("Rate Change not Allowed");

				// //SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.White;
				// //SectDGView.CurrentRow.Cells["Rate"].Value = OriginalUnitRate;
				// //SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
				//}

				#endregion Stuff Commented Out by Joel - 1

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					ProcessTypeChangeRequestResidential();
				}
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "Type")
			{
				CRindex = SectDGView.CurrentRow.Index;

				string newType = String.Empty;
				string newDesc = String.Empty;
				newType = SectDGView.CurrentRow.Cells["Type"].Value.ToString().ToUpper().Trim();
				_jsect = SectDGView.CurrentRow.Cells["Section"].Value.ToString().ToUpper().Trim();

				int thisSect = 0;

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup) || CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
				{
					string curSect = SectDGView.CurrentRow.Cells[1].Value.ToString().PadRight(4).Substring(0, 4).ToUpper();

					SectDGView.CurrentRow.Cells[1].Value = newType;

					_currentSection[CRindex].jstype = newType;

					int rx = 0;
					if (ResTypes.Contains(newType))
					{
						AddNewTypeToResidentialTypes(newType, out newDesc, out thisSect, curSect, out rx);
					}

					int cx = 0;
					if (ComTypes.Contains(newType))
					{
						AddNewTypeToCommercialTypes(newType, out newDesc, out thisSect, curSect, out cx);
					}

					_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
					_getUnitValue(_currentSection[CRindex].js0depr, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, _currentSection[CRindex].jsdeprc);

					_currentSection[CRindex].jsdesc = newDesc;

					SectDGView.CurrentRow.Cells["Rate"].Value = UnitRate;
					SectDGView.CurrentRow.Cells["Value"].Value = _value;

					SectDGView.CurrentRow.Cells["Desc"].Value = newDesc;

					if (_currentSection[CRindex].orig_jstype != _currentSection[CRindex].jstype)
					{
						SectDGView.CurrentRow.Cells["Type"].Style.BackColor = Color.PaleGreen;
						SectDGView.CurrentRow.Cells["Desc"].Style.BackColor = Color.PaleGreen;
						SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.PaleGreen;
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
					}
					if (_currentSection[CRindex].orig_jstype == _currentSection[CRindex].jstype)
					{
						SectDGView.CurrentRow.Cells["Type"].Style.BackColor = Color.White;
						SectDGView.CurrentRow.Cells["Desc"].Style.BackColor = Color.White;
						SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.White;
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
					}
				}
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "Story")
			{
				ProcessNewStory();
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "0Depr")
			{
				if (!CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					ProcessZeroDepr();
				}
				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					PreventResidentialChangeZeroDepr();
				}
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "Class")
			{
				if (!CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					ProcessNewClass();
				}
				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					PreventResidentialChangeNewCOmmercialClass();
				}
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "Factor")
			{
				if (!CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					CRindex = SectDGView.CurrentRow.Index;

					newFactor = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Factor"].Value.ToString());
					_jfactor = _currentSection[CRindex].orig_jsfactor;

					if (newFactor >= 1m)
					{
						tempFactor = Math.Round((newFactor / 100), 2);
						newFactor = tempFactor;
					}

					SectDGView.CurrentRow.Cells["Factor"].Value = newFactor;

					_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
					_getUnitValue(_currentSection[CRindex].js0depr, _currentSection[CRindex].jssqft, newFactor, _currentSection[CRindex].jsdeprc);

					SectDGView.CurrentRow.Cells["Value"].Value = _value;

					_currentSection[CRindex].jsvalue = _value;
					_currentSection[CRindex].jsfactor = newFactor;

					if (newFactor != _jfactor)
					{
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
						SectDGView.CurrentRow.Cells["Factor"].Style.BackColor = Color.PaleGreen;
					}
					if (newFactor == _jfactor)
					{
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
						SectDGView.CurrentRow.Cells["Factor"].Style.BackColor = Color.White;
					}
				}

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					MessageBox.Show("Residential Change not Allowed");

					CRindex = SectDGView.CurrentRow.Index;
					SectDGView.CurrentRow.Cells["Factor"].Style.BackColor = Color.White;
					SectDGView.CurrentRow.Cells["Facotr"].Value = _currentSection[CRindex].orig_jsfactor;
					SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
				}
			}

			if (SectDGView.Columns[e.ColumnIndex].Name == "Deprec")
			{
				if (!CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					CRindex = SectDGView.CurrentRow.Index;
					newDeprec = 0;
					alowNoDep = false;

					newDeprec = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Deprec"].Value);
					_jdeprec = _currentSection[CRindex].orig_jsdeprc;

					if (NewZeroDep == "Y" && newDeprec > 0)
					{
						SectDGView.CurrentRow.Cells["0Depr"].Value = String.Empty;
						_currentSection[CRindex].js0depr = String.Empty;
						NewZeroDep = String.Empty;
					}
					if (NewZeroDep == "Y" && newDeprec == 0)
					{
						newDeprec = 0;

						//SectDGView.CurrentRow.Cells["Deprec"].Value = 0;
						_currentSection[CRindex].jsdeprc = 0;
					}

					if (newDeprec >= 1)
					{
						newDeprec = (newDeprec / 100m);
						_currentSection[CRindex].jsdeprc = newDeprec;
						SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
					}

					if (newDeprec == 0 && alowNoDep == false && NewZeroDep.ToUpper() != "Y")
					{
						DialogResult OKNoDep;
						OKNoDep = (MessageBox.Show("Change to 'Y' 'N' for Defalut ", "Change No Depreciation",
									MessageBoxButtons.YesNo, MessageBoxIcon.Question));

						if (OKNoDep == DialogResult.Yes)
						{
							alowNoDep = true;
							_currentSection[CRindex].js0depr = "Y";
							SectDGView.CurrentRow.Cells["0Depr"].Value = "Y";

							//SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
						}
						if (OKNoDep == DialogResult.No)
						{
						}
					}

					if (newDeprec == 0 && _currentSection[CRindex].js0depr != "Y")
					{
						_getDefaultDepr(newDeprec, _currentSection[CRindex].js0depr);

						newDeprec = _defDepr;

						SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
					}

					if (newDeprec == 0 && _currentSection[CRindex].js0depr == "Y")
					{
						_getDefaultDepr(newDeprec, _currentSection[CRindex].js0depr);

						newDeprec = _defDepr;

						SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
					}

					_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
					_getUnitValue(_currentSection[CRindex].js0depr, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, newDeprec);

					SectDGView.CurrentRow.Cells["Value"].Value = _value;

					//SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
					_currentSection[CRindex].jsdeprc = newDeprec;

					if (newDeprec != _jdeprec)
					{
						SectDGView.CurrentRow.Cells["Deprec"].Style.BackColor = Color.PaleGreen;
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
					}
					if (newDeprec == _jdeprec)
					{
						SectDGView.CurrentRow.Cells["Deprec"].Style.BackColor = Color.White;
						SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
					}
				}
				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					MessageBox.Show("Residential Change not Allowed");

					CRindex = SectDGView.CurrentRow.Index;
					SectDGView.CurrentRow.Cells["Deprec"].Style.BackColor = Color.White;
					SectDGView.CurrentRow.Cells["Deprec"].Value = _currentSection[CRindex].orig_jsdeprc;
					SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
				}
			}

			SumSectionValues();
			SumSectionArea();

			decimal test3 = sumSize;
		}

		private void ProcessNewClass()
		{
			CRindex = SectDGView.CurrentRow.Index;
			newClass = String.Empty;

			List<string> clsList = new List<string>();
			clsList.Add("A");
			clsList.Add("B");
			clsList.Add("C");
			clsList.Add("D");
			clsList.Add("E");
			clsList.Add("M");

			newClass = SectDGView.CurrentRow.Cells["Class"].Value.ToString().ToUpper().Substring(0, 1).Trim();
			_jclass = _currentSection[CRindex].orig_jsclass;

			if (!clsList.Contains(newClass))
			{
				DialogResult redo;
				redo = MessageBox.Show("Incorrect Class - Please Re-Enter", "Class Error",
					MessageBoxButtons.OK, MessageBoxIcon.Warning);

				if (redo == DialogResult.OK)
				{
					int colIndex = SectDGView.CurrentRow.Cells["Class"].ColumnIndex;
					SectDGView.CurrentRow.Cells["Class"].Value = _jclass.ToUpper();
					_currentSection[CRindex].jsclass = _jclass;
				}
			}

			if (clsList.Contains(newClass))
			{
				SectDGView.CurrentRow.Cells["Class"].Value = newClass.ToUpper();
				_currentSection[CRindex].jsclass = newClass;

				if (newClass != _jclass)
				{
					SectDGView.CurrentRow.Cells["Class"].Style.BackColor = Color.PaleGreen;
				}
				if (newClass == _jclass)
				{
					SectDGView.CurrentRow.Cells["Class"].Style.BackColor = Color.White;
				}
			}

			_getUnitRate(_currentSection[CRindex].jstype, newClass);
			_getUnitValue(_currentSection[CRindex].js0depr, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, _currentSection[CRindex].jsdeprc);

			SectDGView.CurrentRow.Cells["Value"].Value = _value;
			SectDGView.CurrentRow.Cells["Rate"].Value = UnitRate;

			if (newClass != _jclass)
			{
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
				SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.PaleGreen;
			}
			if (newClass == _jclass)
			{
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
				SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.White;
			}
		}

		private void ProcessNewStory()
		{
			CRindex = SectDGView.CurrentRow.Index;

			decimal newStory = 0;

			newStory = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Story"].Value.ToString().ToUpper().Trim());
			_jstory = _currentSection[CRindex].orig_jsstory;

			_jssqft = _currentSection[CRindex].orig_jssqft;

			if (_jstory == 0)
			{
				_jstory = 1;
				_currentSection[CRindex].orig_jsstory = 1;
			}

			decimal tststory = (newStory / _jstory);

			_currentSection[CRindex].jsstory = newStory;

			_jssqft = Convert.ToDecimal(Math.Round(Convert.ToDecimal(_currentSection[CRindex].orig_jssqft * tststory), 1));
			_currentSection[CRindex].jssqft = _jssqft;

			_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
			_getUnitValue(_currentSection[CRindex].js0depr, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, _currentSection[CRindex].jsdeprc);

			SectDGView.CurrentRow.Cells["Value"].Value = _value;
			SectDGView.CurrentRow.Cells["Size"].Value = _jssqft.ToString();

			if (newStory != _jstory)
			{
				SectDGView.CurrentRow.Cells["Story"].Style.BackColor = Color.PaleGreen;
				SectDGView.CurrentRow.Cells["Size"].Style.BackColor = Color.PaleGreen;
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
			}
			if (newStory == _jstory)
			{
				SectDGView.CurrentRow.Cells["Story"].Style.BackColor = Color.White;
				SectDGView.CurrentRow.Cells["Size"].Style.BackColor = Color.White;
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
			}
		}

		private void ProcessTypeChangeRequestResidential()
		{
			CRindex = SectDGView.CurrentRow.Index;

			if (typeChange == false)
			{
				MessageBox.Show("Rate Change not Allowed");

				SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.White;
				SectDGView.CurrentRow.Cells["Rate"].Value = OriginalUnitRate;
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
			}
			if (typeChange == true)
			{
				SectDGView.CurrentRow.Cells["Rate"].Style.BackColor = Color.PaleGreen;
				SectDGView.CurrentRow.Cells["Rate"].Value = UnitRate;
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
			}
		}

		private void ProcessZeroDepr()
		{
			CRindex = SectDGView.CurrentRow.Index;
			NewZeroDep = String.Empty;

			NewZeroDep = SectDGView.CurrentRow.Cells["0Depr"].Value.ToString().ToUpper().Trim();
			_j0depr = _currentSection[CRindex].orig_js0depr;

			if (NewZeroDep.ToUpper().Trim() != "Y" && NewZeroDep.ToUpper().Trim() != String.Empty)
			{
				DialogResult noDep;
				noDep = (MessageBox.Show("Must use 'Y' or 'Blank' for No Depreciation ", "No Depreciation Warning",
					MessageBoxButtons.OKCancel, MessageBoxIcon.Warning));

				if (noDep == DialogResult.OK)
				{
					DialogResult noDep2;
					noDep2 = (MessageBox.Show("Change to 'Y' ", "Change No Depreciation",
						MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question));

					if (noDep2 == DialogResult.Yes)
					{
						SectDGView.CurrentRow.Cells["0Depr"].Value = "Y";
						NewZeroDep = "Y";
						_currentSection[CRindex].js0depr = NewZeroDep;
					}

					if (noDep2 == DialogResult.No || noDep2 == DialogResult.Cancel)
					{
						SectDGView.CurrentRow.Cells["0Depr"].Value = String.Empty;
						NewZeroDep = String.Empty;
						_currentSection[CRindex].js0depr = String.Empty;
					}
				}
			}
			if (NewZeroDep.ToUpper().Trim() == "Y" && _currentSection[CRindex].jsdeprc != 0)
			{
				DialogResult useDefault;
				useDefault = MessageBox.Show("Want No Depreciation", "Zero Depreciation Warning",
					MessageBoxButtons.OK, MessageBoxIcon.Question);

				if (useDefault == DialogResult.OK)
				{
					SectDGView.CurrentRow.Cells["0Depr"].Value = NewZeroDep;
					_currentSection[CRindex].js0depr = "Y";
					SectDGView.CurrentRow.Cells["Deprec"].Value = 0;
					_currentSection[CRindex].jsdeprc = 0;
				}
			}

			if (NewZeroDep.ToUpper().Trim() == "Y")
			{
				_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
				_getUnitValue(NewZeroDep, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, _currentSection[CRindex].jsdeprc);

				SectDGView.CurrentRow.Cells["Deprec"].Value = 0;
				_currentSection[CRindex].jsdeprc = 0;

				SectDGView.CurrentRow.Cells["Deprec"].Style.BackColor = Color.PaleGreen;
				SectDGView.CurrentRow.Cells["Value"].Value = _value;
				_currentSection[CRindex].jsvalue = _value;
			}
			if (NewZeroDep.Trim() == String.Empty)
			{
				_getUnitRate(_currentSection[CRindex].jstype, _currentSection[CRindex].jsclass);
				_getUnitValue(NewZeroDep, _currentSection[CRindex].jssqft, _currentSection[CRindex].jsfactor, _currentSection[CRindex].jsdeprc);

				SectDGView.CurrentRow.Cells["Value"].Value = _value;
				_currentSection[CRindex].jsvalue = _value;
			}

			if (NewZeroDep.Trim() == String.Empty && _currentSection[CRindex].jsdeprc == 0)
			{
				_getDefaultDepr(_currentSection[CRindex].jsdeprc, NewZeroDep.Trim());

				newDeprec = _defDepr;

				SectDGView.CurrentRow.Cells["Deprec"].Value = newDeprec;
			}

			SectDGView.CurrentRow.Cells["0Depr"].Value = NewZeroDep;
			_currentSection[CRindex].js0depr = NewZeroDep.Trim();

			if (NewZeroDep != _j0depr)
			{
				SectDGView.CurrentRow.Cells["0Depr"].Style.BackColor = Color.PaleGreen;
			}
			if (NewZeroDep == _j0depr)
			{
				SectDGView.CurrentRow.Cells["0Depr"].Style.BackColor = Color.White;
			}
			if (_currentSection[CRindex].jsvalue != _currentSection[CRindex].orig_jsvalue)
			{
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.PaleGreen;
			}
			if (_currentSection[CRindex].jsvalue == _currentSection[CRindex].orig_jsvalue)
			{
				SectDGView.CurrentRow.Cells["Value"].Style.BackColor = Color.White;
			}
		}

		private void Reorder()
		{
			Garcnt = 0;
			GarSize = 0;
			CPcnt = 0;
			CPSize = 0;

			int tg = _currentParcel.mgart;
			int tg2 = _currentParcel.mgart2;
			int tc = _currentParcel.mcarpt;
			int tcc = _currentParcel.mcarNc;
			int tki = _currentParcel.orig_mgart;
			int tkic = _currentParcel.orig_mgarNc;

			string getSect = string.Format("select jsrecord,jsdwell,jssect,jstype,jssqft from {0}.{1}section where jsrecord = {2} and jsdwell = {3}  order by jssect ",
							 SketchUpGlobals.FcLib,
							 SketchUpGlobals.FcLocalityPrefix,
							 _currentParcel.Record,
							 _currentParcel.Card);

			DataSet ds = conn.DBConnection.RunSelectStatement(getSect);

			if (ds.Tables[0].Rows.Count > 0)
			{
				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					DataRow row = SectionLtrs.NewRow();
					row["RecNo"] = _currentParcel.Record;
					row["CardNo"] = _currentParcel.Card;
					row["CurSecLtr"] = ds.Tables[0].Rows[i]["jssect"].ToString().Trim();
					row["NewSecLtr"] = Letters[i].ToString().Trim();
					row["NewType"] = ds.Tables[0].Rows[i]["jstype"].ToString().Trim();
					row["SectSize"] = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());

					SectionLtrs.Rows.Add(row);

					string tstetype = ds.Tables[0].Rows[i]["jstype"].ToString().Trim();

					if (CamraSupport.GarageTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
					{
						Garcnt++;

						GarSize = Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
					}
					if (CamraSupport.CarPortTypes.Contains(ds.Tables[0].Rows[i]["jstype"].ToString().Trim()))
					{
						CPcnt++;

						CPSize = CPSize + Convert.ToDecimal(ds.Tables[0].Rows[i]["jssqft"].ToString());
					}
				}
			}

			if (Garcnt == 0)
			{
				UpdateForZeroGarage();
			}
			if (CPcnt == 0)
			{
				UpdateForZeroCP();
			}

			UpdateGarages();

			if (CPcnt > 0)
			{
				UodateCarports();

				FixSections(ds);

				// check for cp & Garage Count after delete

				AdjustAttachments();
				setAttPnts();

				//Moved to a single call to getParcel after all other updates have run.
				ParcelData.getParcel(conn, _currentParcel.mrecno, _currentParcel.mdwell);
			}
		}

		private void ResetSect()
		{
			int countsect = 0;

			StringBuilder cntsect = new StringBuilder();
			cntsect.Append(String.Format("select jssect from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
								SketchUpGlobals.LocalLib,
								SketchUpGlobals.LocalityPreFix,
								_currentParcel.mrecno,
								_currentParcel.mdwell));

			DataSet seccnt = conn.DBConnection.RunSelectStatement(cntsect.ToString());

			StringBuilder cntline = new StringBuilder();
			cntline.Append(String.Format("select jlsect from {0}.{1}line where jlrecord = {2} and jldwell = {3} and jlline# = 1 ",
						SketchUpGlobals.LocalLib,
						SketchUpGlobals.LocalityPreFix,
						_currentParcel.mrecno,
						_currentParcel.mdwell));

			DataSet secline = conn.DBConnection.RunSelectStatement(cntline.ToString());
			cursectltr = new List<string>();
			if (secline.Tables[0].Rows.Count > 0)
			{
				for (int j = 0; j < secline.Tables[0].Rows.Count; j++)
				{
					cursectltr.Add(secline.Tables[0].Rows[j]["jlsect"].ToString().Trim());
				}
			}

			if (seccnt.Tables[0].Rows.Count > 0)
			{
				countsect = seccnt.Tables[0].Rows.Count;

				for (int i = 0; i < countsect; i++)
				{
					string sectlr = Letters[i].ToString().Trim();

					string csecltr = cursectltr[i].ToString().Trim();

					if (sectlr != csecltr)
					{
						fixSection(sectlr, csecltr);
					}
				}
			}
		}

		private void SectDGView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex < 0)
			{
				return;
			}

			rowselected = true;

			SectDGView.FirstDisplayedScrollingRowIndex = index;
			SectDGView.Refresh();
			SectDGView.Rows[index].Selected = true;

			OriginalUnitRate = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Rate"].Value.ToString());

			OriginalUnitType = SectDGView.CurrentRow.Cells["TYPE"].Value.ToString().PadRight(4).Substring(0, 4);

			if (SectDGView.SelectedRows.Count > 0)
			{
				if (SectDGView.Columns[e.ColumnIndex].Name == "Rate")
				{
					CRindex = SectDGView.CurrentRow.Index;
					OriginalUnitRate = Convert.ToDecimal(SectDGView.CurrentRow.Cells["Rate"].Value.ToString());
				}

				if (SectDGView.Columns[e.ColumnIndex].Name == "Type")
				{
					if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
					{
						string curSect = SectDGView.CurrentRow.Cells[1].Value.ToString().PadRight(4).Substring(0, 4);

						int thisSect = ResTypes.IndexOf(curSect.Trim());

						ResidentialSectionCbox.SelectedIndex = thisSect + 1;

						string curTypeDesc = ResidentialSectionCbox.SelectedItem.ToString();

						int TypeIndex = curTypeDesc.IndexOf("-");

						typeChange = true;
					}
					if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
					{
						string curSect = SectDGView.CurrentRow.Cells[1].Value.ToString().PadRight(4).Substring(0, 4);

						int thisSect = ResTypes.IndexOf(curSect.Trim());

						int ComSectIndx = ComTypes.IndexOf(curSect.Trim());

						if (thisSect > 0 && ComSectIndx <= 0)
						{
							ResidentialSectionCbox.SelectedIndex = thisSect + 1;
						}
						if (ComSectIndx > 0 && thisSect <= 0)
						{
							CommercialSectionCbox.SelectedIndex = ComSectIndx + 1;
						}
					}
				}

				if (e.ColumnIndex == 0)
				{
					string _secLtr = SectDGView.CurrentRow.Cells[0].Value.ToString().Trim();

					//SketchLines sktLines = new SketchLines(conn, Record, Card, _secLtr);
					//sktLines.ShowDialog(this);
				}
			}
		}

		private void SectDGView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			ProcessChangedSectDGViewCell(e);
		}

		private void SetResOrCommValues(string type)
		{
			if (ResTypes.Contains(type.Trim()))
			{
				resSection = true;
				commSection = false;
			}
			if (ComTypes.Contains(type.Trim()))
			{
				resSection = false;
				commSection = true;
			}
		}

		private void SumSectionArea()
		{
			decimal totalsectionarea = 0;
			int sectcnt = _currentSection.Count;

			for (int i = 0; i < sectcnt; i++)
			{
				totalsectionarea = (totalsectionarea + _currentSection[i].jssqft);
			}

			sumSize = totalsectionarea;
		}

		private void SumSectionValues()
		{
			if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
			{
				sumBaseValue = 0;
				sumFinalValue = 0;

				//sumFinalFullValue = 0;
				sumDepreciation = 0;
				sumFactoredValue = 0;

				int uti = Convert.ToInt32(SectDGView.Rows[0].Cells["Size"].Value);

				int tst2 = Convert.ToInt32(_currentSection[0].jssqft);

				for (int i = 0; i < SectDGView.Rows.Count; i++)
				{
					sumFinalValue = Convert.ToInt32(sumFinalValue + Convert.ToInt32(SectDGView.Rows[i].Cells["Value"].Value));
					sumBaseValue = Convert.ToInt32(sumBaseValue +
						Convert.ToInt32((_currentSection[i].jssqft) * Convert.ToInt32(SectDGView.Rows[i].Cells["Rate"].Value)));
					sumFactoredValue = Convert.ToInt32(sumFactoredValue +
						Convert.ToInt32((_currentSection[i].jssqft) * Convert.ToInt32(SectDGView.Rows[i].Cells["Rate"].Value) *
						(1 + Convert.ToDecimal(SectDGView.Rows[i].Cells["Factor"].Value))));

					sumDepreciation = Convert.ToInt32(sumDepreciation + Convert.ToInt32((_currentSection[i].jssqft) *
						Convert.ToInt32(SectDGView.Rows[i].Cells["Rate"].Value) *
						(1 + Convert.ToDecimal(SectDGView.Rows[i].Cells["Factor"].Value)) *
						Convert.ToDecimal(SectDGView.Rows[i].Cells["Deprec"].Value)));

					//sumFinalFullValue = Convert.ToInt32(sumFinalFullValue + Convert.ToInt32(SectDGView.Rows[i].Cells["NewValue"].Value));
				}
			}
		}

		private void TotalUpSectionValues()
		{
			SectDGView.DataSource = Sections;

			//}

			DataGridViewColumn sectionCol1 = SectDGView.Columns[0];
			DataGridViewColumn typeCol2 = SectDGView.Columns[1];
			DataGridViewColumn descCol3 = SectDGView.Columns[2];
			DataGridViewColumn storyCol4 = SectDGView.Columns[3];
			DataGridViewColumn sizeCol5 = SectDGView.Columns[4];
			DataGridViewColumn NoDeprCol6 = SectDGView.Columns[5];
			DataGridViewColumn ClassCol7 = SectDGView.Columns[6];
			DataGridViewColumn factorCol8 = SectDGView.Columns[7];
			DataGridViewColumn rateCol9 = SectDGView.Columns[8];
			DataGridViewColumn deprcCol10 = SectDGView.Columns[9];

			DataGridViewColumn valueCol11 = SectDGView.Columns[10];
			sectionCol1.Width = 25;
			typeCol2.Width = 75;
			descCol3.Width = 125;
			storyCol4.Width = 50;
			sizeCol5.Width = 50;
			NoDeprCol6.Width = 50;
			ClassCol7.Width = 50;
			factorCol8.Width = 50;
			rateCol9.Width = 50;
			deprcCol10.Width = 50;
			valueCol11.Width = 75;

			SumSectionValues();
		}

		private void UodateCarports()
		{
			if (CPcnt > 0 && _currentParcel.mcarpt == 0 || CPcnt > 0 && _currentParcel.mcarpt == 67)
			{
				if (_sectDelete != true)
				{
					MissingGarageData missCP = new MissingGarageData(conn, _currentParcel, CPSize, "CP");
					missCP.ShowDialog();

					if (MissingGarageData.CPCode != _currentParcel.orig_mcarpt)
					{
						StringBuilder fixCp = new StringBuilder();
						fixCp.Append(String.Format("update {0}.{1}mast set mcarpt = {2},mcar#c = {3} ",
						 SketchUpGlobals.LocalLib,
							 SketchUpGlobals.LocalityPreFix,

							//SketchUpGlobals.FcLib,
							//SketchUpGlobals.FcLocalityPrefix,
							MissingGarageData.CPCode,
							MissingGarageData.CpNbr));
						fixCp.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
						conn.DBConnection.ExecuteNonSelectStatement(fixCp.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
					}
				}

				if (_sectDelete != false && _currentParcel.mcarpt == 67)
				{
					StringBuilder carupd = new StringBuilder();
					carupd.Append(String.Format("update {0}.{1}mast set mcarpt = 65,mcar#c = 1 where mrecno = {2} and mdwell = {3} ",
									SketchUpGlobals.LocalLib,
									SketchUpGlobals.LocalityPreFix,
									_currentParcel.mrecno,
									_currentParcel.mdwell));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "carupd", carupd);
					conn.DBConnection.ExecuteNonSelectStatement(carupd.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "carupd", carupd);
					_currentParcel.mcarpt = 65;
					_currentParcel.mcarNc = 1;
				}
			}

			if (CPcnt > 1 && _currentParcel.mcarpt != 0 || CPcnt > 1 && _currentParcel.mcarpt != 67)
			{
				if (_sectDelete == true)
				{
					MissingGarageData missCPx = new MissingGarageData(conn, _currentParcel, CPSize, "CP");
					missCPx.ShowDialog();

					int newcpcnt = _currentParcel.mcarNc + MissingGarageData.CpNbr;

					StringBuilder addcp = new StringBuilder();
					addcp.Append(String.Format("update {0}.{1}mast set mcar#c = {2} where mrecno = {3} and mdwell = {4} ",
							SketchUpGlobals.LocalLib,
							SketchUpGlobals.LocalityPreFix,
							newcpcnt,
							_currentParcel.mrecno,
							_currentParcel.mdwell));

					//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "addcp", addcp);
					conn.DBConnection.ExecuteNonSelectStatement(addcp.ToString());

					//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "addcp", addcp);
					//ParcelData.getParcel(conn, _currentParcel.mrecno, _currentParcel.mdwell);
				}
			}
		}

		private void UpdateForZeroCP()
		{
			string zerocp = string.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
									SketchUpGlobals.LocalLib,
									SketchUpGlobals.LocalityPreFix,
									_currentParcel.mrecno,
									_currentParcel.mdwell);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "zerocp", zerocp);
			conn.DBConnection.ExecuteNonSelectStatement(zerocp);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "zerocp", zerocp);
		}

		private void UpdateForZeroGarage()
		{
			string zerogar = string.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0,mgart2 = 0,mgar#2 = 0 where mrecno = {2} and mdwell = {3} ",
													SketchUpGlobals.LocalLib,
													SketchUpGlobals.LocalityPreFix,
													_currentParcel.mrecno,
													_currentParcel.mdwell);

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "zerogar", zerogar);
			conn.DBConnection.ExecuteNonSelectStatement(zerogar);

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "zerogar", zerogar);
		}

		private void UpdateGAR_CP()
		{
			int curIndex = SectDGView.CurrentRow.Index;
			string dType2 = SectDGView.Rows[curIndex].Cells["Type"].Value.ToString();

			bool garcptype = false;

			if (CamraSupport.GarageTypes.Contains(OriginalUnitType.Trim()))
			{
				garcptype = true;

				ngar = 0;

				CountGar(OriginalUnitType.Trim());
			}
			if (CamraSupport.CarPortTypes.Contains(OriginalUnitType.Trim()))
			{
				garcptype = true;

				ncp = 0;

				CountCP(OriginalUnitType.Trim());
			}

			if (garcptype == true)
			{
				StringBuilder garcp2 = new StringBuilder();

				//garcp.Append("select rsecto from rat1 where rid = 'P' and rdesc like '%GAR%' and rrpsf <> 0 and rincsf = 'Y' ");
				garcp2.Append(String.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%GAR%' and rrpsf <> 0 ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));

				try
				{
					DataSet ds = conn.DBConnection.RunSelectStatement(garcp2.ToString());

					if (ds.Tables[0].Rows.Count > 0)
					{
						GarTypes = new List<string>();
						for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
						{
							string sect = ds.Tables[0].Rows[i]["rsecto"].ToString().Trim();

							GarTypes.Add(sect);
						}
					}
				}
				catch
				{
				}

				StringBuilder cptype2 = new StringBuilder();

				//cptype.Append("select rsecto from rat1 where rid = 'P' and rdesc like '%CAR%' and rrpsf <> 0 and rincsf = 'Y' ");
				cptype2.Append(String.Format("select rsecto from {0}.{1}rat1 where rid = 'P' and rdesc like '%CAR%' and rrpsf <> 0 ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));

				try
				{
					DataSet ds1 = conn.DBConnection.RunSelectStatement(cptype2.ToString());

					if (ds1.Tables[0].Rows.Count > 0)
					{
						CPTypes = new List<string>();
						for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
						{
							string sect = ds1.Tables[0].Rows[i]["rsecto"].ToString().Trim();

							CPTypes.Add(sect);
						}
					}
				}
				catch
				{
				}

				StringBuilder garcode = new StringBuilder();
				garcode.Append(String.Format("select ttelem from {0}.{1}stab where ttid = 'GAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ",
					SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));

				try
				{
					DataSet gc = conn.DBConnection.RunSelectStatement(garcode.ToString());

					if (gc.Tables[0].Rows.Count > 0)
					{
						GarCodes = new List<int>();
						for (int i = 0; i < gc.Tables[0].Rows.Count; i++)
						{
							int gcode = Convert.ToInt32(gc.Tables[0].Rows[i]["ttelem"].ToString());

							GarCodes.Add(gcode);
						}
					}
				}
				catch
				{
				}

				StringBuilder cpcode = new StringBuilder();
				cpcode.Append(String.Format("select ttelem from {0}.{1}stab where ttid = 'CAR' and tdesc not like '%NONE%' and tdesc not like '%DETACHED%' ",
					SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix));

				try
				{
					DataSet cp = conn.DBConnection.RunSelectStatement(cpcode.ToString());

					if (cp.Tables[0].Rows.Count > 0)
					{
						CPCodes = new List<int>();
						for (int i = 0; i < cp.Tables[0].Rows.Count; i++)
						{
							int cpcodeX = Convert.ToInt32(cp.Tables[0].Rows[i]["ttelem"].ToString());

							CPCodes.Add(cpcodeX);
						}
					}
				}
				catch
				{
				}

				try
				{
					if (GarTypes.Contains(dType2))
					{
						ngar = 0;

						CountGar(dType2);

						if (ngar == 0)
						{
							StringBuilder fixGar = new StringBuilder();
							fixGar.Append(String.Format("update {0}.{1}mast set mgart = 63, mgar#c = 0 where mrecno = {2} and mdwell = {3} ",
								SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell));

							//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixGar (ngar == 0)", fixGar);
							conn.DBConnection.ExecuteNonSelectStatement(fixGar.ToString());

							//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixGar", fixGar);
							_currentParcel.mgart = 63;
							_currentParcel.mgarNc = 0;
						}
					}
				}
				catch
				{
				}

				try
				{
					if (CPTypes.Contains(dType2))
					{
						StringBuilder fixCP = new StringBuilder();
						fixCP.Append(String.Format("update {0}.{1}mast set mcarpt = 67, mcar#c = 0 where mrecno = {2} and mdwell = {3} ",
							 SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _currentParcel.mrecno, _currentParcel.mdwell));

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCP (CPTypes.Contains(dType2))", fixCP);
						conn.DBConnection.ExecuteNonSelectStatement(fixCP.ToString());

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCP", fixCP);
						_currentParcel.mcarpt = 67;
						_currentParcel.mcarNc = 0;
					}
				}
				catch
				{
				}
			}
		}

		private void UpdateGarages()
		{
			if (Garcnt > 0)
			{
				if (Garcnt == 1 && _currentParcel.mgart <= 60 || Garcnt == 1 && _currentParcel.mgart == 63 || Garcnt == 1 && _currentParcel.mgart == 64)
				{
					if (_sectDelete != true)
					{
						GetMissingFirstGarageData();
					}
					if (_sectDelete == true && _currentParcel.mgart == 63)
					{
						string fixgar2 = string.Format("update {0}.{1}mast set mgart2 = 0,mgar#2 = 0, mgart = {2}, mgar#c = {3} where mrecno = {4} and mdwell = {5} ",
										SketchUpGlobals.LocalLib,
										SketchUpGlobals.LocalityPreFix,
										_currentParcel.orig_mgart,
										_currentParcel.orig_mgarNc,
										_currentParcel.mrecno,
										_currentParcel.mdwell);

						//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixgar2", fixgar2);
						conn.DBConnection.ExecuteNonSelectStatement(fixgar2);

						//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixgar2", fixgar2);
					}
				}
				if (Garcnt > 1 && _currentParcel.mgart2 == 0)
				{
					if (_sectDelete != true)
					{
						MissingGarageData missGar = new MissingGarageData(conn, _currentParcel, GarSize, "GAR");
						missGar.ShowDialog();

						if (MissingGarageData.GarCode != _currentParcel.orig_mgart2)
						{
							string fixCp = string.Format("update {0}.{1}mast set mgart2 = {2},mgar#2 = {3} where mrecno = {4} and mdwell = {5} ", SketchUpGlobals.LocalLib, SketchUpGlobals.LocalityPreFix, MissingGarageData.GarCode, MissingGarageData.GarNbr, _currentParcel.mrecno, _currentParcel.mdwell);

							//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
							conn.DBConnection.ExecuteNonSelectStatement(fixCp);

							//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixCp", fixCp);
						}
					}
				}
				if (Garcnt > 2)
				{
					if (_sectDelete != true)
					{
						GetMisingGarageData();
					}
				}
			}
		}

		private void upDlineLtr(string newLtr, string old)
		{
			StringBuilder fixLine = new StringBuilder();
			fixLine.Append(String.Format("update {0}.{1}line set jlsect = '{2}' where jlrecord = {3} and jldwell = {4} ",
							SketchUpGlobals.FcLib,
							SketchUpGlobals.FcLocalityPrefix,
							newLtr,
							_currentParcel.Record,
							_currentParcel.Card));
			fixLine.Append(String.Format(" and jlsect = '{0}' ", old));

			//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "fixLine", fixLine);
			conn.DBConnection.ExecuteNonSelectStatement(fixLine.ToString());

			//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "fixLine", fixLine);
		}
	}

	#endregion Refactored Methods
}