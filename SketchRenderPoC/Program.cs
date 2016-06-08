using System;
using System.Windows.Forms;

namespace SketchUp
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TestSetup ts = new TestSetup();
            SMParcel testParcel = ts.TestParcel();
            Application.Run(new EditSketchSections(testParcel));
        }
    }
}