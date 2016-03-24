namespace SketchRenderPoC
{
	partial class SketchForm1
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SketchForm1));
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.beginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbGetSketch = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsbEditSketch = new System.Windows.Forms.ToolStripButton();
			this.pctMainSketch = new System.Windows.Forms.PictureBox();
			this.MainMenu.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctMainSketch)).BeginInit();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beginToolStripMenuItem});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(976, 28);
			this.MainMenu.TabIndex = 0;
			this.MainMenu.Text = "Menu";
			// 
			// beginToolStripMenuItem
			// 
			this.beginToolStripMenuItem.Name = "beginToolStripMenuItem";
			this.beginToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
			this.beginToolStripMenuItem.Text = "Begin";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(67, 4);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Location = new System.Drawing.Point(0, 700);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(976, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbGetSketch,
            this.toolStripSeparator1,
            this.tsbEditSketch});
			this.toolStrip1.Location = new System.Drawing.Point(0, 28);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(976, 27);
			this.toolStrip1.TabIndex = 3;
			this.toolStrip1.Text = "Tests";
			// 
			// tsbGetSketch
			// 
			this.tsbGetSketch.Image = ((System.Drawing.Image)(resources.GetObject("tsbGetSketch.Image")));
			this.tsbGetSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbGetSketch.Name = "tsbGetSketch";
			this.tsbGetSketch.Size = new System.Drawing.Size(103, 24);
			this.tsbGetSketch.Text = "Get Sketch";
			this.tsbGetSketch.Click += new System.EventHandler(this.tsbGetSketch_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
			// 
			// tsbEditSketch
			// 
			this.tsbEditSketch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbEditSketch.Image = ((System.Drawing.Image)(resources.GetObject("tsbEditSketch.Image")));
			this.tsbEditSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsbEditSketch.Name = "tsbEditSketch";
			this.tsbEditSketch.Size = new System.Drawing.Size(24, 24);
			this.tsbEditSketch.Text = "Edit Sketch";
			// 
			// pctMainSketch
			// 
			this.pctMainSketch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pctMainSketch.Location = new System.Drawing.Point(268, 125);
			this.pctMainSketch.Name = "pctMainSketch";
			this.pctMainSketch.Size = new System.Drawing.Size(400, 400);
			this.pctMainSketch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pctMainSketch.TabIndex = 4;
			this.pctMainSketch.TabStop = false;
			// 
			// SketchForm1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(976, 722);
			this.Controls.Add(this.pctMainSketch);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.MainMenu);
			this.MainMenuStrip = this.MainMenu;
			this.Name = "SketchForm1";
			this.Text = "Render Initial Sketch";
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctMainSketch)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbGetSketch;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbEditSketch;
		private System.Windows.Forms.ToolStripMenuItem beginToolStripMenuItem;
		private System.Windows.Forms.PictureBox pctMainSketch;
	}
}

