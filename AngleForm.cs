using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        private CamraDataEnums.CardinalDirection angleDirection = CamraDataEnums.CardinalDirection.None;

        public CamraDataEnums.CardinalDirection AngleDirection
        {
            get { return angleDirection; }

            set { angleDirection = value; }
        }

        private void btnSelectDirection_Click(object sender, EventArgs e)
        {
            SetDirectionValue();
            this.Close();
        }


        private void SetDirectionValue()
        {
            var buttons = new List<RadioButton>();
            string selectedTag = "None";
            foreach (Control c in directionGroup.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    buttons.Add((RadioButton)c);
                }
            }
            RadioButton selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Tag.ToString();
            }

            AngleDirection = SMGlobal.DirectionFromString(selectedTag);
        }

    }
}