namespace SketchUp
{
    partial class EditSketchSections
    {
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSketchSections));
            this.sectionsDt = new System.Data.DataTable();
            this.selectColumn = new System.Data.DataColumn();
            this.sectionColumn = new System.Data.DataColumn();
            this.descriptionColumn = new System.Data.DataColumn();
            this.storyColumn = new System.Data.DataColumn();
            this.sizeColumn = new System.Data.DataColumn();
            this.selectCheckDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sectionLetterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.storyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sectionSizeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvSections = new System.Windows.Forms.DataGridView();
            this.SectionLetter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StoriesText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SqFt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZeroDepr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SectionClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AdjFactor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depreciation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteSection = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.stlEditStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.sectionDetailsPanel = new System.Windows.Forms.Panel();
            this.chkDeleteSection = new System.Windows.Forms.CheckBox();
            this.lblDescriptionPanelTitle = new System.Windows.Forms.Label();
            this.descriptionTextLabel = new System.Windows.Forms.Label();
            this.sizeTextLabel = new System.Windows.Forms.Label();
            this.txtStories = new System.Windows.Forms.TextBox();
            this.cboSectionType = new System.Windows.Forms.ComboBox();
            this.lblSectionLetter = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.lblStories = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblSection = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblValuationChanges = new System.Windows.Forms.Label();
            this.lblValuationPanelTitle = new System.Windows.Forms.Label();
            this.lblNewValueText = new System.Windows.Forms.Label();
            this.lblNewValue = new System.Windows.Forms.Label();
            this.lblRateValue = new System.Windows.Forms.Label();
            this.lblRate = new System.Windows.Forms.Label();
            this.lblValueText = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.chkZeroDepr = new System.Windows.Forms.CheckBox();
            this.txtDepr = new System.Windows.Forms.TextBox();
            this.lblDepr = new System.Windows.Forms.Label();
            this.txtFactor = new System.Windows.Forms.TextBox();
            this.adjFactorLabel = new System.Windows.Forms.Label();
            this.classLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).BeginInit();
            this.statusStripMain.SuspendLayout();
            this.sectionDetailsPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectCheckDataGridViewCheckBoxColumn
            // 
            this.selectCheckDataGridViewCheckBoxColumn.Name = "selectCheckDataGridViewCheckBoxColumn";
            // 
            // sectionLetterDataGridViewTextBoxColumn
            // 
            this.sectionLetterDataGridViewTextBoxColumn.Name = "sectionLetterDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // storyDataGridViewTextBoxColumn
            // 
            this.storyDataGridViewTextBoxColumn.Name = "storyDataGridViewTextBoxColumn";
            // 
            // sectionSizeDataGridViewTextBoxColumn
            // 
            this.sectionSizeDataGridViewTextBoxColumn.Name = "sectionSizeDataGridViewTextBoxColumn";
            // 
            // dgvSections
            // 
            this.dgvSections.AllowUserToAddRows = false;
            this.dgvSections.AllowUserToDeleteRows = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.Padding = new System.Windows.Forms.Padding(4);
            this.dgvSections.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvSections.AutoGenerateColumns = false;
            this.dgvSections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSections.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.BlanchedAlmond;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSections.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SectionLetter,
            this.SectionType,
            this.Description,
            this.StoriesText,
            this.SqFt,
            this.ZeroDepr,
            this.SectionClass,
            this.AdjFactor,
            this.Depreciation,
            this.Rate,
            this.Value,
            this.DeleteSection});
            this.dgvSections.DataSource = this.sectionsDt;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.DodgerBlue;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSections.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvSections.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvSections.Location = new System.Drawing.Point(26, 65);
            this.dgvSections.Name = "dgvSections";
            this.dgvSections.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvSections.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvSections.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvSections.RowTemplate.Height = 24;
            this.dgvSections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSections.Size = new System.Drawing.Size(1184, 220);
            this.dgvSections.TabIndex = 2;
            this.dgvSections.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSections_DataError);
            this.dgvSections.SelectionChanged += new System.EventHandler(this.dgvSections_SelectionChanged);
            this.dgvSections.Enter += new System.EventHandler(this.dgvSections_Enter);
            // 
            // SectionLetter
            // 
            this.SectionLetter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SectionLetter.DataPropertyName = "SectionLetter";
            this.SectionLetter.HeaderText = "Section";
            this.SectionLetter.MinimumWidth = 60;
            this.SectionLetter.Name = "SectionLetter";
            this.SectionLetter.ReadOnly = true;
            this.SectionLetter.Width = 103;
            // 
            // SectionType
            // 
            this.SectionType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.SectionType.DataPropertyName = "SectionType";
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.SectionType.DefaultCellStyle = dataGridViewCellStyle10;
            this.SectionType.HeaderText = "Type";
            this.SectionType.MinimumWidth = 90;
            this.SectionType.Name = "SectionType";
            this.SectionType.ReadOnly = true;
            this.SectionType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SectionType.Width = 90;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Description.DataPropertyName = "Description";
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 150;
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            this.Description.Width = 150;
            // 
            // StoriesText
            // 
            this.StoriesText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.StoriesText.DataPropertyName = "StoriesText";
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black;
            this.StoriesText.DefaultCellStyle = dataGridViewCellStyle11;
            this.StoriesText.HeaderText = "Story";
            this.StoriesText.MinimumWidth = 60;
            this.StoriesText.Name = "StoriesText";
            this.StoriesText.ReadOnly = true;
            this.StoriesText.Width = 87;
            // 
            // SqFt
            // 
            this.SqFt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SqFt.DataPropertyName = "SqFt";
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black;
            this.SqFt.DefaultCellStyle = dataGridViewCellStyle12;
            this.SqFt.HeaderText = "Size";
            this.SqFt.MinimumWidth = 60;
            this.SqFt.Name = "SqFt";
            this.SqFt.ReadOnly = true;
            this.SqFt.Width = 77;
            // 
            // ZeroDepr
            // 
            this.ZeroDepr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ZeroDepr.DataPropertyName = "ZeroDepr";
            this.ZeroDepr.HeaderText = "0 Depr";
            this.ZeroDepr.MinimumWidth = 30;
            this.ZeroDepr.Name = "ZeroDepr";
            this.ZeroDepr.ReadOnly = true;
            this.ZeroDepr.Width = 98;
            // 
            // SectionClass
            // 
            this.SectionClass.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.SectionClass.DataPropertyName = "SectionClass";
            this.SectionClass.HeaderText = "Class";
            this.SectionClass.MinimumWidth = 30;
            this.SectionClass.Name = "SectionClass";
            this.SectionClass.ReadOnly = true;
            this.SectionClass.Width = 85;
            // 
            // AdjFactor
            // 
            this.AdjFactor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AdjFactor.DataPropertyName = "AdjFactor";
            this.AdjFactor.HeaderText = "Factor";
            this.AdjFactor.MinimumWidth = 60;
            this.AdjFactor.Name = "AdjFactor";
            this.AdjFactor.ReadOnly = true;
            this.AdjFactor.Width = 95;
            // 
            // Depreciation
            // 
            this.Depreciation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Depreciation.DataPropertyName = "Depreciation";
            this.Depreciation.HeaderText = "Depr";
            this.Depreciation.MinimumWidth = 60;
            this.Depreciation.Name = "Depreciation";
            this.Depreciation.ReadOnly = true;
            this.Depreciation.Width = 84;
            // 
            // Rate
            // 
            this.Rate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Rate.DataPropertyName = "Rate";
            this.Rate.HeaderText = "Rate";
            this.Rate.MinimumWidth = 60;
            this.Rate.Name = "Rate";
            this.Rate.ReadOnly = true;
            this.Rate.Width = 82;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Value.DataPropertyName = "Value";
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 120;
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            this.Value.Width = 120;
            // 
            // DeleteSection
            // 
            this.DeleteSection.DataPropertyName = "DeleteSection";
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.Red;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.NullValue = "false";
            this.DeleteSection.DefaultCellStyle = dataGridViewCellStyle13;
            this.DeleteSection.HeaderText = "Del";
            this.DeleteSection.Name = "DeleteSection";
            this.DeleteSection.ReadOnly = true;
            this.DeleteSection.Visible = false;
            // 
            // statusStripMain
            // 
            this.statusStripMain.Font = new System.Drawing.Font("Segoe UI Semibold", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stlEditStatus,
            this.progressBar});
            this.statusStripMain.Location = new System.Drawing.Point(0, 492);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(1225, 25);
            this.statusStripMain.TabIndex = 5;
            // 
            // stlEditStatus
            // 
            this.stlEditStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stlEditStatus.Image = global::SketchUp.Properties.Resources.GreenCheck;
            this.stlEditStatus.Name = "stlEditStatus";
            this.stlEditStatus.Size = new System.Drawing.Size(20, 20);
            this.stlEditStatus.Text = "*";
            // 
            // progressBar
            // 
            this.progressBar.Maximum = 1000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 19);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // sectionDetailsPanel
            // 
            this.sectionDetailsPanel.BackColor = System.Drawing.Color.LightBlue;
            this.sectionDetailsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sectionDetailsPanel.Controls.Add(this.chkDeleteSection);
            this.sectionDetailsPanel.Controls.Add(this.lblDescriptionPanelTitle);
            this.sectionDetailsPanel.Controls.Add(this.descriptionTextLabel);
            this.sectionDetailsPanel.Controls.Add(this.sizeTextLabel);
            this.sectionDetailsPanel.Controls.Add(this.txtStories);
            this.sectionDetailsPanel.Controls.Add(this.cboSectionType);
            this.sectionDetailsPanel.Controls.Add(this.lblSectionLetter);
            this.sectionDetailsPanel.Controls.Add(this.sizeLabel);
            this.sectionDetailsPanel.Controls.Add(this.lblStories);
            this.sectionDetailsPanel.Controls.Add(this.descriptionLabel);
            this.sectionDetailsPanel.Controls.Add(this.lblType);
            this.sectionDetailsPanel.Controls.Add(this.lblSection);
            this.sectionDetailsPanel.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionDetailsPanel.Location = new System.Drawing.Point(26, 291);
            this.sectionDetailsPanel.Name = "sectionDetailsPanel";
            this.sectionDetailsPanel.Size = new System.Drawing.Size(550, 198);
            this.sectionDetailsPanel.TabIndex = 3;
            // 
            // chkDeleteSection
            // 
            this.chkDeleteSection.AutoSize = true;
            this.chkDeleteSection.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDeleteSection.ForeColor = System.Drawing.Color.Maroon;
            this.chkDeleteSection.Location = new System.Drawing.Point(31, 162);
            this.chkDeleteSection.Name = "chkDeleteSection";
            this.chkDeleteSection.Size = new System.Drawing.Size(142, 27);
            this.chkDeleteSection.TabIndex = 12;
            this.chkDeleteSection.Text = "Delete Section";
            this.chkDeleteSection.UseVisualStyleBackColor = true;
            this.chkDeleteSection.CheckedChanged += new System.EventHandler(this.chkDeleteSection_CheckedChanged);
            // 
            // lblDescriptionPanelTitle
            // 
            this.lblDescriptionPanelTitle.AutoSize = true;
            this.lblDescriptionPanelTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescriptionPanelTitle.Location = new System.Drawing.Point(8, 4);
            this.lblDescriptionPanelTitle.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.lblDescriptionPanelTitle.Name = "lblDescriptionPanelTitle";
            this.lblDescriptionPanelTitle.Size = new System.Drawing.Size(138, 20);
            this.lblDescriptionPanelTitle.TabIndex = 0;
            this.lblDescriptionPanelTitle.Text = "Section Description";
            this.lblDescriptionPanelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionTextLabel
            // 
            this.descriptionTextLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTextLabel.Location = new System.Drawing.Point(227, 75);
            this.descriptionTextLabel.Name = "descriptionTextLabel";
            this.descriptionTextLabel.Size = new System.Drawing.Size(124, 27);
            this.descriptionTextLabel.TabIndex = 8;
            this.descriptionTextLabel.Text = "Description";
            this.descriptionTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sizeTextLabel
            // 
            this.sizeTextLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeTextLabel.Location = new System.Drawing.Point(452, 80);
            this.sizeTextLabel.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.sizeTextLabel.Name = "sizeTextLabel";
            this.sizeTextLabel.Size = new System.Drawing.Size(82, 20);
            this.sizeTextLabel.TabIndex = 10;
            this.sizeTextLabel.Text = "000";
            this.sizeTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtStories
            // 
            this.txtStories.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtStories.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStories.Location = new System.Drawing.Point(380, 75);
            this.txtStories.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.txtStories.Name = "txtStories";
            this.txtStories.Size = new System.Drawing.Size(58, 27);
            this.txtStories.TabIndex = 9;
            this.txtStories.Leave += new System.EventHandler(this.storyText_Leave);
            // 
            // cboSectionType
            // 
            this.cboSectionType.DisplayMember = "Description";
            this.cboSectionType.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSectionType.FormattingEnabled = true;
            this.cboSectionType.Location = new System.Drawing.Point(63, 77);
            this.cboSectionType.Name = "cboSectionType";
            this.cboSectionType.Size = new System.Drawing.Size(159, 25);
            this.cboSectionType.TabIndex = 7;
            this.cboSectionType.ValueMember = "Description";
            this.cboSectionType.SelectionChangeCommitted += new System.EventHandler(this.cboType_SelectionChangeCommitted);
            // 
            // lblSectionLetter
            // 
            this.lblSectionLetter.AutoSize = true;
            this.lblSectionLetter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSectionLetter.Location = new System.Drawing.Point(10, 77);
            this.lblSectionLetter.Name = "lblSectionLetter";
            this.lblSectionLetter.Size = new System.Drawing.Size(20, 20);
            this.lblSectionLetter.TabIndex = 6;
            this.lblSectionLetter.Text = "A";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeLabel.Location = new System.Drawing.Point(452, 51);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(36, 20);
            this.sizeLabel.TabIndex = 5;
            this.sizeLabel.Text = "Size";
            this.sizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblStories
            // 
            this.lblStories.AutoSize = true;
            this.lblStories.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStories.Location = new System.Drawing.Point(376, 51);
            this.lblStories.Name = "lblStories";
            this.lblStories.Size = new System.Drawing.Size(45, 20);
            this.lblStories.TabIndex = 4;
            this.lblStories.Text = "Story";
            this.lblStories.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionLabel.Location = new System.Drawing.Point(227, 51);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(87, 20);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(72, 51);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(42, 20);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Type";
            this.lblType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSection
            // 
            this.lblSection.AutoSize = true;
            this.lblSection.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSection.Location = new System.Drawing.Point(10, 51);
            this.lblSection.Name = "lblSection";
            this.lblSection.Size = new System.Drawing.Size(59, 20);
            this.lblSection.TabIndex = 1;
            this.lblSection.Text = "Section";
            this.lblSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDone
            // 
            this.btnDone.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDone.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDone.Image = global::SketchUp.Properties.Resources.GreenCheck16x16;
            this.btnDone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDone.Location = new System.Drawing.Point(1135, 28);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 31);
            this.btnDone.TabIndex = 1;
            this.btnDone.Text = "Done";
            this.btnDone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionsLabel.Location = new System.Drawing.Point(55, 9);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(808, 40);
            this.instructionsLabel.TabIndex = 0;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.lblValuationChanges);
            this.panel1.Controls.Add(this.lblValuationPanelTitle);
            this.panel1.Controls.Add(this.lblNewValueText);
            this.panel1.Controls.Add(this.lblNewValue);
            this.panel1.Controls.Add(this.lblRateValue);
            this.panel1.Controls.Add(this.lblRate);
            this.panel1.Controls.Add(this.lblValueText);
            this.panel1.Controls.Add(this.lblValue);
            this.panel1.Controls.Add(this.cboClass);
            this.panel1.Controls.Add(this.chkZeroDepr);
            this.panel1.Controls.Add(this.txtDepr);
            this.panel1.Controls.Add(this.lblDepr);
            this.panel1.Controls.Add(this.txtFactor);
            this.panel1.Controls.Add(this.adjFactorLabel);
            this.panel1.Controls.Add(this.classLabel);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(583, 291);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 198);
            this.panel1.TabIndex = 4;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(518, 132);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(78, 31);
            this.btnApply.TabIndex = 15;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // lblValuationChanges
            // 
            this.lblValuationChanges.CausesValidation = false;
            this.lblValuationChanges.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValuationChanges.Location = new System.Drawing.Point(14, 112);
            this.lblValuationChanges.Margin = new System.Windows.Forms.Padding(0);
            this.lblValuationChanges.Name = "lblValuationChanges";
            this.lblValuationChanges.Size = new System.Drawing.Size(474, 72);
            this.lblValuationChanges.TabIndex = 14;
            this.lblValuationChanges.Text = "\r\n*Changing the valuation model will show the new value for the section. If you a" +
    "re satisfied with the model, click \"Apply\" to change it in the list above. ";
            // 
            // lblValuationPanelTitle
            // 
            this.lblValuationPanelTitle.AutoSize = true;
            this.lblValuationPanelTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValuationPanelTitle.Location = new System.Drawing.Point(14, 8);
            this.lblValuationPanelTitle.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.lblValuationPanelTitle.Name = "lblValuationPanelTitle";
            this.lblValuationPanelTitle.Size = new System.Drawing.Size(125, 20);
            this.lblValuationPanelTitle.TabIndex = 0;
            this.lblValuationPanelTitle.Text = "Section Valuation";
            this.lblValuationPanelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewValueText
            // 
            this.lblNewValueText.BackColor = System.Drawing.Color.Azure;
            this.lblNewValueText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewValueText.Location = new System.Drawing.Point(514, 81);
            this.lblNewValueText.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.lblNewValueText.Name = "lblNewValueText";
            this.lblNewValueText.Size = new System.Drawing.Size(97, 20);
            this.lblNewValueText.TabIndex = 13;
            this.lblNewValueText.Text = "000";
            this.lblNewValueText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNewValue
            // 
            this.lblNewValue.AutoSize = true;
            this.lblNewValue.BackColor = System.Drawing.Color.Azure;
            this.lblNewValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNewValue.Location = new System.Drawing.Point(514, 55);
            this.lblNewValue.Name = "lblNewValue";
            this.lblNewValue.Size = new System.Drawing.Size(90, 20);
            this.lblNewValue.TabIndex = 7;
            this.lblNewValue.Text = "New Value*";
            this.lblNewValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblRateValue
            // 
            this.lblRateValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRateValue.Location = new System.Drawing.Point(326, 80);
            this.lblRateValue.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.lblRateValue.Name = "lblRateValue";
            this.lblRateValue.Size = new System.Drawing.Size(82, 20);
            this.lblRateValue.TabIndex = 11;
            this.lblRateValue.Text = "000";
            this.lblRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRate
            // 
            this.lblRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRate.Location = new System.Drawing.Point(326, 55);
            this.lblRate.Name = "lblRate";
            this.lblRate.Size = new System.Drawing.Size(64, 20);
            this.lblRate.TabIndex = 5;
            this.lblRate.Text = "Rate";
            this.lblRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblValueText
            // 
            this.lblValueText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValueText.Location = new System.Drawing.Point(417, 81);
            this.lblValueText.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.lblValueText.Name = "lblValueText";
            this.lblValueText.Size = new System.Drawing.Size(82, 20);
            this.lblValueText.TabIndex = 12;
            this.lblValueText.Text = "000";
            this.lblValueText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValue.Location = new System.Drawing.Point(417, 55);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(48, 20);
            this.lblValue.TabIndex = 6;
            this.lblValue.Text = "Value";
            this.lblValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboClass
            // 
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.DropDownWidth = 20;
            this.cboClass.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "M"});
            this.cboClass.Location = new System.Drawing.Point(88, 80);
            this.cboClass.MaxDropDownItems = 5;
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(40, 25);
            this.cboClass.Sorted = true;
            this.cboClass.TabIndex = 10;
            this.cboClass.SelectionChangeCommitted += new System.EventHandler(this.cboClass_SelectionChangeCommitted);
            // 
            // chkZeroDepr
            // 
            this.chkZeroDepr.AutoSize = true;
            this.chkZeroDepr.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkZeroDepr.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkZeroDepr.Location = new System.Drawing.Point(14, 55);
            this.chkZeroDepr.Name = "chkZeroDepr";
            this.chkZeroDepr.Size = new System.Drawing.Size(59, 41);
            this.chkZeroDepr.TabIndex = 1;
            this.chkZeroDepr.Text = "0 Depr";
            this.chkZeroDepr.UseVisualStyleBackColor = true;
            this.chkZeroDepr.CheckedChanged += new System.EventHandler(this.chkZeroDepr_CheckedChanged);
            // 
            // txtDepr
            // 
            this.txtDepr.AutoCompleteCustomSource.AddRange(new string[] {
            "A",
            "B",
            "C",
            "D",
            "M"});
            this.txtDepr.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtDepr.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtDepr.Location = new System.Drawing.Point(223, 79);
            this.txtDepr.Name = "txtDepr";
            this.txtDepr.Size = new System.Drawing.Size(82, 25);
            this.txtDepr.TabIndex = 9;
            this.txtDepr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDepr.Leave += new System.EventHandler(this.txtDepr_Leave);
            // 
            // lblDepr
            // 
            this.lblDepr.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDepr.Location = new System.Drawing.Point(219, 55);
            this.lblDepr.Name = "lblDepr";
            this.lblDepr.Size = new System.Drawing.Size(68, 20);
            this.lblDepr.TabIndex = 4;
            this.lblDepr.Text = "Depr";
            this.lblDepr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtFactor
            // 
            this.txtFactor.AutoCompleteCustomSource.AddRange(new string[] {
            "A",
            "B",
            "C",
            "D",
            "M"});
            this.txtFactor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtFactor.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtFactor.Location = new System.Drawing.Point(136, 79);
            this.txtFactor.Name = "txtFactor";
            this.txtFactor.Size = new System.Drawing.Size(82, 25);
            this.txtFactor.TabIndex = 8;
            this.txtFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtFactor.Leave += new System.EventHandler(this.txtFactor_Leave);
            // 
            // adjFactorLabel
            // 
            this.adjFactorLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.adjFactorLabel.Location = new System.Drawing.Point(136, 55);
            this.adjFactorLabel.Name = "adjFactorLabel";
            this.adjFactorLabel.Size = new System.Drawing.Size(68, 20);
            this.adjFactorLabel.TabIndex = 3;
            this.adjFactorLabel.Text = "Factor";
            this.adjFactorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // classLabel
            // 
            this.classLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.classLabel.Location = new System.Drawing.Point(84, 55);
            this.classLabel.Name = "classLabel";
            this.classLabel.Size = new System.Drawing.Size(68, 20);
            this.classLabel.TabIndex = 2;
            this.classLabel.Text = "Class";
            this.classLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Azure;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(504, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(116, 69);
            this.panel2.TabIndex = 16;
            // 
            // EditSketchSections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ClientSize = new System.Drawing.Size(1225, 517);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.sectionDetailsPanel);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.dgvSections);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditSketchSections";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Sketch Sections";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.EditSketchSections_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.sectionDetailsPanel.ResumeLayout(false);
            this.sectionDetailsPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.ComboBox cboSectionType;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Data.DataColumn descriptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label descriptionTextLabel;
        private System.Windows.Forms.DataGridView dgvSections;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Data.DataColumn sectionColumn;
        private System.Windows.Forms.Panel sectionDetailsPanel;
        private System.Windows.Forms.Label lblSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionLetterDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label lblSectionLetter;
        private System.Data.DataTable sectionsDt;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectCheckDataGridViewCheckBoxColumn;
        private System.Data.DataColumn selectColumn;
        private System.Data.DataColumn sizeColumn;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.Label sizeTextLabel;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel stlEditStatus;
        private System.Windows.Forms.Label lblStories;
        private System.Data.DataColumn storyColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn storyDataGridViewTextBoxColumn;
        private System.Windows.Forms.TextBox txtStories;
        private System.Windows.Forms.Label lblType;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private System.Windows.Forms.DataGridViewTextBoxColumn DeleteSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depreciation;
        private System.Windows.Forms.DataGridViewTextBoxColumn AdjFactor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZeroDepr;
        private System.Windows.Forms.DataGridViewTextBoxColumn SqFt;
        private System.Windows.Forms.DataGridViewTextBoxColumn StoriesText;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionType;
        private System.Windows.Forms.DataGridViewTextBoxColumn SectionLetter;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label classLabel;
        private System.Windows.Forms.Label adjFactorLabel;
        private System.Windows.Forms.TextBox txtFactor;
        private System.Windows.Forms.Label lblDepr;
        private System.Windows.Forms.TextBox txtDepr;
        private System.Windows.Forms.CheckBox chkZeroDepr;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.Label lblNewValueText;
        private System.Windows.Forms.Label lblNewValue;
        private System.Windows.Forms.Label lblRateValue;
        private System.Windows.Forms.Label lblRate;
        private System.Windows.Forms.Label lblValueText;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.Label lblDescriptionPanelTitle;
        private System.Windows.Forms.Label lblValuationPanelTitle;
        private System.Windows.Forms.Label lblValuationChanges;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox chkDeleteSection;
    }
}
