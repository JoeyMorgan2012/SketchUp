using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SketchUp
{
    public partial class ExpandoSketch:Form
    {
        private void DrawParcelLabel()
        {
            Graphics g = Graphics.FromImage(MainImage);
            SolidBrush Lblbrush = new SolidBrush(Color.Black);
            SolidBrush FillBrush = new SolidBrush(Color.White);
            Pen whitePen = new Pen(Color.White, 2);
            Pen blackPen = new Pen(Color.Black, 2);

            Font LbLf = new Font("Segue UI", 10, FontStyle.Bold);
            Font TitleF = new Font("Segue UI", 10, FontStyle.Bold | FontStyle.Underline);
            Font MainTitle = new System.Drawing.Font("Segue UI", 15, FontStyle.Bold | FontStyle.Underline);
            char[] leadzero = new char[] { '0' };

            g.DrawString(SketchUpGlobals.LocalityPreFix, TitleF, Lblbrush, new PointF(10, 10));
            g.DrawString("Edit Sketch", MainTitle, Lblbrush, new PointF(450, 10));
            g.DrawString(String.Format("Record # - {0}", SketchUpGlobals.Record.ToString().TrimStart(leadzero)), LbLf, Lblbrush, new PointF(10, 30));
            g.DrawString(String.Format("Card # - {0}", SketchUpGlobals.Card), LbLf, Lblbrush, new PointF(10, 45));

            g.DrawString(String.Format("Scale - {0}", _currentScale), LbLf, Lblbrush, new PointF(10, 70));
        }
    }
}
