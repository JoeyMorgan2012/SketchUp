namespace SketchUp
{
    partial class SketchDistance
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
            this.EWDirectTxt = new System.Windows.Forms.TextBox();
            this.EWDistanceTxt = new System.Windows.Forms.TextBox();
            this.NSDirectTxt = new System.Windows.Forms.TextBox();
            this.NSDistanceTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Distance Measurement";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(45, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "E/W Dist";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(156, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "N/S Dist";
            // 
            // EWDirectTxt
            // 
            this.EWDirectTxt.BackColor = System.Drawing.SystemColors.Control;
            this.EWDirectTxt.Location = new System.Drawing.Point(40, 62);
            this.EWDirectTxt.Name = "EWDirectTxt";
            this.EWDirectTxt.Size = new System.Drawing.Size(60, 20);
            this.EWDirectTxt.TabIndex = 3;
            // 
            // EWDistanceTxt
            // 
            this.EWDistanceTxt.BackColor = System.Drawing.SystemColors.Control;
            this.EWDistanceTxt.Location = new System.Drawing.Point(40, 90);
            this.EWDistanceTxt.Name = "EWDistanceTxt";
            this.EWDistanceTxt.Size = new System.Drawing.Size(60, 20);
            this.EWDistanceTxt.TabIndex = 5;
            // 
            // NSDirectTxt
            // 
            this.NSDirectTxt.BackColor = System.Drawing.SystemColors.Control;
            this.NSDirectTxt.Location = new System.Drawing.Point(150, 62);
            this.NSDirectTxt.Name = "NSDirectTxt";
            this.NSDirectTxt.Size = new System.Drawing.Size(60, 20);
            this.NSDirectTxt.TabIndex = 4;
            // 
            // NSDistanceTxt
            // 
            this.NSDistanceTxt.BackColor = System.Drawing.SystemColors.Control;
            this.NSDistanceTxt.Location = new System.Drawing.Point(150, 91);
            this.NSDistanceTxt.Name = "NSDistanceTxt";
            this.NSDistanceTxt.Size = new System.Drawing.Size(60, 20);
            this.NSDistanceTxt.TabIndex = 6;
            // 
            // SketchDistance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(247, 119);
            this.Controls.Add(this.NSDistanceTxt);
            this.Controls.Add(this.NSDirectTxt);
            this.Controls.Add(this.EWDistanceTxt);
            this.Controls.Add(this.EWDirectTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SketchDistance";
            this.Text = "Sketch Line Distance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EWDirectTxt;
        private System.Windows.Forms.TextBox EWDistanceTxt;
        private System.Windows.Forms.TextBox NSDirectTxt;
        private System.Windows.Forms.TextBox NSDistanceTxt;
    }
}