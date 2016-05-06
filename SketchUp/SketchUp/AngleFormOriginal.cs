using System;
using System.Text;
using System.Windows.Forms;

namespace SketchUp

{
    public partial class AngleFormOriginal : Form
    {
        public AngleFormOriginal()
        {
            InitializeComponent();
           

        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NorthEast = false;
            NorthWest = false;
            SouthEast = false;
            SouthWest = false;
            this.Close();
        }

        public static bool NorthEast = false;
        public static bool NorthWest = false;

        public static bool SouthEast = false;
        public static bool SouthWest = false;

        private void btnSelectDirection_Click(object sender, EventArgs e)
        {
            SetDirectionValue();
            this.Close();
        }

        private void SetDirectionValue()
        {
            if (rbNW.Checked == true)
            {
                NorthWest = true;
            }

            if (rbNE.Checked == true)
            {
                NorthEast = true;
            }

            if (rbSW.Checked == true)
            {
                SouthWest = true;
            }

            if (rbSE.Checked == true)
            {
                SouthEast = true;
            }
        }
    }
}