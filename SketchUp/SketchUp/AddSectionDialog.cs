using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
	public partial class AddSectionDialog : Form
	{
		public AddSectionDialog(CAMRA_Connection conn, ParcelData parcelData, bool addSection, int lineCount, bool newSketch)
		{
			deConn = conn;
			currentParcel = parcelData;
			_AddSection = addSection;
			InitializeComponent();
			_checkStory = false;
			_nextSectStory = 0;
			InitializeFormUI();
			SetPanels();
		}
		public AddSectionDialog(CAMRA_Connection conn, ParcelData parcelData, bool addSection, int lineCount, bool newSketch,SMParcel smParcel)
		{
			deConn = conn;
			currentParcel = parcelData;
			_AddSection = addSection;
			
			InitializeComponent();
			_checkStory = false;
			_nextSectStory = 0;
			InitializeFormUI();
			SetPanels();
		}
		private void btnAdd_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private string GetNextSection(int _record, int _card)
		{
			char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
			nextSec = String.Empty;

			StringBuilder secSql = new StringBuilder();
			secSql.Append(String.Format("select max(jssect) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
						  MainForm.FClib, MainForm.FCprefix, _record, _card));

			try
			{
				nextSec = (string)deConn.DBConnection.ExecuteScalar(secSql.ToString());
			}
			catch
			{
				if (nextSec.Trim() == String.Empty)
				{
					nextSec = "A";

					_checkStory = false;

					if (_nextSectStory == 0 && nextSec == "A")
					{
						_nextSectStory = currentParcel.mstorN;
						SquareFootageTextBox.Text = _nextSectStory.ToString("N2");
					}
				}
			}

			if (nextSec.Trim() == String.Empty)
			{
				nextSec = "A";

				_checkStory = true;
				isNewSketch = true;

				if (_nextSectStory == 0 && nextSec == "A")
				{
					_nextSectStory = currentParcel.mstorN;
					SquareFootageTextBox.Text = _nextSectStory.ToString("N2");
				}

				//ChkBase();
			}

			if (nextSec.Trim() == "M")
			{
				MessageBox.Show("Cannot Add Sections to this sketch");
			}
			if (nextSec.Trim() != "M")
			{
				newSectionLetterLabel.Text = nextSec.Trim();

				if (nextSec == "A")
				{
					isNewSketch = true;

					_checkStory = true;
				}
			}

			if (addStory == true)
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
				_nextSectStory = 1.0M;
			}

			if (addStory == false && _nextSectStory != 0 && _nextSectLtr != "A")
			{
				_nextSectLtr = _nextSectStory.ToString("N2");
			}
			return _nextSectLtr;
		}

		private void InitializeFormUI()
		{
			Record = currentParcel.Record;
			Card = currentParcel.Card;
			btnAdd.Enabled = false;

			ResetForm();
			List<string>comboBoxData= PopulateSectionTypeCollection();
			SectionTypesCbox.DataSource = comboBoxData;
			SectionTypesCbox.SelectedIndex = -1;


			if (CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
			{
				CurOccTxt.Text = "Residential Occupancy";
			}
			if (CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
			{
				CurOccTxt.Text = "Commercial Occupany";
			}
			if (CamraSupport.TaxExemptOccupancies.Contains(currentParcel.moccup))
			{
				CurOccTxt.Text = "Tax Exempt Occupancy";
			}
		}

		private List<string> PopulateSectionTypeCollection()
		{
			List<string> sectionTypes = new List<string>();

			if (SectionTypesCbox.SelectedIndex <= 0)
			{
				sectionTypes.Add(" (Select Section Type)");

				foreach (var item in CamraSupport.ResidentialSectionTypeCollection)
				{
					string PrtDesc = String.Format("{0} - {1}",
						item._resSectionType.ToString().Trim().PadRight(4, ' '),
						item._resSectionDescription.ToString().Trim());

					sectionTypes.Add(PrtDesc.Trim());
				}
				foreach (var item in CamraSupport.CommercialSectionTypeCollection)
				{
					string PrtDesc = String.Format("{0} - {1}",
								   item._commSectionType.ToString().Trim(),
								   item._commSectionDescription.ToString().Trim());

					sectionTypes.Add(PrtDesc.Trim());
				}
			}
			return sectionTypes;
		}

		private void rbNewSection_CheckedChanged(object sender, EventArgs e)
		{
			SetPanels();
		}

		private void rbSquareFootage_CheckedChanged(object sender, EventArgs e)
		{
			SetPanels();
		}

		private void ResetForm()
		{
			addStory = false;
			addType = false;
			SectionTypesCbox.SelectedIndex = -1;

			SectionStoriesTxt.Text = String.Empty;
			SquareFootageTextBox.Text = String.Empty;
		}

		private void SectionSizeTxt_Leave(object sender, EventArgs e)
		{
			decimal _size = 0;

			if (SquareFootageTextBox.Text == String.Empty)
			{
				blankSize = false;
			}

			if (SquareFootageTextBox.Text != String.Empty)
			{
				decimal.TryParse(SquareFootageTextBox.Text, out _size);

				_nextSectSize = _size;
				_sizeOnly = _size;

				if (_size > 0)
				{
					SquareFootageTextBox.Text = _size.ToString("N1");

					blankSize = true;
				}
			}

			this.Close();
		}

		private void SectionStoriesTxt_Leave(object sender, EventArgs e)
		{
			decimal storey = 0;

			_nextSectStory = 0;

			blankStory = false;

			if (SectionStoriesTxt.Text == String.Empty)
			{
				blankStory = true;
			}

			if (!blankStory)
			{
				decimal.TryParse(SectionStoriesTxt.Text, out storey);
				decimal curstory = currentParcel.mstorN;
				_nextSectStory = storey;
				if (CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSec == "A" && blankStory != true && curstory != storey)
					{
						DialogResult storycheck;
						storycheck = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
										   MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

						if (storycheck == DialogResult.OK)
						{
							if (nextSec == "A" && curstory != storey)
							{
								MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), storey.ToString("N2")));

								_checkStory = false;
							}

							if (storey > 0)
							{
								SectionStoriesTxt.Text = storey.ToString("N2");
								currentParcel.mstorN = storey;

								StringBuilder maststory = new StringBuilder();
								maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, storey));
								maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", currentParcel.mrecno, currentParcel.mdwell));

							deConn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());
								addStory = true;
							}

							if (addStory == true && addType == true)
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

				if (CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSec == "A" && blankStory != true && curstory != storey)
					{
						DialogResult storycheck2;
						storycheck2 = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
											MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

						if (storycheck2 == DialogResult.OK)
						{
							if (nextSec == "A" && curstory != storey)
							{
								MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), storey.ToString("N2")));

								_checkStory = false;
							}

							if (storey > 0)
							{
								SectionStoriesTxt.Text = storey.ToString("N2");
								currentParcel.mstorN = storey;

								StringBuilder maststory = new StringBuilder();
								maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, storey));
								maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", currentParcel.mrecno, currentParcel.mdwell));

								//UtilityMethods.LogSqlExecutionAttempt(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								deConn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());

								//UtilityMethods.LogSqlExecutionSuccess(MethodBase.GetCurrentMethod().Name, "maststory", maststory);
								addStory = true;
							}

							if (addStory == true && addType == true)
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

				addType = true;

				if (nextSec == "A" && CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
				{
					if (_nextSectType != "BASE")
					{
						MessageBox.Show("A - Section must be 'BASE' ");
						SectionTypesCbox.Focus();
					}

					SectionStoriesTxt.Text = currentParcel.mstorN.ToString("N2");
				}
				if (_nextSectLtr != "A" && CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
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

				if (nextSec == "A" && CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
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

					SectionStoriesTxt.Text = currentParcel.mstorN.ToString("N2");
				}
				if (nextSec != "A" && CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup) && CamraSupport.InvalidCommercialSection.Contains(_nextSectType.Trim()))
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
			if (addStory == true && addType == true)
			{
				this.Close();
			}
		}

		private void SetPanels()
		{
			if (rbNewSection.Checked)
			{
				SectLtrLabel.Text = GetNextSection(Record, Card);
				sqFootagePanel.Enabled = false;
				newSectionPanel.Enabled = true;
				SectionTypesCbox.Focus();
			}
			else
			{
				SectLtrLabel.Text = string.Empty;
				sqFootagePanel.Enabled = true;
				newSectionPanel.Enabled = false;
				SquareFootageTextBox.Focus();
			}
		}

		private void sqFootageGroup_Enter(object sender, EventArgs e)
		{
			SquareFootageTextBox.Focus();
		}

		private void Sqftcbox_CheckedChanged(object sender, EventArgs e)
		{
			SquareFootageTextBox.Visible = true;
			SizeOnlyLbl.Visible = true;
			SqftLbl.Visible = true;

			SquareFootageTextBox.Focus();
		}

		public static string nextSec
		{
			get; set;
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

		private int Card = 0;
		private int Record = 0;
		private string Section = String.Empty;
		public static bool _AddSection = false;
		private bool addStory = false;
		private bool addType = false;
		public static bool blankSize = false;
		private bool blankStory = false;
		private bool _checkStory = false;
		private CAMRA_Connection deConn = null;
		private ParcelData currentParcel = null;
		public static bool isNewSketch = false;
		private SMParcel parcelWorkingCopy;
	}
}