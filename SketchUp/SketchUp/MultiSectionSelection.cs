using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SWallTech;

namespace SketchUp
{
    public partial class MultiSectionSelection : Form
    {
        #region Constructors

      

        public MultiSectionSelection(List<string> sectionLetters)
        {
            InitializeComponent();
            SecLetterCbox.Items.AddRange(sectionLetters.ToArray());
            SecLetterCbox.SelectedIndex = 0;
            adjsec = SecLetterCbox.SelectedItem.ToString();
        }

        #endregion Constructors

        #region Refactored and Class Methods

        

        private DataSet MultiSelectAttachmentPoints(List<SMLine>connectionLines)
        {
    

            mulattpts = new DataTable();
            mulattpts.Columns.Add("Sect", typeof(string));
            mulattpts.Columns.Add("Line", typeof(int));
            mulattpts.Columns.Add("X1", typeof(decimal));
            mulattpts.Columns.Add("Y1", typeof(decimal));
            mulattpts.Columns.Add("X2", typeof(decimal));
            mulattpts.Columns.Add("Y2", typeof(decimal));

            DataSet atpts = null;

            foreach (SMLine line in connectionLines)
            {
                
            }
            return atpts;
        }

      

        #endregion Refactored and Class Methods

        #region Event and Control Methods

        private void MultiSectionSelection_FormClosing(object sender, FormClosingEventArgs e)
        {
            adjsec = SecLetterCbox.SelectedItem.ToString();
        }

        private void SecLetterCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecLetterCbox.SelectedIndex > 0)
            {
                adjsec = SecLetterCbox.SelectedItem.ToString();
            }
        }

        #endregion Event and Control Methods

        #region Fields

        public static string adjsec = String.Empty;
        public static List<string> attsec = new List<string>();
        public static DataTable mulattpts = null;

       // private CAMRA_Connection _fox = null;

        #endregion Fields
    }
}