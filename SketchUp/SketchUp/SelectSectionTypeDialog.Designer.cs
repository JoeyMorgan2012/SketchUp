namespace SketchUp
{
    partial class SelectSectionTypeDialog
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.SectionLetterCbox = new System.Windows.Forms.ComboBox();
			this.SectionTypesCbox = new System.Windows.Forms.ComboBox();
			this.SectionStoriesTxt = new System.Windows.Forms.TextBox();
			this.SectLtrTxt = new System.Windows.Forms.TextBox();
			this.SizeOnlyLbl = new System.Windows.Forms.Label();
			this.SectionSizeTxt = new System.Windows.Forms.TextBox();
			this.SqftLbl = new System.Windows.Forms.Label();
			this.Sqftcbox = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CurOccTxt = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(65, 79);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(135, 17);
			this.label1.TabIndex = 2;
			this.label1.Text = "New Section Letter :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(69, 130);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 17);
			this.label2.TabIndex = 5;
			this.label2.Text = "New Section Type :";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(59, 182);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(142, 17);
			this.label3.TabIndex = 9;
			this.label3.Text = "New Section Stories :";
			// 
			// SectionLetterCbox
			// 
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
			this.SectionLetterCbox.Location = new System.Drawing.Point(233, 75);
			this.SectionLetterCbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SectionLetterCbox.Name = "SectionLetterCbox";
			this.SectionLetterCbox.Size = new System.Drawing.Size(65, 24);
			this.SectionLetterCbox.TabIndex = 4;
			this.SectionLetterCbox.Visible = false;
			// 
			// SectionTypesCbox
			// 
			this.SectionTypesCbox.FormattingEnabled = true;
			this.SectionTypesCbox.Location = new System.Drawing.Point(233, 127);
			this.SectionTypesCbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SectionTypesCbox.Name = "SectionTypesCbox";
			this.SectionTypesCbox.Size = new System.Drawing.Size(301, 24);
			this.SectionTypesCbox.TabIndex = 6;
			this.SectionTypesCbox.SelectedIndexChanged += new System.EventHandler(this.SectionTypesCbox_SelectedIndexChanged);
			// 
			// SectionStoriesTxt
			// 
			this.SectionStoriesTxt.Location = new System.Drawing.Point(233, 172);
			this.SectionStoriesTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SectionStoriesTxt.Name = "SectionStoriesTxt";
			this.SectionStoriesTxt.Size = new System.Drawing.Size(65, 22);
			this.SectionStoriesTxt.TabIndex = 7;
			this.SectionStoriesTxt.Leave += new System.EventHandler(this.SectionStoriesTxt_Leave);
			// 
			// SectLtrTxt
			// 
			this.SectLtrTxt.Location = new System.Drawing.Point(233, 75);
			this.SectLtrTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SectLtrTxt.Name = "SectLtrTxt";
			this.SectLtrTxt.Size = new System.Drawing.Size(65, 22);
			this.SectLtrTxt.TabIndex = 3;
			// 
			// SizeOnlyLbl
			// 
			this.SizeOnlyLbl.AutoSize = true;
			this.SizeOnlyLbl.Location = new System.Drawing.Point(51, 238);
			this.SizeOnlyLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SizeOnlyLbl.Name = "SizeOnlyLbl";
			this.SizeOnlyLbl.Size = new System.Drawing.Size(153, 17);
			this.SizeOnlyLbl.TabIndex = 10;
			this.SizeOnlyLbl.Text = "Sq Footage Only Size :";
			this.SizeOnlyLbl.Visible = false;
			// 
			// SectionSizeTxt
			// 
			this.SectionSizeTxt.Location = new System.Drawing.Point(233, 234);
			this.SectionSizeTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SectionSizeTxt.Name = "SectionSizeTxt";
			this.SectionSizeTxt.Size = new System.Drawing.Size(79, 22);
			this.SectionSizeTxt.TabIndex = 11;
			this.SectionSizeTxt.Visible = false;
			this.SectionSizeTxt.Leave += new System.EventHandler(this.SectionSizeTxt_Leave);
			// 
			// SqftLbl
			// 
			this.SqftLbl.AutoSize = true;
			this.SqftLbl.Location = new System.Drawing.Point(329, 238);
			this.SqftLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SqftLbl.Name = "SqftLbl";
			this.SqftLbl.Size = new System.Drawing.Size(49, 17);
			this.SqftLbl.TabIndex = 12;
			this.SqftLbl.Text = "Sq. Ft.";
			this.SqftLbl.Visible = false;
			// 
			// Sqftcbox
			// 
			this.Sqftcbox.AutoSize = true;
			this.Sqftcbox.Location = new System.Drawing.Point(401, 176);
			this.Sqftcbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Sqftcbox.Name = "Sqftcbox";
			this.Sqftcbox.Size = new System.Drawing.Size(96, 21);
			this.Sqftcbox.TabIndex = 8;
			this.Sqftcbox.Text = "Sq Ft Only";
			this.Sqftcbox.UseVisualStyleBackColor = true;
			this.Sqftcbox.CheckedChanged += new System.EventHandler(this.Sqftcbox_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(39, 34);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(157, 17);
			this.label4.TabIndex = 0;
			this.label4.Text = "Current Occupancy :";
			// 
			// CurOccTxt
			// 
			this.CurOccTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.CurOccTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CurOccTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CurOccTxt.Location = new System.Drawing.Point(233, 34);
			this.CurOccTxt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CurOccTxt.Name = "CurOccTxt";
			this.CurOccTxt.ReadOnly = true;
			this.CurOccTxt.Size = new System.Drawing.Size(191, 16);
			this.CurOccTxt.TabIndex = 1;
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(523, 262);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(87, 37);
			this.btnAdd.TabIndex = 13;
			this.btnAdd.Text = "Next";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// SectionTypes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(639, 311);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.CurOccTxt);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Sqftcbox);
			this.Controls.Add(this.SqftLbl);
			this.Controls.Add(this.SectionSizeTxt);
			this.Controls.Add(this.SizeOnlyLbl);
			this.Controls.Add(this.SectLtrTxt);
			this.Controls.Add(this.SectionStoriesTxt);
			this.Controls.Add(this.SectionTypesCbox);
			this.Controls.Add(this.SectionLetterCbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "SectionTypes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Building Section Types";
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
        private System.Windows.Forms.TextBox SectLtrTxt;
        private System.Windows.Forms.Label SizeOnlyLbl;
        private System.Windows.Forms.TextBox SectionSizeTxt;
        private System.Windows.Forms.Label SqftLbl;
        private System.Windows.Forms.CheckBox Sqftcbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CurOccTxt;
		private System.Windows.Forms.Button btnAdd;
	}
}