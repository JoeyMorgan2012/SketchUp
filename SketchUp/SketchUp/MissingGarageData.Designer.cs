namespace SketchUp
{
    partial class MissingGarageData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissingGarageData));
            this.label1 = new System.Windows.Forms.Label();
            this.MissingGarLbl = new System.Windows.Forms.Label();
            this.GarLbl = new System.Windows.Forms.Label();
            this.CarPortLbl = new System.Windows.Forms.Label();
            this.GarTypeCbox = new System.Windows.Forms.ComboBox();
            this.CarPortTypCbox = new System.Windows.Forms.ComboBox();
            this.GarNbrCarTxt = new System.Windows.Forms.TextBox();
            this.CarPortNbrCarTxt = new System.Windows.Forms.TextBox();
            this.MissingCarporLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(79, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current Garage Information Missing";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MissingGarLbl
            // 
            this.MissingGarLbl.AutoSize = true;
            this.MissingGarLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissingGarLbl.Location = new System.Drawing.Point(336, 96);
            this.MissingGarLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MissingGarLbl.Name = "MissingGarLbl";
            this.MissingGarLbl.Size = new System.Drawing.Size(71, 17);
            this.MissingGarLbl.TabIndex = 1;
            this.MissingGarLbl.Text = "No. Cars";
            // 
            // GarLbl
            // 
            this.GarLbl.AutoSize = true;
            this.GarLbl.Location = new System.Drawing.Point(20, 130);
            this.GarLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GarLbl.Name = "GarLbl";
            this.GarLbl.Size = new System.Drawing.Size(64, 17);
            this.GarLbl.TabIndex = 2;
            this.GarLbl.Text = "Garage :";
            // 
            // CarPortLbl
            // 
            this.CarPortLbl.AutoSize = true;
            this.CarPortLbl.Location = new System.Drawing.Point(16, 199);
            this.CarPortLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CarPortLbl.Name = "CarPortLbl";
            this.CarPortLbl.Size = new System.Drawing.Size(68, 17);
            this.CarPortLbl.TabIndex = 3;
            this.CarPortLbl.Text = "Car Port :";
            // 
            // GarTypeCbox
            // 
            this.GarTypeCbox.FormattingEnabled = true;
            this.GarTypeCbox.Location = new System.Drawing.Point(112, 127);
            this.GarTypeCbox.Margin = new System.Windows.Forms.Padding(4);
            this.GarTypeCbox.Name = "GarTypeCbox";
            this.GarTypeCbox.Size = new System.Drawing.Size(160, 24);
            this.GarTypeCbox.TabIndex = 6;
            this.GarTypeCbox.SelectedIndexChanged += new System.EventHandler(this.GarTypeCbox_SelectedIndexChanged);
            // 
            // CarPortTypCbox
            // 
            this.CarPortTypCbox.FormattingEnabled = true;
            this.CarPortTypCbox.Location = new System.Drawing.Point(112, 196);
            this.CarPortTypCbox.Margin = new System.Windows.Forms.Padding(4);
            this.CarPortTypCbox.Name = "CarPortTypCbox";
            this.CarPortTypCbox.Size = new System.Drawing.Size(160, 24);
            this.CarPortTypCbox.TabIndex = 7;
            this.CarPortTypCbox.SelectedIndexChanged += new System.EventHandler(this.CarPortTypCbox_SelectedIndexChanged);
            // 
            // GarNbrCarTxt
            // 
            this.GarNbrCarTxt.Location = new System.Drawing.Point(353, 127);
            this.GarNbrCarTxt.Margin = new System.Windows.Forms.Padding(4);
            this.GarNbrCarTxt.Name = "GarNbrCarTxt";
            this.GarNbrCarTxt.Size = new System.Drawing.Size(32, 22);
            this.GarNbrCarTxt.TabIndex = 4;
            this.GarNbrCarTxt.TextChanged += new System.EventHandler(this.GarNbrCarTxt_TextChanged);
            this.GarNbrCarTxt.Leave += new System.EventHandler(this.GarNbrCarTxt_Leave);
            // 
            // CarPortNbrCarTxt
            // 
            this.CarPortNbrCarTxt.Location = new System.Drawing.Point(353, 196);
            this.CarPortNbrCarTxt.Margin = new System.Windows.Forms.Padding(4);
            this.CarPortNbrCarTxt.Name = "CarPortNbrCarTxt";
            this.CarPortNbrCarTxt.Size = new System.Drawing.Size(32, 22);
            this.CarPortNbrCarTxt.TabIndex = 5;
            this.CarPortNbrCarTxt.TextChanged += new System.EventHandler(this.CarPortNbrCarTxt_TextChanged);
            this.CarPortNbrCarTxt.Leave += new System.EventHandler(this.CarPortNbrCarTxt_Leave);
            // 
            // MissingCarporLbl
            // 
            this.MissingCarporLbl.AutoSize = true;
            this.MissingCarporLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissingCarporLbl.Location = new System.Drawing.Point(336, 167);
            this.MissingCarporLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MissingCarporLbl.Name = "MissingCarporLbl";
            this.MissingCarporLbl.Size = new System.Drawing.Size(71, 17);
            this.MissingCarporLbl.TabIndex = 8;
            this.MissingCarporLbl.Text = "No. Cars";
            // 
            // MissingGarageData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(508, 342);
            this.Controls.Add(this.MissingCarporLbl);
            this.Controls.Add(this.CarPortNbrCarTxt);
            this.Controls.Add(this.GarNbrCarTxt);
            this.Controls.Add(this.CarPortTypCbox);
            this.Controls.Add(this.GarTypeCbox);
            this.Controls.Add(this.CarPortLbl);
            this.Controls.Add(this.GarLbl);
            this.Controls.Add(this.MissingGarLbl);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MissingGarageData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Missing Garage Information";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MissingGarageData_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label MissingGarLbl;
        private System.Windows.Forms.Label GarLbl;
        private System.Windows.Forms.Label CarPortLbl;
        private System.Windows.Forms.ComboBox GarTypeCbox;
        private System.Windows.Forms.ComboBox CarPortTypCbox;
        private System.Windows.Forms.TextBox GarNbrCarTxt;
        private System.Windows.Forms.TextBox CarPortNbrCarTxt;
        private System.Windows.Forms.Label MissingCarporLbl;
    }
}