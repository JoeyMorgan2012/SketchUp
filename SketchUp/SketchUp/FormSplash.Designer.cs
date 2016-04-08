namespace SketchUp
{
    partial class FormSplash
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSplash));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.loadingProgBar = new System.Windows.Forms.ProgressBar();
			this.regLabel = new System.Windows.Forms.Label();
			this.logoPicture = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.LightSteelBlue;
			this.label1.CausesValidation = false;
			this.label1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.MediumBlue;
			this.label1.Location = new System.Drawing.Point(175, 182);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(228, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "Sketch Loading ...";
			this.label1.UseWaitCursor = true;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.LightSteelBlue;
			this.label2.CausesValidation = false;
			this.label2.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.label2.Font = new System.Drawing.Font("MS Reference Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.MediumBlue;
			this.label2.Location = new System.Drawing.Point(155, 120);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(228, 49);
			this.label2.TabIndex = 0;
			this.label2.Text = "SketchUp";
			this.label2.UseWaitCursor = true;
			// 
			// loadingProgBar
			// 
			this.loadingProgBar.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.loadingProgBar.ForeColor = System.Drawing.Color.Teal;
			this.loadingProgBar.Location = new System.Drawing.Point(2, 239);
			this.loadingProgBar.Name = "loadingProgBar";
			this.loadingProgBar.Size = new System.Drawing.Size(503, 23);
			this.loadingProgBar.TabIndex = 3;
			this.loadingProgBar.UseWaitCursor = true;
			// 
			// regLabel
			// 
			this.regLabel.AutoSize = true;
			this.regLabel.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.regLabel.ForeColor = System.Drawing.Color.MediumBlue;
			this.regLabel.Location = new System.Drawing.Point(353, 120);
			this.regLabel.Name = "regLabel";
			this.regLabel.Size = new System.Drawing.Size(22, 18);
			this.regLabel.TabIndex = 1;
			this.regLabel.Text = "®";
			this.regLabel.UseWaitCursor = true;
			// 
			// logoPicture
			// 
			this.logoPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.logoPicture.Dock = System.Windows.Forms.DockStyle.Top;
			//this.logoPicture.Image = global::SketchUp.Properties.Resources.STONE_TECH_LOGO;
			this.logoPicture.Location = new System.Drawing.Point(2, 2);
			this.logoPicture.Name = "logoPicture";
			this.logoPicture.Size = new System.Drawing.Size(503, 96);
			this.logoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.logoPicture.TabIndex = 3;
			this.logoPicture.TabStop = false;
			this.logoPicture.UseWaitCursor = true;
			// 
			// FormSplash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.CausesValidation = false;
			this.ClientSize = new System.Drawing.Size(507, 264);
			this.ControlBox = false;
			this.Controls.Add(this.regLabel);
			this.Controls.Add(this.logoPicture);
			this.Controls.Add(this.loadingProgBar);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormSplash";
			this.Padding = new System.Windows.Forms.Padding(2);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.UseWaitCursor = true;
			this.Load += new System.EventHandler(this.FormSplash_Load);
			((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox logoPicture;
		private System.Windows.Forms.Label regLabel;
		protected internal System.Windows.Forms.ProgressBar loadingProgBar;
	}
}