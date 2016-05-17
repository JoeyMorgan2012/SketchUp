using System;
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

        private CamraDataEnums.CardinalDirection angleDirection=CamraDataEnums.CardinalDirection.None;

        public CamraDataEnums.CardinalDirection AngleDirection
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
                AngleDirection = CamraDataEnums.CardinalDirection.NW;
                
            }

            if (rbNE.Checked == true)
            {
                AngleDirection = CamraDataEnums.CardinalDirection.NE;
                
            }

            if (rbSW.Checked == true)
            {
                AngleDirection = CamraDataEnums.CardinalDirection.SW;
            }

            if (rbSE.Checked == true)
            {
                AngleDirection = CamraDataEnums.CardinalDirection.SE;
            }
        }
    }
}