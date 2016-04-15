using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class DeleteReview : Form
    {
        private Image _mainimage
        {
            get; set;
        }

        private byte[] ms = null;
        private int click = 0;
        private Dictionary<int, byte[]> savpic = null;
        private List<int> curcnt = null;
        private int listcnt = 0;

        public DeleteReview(Dictionary<int, byte[]> curpic, List<int> thiscnt, int thisclick)
        {
            InitializeComponent();

            savpic = curpic;
            curcnt = thiscnt;

            int x = curpic.Count;

            listcnt = curcnt.Count;

            click = thisclick;

            ms = savpic[click];

            ImageNbrTxt.Text = click.ToString();

            ListCountTxt.Text = listcnt.ToString();

            _mainimage = byteArrayToImage(ms);
            DeleteImage.Image = _mainimage;
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        private void nextBtn_Click(object sender, EventArgs e)
        {
            click--;
            ImageNbrTxt.Text = click.ToString();

            curcnt.Remove(listcnt);
            listcnt = curcnt.Max();

            ListCountTxt.Text = listcnt.ToString();

            if (click >= 0)
            {
                ms = savpic[click];

                ImageNbrTxt.Text = click.ToString();

                _mainimage = byteArrayToImage(ms);
                DeleteImage.Image = _mainimage;
            }
        }
    }
}