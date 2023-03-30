using System;
using System.Windows.Forms;
using AutomationEngineering.LoggingHelper;

namespace IR_MALDESI.Browser
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

            // Set up log
            LoggingConfiguration.Configure("IR-MALDESI Browser", "1.0");

            Application.Run(new Forms.Browser());
        }
    }
}