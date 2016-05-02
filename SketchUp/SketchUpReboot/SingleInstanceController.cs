using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;

namespace SketchUpReboot
{
    public class SingleInstanceController
   : WindowsFormsApplicationBase
    {
        public SingleInstanceController()
        {
            // Set whether the application is single instance
            this.IsSingleInstance = true;

            this.StartupNextInstance += new
              StartupNextInstanceEventHandler(this_StartupNextInstance);
        }

        private void this_StartupNextInstance(object sender, StartupNextInstanceEventArgs e)
        {
            CommandLineArguments cla = new CommandLineArguments(e.CommandLine.ToArray(), @"(?<argname>-\w+):(?<argvalue>\S+)");

            e.BringToForeground = true;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            Program.commandLineArgs = new CommandLineArguments(eventArgs.CommandLine.ToArray(), @"(?<argname>-\w+):(?<argvalue>\S+)");

            //base.OnStartupNextInstance(eventArgs);
        }

        protected override void OnCreateMainForm()
        {
            this.MainForm = new Main();
        }
    }
}