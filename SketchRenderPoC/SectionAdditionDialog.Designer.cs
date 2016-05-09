namespace SketchUp
{
    partial class SectionAdditionDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SectionLetterCbox = new System.Windows.Forms.ComboBox();
            this.SectionTypesCbox = new System.Windows.Forms.ComboBox();
            this.SectionStoriesTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.CurOccTxt = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.newSectionGroup = new System.Windows.Forms.GroupBox();
            this.SectLtrTxt = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSquareFt = new System.Windows.Forms.RadioButton();
            this.rbNewSection = new System.Windows.Forms.RadioButton();
            this.squareFootageOnlyGroup = new System.Windows.Forms.GroupBox();
            this.SqftLbl = new System.Windows.Forms.Label();
            this.SectionSizeTxt = new System.Windows.Forms.TextBox();
            this.SizeOnlyLbl = new System.Windows.Forms.Label();
            this.newSectionGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.squareFootageOnlyGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "New Section Letter :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "New Section Type :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 129);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(170, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "New Section Stories :";
            // 
            // SectionLetterCbox
            // 
            this.SectionLetterCbox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.SectionLetterCbox.FormattingEnabled = true;
            this.SectionLetterCbox.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M"});
            this.SectionLetterCbox.Location = new System.Drawing.Point(422, 34);
            this.SectionLetterCbox.Margin = new System.Windows.Forms.Padding(4);
            this.SectionLetterCbox.Name = "SectionLetterCbox";
            this.SectionLetterCbox.Size = new System.Drawing.Size(65, 31);
            this.SectionLetterCbox.TabIndex = 4;
            this.SectionLetterCbox.Visible = false;
            // 
            // SectionTypesCbox
            // 
            this.SectionTypesCbox.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectionTypesCbox.FormattingEnabled = true;
            this.SectionTypesCbox.Location = new System.Drawing.Point(189, 81);
            this.SectionTypesCbox.Margin = new System.Windows.Forms.Padding(4);
            this.SectionTypesCbox.Name = "SectionTypesCbox";
            this.SectionTypesCbox.Size = new System.Drawing.Size(301, 31);
            this.SectionTypesCbox.TabIndex = 6;
            this.SectionTypesCbox.SelectedIndexChanged += new System.EventHandler(this.SectionTypesCbox_SelectedIndexChanged);
            // 
            // SectionStoriesTxt
            // 
            this.SectionStoriesTxt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectionStoriesTxt.Location = new System.Drawing.Point(189, 126);
            this.SectionStoriesTxt.Margin = new System.Windows.Forms.Padding(4);
            this.SectionStoriesTxt.Name = "SectionStoriesTxt";
            this.SectionStoriesTxt.Size = new System.Drawing.Size(65, 30);
            this.SectionStoriesTxt.TabIndex = 7;
            this.SectionStoriesTxt.Leave += new System.EventHandler(this.SectionStoriesTxt_Leave);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(32, 23);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Current Occupancy :";
            // 
            // CurOccTxt
            // 
            this.CurOccTxt.BackColor = System.Drawing.Color.PowderBlue;
            this.CurOccTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CurOccTxt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurOccTxt.ForeColor = System.Drawing.Color.Blue;
            this.CurOccTxt.Location = new System.Drawing.Point(226, 23);
            this.CurOccTxt.Margin = new System.Windows.Forms.Padding(4);
            this.CurOccTxt.Name = "CurOccTxt";
            this.CurOccTxt.ReadOnly = true;
            this.CurOccTxt.Size = new System.Drawing.Size(191, 23);
            this.CurOccTxt.TabIndex = 1;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(478, 401);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(87, 37);
            this.btnNext.TabIndex = 13;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // newSectionGroup
            // 
            this.newSectionGroup.Controls.Add(this.SectLtrTxt);
            this.newSectionGroup.Controls.Add(this.label1);
            this.newSectionGroup.Controls.Add(this.SectionLetterCbox);
            this.newSectionGroup.Controls.Add(this.SectionStoriesTxt);
            this.newSectionGroup.Controls.Add(this.label2);
            this.newSectionGroup.Controls.Add(this.label3);
            this.newSectionGroup.Controls.Add(this.SectionTypesCbox);
            this.newSectionGroup.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newSectionGroup.Location = new System.Drawing.Point(56, 136);
            this.newSectionGroup.Name = "newSectionGroup";
            this.newSectionGroup.Size = new System.Drawing.Size(510, 173);
            this.newSectionGroup.TabIndex = 14;
            this.newSectionGroup.TabStop = false;
            this.newSectionGroup.Text = "New Section";
            // 
            // SectLtrTxt
            // 
            this.SectLtrTxt.AutoSize = true;
            this.SectLtrTxt.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectLtrTxt.ForeColor = System.Drawing.Color.Blue;
            this.SectLtrTxt.Location = new System.Drawing.Point(185, 32);
            this.SectLtrTxt.Name = "SectLtrTxt";
            this.SectLtrTxt.Size = new System.Drawing.Size(26, 28);
            this.SectLtrTxt.TabIndex = 10;
            this.SectLtrTxt.Text = "A";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSquareFt);
            this.groupBox1.Controls.Add(this.rbNewSection);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(56, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Addition Type";
            // 
            // rbSquareFt
            // 
            this.rbSquareFt.AutoSize = true;
            this.rbSquareFt.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSquareFt.Location = new System.Drawing.Point(218, 34);
            this.rbSquareFt.Name = "rbSquareFt";
            this.rbSquareFt.Size = new System.Drawing.Size(193, 27);
            this.rbSquareFt.TabIndex = 1;
            this.rbSquareFt.Text = "Square Footage Only";
            this.rbSquareFt.UseVisualStyleBackColor = true;
            this.rbSquareFt.CheckedChanged += new System.EventHandler(this.rbSquareFt_CheckedChanged);
            // 
            // rbNewSection
            // 
            this.rbNewSection.AutoSize = true;
            this.rbNewSection.Checked = true;
            this.rbNewSection.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNewSection.Location = new System.Drawing.Point(75, 34);
            this.rbNewSection.Name = "rbNewSection";
            this.rbNewSection.Size = new System.Drawing.Size(127, 27);
            this.rbNewSection.TabIndex = 0;
            this.rbNewSection.TabStop = true;
            this.rbNewSection.Text = "New Section";
            this.rbNewSection.UseVisualStyleBackColor = true;
            this.rbNewSection.CheckedChanged += new System.EventHandler(this.rbNewSection_CheckedChanged);
            // 
            // squareFootageOnlyGroup
            // 
            this.squareFootageOnlyGroup.Controls.Add(this.SqftLbl);
            this.squareFootageOnlyGroup.Controls.Add(this.SectionSizeTxt);
            this.squareFootageOnlyGroup.Controls.Add(this.SizeOnlyLbl);
            this.squareFootageOnlyGroup.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.squareFootageOnlyGroup.Location = new System.Drawing.Point(58, 320);
            this.squareFootageOnlyGroup.Name = "squareFootageOnlyGroup";
            this.squareFootageOnlyGroup.Size = new System.Drawing.Size(508, 73);
            this.squareFootageOnlyGroup.TabIndex = 15;
            this.squareFootageOnlyGroup.TabStop = false;
            this.squareFootageOnlyGroup.Text = "Square Footage Only";
            // 
            // SqftLbl
            // 
            this.SqftLbl.AutoSize = true;
            this.SqftLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SqftLbl.Location = new System.Drawing.Point(298, 31);
            this.SqftLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SqftLbl.Name = "SqftLbl";
            this.SqftLbl.Size = new System.Drawing.Size(56, 23);
            this.SqftLbl.TabIndex = 16;
            this.SqftLbl.Text = "Sq. Ft.";
            this.SqftLbl.Visible = false;
            // 
            // SectionSizeTxt
            // 
            this.SectionSizeTxt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SectionSizeTxt.Location = new System.Drawing.Point(202, 27);
            this.SectionSizeTxt.Margin = new System.Windows.Forms.Padding(4);
            this.SectionSizeTxt.Name = "SectionSizeTxt";
            this.SectionSizeTxt.Size = new System.Drawing.Size(79, 30);
            this.SectionSizeTxt.TabIndex = 15;
            this.SectionSizeTxt.Visible = false;
            // 
            // SizeOnlyLbl
            // 
            this.SizeOnlyLbl.AutoSize = true;
            this.SizeOnlyLbl.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SizeOnlyLbl.Location = new System.Drawing.Point(13, 31);
            this.SizeOnlyLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SizeOnlyLbl.Name = "SizeOnlyLbl";
            this.SizeOnlyLbl.Size = new System.Drawing.Size(180, 23);
            this.SizeOnlyLbl.TabIndex = 14;
            this.SizeOnlyLbl.Text = "Sq Footage Only Size :";
            this.SizeOnlyLbl.Visible = false;
            // 
            // SectionAdditionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PowderBlue;
            this.ClientSize = new System.Drawing.Size(583, 450);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.newSectionGroup);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.CurOccTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.squareFootageOnlyGroup);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SectionAdditionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Building Section Types";
            this.newSectionGroup.ResumeLayout(false);
            this.newSectionGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.squareFootageOnlyGroup.ResumeLayout(false);
            this.squareFootageOnlyGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SectionLetterCbox;
        private System.Windows.Forms.ComboBox SectionTypesCbox;
        private System.Windows.Forms.TextBox SectionStoriesTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CurOccTxt;
		private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox newSectionGroup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSquareFt;
        private System.Windows.Forms.RadioButton rbNewSection;
        private System.Windows.Forms.GroupBox squareFootageOnlyGroup;
        private System.Windows.Forms.Label SizeOnlyLbl;
        private System.Windows.Forms.TextBox SectionSizeTxt;
        private System.Windows.Forms.Label SqftLbl;
        private System.Windows.Forms.Label SectLtrTxt;
    }
}