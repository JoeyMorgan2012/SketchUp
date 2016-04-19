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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExpandoSketch));
            this.ExpandoSketchTools = new System.Windows.Forms.ToolStrip();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.jumpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beginPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.endSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BeginSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.DistLbl = new System.Windows.Forms.ToolStripLabel();
            this.DistText = new System.Windows.Forms.ToolStripTextBox();
            this.TextBtn = new System.Windows.Forms.ToolStripButton();
            this.TextLbl = new System.Windows.Forms.ToolStripLabel();
            this.FieldText = new System.Windows.Forms.ToolStripTextBox();
            this.UnDoBtn = new System.Windows.Forms.ToolStripButton();
            this.AddSectionBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.autoCloseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chanageSectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteExinstingSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateSketchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AutoCloseBtn = new System.Windows.Forms.ToolStripButton();
            this.dgSections = new System.Windows.Forms.DataGridView();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.ExpSketchPBox = new System.Windows.Forms.PictureBox();
            this.ExpandoSketchTools.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ExpandoSketchTools
            // 
            this.ExpandoSketchTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ExpandoSketchTools.ContextMenuStrip = this.contextMenuStrip1;
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
            this.toolStripDropDownButton1,
            this.AutoCloseBtn});
            this.ExpandoSketchTools.Location = new System.Drawing.Point(0, 0);
            this.ExpandoSketchTools.Name = "ExpandoSketchTools";
            this.ExpandoSketchTools.Size = new System.Drawing.Size(1014, 27);
            this.ExpandoSketchTools.TabIndex = 0;
            this.ExpandoSketchTools.Text = "toolStrip1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToolStripMenuItem,
            this.beginPointToolStripMenuItem,
            this.endSectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(193, 82);
            this.contextMenuStrip1.Click += new System.EventHandler(this.beginPointToolStripMenuItem_Click);
            // 
            // jumpToolStripMenuItem
            // 
            this.jumpToolStripMenuItem.Name = "jumpToolStripMenuItem";
            this.jumpToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.jumpToolStripMenuItem.Text = "Jump";
            this.jumpToolStripMenuItem.Click += new System.EventHandler(this.jumpToolStripMenuItem_Click);
            // 
            // beginPointToolStripMenuItem
            // 
            this.beginPointToolStripMenuItem.Name = "beginPointToolStripMenuItem";
            this.beginPointToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.beginPointToolStripMenuItem.Text = "Begin Point";
            this.beginPointToolStripMenuItem.Visible = false;
            this.beginPointToolStripMenuItem.Click += new System.EventHandler(this.beginPointToolStripMenuItem_Click_1);
            // 
            // endSectionToolStripMenuItem
            // 
            this.endSectionToolStripMenuItem.Name = "endSectionToolStripMenuItem";
            this.endSectionToolStripMenuItem.Size = new System.Drawing.Size(192, 26);
            this.endSectionToolStripMenuItem.Text = "End Section Edit";
            this.endSectionToolStripMenuItem.Click += new System.EventHandler(this.endSectionToolStripMenuItem_Click);
            // 
            // BeginSectionBtn
            // 
            this.BeginSectionBtn.BackColor = System.Drawing.Color.PaleTurquoise;
            this.BeginSectionBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
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
            this.TextBtn.Click += new System.EventHandler(this.TextBtn_Click_1);
            // 
            // TextLbl
            // 
            this.TextLbl.Name = "TextLbl";
            this.TextLbl.Size = new System.Drawing.Size(88, 24);
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
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoCloseToolStripMenuItem,
            this.angleToolStripMenuItem,
            this.viewSectionToolStripMenuItem,
            this.addSectionToolStripMenuItem,
            this.deleteSectionToolStripMenuItem,
            this.chanageSectionToolStripMenuItem,
            this.deleteExinstingSketchToolStripMenuItem,
            this.exportSketchToolStripMenuItem,
            this.rotateSketchToolStripMenuItem});
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0, 1, 10, 2);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(125, 23);
            this.toolStripDropDownButton1.Text = "Tools";
            // 
            // autoCloseToolStripMenuItem
            // 
            this.autoCloseToolStripMenuItem.AutoSize = false;
            this.autoCloseToolStripMenuItem.Name = "autoCloseToolStripMenuItem";
            this.autoCloseToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.autoCloseToolStripMenuItem.Text = "AutoClose";
            this.autoCloseToolStripMenuItem.Visible = false;
            this.autoCloseToolStripMenuItem.Click += new System.EventHandler(this.autoCloseToolStripMenuItem_Click);
            // 
            // angleToolStripMenuItem
            // 
            this.angleToolStripMenuItem.AutoSize = false;
            this.angleToolStripMenuItem.Name = "angleToolStripMenuItem";
            this.angleToolStripMenuItem.Size = new System.Drawing.Size(150, 30);
            this.angleToolStripMenuItem.Text = "Angle";
            this.angleToolStripMenuItem.Visible = false;
            // 
            // viewSectionToolStripMenuItem
            // 
            this.viewSectionToolStripMenuItem.AutoSize = false;
            this.viewSectionToolStripMenuItem.Name = "viewSectionToolStripMenuItem";
            this.viewSectionToolStripMenuItem.Size = new System.Drawing.Size(152, 30);
            this.viewSectionToolStripMenuItem.Text = "Edit Sections";
            this.viewSectionToolStripMenuItem.Click += new System.EventHandler(this.viewSectionsToolStripMenuItem_Click);
            // 
            // addSectionToolStripMenuItem
            // 
            this.addSectionToolStripMenuItem.AutoSize = false;
            this.addSectionToolStripMenuItem.Name = "addSectionToolStripMenuItem";
            this.addSectionToolStripMenuItem.Size = new System.Drawing.Size(138, 30);
            this.addSectionToolStripMenuItem.Text = "AddSection";
            this.addSectionToolStripMenuItem.Visible = false;
            this.addSectionToolStripMenuItem.Click += new System.EventHandler(this.addSectionToolStripMenuItem_Click);
            // 
            // deleteSectionToolStripMenuItem
            // 
            this.deleteSectionToolStripMenuItem.AutoSize = false;
            this.deleteSectionToolStripMenuItem.Name = "deleteSectionToolStripMenuItem";
            this.deleteSectionToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.deleteSectionToolStripMenuItem.Text = "Delete Section";
            this.deleteSectionToolStripMenuItem.Visible = false;
            this.deleteSectionToolStripMenuItem.Click += new System.EventHandler(this.deleteSectionToolStripMenuItem_Click);
            // 
            // chanageSectionToolStripMenuItem
            // 
            this.chanageSectionToolStripMenuItem.AutoSize = false;
            this.chanageSectionToolStripMenuItem.Name = "chanageSectionToolStripMenuItem";
            this.chanageSectionToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.chanageSectionToolStripMenuItem.Text = "Change Section";
            this.chanageSectionToolStripMenuItem.Visible = false;
            this.chanageSectionToolStripMenuItem.Click += new System.EventHandler(this.changeSectionToolStripMenuItem_Click);
            // 
            // deleteExinstingSketchToolStripMenuItem
            // 
            this.deleteExinstingSketchToolStripMenuItem.AutoSize = false;
            this.deleteExinstingSketchToolStripMenuItem.Name = "deleteExinstingSketchToolStripMenuItem";
            this.deleteExinstingSketchToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.deleteExinstingSketchToolStripMenuItem.Text = "Delete Existing Sketch";
            this.deleteExinstingSketchToolStripMenuItem.Click += new System.EventHandler(this.deleteExistingSketchToolStripMenuItem_Click);
            // 
            // exportSketchToolStripMenuItem
            // 
            this.exportSketchToolStripMenuItem.AutoSize = false;
            this.exportSketchToolStripMenuItem.Name = "exportSketchToolStripMenuItem";
            this.exportSketchToolStripMenuItem.Size = new System.Drawing.Size(188, 30);
            this.exportSketchToolStripMenuItem.Text = "Export Sketch";
            this.exportSketchToolStripMenuItem.Visible = false;
            this.exportSketchToolStripMenuItem.Click += new System.EventHandler(this.exportSketchToolStripMenuItem_Click);
            // 
            // rotateSketchToolStripMenuItem
            // 
            this.rotateSketchToolStripMenuItem.Name = "rotateSketchToolStripMenuItem";
            this.rotateSketchToolStripMenuItem.Size = new System.Drawing.Size(238, 26);
            this.rotateSketchToolStripMenuItem.Text = "Flip Sketch";
            this.rotateSketchToolStripMenuItem.Click += new System.EventHandler(this.rotateSketchToolStripMenuItem_Click);
            // 
            // AutoCloseBtn
            // 
            this.AutoCloseBtn.BackColor = System.Drawing.Color.Cyan;
            this.AutoCloseBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.AutoCloseBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.AutoCloseBtn.Image = ((System.Drawing.Image)(resources.GetObject("AutoCloseBtn.Image")));
            this.AutoCloseBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AutoCloseBtn.Margin = new System.Windows.Forms.Padding(0, 1, 15, 2);
            this.AutoCloseBtn.Name = "AutoCloseBtn";
            this.AutoCloseBtn.Size = new System.Drawing.Size(135, 24);
            this.AutoCloseBtn.Text = "< Close Section >";
            this.AutoCloseBtn.ToolTipText = "Close section";
            this.AutoCloseBtn.Click += new System.EventHandler(this.AutoCloseBtn_Click);
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
            this.ExpSketchPBox.ContextMenuStrip = this.contextMenuStrip1;
            this.ExpSketchPBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExpSketchPBox.Location = new System.Drawing.Point(0, 27);
            this.ExpSketchPBox.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ExpSketchPBox.Name = "ExpSketchPBox";
            this.ExpSketchPBox.Size = new System.Drawing.Size(1014, 572);
            this.ExpSketchPBox.TabIndex = 2;
            this.ExpSketchPBox.TabStop = false;
            this.ExpSketchPBox.Click += new System.EventHandler(this.ExpSketchPBox_Click);
            this.ExpSketchPBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseClick);
            this.ExpSketchPBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseDown);
            this.ExpSketchPBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseMove);
            this.ExpSketchPBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ExpSketchPbox_MouseUp);
            // 
            // ExpandoSketch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.ClientSize = new System.Drawing.Size(1014, 599);
            this.Controls.Add(this.dgSections);
            this.Controls.Add(this.ExpSketchPBox);
            this.Controls.Add(this.ExpandoSketchTools);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ExpandoSketch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Sketch";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExpandoSketch_FormClosing);
            this.ExpandoSketchTools.ResumeLayout(false);
            this.ExpandoSketchTools.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpSketchPBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip ExpandoSketchTools;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem jumpToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton BeginSectionBtn;
        private System.Windows.Forms.ToolStripLabel DistLbl;
        private System.Windows.Forms.ToolStripTextBox DistText;
        private System.Windows.Forms.ToolStripButton TextBtn;
        private System.Windows.Forms.ToolStripLabel TextLbl;
        private System.Windows.Forms.ToolStripTextBox FieldText;
        private System.Windows.Forms.ToolStripButton UnDoBtn;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem autoCloseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem angleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chanageSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteExinstingSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSketchToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton AddSectionBtn;
        private System.Windows.Forms.ToolStripButton AutoCloseBtn;
        private System.Windows.Forms.PictureBox ExpSketchPBox;
        private System.Windows.Forms.DataGridView dgSections;
        private System.Windows.Forms.ToolStripMenuItem beginPointToolStripMenuItem;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.ToolStripMenuItem endSectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateSketchToolStripMenuItem;
    }
}