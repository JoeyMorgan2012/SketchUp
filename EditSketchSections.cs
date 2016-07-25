/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/
/*  CAMRA SketchUp Version 1.0
     Add-on to CAMRA_UP (Computer Aided Mass Re-Assessment)
     © 2009,2012 Stonewall Technologies, Inc.
     Portions © Blue Ridge Mass Appraisal, used by permission.
     Developed by: Joel Cohen, David Hickey, Joseph Morgan CSM
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class EditSketchSections : Form
    {
        #region Public Methods



        #endregion Public Methods

        #region Public Properties

        public SectionEditState EditState { get; set; }

        public SMParcel Parcel { get; private set; }

        public SMParcelMast ParcelMast => Parcel.ParcelMast;

        public List<ListOrComboBoxItem> SectionCboList
        {
            get {
                if (sectionCboList == null)
                {
                    sectionCboList = new List<ListOrComboBoxItem>();
                }
                return sectionCboList;
            }
        }

        public DataGridViewRow SelectedRow
        {
            get {
                if (dgvSections.SelectedRows.Count > 0)
                {
                    selectedRow = dgvSections.SelectedRows[0];
                }
                return selectedRow;
            }

            set {
                selectedRow = value;
            }
        }

        public SMSection SelectedSection
        {
            get {
                if (SelectedRow != null && !string.IsNullOrEmpty(SelectedSectionLetter))
                {
                    selectedSection = Parcel.SelectSectionByLetter(SelectedRow.Cells["SectionLetter"].Value.ToString());
                }
                else
                {
                    selectedSection = Parcel.SelectSectionByLetter("A");
                }
                return selectedSection;
            }

            set {
                selectedSection = value;
            }
        }

        public string SelectedSectionLetter
        {
            get {
                if (SelectedRow != null)
                {
                    selectedSectionLetter = SelectedRow.Cells["SectionLetter"].Value.ToString();
                }
                else
                {
                    //selectedSectionLetter =lblSectionLetter.Text ?? "A";
                }
                return selectedSectionLetter;
            }

            set {
                selectedSectionLetter = value;
            }
        }

        public bool ShowProgressBar { get; set; }

        public SketchRepository SketchRepo
        {
            get {
                if (sketchRepo == null && Parcel != null)
                {
                    sketchRepo = new SketchRepository(Parcel);
                }
                return sketchRepo;
            }
        }

        public Bitmap StatusImage { get; set; }

        public string StatusText { get; set; }

        public decimal TotalArea { get; set; } = 0.00M;

        public decimal TotalLivingArea { get; set; } = 0.00M;

        public bool UnsavedChangesExist { get; set; }

        #endregion Public Properties

        #region Public Enums

        public enum SectionEditState
        {
            Loading,
            Loaded,
            Ready,
            Edited,
            Saved,
            Saving
        }

        #endregion Public Enums

        #region Private Methods

        private void btnDeleteSection_Click(object sender, EventArgs e)
        {
            //MarkSectionForDeletion();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            CloseWithSavePrompt();
        }

        private void CalculateAreaTotals()
        {
            List<string> laTypes = (from la in SketchUpLookups.LivingAreaSectionTypeCollection select la._LAattSectionType).ToList();
            TotalArea = (from s in Parcel.Sections select s.SqFt * (decimal)s.StoriesValue).Sum();
            TotalLivingArea = (from s in Parcel.Sections where laTypes.Contains(s.SectionType) select s.SqFt * (decimal)s.StoriesValue).Sum();
        }

        private void cboClass_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateNewClass(sender);
        }

        private void cboType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UpdateNewSectionType(sender);
        }

        private bool CheckGaragesAndCarports()
        {
            bool carsOk = false;

            SMParcel dbVersion = SketchUpGlobals.SMParcelFromData;
            int priorCarports = (from s in dbVersion.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).Count();
            int currentCarports = (from s in Parcel.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).Count();
            int priorGarages = (from s in dbVersion.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).Count();
            int currentGarages = (from s in Parcel.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).Count();
            if (currentCarports == 0)
            {
                SetCarportsToZero();
            }
            if (currentGarages == 0)
            {
                SetGaragesToZero();
            }
            if (currentGarages != priorGarages || currentCarports != priorCarports)
            {
                var mgd = new MissingGarageData(Parcel.ParcelMast);
                ParcelMast.Garage1NumCars = mgd.Gar1NumCars;
                ParcelMast.Garage1TypeCode = mgd.Garage1Code;
                ParcelMast.Garage2NumCars = mgd.Gar2NumCars;
                ParcelMast.Garage2TypeCode = mgd.Garage2Code;
                ParcelMast.CarportNumCars = mgd.CarportNumCars;
                ParcelMast.CarportTypeCode = mgd.CarportCode;
            }
            carsOk = true;
            return carsOk;
        }

        private void chkDeleteSection_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDeleteSection.Checked && !setByCode)
            {
                deleteSection = ConfirmDeletion(lblSectionLetter.Text) == DialogResult.Yes;
            }
        }

        private void chkZeroDepr_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedRow != null && !setByCode)
            {
                newZeroDepr = chkZeroDepr.Checked ? "Y" : string.Empty;
            }
        }

        private void ClearDetails()
        {
            cboSectionType.SelectedIndex = -1;
            txtStories.Text = string.Empty;
            txtStories.Enabled = false;
            cboSectionType.SelectedIndex = 0;
            cboSectionType.Enabled = false;
            sizeTextLabel.Text = string.Empty;
            txtFactor.Text = string.Empty;
            txtDepr.Text = string.Empty;
            lblRateValue.Text = string.Empty;
            lblValueText.Text = string.Empty;
            lblNewValueText.Text = string.Empty;
            SelectedSection = null;
        }

        private void CloseWithSavePrompt()
        {
            if (UnsavedChangesExist)
            {
                UseWaitCursor = true;
                EditState = SectionEditState.Saving;
                UpdateWorkingParcelModel();
                Application.DoEvents();
                Close();
            }
        }

        private void ComputeNewSectionValue()
        {
            newValue = SelectedSection.ComputedSectionValue(SelectedSection.ParentParcel.ParcelMast.OccupancyType, newSectionType, newSectionClass, newFactor, newDepreciation, hasNewZeroDepr);
        }

        private DialogResult ConfirmDeletion(string sectionLetter)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            setByCode = true;
            string title = string.Empty;
            string warning = string.Empty;
            MessageBoxDefaultButton defButton = MessageBoxDefaultButton.Button2;
            SelectedSection = Parcel.SelectSectionByLetter(sectionLetter);
            if (sectionLetter == "A")
            {
                title = "Action Unavailable";
                warning = $"You can only delete the {SelectedSection.SectionType} section (A) if you delete the entire sketch. Do this in the main sketch screen using the \"Delete Sketch\" button.";
                buttons = MessageBoxButtons.OK;
                icon = MessageBoxIcon.Information;
                return MessageBox.Show(warning, title, buttons, icon, defButton);
            }
            else
            {
                title = $"Delete section {sectionLetter}?";
                buttons = MessageBoxButtons.YesNo;
                warning = $"Delete section {sectionLetter} ({SelectedSection.SectionType})?\n\nNOTE: This cannot be undone--you will have to redraw the section once you save! (Uncheck the box to cancel deletion.)";
                return MessageBox.Show(warning, title, buttons, icon, defButton);
            }
        }

        private DataTable CreateSectionsDataTable()
        {
            var dt = new DataTable("sectionDt");

            dt.Columns.Add(new DataColumn
            {
                ColumnName = "SectionLetter",
                Caption = "Section",
                DataType = typeof(string),
                AllowDBNull =
                false
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "SectionType",
                Caption = "Type",
                DataType = typeof(string),
                AllowDBNull =
                false
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "Description",
                Caption = "Description",
                DataType = typeof(string),
                AllowDBNull
                = true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "StoriesText",
                Caption = "Story",
                DataType = typeof(string),
                AllowDBNull =
                true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "SectionSize",
                Caption = "Size",
                DataType = typeof(string),
                AllowDBNull =
                false
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "ZeroDepr",
                Caption = "0 Depr",
                DataType = typeof(string),
                AllowDBNull =
                        true,
                DefaultValue = string.Empty
            });

            dt.Columns.Add(new DataColumn
            {
                ColumnName = "SectionClass",
                Caption = "Class",
                DataType = typeof(string),
                AllowDBNull = true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "AdjFactor",
                Caption = "Factor",
                DataType = typeof(int),
                AllowDBNull = true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "Depreciation",
                Caption = "Depr",
                DataType = typeof(double),
                AllowDBNull = true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "Rate",
                Caption = "Rate",
                DataType = typeof(decimal),
                AllowDBNull = true
            });
            dt.Columns.Add(new DataColumn
            {
                ColumnName = "Value",
                Caption = "Value",
                DataType = typeof(int),
                AllowDBNull = true
            });

            dt.Columns.Add(new DataColumn
            {
                ColumnName = "DeleteSection",
                Caption = "Delete",
                DataType = typeof(bool),
                AllowDBNull = true,
                DefaultValue = false
            });
            return dt;
        }

        private DataTable CreateSectionsDt()
        {
            try
            {
                DataTable dt = CreateSectionsDataTable();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    dgvSections.Columns[i].DataPropertyName = dt.Columns[i].ColumnName;
                }

                return dt;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);
                Console.WriteLine(errMessage);
#if DEBUG

                MessageBox.Show(errMessage);
#endif
                throw;
            }
        }

        private void dgvSections_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            string title = "Data Error Message";
            FormattableString message = $"Column {e.ColumnIndex} has an error {e.Exception.Message} - {e.Exception.InnerException.Message}";
            Console.WriteLine($"{message}");
            Trace.WriteLine($"{e.Exception.StackTrace}");
            Trace.WriteLine($"{e.Exception.Source}");
            Trace.WriteLine($"{e.Exception.TargetSite}");
#if DEBUG
            MessageBoxIcon icon = MessageBoxIcon.Error;
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            MessageBox.Show(message.ToString(), title, buttons, icon);
#endif
        }

        private void dgvSections_Enter(object sender, EventArgs e)
        {
            ShowDetails(SelectedRow);
        }

        private void dgvSections_SelectionChanged(object sender, EventArgs e)
        {
            RefreshFormInformation();
        }

        private void EditSketchSections_Load(object sender, EventArgs e)
        {
            EditState = SectionEditState.Loaded;
        }

        private void FillDataGridView()
        {
            CalculateAreaTotals();
            DataTable sections = CreateSectionsDt();
            FillDataTable(sections);
            dgvSections.DataSource = sections;
            for (int i = 0; i < sections.Columns.Count; i++)
            {
                dgvSections.Columns[i].DataPropertyName = sections.Columns[i].ColumnName;
            }
        }

        private void FillDataTable(DataTable dt)
        {
            dt.Rows.Clear();
            foreach (SMSection s in Parcel.Sections.OrderBy(s => s.SectionLetter).Where(s => !s.DeleteSection))
            {
                DataRow row = dt.NewRow();
                row.SetField("SectionLetter", s.SectionLetter);
                row.SetField("SectionType", s.SectionType);
                row.SetField("Description", s.Description);
                row.SetField("StoriesText", s.StoriesText);
                row.SetField("SectionSize", s.SqFt);
                row.SetField("ZeroDepr", s.ZeroDepr.ToUpper().Trim());
                row.SetField("SectionClass", s.SectionClass.ToUpper());
                row.SetField("AdjFactor", s.AdjFactor);
                row.SetField("Depreciation", s.Depreciation);
                row.SetField("Rate", s.Rate);
                row.SetField("Value", s.SectionValue);
                row.SetField("DeleteSection", s.DeleteSection);

                dt.Rows.Add(row);
            }
        }

        private void InitializeComboBox()
        {
            var lci = new ListOrComboBoxItem { Code = "(NONE)", Description = "<Select Section Type>", PrintDescription = "NONE" };
            SectionCboList.Add(lci);
            SectionCboList.AddRange(SketchUpLookups.SectionsByOccupancy(ParcelMast.OccupancyType));
            cboSectionType.DataSource = SectionCboList;
            cboSectionType.DisplayMember = "Description";
            cboSectionType.ValueMember = "Code";
            cboSectionType.SelectedIndex = 0;

            dgvSections.Focus();
        }

        private void MarkSectionForDeletion(string sectLetter)
        {
            Parcel.Sections.FirstOrDefault(s => s.SectionLetter == sectLetter).DeleteSection = true;
        }

        private void RefreshFormInformation()
        {
            if (SelectedRow == null)
            {
                //need to re-select A
                dgvSections.Rows[0].Selected = true;
                ClearDetails();
                SelectedSectionLetter = string.Empty;
                StatusImage = UnsavedChangesExist ? Asterisk : GreenCheckImage;
            }
            else
            {
                SelectedSectionLetter = SelectedRow.Cells["SectionLetter"].Value.ToString();
                SelectedSection = Parcel.SelectSectionByLetter(SelectedSectionLetter);
                foreach (DataGridViewRow r in dgvSections.Rows)
                {
                    if ((bool)r.Cells["DeleteSection"].Value)
                    {
                        r.DefaultCellStyle.BackColor = Color.Red;
                        r.DefaultCellStyle.ForeColor = Color.Goldenrod;
                        r.DefaultCellStyle.SelectionForeColor = Color.Red;
                        r.DefaultCellStyle.SelectionBackColor = Color.Goldenrod;
                    }
                    else
                    {
                        r.DefaultCellStyle.BackColor = Color.White;
                        r.DefaultCellStyle.ForeColor = Color.Black;
                        r.DefaultCellStyle.SelectionForeColor = Color.White;
                        r.DefaultCellStyle.SelectionBackColor = Color.CadetBlue;
                    }
                }

                ShowDetails(SelectedRow);
            }
            ShowProgressBar = false;
            StatusText = "Ready";
            UpdateStatusStrip();
        }

        private void SetCarportsToZero()
        {
            string noCpCode = (from c in SketchUpLookups.CarPortTypeCollection where c.Description == "NONE" select c.Code).FirstOrDefault();
            int noCpCodeValue = 0;
            int.TryParse(noCpCode, out noCpCodeValue);
            Parcel.ParcelMast.CarportTypeCode = noCpCodeValue;
            ParcelMast.CarportNumCars = 0;
        }

        private void SetGaragesToZero()
        {
            string noGarCode = (from g in SketchUpLookups.GarageTypeCollection where g.Description == "NONE" select g.Code).FirstOrDefault();
            int noGarCodeValue = 0;
            int.TryParse(noGarCode, out noGarCodeValue);
            Parcel.ParcelMast.Garage1TypeCode = noGarCodeValue;
            Parcel.ParcelMast.Garage2TypeCode = noGarCodeValue;
            ParcelMast.Garage1NumCars = 0;
            ParcelMast.Garage2NumCars = 0;
        }

        private void ShowDetails(DataGridViewRow currentRow)
        {
            cboSectionType.Enabled = true;
            txtStories.Enabled = true;
            SelectedRow = currentRow;
            setByCode = true;
            bool aRowIsSelected = currentRow != null && SelectedSection != null;
            if (aRowIsSelected)
            {
                // Section Description Controls
                string rowSectionLetter = currentRow.Cells["SectionLetter"].Value.ToString();

                lblSectionLetter.Text = rowSectionLetter;

                int cboTypeIndex = cboSectionType.FindString(SelectedSection.SectionType);
                cboSectionType.SelectedIndex = cboTypeIndex;
                cboSectionType.Enabled = rowSectionLetter != "A";
                string rowDescription = currentRow.Cells["Description"].Value.ToString();
                descriptionTextLabel.Text = rowDescription;

                string rowStories = currentRow.Cells["StoriesText"].Value.ToString();
                txtStories.Text = $"{rowStories:N2}";

                string squareFootage = $"{currentRow.Cells["SqFt"].Value.ToString()}";
                sizeTextLabel.Text = $"{squareFootage:N2}";

                chkDeleteSection.Checked = SelectedSection.DeleteSection;

                // Section Valuation Controls
                bool rowZeroDepr = currentRow.Cells["ZeroDepr"].Value.ToString().ToUpper().Trim() == "Y";
                chkZeroDepr.Checked = rowZeroDepr;

                int cboClassIndex = cboClass.FindString(SelectedSection.SectionClass);
                cboClass.SelectedIndex = cboClassIndex;

                string rowAdjFactor = $"{currentRow.Cells["AdjFactor"].Value.ToString():N0}";
                txtFactor.Text = rowAdjFactor;

                string rowDepr = $"{currentRow.Cells["Depreciation"].Value.ToString():N0}";
                txtDepr.Text = rowDepr;

                string rowRate = $"{SelectedSection.Rate.ToString():C0}";
                lblRateValue.Text = rowRate;
                string rowCurrrentValue = $"{currentRow.Cells["Value"].Value.ToString():C0}";
                lblValueText.Text = rowCurrrentValue;

                // Set the variables used to project a new value if the model changes.
                double.TryParse(rowRate, out newRate);
                double.TryParse(rowAdjFactor, out newFactor);
                double.TryParse(rowDepr, out newDepreciation);
                hasNewZeroDepr = rowZeroDepr;
                newSectionClass = SelectedSection.SectionClass;
                newSectionType = SelectedSection.SectionType;
                int sectionValue = 0;
                int.TryParse(rowCurrrentValue, out sectionValue);
                ComputeNewSectionValue();
                if (newValue > 0 && newValue != sectionValue)
                {
                    lblNewValueText.Text = $"{newValue.ToString():C0}";
                }
                else
                {
                    lblNewValueText.Text = lblValueText.Text;
                }
            }
            setByCode = false;
        }

        private void storyText_Leave(object sender, EventArgs e)
        {
            UpdateSectionStories(txtStories.Text);
        }

        private void txtDepr_Leave(object sender, EventArgs e)
        {
            UpdateNewDepreciation(txtDepr.Text);
        }

        private void txtFactor_Leave(object sender, EventArgs e)
        {
            UpdateNewFactor(txtFactor.Text);
        }

        private void UpdateClassInGrid()
        {
        }

        private void UpdateFactorInGrid(string text)
        {
            double adjFactorVal = 0.00;
            double.TryParse(text, out adjFactorVal);
            if (SelectedRow != null)
            {
                SelectedRow.Cells["AdjFactor"].Value = $"{adjFactorVal.ToString():N2}";
            }
        }

        private void UpdateNewClass(object sender)
        {
            try
            {
                var combo = (ComboBox)sender;
                if (combo.SelectedIndex >= 0)
                {
                    newSectionClass = ((string)combo.SelectedItem).ToUpper().Trim();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateNewDepreciation(string text = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    double deprVal = 0.00;
                    double.TryParse(text, out deprVal);
                    newDepreciation = deprVal;
                }
                else
                {
                    newDepreciation = 0.00;
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateNewFactor(string text = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(text) && !setByCode)
                {
                    double.TryParse(text, out newFactor);
                    UpdateNewSectionValue();
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateNewSectionType(object sender)
        {
            try
            {
                var combo = (ComboBox)sender;
                if (SelectedRow != null && combo.SelectedIndex >= 0)
                {
                    var sectionType = (ListOrComboBoxItem)combo.SelectedItem;
                    newSectionType = sectionType.Code;
                    SelectedRow.Cells["SectionType"].Value = sectionType.Code;
                    SelectedRow.Cells["Description"].Value = sectionType.Description;
                    ComputeNewSectionValue();
                    lblNewValueText.Text = $"{newValue.ToString():C0}";
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateNewSectionValue()
        {
            ComputeNewSectionValue();
            lblNewValueText.Text = newValue.ToString("C0", CultureInfo.CurrentCulture);
        }

        private void UpdateSectionsFromGrid(string sectionLetter, DataGridViewRow row)
        {
            try
            {
                double newDepr = 0.00;
                double newFactor = 0.00;

                SMSection section = Parcel.SelectSectionByLetter(sectionLetter);

                section.SectionType = row.Cells["SectionType"].Value.ToString();
                section.Description = row.Cells["Description"].Value.ToString();
                section.StoriesText = row.Cells["StoriesText"].Value.ToString();
                section.StoriesValue = SMGlobal.StoryValueFromText(section.StoriesText);
                section.ZeroDepr = row.Cells["ZeroDepr"].Value.ToString();
                section.SectionClass = cboClass.SelectedItem.ToString();
                double.TryParse(row.Cells["AdjFactor"].Value.ToString(), out newFactor);

                section.AdjFactor = newFactor;

                double.TryParse(row.Cells["Depreciation"].Value.ToString(), out newDepr);
                section.Depreciation = newDepr;
                section.DeleteSection = (bool)row.Cells["DeleteSection"].Value;
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateSectionStories(string textEntered)
        {
            dgvSections.BeginEdit(true);
            SelectedRow.Cells["StoriesText"].Value = textEntered;
            dgvSections.EndEdit();
            UnsavedChangesExist = (textEntered != ParcelMast.StoriesText);
            if (!string.IsNullOrEmpty(textEntered))
            {
                string stories = textEntered.Trim().ToUpper();
                double storiesNumber = 0.00;
                if (stories == "S/F" || stories == "S/L")
                {
                    SelectedSection.StoriesText = stories;
                    SelectedSection.StoriesValue = 1;
                }
                else
                {
                    SelectedSection.StoriesText = stories;
                    double.TryParse(stories, out storiesNumber);
                    SelectedSection.StoriesValue = storiesNumber;
                }
            }

            RefreshFormInformation();
        }

        private void UpdateSectionTypeInGrid()
        {
            if (cboSectionType.SelectedIndex > 0 && SelectedSection != null)
            {
                UnsavedChangesExist = true;
                EditState = SectionEditState.Edited;
                string storedSectionType = SelectedSection.SectionType.ToUpper();
                newSectionType = cboSectionType.SelectedValue.ToString().ToUpper().Trim();
                string newDescription = ((ListOrComboBoxItem)cboSectionType.SelectedItem).PrintDescription;
                if (newSectionType != storedSectionType)
                {
                    dgvSections.BeginEdit(true);

                    dgvSections.Rows[SelectedRow.Index].Cells["SectionType"].Value = newSectionType.ToUpper().Trim();
                    dgvSections.Rows[SelectedRow.Index].Cells["Description"].Value = newDescription.ToUpper().Trim();
                    descriptionTextLabel.Text = newDescription.ToUpper().Trim();

                    dgvSections.EndEdit();

                    SelectedSection.SectionType = newSectionType;
                    SelectedSection.Description = newDescription;
                    StatusText = $"Section {SelectedSection.SectionType} changed from {storedSectionType} to {newSectionType}";
                    StatusImage = EditedImage;
                    UpdateStatusStrip();
                }
            }
        }

        private void UpdateSectionTypeInGrid(object sender)
        {
            try
            {
                if (SelectedRow != null && cboSectionType.SelectedIndex >= 0)
                {
                    var sectionType = (ListOrComboBoxItem)cboSectionType.SelectedItem;
                    SelectedRow.Cells["SectionType"].Value = sectionType.Code;
                    SelectedRow.Cells["Description"].Value = sectionType.Description;
                }
            }
            catch (Exception ex)
            {
                string errMessage = $"Error occurred in {MethodBase.GetCurrentMethod().Module}, in procedure {MethodBase.GetCurrentMethod().Name}: {ex.Message}".ToString();
                Trace.WriteLine(errMessage);

#if DEBUG

                MessageBox.Show(errMessage);
#endif
            }
        }

        private void UpdateStatusStrip(string displayText = null)
        {
            ShowProgressBar = false;
            UseWaitCursor = false;
            switch (EditState)
            {
                case SectionEditState.Loading:
                    StatusText = displayText ?? "Loading...";

                    StatusImage = LoadingImage;
                    ShowProgressBar = true;
                    break;

                case SectionEditState.Edited:
                    StatusText = displayText ?? "Edited...";

                    StatusImage = LoadingImage;
                    ShowProgressBar = false;
                    break;

                default:
                    StatusText = displayText ?? "Ready";
                    StatusImage = UnsavedChangesExist ? Asterisk : GreenCheckImage;
                    ShowProgressBar = false;
                    break;
            }

            stlEditStatus.Image = StatusImage;

            progressBar.Visible = ShowProgressBar;
            UseWaitCursor = ShowProgressBar;
        }

        private void UpdateWorkingParcelModel()
        {
            UseWaitCursor = true;
            string sectionLetter = string.Empty;
            UnsavedChangesExist = true;
            EditState = SectionEditState.Edited;
            if (CheckGaragesAndCarports())
            {
                foreach (DataGridViewRow row in dgvSections.Rows)
                {
                    sectionLetter = row.Cells["SectionLetter"].Value.ToString();
                    UpdateStatusStrip($"Updating Section {sectionLetter}...");
                    UpdateSectionsFromGrid(sectionLetter, row);
                    Application.DoEvents();
                }
                UpdateStatusStrip("Removing any deleted section(s)...");
                Application.DoEvents();
                Parcel.Sections.RemoveAll(s => s.DeleteSection);
                UpdateStatusStrip("Passing changes to sketch engine...");
                Application.DoEvents();
                SketchRepo.AddSketchToSnapshots(Parcel);
                Application.DoEvents();
                UseWaitCursor = false;
                Application.DoEvents();
            }
        }

        #endregion Private Methods

        #region Private Fields

        private Bitmap Asterisk = Properties.Resources.Asterisk;
        private DataGridViewCellStyle deletedRowStyle = new DataGridViewCellStyle { ForeColor = Color.DarkRed, BackColor = Color.LightGray, SelectionBackColor = Color.DarkRed, SelectionForeColor = Color.Yellow };
        private List<DataGridViewRow> deleteRows = new List<DataGridViewRow>();
        private bool deleteSection = false;
        private Bitmap DeleteSectionImage = Properties.Resources.DeleteListItem_32x;
        private Bitmap EditedImage = Properties.Resources.EditImage;
        private Bitmap GreenCheckImage = Properties.Resources.GreenCheckCircle;
        private bool hasNewZeroDepr = false;
        private Bitmap LoadingImage = Properties.Resources.Loading_BlueSlow;
        private double newDepreciation;
        private double newFactor = 0.00;
        private double newRate = 0.00;
        private string newSectionClass = string.Empty;
        private string newSectionType = string.Empty;
        private int newValue = 0;
        private string newZeroDepr = string.Empty;
        private DataGridViewCellStyle normalCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black, BackColor = Color.White, SelectionBackColor = Color.Green };
        private Bitmap ProgressImage = Properties.Resources.Progress_Ring_24;
        private Bitmap SaveAndCloseImage = Properties.Resources.Save;
        private List<ListOrComboBoxItem> sectionCboList;

        private DataGridViewRow selectedRow;
        private List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
        private SMSection selectedSection;
        private string selectedSectionLetter;
        private bool setByCode;
        private SketchRepository sketchRepo;
        private Bitmap SlowBlueLoadingImage = Properties.Resources.Loading_BlueSlow;

        #endregion Private Fields

        #region Constructor

        public EditSketchSections(SMParcel workingParcel)
        {
            InitializeComponent();
            Parcel = workingParcel;
            InitializeComboBox();
            FillDataGridView();
            ShowDetails(SelectedRow);
            UseWaitCursor = false;
        }

        #endregion Constructor
    }
}