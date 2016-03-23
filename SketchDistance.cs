using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
	public partial class SketchDistance : Form
	{
		public SketchDistance(string EWdir, decimal EWdist, string NSdir, decimal NSdist)
		{
			InitializeComponent();

			EWDirectTxt.Text = String.Empty;
			EWDistanceTxt.Text = String.Empty;
			NSDirectTxt.Text = String.Empty;
			NSDistanceTxt.Text = String.Empty;

			Rectangle r = Screen.PrimaryScreen.WorkingArea;
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (this.Width + 50),
				Screen.PrimaryScreen.WorkingArea.Height - (this.Height + 50));

			EWDirectTxt.Text = EWdir.Trim();
			EWDistanceTxt.Text = EWdist.ToString("N1");
			NSDirectTxt.Text = NSdir.Trim();
			NSDistanceTxt.Text = NSdist.ToString("N1");

			if (NSdist == 0 && EWdist == 0)
			{
				this.Close();
			}
		}

		protected override bool ShowWithoutActivation
		{
			get
			{
				return false;
			}
		}
	}
}