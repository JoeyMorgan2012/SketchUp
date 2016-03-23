using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CamraSketch
{
    public partial class TestOutPut : Form
    {
        ParcelData _currentParcel = null;

        public TestOutPut(ParcelData data)
        {
            InitializeComponent();

            _currentParcel = data;

            LibraryNameTxt.Text = MainForm.localLib.ToString().Trim();
            FilePrefixTxt.Text = MainForm.localPreFix.ToString().Trim();

            RecordNoTxt.Text = MainForm.Record.ToString();
            CardNoTxt.Text = MainForm.Card.ToString();

            NameTxt.Text = _currentParcel.mlnam.ToString().Trim();

        }
    }
}
