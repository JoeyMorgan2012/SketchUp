namespace SWallTech
{
	partial class AddSectionForm
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
			this.topMenu = new System.Windows.Forms.MenuStrip();
			this.sectionType = new System.Windows.Forms.ToolStripComboBox();
			this.distanceText = new System.Windows.Forms.ToolStripTextBox();
			this.topMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// topMenu
			// 
			this.topMenu.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.topMenu.Dock = System.Windows.Forms.DockStyle.None;
			this.topMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sectionType,
            this.distanceText});
			this.topMenu.Location = new System.Drawing.Point(20, 38);
			this.topMenu.Name = "topMenu";
			this.topMenu.Size = new System.Drawing.Size(348, 32);
			this.topMenu.TabIndex = 1;
			this.topMenu.Text = "New Section";
			// 
			// sectionType
			// 
			this.sectionType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.sectionType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.sectionType.Name = "sectionType";
			this.sectionType.Size = new System.Drawing.Size(121, 28);
			// 
			// distanceText
			// 
			this.distanceText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.distanceText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.distanceText.MergeAction = System.Windows.Forms.MergeAction.Replace;
			this.distanceText.Name = "distanceText";
			this.distanceText.Padding = new System.Windows.Forms.Padding(1);
			this.distanceText.Size = new System.Drawing.Size(100, 28);
			// 
			// AddSectionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1160, 764);
			this.Controls.Add(this.topMenu);
			this.MainMenuStrip = this.topMenu;
			this.Name = "AddSectionForm";
			this.Text = "AddSectionForm";
			this.topMenu.ResumeLayout(false);
			this.topMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip topMenu;
		private System.Windows.Forms.ToolStripComboBox sectionType;
		private System.Windows.Forms.ToolStripTextBox distanceText;
	}
}