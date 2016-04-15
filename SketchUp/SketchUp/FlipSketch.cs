using System;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class FlipSketch : Form
    {
        public static bool FrontBack = false;
        public static bool RightLeft = false;

        public FlipSketch()
        {
            InitializeComponent();
        }

        private void RLBtn_Click(object sender, EventArgs e)
        {
            RightLeft = true;
            FrontBack = false;
            this.Close();
        }

        private void FRBtn_Click(object sender, EventArgs e)
        {
            RightLeft = false;
            FrontBack = true;
            this.Close();
        }
    }
}