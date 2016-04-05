namespace SketchUp
{
    partial class AddSectionDialog
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
			this.currOccupancyLabel = new System.Windows.Forms.Label();
			this.CurOccTxt = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.rbNewSection = new System.Windows.Forms.RadioButton();
			this.rbSquareFootage = new System.Windows.Forms.RadioButton();
			this.additionTypeGroupBox = new System.Windows.Forms.GroupBox();
			this.sqFootageGroupBox = new System.Windows.Forms.GroupBox();
			this.SizeOnlyLbl = new System.Windows.Forms.Label();
			this.SquareFootageTextBox = new System.Windows.Forms.TextBox();
			this.SqftLbl = new System.Windows.Forms.Label();
			this.addSectionGroupBox = new System.Windows.Forms.GroupBox();
			this.SectionTypesCbox = new System.Windows.Forms.ComboBox();
			this.SectLtr = new System.Windows.Forms.Label();
			this.newSectionLetterLabel = new System.Windows.Forms.Label();
			this.newSectionTypeLabel = new System.Windows.Forms.Label();
			this.storeysLabel = new System.Windows.Forms.Label();
			this.SectionStoriesTxt = new System.Windows.Forms.TextBox();
			this.additionTypeGroupBox.SuspendLayout();
			this.sqFootageGroupBox.SuspendLayout();
			this.addSectionGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// currOccupancyLabel
			// 
			this.currOccupancyLabel.AutoSize = true;
			this.currOccupancyLabel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.currOccupancyLabel.Location = new System.Drawing.Point(48, 16);
			this.currOccupancyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.currOccupancyLabel.Name = "currOccupancyLabel";
			this.currOccupancyLabel.Size = new System.Drawing.Size(172, 23);
			this.currOccupancyLabel.TabIndex = 0;
			this.currOccupancyLabel.Text = "Current Occupancy :";
			// 
			// CurOccTxt
			// 
			this.CurOccTxt.BackColor = System.Drawing.Color.LightCyan;
			this.CurOccTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CurOccTxt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CurOccTxt.Location = new System.Drawing.Point(228, 16);
			this.CurOccTxt.Margin = new System.Windows.Forms.Padding(4, 2, 2, 2);
			this.CurOccTxt.Name = "CurOccTxt";
			this.CurOccTxt.ReadOnly = true;
			this.CurOccTxt.Size = new System.Drawing.Size(191, 23);
			this.CurOccTxt.TabIndex = 1;
			this.CurOccTxt.Text = "Residential";
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnAdd.Location = new System.Drawing.Point(507, 381);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(87, 46);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "Next";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// rbNewSection
			// 
			this.rbNewSection.AutoSize = true;
			this.rbNewSection.Checked = true;
			this.rbNewSection.Location = new System.Drawing.Point(59, 26);
			this.rbNewSection.Name = "rbNewSection";
			this.rbNewSection.Size = new System.Drawing.Size(113, 24);
			this.rbNewSection.TabIndex = 0;
			this.rbNewSection.TabStop = true;
			this.rbNewSection.Text = "New Section";
			this.rbNewSection.UseVisualStyleBackColor = true;
			this.rbNewSection.CheckedChanged += new System.EventHandler(this.rbNewSection_CheckedChanged);
			// 
			// rbSquareFootage
			// 
			this.rbSquareFootage.AutoSize = true;
			this.rbSquareFootage.Location = new System.Drawing.Point(243, 26);
			this.rbSquareFootage.Name = "rbSquareFootage";
			this.rbSquareFootage.Size = new System.Drawing.Size(169, 24);
			this.rbSquareFootage.TabIndex = 1;
			this.rbSquareFootage.Text = "Square Footage Only";
			this.rbSquareFootage.UseVisualStyleBackColor = true;
			this.rbSquareFootage.CheckedChanged += new System.EventHandler(this.rbSquareFootage_CheckedChanged);
			// 
			// additionTypeGroupBox
			// 
			this.additionTypeGroupBox.Controls.Add(this.rbSquareFootage);
			this.additionTypeGroupBox.Controls.Add(this.rbNewSection);
			this.additionTypeGroupBox.Location = new System.Drawing.Point(100, 42);
			this.additionTypeGroupBox.Name = "additionTypeGroupBox";
			this.additionTypeGroupBox.Size = new System.Drawing.Size(494, 68);
			this.additionTypeGroupBox.TabIndex = 2;
			this.additionTypeGroupBox.TabStop = false;
			this.additionTypeGroupBox.Text = "Add:";
			// 
			// sqFootageGroupBox
			// 
			this.sqFootageGroupBox.Controls.Add(this.SizeOnlyLbl);
			this.sqFootageGroupBox.Controls.Add(this.SquareFootageTextBox);
			this.sqFootageGroupBox.Controls.Add(this.SqftLbl);
			this.sqFootageGroupBox.Location = new System.Drawing.Point(100, 288);
			this.sqFootageGroupBox.Name = "sqFootageGroupBox";
			this.sqFootageGroupBox.Size = new System.Drawing.Size(494, 76);
			this.sqFootageGroupBox.TabIndex = 6;
			this.sqFootageGroupBox.TabStop = false;
			// 
			// SizeOnlyLbl
			// 
			this.SizeOnlyLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.SizeOnlyLbl.AutoSize = true;
			this.SizeOnlyLbl.Location = new System.Drawing.Point(26, 31);
			this.SizeOnlyLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SizeOnlyLbl.Name = "SizeOnlyLbl";
			this.SizeOnlyLbl.Size = new System.Drawing.Size(157, 20);
			this.SizeOnlyLbl.TabIndex = 3;
			this.SizeOnlyLbl.Text = "Sq Footage Only Size :";
			// 
			// SquareFootageTextBox
			// 
			this.SquareFootageTextBox.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.SquareFootageTextBox.Location = new System.Drawing.Point(191, 28);
			this.SquareFootageTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SquareFootageTextBox.MaxLength = 10;
			this.SquareFootageTextBox.Name = "SquareFootageTextBox";
			this.SquareFootageTextBox.Size = new System.Drawing.Size(79, 27);
			this.SquareFootageTextBox.TabIndex = 4;
			this.SquareFootageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// SqftLbl
			// 
			this.SqftLbl.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.SqftLbl.AutoSize = true;
			this.SqftLbl.Location = new System.Drawing.Point(278, 31);
			this.SqftLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SqftLbl.Name = "SqftLbl";
			this.SqftLbl.Size = new System.Drawing.Size(48, 20);
			this.SqftLbl.TabIndex = 5;
			this.SqftLbl.Text = "Sq. Ft.";
			// 
			// addSectionGroupBox
			// 
			this.addSectionGroupBox.Controls.Add(this.newSectionLetterLabel);
			this.addSectionGroupBox.Controls.Add(this.SectionTypesCbox);
			this.addSectionGroupBox.Controls.Add(this.SectLtr);
			this.addSectionGroupBox.Controls.Add(this.newSectionTypeLabel);
			this.addSectionGroupBox.Controls.Add(this.storeysLabel);
			this.addSectionGroupBox.Controls.Add(this.SectionStoriesTxt);
			this.addSectionGroupBox.Location = new System.Drawing.Point(100, 123);
			this.addSectionGroupBox.Name = "addSectionGroupBox";
			this.addSectionGroupBox.Size = new System.Drawing.Size(494, 147);
			this.addSectionGroupBox.TabIndex = 7;
			this.addSectionGroupBox.TabStop = false;
			this.addSectionGroupBox.Text = "New Section";
			// 
			// SectionTypesCbox
			// 
			this.SectionTypesCbox.FormattingEnabled = true;
			this.SectionTypesCbox.Location = new System.Drawing.Point(174, 68);
			this.SectionTypesCbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SectionTypesCbox.Name = "SectionTypesCbox";
			this.SectionTypesCbox.Size = new System.Drawing.Size(301, 28);
			this.SectionTypesCbox.Sorted = true;
			this.SectionTypesCbox.TabIndex = 15;
			this.SectionTypesCbox.SelectedIndexChanged += new System.EventHandler(this.SectionTypesCbox_SelectedIndexChanged);
			// 
			// SectLtr
			// 
			this.SectLtr.BackColor = System.Drawing.Color.LightCyan;
			this.SectLtr.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SectLtr.Location = new System.Drawing.Point(174, 31);
			this.SectLtr.Name = "SectLtr";
			this.SectLtr.Size = new System.Drawing.Size(27, 29);
			this.SectLtr.TabIndex = 13;
			this.SectLtr.Text = "A";
			this.SectLtr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// newSectionLetterLabel
			// 
			this.newSectionLetterLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newSectionLetterLabel.Location = new System.Drawing.Point(26, 34);
			this.newSectionLetterLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.newSectionLetterLabel.Name = "newSectionLetterLabel";
			this.newSectionLetterLabel.Size = new System.Drawing.Size(141, 20);
			this.newSectionLetterLabel.TabIndex = 12;
			this.newSectionLetterLabel.Text = "New Section Letter :";
			this.newSectionLetterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// newSectionTypeLabel
			// 
			this.newSectionTypeLabel.AutoSize = true;
			this.newSectionTypeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newSectionTypeLabel.Location = new System.Drawing.Point(31, 71);
			this.newSectionTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.newSectionTypeLabel.Name = "newSectionTypeLabel";
			this.newSectionTypeLabel.Size = new System.Drawing.Size(135, 20);
			this.newSectionTypeLabel.TabIndex = 14;
			this.newSectionTypeLabel.Text = "New Section Type :";
			this.newSectionTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// storeysLabel
			// 
			this.storeysLabel.AutoSize = true;
			this.storeysLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.storeysLabel.Location = new System.Drawing.Point(19, 111);
			this.storeysLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.storeysLabel.Name = "storeysLabel";
			this.storeysLabel.Size = new System.Drawing.Size(148, 20);
			this.storeysLabel.TabIndex = 16;
			this.storeysLabel.Text = "New Section Stories :";
			this.storeysLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SectionStoriesTxt
			// 
			this.SectionStoriesTxt.Location = new System.Drawing.Point(175, 106);
			this.SectionStoriesTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SectionStoriesTxt.MaxLength = 6;
			this.SectionStoriesTxt.Name = "SectionStoriesTxt";
			this.SectionStoriesTxt.Size = new System.Drawing.Size(65, 27);
			this.SectionStoriesTxt.TabIndex = 17;
			this.SectionStoriesTxt.Text = "1";
			this.SectionStoriesTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.SectionStoriesTxt.WordWrap = false;
			this.SectionStoriesTxt.Enter += new System.EventHandler(this.SectionStoriesTxt_Enter);
			// 
			// AddSectionDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightBlue;
			this.ClientSize = new System.Drawing.Size(675, 449);
			this.Controls.Add(this.sqFootageGroupBox);
			this.Controls.Add(this.additionTypeGroupBox);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.CurOccTxt);
			this.Controls.Add(this.currOccupancyLabel);
			this.Controls.Add(this.addSectionGroupBox);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "AddSectionDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Select Building Section Type";
		
			this.additionTypeGroupBox.ResumeLayout(false);
			this.additionTypeGroupBox.PerformLayout();
			this.sqFootageGroupBox.ResumeLayout(false);
			this.sqFootageGroupBox.PerformLayout();
			this.addSectionGroupBox.ResumeLayout(false);
			this.addSectionGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label currOccupancyLabel;
        private System.Windows.Forms.TextBox CurOccTxt;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.RadioButton rbNewSection;
		private System.Windows.Forms.RadioButton rbSquareFootage;
		private System.Windows.Forms.GroupBox additionTypeGroupBox;
		private System.Windows.Forms.GroupBox sqFootageGroupBox;
		private System.Windows.Forms.Label SqftLbl;
		private System.Windows.Forms.TextBox SquareFootageTextBox;
		private System.Windows.Forms.Label SizeOnlyLbl;
		private System.Windows.Forms.GroupBox addSectionGroupBox;
		private System.Windows.Forms.TextBox SectionStoriesTxt;
		private System.Windows.Forms.Label storeysLabel;
		private System.Windows.Forms.Label newSectionTypeLabel;
		private System.Windows.Forms.Label newSectionLetterLabel;
		private System.Windows.Forms.Label SectLtr;
		private System.Windows.Forms.ComboBox SectionTypesCbox;
	}
}