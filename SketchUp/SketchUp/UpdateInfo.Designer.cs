namespace SketchUp
{
	partial class UpdateInfo
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
			this.lblUpdate = new System.Windows.Forms.Label();
			this.buttonOk = new System.Windows.Forms.Button();
			this.rtbVersionInfo = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// lblUpdate
			// 
			this.lblUpdate.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblUpdate.Font = new System.Drawing.Font("MS Outlook", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblUpdate.Location = new System.Drawing.Point(5, 35);
			this.lblUpdate.Name = "lblUpdate";
			this.lblUpdate.Padding = new System.Windows.Forms.Padding(16);
			this.lblUpdate.Size = new System.Drawing.Size(370, 76);
			this.lblUpdate.TabIndex = 0;
			this.lblUpdate.Text = "Update Check Completed";
			this.lblUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// buttonOk
			// 
			this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOk.Location = new System.Drawing.Point(143, 291);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(74, 40);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "Ok";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// rtbVersionINfo
			// 
			this.rtbVersionInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.rtbVersionInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtbVersionInfo.Location = new System.Drawing.Point(38, 129);
			this.rtbVersionInfo.Name = "rtbVersionINfo";
			this.rtbVersionInfo.ReadOnly = true;
			this.rtbVersionInfo.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.rtbVersionInfo.Size = new System.Drawing.Size(313, 130);
			this.rtbVersionInfo.TabIndex = 1;
			this.rtbVersionInfo.Text = "Current Version:\n\n";
			// 
			// UpdateInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(382, 355);
			this.Controls.Add(this.rtbVersionInfo);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.lblUpdate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "UpdateInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CAMRA Sketch Update Check";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblUpdate;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.RichTextBox rtbVersionInfo;
	}
}