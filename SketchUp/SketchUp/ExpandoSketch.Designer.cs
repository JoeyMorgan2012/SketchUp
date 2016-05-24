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
            this.DistLbl = new System.Windows.Forms.ToolStripLabel();
            this.DistText = new System.Windows.Forms.ToolStripTextBox();
            this.TextLbl = new System.Windows.Forms.ToolStripLabel();
            this.FieldText = new System.Windows.Forms.ToolStripTextBox();
            this.dgSections = new System.Windows.Forms.DataGridView();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.SketchStatusBar = new System.Windows.Forms.StatusStrip();
            this.sketchStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.snapshotIndexLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ExpSketchPBox = new System.Windows.Forms.PictureBox();
            this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BeginSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.UnDoBtn = new System.Windows.Forms.ToolStripButton();
            this.AddSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.DrawingDoneBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbExitSketch = new System.Windows.Forms.ToolStripButton();
            this.beginDrawingCMI = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDrawingCMI = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.flipHorizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipVeritcallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.doneSketchingCMI = new System.Windows.Forms.ToolStripMenuItem();
            this.exitSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExpandoSketchTools.SuspendLayout();
            this.AddSectionContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).BeginInit();
            this.SketchStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ExpandoSketchTools
            // 
            this.ExpandoSketchTools.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ExpandoSketchTools.ContextMenuStrip = this.AddSectionContextMenu;
            this.ExpandoSketchTools.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ExpandoSketchTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BeginSectionBtn,
            this.DistLbl,
            this.DistText,
            this.TextLbl,
            this.FieldText,
            this.UnDoBtn,
            this.AddSectionBtn,
            this.DrawingDoneBtn,
            this.toolStripButton1,
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
            this.jumpToolStripMenuItem,
            this.beginDrawingCMI,
            this.doneSketchingCMI,
            this.saveDrawingCMI,
            this.toolStripSeparator2,
            this.flipHorizontalToolStripMenuItem,
            this.flipVeritcallyToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitSketchToolStripMenuItem});
            this.AddSectionContextMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.AddSectionContextMenu.Name = "contextMenuStrip1";
            this.AddSectionContextMenu.Size = new System.Drawing.Size(287, 198);
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
            // TextLbl
            // 
            this.TextLbl.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(81, 19);
            this.TextLbl.Text = "Description:";
            // 
            // FieldText
            // 
            this.FieldText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FieldText.Margin = new System.Windows.Forms.Padding(1, 0, 15, 0);
            this.FieldText.Name = "FieldText";
            this.FieldText.Size = new System.Drawing.Size(200, 27);
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
            // SketchStatusBar
            // 
            this.SketchStatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SketchStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sketchStatusMessage,
            this.snapshotIndexLabel});
            this.SketchStatusBar.Location = new System.Drawing.Point(0, 575);
            this.SketchStatusBar.Name = "SketchStatusBar";
            this.SketchStatusBar.Size = new System.Drawing.Size(1014, 24);
            this.SketchStatusBar.TabIndex = 3;
            // 
            // sketchStatusMessage
            // 
            this.sketchStatusMessage.AutoSize = false;
            this.sketchStatusMessage.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sketchStatusMessage.Name = "sketchStatusMessage";
            this.sketchStatusMessage.Padding = new System.Windows.Forms.Padding(2, 0, 6, 0);
            this.sketchStatusMessage.Size = new System.Drawing.Size(499, 19);
            this.sketchStatusMessage.Spring = true;
            this.sketchStatusMessage.Text = "Sketch Loaded";
            this.sketchStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // snapshotIndexLabel
            // 
            this.snapshotIndexLabel.AutoSize = false;
            this.snapshotIndexLabel.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.snapshotIndexLabel.Name = "snapshotIndexLabel";
            this.snapshotIndexLabel.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.snapshotIndexLabel.Size = new System.Drawing.Size(499, 19);
            this.snapshotIndexLabel.Spring = true;
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
            // jumpToolStripMenuItem
            // 
            this.jumpToolStripMenuItem.AutoToolTip = true;
            this.jumpToolStripMenuItem.Enabled = false;
            this.jumpToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("jumpToolStripMenuItem.Image")));
            this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
            this.jumpToolStripMenuItem.ShowShortcutKeys = false;
            this.jumpToolStripMenuItem.Size = new System.Drawing.Size(286, 26);
            this.jumpToolStripMenuItem.Text = "Jump to Nearest Corner";
            this.jumpToolStripMenuItem.ToolTipText = "Jump to Nearest Corner (Add section to Enable";
            this.jumpToolStripMenuItem.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // BeginSectionBtn
            // 
            this.BeginSectionBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.BeginSectionBtn.Enabled = false;
            this.BeginSectionBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BeginSectionBtn.Image = global::SketchUp.Properties.Resources.Edit_32xMD;
            this.BeginSectionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BeginSectionBtn.Margin = new System.Windows.Forms.Padding(25, 1, 15, 2);
            this.BeginSectionBtn.Name = "BeginSectionBtn";
            this.BeginSectionBtn.Size = new System.Drawing.Size(124, 24);
            this.BeginSectionBtn.Text = "Begin Section";
            this.BeginSectionBtn.ToolTipText = "Begin Drawing Section";
            this.BeginSectionBtn.Click += new System.EventHandler(this.BeginSectionBtn_Click);
            // 
            // UnDoBtn
            // 
            this.UnDoBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.UnDoBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnDoBtn.Image = global::SketchUp.Properties.Resources.Undo_grey_32x;
            this.UnDoBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UnDoBtn.Margin = new System.Windows.Forms.Padding(10, 1, 15, 2);
            this.UnDoBtn.Name = "UnDoBtn";
            this.UnDoBtn.Size = new System.Drawing.Size(71, 24);
            this.UnDoBtn.Text = "UnDo";
            this.UnDoBtn.Click += new System.EventHandler(this.UnDoBtn_Click);
            // 
            // AddSectionBtn
            // 
            this.AddSectionBtn.AutoSize = false;
            this.AddSectionBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AddSectionBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddSectionBtn.Image = global::SketchUp.Properties.Resources.Add_inverse_16x;
            this.AddSectionBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddSectionBtn.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.AddSectionBtn.Name = "AddSectionBtn";
            this.AddSectionBtn.Size = new System.Drawing.Size(60, 23);
            this.AddSectionBtn.Text = "Add";
            this.AddSectionBtn.Click += new System.EventHandler(this.AddSectionBtn_Click);
            // 
            // DrawingDoneBtn
            // 
            this.DrawingDoneBtn.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.DrawingDoneBtn.Enabled = false;
            this.DrawingDoneBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrawingDoneBtn.Image = global::SketchUp.Properties.Resources.GreenCheck;
            this.DrawingDoneBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DrawingDoneBtn.Margin = new System.Windows.Forms.Padding(0, 1, 15, 2);
            this.DrawingDoneBtn.Name = "DrawingDoneBtn";
            this.DrawingDoneBtn.Size = new System.Drawing.Size(129, 24);
            this.DrawingDoneBtn.Text = "Done Drawing";
            this.DrawingDoneBtn.ToolTipText = "Click when you have added all lines in the section.";
            this.DrawingDoneBtn.Click += new System.EventHandler(this.DrawingDoneBtn_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButton1.Image = global::SketchUp.Properties.Resources.Save;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(64, 24);
            this.toolStripButton1.Text = "Save";
            // 
            // tsbExitSketch
            // 
            this.tsbExitSketch.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tsbExitSketch.Image = global::SketchUp.Properties.Resources.icon_home;
            this.tsbExitSketch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExitSketch.Name = "tsbExitSketch";
            this.tsbExitSketch.Size = new System.Drawing.Size(163, 24);
            this.tsbExitSketch.Text = "Exit Sketch Window";
            this.tsbExitSketch.ToolTipText = "Return to Main Window";
            this.tsbExitSketch.Click += new System.EventHandler(this.tsbExitSketch_Click);
            // 
            // beginDrawingCMI
            // 
            this.beginDrawingCMI.Image = global::SketchUp.Properties.Resources.Edit_32xMD;
            this.beginDrawingCMI.Name = "beginDrawingCMI";
            this.beginDrawingCMI.Size = new System.Drawing.Size(286, 26);
            this.beginDrawingCMI.Text = "Begin Drawing";
            // 
            // saveDrawingCMI
            // 
            this.saveDrawingCMI.Image = global::SketchUp.Properties.Resources.Save;
            this.saveDrawingCMI.Name = "saveDrawingCMI";
            this.saveDrawingCMI.Size = new System.Drawing.Size(286, 26);
            this.saveDrawingCMI.Text = "Save to Database";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(283, 6);
            // 
            // flipHorizontalToolStripMenuItem
            // 
            this.flipHorizontalToolStripMenuItem.Image = global::SketchUp.Properties.Resources.FlipHorizontal_16x;
            this.flipHorizontalToolStripMenuItem.Name = "flipHorizontalToolStripMenuItem";
            this.flipHorizontalToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.flipHorizontalToolStripMenuItem.Size = new System.Drawing.Size(286, 26);
            this.flipHorizontalToolStripMenuItem.Text = "Flip Horizontally";
            // 
            // flipVeritcallyToolStripMenuItem
            // 
            this.flipVeritcallyToolStripMenuItem.Image = global::SketchUp.Properties.Resources.FlipVertical_16x;
            this.flipVeritcallyToolStripMenuItem.Name = "flipVeritcallyToolStripMenuItem";
            this.flipVeritcallyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.flipVeritcallyToolStripMenuItem.Size = new System.Drawing.Size(286, 26);
            this.flipVeritcallyToolStripMenuItem.Text = "Flip Veritcally";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(283, 6);
            // 
            // doneSketchingCMI
            // 
            this.doneSketchingCMI.Image = global::SketchUp.Properties.Resources.GreenCheck;
            this.doneSketchingCMI.Name = "doneSketchingCMI";
            this.doneSketchingCMI.Size = new System.Drawing.Size(286, 26);
            this.doneSketchingCMI.Text = "Done Sketching";
            // 
            // exitSketchToolStripMenuItem
            // 
            this.exitSketchToolStripMenuItem.Image = global::SketchUp.Properties.Resources.icon_home;
            this.exitSketchToolStripMenuItem.Name = "exitSketchToolStripMenuItem";
            this.exitSketchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.exitSketchToolStripMenuItem.Size = new System.Drawing.Size(286, 26);
            this.exitSketchToolStripMenuItem.Text = "Exit Sketch";
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
            this.SketchStatusBar.ResumeLayout(false);
            this.SketchStatusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).EndInit();
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
        private System.Windows.Forms.ToolStripStatusLabel sketchStatusMessage;
        private System.Windows.Forms.ToolStripStatusLabel snapshotIndexLabel;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem beginDrawingCMI;
        private System.Windows.Forms.ToolStripMenuItem doneSketchingCMI;
        private System.Windows.Forms.ToolStripMenuItem saveDrawingCMI;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem flipHorizontalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipVeritcallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitSketchToolStripMenuItem;
    }
}