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
            InitializeComponent();
#if DEBUG
            TestSetup ts = new TestSetup();

            SketchUpLookups.InitializeWithTestSettings();

#endif
            InitializeComboBox(workingParcel);
            FillDataGridView(workingParcel);
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
            SelectCurrentSectionTypeFromList();
        }

        private void dgvSections_SelectionChanged(object sender, EventArgs e)
        {
            RefreshFormInformation();
        }

        private void FillDataGridView(SMParcel workingParcel)
        {
            Parcel = workingParcel;
            ParcelMast = workingParcel.ParcelMast;

            CalculateAreaTotals();

            DataTable sections = CreateSectionsDt(Parcel);
            dgvSections.DataSource = sections;
            for (int i = 0; i < sections.Columns.Count; i++)
            {
                dgvSections.Columns[i].DataPropertyName = sections.Columns[i].ColumnName;
            }
        }

        private void InitializeComboBox(SMParcel workingParcel)
        {
           
            ListOrComboBoxItem lci = new ListOrComboBoxItem { Code = "(NONE)", Description = "<Select Section Type>", PrintDescription = "NONE" };
            SectionCboList.Add(lci);
            SectionCboList.AddRange(SketchUpLookups.SectionsByOccupancy(workingParcel.ParcelMast.OccupancyType));
            cboSectionType.DataSource = SectionCboList;
            cboSectionType.DisplayMember = "Description";
            cboSectionType.ValueMember = "Code";
            cboSectionType.SelectedIndex = 4;
     
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
                if (SelectedRow==null)
                {
                    ClearDetails();
                sectionLetter = string.Empty;
                }
                
                else
                {
                if (SelectedRow==null)
                {
                    dgvSections.Rows[0].Selected = true;
                }
                     sectionLetter = sectionLetterLabel.Text = SelectedRow.Cells["sectionCol"].Value.ToString();
                    SelectedSection = Parcel.SelectSectionByLetter(sectionLetter);
                    ShowDetails(SelectedRow);

                }
              
                UpdateStatusStrip(sectionLetter);
            }

        private void SelectCurrentSectionTypeFromList()
        {
            if (SelectedSection!=null)
            {
         string sectionType = SelectedSection.SectionType;
                int cboIndex = cboSectionType.FindStringExact(sectionType);
                cboSectionType.SelectedIndex = cboIndex;
            }
            else
            {
                cboSectionType.SelectedIndex = 0;
            }

          

        }

    private void ShowDetails(DataGridViewRow selectedRow)
    {
        cboSectionType.Enabled = true;
        storyText.Enabled = true;

        sectionLetterLabel.Text = selectedRow.Cells["sectionCol"].Value.ToString();
          int cboIndex=  cboSectionType.FindString(SelectedSection.SectionType);
            cboSectionType.SelectedIndex = cboIndex;
        descriptionTextLabel.Text = selectedRow.Cells["descriptionCol"].Value.ToString();
        storyText.Text = string.Format("{0:N2}", selectedRow.Cells["storyCol"].Value.ToString());
        sizeTextLabel.Text = string.Format("{0:N2}", selectedRow.Cells["sizeCol"].Value.ToString());

    }

        private void UpdateSectionType()
        {
            if (cboSectionType.SelectedIndex > 0 && SelectedSection != null)
            {
                string newType = cboSectionType.SelectedValue.ToString();
                string newDescription = ((ListOrComboBoxItem)cboSectionType.SelectedItem).PrintDescription;
                if (newType.ToUpper().Trim() != SelectedSection.SectionType.ToUpper().Trim())
                {
                    DataGridViewRow rowCopy = (DataGridViewRow)dgvSections.Rows[SelectedRow.Index].Clone();
                    
                    dgvSections.BeginEdit(true);
                   
                    
                    SelectedSection.SectionType = newType;
                    SelectedSection.Description = newDescription;
                    string currentType = dgvSections.Rows[SelectedRow.Index].Cells["typeCol"].Value.ToString();
                    if (currentType!=newType)
                    {
      dgvSections.Rows[SelectedRow.Index].Cells["typeCol"].Value = newType.ToUpper().Trim();
                    dgvSections.Rows[SelectedRow.Index].Cells["descriptionCol"].Value = newDescription.ToUpper().Trim();
                    }
                    dgvSections.EndEdit();
                    unsavedChangesExist = true;
                    RefreshFormInformation();
               
                   
                }


            }
        }

    private void UpdateStatusStrip(string sectionLetter = "")
    {

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
                stlSection.Text = $"Section {sectionLetter.ToUpper().Trim()} selected.";
            }
            else
            {
                stlSection.Text = "No section or multiple sections selected.";
            }
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
                if (sectionCboList==null)
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
                if (SelectedRow!=null)
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

#endregion

#region "Private Fields"

        private Bitmap ChangesSavedImage = Properties.Resources.GreenCheck;
    private Bitmap DeleteSectionImage = Properties.Resources.DeleteListItem_32x;
        private DataTable dtSections;
    private Bitmap EditedImage = Properties.Resources.Edit_32xMD;
    private Bitmap SaveAndCloseImage = Properties.Resources.Save;
        List<ListOrComboBoxItem> sectionCboList;
        DataGridViewRow selectedRow;
        private List<DataGridViewRow> selectedRows = new List<DataGridViewRow>();
        private SMSection selectedSection;
        private decimal totalArea = 0.00M;
        private decimal totalLivingArea = 0.00M;
        private bool unsavedChangesExist;

#endregion
    }
}
