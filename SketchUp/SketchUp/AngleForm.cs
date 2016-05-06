﻿using System;
using System.Text;
using System.Windows.Forms;
using SketchUp;

namespace SketchUp

{
    public partial class AngleForm : Form
    {
        public AngleForm()
        {
            InitializeComponent();
           

        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private ExpandoSketch.MoveDirections angleDirection=ExpandoSketch.MoveDirections.None;

        public ExpandoSketch.MoveDirections AngleDirection
        {
            get
            {
                return angleDirection;
            }

            set
            {
                angleDirection = value;
            }
        }

        private void btnSelectDirection_Click(object sender, EventArgs e)
        {
            SetDirectionValue();
            this.Close();
        }

        private void SetDirectionValue()
        {
            if (rbNW.Checked == true)
            {
                AngleDirection = ExpandoSketch.MoveDirections.NW;
                
            }

            if (rbNE.Checked == true)
            {
                AngleDirection = ExpandoSketch.MoveDirections.NE;
                
            }

            if (rbSW.Checked == true)
            {
                AngleDirection = ExpandoSketch.MoveDirections.SW;
            }

            if (rbSE.Checked == true)
            {
                AngleDirection = ExpandoSketch.MoveDirections.SE;
            }
        }
    }
}