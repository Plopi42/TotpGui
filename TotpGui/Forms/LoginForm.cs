using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotpGui.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            DialogResult = DialogResult.Abort;
        }

        private void password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ProgramData.checkPassword(password.Text) > 0)
                {
                    e.Handled = true;
                    DialogResult = DialogResult.OK;
                    Close();
                    return;
                }

                password.ForeColor = Color.Red;
            }
            else
                password.ForeColor = Color.Black;
        }
    }
}
