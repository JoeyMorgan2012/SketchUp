using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class SelectSectionTypeDialog : Form
    {
        public SelectSectionTypeDialog(SMParcelMast parcelMast, bool addSection, int lineCount, bool newSketch)
        {
            ParcelMaster = parcelMast;
;
            _AddSection = addSection;

            InitializeComponent();

            _checkStory = false;
            newSectionStoreys = 0;
            SectionStoriesTxt.Text = ParcelMaster.Parcel.SelectSectionByLetter("A").StoreysText.ToString();
            SectionStoriesTxt.Focus();

            Record = ParcelMaster.Record;
            Card = ParcelMaster.Card;
            btnNext.Enabled = false;

            PopulateComboSource();

            ReSet();
            GetNextSection(Record, Card);

            if (SketchUpLookups.ResidentialOccupancies.Contains(ParcelMaster.OccupancyCode))
            {
                CurOccTxt.Text = "Residential Occupancy";
            }
            if (SketchUpLookups.CommercialOccupancies.Contains(ParcelMaster.OccupancyCode))
            {
                CurOccTxt.Text = "Commercial Occupany";
            }
            if (CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.TaxExemptOccupancies)).Contains(ParcelMaster.OccupancyCode))
            {
                CurOccTxt.Text = "Tax Exempt Occupancy";
            }
        }

