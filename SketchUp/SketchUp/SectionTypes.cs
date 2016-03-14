using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
	public partial class SectionTypes : Form
	{
		public SectionTypes(SWallTech.CAMRA_Connection conn, ParcelData data, bool addSection, int lineCount, bool newSketch)
		{
			_conn = conn;
			_currentParcel = data;

			_AddSection = addSection;

			InitializeComponent();

			_checkStory = false;
			_nextSectStory = 0;

			Record = _currentParcel.Record;
			Card = _currentParcel.Card;
			btnAdd.Enabled = false;

			//Rectangle r = Screen.PrimaryScreen.WorkingArea;
			//this.StartPosition = FormStartPosition.Manual;
			//this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (this.Width + 50),
			//    Screen.PrimaryScreen.WorkingArea.Height - (this.Height + 50));

			ListSectionCollection();
			SectionTypesCbox.DataSource = sectTypeList;
			ReSet();
			GetNextSection(Record, Card);

			if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
			{
				CurOccTxt.Text = "Residential Occupancy";
			}
			if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
			{
				CurOccTxt.Text = "Commercial Occupany";
			}
			if (CamraSupport.TaxExemptOccupancies.Contains(_currentParcel.moccup))
			{
				CurOccTxt.Text = "Tax Exempt Occupancy";
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void ChkBase()
		{
		}

		private void GetNextSection(int _record, int _card)
		{
			char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
			nextSec = String.Empty;

			StringBuilder secSql = new StringBuilder();
			secSql.Append(String.Format("select max(jssect) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
						  MainForm.FClib, MainForm.FCprefix, _record, _card));

			try
			{
				nextSec = (string)_conn.DBConnection.ExecuteScalar(secSql.ToString());
			}
			catch
			{
				if (nextSec.Trim() == String.Empty)
				{
					nextSec = "A";

					_checkStory = false;

					if (_nextSectStory == 0 && nextSec == "A")
					{
						_nextSectStory = _currentParcel.mstorN;
						SectionSizeTxt.Text = _nextSectStory.ToString("N2");
					}

					//ChkBase();
				}
			}

			//if (nextSec == null)
			//{
			//    StringBuilder secSql = new StringBuilder();
			//    secSql.Append(String.Format("select max(jssect) from section where jsrecord = {0} and jsdwell = {1} ", _record, _card));

			//    nextSec = (string)_fox.ExecuteScalar(secSql.ToString());
			//}

			if (nextSec.Trim() == String.Empty)
			{
				nextSec = "A";

				_checkStory = true;
				_isnewSketch = true;

				if (_nextSectStory == 0 && nextSec == "A")
				{
					_nextSectStory = _currentParcel.mstorN;
					SectionSizeTxt.Text = _nextSectStory.ToString("N2");
				}

				//ChkBase();
			}

			if (nextSec.Trim() == "M")
			{
				MessageBox.Show("Cannot Add Sections to this sketch");
			}
			if (nextSec.Trim() != "M")
			{
				SectionLetterCbox.SelectedItem = nextSec.Trim();
				int index = SectionLetterCbox.SelectedIndex;

				if (nextSec == "A")
				{
					index = -1;

					_isnewSketch = true;

					_checkStory = true;
				}
				else
				{
					index = SectionLetterCbox.SelectedIndex;
				}

				SectionLetterCbox.SelectedIndex = (index + 1);

				_nextSectLtr = SectionLetterCbox.SelectedItem.ToString().Trim();

				SectLtrTxt.Text = _nextSectLtr;
			}

			if (_AddStory == true)
			{
				if (_nextSectStory <= 0)
				{
					MessageBox.Show("Must enter Story Height", "No Story Warning");
					SectionStoriesTxt.Text = String.Empty;
					SectionStoriesTxt.Focus();
				}
			}
			if (_nextSectStory == 0)
			{
				_nextSectStory = 1.0m;
			}

			if (_AddStory == false && _nextSectStory != 0 && _nextSectLtr != "A")
			{
				//SectionStoriesTxt.Focus();
				SectionStoriesTxt.Text = _nextSectStory.ToString("N2");
			}
		}

		private void ListSectionCollection()
		{
			//    int _cboxIndex = 0;

			sectTypeList = new List<string>();

			// var index = -1;
			if (SectionTypesCbox.SelectedIndex <= 0)
			{
				sectTypeList.Add(" < Section Types > ");

				foreach (var item in CamraSupport.ResidentialSectionTypeCollection)
				{
					string PrtDesc = String.Format("{0} - {1}",
						item._resSectionType.ToString().Trim().PadRight(4, ' '),
						item._resSectionDescription.ToString().Trim());

					sectTypeList.Add(PrtDesc.Trim());
				}
				foreach (var item in CamraSupport.CommercialSectionTypeCollection)
				{
					string PrtDesc = String.Format("{0} - {1}",
								   item._commSectionType.ToString().Trim(),
								   item._commSectionDescription.ToString().Trim());

					sectTypeList.Add(PrtDesc.Trim());
				}
			}
		}

		private void ReSet()
		{
			_AddStory = false;
			_AddType = false;
			SectionTypesCbox.SelectedIndex = 0;
			SectionLetterCbox.SelectedIndex = 0;
			SectionStoriesTxt.Text = String.Empty;
			SectionSizeTxt.Text = String.Empty;
		}

		private void SectionSizeTxt_Leave(object sender, EventArgs e)
		{
			decimal _size = 0;

			if (SectionSizeTxt.Text == String.Empty)
			{
				_blankSize = false;
			}

			if (SectionSizeTxt.Text != String.Empty)
			{
				decimal.TryParse(SectionSizeTxt.Text, out _size);

				_nextSectSize = _size;
				_sizeOnly = _size;

				if (_size > 0)
				{
					SectionSizeTxt.Text = _size.ToString("N1");

					_blankSize = true;
				}
			}

			this.Close();
		}

		private void SectionStoriesTxt_Leave(object sender, EventArgs e)
		{
			decimal _sty = 0;

			_nextSectStory = 0;

			_blankStory = false;

			if (SectionStoriesTxt.Text == String.Empty)
			{
				_blankStory = true;
			}

			if (SectionStoriesTxt.Text != String.Empty)
			{
				decimal.TryParse(SectionStoriesTxt.Text, out _sty);

				decimal curstory = _currentParcel.mstorN;

				_nextSectStory = _sty;

				if (CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					if (nextSec == "A" && _blankStory != true && curstory != _sty)
					{
						DialogResult storycheck;
						storycheck = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
										   MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

						if (storycheck == DialogResult.OK)
						{
							if (nextSec == "A" && curstory != _sty)
							{
								MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), _sty.ToString("N2")));

								_checkStory = false;
							}

							if (_sty > 0)
							{
								SectionStoriesTxt.Text = _sty.ToString("N2");
								_currentParcel.mstorN = _sty;

								StringBuilder maststory = new StringBuilder();
								maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, _sty));
								maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

								//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								_conn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());

								//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								_AddStory = true;
							}

							if (_AddStory == true && _AddType == true)
							{
								this.Close();
							}
						}
						if (storycheck == DialogResult.Cancel)
						{
							this.Close();
						}

						// }
					}
				}

				if (CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
				{
					if (nextSec == "A" && _blankStory != true && curstory != _sty)
					{
						DialogResult storycheck2;
						storycheck2 = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
											MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

						if (storycheck2 == DialogResult.OK)
						{
							if (nextSec == "A" && curstory != _sty)
							{
								MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), _sty.ToString("N2")));

								_checkStory = false;
							}

							if (_sty > 0)
							{
								SectionStoriesTxt.Text = _sty.ToString("N2");
								_currentParcel.mstorN = _sty;

								StringBuilder maststory = new StringBuilder();
								maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, _sty));
								maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

								//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								_conn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());

								//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								_AddStory = true;
							}

							if (_AddStory == true && _AddType == true)
							{
								this.Close();
							}
						}
						if (storycheck2 == DialogResult.Cancel)
						{
							this.Close();
						}
					}
				}
			}
		}

		private void SectionTypesCbox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SectionTypesCbox.SelectedIndex > 0)
			{
				btnAdd.Enabled = true;
				_nextSectType = SectionTypesCbox.SelectedItem.ToString().Substring(0, 4);

				_AddType = true;

				if (nextSec == "A" && CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					if (_nextSectType != "BASE")
					{
						MessageBox.Show("A - Section must be 'BASE' ");
						SectionTypesCbox.Focus();
					}

					SectionStoriesTxt.Text = _currentParcel.mstorN.ToString("N2");
				}
				if (_nextSectLtr != "A" && CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup))
				{
					if (_nextSectType == "BASE")
					{
						MessageBox.Show(String.Format("Section - '{0}' Cannot be 'BASE' ", _nextSectLtr));
						_nextSectType = String.Empty;
						SectionTypesCbox.SelectedIndex = 0;
						SectionTypesCbox.Focus();

						//SectionLetterCbox.Focus();
					}
				}

				if (nextSec == "A" && CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup))
				{
					if (_nextSectType == "BASE")
					{
						DialogResult result;

						//result = MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?");
						result = (MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?", "Residential Sketch Section Warning",
							MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

						if (result == DialogResult.Yes)
						{
						}
						if (result == DialogResult.No)
						{
							_nextSectType = String.Empty;
							SectionTypesCbox.Focus();
						}
					}

					SectionStoriesTxt.Text = _currentParcel.mstorN.ToString("N2");
				}
				if (nextSec != "A" && CamraSupport.CommercialOccupancies.Contains(_currentParcel.moccup) && CamraSupport.InvalidCommercialSection.Contains(_nextSectType.Trim()))
				{
					DialogResult result2;

					//result = MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?");
					result2 = (MessageBox.Show("Commercial Parcel - Invalid Residential Structrue Type !", "Commercial Sketch Section Warning",
						MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation));

					if (result2 == DialogResult.OK)
					{
						_nextSectType = String.Empty;
						SectionTypesCbox.Focus();
					}
					if (result2 == DialogResult.Cancel)
					{
						_nextSectType = String.Empty;
						SectionTypesCbox.Focus();
					}
				}

				SectionStoriesTxt.Focus();
			}
			if (_AddStory == true && _AddType == true)
			{
				this.Close();
			}
		}

		private void Sqftcbox_CheckedChanged(object sender, EventArgs e)
		{
			SectionSizeTxt.Visible = true;
			SizeOnlyLbl.Visible = true;
			SqftLbl.Visible = true;

			SectionSizeTxt.Focus();
		}

		public static int _nextLineCount
		{
			get; set;
		}

		public static string _nextSectLtr
		{
			get; set;
		}

		public static decimal _nextSectSize
		{
			get; set;
		}

		public static decimal _nextSectStory
		{
			get; set;
		}

		public static string _nextSectType
		{
			get; set;
		}

		public static decimal _sizeOnly
		{
			get; set;
		}

		public static string nextSec
		{
			get; set;
		}

		public static bool _AddSection = false;

		public static bool _blankSize = false;

		public static bool _isnewSketch = false;

		private bool _AddStory = false;

		private bool _AddType = false;

		private bool _blankStory = false;

		private bool _checkStory = false;

		// TODO: Remove if not needed:	DBAccessManager _fox = null;
		private SWallTech.CAMRA_Connection _conn = null;

		private ParcelData _currentParcel = null;

		private int Card = 0;
		private int Record = 0;
		private string Section = String.Empty;
		private List<string> sectTypeList = null;
	}
}