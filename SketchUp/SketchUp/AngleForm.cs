using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SketchUp;

namespace SketchUp

{
    public partial class AngleForm : Form
    {
        public AngleForm()
        {
            InitializeComponent();
           

        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private CamraDataEnums.CardinalDirection angleDirection=CamraDataEnums.CardinalDirection.None;

        public CamraDataEnums.CardinalDirection AngleDirection
        {
            get
            {
                return angleDirection;
            }

            set
            {
                angleDirection = value;
            }
        }

        private void btnSelectDirection_Click(object sender, EventArgs e)
        {
           
            SetDirectionValue();
            this.Close();
        }

        private void ShowSelectedRadioButton()
        {
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "No selection";
            foreach (Control c in directionGroup.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Tag.ToString();
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            MessageBox.Show(result.ToString());
        }

        private void SetDirectionValue()
        {
            List<RadioButton> buttons = new List<RadioButton>();
            string selectedTag = "None";
            foreach (Control c in directionGroup.Controls)
            {
                if (c.GetType() == typeof(RadioButton))
                {
                    buttons.Add((RadioButton)c);
                }
            }
            var selectedRb = (from rb in buttons where rb.Checked == true select rb).FirstOrDefault();
            if (selectedRb != null)
            {
                selectedTag = selectedRb.Tag.ToString();
            }

            FormattableString result = $"Selected Radio button tag ={selectedTag}";
            AngleDirection = SMGlobal.DirectionFromString(selectedTag);
#if DEBUG
            Trace.WriteLine(result.ToString());
            Console.WriteLine(result.ToString());
            Trace.Flush();
#endif
            //if (rbNW.Checked == true)
            //{
            //    AngleDirection = CamraDataEnums.CardinalDirection.NW;

            //}

            //if (rbNE.Checked == true)
            //{
            //    AngleDirection = CamraDataEnums.CardinalDirection.NE;

            //}

            //if (rbSW.Checked == true)
            //{
            //    AngleDirection = CamraDataEnums.CardinalDirection.SW;
            //}

            //if (rbSE.Checked == true)
            //{
            //    AngleDirection = CamraDataEnums.CardinalDirection.SE;
            //}
        }

     

       
    }
}