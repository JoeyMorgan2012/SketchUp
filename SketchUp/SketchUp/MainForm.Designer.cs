namespace SketchUp
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.RecordTxt = new System.Windows.Forms.TextBox();
			this.CardTxt = new System.Windows.Forms.TextBox();
			this.SelectRecordBtn = new System.Windows.Forms.Button();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.sketchBox = new System.Windows.Forms.PictureBox();
			this.EditImage = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.LibraryTxt = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.PreFixTxt = new System.Windows.Forms.TextBox();
			this.LocNameTxt = new System.Windows.Forms.TextBox();
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.sketchBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label1.Location = new System.Drawing.Point(154, 168);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(198, 20);
			this.label1.TabIndex = 6;
			this.label1.Text = "Current Parcel Record";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(47, 222);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 17);
			this.label2.TabIndex = 7;
			this.label2.Text = "Record No :";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(65, 256);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(68, 17);
			this.label3.TabIndex = 9;
			this.label3.Text = "Card No :";
			// 
			// RecordTxt
			// 
			this.RecordTxt.AcceptsTab = true;
			this.RecordTxt.Location = new System.Drawing.Point(178, 222);
			this.RecordTxt.Margin = new System.Windows.Forms.Padding(4);
			this.RecordTxt.Name = "RecordTxt";
			this.RecordTxt.Size = new System.Drawing.Size(132, 26);
			this.RecordTxt.TabIndex = 8;
			this.RecordTxt.Leave += new System.EventHandler(this.RecordTxt_Leave);
			// 
			// CardTxt
			// 
			this.CardTxt.AcceptsTab = true;
			this.CardTxt.Location = new System.Drawing.Point(178, 256);
			this.CardTxt.Margin = new System.Windows.Forms.Padding(4);
			this.CardTxt.Name = "CardTxt";
			this.CardTxt.Size = new System.Drawing.Size(132, 26);
			this.CardTxt.TabIndex = 10;
			this.CardTxt.Leave += new System.EventHandler(this.CardTxt_Leave);
			// 
			// SelectRecordBtn
			// 
			this.SelectRecordBtn.Location = new System.Drawing.Point(190, 290);
			this.SelectRecordBtn.Margin = new System.Windows.Forms.Padding(4);
			this.SelectRecordBtn.Name = "SelectRecordBtn";
			this.SelectRecordBtn.Size = new System.Drawing.Size(100, 32);
			this.SelectRecordBtn.TabIndex = 11;
			this.SelectRecordBtn.Text = "Select";
			this.SelectRecordBtn.UseVisualStyleBackColor = true;
			this.SelectRecordBtn.Click += new System.EventHandler(this.SelectRecordBtn_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage});
			this.statusStrip1.Location = new System.Drawing.Point(0, 652);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1051, 22);
			this.statusStrip1.TabIndex = 13;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// statusMessage
			// 
			this.statusMessage.Name = "statusMessage";
			this.statusMessage.Size = new System.Drawing.Size(0, 17);
			// 
			// sketchBox
			// 
			this.sketchBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.sketchBox.Location = new System.Drawing.Point(462, 88);
			this.sketchBox.Margin = new System.Windows.Forms.Padding(4);
			this.sketchBox.Name = "sketchBox";
			this.sketchBox.Size = new System.Drawing.Size(498, 517);
			this.sketchBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.sketchBox.TabIndex = 9;
			this.sketchBox.TabStop = false;
			// 
			// EditImage
			// 
			this.EditImage.Location = new System.Drawing.Point(462, 613);
			this.EditImage.Margin = new System.Windows.Forms.Padding(4);
			this.EditImage.Name = "EditImage";
			this.EditImage.Size = new System.Drawing.Size(100, 32);
			this.EditImage.TabIndex = 12;
			this.EditImage.Text = "Edit Sketch";
			this.EditImage.UseVisualStyleBackColor = true;
			this.EditImage.Click += new System.EventHandler(this.EditImage_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.label4.Location = new System.Drawing.Point(520, 43);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(255, 29);
			this.label4.TabIndex = 1;
			this.label4.Text = "CAMRA SKETCH UP";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(24, 64);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(111, 17);
			this.label5.TabIndex = 2;
			this.label5.Text = "Current Library :";
			// 
			// LibraryTxt
			// 
			this.LibraryTxt.BackColor = System.Drawing.SystemColors.Control;
			this.LibraryTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.LibraryTxt.Cursor = System.Windows.Forms.Cursors.No;
			this.LibraryTxt.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LibraryTxt.Location = new System.Drawing.Point(177, 68);
			this.LibraryTxt.Margin = new System.Windows.Forms.Padding(4);
			this.LibraryTxt.Name = "LibraryTxt";
			this.LibraryTxt.ReadOnly = true;
			this.LibraryTxt.Size = new System.Drawing.Size(133, 19);
			this.LibraryTxt.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(41, 97);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 17);
			this.label6.TabIndex = 4;
			this.label6.Text = "Local Prefix :";
			// 
			// PreFixTxt
			// 
			this.PreFixTxt.BackColor = System.Drawing.SystemColors.Control;
			this.PreFixTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.PreFixTxt.Cursor = System.Windows.Forms.Cursors.No;
			this.PreFixTxt.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PreFixTxt.Location = new System.Drawing.Point(177, 97);
			this.PreFixTxt.Margin = new System.Windows.Forms.Padding(4);
			this.PreFixTxt.Name = "PreFixTxt";
			this.PreFixTxt.ReadOnly = true;
			this.PreFixTxt.Size = new System.Drawing.Size(133, 19);
			this.PreFixTxt.TabIndex = 5;
			// 
			// LocNameTxt
			// 
			this.LocNameTxt.BackColor = System.Drawing.SystemColors.Control;
			this.LocNameTxt.Cursor = System.Windows.Forms.Cursors.No;
			this.LocNameTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LocNameTxt.ForeColor = System.Drawing.SystemColors.ControlText;
			this.LocNameTxt.Location = new System.Drawing.Point(63, 18);
			this.LocNameTxt.Margin = new System.Windows.Forms.Padding(4);
			this.LocNameTxt.Name = "LocNameTxt";
			this.LocNameTxt.ReadOnly = true;
			this.LocNameTxt.Size = new System.Drawing.Size(272, 26);
			this.LocNameTxt.TabIndex = 0;
			this.LocNameTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.BalloonTipTitle = "Sketch_Up";
			this.notifyIcon1.Text = "Info";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
			// 
			// timer
			// 
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightSteelBlue;
			this.ClientSize = new System.Drawing.Size(1051, 674);
			this.Controls.Add(this.LocNameTxt);
			this.Controls.Add(this.PreFixTxt);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.LibraryTxt);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.EditImage);
			this.Controls.Add(this.sketchBox);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.SelectRecordBtn);
			this.Controls.Add(this.CardTxt);
			this.Controls.Add(this.RecordTxt);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MinimumSize = new System.Drawing.Size(1041, 701);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Current CAMRA Parcel";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.sketchBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox RecordTxt;
        private System.Windows.Forms.TextBox CardTxt;
        private System.Windows.Forms.Button SelectRecordBtn;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusMessage;
        private System.Windows.Forms.PictureBox sketchBox;
        private System.Windows.Forms.Button EditImage;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LibraryTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox PreFixTxt;
        private System.Windows.Forms.TextBox LocNameTxt;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.Timer timer;
	}
}

