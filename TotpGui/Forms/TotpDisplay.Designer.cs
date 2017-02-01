namespace TotpGui.Forms
{
    partial class TotpDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.totpList = new System.Windows.Forms.ComboBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.totpValue = new System.Windows.Forms.TextBox();
            this.totpAddButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // totpList
            // 
            this.totpList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.totpList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.totpList.FormattingEnabled = true;
            this.totpList.Location = new System.Drawing.Point(43, 30);
            this.totpList.Name = "totpList";
            this.totpList.Size = new System.Drawing.Size(245, 21);
            this.totpList.TabIndex = 0;
            this.totpList.SelectedIndexChanged += new System.EventHandler(this.totpList_SelectedIndexChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(43, 110);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(300, 23);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 1;
            // 
            // totpValue
            // 
            this.totpValue.BackColor = System.Drawing.SystemColors.Control;
            this.totpValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totpValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totpValue.Location = new System.Drawing.Point(43, 64);
            this.totpValue.Name = "totpValue";
            this.totpValue.ReadOnly = true;
            this.totpValue.Size = new System.Drawing.Size(300, 33);
            this.totpValue.TabIndex = 2;
            this.totpValue.Text = "Token value";
            this.totpValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // totpAddButton
            // 
            this.totpAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totpAddButton.Location = new System.Drawing.Point(294, 28);
            this.totpAddButton.Name = "totpAddButton";
            this.totpAddButton.Size = new System.Drawing.Size(23, 23);
            this.totpAddButton.TabIndex = 3;
            this.totpAddButton.Text = "+";
            this.totpAddButton.UseVisualStyleBackColor = true;
            this.totpAddButton.Click += new System.EventHandler(this.totpAddButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteButton.Location = new System.Drawing.Point(320, 28);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(23, 23);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "-";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // TotpDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 162);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.totpAddButton);
            this.Controls.Add(this.totpValue);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.totpList);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 200);
            this.MinimumSize = new System.Drawing.Size(400, 200);
            this.Name = "TotpDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TotpDisplay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TotpDisplay_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox totpList;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox totpValue;
        private System.Windows.Forms.Button totpAddButton;
        private System.Windows.Forms.Button deleteButton;
    }
}