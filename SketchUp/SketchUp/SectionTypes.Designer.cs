namespace SketchUp
{
    partial class SectionTypes
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
			this.SectionTypesCbox = new System.Windows.Forms.ComboBox();
			this.SectionStoriesTxt = new System.Windows.Forms.TextBox();
			this.SizeOnlyLbl = new System.Windows.Forms.Label();
			this.SectionSizeTxt = new System.Windows.Forms.TextBox();
			this.SqftLbl = new System.Windows.Forms.Label();
			this.Sqftcbox = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.CurOccTxt = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.SectLtrLabel = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(91, 144);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(141, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "New Section Letter :";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(96, 181);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(135, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "New Section Type :";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(84, 221);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(148, 20);
			this.label3.TabIndex = 8;
			this.label3.Text = "New Section Stories :";
			// 
			// SectionTypesCbox
			// 
			this.SectionTypesCbox.FormattingEnabled = true;
			this.SectionTypesCbox.Location = new System.Drawing.Point(260, 178);
			this.SectionTypesCbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SectionTypesCbox.Name = "SectionTypesCbox";
			this.SectionTypesCbox.Size = new System.Drawing.Size(301, 28);
			this.SectionTypesCbox.TabIndex = 5;
			this.SectionTypesCbox.SelectedIndexChanged += new System.EventHandler(this.SectionTypesCbox_SelectedIndexChanged);
			// 
			// SectionStoriesTxt
			// 
			this.SectionStoriesTxt.Location = new System.Drawing.Point(260, 216);
			this.SectionStoriesTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SectionStoriesTxt.Name = "SectionStoriesTxt";
			this.SectionStoriesTxt.Size = new System.Drawing.Size(65, 27);
			this.SectionStoriesTxt.TabIndex = 6;
			this.SectionStoriesTxt.Leave += new System.EventHandler(this.SectionStoriesTxt_Leave);
			// 
			// SizeOnlyLbl
			// 
			this.SizeOnlyLbl.AutoSize = true;
			this.SizeOnlyLbl.Location = new System.Drawing.Point(78, 303);
			this.SizeOnlyLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SizeOnlyLbl.Name = "SizeOnlyLbl";
			this.SizeOnlyLbl.Size = new System.Drawing.Size(157, 20);
			this.SizeOnlyLbl.TabIndex = 9;
			this.SizeOnlyLbl.Text = "Sq Footage Only Size :";
			this.SizeOnlyLbl.Visible = false;
			// 
			// SectionSizeTxt
			// 
			this.SectionSizeTxt.Location = new System.Drawing.Point(260, 297);
			this.SectionSizeTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.SectionSizeTxt.Name = "SectionSizeTxt";
			this.SectionSizeTxt.Size = new System.Drawing.Size(79, 27);
			this.SectionSizeTxt.TabIndex = 10;
			this.SectionSizeTxt.Visible = false;
			this.SectionSizeTxt.Leave += new System.EventHandler(this.SectionSizeTxt_Leave);
			// 
			// SqftLbl
			// 
			this.SqftLbl.AutoSize = true;
			this.SqftLbl.Location = new System.Drawing.Point(356, 303);
			this.SqftLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SqftLbl.Name = "SqftLbl";
			this.SqftLbl.Size = new System.Drawing.Size(48, 20);
			this.SqftLbl.TabIndex = 11;
			this.SqftLbl.Text = "Sq. Ft.";
			this.SqftLbl.Visible = false;
			// 
			// Sqftcbox
			// 
			this.Sqftcbox.AutoSize = true;
			this.Sqftcbox.Location = new System.Drawing.Point(427, 265);
			this.Sqftcbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Sqftcbox.Name = "Sqftcbox";
			this.Sqftcbox.Size = new System.Drawing.Size(98, 24);
			this.Sqftcbox.TabIndex = 7;
			this.Sqftcbox.Text = "Sq Ft Only";
			this.Sqftcbox.UseVisualStyleBackColor = true;
			this.Sqftcbox.CheckedChanged += new System.EventHandler(this.Sqftcbox_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(48, 16);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(172, 23);
			this.label4.TabIndex = 0;
			this.label4.Text = "Current Occupancy :";
			// 
			// CurOccTxt
			// 
			this.CurOccTxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.CurOccTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.CurOccTxt.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CurOccTxt.Location = new System.Drawing.Point(228, 16);
			this.CurOccTxt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.CurOccTxt.Name = "CurOccTxt";
			this.CurOccTxt.ReadOnly = true;
			this.CurOccTxt.Size = new System.Drawing.Size(191, 23);
			this.CurOccTxt.TabIndex = 1;
			this.CurOccTxt.Text = "Residential";
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd.Location = new System.Drawing.Point(681, 310);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(87, 46);
			this.btnAdd.TabIndex = 12;
			this.btnAdd.Text = "Next";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// SectLtrLabel
			// 
			this.SectLtrLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SectLtrLabel.Location = new System.Drawing.Point(256, 144);
			this.SectLtrLabel.Name = "SectLtrLabel";
			this.SectLtrLabel.Size = new System.Drawing.Size(100, 29);
			this.SectLtrLabel.TabIndex = 3;
			this.SectLtrLabel.Text = "A";
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(360, 41);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(349, 100);
			this.panel1.TabIndex = 13;
			// 
			// SectionTypes
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(788, 397);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.SectLtrLabel);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.CurOccTxt);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.Sqftcbox);
			this.Controls.Add(this.SqftLbl);
			this.Controls.Add(this.SectionSizeTxt);
			this.Controls.Add(this.SizeOnlyLbl);
			this.Controls.Add(this.SectionStoriesTxt);
			this.Controls.Add(this.SectionTypesCbox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        private System.Windows.Forms.ComboBox SectionTypesCbox;
        private System.Windows.Forms.TextBox SectionStoriesTxt;
        private System.Windows.Forms.Label SizeOnlyLbl;
        private System.Windows.Forms.TextBox SectionSizeTxt;
        private System.Windows.Forms.Label SqftLbl;
        private System.Windows.Forms.CheckBox Sqftcbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox CurOccTxt;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label SectLtrLabel;
		private System.Windows.Forms.Panel panel1;
	}
}