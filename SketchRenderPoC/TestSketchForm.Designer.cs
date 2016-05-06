namespace SketchUp
{
    partial class TestSketchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestSketchForm));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.beginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runTest1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.runTest2 = new System.Windows.Forms.ToolStripMenuItem();
            this.runTest3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMenuExitForm = new System.Windows.Forms.ToolStripMenuItem();
            this.drawSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceText = new System.Windows.Forms.ToolStripTextBox();
            this.wholeSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuDrawing = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenuJump = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMain = new System.Windows.Forms.StatusStrip();
            this.mouseLocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.feedbackStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.sketchBox = new System.Windows.Forms.PictureBox();
            this.sketchUpGlobalsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.debugInfoLabel = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.cmenuDrawing.SuspendLayout();
            this.StatusMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sketchUpGlobalsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beginToolStripMenuItem,
            this.drawingToolStripMenuItem,
            this.drawSketchToolStripMenuItem,
            this.distanceText});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1057, 31);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "Menu";
            // 
            // beginToolStripMenuItem
            // 
            this.beginToolStripMenuItem.Name = "beginToolStripMenuItem";
            this.beginToolStripMenuItem.Size = new System.Drawing.Size(12, 27);
            // 
            // drawingToolStripMenuItem
            // 
            this.drawingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runTest1,
            this.toolStripSeparator1,
            this.runTest2,
            this.runTest3,
            this.toolStripSeparator2,
            this.toolStripSeparator3,
            this.tsMenuExitForm});
            this.drawingToolStripMenuItem.Name = "drawingToolStripMenuItem";
            this.drawingToolStripMenuItem.Size = new System.Drawing.Size(54, 27);
            this.drawingToolStripMenuItem.Text = "Tests";
            this.drawingToolStripMenuItem.Click += new System.EventHandler(this.drawingToolStripMenuItem_Click);
            // 
            // runTest1
            // 
            this.runTest1.AutoToolTip = true;
            this.runTest1.Image = global::SketchUp.Properties.Resources.AddControl_16x_32;
            this.runTest1.Name = "runTest1";
            this.runTest1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.runTest1.Size = new System.Drawing.Size(238, 26);
            this.runTest1.Text = "Test 1";
            this.runTest1.ToolTipText = "Add a Building Section";
            this.runTest1.Click += new System.EventHandler(this.runTest1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(235, 6);
            // 
            // runTest2
            // 
            this.runTest2.Name = "runTest2";
            this.runTest2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.runTest2.Size = new System.Drawing.Size(238, 26);
            this.runTest2.Text = "Begin Drawing";
            this.runTest2.Click += new System.EventHandler(this.tsmListParcelSnapshots_Click);
            // 
            // runTest3
            // 
            this.runTest3.Name = "runTest3";
            this.runTest3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.runTest3.Size = new System.Drawing.Size(238, 26);
            this.runTest3.Text = "End Drawing";
            this.runTest3.Click += new System.EventHandler(this.tsmAllTests_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(235, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(235, 6);
            // 
            // tsMenuExitForm
            // 
            this.tsMenuExitForm.AutoToolTip = true;
            this.tsMenuExitForm.Name = "tsMenuExitForm";
            this.tsMenuExitForm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.tsMenuExitForm.Size = new System.Drawing.Size(238, 26);
            this.tsMenuExitForm.Text = "Exit Sketch View";
            this.tsMenuExitForm.ToolTipText = "Return to the Main Screen";
            this.tsMenuExitForm.Click += new System.EventHandler(this.tsMenuExitForm_Click);
            // 
            // drawSketchToolStripMenuItem
            // 
            this.drawSketchToolStripMenuItem.Name = "drawSketchToolStripMenuItem";
            this.drawSketchToolStripMenuItem.Size = new System.Drawing.Size(103, 27);
            this.drawSketchToolStripMenuItem.Text = "Draw Sketch";
            this.drawSketchToolStripMenuItem.Click += new System.EventHandler(this.drawSketchToolStripMenuItem_Click);
            // 
            // distanceText
            // 
            this.distanceText.AcceptsTab = true;
            this.distanceText.AutoSize = false;
            this.distanceText.AutoToolTip = true;
            this.distanceText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.distanceText.MaxLength = 10;
            this.distanceText.Name = "distanceText";
            this.distanceText.Size = new System.Drawing.Size(100, 27);
            this.distanceText.ToolTipText = "For an angle, enter the horizontal distance, a comma, and the vertical distance";
            // 
            // wholeSketchToolStripMenuItem
            // 
            this.wholeSketchToolStripMenuItem.Name = "wholeSketchToolStripMenuItem";
            this.wholeSketchToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // sectionAToolStripMenuItem
            // 
            this.sectionAToolStripMenuItem.Name = "sectionAToolStripMenuItem";
            this.sectionAToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // sectionBToolStripMenuItem
            // 
            this.sectionBToolStripMenuItem.Name = "sectionBToolStripMenuItem";
            this.sectionBToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // sectionCToolStripMenuItem
            // 
            this.sectionCToolStripMenuItem.Name = "sectionCToolStripMenuItem";
            this.sectionCToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // sectionDToolStripMenuItem
            // 
            this.sectionDToolStripMenuItem.Name = "sectionDToolStripMenuItem";
            this.sectionDToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // sectionFToolStripMenuItem
            // 
            this.sectionFToolStripMenuItem.Name = "sectionFToolStripMenuItem";
            this.sectionFToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // cmenuDrawing
            // 
            this.cmenuDrawing.DropShadowEnabled = false;
            this.cmenuDrawing.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmenuDrawing.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmenuJump});
            this.cmenuDrawing.Name = "cmenuDrawing";
            this.cmenuDrawing.Size = new System.Drawing.Size(163, 30);
            // 
            // cmenuJump
            // 
            this.cmenuJump.AutoToolTip = true;
            this.cmenuJump.Image = global::SketchUp.Properties.Resources.Jump_Point;
            this.cmenuJump.Name = "cmenuJump";
            this.cmenuJump.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.J)));
            this.cmenuJump.Size = new System.Drawing.Size(162, 26);
            this.cmenuJump.Text = "Jump";
            this.cmenuJump.ToolTipText = "Jump to the nearest corner";
            this.cmenuJump.Click += new System.EventHandler(this.cmenuJump_Click);
            // 
            // StatusMain
            // 
            this.StatusMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mouseLocationLabel,
            this.feedbackStatus});
            this.StatusMain.Location = new System.Drawing.Point(0, 702);
            this.StatusMain.Name = "StatusMain";
            this.StatusMain.Size = new System.Drawing.Size(1057, 25);
            this.StatusMain.TabIndex = 2;
            this.StatusMain.Text = "Status";
            // 
            // mouseLocationLabel
            // 
            this.mouseLocationLabel.AutoSize = false;
            this.mouseLocationLabel.Name = "mouseLocationLabel";
            this.mouseLocationLabel.Size = new System.Drawing.Size(0, 20);
            // 
            // feedbackStatus
            // 
            this.feedbackStatus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.feedbackStatus.Name = "feedbackStatus";
            this.feedbackStatus.Size = new System.Drawing.Size(1042, 20);
            this.feedbackStatus.Spring = true;
            // 
            // sketchBox
            // 
            this.sketchBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.sketchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sketchBox.ContextMenuStrip = this.cmenuDrawing;
            this.sketchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.sketchBox.Location = new System.Drawing.Point(0, 31);
            this.sketchBox.Name = "sketchBox";
            this.sketchBox.Size = new System.Drawing.Size(1057, 629);
            this.sketchBox.TabIndex = 6;
            this.sketchBox.TabStop = false;
            this.sketchBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sketchBox_MouseMove);
            // 
            // sketchUpGlobalsBindingSource
            // 
            this.sketchUpGlobalsBindingSource.DataSource = typeof(SketchUp.SketchUpGlobals);
            // 
            // debugInfoLabel
            // 
            this.debugInfoLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.debugInfoLabel.AutoEllipsis = true;
            this.debugInfoLabel.AutoSize = true;
            this.debugInfoLabel.Location = new System.Drawing.Point(0, 667);
            this.debugInfoLabel.Name = "debugInfoLabel";
            this.debugInfoLabel.Size = new System.Drawing.Size(60, 17);
            this.debugInfoLabel.TabIndex = 7;
            this.debugInfoLabel.Text = "Status...";
            // 
            // TestSketchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1057, 727);
            this.Controls.Add(this.debugInfoLabel);
            this.Controls.Add(this.sketchBox);
            this.Controls.Add(this.StatusMain);
            this.Controls.Add(this.MainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.Name = "TestSketchForm";
            this.Text = "Render Initial Sketch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestSketchForm_FormClosing);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.cmenuDrawing.ResumeLayout(false);
            this.StatusMain.ResumeLayout(false);
            this.StatusMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sketchUpGlobalsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ContextMenuStrip cmenuDrawing;
        private System.Windows.Forms.StatusStrip StatusMain;
        private System.Windows.Forms.ToolStripMenuItem beginToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel mouseLocationLabel;
        private System.Windows.Forms.ToolStripMenuItem wholeSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmenuJump;
        private System.Windows.Forms.ToolStripMenuItem drawingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runTest1;
        private System.Windows.Forms.ToolStripMenuItem runTest3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsMenuExitForm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem drawSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runTest2;
        private System.Windows.Forms.BindingSource sketchUpGlobalsBindingSource;
        private System.Windows.Forms.PictureBox sketchBox;
        private System.Windows.Forms.ToolStripTextBox distanceText;
        private System.Windows.Forms.ToolStripStatusLabel feedbackStatus;
        private System.Windows.Forms.Label debugInfoLabel;
    }
}

