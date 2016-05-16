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

        

        public DataTable MultiSelectAttachmentPoints(List<SMLine>connectionLines)
        {
    

            MultiplePointsDataTable = new DataTable();
            MultiplePointsDataTable.Columns.Add("Sect", typeof(string));
            MultiplePointsDataTable.Columns.Add("Line", typeof(int));
            MultiplePointsDataTable.Columns.Add("X1", typeof(decimal));
            MultiplePointsDataTable.Columns.Add("Y1", typeof(decimal));
            MultiplePointsDataTable.Columns.Add("X2", typeof(decimal));
            MultiplePointsDataTable.Columns.Add("Y2", typeof(decimal));

            DataSet atpts = null;

            foreach (SMLine line in connectionLines)
            {
                DataRow newRow = MultiplePointsDataTable.NewRow();
                newRow.SetField("Sect", line.SectionLetter);
                newRow.SetField("Line", line.LineNumber);
                newRow.SetField("X1", line.StartX);
                newRow.SetField("Y1", line.StartY);
                newRow.SetField("X2", line.EndX);
                newRow.SetField("Y2", line.EndY);
                MultiplePointsDataTable.Rows.Add(newRow);
            }
            return MultiplePointsDataTable;
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
        public static DataTable MultiplePointsDataTable = null;

        // private CAMRA_Connection _fox = null;

        #endregion Fields

        private void btnSelect_Click(object sender, EventArgs e)
        {
            adjsec = SecLetterCbox.SelectedText;
            
            this.Close();
        }
    }
}