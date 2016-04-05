using System;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
	public partial class FormConfigure : Form
	{
		public static string IPAddress
		{
			get; set;
		}

		public static string LocPreFix
		{
			get; set;
		}

		public FormConfigure()
		{
			InitializeComponent();
		}

		private void FormConfigure_Load(object sender, EventArgs e)
		{
			IPAddress = txtIPAddess.Text.ToString().Trim();
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			//IPAddress = txtIPAddess.Text.ToString().Trim();

			this.Close();
		}

		private void FormConfigure_Leave(object sender, EventArgs e)
		{
			//IPAddress = txtIPAddess.Text.ToString().Trim();

			this.Close();
		}

		private void txtIPAddess_Leave(object sender, EventArgs e)
		{
			IPAddress = txtIPAddess.Text.ToString().Trim();
			LocPrefixTxt.Focus();
		}

		private void LocPrefixTxt_Leave(object sender, EventArgs e)
		{
			LocPreFix = LocPrefixTxt.Text.ToString().Trim();
			btnSave.Focus();
		}

		private void btnSave_Click_1(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}