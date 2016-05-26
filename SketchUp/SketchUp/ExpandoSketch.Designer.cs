using System.Windows.Forms;

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
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.AddSectionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiSepLine1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiSepLine2 = new System.Windows.Forms.ToolStripSeparator();
            this.DistLbl = new System.Windows.Forms.ToolStripLabel();
            this.DistText = new System.Windows.Forms.ToolStripTextBox();
            this.TextLbl = new System.Windows.Forms.ToolStripLabel();
            this.FieldText = new System.Windows.Forms.ToolStripTextBox();
            this.dgSections = new System.Windows.Forms.DataGridView();
            this.selectFontDlg = new System.Windows.Forms.FontDialog();
            this.SketchStatusBar = new System.Windows.Forms.StatusStrip();
            this.sketchStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.snapshotIndexLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.miFile = new System.Windows.Forms.ToolStripMenuItem();
            this.separator1 = new System.Windows.Forms.ToolStripSeparator();
            this.separator2 = new System.Windows.Forms.ToolStripSeparator();
            this.miEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditUndoSectionLines = new System.Windows.Forms.ToolStripMenuItem();
            this.miUndoAddSection = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.miHelpContact = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.miHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.sketchBox = new System.Windows.Forms.PictureBox();
            this.cmiJumpToCorner = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiBeginDrawing = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiAddClosingLine = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiDoneSketching = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSaveDrawing = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFlipHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFlipVeritcally = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiExitSketch = new System.Windows.Forms.ToolStripMenuItem();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnUnDo = new System.Windows.Forms.ToolStripButton();
            this.btnEditSections = new System.Windows.Forms.ToolStripButton();
            this.miFileAddSection = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileEditSection = new System.Windows.Forms.ToolStripMenuItem();
            this.miFileCloseNoSave = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAndContinue = new System.Windows.Forms.ToolStripMenuItem();
            this.miSaveAndClose = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditUndoLine = new System.Windows.Forms.ToolStripMenuItem();
            this.MainToolStrip.SuspendLayout();
            this.AddSectionContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).BeginInit();
            this.SketchStatusBar.SuspendLayout();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.BackColor = System.Drawing.SystemColors.MenuBar;
            this.MainToolStrip.ContextMenuStrip = this.AddSectionContextMenu;
            this.MainToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DistLbl,
            this.DistText,
            this.TextLbl,
            this.FieldText,
            this.toolStripSeparator3,
            this.btnAdd,
            this.btnUnDo,
            this.toolStripSeparator2,
            this.btnEditSections});
            this.MainToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.MainToolStrip.Location = new System.Drawing.Point(0, 28);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Size = new System.Drawing.Size(1022, 27);
            this.MainToolStrip.Stretch = true;
            this.MainToolStrip.TabIndex = 0;
            this.MainToolStrip.Text = "Sketch Tools";
            this.MainToolStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.ExpandoSketchTools_ItemClicked);
            // 
            // AddSectionContextMenu
            // 
            this.AddSectionContextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.AddSectionContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.AddSectionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiJumpToCorner,
            this.cmiBeginDrawing,
            this.cmiAddClosingLine,
            this.cmiDoneSketching,
            this.cmiSaveDrawing,
            this.cmiSepLine1,
            this.cmiFlipHorizontally,
            this.cmiFlipVeritcally,
            this.cmiSepLine2,
            this.cmiExitSketch});
            this.AddSectionContextMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.AddSectionContextMenu.Name = "AddSectionContextMenu";
            this.AddSectionContextMenu.Size = new System.Drawing.Size(287, 252);
            // 
            // cmiSepLine1
            // 
            this.cmiSepLine1.Name = "cmiSepLine1";
            this.cmiSepLine1.Size = new System.Drawing.Size(283, 6);
            // 
            // cmiSepLine2
            // 
            this.cmiSepLine2.Name = "cmiSepLine2";
            this.cmiSepLine2.Size = new System.Drawing.Size(283, 6);
            // 
            // DistLbl
            // 
            this.DistLbl.Name = "DistLbl";
            this.DistLbl.Size = new System.Drawing.Size(66, 24);
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
            this.TextLbl.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(88, 24);
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
            this.SketchStatusBar.Location = new System.Drawing.Point(0, 652);
            this.SketchStatusBar.Name = "SketchStatusBar";
            this.SketchStatusBar.Size = new System.Drawing.Size(1022, 24);
            this.SketchStatusBar.TabIndex = 3;
            // 
            // sketchStatusMessage
            // 
            this.sketchStatusMessage.AutoSize = false;
            this.sketchStatusMessage.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sketchStatusMessage.Name = "sketchStatusMessage";
            this.sketchStatusMessage.Padding = new System.Windows.Forms.Padding(2, 0, 6, 0);
            this.sketchStatusMessage.Size = new System.Drawing.Size(503, 19);
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
            this.snapshotIndexLabel.Size = new System.Drawing.Size(503, 19);
            this.snapshotIndexLabel.Spring = true;
            // 
            // mnuMain
            // 
            this.mnuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFile,
            this.miEdit,
            this.miHelp});
            this.mnuMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1022, 28);
            this.mnuMain.TabIndex = 4;
            this.mnuMain.Text = "Sketch Editor";
            // 
            // miFile
            // 
            this.miFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miFileAddSection,
            this.miFileEditSection,
            this.separator1,
            this.miFileCloseNoSave,
            this.separator2,
            this.miSaveAndContinue,
            this.miSaveAndClose});
            this.miFile.Name = "miFile";
            this.miFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F)));
            this.miFile.Size = new System.Drawing.Size(44, 24);
            this.miFile.Text = "File";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(264, 6);
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(264, 6);
            // 
            // miEdit
            // 
            this.miEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miEditUndoLine,
            this.miEditUndoSectionLines,
            this.miUndoAddSection});
            this.miEdit.Name = "miEdit";
            this.miEdit.Size = new System.Drawing.Size(47, 24);
            this.miEdit.Text = "Edit";
            // 
            // miEditUndoSectionLines
            // 
            this.miEditUndoSectionLines.Name = "miEditUndoSectionLines";
            this.miEditUndoSectionLines.Size = new System.Drawing.Size(210, 26);
            this.miEditUndoSectionLines.Text = "Undo Section Lines";
            // 
            // miUndoAddSection
            // 
            this.miUndoAddSection.Name = "miUndoAddSection";
            this.miUndoAddSection.Size = new System.Drawing.Size(210, 26);
            this.miUndoAddSection.Text = "Undo Add Section";
            // 
            // miHelp
            // 
            this.miHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miHelpContact,
            this.toolStripSeparator1,
            this.miHelpAbout});
            this.miHelp.Name = "miHelp";
            this.miHelp.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.miHelp.Size = new System.Drawing.Size(53, 24);
            this.miHelp.Text = "Help";
            this.miHelp.Visible = false;
            // 
            // miHelpContact
            // 
            this.miHelpContact.Name = "miHelpContact";
            this.miHelpContact.Size = new System.Drawing.Size(296, 26);
            this.miHelpContact.Text = "Contact Stonewall Technologies";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(293, 6);
            // 
            // miHelpAbout
            // 
            this.miHelpAbout.Name = "miHelpAbout";
            this.miHelpAbout.Size = new System.Drawing.Size(296, 26);
            this.miHelpAbout.Text = "About CAMRA SketchUp";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // sketchBox
            // 
            this.sketchBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.sketchBox.ContextMenuStrip = this.AddSectionContextMenu;
            this.sketchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sketchBox.Location = new System.Drawing.Point(0, 55);
            this.sketchBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sketchBox.Name = "sketchBox";
            this.sketchBox.Size = new System.Drawing.Size(1022, 621);
            this.sketchBox.TabIndex = 2;
            this.sketchBox.TabStop = false;
            this.sketchBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseClick);
            this.sketchBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseDown);
            this.sketchBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseUp);
            // 
            // cmiJumpToCorner
            // 
            this.cmiJumpToCorner.AutoToolTip = true;
            this.cmiJumpToCorner.Enabled = false;
            this.cmiJumpToCorner.Image = global::SketchUp.Properties.Resources.JumpPointImage;
            this.cmiJumpToCorner.Name = "cmiJumpToCorner";
            this.cmiJumpToCorner.ShowShortcutKeys = false;
            this.cmiJumpToCorner.Size = new System.Drawing.Size(286, 26);
            this.cmiJumpToCorner.Text = "Jump to Nearest Corner";
            this.cmiJumpToCorner.ToolTipText = "Jump to Nearest Corner (Add section to Enable";
            this.cmiJumpToCorner.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // cmiBeginDrawing
            // 
            this.cmiBeginDrawing.Image = global::SketchUp.Properties.Resources.EditImage;
            this.cmiBeginDrawing.Name = "cmiBeginDrawing";
            this.cmiBeginDrawing.Size = new System.Drawing.Size(286, 26);
            this.cmiBeginDrawing.Text = "Begin Drawing";
            this.cmiBeginDrawing.Click += new System.EventHandler(this.cmiBeginDrawing_Click);
            // 
            // cmiAddClosingLine
            // 
            this.cmiAddClosingLine.Image = global::SketchUp.Properties.Resources.CloseSection;
            this.cmiAddClosingLine.Name = "cmiAddClosingLine";
            this.cmiAddClosingLine.Size = new System.Drawing.Size(286, 26);
            this.cmiAddClosingLine.Text = "Add Closing Line";
            this.cmiAddClosingLine.Click += new System.EventHandler(this.cmiAddClosingLine_Click);
            // 
            // cmiDoneSketching
            // 
            this.cmiDoneSketching.Image = global::SketchUp.Properties.Resources.GreenCheck;
            this.cmiDoneSketching.Name = "cmiDoneSketching";
            this.cmiDoneSketching.Size = new System.Drawing.Size(286, 26);
            this.cmiDoneSketching.Text = "Done Sketching";
            // 
            // cmiSaveDrawing
            // 
            this.cmiSaveDrawing.Image = global::SketchUp.Properties.Resources.Save;
            this.cmiSaveDrawing.Name = "cmiSaveDrawing";
            this.cmiSaveDrawing.Size = new System.Drawing.Size(286, 26);
            this.cmiSaveDrawing.Text = "Save to Database";
            // 
            // cmiFlipHorizontally
            // 
            this.cmiFlipHorizontally.Image = global::SketchUp.Properties.Resources.FlipHorizontalImage;
            this.cmiFlipHorizontally.Name = "cmiFlipHorizontally";
            this.cmiFlipHorizontally.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.cmiFlipHorizontally.Size = new System.Drawing.Size(286, 26);
            this.cmiFlipHorizontally.Text = "Flip Horizontally";
            // 
            // cmiFlipVeritcally
            // 
            this.cmiFlipVeritcally.Image = global::SketchUp.Properties.Resources.FlipVerticalImage;
            this.cmiFlipVeritcally.Name = "cmiFlipVeritcally";
            this.cmiFlipVeritcally.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.cmiFlipVeritcally.Size = new System.Drawing.Size(286, 26);
            this.cmiFlipVeritcally.Text = "Flip Veritcally";
            // 
            // cmiExitSketch
            // 
            this.cmiExitSketch.Image = global::SketchUp.Properties.Resources.Close_16x;
            this.cmiExitSketch.Name = "cmiExitSketch";
            this.cmiExitSketch.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.X)));
            this.cmiExitSketch.Size = new System.Drawing.Size(286, 26);
            this.cmiExitSketch.Text = "Exit Sketch";
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAdd.Image = global::SketchUp.Properties.Resources.AddSection;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Margin = new System.Windows.Forms.Padding(25, 1, 15, 2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(114, 24);
            this.btnAdd.Text = "Add Section";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUnDo
            // 
            this.btnUnDo.BackColor = System.Drawing.Color.Transparent;
            this.btnUnDo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnDo.Image = global::SketchUp.Properties.Resources.Undo_grey_32x;
            this.btnUnDo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnDo.Margin = new System.Windows.Forms.Padding(10, 1, 15, 2);
            this.btnUnDo.Name = "btnUnDo";
            this.btnUnDo.Size = new System.Drawing.Size(71, 24);
            this.btnUnDo.Text = "UnDo";
            this.btnUnDo.Click += new System.EventHandler(this.UnDoBtn_Click);
            // 
            // btnEditSections
            // 
            this.btnEditSections.Image = global::SketchUp.Properties.Resources.EditImage;
            this.btnEditSections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEditSections.Name = "btnEditSections";
            this.btnEditSections.Size = new System.Drawing.Size(118, 24);
            this.btnEditSections.Text = "Edit Sections";
            // 
            // miFileAddSection
            // 
            this.miFileAddSection.Image = global::SketchUp.Properties.Resources.AddSection;
            this.miFileAddSection.Name = "miFileAddSection";
            this.miFileAddSection.Size = new System.Drawing.Size(267, 26);
            this.miFileAddSection.Text = "Add Section";
            this.miFileAddSection.Click += new System.EventHandler(this.miFileAddSection_Click);
            // 
            // miFileEditSection
            // 
            this.miFileEditSection.Image = global::SketchUp.Properties.Resources.EditImage;
            this.miFileEditSection.Name = "miFileEditSection";
            this.miFileEditSection.Size = new System.Drawing.Size(267, 26);
            this.miFileEditSection.Text = "Edit Sections";
            this.miFileEditSection.Click += new System.EventHandler(this.miFileEditSection_Click);
            // 
            // miFileCloseNoSave
            // 
            this.miFileCloseNoSave.Image = global::SketchUp.Properties.Resources.DeleteListItem_32x;
            this.miFileCloseNoSave.Name = "miFileCloseNoSave";
            this.miFileCloseNoSave.Size = new System.Drawing.Size(267, 26);
            this.miFileCloseNoSave.Text = "Discard Changes and Close ";
            this.miFileCloseNoSave.Click += new System.EventHandler(this.miFileCloseNoSave_Click);
            // 
            // miSaveAndContinue
            // 
            this.miSaveAndContinue.Image = global::SketchUp.Properties.Resources.SaveAndContinue;
            this.miSaveAndContinue.Name = "miSaveAndContinue";
            this.miSaveAndContinue.Size = new System.Drawing.Size(267, 26);
            this.miSaveAndContinue.Text = "Save and Continue";
            this.miSaveAndContinue.Click += new System.EventHandler(this.miSaveAndContinue_Click);
            // 
            // miSaveAndClose
            // 
            this.miSaveAndClose.Image = global::SketchUp.Properties.Resources.Save;
            this.miSaveAndClose.Name = "miSaveAndClose";
            this.miSaveAndClose.Size = new System.Drawing.Size(267, 26);
            this.miSaveAndClose.Text = "Save and Close";
            this.miSaveAndClose.Click += new System.EventHandler(this.miSaveAndClose_Click);
            // 
            // miEditUndoLine
            // 
            this.miEditUndoLine.Image = global::SketchUp.Properties.Resources.Undo_grey_32x;
            this.miEditUndoLine.Name = "miEditUndoLine";
            this.miEditUndoLine.Size = new System.Drawing.Size(210, 26);
            this.miEditUndoLine.Text = "Undo Line";
            // 
            // ExpandoSketch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1022, 676);
            this.Controls.Add(this.SketchStatusBar);
            this.Controls.Add(this.dgSections);
            this.Controls.Add(this.sketchBox);
            this.Controls.Add(this.MainToolStrip);
            this.Controls.Add(this.mnuMain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.mnuMain;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ExpandoSketch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Sketch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExpandoSketch_FormClosing);
            this.Shown += new System.EventHandler(this.ExpandoSketch_Shown);
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.AddSectionContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).EndInit();
            this.SketchStatusBar.ResumeLayout(false);
            this.SketchStatusBar.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainToolStrip;
        private System.Windows.Forms.ContextMenuStrip AddSectionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cmiJumpToCorner;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripLabel DistLbl;
        private System.Windows.Forms.ToolStripTextBox DistText;
        private System.Windows.Forms.ToolStripLabel TextLbl;
        private System.Windows.Forms.ToolStripTextBox FieldText;
        private System.Windows.Forms.ToolStripButton btnUnDo;
        private System.Windows.Forms.PictureBox sketchBox;
        private System.Windows.Forms.DataGridView dgSections;
        private System.Windows.Forms.FontDialog selectFontDlg;
        private System.Windows.Forms.StatusStrip SketchStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel sketchStatusMessage;
        private System.Windows.Forms.ToolStripStatusLabel snapshotIndexLabel;
        private System.Windows.Forms.ToolStripMenuItem cmiBeginDrawing;
        private System.Windows.Forms.ToolStripMenuItem cmiDoneSketching;
        private System.Windows.Forms.ToolStripMenuItem cmiSaveDrawing;
        private System.Windows.Forms.ToolStripSeparator cmiSepLine1;
        private System.Windows.Forms.ToolStripMenuItem cmiFlipHorizontally;
        private System.Windows.Forms.ToolStripMenuItem cmiFlipVeritcally;
        private System.Windows.Forms.ToolStripSeparator cmiSepLine2;
        private System.Windows.Forms.ToolStripMenuItem cmiExitSketch;
        private MenuStrip mnuMain;
        private ToolStripMenuItem miFile;
        private ToolStripMenuItem miEdit;
        private ToolStripMenuItem miSaveAndClose;
        private ToolStripMenuItem miFileCloseNoSave;
        private ToolStripMenuItem miSaveAndContinue;
        private ToolStripMenuItem miFileAddSection;
        private ToolStripMenuItem miEditUndoLine;
        private ToolStripMenuItem miEditUndoSectionLines;
        private ToolStripMenuItem miUndoAddSection;
        private ToolStripSeparator separator1;
        private ToolStripMenuItem miFileEditSection;
        private ToolStripSeparator separator2;
        private ToolStripMenuItem miHelp;
        private ToolStripMenuItem miHelpContact;
        private ToolStripMenuItem miHelpAbout;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripButton btnEditSections;
        private ToolStripMenuItem cmiAddClosingLine;
        // private ToolStripMenuItem cmiExitSketch;
    }
}