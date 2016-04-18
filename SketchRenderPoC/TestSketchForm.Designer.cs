namespace SketchRenderPoC
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
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.beginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSectionTsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.editSectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmListParcelSnapshots = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAllTests = new System.Windows.Forms.ToolStripMenuItem();
            this.flipHorizontalEWToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipVerticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMenuExitForm = new System.Windows.Forms.ToolStripMenuItem();
            this.drawSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wholeSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sectionFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmenuDrawing = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmenuJump = new System.Windows.Forms.ToolStripMenuItem();
            this.StatusMain = new System.Windows.Forms.StatusStrip();
            this.MouseLocationLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statLblStepInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.pctMain = new System.Windows.Forms.PictureBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Line = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.End = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attached = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sketchUpGlobalsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.MainMenu.SuspendLayout();
            this.cmenuDrawing.SuspendLayout();
            this.StatusMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sketchUpGlobalsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beginToolStripMenuItem,
            this.drawingToolStripMenuItem,
            this.drawSketchToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(1427, 28);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "Menu";
            // 
            // beginToolStripMenuItem
            // 
            this.beginToolStripMenuItem.Name = "beginToolStripMenuItem";
            this.beginToolStripMenuItem.Size = new System.Drawing.Size(12, 24);
            // 
            // drawingToolStripMenuItem
            // 
            this.drawingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addSectionTsMenu,
            this.editSectionsToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem2,
            this.tsmListParcelSnapshots,
            this.tsmAllTests,
            this.toolStripSeparator2,
            this.deleteSketchToolStripMenuItem,
            this.toolStripSeparator3,
            this.tsMenuExitForm});
            this.drawingToolStripMenuItem.Name = "drawingToolStripMenuItem";
            this.drawingToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.drawingToolStripMenuItem.Text = "Tests";
            this.drawingToolStripMenuItem.Click += new System.EventHandler(this.drawingToolStripMenuItem_Click);
            // 
            // addSectionTsMenu
            // 
            this.addSectionTsMenu.AutoToolTip = true;
            this.addSectionTsMenu.Image = global::SketchUp.Properties.Resources.AddControl_16x_32;
            this.addSectionTsMenu.Name = "addSectionTsMenu";
            this.addSectionTsMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D1)));
            this.addSectionTsMenu.Size = new System.Drawing.Size(279, 26);
            this.addSectionTsMenu.Text = "Break Lines D3 and D4";
            this.addSectionTsMenu.ToolTipText = "Add a Building Section";
            this.addSectionTsMenu.Click += new System.EventHandler(this.addSectionTsMenu_Click);
            // 
            // editSectionsToolStripMenuItem
            // 
            this.editSectionsToolStripMenuItem.AutoToolTip = true;
            this.editSectionsToolStripMenuItem.Image = global::SketchUp.Properties.Resources.Edit_grey_32xMD;
            this.editSectionsToolStripMenuItem.Name = "editSectionsToolStripMenuItem";
            this.editSectionsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D2)));
            this.editSectionsToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.editSectionsToolStripMenuItem.Text = "Test Combinability";
            this.editSectionsToolStripMenuItem.ToolTipText = "Delete or Change Section Type";
            this.editSectionsToolStripMenuItem.Click += new System.EventHandler(this.editSectionsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(276, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.D3)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(279, 26);
            this.toolStripMenuItem2.Text = "Combine Lines in D";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuCombinLinesInD_Click);
            // 
            // tsmListParcelSnapshots
            // 
            this.tsmListParcelSnapshots.Name = "tsmListParcelSnapshots";
            this.tsmListParcelSnapshots.Size = new System.Drawing.Size(279, 26);
            this.tsmListParcelSnapshots.Text = "List Parcel Snapshots";
            this.tsmListParcelSnapshots.Click += new System.EventHandler(this.tsmListParcelSnapshots_Click);
            // 
            // tsmAllTests
            // 
            this.tsmAllTests.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.flipHorizontalEWToolStripMenuItem,
            this.flipVerticalToolStripMenuItem});
            this.tsmAllTests.Name = "tsmAllTests";
            this.tsmAllTests.Size = new System.Drawing.Size(279, 26);
            this.tsmAllTests.Text = "All Tests";
            this.tsmAllTests.Click += new System.EventHandler(this.tsmAllTests_Click);
            // 
            // flipHorizontalEWToolStripMenuItem
            // 
            this.flipHorizontalEWToolStripMenuItem.Image = global::SketchUp.Properties.Resources.FlipHorizontal_16x_32;
            this.flipHorizontalEWToolStripMenuItem.Name = "flipHorizontalEWToolStripMenuItem";
            this.flipHorizontalEWToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.flipHorizontalEWToolStripMenuItem.Text = "Flip Horizontal";
            this.flipHorizontalEWToolStripMenuItem.Click += new System.EventHandler(this.flipHorizontalMenuItem_Click);
            // 
            // flipVerticalToolStripMenuItem
            // 
            this.flipVerticalToolStripMenuItem.Image = global::SketchUp.Properties.Resources.FlipVertical_16x_32;
            this.flipVerticalToolStripMenuItem.Name = "flipVerticalToolStripMenuItem";
            this.flipVerticalToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.flipVerticalToolStripMenuItem.Text = "Flip Vertical";
            this.flipVerticalToolStripMenuItem.Click += new System.EventHandler(this.flipVerticalMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(276, 6);
            // 
            // deleteSketchToolStripMenuItem
            // 
            this.deleteSketchToolStripMenuItem.Image = global::SketchUp.Properties.Resources.DeleteListItem_32x;
            this.deleteSketchToolStripMenuItem.Name = "deleteSketchToolStripMenuItem";
            this.deleteSketchToolStripMenuItem.Size = new System.Drawing.Size(279, 26);
            this.deleteSketchToolStripMenuItem.Text = "Delete Sketch";
            this.deleteSketchToolStripMenuItem.Click += new System.EventHandler(this.deleteSketchToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(276, 6);
            // 
            // tsMenuExitForm
            // 
            this.tsMenuExitForm.AutoToolTip = true;
            this.tsMenuExitForm.Name = "tsMenuExitForm";
            this.tsMenuExitForm.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.tsMenuExitForm.Size = new System.Drawing.Size(279, 26);
            this.tsMenuExitForm.Text = "Exit Sketch View";
            this.tsMenuExitForm.ToolTipText = "Return to the Main Screen";
            this.tsMenuExitForm.Click += new System.EventHandler(this.tsMenuExitForm_Click);
            // 
            // drawSketchToolStripMenuItem
            // 
            this.drawSketchToolStripMenuItem.Name = "drawSketchToolStripMenuItem";
            this.drawSketchToolStripMenuItem.Size = new System.Drawing.Size(103, 24);
            this.drawSketchToolStripMenuItem.Text = "Draw Sketch";
            this.drawSketchToolStripMenuItem.Click += new System.EventHandler(this.drawSketchToolStripMenuItem_Click);
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
            // 
            // StatusMain
            // 
            this.StatusMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MouseLocationLabel,
            this.statLblStepInfo});
            this.StatusMain.Location = new System.Drawing.Point(0, 715);
            this.StatusMain.Name = "StatusMain";
            this.StatusMain.Size = new System.Drawing.Size(1427, 25);
            this.StatusMain.TabIndex = 2;
            this.StatusMain.Text = "Status";
            // 
            // MouseLocationLabel
            // 
            this.MouseLocationLabel.Name = "MouseLocationLabel";
            this.MouseLocationLabel.Size = new System.Drawing.Size(0, 20);
            // 
            // statLblStepInfo
            // 
            this.statLblStepInfo.Name = "statLblStepInfo";
            this.statLblStepInfo.Size = new System.Drawing.Size(46, 20);
            this.statLblStepInfo.Text = "Step: ";
            // 
            // pctMain
            // 
            this.pctMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pctMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctMain.ContextMenuStrip = this.cmenuDrawing;
            this.pctMain.Location = new System.Drawing.Point(49, 31);
            this.pctMain.Name = "pctMain";
            this.pctMain.Size = new System.Drawing.Size(954, 514);
            this.pctMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pctMain.TabIndex = 4;
            this.pctMain.TabStop = false;
            this.pctMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pctMain_MouseMove);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Version,
            this.Section,
            this.Line,
            this.Start,
            this.End,
            this.Attached});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 565);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1427, 150);
            this.dataGridView1.TabIndex = 5;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.Width = 5;
            // 
            // Section
            // 
            this.Section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Section.HeaderText = "Section";
            this.Section.Name = "Section";
            this.Section.ReadOnly = true;
            this.Section.Width = 5;
            // 
            // Line
            // 
            this.Line.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Line.HeaderText = "Line";
            this.Line.Name = "Line";
            this.Line.ReadOnly = true;
            this.Line.Width = 5;
            // 
            // Start
            // 
            this.Start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Start.HeaderText = "Start";
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            this.Start.Width = 5;
            // 
            // End
            // 
            this.End.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.End.HeaderText = "End";
            this.End.Name = "End";
            this.End.ReadOnly = true;
            this.End.Width = 5;
            // 
            // Attached
            // 
            this.Attached.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Attached.HeaderText = "Attached";
            this.Attached.Name = "Attached";
            this.Attached.ReadOnly = true;
            this.Attached.Width = 5;
            // 
            // sketchUpGlobalsBindingSource
            // 
            this.sketchUpGlobalsBindingSource.DataSource = typeof(SketchUp.SketchUpGlobals);
            // 
            // TestSketchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1427, 740);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pctMain);
            this.Controls.Add(this.StatusMain);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "TestSketchForm";
            this.Text = "Render Initial Sketch";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.cmenuDrawing.ResumeLayout(false);
            this.StatusMain.ResumeLayout(false);
            this.StatusMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sketchUpGlobalsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ContextMenuStrip cmenuDrawing;
        private System.Windows.Forms.StatusStrip StatusMain;
        private System.Windows.Forms.ToolStripMenuItem beginToolStripMenuItem;
        private System.Windows.Forms.PictureBox pctMain;
        private System.Windows.Forms.ToolStripStatusLabel MouseLocationLabel;
        private System.Windows.Forms.ToolStripMenuItem wholeSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sectionFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmenuJump;
        private System.Windows.Forms.ToolStripMenuItem drawingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSectionTsMenu;
        private System.Windows.Forms.ToolStripMenuItem editSectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmAllTests;
        private System.Windows.Forms.ToolStripMenuItem deleteSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsMenuExitForm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem flipHorizontalEWToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipVerticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem tsmListParcelSnapshots;
        private System.Windows.Forms.ToolStripStatusLabel statLblStepInfo;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource sketchUpGlobalsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attached;
        private System.Windows.Forms.DataGridViewTextBoxColumn End;
        private System.Windows.Forms.DataGridViewTextBoxColumn Start;
        private System.Windows.Forms.DataGridViewTextBoxColumn Line;
        private System.Windows.Forms.DataGridViewTextBoxColumn Section;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
    }
}

