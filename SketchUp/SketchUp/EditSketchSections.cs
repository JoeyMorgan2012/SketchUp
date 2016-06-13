using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class EditSketchSections : Form
    {
#region "Constructor"

        public EditSketchSections(SMParcel workingParcel)
        {
            Cursor.Current = Cursors.Default;
            InitializeComponent();
#if DEBUG
            TestSetup ts = new TestSetup();

            SketchUpLookups.InitializeWithTestSettings();

#endif

         
            Parcel = workingParcel;
            //May be redundant:
            ParcelMast = Parcel.ParcelMast;
            //------------------
            originalSnapshotIndex = Parcel.SnapShotIndex;
            SketchRepo.AddSketchToSnapshots(Parcel);
            Parcel.SnapShotIndex++;
            InitializeComboBox();
            FillDataGridView();
            ShowDetails(SelectedRow);
        }

#endregion

#region "Private methods"

        private void CalculateAreaTotals()
        {
            List<string> laTypes = (from la in SketchUpLookups.LivingAreaSectionTypeCollection select la._LAattSectionType).ToList();
            TotalArea = (from s in Parcel.Sections select s.SqFt).Sum();
            TotalLivingArea = (from s in Parcel.Sections where laTypes.Contains(s.SectionType) select s.SqFt).Sum();
        }

        private void cboSectionType_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateSectionType();
        }

        private bool CheckGaragesAndCarports()
        {
            bool carsOk = false;
            if (Parcel.SnapShotIndex>0)
            {
                originalSnapshotIndex = (from p in SketchUpGlobals.SketchSnapshots where p.SnapShotIndex < Parcel.SnapShotIndex select p.SnapShotIndex).Max();
            }
            SMParcel priorVersion = (from p in SketchUpGlobals.SketchSnapshots where p.SnapShotIndex ==originalSnapshotIndex select p).FirstOrDefault();
            int priorCarports = (from s in priorVersion.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).Count();
            int currentCarports = (from s in Parcel.Sections where SketchUpLookups.CarPortTypes.Contains(s.SectionType) select s).Count();
            int priorGarages = (from s in priorVersion.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).Count();
            int currentGarages = (from s in Parcel.Sections where SketchUpLookups.GarageTypes.Contains(s.SectionType) select s).Count();
            if (currentCarports == 0)
            {
                SetCarportsToZero(Parcel);
            }
            if (currentGarages == 0)
            {
                SetGaragesToZero(Parcel);
            }
            if (currentGarages != priorGarages || currentCarports != priorCarports)
            {
                MissingGarageData mgd = new MissingGarageData(Parcel.ParcelMast);
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

        private void ClearDetails()
        {
            sectionLetterLabel.Text = string.Empty;
            cboSectionType.SelectedIndex = -1;
            storyText.Text = string.Empty;
            storyText.Enabled = false;
            cboSectionType.SelectedIndex = 0;
            cboSectionType.Enabled = false;
            sizeTextLabel.Text = string.Empty;
            SelectedSection = null;
        }

        private DataTable CreateSectionsDataTable()
        {
            DataTable dt = new DataTable("sectionDt");

            dt.Columns.Add(new DataColumn { ColumnName = "SectionLetter", Caption = "Section", DataType = typeof(string), AllowDBNull = false });
            dt.Columns.Add(new DataColumn { ColumnName = "SectionType", Caption = "Type", DataType = typeof(string), AllowDBNull = false });
            dt.Columns.Add(new DataColumn { ColumnName = "Description", Caption = "Description", DataType = typeof(string), AllowDBNull = true });
            dt.Columns.Add(new DataColumn { ColumnName = "StoreysText", Caption = "Story", DataType = typeof(string), AllowDBNull = true });
            dt.Columns.Add(new DataColumn { ColumnName = "SectionSize", Caption = "Size", DataType = typeof(string), AllowDBNull = false });
            return dt;
        }

        private DataTable CreateSectionsDt(SMParcel parcel)
        {
            try
            {
                DataTable dt = CreateSectionsDataTable();
                PopulateSectionData(dt);
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
            Console.WriteLine(string.Format("{0}", message));
            Trace.WriteLine(string.Format("{0}", e.Exception.StackTrace));
            Trace.WriteLine(string.Format("{0}", e.Exception.Source));
            Trace.WriteLine(string.Format("{0}", e.Exception.TargetSite));
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

        private void ExitWithoutSaving()
        {
            List<SMParcel> versionsToDiscard = (from p in SketchUpGlobals.SketchSnapshots where p.SnapShotIndex > originalSnapshotIndex select p).ToList();
            foreach (SMParcel p in versionsToDiscard)
            {
  SketchUpGlobals.SketchSnapshots.Remove(p);
            }
            Parcel = SketchUpGlobals.ParcelWorkingCopy;
        }

        private void FillDataGridView()
        {
           

            CalculateAreaTotals();

            DataTable sections = CreateSectionsDt(Parcel);
            dgvSections.DataSource = sections;
            for (int i = 0; i < sections.Columns.Count; i++)
            {
                dgvSections.Columns[i].DataPropertyName = sections.Columns[i].ColumnName;
            }
        }

        private void InitializeComboBox()
        {
            ListOrComboBoxItem lci = new ListOrComboBoxItem { Code = "(NONE)", Description = "<Select Section Type>", PrintDescription = "NONE" };
            SectionCboList.Add(lci);
            SectionCboList.AddRange(SketchUpLookups.SectionsByOccupancy(ParcelMast.OccupancyType));
            cboSectionType.DataSource = SectionCboList;
            cboSectionType.DisplayMember = "Description";
            cboSectionType.ValueMember = "Code";
            cboSectionType.SelectedIndex = 0;

            dgvSections.Focus();
        }

        private void PopulateSectionData(DataTable dt)
        {
            foreach (SMSection s in Parcel.Sections.OrderBy(s => s.SectionLetter))
            {
                DataRow row = dt.NewRow();

                row.SetField("SectionLetter", s.SectionLetter);
                row.SetField("SectionType", s.SectionType);
                row.SetField("Description", s.Description);
                row.SetField("StoreysText", s.StoreysText);
                row.SetField("SectionSize", s.SqFt);
                dt.Rows.Add(row);
            }
        }

        private void RefreshFormInformation()
        {
            string sectionLetter = string.Empty;
            if (SelectedRow == null)
            {
                ClearDetails();
                sectionLetter = string.Empty;
            }
            else
            {
                if (SelectedRow == null)
                {
                    dgvSections.Rows[0].Selected = true;
                }
                sectionLetter = sectionLetterLabel.Text = SelectedRow.Cells["sectionCol"].Value.ToString();
                SelectedSection = Parcel.SelectSectionByLetter(sectionLetter);

                ShowDetails(SelectedRow);
            }

            UpdateStatusStrip(sectionLetter);
        }

        private void SaveChanges()
        {
            SketchUpGlobals.SketchSnapshots.Add(Parcel);
            foreach (DataGridViewRow row in dgvSections.Rows)
            {
               
                string sectionLetter =row.Cells["sectionCol"].Value.ToString();
                SMSection section = Parcel.SelectSectionByLetter(sectionLetter);
                section.SectionType = row.Cells["typeCol"].Value.ToString();
                section.Description = row.Cells["descriptionCol"].Value.ToString();
                section.StoreysText= row.Cells["storyCol"].Value.ToString();
                section.StoreysValue = SMGlobal.StoryValueFromText(section.StoreysText);
            }
            SketchRepository sr = new SketchRepository(Parcel);
            CheckGaragesAndCarports();
            sr.SaveCurrentParcel(Parcel);
            Parcel.SnapShotIndex++;
            SketchUpGlobals.SketchSnapshots.Add(Parcel);

        }

        private void SelectCurrentSectionTypeFromList()
        {
            if (SelectedRow != null)
            {
                string sectionType = SelectedRow.Cells["typeCol"].Value.ToString();
                int cboIndex = cboSectionType.FindStringExact(sectionType);
                cboSectionType.SelectedIndex = cboIndex;
            }
            else
            {
                cboSectionType.SelectedIndex = 0;
            }
        }

        private void SetCarportsToZero(SMParcel parcel)
        {
            string noCpCode = (from c in SketchUpLookups.CarPortTypeCollection where c.Description == "NONE" select c.Code).FirstOrDefault();
            int noCpCodeValue = 0;
            int.TryParse(noCpCode, out noCpCodeValue);
            Parcel.ParcelMast.CarportTypeCode = noCpCodeValue;
            ParcelMast.CarportNumCars= 0;
        }

        private void SetGaragesToZero(SMParcel parcel)
        {
            string noGarCode =(from g in SketchUpLookups.GarageTypeCollection where g.Description == "NONE" select g.Code).FirstOrDefault();
            int noGarCodeValue = 0;
            int.TryParse(noGarCode, out noGarCodeValue);
            Parcel.ParcelMast.Garage1TypeCode = noGarCodeValue;
            Parcel.ParcelMast.Garage2TypeCode = noGarCodeValue;
            ParcelMast.Garage1NumCars = 0;
            ParcelMast.Garage2NumCars = 0;

        }

        private void ShowDetails(DataGridViewRow selectedRow)
        {
            cboSectionType.Enabled = true;
            storyText.Enabled = true;

            sectionLetterLabel.Text = selectedRow.Cells["sectionCol"].Value.ToString();
            int cboIndex = cboSectionType.FindString(SelectedSection.SectionType);
            cboSectionType.SelectedIndex = cboIndex;
            descriptionTextLabel.Text = selectedRow.Cells["descriptionCol"].Value.ToString();
            storyText.Text = string.Format("{0:N2}", selectedRow.Cells["storyCol"].Value.ToString());
            sizeTextLabel.Text = string.Format("{0:N2}", selectedRow.Cells["sizeCol"].Value.ToString());
            cboSectionType.Enabled = (selectedRow.Cells["sectionCol"].Value.ToString() != "A");
        }

        private void storyText_Leave(object sender, EventArgs e)
        {
            SelectedRow.Cells["storyCol"].Value = storyText.Text;
        }

        private void tsbExit_Click(object sender, EventArgs e)
        {
            if (unsavedChangesExist)
            {
                string message = "You have not saved your changes. Save now? choose \"Yes\" to save, \"No\" to close without saving, or \"Cancel\" to continue editing.";
                DialogResult response = MessageBox.Show(message, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                switch (response)
                {
                    case DialogResult.Yes:
                    case DialogResult.OK:
                        SaveChanges();
                        Close();
                        break;

                    case DialogResult.No:
                        ExitWithoutSaving();
                        break;

                    case DialogResult.None:

                    case DialogResult.Cancel:

                    default:
                        dgvSections.Focus();
                        break;
                }
            }
        }

        private void tsbSave_Click(object sender, EventArgs e)
        {
            if (unsavedChangesExist)
            {
                SaveChanges();
                originalSnapshotIndex = Parcel.SnapShotIndex;
                Parcel.SnapShotIndex++;
                SketchUpGlobals.SketchSnapshots.Add(Parcel);
                unsavedChangesExist = false;
                UpdateStatusStrip();
            }
        }

        private void UpdateSectionStorys(string textEntered)
        {
            if (!string.IsNullOrEmpty(textEntered))
            {
                string storeys = textEntered.Trim().ToUpper();
                decimal storeysNumber = 0.00M;
                if (storeys == "S/F" || storeys == "S/L")
                {
                    SelectedSection.StoreysText = storeys;
                    SelectedSection.StoreysValue = 1;
                }
                else
                {
                    SelectedSection.StoreysText = storeys;
                    decimal.TryParse(storeys, out storeysNumber);
                    SelectedSection.StoreysValue = storeysNumber;
                }
            }
            RefreshFormInformation();
        }

        private void UpdateSectionType()
        {
            if (cboSectionType.SelectedIndex > 0 && SelectedSection != null)
            {
                editedSectionType = cboSectionType.SelectedValue.ToString();
                string newDescription = ((ListOrComboBoxItem)cboSectionType.SelectedItem).PrintDescription;
                if (editedSectionType.ToUpper().Trim() != SelectedSection.SectionType.ToUpper().Trim())
                {
                    unsavedChangesExist = true;
                    UpdateStatusStrip(dgvSections.Rows[SelectedRow.Index].Cells["sectionCol"].Value.ToString());
                    dgvSections.BeginEdit(true);

                    string currentType = dgvSections.Rows[SelectedRow.Index].Cells["typeCol"].Value.ToString();
                    if (currentType != editedSectionType)
                    {
                        dgvSections.Rows[SelectedRow.Index].Cells["typeCol"].Value = editedSectionType.ToUpper().Trim();
                        dgvSections.Rows[SelectedRow.Index].Cells["descriptionCol"].Value = newDescription.ToUpper().Trim();
                        descriptionTextLabel.Text = newDescription.ToUpper().Trim();
                    }
                    dgvSections.EndEdit();
                  
                }
            }
        }

        private void UpdateStatusStrip(string sectionLetter = "")
        {
            string statusText = string.Empty;
            if (unsavedChangesExist)
            {
                stlEditStatus.Image = EditedImage;
            }
            else
            {
                stlEditStatus.Image = ChangesSavedImage;
            }
            if (!string.IsNullOrEmpty(sectionLetter))
            {
               statusText = $"Section {sectionLetter.ToUpper().Trim()} selected.";
                if (sectionLetter.ToUpper().Trim()=="A")
                {
                    statusText += $"\tSection A must be {dgvSections.Rows[SelectedRow.Index].Cells["descriptionCol"].Value} for Occupancy Type {ParcelMast.OccupancyType.ToString()}";

                }

            }
            else
            {
                statusText= "No section or multiple sections selected.";
               
            }
            stlSection.Text = statusText;
        }

#endregion

#region "Properties"

        public DataTable DtSections
        {
            get
            {
                dtSections = CreateSectionsDataTable();
                return dtSections;
            }
            set
            {
                dtSections = value;
            }
        }

        public SMParcel Parcel
        {
            get;
            private set;
        }

        public SMParcelMast ParcelMast
        {
            get
            {
                return Parcel.ParcelMast;
            }
            private set
            {
                Parcel.ParcelMast = value;
            }
        }

        public List<ListOrComboBoxItem> SectionCboList
        {
            get
            {
                if (sectionCboList == null)
                {
                    sectionCboList = new List<ListOrComboBoxItem>();
                }
                return sectionCboList;
            }
            set
            {
                sectionCboList = value;
            }
        }

        public DataGridViewRow SelectedRow
        {
            get
            {
                if (dgvSections.SelectedRows != null && dgvSections.SelectedRows.Count == 1)
                {
                    selectedRows.Clear();
                    if (dgvSections.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow r in dgvSections.Rows)
                        {
                            if (dgvSections.SelectedRows.Contains(r))
                            {
                                selectedRows.Add(r);
                            }
                        }
                        selectedRow = selectedRows[0];
                    }
                }
                return selectedRow;
            }
            set
            {
                selectedRow = value;
            }
        }

        public SMSection SelectedSection
        {
            get
            {
                if (SelectedRow != null)
                {
                    string sectionLetter = SelectedRow.Cells["sectionCol"].Value.ToString();
                    selectedSection = Parcel.SelectSectionByLetter(sectionLetter);
                }
                return selectedSection;
            }
            set
            {
                selectedSection = value;
            }
        }

        public decimal TotalArea
        {
            get
            {
                return totalArea;
            }
            set
            {
                totalArea = value;
            }
        }

        public decimal TotalLivingArea
        {
            get
            {
                return totalLivingArea;
            }
            set
            {
                totalLivingArea = value;
            }
        }

        public string SelectedSectionLetter
        {
            get
            {
                return selectedSectionLetter;
            }

            set
            {
                selectedSectionLetter = value;
            }
        }

        public SketchRepository SketchRepo
        {
            get
            {
                if (sketchRepo==null&&Parcel!=null)
                {
                    sketchRepo = new SketchRepository(Parcel);
                }
                return sketchRepo;
            }

            set
            {
                sketchRepo = value;
            }
        }

        #endregion

        #region "Private Fields"

        private Bitmap ChangesSavedImage = Properties.Resources.GreenCheck;
        private Bitmap DeleteSectionImage = Properties.Resources.DeleteListItem_32x;
        private DataTable dtSections;
        private Bitmap EditedImage = Properties.Resources.Edit_32xMD;
        private string editedSectionType;
        private string editedStoreysText;
        private Bitmap SaveAndCloseImage = Properties.Resources.Save;
        private List<ListOrComboBoxItem> sectionCboList;
        private DataGridViewRow selectedRow;
        private List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
        private SMSection selectedSection;
        private string selectedSectionLetter;
        private decimal totalArea = 0.00M;
        private decimal totalLivingArea = 0.00M;
        private bool unsavedChangesExist;
        private int originalSnapshotIndex = 0;
        private SketchRepository sketchRepo;
        #endregion

        private void storyText_TextChanged(object sender, EventArgs e)
        {
            editedStoreysText = storyText.Text;
            unsavedChangesExist =(editedStoreysText!=ParcelMast.StoreysText);
            
        }
    }
}
