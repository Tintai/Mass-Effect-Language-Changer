using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mass_Effect_Language_Changer
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MELangChange());
        }
    }
}
