using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TotpGui.Forms
{
    public partial class RegisterForm : Form
    {
        private const string ERROR_TITLE      = "Password error";
        private const string MSG_PASS_CONFIRM = "Your passwords do not match !";
        private const string MSG_NO_INSERT    = "Could not insert the password !";

        public RegisterForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            DialogResult = DialogResult.Abort;
        }

        private bool errorMsg(string msg)
        {
            MessageBox.Show(msg, ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return true;
        }

        public static string matchPasswordPolicy(string password)
        {
            if (password.Length < 6)
                return "Password length must be over 5 chars";

            if (Regex.IsMatch(password, @"^([bB]onjour|[Pp]assword|1234)[0-9]{0,7}[^a-zA-Z0-9]{0,2}$"))
                return "Password must not start with 1234, vp, bonjour or password";

            string[] blackList = {
                "password",
                "qwerty",
                "baseball",
                "dragon",
                "football",
                "monkey",
                "letmein",
                "mustang",
                "access",
                "shadow",
                "master",
                "michael",
                "superman",
                "batman",
                "trustno1",
                "azerty",
                "696969"
            };

            foreach (string p in blackList)
            {
                if (p == password)
                {
                    return "Password must not be part of the top 20";
                }
            }

            char c = password[1];
            for (int i = 1; i < password.Length; ++i)
                if (c != password[i])
                    return "";

            return "Password must not be composed of only one character";
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            if ((password.Text != passwordConfirm.Text) && errorMsg(MSG_PASS_CONFIRM))
                return;

            string msg = matchPasswordPolicy(password.Text);
            if ((msg != "") && errorMsg(msg))
                return;

            if (!ProgramData.setPassword(password.Text) && errorMsg(MSG_NO_INSERT))
                return;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
