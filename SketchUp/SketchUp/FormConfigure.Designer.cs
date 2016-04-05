namespace SketchUp
{
    partial class FormConfigure
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
            this.txtIPAddess = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LocPrefixTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtIPAddess
            // 
            this.txtIPAddess.Location = new System.Drawing.Point(154, 36);
            this.txtIPAddess.Name = "txtIPAddess";
            this.txtIPAddess.Size = new System.Drawing.Size(148, 20);
            this.txtIPAddess.TabIndex = 0;
            this.txtIPAddess.Leave += new System.EventHandler(this.txtIPAddess_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP Address:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(154, 128);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(58, 24);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Select";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Locality Prefix :";
            // 
            // LocPrefixTxt
            // 
            this.LocPrefixTxt.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LocPrefixTxt.Location = new System.Drawing.Point(154, 84);
            this.LocPrefixTxt.Name = "LocPrefixTxt";
            this.LocPrefixTxt.Size = new System.Drawing.Size(79, 20);
            this.LocPrefixTxt.TabIndex = 4;
            this.LocPrefixTxt.Leave += new System.EventHandler(this.LocPrefixTxt_Leave);
            // 
            // FormConfigure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.ClientSize = new System.Drawing.Size(390, 198);
            this.Controls.Add(this.LocPrefixTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIPAddess);
            this.Name = "FormConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configure";
            this.Leave += new System.EventHandler(this.FormConfigure_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtIPAddess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox LocPrefixTxt;
    }
}