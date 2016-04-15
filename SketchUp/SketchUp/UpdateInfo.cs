using System;
using System.Deployment.Application;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class UpdateInfo : Form
    {
        public UpdateInfo()
        {
            InitializeComponent();
            DisplayVersion();
        }

        private void DisplayVersion()
        {
            StringBuilder sb = new StringBuilder();
            string versionNo = this.ProductVersion;
            sb.AppendLine(string.Format("Update check completed at {0} on {1}", DateTime.Now.ToShortTimeString(), DateTime.Now.ToShortDateString()));
            sb.AppendLine();
            sb.AppendLine(string.Format("Currently running version {0}", GetTheVersion()));

            //	Program.ShowCheckpointLog();
            rtbVersionInfo.Text = sb.ToString();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private string GetTheVersion()
        {
            string version = string.Empty;
            Version currentVersion;
            Version updateVersion;
            StringBuilder sb = new StringBuilder();
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //	currentVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                updateVersion = ApplicationDeployment.CurrentDeployment.UpdatedVersion;

                //sb.AppendLine(string.Format("Current Version: {0}.{1}.{2}.{3}", currentVersion.Major.ToString(), currentVersion.Minor.ToString(), currentVersion.MajorRevision.ToString(), currentVersion.MinorRevision.ToString()));
                sb.AppendLine(string.Format("Updated Version: {0}.{1}.{2}.{3}", updateVersion.Major.ToString(), updateVersion.Minor.ToString(), updateVersion.MajorRevision.ToString(), updateVersion.MinorRevision.ToString()));
                version = sb.ToString();
            }
            else
            {
                currentVersion = Assembly.GetCallingAssembly().GetName().Version;
                version = string.Format("Current Version: {0}.{1}.{2}.{3}", currentVersion.Major.ToString(), currentVersion.Minor.ToString(), currentVersion.MajorRevision.ToString(), currentVersion.MinorRevision.ToString());
            }

            return version;
        }
    }
}