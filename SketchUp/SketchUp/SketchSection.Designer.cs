namespace SketchUp
{
    partial class SketchSection
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
            this.SectDGView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.UpdateSectionBtn = new System.Windows.Forms.Button();
            this.DeleteSectBtn = new System.Windows.Forms.Button();
            this.ResidentialSectionCbox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CommercialSectionCbox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ReOpenSectionBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SectDGView)).BeginInit();
            this.SuspendLayout();
            // 
            // SectDGView
            // 
            this.SectDGView.AllowUserToAddRows = false;
            this.SectDGView.AllowUserToDeleteRows = false;
            this.SectDGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SectDGView.Location = new System.Drawing.Point(104, 25);
            this.SectDGView.Name = "SectDGView";
            this.SectDGView.Size = new System.Drawing.Size(695, 188);
            this.SectDGView.TabIndex = 0;
            this.SectDGView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SectDGView_CellClick);
            this.SectDGView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.SectDGView_CellValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(103, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Building Sections";
            // 
            // UpdateSectionBtn
            // 
            this.UpdateSectionBtn.BackColor = System.Drawing.Color.Yellow;
            this.UpdateSectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UpdateSectionBtn.Location = new System.Drawing.Point(12, 25);
            this.UpdateSectionBtn.Name = "UpdateSectionBtn";
            this.UpdateSectionBtn.Size = new System.Drawing.Size(75, 54);
            this.UpdateSectionBtn.TabIndex = 6;
            this.UpdateSectionBtn.Text = "Update Sections";
            this.UpdateSectionBtn.UseVisualStyleBackColor = false;
            this.UpdateSectionBtn.Click += new System.EventHandler(this.UpdateSectionBtn_Click);
            // 
            // DeleteSectBtn
            // 
            this.DeleteSectBtn.BackColor = System.Drawing.Color.Red;
            this.DeleteSectBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteSectBtn.Location = new System.Drawing.Point(12, 96);
            this.DeleteSectBtn.Name = "DeleteSectBtn";
            this.DeleteSectBtn.Size = new System.Drawing.Size(75, 54);
            this.DeleteSectBtn.TabIndex = 7;
            this.DeleteSectBtn.Text = "Delete Section";
            this.DeleteSectBtn.UseVisualStyleBackColor = false;
            this.DeleteSectBtn.Click += new System.EventHandler(this.DeleteSectBtn_Click);
            // 
            // ResidentialSectionCbox
            // 
            this.ResidentialSectionCbox.FormattingEnabled = true;
            this.ResidentialSectionCbox.Location = new System.Drawing.Point(100, 248);
            this.ResidentialSectionCbox.Name = "ResidentialSectionCbox";
            this.ResidentialSectionCbox.Size = new System.Drawing.Size(177, 21);
            this.ResidentialSectionCbox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(105, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Residential Section Types";
            // 
            // CommercialSectionCbox
            // 
            this.CommercialSectionCbox.FormattingEnabled = true;
            this.CommercialSectionCbox.Location = new System.Drawing.Point(539, 248);
            this.CommercialSectionCbox.Name = "CommercialSectionCbox";
            this.CommercialSectionCbox.Size = new System.Drawing.Size(177, 21);
            this.CommercialSectionCbox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(543, 228);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(156, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Commercial Section Types";
            // 
            // ReOpenSectionBtn
            // 
            this.ReOpenSectionBtn.BackColor = System.Drawing.Color.DodgerBlue;
            this.ReOpenSectionBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReOpenSectionBtn.ForeColor = System.Drawing.Color.White;
            this.ReOpenSectionBtn.Location = new System.Drawing.Point(12, 170);
            this.ReOpenSectionBtn.Name = "ReOpenSectionBtn";
            this.ReOpenSectionBtn.Size = new System.Drawing.Size(75, 54);
            this.ReOpenSectionBtn.TabIndex = 9;
            this.ReOpenSectionBtn.Text = "Re-Open Section";
            this.ReOpenSectionBtn.UseVisualStyleBackColor = false;
            this.ReOpenSectionBtn.Visible = false;
            this.ReOpenSectionBtn.Click += new System.EventHandler(this.ReOpenSectionBtn_Click);
            // 
            // SketchSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(834, 306);
            this.Controls.Add(this.ReOpenSectionBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CommercialSectionCbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ResidentialSectionCbox);
            this.Controls.Add(this.DeleteSectBtn);
            this.Controls.Add(this.UpdateSectionBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SectDGView);
            this.Location = new System.Drawing.Point(750, 35);
            this.Name = "SketchSection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sketch Sections";
            ((System.ComponentModel.ISupportInitialize)(this.SectDGView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView SectDGView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button UpdateSectionBtn;
        private System.Windows.Forms.Button DeleteSectBtn;
        private System.Windows.Forms.ComboBox ResidentialSectionCbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CommercialSectionCbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ReOpenSectionBtn;
    }
}