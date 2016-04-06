namespace SketchRenderPoC
{
	partial class MainForm
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
			this.pctMain = new System.Windows.Forms.PictureBox();
			this.wholeSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sectionFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
			this.beginToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wholeSketchToolStripMenuItem,
            this.sectionAToolStripMenuItem,
            this.sectionBToolStripMenuItem,
            this.sectionCToolStripMenuItem,
            this.sectionDToolStripMenuItem,
            this.sectionFToolStripMenuItem});
			this.beginToolStripMenuItem.Name = "beginToolStripMenuItem";
			this.beginToolStripMenuItem.Size = new System.Drawing.Size(56, 24);
			this.beginToolStripMenuItem.Text = "Draw";
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
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
            this.toolStripSeparator1});
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
			// pctMain
			// 
			this.pctMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.pctMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pctMain.Location = new System.Drawing.Point(0, 58);
			this.pctMain.Name = "pctMain";
			this.pctMain.Size = new System.Drawing.Size(1415, 657);
			this.pctMain.TabIndex = 4;
			this.pctMain.TabStop = false;
			this.pctMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pctMain_MouseDoubleClick);
			this.pctMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctMain_MouseMove);
			// 
			// wholeSketchToolStripMenuItem
			// 
			this.wholeSketchToolStripMenuItem.Name = "wholeSketchToolStripMenuItem";
			this.wholeSketchToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.wholeSketchToolStripMenuItem.Text = "Whole Sketch";
			// 
			// sectionAToolStripMenuItem
			// 
			this.sectionAToolStripMenuItem.Name = "sectionAToolStripMenuItem";
			this.sectionAToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.sectionAToolStripMenuItem.Text = "Section A";
			this.sectionAToolStripMenuItem.Click += new System.EventHandler(this.sectionAToolStripMenuItem_Click);
			// 
			// sectionBToolStripMenuItem
			// 
			this.sectionBToolStripMenuItem.Name = "sectionBToolStripMenuItem";
			this.sectionBToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.sectionBToolStripMenuItem.Text = "Section B";
			this.sectionBToolStripMenuItem.Click += new System.EventHandler(this.sectionBToolStripMenuItem_Click);
			// 
			// sectionCToolStripMenuItem
			// 
			this.sectionCToolStripMenuItem.Name = "sectionCToolStripMenuItem";
			this.sectionCToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.sectionCToolStripMenuItem.Text = "Section C";
			this.sectionCToolStripMenuItem.Click += new System.EventHandler(this.sectionCToolStripMenuItem_Click);
			// 
			// sectionDToolStripMenuItem
			// 
			this.sectionDToolStripMenuItem.Name = "sectionDToolStripMenuItem";
			this.sectionDToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.sectionDToolStripMenuItem.Text = "Section D";
			this.sectionDToolStripMenuItem.Click += new System.EventHandler(this.sectionDToolStripMenuItem_Click);
			// 
			// sectionFToolStripMenuItem
			// 
			this.sectionFToolStripMenuItem.Name = "sectionFToolStripMenuItem";
			this.sectionFToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
			this.sectionFToolStripMenuItem.Text = "Section F";
			this.sectionFToolStripMenuItem.Click += new System.EventHandler(this.sectionFToolStripMenuItem_Click);
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
		private System.Windows.Forms.ToolStripMenuItem beginToolStripMenuItem;
		private System.Windows.Forms.PictureBox pctMain;
		private System.Windows.Forms.ToolStripStatusLabel MouseLocationLabel;
		private System.Windows.Forms.ToolStripMenuItem wholeSketchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionBToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionDToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sectionFToolStripMenuItem;
	}
}

