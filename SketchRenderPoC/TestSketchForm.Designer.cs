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
            this.debugInfoStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.debugInfoLabel = new System.Windows.Forms.Label();
            this.sketchBox = new System.Windows.Forms.PictureBox();
            this.sketchUpGlobalsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.x1TextBox = new System.Windows.Forms.TextBox();
            this.y1TextBox = new System.Windows.Forms.TextBox();
            this.x2TextBox = new System.Windows.Forms.TextBox();
            this.y2TextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.screenY2TextBox = new System.Windows.Forms.TextBox();
            this.screenX2TextBox = new System.Windows.Forms.TextBox();
            this.screenY1TextBox = new System.Windows.Forms.TextBox();
            this.screenX1TextBox = new System.Windows.Forms.TextBox();
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbDrawRed = new System.Windows.Forms.ToolStripButton();
            this.tsbDrawTeal = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveToPoint = new System.Windows.Forms.ToolStripButton();
            this.infoLabel = new System.Windows.Forms.Label();
            this.MainMenu.SuspendLayout();
            this.cmenuDrawing.SuspendLayout();
            this.StatusMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sketchUpGlobalsBindingSource)).BeginInit();
            this.toolStripMain.SuspendLayout();
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
            this.runTest1.Text = "Add Section";
            this.runTest1.ToolTipText = "Run the first test";
            this.runTest1.Click += new System.EventHandler(this.runTest1_Click);
            // 
            // runTest2
            // 
            this.runTest2.Name = "runTest2";
            this.runTest2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.runTest2.Size = new System.Drawing.Size(238, 26);
            this.runTest2.Text = "Undo Last Line";
            this.runTest2.ToolTipText = "Run the second test";
            this.runTest2.Click += new System.EventHandler(this.tsmListParcelSnapshots_Click);
            // 
            // runTest3
            // 
            this.runTest3.Name = "runTest3";
            this.runTest3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.runTest3.Size = new System.Drawing.Size(238, 26);
            this.runTest3.Text = "Identify Corners";
            this.runTest3.ToolTipText = "Run the third test";
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
            this.distanceText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.distanceText_KeyDown);
            this.distanceText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.distanceText_KeyPress);
            this.distanceText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.distanceText_KeyUp);
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
            this.feedbackStatus,
            this.debugInfoStatusLabel});
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
            this.feedbackStatus.Size = new System.Drawing.Size(951, 20);
            this.feedbackStatus.Spring = true;
            // 
            // debugInfoStatusLabel
            // 
            this.debugInfoStatusLabel.Name = "debugInfoStatusLabel";
            this.debugInfoStatusLabel.Size = new System.Drawing.Size(91, 20);
            this.debugInfoStatusLabel.Text = "Debug info: ";
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
            // sketchBox
            // 
            this.sketchBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.sketchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.sketchBox.Location = new System.Drawing.Point(0, 31);
            this.sketchBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sketchBox.Name = "sketchBox";
            this.sketchBox.Size = new System.Drawing.Size(1057, 545);
            this.sketchBox.TabIndex = 8;
            this.sketchBox.TabStop = false;
            // 
            // sketchUpGlobalsBindingSource
            // 
            this.sketchUpGlobalsBindingSource.DataSource = typeof(SketchUp.SketchUpGlobals);
            // 
            // x1TextBox
            // 
            this.x1TextBox.Location = new System.Drawing.Point(57, 606);
            this.x1TextBox.Name = "x1TextBox";
            this.x1TextBox.Size = new System.Drawing.Size(46, 22);
            this.x1TextBox.TabIndex = 9;
            // 
            // y1TextBox
            // 
            this.y1TextBox.Location = new System.Drawing.Point(111, 606);
            this.y1TextBox.Name = "y1TextBox";
            this.y1TextBox.Size = new System.Drawing.Size(46, 22);
            this.y1TextBox.TabIndex = 10;
            // 
            // x2TextBox
            // 
            this.x2TextBox.Location = new System.Drawing.Point(195, 606);
            this.x2TextBox.Name = "x2TextBox";
            this.x2TextBox.Size = new System.Drawing.Size(46, 22);
            this.x2TextBox.TabIndex = 11;
            // 
            // y2TextBox
            // 
            this.y2TextBox.Location = new System.Drawing.Point(248, 606);
            this.y2TextBox.Name = "y2TextBox";
            this.y2TextBox.Size = new System.Drawing.Size(46, 22);
            this.y2TextBox.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 583);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Db Coordinates";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 609);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "to";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(543, 609);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 17);
            this.label3.TabIndex = 20;
            this.label3.Text = "to";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(423, 583);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 17);
            this.label4.TabIndex = 19;
            this.label4.Text = "Screen Coordinates";
            // 
            // screenY2TextBox
            // 
            this.screenY2TextBox.Location = new System.Drawing.Point(641, 606);
            this.screenY2TextBox.Name = "screenY2TextBox";
            this.screenY2TextBox.Size = new System.Drawing.Size(46, 22);
            this.screenY2TextBox.TabIndex = 18;
            // 
            // screenX2TextBox
            // 
            this.screenX2TextBox.Location = new System.Drawing.Point(588, 606);
            this.screenX2TextBox.Name = "screenX2TextBox";
            this.screenX2TextBox.Size = new System.Drawing.Size(46, 22);
            this.screenX2TextBox.TabIndex = 17;
            // 
            // screenY1TextBox
            // 
            this.screenY1TextBox.Location = new System.Drawing.Point(477, 606);
            this.screenY1TextBox.Name = "screenY1TextBox";
            this.screenY1TextBox.Size = new System.Drawing.Size(46, 22);
            this.screenY1TextBox.TabIndex = 16;
            // 
            // screenX1TextBox
            // 
            this.screenX1TextBox.Location = new System.Drawing.Point(423, 606);
            this.screenX1TextBox.Name = "screenX1TextBox";
            this.screenX1TextBox.Size = new System.Drawing.Size(46, 22);
            this.screenX1TextBox.TabIndex = 15;
            // 
            // toolStripMain
            // 
            this.toolStripMain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDrawRed,
            this.tsbDrawTeal,
            this.tsbMoveToPoint});
            this.toolStripMain.Location = new System.Drawing.Point(0, 655);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1057, 47);
            this.toolStripMain.TabIndex = 21;
            this.toolStripMain.Text = "Draw";
            // 
            // tsbDrawRed
            // 
            this.tsbDrawRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDrawRed.Image = ((System.Drawing.Image)(resources.GetObject("tsbDrawRed.Image")));
            this.tsbDrawRed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDrawRed.Name = "tsbDrawRed";
            this.tsbDrawRed.Size = new System.Drawing.Size(70, 44);
            this.tsbDrawRed.Text = "Red Line";
            this.tsbDrawRed.Click += new System.EventHandler(this.tsbDrawRed_Click);
            // 
            // tsbDrawTeal
            // 
            this.tsbDrawTeal.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDrawTeal.Image = ((System.Drawing.Image)(resources.GetObject("tsbDrawTeal.Image")));
            this.tsbDrawTeal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDrawTeal.Name = "tsbDrawTeal";
            this.tsbDrawTeal.Padding = new System.Windows.Forms.Padding(10);
            this.tsbDrawTeal.Size = new System.Drawing.Size(92, 44);
            this.tsbDrawTeal.Text = "Teal Line";
            // 
            // tsbMoveToPoint
            // 
            this.tsbMoveToPoint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMoveToPoint.Image = ((System.Drawing.Image)(resources.GetObject("tsbMoveToPoint.Image")));
            this.tsbMoveToPoint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveToPoint.Name = "tsbMoveToPoint";
            this.tsbMoveToPoint.Padding = new System.Windows.Forms.Padding(10);
            this.tsbMoveToPoint.Size = new System.Drawing.Size(129, 44);
            this.tsbMoveToPoint.Text = "Move To Point";
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(722, 594);
            this.infoLabel.Margin = new System.Windows.Forms.Padding(2);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(31, 17);
            this.infoLabel.TabIndex = 22;
            this.infoLabel.Text = "Info";
            // 
            // TestSketchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1057, 727);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.screenY2TextBox);
            this.Controls.Add(this.screenX2TextBox);
            this.Controls.Add(this.screenY1TextBox);
            this.Controls.Add(this.screenX1TextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.y2TextBox);
            this.Controls.Add(this.x2TextBox);
            this.Controls.Add(this.y1TextBox);
            this.Controls.Add(this.x1TextBox);
            this.Controls.Add(this.sketchBox);
            this.Controls.Add(this.debugInfoLabel);
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
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem drawSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runTest2;
        private System.Windows.Forms.BindingSource sketchUpGlobalsBindingSource;
        private System.Windows.Forms.ToolStripTextBox distanceText;
        private System.Windows.Forms.ToolStripStatusLabel feedbackStatus;
        private System.Windows.Forms.Label debugInfoLabel;
        private System.Windows.Forms.ToolStripStatusLabel debugInfoStatusLabel;
        private System.Windows.Forms.PictureBox sketchBox;
        private System.Windows.Forms.TextBox x1TextBox;
        private System.Windows.Forms.TextBox y1TextBox;
        private System.Windows.Forms.TextBox x2TextBox;
        private System.Windows.Forms.TextBox y2TextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox screenY2TextBox;
        private System.Windows.Forms.TextBox screenX2TextBox;
        private System.Windows.Forms.TextBox screenY1TextBox;
        private System.Windows.Forms.TextBox screenX1TextBox;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton tsbDrawRed;
        private System.Windows.Forms.ToolStripButton tsbDrawTeal;
        private System.Windows.Forms.ToolStripButton tsbMoveToPoint;
        private System.Windows.Forms.Label infoLabel;
    }
}

