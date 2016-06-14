namespace SketchUp
{
    partial class EditSketchSections
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditSketchSections));
            this.dtSectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.sectionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.storyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sizeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.stlSection = new System.Windows.Forms.ToolStripStatusLabel();
            this.stlEditStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.editSketchSectionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.editSketchToolstrip = new System.Windows.Forms.ToolStrip();
            this.tsbSave = new System.Windows.Forms.ToolStripButton();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.btnDeleteSection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.descriptionTextLabel = new System.Windows.Forms.Label();
            this.sizeTextLabel = new System.Windows.Forms.Label();
            this.storyText = new System.Windows.Forms.TextBox();
            this.cboSectionType = new System.Windows.Forms.ComboBox();
            this.sectionLetterLabel = new System.Windows.Forms.Label();
            this.sizeLabel = new System.Windows.Forms.Label();
            this.storeysTextLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.typeLabel = new System.Windows.Forms.Label();
            this.sectionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtSectionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).BeginInit();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSketchSectionsBindingSource)).BeginInit();
            this.editSketchToolstrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dtSectionsBindingSource
            // 
            this.dtSectionsBindingSource.AllowNew = false;
            this.dtSectionsBindingSource.DataSource = this.sectionsDt;
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
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(4);
            this.dgvSections.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSections.AutoGenerateColumns = false;
            this.dgvSections.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSections.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvSections.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.BlanchedAlmond;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(4);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSections.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sectionCol,
            this.typeCol,
            this.descriptionCol,
            this.storyCol,
            this.sizeCol});
            this.dgvSections.DataSource = this.sectionsDt;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSections.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvSections.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnF2;
            this.dgvSections.Location = new System.Drawing.Point(26, 36);
            this.dgvSections.Name = "dgvSections";
            this.dgvSections.ReadOnly = true;
            this.dgvSections.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            this.dgvSections.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvSections.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(2);
            this.dgvSections.RowTemplate.Height = 24;
            this.dgvSections.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSections.ShowEditingIcon = false;
            this.dgvSections.Size = new System.Drawing.Size(857, 220);
            this.dgvSections.TabIndex = 3;
            this.dgvSections.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvSections_DataError);
            this.dgvSections.SelectionChanged += new System.EventHandler(this.dgvSections_SelectionChanged);
            this.dgvSections.Enter += new System.EventHandler(this.dgvSections_Enter);
            // 
            // sectionCol
            // 
            this.sectionCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.sectionCol.DataPropertyName = "SectionLetter";
            this.sectionCol.FillWeight = 150F;
            this.sectionCol.HeaderText = "Section";
            this.sectionCol.MinimumWidth = 150;
            this.sectionCol.Name = "sectionCol";
            this.sectionCol.ReadOnly = true;
            // 
            // typeCol
            // 
            this.typeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.typeCol.DataPropertyName = "SectionType";
            this.typeCol.FillWeight = 200F;
            this.typeCol.HeaderText = "Type";
            this.typeCol.MinimumWidth = 30;
            this.typeCol.Name = "typeCol";
            this.typeCol.ReadOnly = true;
            // 
            // descriptionCol
            // 
            this.descriptionCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.descriptionCol.DataPropertyName = "Description";
            this.descriptionCol.FillWeight = 400F;
            this.descriptionCol.HeaderText = "Description";
            this.descriptionCol.MinimumWidth = 75;
            this.descriptionCol.Name = "descriptionCol";
            this.descriptionCol.ReadOnly = true;
            // 
            // storyCol
            // 
            this.storyCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.storyCol.DataPropertyName = "StoreysText";
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            this.storyCol.DefaultCellStyle = dataGridViewCellStyle3;
            this.storyCol.FillWeight = 150F;
            this.storyCol.HeaderText = "Story";
            this.storyCol.MinimumWidth = 30;
            this.storyCol.Name = "storyCol";
            this.storyCol.ReadOnly = true;
            // 
            // sizeCol
            // 
            this.sizeCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.Black;
            this.sizeCol.DefaultCellStyle = dataGridViewCellStyle4;
            this.sizeCol.FillWeight = 200F;
            this.sizeCol.HeaderText = "Size";
            this.sizeCol.MinimumWidth = 30;
            this.sizeCol.Name = "sizeCol";
            this.sizeCol.ReadOnly = true;
            // 
            // statusStripMain
            // 
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stlSection,
            this.stlEditStatus});
            this.statusStripMain.Location = new System.Drawing.Point(0, 512);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(909, 25);
            this.statusStripMain.TabIndex = 5;
            this.statusStripMain.Text = "statusStrip1";
            // 
            // stlSection
            // 
            this.stlSection.Name = "stlSection";
            this.stlSection.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.stlSection.Size = new System.Drawing.Size(859, 20);
            this.stlSection.Spring = true;
            this.stlSection.Text = "Selected Section(s):";
            this.stlSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stlEditStatus
            // 
            this.stlEditStatus.Image = global::SketchUp.Properties.Resources.GreenCheck;
            this.stlEditStatus.Name = "stlEditStatus";
            this.stlEditStatus.Size = new System.Drawing.Size(35, 20);
            this.stlEditStatus.Text = "*";
            // 
            // editSketchSectionsBindingSource
            // 
            this.editSketchSectionsBindingSource.DataMember = "SectionCboList";
            this.editSketchSectionsBindingSource.DataSource = typeof(SketchUp.EditSketchSections);
            // 
            // editSketchToolstrip
            // 
            this.editSketchToolstrip.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.editSketchToolstrip.ImageScalingSize = new System.Drawing.Size(30, 30);
            this.editSketchToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbSave,
            this.tsbExit});
            this.editSketchToolstrip.Location = new System.Drawing.Point(0, 0);
            this.editSketchToolstrip.Margin = new System.Windows.Forms.Padding(4);
            this.editSketchToolstrip.Name = "editSketchToolstrip";
            this.editSketchToolstrip.Size = new System.Drawing.Size(909, 33);
            this.editSketchToolstrip.Stretch = true;
            this.editSketchToolstrip.TabIndex = 16;
            this.editSketchToolstrip.Text = "Menu";
            // 
            // tsbSave
            // 
            this.tsbSave.AutoSize = false;
            this.tsbSave.Image = global::SketchUp.Properties.Resources.Save;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(75, 30);
            this.tsbSave.Text = "Save";
            this.tsbSave.ToolTipText = "Save Changes";
            this.tsbSave.Click += new System.EventHandler(this.tsbSave_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.AutoSize = false;
            this.tsbExit.Image = global::SketchUp.Properties.Resources.Exit_16x;
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(75, 30);
            this.tsbExit.Text = "Exit";
            this.tsbExit.ToolTipText = "Exit to Sketch";
            this.tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // btnDeleteSection
            // 
            this.btnDeleteSection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSection.Location = new System.Drawing.Point(764, 72);
            this.btnDeleteSection.Name = "btnDeleteSection";
            this.btnDeleteSection.Size = new System.Drawing.Size(79, 40);
            this.btnDeleteSection.TabIndex = 17;
            this.btnDeleteSection.Text = "Delete";
            this.btnDeleteSection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDeleteSection.UseVisualStyleBackColor = true;
            this.btnDeleteSection.Click += new System.EventHandler(this.btnDeleteSection_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightBlue;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.descriptionTextLabel);
            this.panel1.Controls.Add(this.btnDeleteSection);
            this.panel1.Controls.Add(this.sizeTextLabel);
            this.panel1.Controls.Add(this.storyText);
            this.panel1.Controls.Add(this.cboSectionType);
            this.panel1.Controls.Add(this.sectionLetterLabel);
            this.panel1.Controls.Add(this.sizeLabel);
            this.panel1.Controls.Add(this.storeysTextLabel);
            this.panel1.Controls.Add(this.descriptionLabel);
            this.panel1.Controls.Add(this.typeLabel);
            this.panel1.Controls.Add(this.sectionLabel);
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(26, 275);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(857, 126);
            this.panel1.TabIndex = 18;
            // 
            // descriptionTextLabel
            // 
            this.descriptionTextLabel.AutoSize = true;
            this.descriptionTextLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionTextLabel.Location = new System.Drawing.Point(218, 60);
            this.descriptionTextLabel.Name = "descriptionTextLabel";
            this.descriptionTextLabel.Size = new System.Drawing.Size(87, 20);
            this.descriptionTextLabel.TabIndex = 25;
            this.descriptionTextLabel.Text = "Description";
            this.descriptionTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sizeTextLabel
            // 
            this.sizeTextLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeTextLabel.Location = new System.Drawing.Point(660, 25);
            this.sizeTextLabel.Margin = new System.Windows.Forms.Padding(3, 0, 2, 0);
            this.sizeTextLabel.Name = "sizeTextLabel";
            this.sizeTextLabel.Size = new System.Drawing.Size(87, 20);
            this.sizeTextLabel.TabIndex = 24;
            this.sizeTextLabel.Text = "000";
            this.sizeTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // storyText
            // 
            this.storyText.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storyText.Location = new System.Drawing.Point(525, 21);
            this.storyText.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.storyText.Name = "storyText";
            this.storyText.Size = new System.Drawing.Size(75, 27);
            this.storyText.TabIndex = 23;
            // 
            // cboSectionType
            // 
            this.cboSectionType.DataSource = this.editSketchSectionsBindingSource;
            this.cboSectionType.DisplayMember = "Code";
            this.cboSectionType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSectionType.FormattingEnabled = true;
            this.cboSectionType.Location = new System.Drawing.Point(220, 21);
            this.cboSectionType.Name = "cboSectionType";
            this.cboSectionType.Size = new System.Drawing.Size(234, 28);
            this.cboSectionType.TabIndex = 22;
            this.cboSectionType.ValueMember = "Description";
            // 
            // sectionLetterLabel
            // 
            this.sectionLetterLabel.AutoSize = true;
            this.sectionLetterLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionLetterLabel.Location = new System.Drawing.Point(79, 24);
            this.sectionLetterLabel.Name = "sectionLetterLabel";
            this.sectionLetterLabel.Size = new System.Drawing.Size(20, 20);
            this.sectionLetterLabel.TabIndex = 21;
            this.sectionLetterLabel.Text = "A";
            // 
            // sizeLabel
            // 
            this.sizeLabel.AutoSize = true;
            this.sizeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sizeLabel.Location = new System.Drawing.Point(618, 24);
            this.sizeLabel.Name = "sizeLabel";
            this.sizeLabel.Size = new System.Drawing.Size(36, 20);
            this.sizeLabel.TabIndex = 20;
            this.sizeLabel.Text = "Size";
            this.sizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // storeysTextLabel
            // 
            this.storeysTextLabel.AutoSize = true;
            this.storeysTextLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeysTextLabel.Location = new System.Drawing.Point(473, 24);
            this.storeysTextLabel.Name = "storeysTextLabel";
            this.storeysTextLabel.Size = new System.Drawing.Size(45, 20);
            this.storeysTextLabel.TabIndex = 19;
            this.storeysTextLabel.Text = "Story";
            this.storeysTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.descriptionLabel.Location = new System.Drawing.Point(125, 60);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(87, 20);
            this.descriptionLabel.TabIndex = 18;
            this.descriptionLabel.Text = "Description";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.typeLabel.Location = new System.Drawing.Point(116, 24);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(96, 20);
            this.typeLabel.TabIndex = 17;
            this.typeLabel.Text = "Section Type";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sectionLabel
            // 
            this.sectionLabel.AutoSize = true;
            this.sectionLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionLabel.Location = new System.Drawing.Point(14, 24);
            this.sectionLabel.Name = "sectionLabel";
            this.sectionLabel.Size = new System.Drawing.Size(59, 20);
            this.sectionLabel.TabIndex = 16;
            this.sectionLabel.Text = "Section";
            this.sectionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // EditSketchSections
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.ClientSize = new System.Drawing.Size(909, 537);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.editSketchToolstrip);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.dgvSections);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditSketchSections";
            this.Text = "Edit Sketch Sections";
            ((System.ComponentModel.ISupportInitialize)(this.dtSectionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionsDt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSections)).EndInit();
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editSketchSectionsBindingSource)).EndInit();
            this.editSketchToolstrip.ResumeLayout(false);
            this.editSketchToolstrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource dtSectionsBindingSource;
        private System.Data.DataTable sectionsDt;
        private System.Data.DataColumn selectColumn;
        private System.Data.DataColumn sectionColumn;
        private System.Data.DataColumn descriptionColumn;
        private System.Data.DataColumn storyColumn;
        private System.Data.DataColumn sizeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectCheckDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionLetterDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn storyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionSizeDataGridViewTextBoxColumn;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.ToolStripStatusLabel stlSection;
        private System.Windows.Forms.DataGridViewTextBoxColumn sizeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn storyCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn sectionCol;
        private System.Windows.Forms.DataGridView dgvSections;
        private System.Windows.Forms.ToolStripStatusLabel stlEditStatus;
        private System.Windows.Forms.BindingSource editSketchSectionsBindingSource;
        private System.Windows.Forms.ToolStrip editSketchToolstrip;
        private System.Windows.Forms.ToolStripButton tsbSave;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.Button btnDeleteSection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label sectionLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label storeysTextLabel;
        private System.Windows.Forms.Label sizeLabel;
        private System.Windows.Forms.Label sectionLetterLabel;
        private System.Windows.Forms.ComboBox cboSectionType;
        private System.Windows.Forms.TextBox storyText;
        private System.Windows.Forms.Label sizeTextLabel;
        private System.Windows.Forms.Label descriptionTextLabel;
    }
}