#region "Private methods"

        private void AddSectionToWorkingParcel()
        {
          
            decimal storeys = 0.00M;
            SMSection newSection = new SMSection(ParcelMaster.Parcel);
            newSection.SectionLetter = SectLtrTxt.Text;
            newSection.SectionType = SectionTypesCbox.SelectedValue.ToString();
            decimal.TryParse(SectionStoriesTxt.Text, out storeys);
            newSection.StoreysValue = storeys;
            newSection.ParentParcel = ParcelMaster.Parcel;
            newSection.Record = SketchUpGlobals.Record;
            newSection.AttachedTo = string.Empty;
            ParcelMaster.Parcel.Sections.Add(newSection);
            ParcelMaster.Parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(ParcelMaster.Parcel);
            SectionWasAdded = true;

        }

        private bool BaseIsValid()
        {
            bool baseIsValid = false;
            if (nextSec == "A" && ParcelMaster.OccupancyType == CamraDataEnums.OccupancyType.Residential && _nextSectType != "BASE")
            {
                MessageBox.Show("A - Section must be of type \"BASE\" ", "Base Section Required");
                SectionTypesCbox.Focus();
                baseIsValid = false;
            }
            else if (_nextSectType == "BASE" && nextSec == "A" && ParcelMaster.OccupancyType != CamraDataEnums.OccupancyType.Residential)

            {
                MessageBox.Show(string.Format("Section - \"{0}\" Cannot be \"BASE\"", _nextSectLtr));
                _nextSectType = string.Empty;
                SectionTypesCbox.SelectedIndex = 0;
                SectionTypesCbox.Focus();
                baseIsValid = false;
            }
            else
            {
                baseIsValid = true;
            }
            return baseIsValid;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ValidateAndAddSection();
        }

        private void GetNextSection(int _record, int _card)
        {
            char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            nextSec = ParcelMaster.Parcel.NextSectionLetter;
            if (nextSec.Trim() == string.Empty)
            {
                nextSec = "A";

                _checkStory = true;
                _isnewSketch = true;

                if (newSectionStoreys == 0 && nextSec == "A")
                {
                    newSectionStoreys = ParcelMaster.StoreysValue;
                    SectionSizeTxt.Text = newSectionStoreys.ToString("N2");
                }
            }
            SectLtrTxt.Text = nextSec;
            if (nextSec.Trim() == "M")
            {
                MessageBox.Show("Cannot Add Sections to this sketch");
            }

       

            if (_AddStory == true)
            {
                if (newSectionStoreys <= 0)
                {
                    MessageBox.Show("Must enter Story Height", "No Story Warning");
                    SectionStoriesTxt.Text = string.Empty;
                    SectionStoriesTxt.Focus();
                }
            }
            if (newSectionStoreys == 0)
            {
                newSectionStoreys = 1.0m;
            }

            if (_AddStory == false && newSectionStoreys != 0 && _nextSectLtr != "A")
            {
                SectionStoriesTxt.Text = newSectionStoreys.ToString("N2");
            }

        }

        private void PopulateComboSource()
        {
            List<ListOrComboBoxItem> sectionsList = new List<ListOrComboBoxItem>();

            sectionsList.Add(new ListOrComboBoxItem
            {
                Code = string.Empty,
                Description = "(Select Section Type)"
            });
            sectionsList.AddRange(SketchUpLookups.SectionsByOccupancy(ParcelMaster.OccupancyType));
            SectionTypesCbox.DataSource = sectionsList;
            SectionTypesCbox.ValueMember = "Code";
            SectionTypesCbox.DisplayMember = "Description";
            SectionLetterCbox.SelectedIndex = 0;
        }

        private void ReSet()
        {
            _AddStory = false;
            _AddType = false;
            SectionTypesCbox.SelectedIndex = 0;

            // SectionLetterCbox.SelectedIndex = 0;
            SectionStoriesTxt.Text = string.Empty;
            SectionSizeTxt.Text = string.Empty;
        }

        private void SectionSizeTxt_Leave(object sender, EventArgs e)
        {
            decimal _size = 0;

            if (SectionSizeTxt.Text == string.Empty)
            {
                _blankSize = false;
            }

            if (SectionSizeTxt.Text != string.Empty)
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

        private void SectionTypesCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SectionTypesCbox.SelectedIndex > 0)
            {
                btnNext.Enabled = true;
                _nextSectType = SectionTypesCbox.SelectedValue.ToString();
            }
        }

        private void Sqftcbox_CheckedChanged(object sender, EventArgs e)
        {
            SectionSizeTxt.Visible = true;
            SizeOnlyLbl.Visible = true;
            SqftLbl.Visible = true;

            SectionSizeTxt.Focus();
        }

        private bool StoreyCountMatches()
        {
            decimal storeyTextValue = 0;
            string storeyTextEntered = string.Empty;
            bool storeyCountMatches = false;
            newSectionStoreys = 0;
            decimal dbStoreys = 0.00M;
            storeyTextIsBlank = false;
            if (SectionStoriesTxt.Text == string.Empty)
            {
                storeyTextIsBlank = true;
            }
            else
            {
                if (SectionStoriesTxt.Text=="S/L"|| SectionStoriesTxt.Text == "S/F")
                {
                    storeyTextValue = 1.0M;
                }
                else
                {
                   
                    decimal.TryParse(SectionStoriesTxt.Text, out storeyTextValue);
                    dbStoreys = ParcelMaster.StoreysValue;
                    newSectionStoreys = storeyTextValue;
                }
                
            }
            if (nextSec == "A" && storeyTextIsBlank != true && dbStoreys != storeyTextValue)
            {
                //TODO: Cross-reference text values to numeric.
                // Update text field as well.

                DialogResult storycheck;
                FormattableString warningMessage = $"Master Parcel shows {dbStoreys.ToString("N2")} stories .. Entered Stories = {storeyTextValue.ToString("N2")}.\nDo you want to update the Master Parcel Record?";
                FormattableString message = $"Master Parcel shows {dbStoreys.ToString("N2")} stories.\n\n Entered Stories = {storeyTextValue.ToString("N2")}.\n\nDo you want to update the Master Parcel Record?";
                string title = "Story Conflict";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
                storycheck = MessageBox.Show(message.ToString(), title, buttons, icon, defButton);
             

                if (storycheck == DialogResult.Yes)
                {
                    if (SectionStoriesTxt.Text == "S/L" || SectionStoriesTxt.Text == "S/F")
                    {
                        storeyTextValue = 1.0M;
                    }
                    else
                    {

                        decimal.TryParse(SectionStoriesTxt.Text, out storeyTextValue);
                      
                    }
                    if (storeyTextValue > 0)
                    {
                        SectionStoriesTxt.Text = storeyTextValue.ToString("N2");

                        ParcelMaster.StoreysValue = storeyTextValue;
                        _AddStory = true;
                        storeyCountMatches = true;
                    }
                }
                else
                {
                    storeyCountMatches = false;
                }
            }
            else
            {
                storeyCountMatches = true;
            }
            
            return storeyCountMatches;
        }

        private void ValidateAndAddSection()
        {
            _AddType = ValidSectionSelection();
            _AddStory = StoreyCountMatches();

            if (_AddStory == true && _AddType == true)
            {
                AddSectionToWorkingParcel();
                this.Close();
            }
            else
            {
                string message = "There are errors in the section definition. Do you wish to change them?";
                DialogResult result = MessageBox.Show(message, "Fix errors?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (result)
                {
                    case DialogResult.Yes:
                        SectionStoriesTxt.Text = string.Format("{0:N2}", ParcelMaster.StoreysValue);
                        SectionTypesCbox.Focus();
                        break;

                    default:
                        this.Close();
                        break;
                }
            }
        }

        private void ValidateBaseSectionType()
        {
            _nextSectType = string.Empty;
            SectionTypesCbox.Focus();
        }

        private bool ValidSectionSelection()
        {
            bool baseValid = false;
            baseValid = BaseIsValid();
            bool selectionValid = true;
        
            //Commercial

            if (ParcelMaster.OccupancyType == CamraDataEnums.OccupancyType.Commercial && CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.InvalidCommercialSection)).Contains(_nextSectType.Trim()))
            {
                MessageBox.Show("Commercial Parcel - Invalid Residential Structure Type !", "Commercial Sketch Section Warning",
                     MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                _nextSectType = string.Empty;
                SectionTypesCbox.Focus();
                selectionValid = false;
            }

            return selectionValid && baseValid;
        }

#endregion

        public static decimal newSectionStoreys
        {
            get; set;
        }

        public static string nextSec
        {
            get; set;
        }

        public SMParcelMast ParcelMaster
        {
            get
            {
                return parcelMaster;
            }
            set
            {
                parcelMaster = value;
            }
        }

        public SMParcel ParcelWorkingCopy
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

        public bool SectionWasAdded
        {
            get
            {
                return sectionWasAdded;
            }
            set
            {
                sectionWasAdded = value;
            }
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

        public static string _nextSectType
        {
            get; set;
        }

        public static decimal _sizeOnly
        {
            get; set;
        }

        //   private bool _checkStory = false;
        // private SWallTech.CAMRA_Connection _conn = null;
        private int Card = 0;
        private SMParcelMast parcelMaster;
        private SMParcel parcelWorkingCopy;
        private int Record = 0;
        private string Section = string.Empty;
        private bool sectionWasAdded = false;
        private bool storeyTextIsBlank = false;
        public static bool _AddSection = false;
        private bool _AddStory = false;
        private bool _AddType = false;
        public static bool _blankSize = false;
        private bool _checkStory = false;
        public static bool _isnewSketch = false;
    }
}
