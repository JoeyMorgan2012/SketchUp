namespace SketchUp
{
    partial class ExpandoSketch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpandoSketch));
            this.ExpandoSketchTools = new System.Windows.Forms.ToolStrip();
            this.AddSectionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BeginSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.DistLbl = new System.Windows.Forms.ToolStripLabel();
            this.DistText = new System.Windows.Forms.ToolStripTextBox();
            this.TextBtn = new System.Windows.Forms.ToolStripButton();
            this.TextLbl = new System.Windows.Forms.ToolStripLabel();
            this.FieldText = new System.Windows.Forms.ToolStripTextBox();
            this.UnDoBtn = new System.Windows.Forms.ToolStripButton();
            this.AddSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.DrawingDoneBtn = new System.Windows.Forms.ToolStripButton();
            this.tsbExitSketch = new System.Windows.Forms.ToolStripButton();
            this.dgSections = new System.Windows.Forms.DataGridView();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.ExpSketchPBox = new System.Windows.Forms.PictureBox();
            this.SketchStatusBar = new System.Windows.Forms.StatusStrip();
            this.sketchStatusMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExpandoSketchTools.SuspendLayout();
            this.AddSectionContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).BeginInit();
            this.SketchStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ExpandoSketchTools
            // 
            this.ExpandoSketchTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ExpandoSketchTools.ContextMenuStrip = this.AddSectionContextMenu;
            this.ExpandoSketchTools.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ExpandoSketchTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BeginSectionBtn,
            this.DistLbl,
            this.DistText,
            this.TextBtn,
            this.TextLbl,
            this.FieldText,
            this.UnDoBtn,
            this.AddSectionBtn,
            this.DrawingDoneBtn,
            this.tsbExitSketch});
            this.ExpandoSketchTools.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.ExpandoSketchTools.Location = new System.Drawing.Point(0, 0);
            this.ExpandoSketchTools.Name = "ExpandoSketchTools";
            this.ExpandoSketchTools.Size = new System.Drawing.Size(1014, 54);
            this.ExpandoSketchTools.TabIndex = 0;
            this.ExpandoSketchTools.Text = "Sketch Tools";
            // 
            // AddSectionContextMenu
            // 
            this.AddSectionContextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.AddSectionContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.AddSectionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToolStripMenuItem});
            this.AddSectionContextMenu.Name = "contextMenuStrip1";
            this.AddSectionContextMenu.Size = new System.Drawing.Size(230, 30);
            this.AddSectionContextMenu.Click += new System.EventHandler(this.beginPointToolStripMenuItem_Click);
            // 
            // jumpToolStripMenuItem
            // 
            this.jumpToolStripMenuItem.AutoToolTip = true;
            this.jumpToolStripMenuItem.Enabled = false;
            this.jumpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jumpToolStripMenuItem.Image")));
            this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
            this.jumpToolStripMenuItem.ShowShortcutKeys = false;
            this.jumpToolStripMenuItem.Size = new System.Drawing.Size(231, 26);
            this.jumpToolStripMenuItem.Text = "Jump to Nearest Corner";
            this.jumpToolStripMenuItem.ToolTipText = "Jump to Nearest Corner (Choose Section Type to Enable";
            this.jumpToolStripMenuItem.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // BeginSectionBtn
            // 
            this.BeginSectionBtn.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BeginSectionBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.BeginSectionBtn.Enabled = false;
            this.BeginSectionBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.BeginSectionBtn.Image = ((System.Drawing.Image)(resources.GetObject("BeginSectionBtn.Image")));
            this.BeginSectionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BeginSectionBtn.Margin = new System.Windows.Forms.Padding(25, 1, 15, 2);
            this.BeginSectionBtn.Name = "BeginSectionBtn";
            this.BeginSectionBtn.Size = new System.Drawing.Size(108, 24);
            this.BeginSectionBtn.Text = "Begin Section";
            this.BeginSectionBtn.ToolTipText = "Begin Drawing Section";
            this.BeginSectionBtn.Click += new System.EventHandler(this.BeginSectionBtn_Click);
            // 
            // DistLbl
            // 
            this.DistLbl.Name = "DistLbl";
            this.DistLbl.Size = new System.Drawing.Size(66, 20);
            this.DistLbl.Text = "Distance";
            // 
            // DistText
            // 
            this.DistText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DistText.Margin = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.DistText.Name = "DistText";
            this.DistText.Size = new System.Drawing.Size(75, 27);
            this.DistText.Leave += new System.EventHandler(this.DistText_Leave);
            this.DistText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DistText_KeyDown);
            this.DistText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DistText_KeyPress);
            this.DistText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DistText_KeyUp);
            // 
            // TextBtn
            // 
            this.TextBtn.AutoSize = false;
            this.TextBtn.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.TextBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TextBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBtn.Image = ((System.Drawing.Image)(resources.GetObject("TextBtn.Image")));
            this.TextBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TextBtn.Margin = new System.Windows.Forms.Padding(10, 1, 10, 2);
            this.TextBtn.Name = "TextBtn";
            this.TextBtn.Size = new System.Drawing.Size(50, 23);
            this.TextBtn.Text = "Text";
            this.TextBtn.Visible = false;
            this.TextBtn.Click += new System.EventHandler(this.TextBtn_Click);
            // 
            // TextLbl
            // 
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(88, 20);
            this.TextLbl.Text = "Description:";
            // 
            // FieldText
            // 
            this.FieldText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FieldText.Margin = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.FieldText.Name = "FieldText";
            this.FieldText.Size = new System.Drawing.Size(150, 27);
            // 
            // UnDoBtn
            // 
            this.UnDoBtn.AutoSize = false;
            this.UnDoBtn.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.UnDoBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.UnDoBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnDoBtn.Image = ((System.Drawing.Image)(resources.GetObject("UnDoBtn.Image")));
            this.UnDoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UnDoBtn.Margin = new System.Windows.Forms.Padding(10, 1, 15, 2);
            this.UnDoBtn.Name = "UnDoBtn";
            this.UnDoBtn.Size = new System.Drawing.Size(60, 23);
            this.UnDoBtn.Text = "UnDo";
            this.UnDoBtn.Click += new System.EventHandler(this.UnDoBtn_Click);
            // 
            // AddSectionBtn
            // 
            this.AddSectionBtn.AutoSize = false;
            this.AddSectionBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.AddSectionBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AddSectionBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.AddSectionBtn.Image = ((System.Drawing.Image)(resources.GetObject("AddSectionBtn.Image")));
            this.AddSectionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddSectionBtn.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.AddSectionBtn.Name = "AddSectionBtn";
            this.AddSectionBtn.Size = new System.Drawing.Size(60, 23);
            this.AddSectionBtn.Text = "Add";
            this.AddSectionBtn.Click += new System.EventHandler(this.AddSectionBtn_Click);
            // 
            // DrawingDoneBtn
            // 
            this.DrawingDoneBtn.BackColor = System.Drawing.Color.Cyan;
            this.DrawingDoneBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DrawingDoneBtn.Enabled = false;
            this.DrawingDoneBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.DrawingDoneBtn.Image = ((System.Drawing.Image)(resources.GetObject("DrawingDoneBtn.Image")));
            this.DrawingDoneBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawingDoneBtn.Margin = new System.Windows.Forms.Padding(0, 1, 15, 2);
            this.DrawingDoneBtn.Name = "DrawingDoneBtn";
            this.DrawingDoneBtn.Size = new System.Drawing.Size(135, 24);
            this.DrawingDoneBtn.Text = "<Done Drawing>";
            this.DrawingDoneBtn.ToolTipText = "Click when you have added all lines in the section.";
            this.DrawingDoneBtn.Click += new System.EventHandler(this.DoneDrawingBtn_Click);
            // 
            // tsbExitSketch
            // 
            this.tsbExitSketch.Image = global::SketchUp.Properties.Resources.icon_home;
            this.tsbExitSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExitSketch.Name = "tsbExitSketch";
            this.tsbExitSketch.Size = new System.Drawing.Size(163, 24);
            this.tsbExitSketch.Text = "Exit Sketch Window";
            this.tsbExitSketch.ToolTipText = "Return to Main Window";
            this.tsbExitSketch.Click += new System.EventHandler(this.tsbExitSketch_Click);
            // 
            // dgSections
            // 
            this.dgSections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgSections.Location = new System.Drawing.Point(169, 112);
            this.dgSections.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgSections.Name = "dgSections";
            this.dgSections.Size = new System.Drawing.Size(280, 173);
            this.dgSections.TabIndex = 1;
            this.dgSections.Visible = false;
            // 
            // ExpSketchPBox
            // 
            this.ExpSketchPBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ExpSketchPBox.ContextMenuStrip = this.AddSectionContextMenu;
            this.ExpSketchPBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpSketchPBox.Location = new System.Drawing.Point(0, 54);
            this.ExpSketchPBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExpSketchPBox.Name = "ExpSketchPBox";
            this.ExpSketchPBox.Size = new System.Drawing.Size(1014, 545);
            this.ExpSketchPBox.TabIndex = 2;
            this.ExpSketchPBox.TabStop = false;
            this.ExpSketchPBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseClick);
            this.ExpSketchPBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseDown);
            this.ExpSketchPBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseUp);
            // 
            // SketchStatusBar
            // 
            this.SketchStatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SketchStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sketchStatusMain,
            this.statusLabel});
            this.SketchStatusBar.Location = new System.Drawing.Point(0, 577);
            this.SketchStatusBar.Name = "SketchStatusBar";
            this.SketchStatusBar.Size = new System.Drawing.Size(1014, 22);
            this.SketchStatusBar.TabIndex = 3;
            // 
            // sketchStatusMain
            // 
            this.sketchStatusMain.Name = "sketchStatusMain";
            this.sketchStatusMain.Size = new System.Drawing.Size(0, 19);
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // ExpandoSketch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1014, 599);
            this.Controls.Add(this.SketchStatusBar);
            this.Controls.Add(this.dgSections);
            this.Controls.Add(this.ExpSketchPBox);
            this.Controls.Add(this.ExpandoSketchTools);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ExpandoSketch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Sketch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExpandoSketch_FormClosing);
            this.Shown += new System.EventHandler(this.ExpandoSketch_Shown);
            this.ExpandoSketchTools.ResumeLayout(false);
            this.ExpandoSketchTools.PerformLayout();
            this.AddSectionContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).EndInit();
            this.SketchStatusBar.ResumeLayout(false);
            this.SketchStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ExpandoSketchTools;
        private System.Windows.Forms.ContextMenuStrip AddSectionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem jumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton BeginSectionBtn;
        private System.Windows.Forms.ToolStripLabel DistLbl;
        private System.Windows.Forms.ToolStripTextBox DistText;
        private System.Windows.Forms.ToolStripButton TextBtn;
        private System.Windows.Forms.ToolStripLabel TextLbl;
        private System.Windows.Forms.ToolStripTextBox FieldText;
        private System.Windows.Forms.ToolStripButton UnDoBtn;
        private System.Windows.Forms.ToolStripButton AddSectionBtn;
        private System.Windows.Forms.ToolStripButton DrawingDoneBtn;
        private System.Windows.Forms.PictureBox ExpSketchPBox;
        private System.Windows.Forms.DataGridView dgSections;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripButton tsbExitSketch;
        private System.Windows.Forms.StatusStrip SketchStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel sketchStatusMain;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    }
}