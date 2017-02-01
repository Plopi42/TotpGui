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
    public partial class TotpAddForm : Form
    {
        public TotpAddForm()
        {
            InitializeComponent();
            this.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            DialogResult = DialogResult.Abort;
        }

        public string addedName { get; private set; }

        private void Submit_Click(object sender, EventArgs e)
        {
            if (!ProgramData.addTotp(totpName.Text, seed.Text))
            {
                seed.Text = "Bad value !";
                return;
            }

            addedName = totpName.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
