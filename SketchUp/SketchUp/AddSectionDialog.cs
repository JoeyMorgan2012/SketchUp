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
			dbConn = conn;
			currentParcel = parcelData;
			_AddSection = addSection;
			InitializeComponent();
			_checkStory = false;
			nextSectionStoreys = 0;
			InitializeFormUI();
			SetPanels();
		}

		public AddSectionDialog(CAMRA_Connection conn, ParcelData parcelData, bool addSection, int lineCount, bool newSketch, SMParcel smParcel)
		{
			dbConn = conn;
			currentParcel = parcelData;
			_AddSection = addSection;
			_checkStory = false;
			nextSectionStoreys = 0;
			InitializeComponent();
			InitializeFormUI();
			SetPanels();
			ParcelWorkingCopy = GetParcelFromDataBase(currentParcel);
		}

		private SMParcel GetParcelFromDataBase(ParcelData currentParcel)
		{
			SketchRepository sr = new SketchRepository(dbConn.DataSource, dbConn.User, dbConn.Password, dbConn.LocalityPrefix);
			SMParcel parcel = sr.SelectParcelData(currentParcel.Record, currentParcel.Card);
			parcel.Sections = sr.SelectParcelSections(parcel);
			foreach (SMSection sms in parcel.Sections)
			{
				sms.Lines = sr.SelectSectionLines(sms);
			}
			parcel.IdentifyAttachedToSections();

			return parcel;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private string GetNextSection(int _record, int _card)
		{
			char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
			nextSecLetter = String.Empty;

			StringBuilder secSql = new StringBuilder();
			secSql.Append(String.Format("select max(jssect) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
						  MainForm.FClib, MainForm.FCprefix, _record, _card));

			try
			{
				nextSecLetter = (string)dbConn.DBConnection.ExecuteScalar(secSql.ToString());
			}
			catch
			{
				if (nextSecLetter.Trim() == String.Empty)
				{
					nextSecLetter = "A";

					_checkStory = false;

					if (nextSectionStoreys == 0 && nextSecLetter == "A")
					{
						nextSectionStoreys = currentParcel.mstorN;
						SquareFootageTextBox.Text = nextSectionStoreys.ToString("N2");
					}
				}
			}

			if (nextSecLetter.Trim() == String.Empty)
			{
				nextSecLetter = "A";

				_checkStory = true;
				isNewSketch = true;

				if (nextSectionStoreys == 0 && nextSecLetter == "A")
				{
					nextSectionStoreys = currentParcel.mstorN;
					SquareFootageTextBox.Text = nextSectionStoreys.ToString("N2");
				}

				//ChkBase();
			}

			if (nextSecLetter.Trim() == "M")
			{
				MessageBox.Show("Cannot Add Sections to this sketch");
			}
			if (nextSecLetter.Trim() != "M")
			{
				SectLtr.Text = nextSecLetter.Trim();

				if (nextSecLetter == "A")
				{
					isNewSketch = true;

					_checkStory = true;
				}
			}

			if (addStory == true)
			{
				if (nextSectionStoreys <= 0)
				{
					MessageBox.Show("Must enter Story Height", "No Story Warning");
					SectionStoriesTxt.Text = String.Empty;
					SectionStoriesTxt.Focus();
				}
			}
			if (nextSectionStoreys == 0)
			{
				nextSectionStoreys = 1.0M;
			}

			if (addStory == false && nextSectionStoreys != 0 && _nextSectLtr != "A")
			{
				SectionStoriesTxt.Text = nextSectionStoreys.ToString("N2");
			}
			return _nextSectLtr;
		}

		private void InitializeFormUI()
		{
			Record = currentParcel.Record;
			Card = currentParcel.Card;
			btnAdd.Enabled = false;

			ResetForm();
			List<string> comboBoxData = PopulateSectionTypeCollection();
			SectionTypesCbox.DataSource = comboBoxData;
			SectionTypesCbox.SelectedIndex = -1;
			SectLtr.Text = GetNextSection(Record, Card);

			if (CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
			{
				CurOccTxt.Text = "Residential Occupancy";
			}
			if (CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
			{
				CurOccTxt.Text = "Commercial Occupancy";
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
			rbNewSection.Checked = true;
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
		}

		private void SectionStoriesTxt_Leave(object sender, EventArgs e)
		{
			decimal storey = 0;

			nextSectionStoreys = 0;

			blankStory = false;

			if (SectionStoriesTxt.Text == String.Empty)
			{
				blankStory = true;
			}

			if (!blankStory)
			{
				storey = ProcessBlankStoreyIssue();
			}
		}

		private decimal ProcessBlankStoreyIssue()
		{
			decimal storey;
			{
				decimal.TryParse(SectionStoriesTxt.Text, out storey);
				decimal curstory = currentParcel.mstorN;
				nextSectionStoreys = storey;
				if (CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSecLetter == "A" && blankStory != true && curstory != storey)
						HandleBlankStoreyResidental(storey, curstory);
				}

				if (CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSecLetter == "A" && blankStory != true && curstory != storey)
					{
						DialogResult storycheck2;
						storycheck2 = (MessageBox.Show("Master Story Conflict", "Check Stories Error",
							MessageBoxButtons.OKCancel, 
							MessageBoxIcon.Question));

						if (storycheck2 == DialogResult.OK)
						{
							if (nextSecLetter == "A" && curstory != storey)
							{
								MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), storey.ToString("N2")));

								_checkStory = false;
							}

							if (storey > 0)
							{
								UpdateStoreysInMaster(storey);
								ParcelWorkingCopy.Storeys = storey;

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

			return storey;
		}

		private void UpdateStoreysInMaster(decimal storey)
		{
			SectionStoriesTxt.Text = storey.ToString("N2");
			currentParcel.mstorN = storey;

			StringBuilder maststory = new StringBuilder();
			maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, storey));
			maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", currentParcel.mrecno, currentParcel.mdwell));

			dbConn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());
		}

		private void HandleBlankStoreyResidental(decimal storey, decimal curstory)
		{
			DialogResult storycheck = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
							   MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

			if (storycheck == DialogResult.OK)
			{
				if (nextSecLetter == "A" && curstory != storey)
				{
					MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Stories = {1} ", curstory.ToString("N2"), storey.ToString("N2")));

					_checkStory = false;
				}

				if (storey > 0)
				{
					SectionStoriesTxt.Text = storey.ToString("N2");
					currentParcel.mstorN = storey;

					StringBuilder maststory = new StringBuilder();
					maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", MainForm.FClib, MainForm.FCprefix, storey));
					maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", currentParcel.mrecno, currentParcel.mdwell));

					dbConn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());
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
		}

		private void SectionTypesCbox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (SectionTypesCbox.SelectedIndex > 0)
			{
				btnAdd.Enabled = true;
				nextSectionType = SectionTypesCbox.SelectedItem.ToString().Substring(0, 4);

				addType = true;

				if (nextSecLetter == "A" && CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSectionType != "BASE")
					{
						MessageBox.Show("A - Section must be 'BASE' ");
						SectionTypesCbox.Focus();
					}

					SectionStoriesTxt.Text = currentParcel.mstorN.ToString("N2");
				}
				if (_nextSectLtr != "A" && CamraSupport.ResidentialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSectionType == "BASE")
					{
						MessageBox.Show(String.Format("Section - '{0}' Cannot be 'BASE' ", _nextSectLtr));
						nextSectionType = String.Empty;
						SectionTypesCbox.SelectedIndex = 0;
						SectionTypesCbox.Focus();

						//SectionLetterCbox.Focus();
					}
				}

				if (nextSecLetter == "A" && CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup))
				{
					if (nextSectionType == "BASE")
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
							nextSectionType = String.Empty;
							SectionTypesCbox.Focus();
						}
					}

					SectionStoriesTxt.Text = currentParcel.mstorN.ToString("N2");
				}
				if (nextSecLetter != "A" && CamraSupport.CommercialOccupancies.Contains(currentParcel.moccup) && CamraSupport.InvalidCommercialSection.Contains(nextSectionType.Trim()))
				{
					DialogResult result2;

					//result = MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?");
					result2 = (MessageBox.Show("Commercial Parcel - Invalid Residential Structrue Type !", "Commercial Sketch Section Warning",
						MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation));

					if (result2 == DialogResult.OK)
					{
						nextSectionType = String.Empty;
						SectionTypesCbox.Focus();
					}
					if (result2 == DialogResult.Cancel)
					{
						nextSectionType = String.Empty;
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
				SectLtr.Text = GetNextSection(Record, Card);
				sqFootageGroupBox.Enabled = false;
				addSectionGroupBox.Enabled = true;
				SectionTypesCbox.Focus();
			}
			else
			{
				SectLtr.Text = string.Empty;
				sqFootageGroupBox.Enabled = true;
				addSectionGroupBox.Enabled = false;
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

		public static string nextSecLetter
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

		public static decimal nextSectionStoreys
		{
			get; set;
		}

		public static string nextSectionType
		{
			get; set;
		}

		public static decimal _sizeOnly
		{
			get; set;
		}

		public static SMParcel ParcelWorkingCopy
		{
			get
			{
				return parcelWorkingCopy;
			}

			set
			{
				parcelWorkingCopy = value;
			}
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
		private CAMRA_Connection dbConn = null;
		private ParcelData currentParcel = null;
		public static bool isNewSketch = false;
		private static SMParcel parcelWorkingCopy;

		private void AddSectionDialog_Load(object sender, EventArgs e)
		{
		}

		private void SectionStoriesTxt_Enter(object sender, EventArgs e)
		{
			SectionStoriesTxt.SelectAll();
		}
	}
}