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
            parcelWorkingCopy = SketchUpGlobals.ParcelWorkingCopy;
            _AddSection = addSection;

            InitializeComponent();

            _checkStory = false;
            newSectionStoreys = 0;

            Record = parcelMast.Record;
            Card = parcelMast.Card;
            btnAdd.Enabled = false;

            PopulateComboSource();
            SectionTypesCbox.DataSource = sectTypeList;
            ReSet();
            GetNextSection(Record, Card);

            if (SketchUpLookups.ResidentialOccupancies.Contains(parcelMast.OccupancyCode))
            {
                CurOccTxt.Text = "Residential Occupancy";
            }
            if (SketchUpLookups.CommercialOccupancies.Contains(parcelMast.OccupancyCode))
            {
                CurOccTxt.Text = "Commercial Occupany";
            }
            if (CamraDataEnums.GetEnumValues(typeof(CamraDataEnums.TaxExemptOccupancies)).Contains(parcelMast.OccupancyCode))
            {
                CurOccTxt.Text = "Tax Exempt Occupancy";
            }
        }

        private void AddSectionToWorkingParcel()
        {
            decimal storeys = 0.00M;
            SMSection newSection = new SMSection(ParcelWorkingCopy);
            newSection.SectionLetter = SectLtrTxt.Text;
            newSection.SectionType = SectionTypesCbox.SelectedValue.ToString();
            decimal.TryParse(SectionStoriesTxt.Text, out storeys);
            newSection.Storeys = storeys;
            newSection.ParentParcel = ParcelWorkingCopy;
            newSection.Record = SketchUpGlobals.Record;
            newSection.AttachedTo = string.Empty;
            ParcelWorkingCopy.Sections.Add(newSection);
            ParcelWorkingCopy.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(ParcelWorkingCopy);

#if DEBUG
            StringBuilder traceOut = new StringBuilder();
            traceOut.AppendLine(string.Format("New section added: {0}", newSection.SectionLetter));
            traceOut.AppendLine(string.Format("{0}", ""));
            Debug.WriteLine(string.Format("{0}", traceOut.ToString()));

#endif
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSectionToWorkingParcel();

            this.Close();
        }

        private void CheckForMismatchInStoreyCount()
        {
            decimal storeyTextEntered = 0;
            newSectionStoreys = 0;
            storeyTextIsBlank = false;
            if (SectionStoriesTxt.Text == string.Empty)
            {
                storeyTextIsBlank = true;
            }
            else
            {
                decimal.TryParse(SectionStoriesTxt.Text, out storeyTextEntered);
                decimal dbStoreys = ParcelWorkingCopy.ParcelMast.MasterParcelStoreys;
                newSectionStoreys = storeyTextEntered;

                if (nextSec == "A" && storeyTextIsBlank != true && dbStoreys != storeyTextEntered)
                {
                    DialogResult storycheck;
                    storycheck = (MessageBox.Show("Master Story Conflict", "Check Stories Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

                    if (storycheck == DialogResult.OK)
                    {
                        if (nextSec == "A" && dbStoreys != storeyTextEntered)
                        {
                            MessageBox.Show(string.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", dbStoreys.ToString("N2"), storeyTextEntered.ToString("N2")));

                            _checkStory = false;
                        }

                        if (storeyTextEntered > 0)
                        {
                            SectionStoriesTxt.Text = storeyTextEntered.ToString("N2");
                            ParcelWorkingCopy.ParcelMast.MasterParcelStoreys = storeyTextEntered;
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
                }
            }
        }

        private void GetNextSection(int _record, int _card)
        {
            char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            nextSec = ParcelWorkingCopy.NextSectionLetter;
            if (nextSec.Trim() == string.Empty)
            {
                nextSec = "A";

                _checkStory = true;
                _isnewSketch = true;

                if (newSectionStoreys == 0 && nextSec == "A")
                {
                    newSectionStoreys = ParcelWorkingCopy.ParcelMast.MasterParcelStoreys;
                    SectionSizeTxt.Text = newSectionStoreys.ToString("N2");
                }

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
          
            sectTypeList = new List<string>();

            if (SectionTypesCbox.SelectedIndex <= 0)
            {
                sectTypeList.Add("(Select Section Type)");


                foreach (var item in SketchUpLookups.ResidentialSectionTypeCollection)
                {
                    string PrtDesc = string.Format("{0} - {1}",
                        item._resSectionType.ToString().Trim().PadRight(4, ' '),
                        item._resSectionDescription.ToString().Trim());

                    sectTypeList.Add(PrtDesc.Trim());
                }
                foreach (var item in SketchUpLookups.CommercialSectionTypeCollection)
                {
                    string PrtDesc = string.Format("{0} - {1}",
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

        private void SectionStoriesTxt_Leave(object sender, EventArgs e)
        {
            CheckForMismatchInStoreyCount();
        }

        private void SectionTypesCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SectionTypesCbox.SelectedIndex > 0)
            {
                btnAdd.Enabled = true;
                _nextSectType = SectionTypesCbox.SelectedItem.ToString().Substring(0, 4);
                _AddType =ValidateSectionSelection();
            }
            if (_AddStory == true && _AddType == true)
            {
                this.Close();
            }
        }

        private bool ValidateSectionSelection()
        {
            bool selectionValid = false;

            if (SketchUpLookups.ResidentialOccupancies.Contains(ParcelWorkingCopy.ParcelMast.OccupancyCode))
            {
                selectionValid = ValidResidentialSelection();
            }

            //Commercial
            else if (SketchUpLookups.CommercialOccupancies.Contains(ParcelWorkingCopy.ParcelMast.OccupancyCode))
            {

                if (nextSec == "A")
                    PreventAddingBASEtoCommParcel();

                SectionStoriesTxt.Text = ParcelWorkingCopy.SelectSectionByLetter("A").Storeys.ToString("N2");
            }
            else

                if (CamraDataEnums.GetEnumStrings(typeof(CamraDataEnums.InvalidCommercialSection)).Contains(_nextSectType.Trim()))
            {
                DialogResult result2;

                //result = MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?");
                result2 = (MessageBox.Show("Commercial Parcel - Invalid Residential Structure Type !", "Commercial Sketch Section Warning",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation));

                if (result2 == DialogResult.OK)
                {
                    _nextSectType = string.Empty;
                    SectionTypesCbox.Focus();
                }
                if (result2 == DialogResult.Cancel)
                {
                    _nextSectType = string.Empty;
                    SectionTypesCbox.Focus();
                }
            }
            
            SectionStoriesTxt.Focus();
            return selectionValid;
        }

        private bool ValidResidentialSelection()
        {
            bool isValid = false;

            AlertBaseStatusInvalid();

            return isValid;
        }

        private void PreventAddingBASEtoCommParcel()
        {
            if (_nextSectType == "BASE")
            {
                DialogResult result;
                result = (MessageBox.Show("Commercial Parcel - Do you want Residential Structure ?", "Residential Sketch Section Warning",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning));

                if (result == DialogResult.Yes)
                {
                }
                if (result == DialogResult.No)
                {
                    _nextSectType = string.Empty;
                    SectionTypesCbox.Focus();
                }
            }
        }

        private void AlertBaseStatusInvalid()
        {
            if (nextSec == "A")
            {
                if (_nextSectType != "BASE")
                {
                    MessageBox.Show("A - Section must be of type \"BASE\" ", "Base Section Required");
                    SectionTypesCbox.Focus();
                }

              
            }
        else
            {
                if (_nextSectType == "BASE")
                {
                    MessageBox.Show(string.Format("Section - \"{0}\" Cannot be \"BASE\"", _nextSectLtr));
                    _nextSectType = string.Empty;
                    SectionTypesCbox.SelectedIndex = 0;
                    SectionTypesCbox.Focus();
                }
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

        public static string _nextSectType
        {
            get; set;
        }

        public static decimal _sizeOnly
        {
            get; set;
        }

        public static decimal newSectionStoreys
        {
            get; set;
        }

        public static string nextSec
        {
            get; set;
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

        public static bool _AddSection = false;
        public static bool _blankSize = false;
        public static bool _isnewSketch = false;
        private bool _AddStory = false;
        private bool _AddType = false;
        private bool _checkStory = false;
        private SWallTech.CAMRA_Connection _conn = null;
        private int Card = 0;
        private SMParcel parcelWorkingCopy;
        private int Record = 0;
        private string Section = string.Empty;
        private List<string> sectTypeList = null;
        private bool storeyTextIsBlank = false;
    }
}