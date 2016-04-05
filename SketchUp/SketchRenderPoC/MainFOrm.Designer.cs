using System.Windows.Forms;

namespace SketchRenderPoC
{
	partial class MainForm :Form
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.MainMenu = new System.Windows.Forms.MenuStrip();
			this.beginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.StatusMain = new System.Windows.Forms.StatusStrip();
			this.MouseLocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.tsbGetSketch = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsb2DSketch = new System.Windows.Forms.ToolStripButton();
			this.pctMain = new System.Windows.Forms.PictureBox();
			this.MainMenu.SuspendLayout();
			this.StatusMain.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctMain)).BeginInit();
			this.SuspendLayout();
			// 
			// MainMenu
			// 
			this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beginToolStripMenuItem});
			this.MainMenu.Location = new System.Drawing.Point(0, 0);
			this.MainMenu.Name = "MainMenu";
			this.MainMenu.Size = new System.Drawing.Size(1427, 28);
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
			// StatusMain
			// 
			this.StatusMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.StatusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MouseLocationLabel});
			this.StatusMain.Location = new System.Drawing.Point(0, 718);
			this.StatusMain.Name = "StatusMain";
			this.StatusMain.Size = new System.Drawing.Size(1427, 22);
			this.StatusMain.TabIndex = 2;
			this.StatusMain.Text = "Status";
			// 
			// MouseLocationLabel
			// 
			this.MouseLocationLabel.Name = "MouseLocationLabel";
			this.MouseLocationLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbGetSketch,
            this.toolStripSeparator1,
            this.tsb2DSketch});
			this.toolStrip1.Location = new System.Drawing.Point(0, 28);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(1427, 27);
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
			// tsb2DSketch
			// 
			this.tsb2DSketch.Image = global::SketchRenderPoC.Properties.Resources.Font_16x;
			this.tsb2DSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb2DSketch.Name = "tsb2DSketch";
			this.tsb2DSketch.Size = new System.Drawing.Size(107, 24);
			this.tsb2DSketch.Text = "CheckFonts";
			// 
			// pctMain
			// 
			this.pctMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.pctMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pctMain.Location = new System.Drawing.Point(0, 58);
			this.pctMain.Name = "pctMain";
			this.pctMain.Size = new System.Drawing.Size(1415, 657);
			this.pctMain.TabIndex = 4;
			this.pctMain.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
			this.ClientSize = new System.Drawing.Size(1427, 740);
			this.Controls.Add(this.pctMain);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.StatusMain);
			this.Controls.Add(this.MainMenu);
			this.MainMenuStrip = this.MainMenu;
			this.Name = "MainForm";
			this.Text = "Render Initial Sketch";
			this.MainMenu.ResumeLayout(false);
			this.MainMenu.PerformLayout();
			this.StatusMain.ResumeLayout(false);
			this.StatusMain.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pctMain)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip MainMenu;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.StatusStrip StatusMain;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton tsbGetSketch;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsb2DSketch;
		private System.Windows.Forms.ToolStripMenuItem beginToolStripMenuItem;
		private System.Windows.Forms.PictureBox pctMain;
		private System.Windows.Forms.ToolStripStatusLabel MouseLocationLabel;
	}
}

