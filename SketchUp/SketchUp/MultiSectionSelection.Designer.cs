namespace SketchUp
{
    partial class MultiSectionSelection
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
			this.SecLetterCbox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(67, 38);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(238, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Multiple Section Letter List";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// SecLetterCbox
			// 
			this.SecLetterCbox.FormattingEnabled = true;
			this.SecLetterCbox.Location = new System.Drawing.Point(129, 139);
			this.SecLetterCbox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.SecLetterCbox.Name = "SecLetterCbox";
			this.SecLetterCbox.Size = new System.Drawing.Size(160, 24);
			this.SecLetterCbox.TabIndex = 1;
			this.SecLetterCbox.SelectedIndexChanged += new System.EventHandler(this.SecLetterCbox_SelectedIndexChanged);
			// 
			// MultiSectionSelection
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
			this.ClientSize = new System.Drawing.Size(379, 322);
			this.Controls.Add(this.SecLetterCbox);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MultiSectionSelection";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Multiple Section Selection";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MultiSectionSelection_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox SecLetterCbox;
    }
}