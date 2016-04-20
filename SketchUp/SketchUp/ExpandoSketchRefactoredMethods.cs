using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class ExpandoSketch : Form
    {
        #region Form Events and Menu

        #region May not be needed
        private void AutoClose()
        {
            PromptToSaveOrDiscard();

            //Cursor = Cursors.WaitCursor;

            //savcnt = new List<int>();
            //savpic = curpic;

            //_isClosed = true;

            //string stx = _nextSectLtr;

            //click = curclick;

            //float tx1 = NextStartX;

            //float ty1 = NextStartY;

            ExpSketchPBox.Image = _mainimage;
            //click++;
            //savpic.Add(click, imageToByteArray(_mainimage));

            //foreach (KeyValuePair<int, byte[]> pair in savpic)
            //{
            //    savcnt.Add(pair.Key);
            //}

            //finalClick = click;

            //_isclosing = true;



            //_addSection = false;

            //computeArea();

            //AddSectionSQL(finalDirect, finalDistanceF);

            //string finalDesc = String.Format("{0}, {1} sf",
            //    FieldText.Text.Trim(),
            //    _nextSectArea.ToString("N1"));

            //FieldText.Text = finalDesc.Trim();

            //ExpSketchPBox.Image = _mainimage;

            //sortSection();

            //setAttPnts();

            //Cursor = Cursors.Default;

            //this.Close();
        } 
        #endregion


        //ToDo: Begin here to hook in parcel updates
        private void DoneDrawingBtn_Click(object sender, EventArgs e)
        {
            ReorderParcelStructure();
            RefreshParcelImage();
            SetActiveButtonAppearance();
        }

        private void RefreshParcelImage()
        {
            throw new NotImplementedException();
        }

        private void ReorderParcelStructure()
        {
            throw new NotImplementedException();
        }

        private void endSectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        #endregion Form Events and Menu

        #region Save or Discard Changes Refactored

        private void DiscardChangesAndExit()
        {
            SketchUpGlobals.SketchSnapshots.Clear();
            SketchUpGlobals.SMParcelFromData.SnapShotIndex = 0;
            SketchUpGlobals.SketchSnapshots.Add(SketchUpGlobals.SMParcelFromData);

            MessageBox.Show(
                string.Format("Reverting to Version {0} with {1} Sections.",
                SketchUpGlobals.SMParcelFromData.SnapShotIndex,
                SketchUpGlobals.SMParcelFromData.Sections.Count));

            this.Close();
        }

        private void PromptToSaveOrDiscard()
        {
            string message = "Do you want to save changes?";
            DialogResult response = MessageBox.Show(message, "Save Changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            switch (response)
            {
                case DialogResult.Cancel:
                case DialogResult.None:

                    // Do we need to do anything here?
                    break;

                case DialogResult.Yes:
                    SaveCurrentParcelToDatabaseAndExit();

                    break;

                case DialogResult.No:
                    DiscardChangesAndExit();
                    break;

                default:
                    break;
            }
        }

        private void SaveCurrentParcelToDatabaseAndExit()
        {
            Reorder();
            MessageBox.Show(
                string.Format("Saving Version {0} with {1} Sections to Database.",
                SketchUpGlobals.ParcelWorkingCopy.SnapShotIndex,
                SketchUpGlobals.ParcelWorkingCopy.Sections.Count));
            this.Close();
        }

        private void SetActiveButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.Cyan;
            BeginSectionBtn.Text = "Active";
          
        }
        private void SetReadyButtonAppearance()
        {
            BeginSectionBtn.BackColor = Color.PaleTurquoise;
            BeginSectionBtn.Text = "Begin";
          
        }
        private void tsbExitSketch_Click(object sender, EventArgs e)
        {
            PromptToSaveOrDiscard();
        }

        
        #endregion
    }
}
