namespace SketchUp
{
    partial class DeleteReview
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
            this.DeleteImage = new System.Windows.Forms.PictureBox();
            this.ImageNbrTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nextBtn = new System.Windows.Forms.Button();
            this.ListCountTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DeleteImage)).BeginInit();
            this.SuspendLayout();
            // 
            // DeleteImage
            // 
            this.DeleteImage.Location = new System.Drawing.Point(15, 49);
            this.DeleteImage.Name = "DeleteImage";
            this.DeleteImage.Size = new System.Drawing.Size(1167, 600);
            this.DeleteImage.TabIndex = 0;
            this.DeleteImage.TabStop = false;
            // 
            // ImageNbrTxt
            // 
            this.ImageNbrTxt.Location = new System.Drawing.Point(82, 13);
            this.ImageNbrTxt.Name = "ImageNbrTxt";
            this.ImageNbrTxt.Size = new System.Drawing.Size(100, 20);
            this.ImageNbrTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Image";
            // 
            // nextBtn
            // 
            this.nextBtn.Location = new System.Drawing.Point(457, 13);
            this.nextBtn.Name = "nextBtn";
            this.nextBtn.Size = new System.Drawing.Size(75, 23);
            this.nextBtn.TabIndex = 3;
            this.nextBtn.Text = "Enter";
            this.nextBtn.UseVisualStyleBackColor = true;
            this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
            // 
            // ListCountTxt
            // 
            this.ListCountTxt.Location = new System.Drawing.Point(306, 13);
            this.ListCountTxt.Name = "ListCountTxt";
            this.ListCountTxt.Size = new System.Drawing.Size(100, 20);
            this.ListCountTxt.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "List Count";
            // 
            // DeleteReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1203, 692);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ListCountTxt);
            this.Controls.Add(this.nextBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImageNbrTxt);
            this.Controls.Add(this.DeleteImage);
            this.Name = "DeleteReview";
            this.Text = "Delete Review";
            ((System.ComponentModel.ISupportInitialize)(this.DeleteImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox DeleteImage;
        private System.Windows.Forms.TextBox ImageNbrTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.TextBox ListCountTxt;
        private System.Windows.Forms.Label label2;
    }
}