using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TotpGui.Crypto;

namespace TotpGui.Forms
{
    public partial class TotpDisplay : Form
    {
        private Timer totpTimer = null;

        public TotpDisplay()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            loadTotpList();

            // Intialize the timer
            totpTimer = new Timer();
            totpTimer.Tick += new EventHandler(totpTimer_Tick);
            totpTimer.Interval = 1500;
        }

        private void totpTimer_Tick(object sender, EventArgs e)
        {
            updateTotp();
        }

        private void totpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTotp();
            totpTimer.Start();
        }

        private void updateTotp()
        {
            if (totpList.Text == "")
            {
                totpTimer.Stop();
                totpValue.Text = "Token value";
                progressBar.Value = 0;
                return;
            }

            string secret = ProgramData.getTotpSecret(totpList.Text);

            if (secret == null)
            {
                totpValue.Text = "Token value";
                return;
            }

            totpValue.Text = Totp.now(Base32.decode(secret));

            progressBar.Value = 100 - (int)Totp.getIntervalPercent();
        }

        private void totpAddButton_Click(object sender, EventArgs e)
        {
            // Open TOTP add form
            TotpAddForm f = new TotpAddForm();
            if (f.ShowDialog() != DialogResult.OK)
                return;

            totpList.Items.Add(f.addedName);
            totpList.Enabled = true;
        }

        private void loadTotpList()
        {
            List<string> l = ProgramData.getTotpTitles();

            if ((l != null) && (l.Count > 0))
                totpList.Items.AddRange(l.ToArray());
            else
                totpList.Enabled = false;
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Delete TOTP
            if (!checkTotpSelected())
                return;

            if ((MessageBox.Show("Are you sure that you would like to delete this token ?",
                                "Token delete",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question) == DialogResult.Yes)
                && ProgramData.removeTotp(totpList.Text))
                totpList.Items.RemoveAt(totpList.SelectedIndex);
        }

        // ex from : https://msdn.microsoft.com/fr-fr/library/system.windows.forms.messagebox(v=vs.110).aspx
        private void TotpDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            const string message = "Are you sure that you would like to close the application ?";
            const string caption = "Form Closing";
            var result = MessageBox.Show(message, caption,
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            // If the no button was pressed ...
            if (result == DialogResult.No)
            {
                // cancel the closure of the form.
                e.Cancel = true;
            }
        }

        private bool checkTotpSelected()
        {
            if (totpList.SelectedIndex < 0)
            {
                MessageBox.Show("You need to select a token first.",
                                "No selected token",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }
    }
}
