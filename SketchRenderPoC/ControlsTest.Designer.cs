namespace SketchUp
{
    partial class ControlsTest
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
            this.cboTest = new System.Windows.Forms.ComboBox();
            this.valueLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboTest
            // 
            this.cboTest.FormattingEnabled = true;
            this.cboTest.Location = new System.Drawing.Point(254, 113);
            this.cboTest.Name = "cboTest";
            this.cboTest.Size = new System.Drawing.Size(631, 24);
            this.cboTest.TabIndex = 0;
            this.cboTest.SelectedIndexChanged += new System.EventHandler(this.cboTest_SelectedIndexChanged);
            // 
            // valueLabel
            // 
            this.valueLabel.AutoSize = true;
            this.valueLabel.Location = new System.Drawing.Point(114, 317);
            this.valueLabel.Name = "valueLabel";
            this.valueLabel.Size = new System.Drawing.Size(44, 17);
            this.valueLabel.TabIndex = 1;
            this.valueLabel.Text = "Value";
            // 
            // ControlsTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 448);
            this.Controls.Add(this.valueLabel);
            this.Controls.Add(this.cboTest);
            this.Name = "ControlsTest";
            this.Text = "ControlsTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboTest;
        private System.Windows.Forms.Label valueLabel;
    }
}