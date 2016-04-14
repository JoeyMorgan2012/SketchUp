using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class SectionAdditionDialog : Form
    {
        public SectionAdditionDialog(SWallTech.CAMRA_Connection conn, ParcelData data, bool addSection, int lineCount, bool newSketch)
        {
            //_conn = conn;
            //_currentParcel = data;

            _AddSection = addSection;

            InitializeComponent();

            storeyNumberChecksOk = false;
            _nextSectStory = 0;

            Record = SketchUpGlobals.Record;
            Card = SketchUpGlobals.Card;
            btnNext.Enabled = false;

            ListSectionCollection();
            SectionTypesCbox.DataSource = sectTypeList;
            InitializeUI();
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
        #region Constructor and Supporting
        #region refactored from legacy constructor

        private void EnableCorrectGroup(AdditionType selectedAdditionType)
        {
            switch (selectedAdditionType)
            {
                case AdditionType.NewSection:
                    newSectionGroup.Enabled = true;
                    squareFootageOnlyGroup.Enabled = false;
                    SectionTypesCbox.Focus();
                    break;

                case AdditionType.SquareFootageOnly:
                    newSectionGroup.Enabled = false;
                    squareFootageOnlyGroup.Enabled = true;
                    SectionSizeTxt.Focus();
                    break;

                default:
                    InitializeUI();
                    break;
            }
        }

        private void GetNextSection(int _record, int _card)
        {
            char[] validSec = new char[] { 'A', 'B', 'C', 'D', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' };
            nextSec = String.Empty;

            StringBuilder secSql = new StringBuilder();
            secSql.Append(String.Format("select max(jssect) from {0}.{1}section where jsrecord = {2} and jsdwell = {3} ",
                          SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _record, _card));

            try
            {
                nextSec = (string)_conn.DBConnection.ExecuteScalar(secSql.ToString());
            }
            catch
            {
                if (nextSec.Trim() == String.Empty)
                {
                    nextSec = "A";

                    storeyNumberChecksOk = false;

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

                storeyNumberChecksOk = true;
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
                SetNextSectionLabel();
            }

            if (storeyCheckOneOk == true)
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

            if (storeyCheckOneOk == false && _nextSectStory != 0 && _nextSectLtr != "A")
            {
                //SectionStoriesTxt.Focus();
                SectionStoriesTxt.Text = _nextSectStory.ToString("N2");
            }
        }

        private void SetNextSectionLabel()
        {
            SectionLetterCbox.SelectedItem = nextSec.Trim();
            int index = SectionLetterCbox.SelectedIndex;

            if (nextSec == "A")
            {
                index = -1;

                _isnewSketch = true;

                storeyNumberChecksOk = true;
            }
            else
            {
                index = SectionLetterCbox.SelectedIndex;
            }

            SectionLetterCbox.SelectedIndex = (index + 1);

            _nextSectLtr = SectionLetterCbox.SelectedItem.ToString().Trim();

            SectLtrTxt.Text = _nextSectLtr;
        }

        private void ListSectionCollection()
        {
            //    int _cboxIndex = 0;

            sectTypeList = new List<string>();

            // var index = -1;
            if (SectionTypesCbox.SelectedIndex <= 0)
            {
                sectTypeList.Add(" (Select Section Type) ");

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

        private void InitializeUI()
        {
            storeyCheckOneOk = false;
            sectionTypeSelected = false;
            AdditionType1 = AdditionType.NewSection;
            SectionTypesCbox.SelectedIndex = 0;
            SectionLetterCbox.SelectedIndex = 0;
            SectionStoriesTxt.Text = String.Empty;
            SectionSizeTxt.Text = String.Empty;
            rbNewSection.Checked = true;
            rbSquareFt.Checked = false;
            rbNewSection.Focus();
        }

        #endregion refactored from legacy constructor 
        #endregion
        private void AddSelectedSectionType(SMParcel parcelWorkingCopy)
        {
            throw new NotImplementedException();
        }

        private void ChkBase()
        {
        }

        #region Control Event Handlers

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddSelectedSectionType(SketchUpGlobals.ParcelWorkingCopy);

            //TODO: refactor
            this.Close();
        }

        private void rbNewSection_CheckedChanged(object sender, EventArgs e)
        {
            AdditionType selectedAdditionType = rbNewSection.Checked ? AdditionType.NewSection : AdditionType.SquareFootageOnly;
            EnableCorrectGroup(selectedAdditionType);
        }

        private void rbSquareFt_CheckedChanged(object sender, EventArgs e)
        {
            AdditionType selectedAdditionType = rbSquareFt.Checked ? AdditionType.SquareFootageOnly : AdditionType.NewSection;
            EnableCorrectGroup(selectedAdditionType);
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
            ValidateSectionStories();
        }

        private void SectionTypes_Load(object sender, EventArgs e)
        {
            InitializeUI();
        }

        private void SectionTypesCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SectionTypesCbox.SelectedIndex > 0)
            {
                btnNext.Enabled = true;
                _nextSectType = SectionTypesCbox.SelectedItem.ToString().Substring(0, 4);

                sectionTypeSelected = true;

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
                        {//ASK DAVE: What is supposed to happen here?
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
            if (storeyCheckOneOk == true && sectionTypeSelected == true)
            {
                this.Close();
            }
        }

        #endregion Control Event Handlers

    

        private void ValidateSectionStories()
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
                bool residentialParcel = CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup);
                bool commercialParcel = CamraSupport.ResidentialOccupancies.Contains(_currentParcel.moccup);
                if (residentialParcel)
                {
                    ProcessResidential(_sty, curstory);
                }

                if (commercialParcel)
                {
                    ProcessCommercial(_sty, curstory);
                }
            }
        }

        private void ProcessCommercial(decimal _sty, decimal curstory)
        {
            if (nextSec == "A" && _blankStory != true && curstory != _sty)
            {
                DialogResult StoryCheckTwoOk;
                StoryCheckTwoOk = (MessageBox.Show("Master Story Confilct", "Check Stories Error",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question));

                if (StoryCheckTwoOk == DialogResult.OK)
                {
                    if (nextSec == "A" && curstory != _sty)
                    {
                        MessageBox.Show(String.Format("Master Parcel shows {0} stories .. Entered Sories = {1} ", curstory.ToString("N2"), _sty.ToString("N2")));

                        storeyNumberChecksOk = false;
                    }

                    if (_sty > 0)
                    {
                        SectionStoriesTxt.Text = _sty.ToString("N2");
                        _currentParcel.mstorN = _sty;
                        UpdateStoriesInWorkingCopy(_sty);

                        storeyCheckOneOk = true;
                    }

                    if (storeyCheckOneOk == true && sectionTypeSelected == true)
                    {
                        this.Close();
                    }
                }
                if (StoryCheckTwoOk == DialogResult.Cancel)
                {
                    this.Close();
                }
            }
        }

        //TODO: Move this to the competion step--leave the master alone.
        //private void UpdateStoriesInMaster(decimal numStories)
        //{
        //    StringBuilder maststory = new StringBuilder();
        //    maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, numStories));
        //    maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

        //    _conn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());
        //}
        private void UpdateStoriesInWorkingCopy(decimal numStories)
        {
            SketchUpGlobals.ParcelWorkingCopy.Storeys = numStories;
        }

        private void ProcessResidential(decimal _sty, decimal curstory)
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

                        storeyNumberChecksOk = false;
                    }

                    if (_sty > 0)
                    {
                        //SectionStoriesTxt.Text = _sty.ToString("N2");
                        //_currentParcel.mstorN = _sty;

                        //StringBuilder maststory = new StringBuilder();
                        //maststory.Append(String.Format("update {0}.{1}mast set mstor# = {2} ", SketchUpGlobals.FcLib, SketchUpGlobals.FcLocalityPrefix, _sty));
                        //maststory.Append(String.Format("where mrecno = {0} and mdwell = {1} ", _currentParcel.mrecno, _currentParcel.mdwell));

                        //_conn.DBConnection.ExecuteNonSelectStatement(maststory.ToString());

                        UpdateStoriesInWorkingCopy(_sty);
                        storeyCheckOneOk = true;
                    }

                    if (storeyCheckOneOk == true && sectionTypeSelected == true)
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

        #region enums
        private enum AdditionType
        {
            NewSection,
            SquareFootageOnly
        } 
        #endregion

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

        private AdditionType AdditionType1
        {
            get
            {
                return additionType;
            }

            set
            {
                additionType = value;
            }
        }

        public SMParcel ParcelSnapshot
        {
            get
            {
                return parcelSnapshot;
            }

            set
            {
                parcelSnapshot = value;
            }
        }

        public static bool _AddSection = false;

        public static bool _blankSize = false;

        public static bool _isnewSketch = false;

        private bool _blankStory = false;

        // TODO: Remove if not needed:	DBAccessManager _fox = null;
        private SWallTech.CAMRA_Connection _conn = null;

        private ParcelData _currentParcel = SketchUpGlobals.CurrentParcel;
        private SMParcel parcelSnapshot;
        private AdditionType additionType = AdditionType.NewSection;
        private int Card = 0;
        private int Record = 0;
        private string Section = String.Empty;
        private bool sectionTypeSelected = false;
        private List<string> sectTypeList = null;
        private bool storeyCheckOneOk = false;
        private bool storeyNumberChecksOk = false;
    }
}