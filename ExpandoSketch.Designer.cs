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
            this.AddSectionContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmiJumpToCorner = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiSepLine1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmiFlipH = new System.Windows.Forms.ToolStripMenuItem();
            this.cmiFlipV = new System.Windows.Forms.ToolStripMenuItem();
            this.SketchStatusBar = new System.Windows.Forms.StatusStrip();
            this.sketchStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.legalDirectionsLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.sketchBox = new System.Windows.Forms.PictureBox();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUndo = new System.Windows.Forms.Button();
            this.btnDeleteSketch = new System.Windows.Forms.Button();
            this.btnEditSections = new System.Windows.Forms.Button();
            this.FieldText = new System.Windows.Forms.TextBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.DistText = new System.Windows.Forms.TextBox();
            this.distanceLabel = new System.Windows.Forms.Label();
            this.btnAddSection = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.btnAutoClose = new System.Windows.Forms.Button();
            this.btnBegin = new System.Windows.Forms.Button();
            this.cmiFlipVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.AddSectionContextMenu.SuspendLayout();
            this.SketchStatusBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).BeginInit();
            this.buttonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AddSectionContextMenu
            // 
            this.AddSectionContextMenu.BackColor = System.Drawing.SystemColors.Control;
            this.AddSectionContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.AddSectionContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiJumpToCorner,
            this.cmiSepLine1,
            this.cmiFlipH,
            this.cmiFlipV});
            this.AddSectionContextMenu.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.AddSectionContextMenu.Name = "AddSectionContextMenu";
            this.AddSectionContextMenu.Size = new System.Drawing.Size(332, 88);
            // 
            // cmiJumpToCorner
            // 
            this.cmiJumpToCorner.AutoToolTip = true;
            this.cmiJumpToCorner.Enabled = false;
            this.cmiJumpToCorner.Image = global::SketchUp.Properties.Resources.JumpPointImage;
            this.cmiJumpToCorner.Name = "cmiJumpToCorner";
            this.cmiJumpToCorner.ShowShortcutKeys = false;
            this.cmiJumpToCorner.Size = new System.Drawing.Size(331, 26);
            this.cmiJumpToCorner.Text = "Jump to Nearest Corner";
            this.cmiJumpToCorner.ToolTipText = "Jump to Nearest Corner (Add section to Enable";
            this.cmiJumpToCorner.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // cmiSepLine1
            // 
            this.cmiSepLine1.Name = "cmiSepLine1";
            this.cmiSepLine1.Size = new System.Drawing.Size(328, 6);
            // 
            // cmiFlipH
            // 
            this.cmiFlipH.Name = "cmiFlipH";
            this.cmiFlipH.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.cmiFlipH.Size = new System.Drawing.Size(331, 26);
            this.cmiFlipH.Text = "Flip Sketch Horizontally";
            this.cmiFlipH.Click += new System.EventHandler(this.cmiFlipH_Click);
            // 
            // cmiFlipV
            // 
            this.cmiFlipV.Name = "cmiFlipV";
            this.cmiFlipV.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.cmiFlipV.Size = new System.Drawing.Size(331, 26);
            this.cmiFlipV.Text = "Flip Sketch Vertically";
            this.cmiFlipV.Click += new System.EventHandler(this.cmiFlipV_Click);
            // 
            // SketchStatusBar
            // 
            this.SketchStatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.SketchStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sketchStatusMessage,
            this.progressBar,
            this.legalDirectionsLabel});
            this.SketchStatusBar.Location = new System.Drawing.Point(0, 680);
            this.SketchStatusBar.Name = "SketchStatusBar";
            this.SketchStatusBar.Size = new System.Drawing.Size(1126, 24);
            this.SketchStatusBar.TabIndex = 1;
            // 
            // sketchStatusMessage
            // 
            this.sketchStatusMessage.BackColor = System.Drawing.SystemColors.Control;
            this.sketchStatusMessage.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sketchStatusMessage.Name = "sketchStatusMessage";
            this.sketchStatusMessage.Padding = new System.Windows.Forms.Padding(2, 0, 6, 0);
            this.sketchStatusMessage.Size = new System.Drawing.Size(106, 19);
            this.sketchStatusMessage.Text = "Sketch Loaded";
            this.sketchStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar.BackColor = System.Drawing.SystemColors.Control;
            this.progressBar.Maximum = 1000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 19);
            this.progressBar.Step = 20;
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.Visible = false;
            // 
            // legalDirectionsLabel
            // 
            this.legalDirectionsLabel.Name = "legalDirectionsLabel";
            this.legalDirectionsLabel.Size = new System.Drawing.Size(0, 19);
            // 
            // sketchBox
            // 
            this.sketchBox.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.sketchBox.ContextMenuStrip = this.AddSectionContextMenu;
            this.sketchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sketchBox.Location = new System.Drawing.Point(0, 0);
            this.sketchBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.sketchBox.Name = "sketchBox";
            this.sketchBox.Size = new System.Drawing.Size(1126, 704);
            this.sketchBox.TabIndex = 2;
            this.sketchBox.TabStop = false;
            this.sketchBox.VisibleChanged += new System.EventHandler(this.sketchBox_VisibleChanged);
            this.sketchBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseClick);
            this.sketchBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseDown);
            this.sketchBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseUp);
            // 
            // buttonPanel
            // 
            this.buttonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.buttonPanel.Controls.Add(this.btnSave);
            this.buttonPanel.Controls.Add(this.btnUndo);
            this.buttonPanel.Controls.Add(this.btnDeleteSketch);
            this.buttonPanel.Controls.Add(this.btnEditSections);
            this.buttonPanel.Controls.Add(this.FieldText);
            this.buttonPanel.Controls.Add(this.descriptionLabel);
            this.buttonPanel.Controls.Add(this.DistText);
            this.buttonPanel.Controls.Add(this.distanceLabel);
            this.buttonPanel.Controls.Add(this.btnAddSection);
            this.buttonPanel.Controls.Add(this.btnDone);
            this.buttonPanel.Controls.Add(this.btnAutoClose);
            this.buttonPanel.Controls.Add(this.btnBegin);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(1126, 51);
            this.buttonPanel.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Location = new System.Drawing.Point(993, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 30);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUndo.Location = new System.Drawing.Point(602, 10);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(121, 30);
            this.btnUndo.TabIndex = 8;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnDeleteSketch
            // 
            this.btnDeleteSketch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnDeleteSketch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSketch.Location = new System.Drawing.Point(866, 10);
            this.btnDeleteSketch.Name = "btnDeleteSketch";
            this.btnDeleteSketch.Size = new System.Drawing.Size(121, 30);
            this.btnDeleteSketch.TabIndex = 10;
            this.btnDeleteSketch.Text = "Delete Sketch";
            this.btnDeleteSketch.UseVisualStyleBackColor = true;
            this.btnDeleteSketch.Click += new System.EventHandler(this.btnDeleteSketch_Click);
            // 
            // btnEditSections
            // 
            this.btnEditSections.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEditSections.Location = new System.Drawing.Point(729, 10);
            this.btnEditSections.Name = "btnEditSections";
            this.btnEditSections.Size = new System.Drawing.Size(121, 30);
            this.btnEditSections.TabIndex = 9;
            this.btnEditSections.Text = "Edit Sections";
            this.btnEditSections.UseVisualStyleBackColor = true;
            this.btnEditSections.Click += new System.EventHandler(this.btnEditSections_Click);
            // 
            // FieldText
            // 
            this.FieldText.Location = new System.Drawing.Point(279, 10);
            this.FieldText.Name = "FieldText";
            this.FieldText.ReadOnly = true;
            this.FieldText.Size = new System.Drawing.Size(190, 27);
            this.FieldText.TabIndex = 3;
            this.FieldText.TabStop = false;
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(188, 13);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(85, 20);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "Description";
            // 
            // DistText
            // 
            this.DistText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.DistText.Cursor = System.Windows.Forms.Cursors.Default;
            this.DistText.Location = new System.Drawing.Point(81, 10);
            this.DistText.Name = "DistText";
            this.DistText.Size = new System.Drawing.Size(100, 27);
            this.DistText.TabIndex = 1;
            this.DistText.WordWrap = false;
            this.DistText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DistText_KeyDown);
            this.DistText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DistText_KeyPress);
            this.DistText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DistText_KeyUp);
           
            // 
            // distanceLabel
            // 
            this.distanceLabel.AutoSize = true;
            this.distanceLabel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.distanceLabel.Location = new System.Drawing.Point(9, 13);
            this.distanceLabel.Name = "distanceLabel";
            this.distanceLabel.Size = new System.Drawing.Size(66, 20);
            this.distanceLabel.TabIndex = 0;
            this.distanceLabel.Text = "Distance";
            // 
            // btnAddSection
            // 
            this.btnAddSection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddSection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSection.Location = new System.Drawing.Point(475, 10);
            this.btnAddSection.Name = "btnAddSection";
            this.btnAddSection.Size = new System.Drawing.Size(121, 30);
            this.btnAddSection.TabIndex = 4;
            this.btnAddSection.Text = "Add Section";
            this.btnAddSection.UseVisualStyleBackColor = true;
            this.btnAddSection.Click += new System.EventHandler(this.btnAddSection_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(475, 10);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(121, 30);
            this.btnDone.TabIndex = 5;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // btnAutoClose
            // 
            this.btnAutoClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAutoClose.Location = new System.Drawing.Point(475, 10);
            this.btnAutoClose.Name = "btnAutoClose";
            this.btnAutoClose.Size = new System.Drawing.Size(121, 30);
            this.btnAutoClose.TabIndex = 6;
            this.btnAutoClose.Text = "Auto-Close";
            this.btnAutoClose.UseVisualStyleBackColor = true;
            this.btnAutoClose.Click += new System.EventHandler(this.btnAutoClose_Click);
            // 
            // btnBegin
            // 
            this.btnBegin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBegin.Location = new System.Drawing.Point(475, 10);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(121, 30);
            this.btnBegin.TabIndex = 7;
            this.btnBegin.Text = "Begin";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // cmiFlipVertically
            // 
            this.cmiFlipVertically.Image = global::SketchUp.Properties.Resources.FlipVerticalImage;
            this.cmiFlipVertically.Name = "cmiFlipVertically";
            this.cmiFlipVertically.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.cmiFlipVertically.Size = new System.Drawing.Size(286, 26);
            this.cmiFlipVertically.Text = "Flip Veritcally";
            // 
            // ExpandoSketch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1126, 704);
            this.ContextMenuStrip = this.AddSectionContextMenu;
            this.Controls.Add(this.buttonPanel);
            this.Controls.Add(this.SketchStatusBar);
            this.Controls.Add(this.sketchBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ExpandoSketch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Sketch";
            this.Activated += new System.EventHandler(this.ExpandoSketch_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExpandoSketch_FormClosing);
            this.Load += new System.EventHandler(this.ExpandoSketch_Load);
            this.Shown += new System.EventHandler(this.ExpandoSketch_Shown);
            this.AddSectionContextMenu.ResumeLayout(false);
            this.SketchStatusBar.ResumeLayout(false);
            this.SketchStatusBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sketchBox)).EndInit();
            this.buttonPanel.ResumeLayout(false);
            this.buttonPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip AddSectionContextMenu;
        private System.Windows.Forms.ToolStripMenuItem cmiJumpToCorner;
        private System.Windows.Forms.PictureBox sketchBox;
        private System.Windows.Forms.StatusStrip SketchStatusBar;
        private System.Windows.Forms.ToolStripStatusLabel sketchStatusMessage;
        private System.Windows.Forms.ToolStripSeparator cmiSepLine1;
        private ToolStripProgressBar progressBar;
        private Panel buttonPanel;
        private Button btnAddSection;
        private Button btnEditSections;
        private TextBox FieldText;
        private Label descriptionLabel;
        private TextBox DistText;
        private Label distanceLabel;
        private Button btnAutoClose;
        private Button btnDeleteSketch;
        private Button btnBegin;
        private Button btnUndo;
        private Button btnSave;
        private ToolStripMenuItem cmiFlipVertically;
        private Button btnDone;
        private ToolStripMenuItem cmiFlipH;
        private ToolStripMenuItem cmiFlipV;
        private ToolStripStatusLabel legalDirectionsLabel;
        // private ToolStripMenuItem cmiExitSketch;
    }
}