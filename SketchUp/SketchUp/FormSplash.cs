using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class FormSplash : Form
    {
        public FormSplash()
        {
            InitializeComponent();
        }

        public void UpdateProgress()
        {
            int newValue = ((loadingProgBar.Maximum - loadingProgBar.Value) / 4) + loadingProgBar.Value;
            loadingProgBar.Value = newValue;
            loadingProgBar.Update();
            this.Update();
        }

        public void UpdateProgress(int newValue)
        {
            loadingProgBar.Value = newValue;
            loadingProgBar.Update();
            this.Update();
        }

        private void FormSplash_Load(object sender, EventArgs e)
        {
            loadingProgBar.Minimum = 0;
            loadingProgBar.Maximum = 100;
        }
    }
}