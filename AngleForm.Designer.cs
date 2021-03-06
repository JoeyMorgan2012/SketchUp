﻿namespace SketchUp
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
            this.directionGroup = new System.Windows.Forms.GroupBox();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.rbSW = new System.Windows.Forms.RadioButton();
            this.rbSE = new System.Windows.Forms.RadioButton();
            this.rbNW = new System.Windows.Forms.RadioButton();
            this.rbNE = new System.Windows.Forms.RadioButton();
            this.btnSelectDirection = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.directionGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // directionGroup
            // 
            this.directionGroup.Controls.Add(this.rbNone);
            this.directionGroup.Controls.Add(this.rbSW);
            this.directionGroup.Controls.Add(this.rbSE);
            this.directionGroup.Controls.Add(this.rbNW);
            this.directionGroup.Controls.Add(this.rbNE);
            this.directionGroup.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.directionGroup.Location = new System.Drawing.Point(18, 29);
            this.directionGroup.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.directionGroup.Name = "directionGroup";
            this.directionGroup.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.directionGroup.Size = new System.Drawing.Size(269, 253);
            this.directionGroup.TabIndex = 0;
            this.directionGroup.TabStop = false;
            this.directionGroup.Text = "( Distances N/S , E/W )";
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNone.Location = new System.Drawing.Point(65, 38);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(83, 27);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Tag = "rbNone";
            this.rbNone.Text = "(None)";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // rbSW
            // 
            this.rbSW.AutoSize = true;
            this.rbSW.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSW.Location = new System.Drawing.Point(65, 202);
            this.rbSW.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbSW.Name = "rbSW";
            this.rbSW.Size = new System.Drawing.Size(121, 27);
            this.rbSW.TabIndex = 4;
            this.rbSW.TabStop = true;
            this.rbSW.Tag = "SW";
            this.rbSW.Text = "South-West";
            this.rbSW.UseVisualStyleBackColor = true;
            // 
            // rbSE
            // 
            this.rbSE.AutoSize = true;
            this.rbSE.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSE.Location = new System.Drawing.Point(65, 159);
            this.rbSE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbSE.Name = "rbSE";
            this.rbSE.Size = new System.Drawing.Size(114, 27);
            this.rbSE.TabIndex = 3;
            this.rbSE.TabStop = true;
            this.rbSE.Tag = "SE";
            this.rbSE.Text = "South-East";
            this.rbSE.UseVisualStyleBackColor = true;
            // 
            // rbNW
            // 
            this.rbNW.AutoSize = true;
            this.rbNW.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNW.Location = new System.Drawing.Point(65, 117);
            this.rbNW.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbNW.Name = "rbNW";
            this.rbNW.Size = new System.Drawing.Size(121, 27);
            this.rbNW.TabIndex = 2;
            this.rbNW.TabStop = true;
            this.rbNW.Tag = "NW";
            this.rbNW.Text = "North-West";
            this.rbNW.UseVisualStyleBackColor = true;
            // 
            // rbNE
            // 
            this.rbNE.AutoSize = true;
            this.rbNE.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbNE.Location = new System.Drawing.Point(65, 74);
            this.rbNE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.rbNE.Name = "rbNE";
            this.rbNE.Size = new System.Drawing.Size(114, 27);
            this.rbNE.TabIndex = 1;
            this.rbNE.TabStop = true;
            this.rbNE.Tag = "NE";
            this.rbNE.Text = "North-East";
            this.rbNE.UseVisualStyleBackColor = true;
            // 
            // btnSelectDirection
            // 
            this.btnSelectDirection.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSelectDirection.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectDirection.Location = new System.Drawing.Point(216, 312);
            this.btnSelectDirection.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSelectDirection.Name = "btnSelectDirection";
            this.btnSelectDirection.Size = new System.Drawing.Size(73, 39);
            this.btnSelectDirection.TabIndex = 2;
            this.btnSelectDirection.Text = "Select";
            this.btnSelectDirection.UseVisualStyleBackColor = true;
            this.btnSelectDirection.Click += new System.EventHandler(this.btnSelectDirection_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(20, 312);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(73, 39);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // AngleForm
            // 
            this.AcceptButton = this.btnSelectDirection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(304, 369);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelectDirection);
            this.Controls.Add(this.directionGroup);
            this.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "AngleForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Angle Moves ";
            this.directionGroup.ResumeLayout(false);
            this.directionGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox directionGroup;
        private System.Windows.Forms.RadioButton rbNE;
        private System.Windows.Forms.RadioButton rbSW;
        private System.Windows.Forms.RadioButton rbSE;
        private System.Windows.Forms.RadioButton rbNW;
        private System.Windows.Forms.Button btnSelectDirection;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.RadioButton rbNone;
    }
}