namespace CamraSketch
{
    partial class TestOutPut
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
            this.LibraryNameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.FilePrefixTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RecordNoTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CardNoTxt = new System.Windows.Forms.TextBox();
            this.NameTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LibraryNameTxt
            // 
            this.LibraryNameTxt.Location = new System.Drawing.Point(130, 29);
            this.LibraryNameTxt.Name = "LibraryNameTxt";
            this.LibraryNameTxt.Size = new System.Drawing.Size(100, 20);
            this.LibraryNameTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(53, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Library :";
            // 
            // FilePrefixTxt
            // 
            this.FilePrefixTxt.Location = new System.Drawing.Point(130, 76);
            this.FilePrefixTxt.Name = "FilePrefixTxt";
            this.FilePrefixTxt.Size = new System.Drawing.Size(100, 20);
            this.FilePrefixTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "File Prefix :";
            // 
            // RecordNoTxt
            // 
            this.RecordNoTxt.Location = new System.Drawing.Point(381, 29);
            this.RecordNoTxt.Name = "RecordNoTxt";
            this.RecordNoTxt.Size = new System.Drawing.Size(52, 20);
            this.RecordNoTxt.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(305, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Record No :";
            // 
            // CardNoTxt
            // 
            this.CardNoTxt.Location = new System.Drawing.Point(470, 29);
            this.CardNoTxt.Name = "CardNoTxt";
            this.CardNoTxt.Size = new System.Drawing.Size(52, 20);
            this.CardNoTxt.TabIndex = 6;
            // 
            // NameTxt
            // 
            this.NameTxt.Location = new System.Drawing.Point(130, 150);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(289, 20);
            this.NameTxt.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(53, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Name :";
            // 
            // TestOutPut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(554, 425);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NameTxt);
            this.Controls.Add(this.CardNoTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.RecordNoTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FilePrefixTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LibraryNameTxt);
            this.Name = "TestOutPut";
            this.Text = "Parcel Out Put Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LibraryNameTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FilePrefixTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox RecordNoTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox CardNoTxt;
        private System.Windows.Forms.TextBox NameTxt;
        private System.Windows.Forms.Label label4;
    }
}