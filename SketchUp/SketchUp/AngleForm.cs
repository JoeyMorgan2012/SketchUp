using System;
using System.Text;
using System.Windows.Forms;

namespace SketchUp

{
    public partial class AngleForm : Form
    {
        public static bool NorthEast = false;
        public static bool NorthWest = false;

        public static bool SouthEast = false;
        public static bool SouthWest = false;

        public AngleForm()
        {
            InitializeComponent();

            if (NWcbox.Checked == true)
            {
                NorthWest = true;
            }

            if (NEcbox.Checked == true)
            {
                NorthEast = true;
            }

            if (SWcbox.Checked == true)
            {
                SouthWest = true;
            }

            if (SEcbox.Checked == true)
            {
                SouthEast = true;
            }
        }

        private void NEcbox_CheckedChanged(object sender, EventArgs e)
        {
            NorthEast = true;

            this.Close();
        }

        private void NWcbox_CheckedChanged(object sender, EventArgs e)
        {
            NorthWest = true;
            this.Close();
        }

        private void SEcbox_CheckedChanged(object sender, EventArgs e)
        {
            SouthEast = true;
            this.Close();
        }

        private void SWcbox_CheckedChanged(object sender, EventArgs e)
        {
            SouthWest = true;
            this.Close();
        }
    }
}