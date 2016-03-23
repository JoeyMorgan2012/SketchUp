namespace SketchUp
{
    partial class AngleForm
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
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NEcbox = new System.Windows.Forms.CheckBox();
            this.NWcbox = new System.Windows.Forms.CheckBox();
            this.SEcbox = new System.Windows.Forms.CheckBox();
            this.SWcbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(176, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "( Distances N/S , E/W )";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(59, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "North-East";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(59, 126);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "North-West";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(59, 183);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "South-East";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(59, 239);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "South-West";
            // 
            // NEcbox
            // 
            this.NEcbox.AutoSize = true;
            this.NEcbox.Location = new System.Drawing.Point(202, 73);
            this.NEcbox.Margin = new System.Windows.Forms.Padding(4);
            this.NEcbox.Name = "NEcbox";
            this.NEcbox.Size = new System.Drawing.Size(15, 14);
            this.NEcbox.TabIndex = 1;
            this.NEcbox.UseVisualStyleBackColor = true;
            this.NEcbox.CheckedChanged += new System.EventHandler(this.NEcbox_CheckedChanged);
            // 
            // NWcbox
            // 
            this.NWcbox.AutoSize = true;
            this.NWcbox.Location = new System.Drawing.Point(202, 130);
            this.NWcbox.Margin = new System.Windows.Forms.Padding(4);
            this.NWcbox.Name = "NWcbox";
            this.NWcbox.Size = new System.Drawing.Size(15, 14);
            this.NWcbox.TabIndex = 2;
            this.NWcbox.UseVisualStyleBackColor = true;
            this.NWcbox.CheckedChanged += new System.EventHandler(this.NWcbox_CheckedChanged);
            // 
            // SEcbox
            // 
            this.SEcbox.AutoSize = true;
            this.SEcbox.Location = new System.Drawing.Point(202, 187);
            this.SEcbox.Margin = new System.Windows.Forms.Padding(4);
            this.SEcbox.Name = "SEcbox";
            this.SEcbox.Size = new System.Drawing.Size(15, 14);
            this.SEcbox.TabIndex = 3;
            this.SEcbox.UseVisualStyleBackColor = true;
            this.SEcbox.CheckedChanged += new System.EventHandler(this.SEcbox_CheckedChanged);
            // 
            // SWcbox
            // 
            this.SWcbox.AutoSize = true;
            this.SWcbox.Location = new System.Drawing.Point(202, 243);
            this.SWcbox.Margin = new System.Windows.Forms.Padding(4);
            this.SWcbox.Name = "SWcbox";
            this.SWcbox.Size = new System.Drawing.Size(15, 14);
            this.SWcbox.TabIndex = 4;
            this.SWcbox.UseVisualStyleBackColor = true;
            this.SWcbox.CheckedChanged += new System.EventHandler(this.SWcbox_CheckedChanged);
            // 
            // AngelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(266, 300);
            this.Controls.Add(this.SWcbox);
            this.Controls.Add(this.SEcbox);
            this.Controls.Add(this.NWcbox);
            this.Controls.Add(this.NEcbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AngelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Angle Moves ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox NEcbox;
        private System.Windows.Forms.CheckBox NWcbox;
        private System.Windows.Forms.CheckBox SEcbox;
        private System.Windows.Forms.CheckBox SWcbox;
    }
}