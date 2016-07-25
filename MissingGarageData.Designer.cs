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
            this.Garage1Label = new System.Windows.Forms.Label();
            this.CarPortLbl = new System.Windows.Forms.Label();
            this.Gar1TypeCbox = new System.Windows.Forms.ComboBox();
            this.CarPortTypCbox = new System.Windows.Forms.ComboBox();
            this.Gar1NbrCarTxt = new System.Windows.Forms.TextBox();
            this.CarPortNbrCarTxt = new System.Windows.Forms.TextBox();
            this.MissingCarporLbl = new System.Windows.Forms.Label();
            this.Gar2TypeCbox = new System.Windows.Forms.ComboBox();
            this.Garage2Label = new System.Windows.Forms.Label();
            this.Gar2NbrCarTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDone = new System.Windows.Forms.Button();
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
            // Garage1Label
            // 
            this.Garage1Label.AutoSize = true;
            this.Garage1Label.Location = new System.Drawing.Point(20, 130);
            this.Garage1Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Garage1Label.Name = "Garage1Label";
            this.Garage1Label.Size = new System.Drawing.Size(76, 17);
            this.Garage1Label.TabIndex = 2;
            this.Garage1Label.Text = "Garage  1:";
            // 
            // CarPortLbl
            // 
            this.CarPortLbl.AutoSize = true;
            this.CarPortLbl.Location = new System.Drawing.Point(16, 242);
            this.CarPortLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CarPortLbl.Name = "CarPortLbl";
            this.CarPortLbl.Size = new System.Drawing.Size(68, 17);
            this.CarPortLbl.TabIndex = 10;
            this.CarPortLbl.Text = "Car Port :";
            // 
            // Gar1TypeCbox
            // 
            this.Gar1TypeCbox.DisplayMember = "Description ";
            this.Gar1TypeCbox.FormattingEnabled = true;
            this.Gar1TypeCbox.Location = new System.Drawing.Point(112, 127);
            this.Gar1TypeCbox.Margin = new System.Windows.Forms.Padding(4);
            this.Gar1TypeCbox.Name = "Gar1TypeCbox";
            this.Gar1TypeCbox.Size = new System.Drawing.Size(160, 24);
            this.Gar1TypeCbox.TabIndex = 3;
            this.Gar1TypeCbox.ValueMember = "Code";
            this.Gar1TypeCbox.SelectedIndexChanged += new System.EventHandler(this.Gar1TypeCbox_SelectedIndexChanged);
            // 
            // CarPortTypCbox
            // 
            this.CarPortTypCbox.DisplayMember = "Description";
            this.CarPortTypCbox.FormattingEnabled = true;
            this.CarPortTypCbox.Location = new System.Drawing.Point(112, 239);
            this.CarPortTypCbox.Margin = new System.Windows.Forms.Padding(4);
            this.CarPortTypCbox.Name = "CarPortTypCbox";
            this.CarPortTypCbox.Size = new System.Drawing.Size(160, 24);
            this.CarPortTypCbox.TabIndex = 11;
            this.CarPortTypCbox.ValueMember = "Code";
            this.CarPortTypCbox.SelectedIndexChanged += new System.EventHandler(this.CarPortTypCbox_SelectedIndexChanged);
            // 
            // Gar1NbrCarTxt
            // 
            this.Gar1NbrCarTxt.Location = new System.Drawing.Point(353, 127);
            this.Gar1NbrCarTxt.Margin = new System.Windows.Forms.Padding(4);
            this.Gar1NbrCarTxt.Name = "Gar1NbrCarTxt";
            this.Gar1NbrCarTxt.Size = new System.Drawing.Size(32, 22);
            this.Gar1NbrCarTxt.TabIndex = 4;
            this.Gar1NbrCarTxt.Leave += new System.EventHandler(this.Gar1NbrCarTxt_Leave);
            // 
            // CarPortNbrCarTxt
            // 
            this.CarPortNbrCarTxt.Location = new System.Drawing.Point(353, 241);
            this.CarPortNbrCarTxt.Margin = new System.Windows.Forms.Padding(4);
            this.CarPortNbrCarTxt.Name = "CarPortNbrCarTxt";
            this.CarPortNbrCarTxt.Size = new System.Drawing.Size(32, 22);
            this.CarPortNbrCarTxt.TabIndex = 12;
            this.CarPortNbrCarTxt.Leave += new System.EventHandler(this.CarPortNbrCarTxt_Leave);
            // 
            // MissingCarporLbl
            // 
            this.MissingCarporLbl.AutoSize = true;
            this.MissingCarporLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MissingCarporLbl.Location = new System.Drawing.Point(336, 212);
            this.MissingCarporLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MissingCarporLbl.Name = "MissingCarporLbl";
            this.MissingCarporLbl.Size = new System.Drawing.Size(71, 17);
            this.MissingCarporLbl.TabIndex = 9;
            this.MissingCarporLbl.Text = "No. Cars";
            // 
            // Gar2TypeCbox
            // 
            this.Gar2TypeCbox.DisplayMember = "Description";
            this.Gar2TypeCbox.FormattingEnabled = true;
            this.Gar2TypeCbox.Location = new System.Drawing.Point(112, 182);
            this.Gar2TypeCbox.Margin = new System.Windows.Forms.Padding(4);
            this.Gar2TypeCbox.Name = "Gar2TypeCbox";
            this.Gar2TypeCbox.Size = new System.Drawing.Size(160, 24);
            this.Gar2TypeCbox.TabIndex = 7;
            this.Gar2TypeCbox.ValueMember = "Code";
            this.Gar2TypeCbox.SelectedIndexChanged += new System.EventHandler(this.Gar2TypeCbox_SelectedIndexChanged);
            // 
            // Garage2Label
            // 
            this.Garage2Label.AutoSize = true;
            this.Garage2Label.Location = new System.Drawing.Point(20, 185);
            this.Garage2Label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Garage2Label.Name = "Garage2Label";
            this.Garage2Label.Size = new System.Drawing.Size(76, 17);
            this.Garage2Label.TabIndex = 6;
            this.Garage2Label.Text = "Garage 2 :";
            // 
            // Gar2NbrCarTxt
            // 
            this.Gar2NbrCarTxt.Location = new System.Drawing.Point(353, 184);
            this.Gar2NbrCarTxt.Margin = new System.Windows.Forms.Padding(4);
            this.Gar2NbrCarTxt.Name = "Gar2NbrCarTxt";
            this.Gar2NbrCarTxt.Size = new System.Drawing.Size(32, 22);
            this.Gar2NbrCarTxt.TabIndex = 8;
            this.Gar2NbrCarTxt.Leave += new System.EventHandler(this.Gar2NbrCarTxt_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(336, 153);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "No. Cars";
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(339, 285);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(75, 32);
            this.btnDone.TabIndex = 13;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // MissingGarageData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(508, 342);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.Gar2NbrCarTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Gar2TypeCbox);
            this.Controls.Add(this.Garage2Label);
            this.Controls.Add(this.MissingCarporLbl);
            this.Controls.Add(this.CarPortNbrCarTxt);
            this.Controls.Add(this.Gar1NbrCarTxt);
            this.Controls.Add(this.CarPortTypCbox);
            this.Controls.Add(this.Gar1TypeCbox);
            this.Controls.Add(this.CarPortLbl);
            this.Controls.Add(this.Garage1Label);
            this.Controls.Add(this.MissingGarLbl);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MissingGarageData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Missing Garage Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label MissingGarLbl;
        private System.Windows.Forms.Label Garage1Label;
        private System.Windows.Forms.Label CarPortLbl;
        private System.Windows.Forms.ComboBox Gar1TypeCbox;
        private System.Windows.Forms.ComboBox CarPortTypCbox;
        private System.Windows.Forms.TextBox Gar1NbrCarTxt;
        private System.Windows.Forms.TextBox CarPortNbrCarTxt;
        private System.Windows.Forms.Label MissingCarporLbl;
        private System.Windows.Forms.ComboBox Gar2TypeCbox;
        private System.Windows.Forms.Label Garage2Label;
        private System.Windows.Forms.TextBox Gar2NbrCarTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDone;
    }
}