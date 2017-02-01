using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using TotpGui.Forms;

namespace TotpGui
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form f;
            if (ProgramData.hasPassword())
                f = new LoginForm();
            else
                f = new RegisterForm();

            if (f.ShowDialog() == DialogResult.OK)
                Application.Run(new TotpDisplay());
        }
    }
}
