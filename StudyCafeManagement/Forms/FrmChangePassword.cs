using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmChangePassword : Form
    {
        private string username;

        public FrmChangePassword(string loggedInUser)
        {
            username = loggedInUser;
            InitializeComponent(); // Loads controls from designer
            ApplyCafeTheme();      // Applies theme colors
        }

        private void ApplyCafeTheme()
        {
            // Form Background
            this.BackColor = Color.FromArgb(247, 244, 239);
            this.Text = "Đổi Mật Khẩu";

            // Style Save Button (Soft Caramel)
            if (btnSave != null)
            {
                btnSave.BackColor = Color.FromArgb(200, 138, 88); // #C88A58
                btnSave.ForeColor = Color.White;
                btnSave.FlatStyle = FlatStyle.Flat;
                btnSave.FlatAppearance.BorderSize = 0;
                btnSave.Cursor = Cursors.Hand;
            }

            // Style Cancel Button
            if (btnCancel != null)
            {
                btnCancel.BackColor = Color.FromArgb(220, 215, 208);
                btnCancel.ForeColor = Color.FromArgb(60, 42, 33);
                btnCancel.FlatStyle = FlatStyle.Flat;
                btnCancel.FlatAppearance.BorderSize = 0;
                btnCancel.Cursor = Cursors.Hand;
            }
        }

        // Keep your existing validation and database code below...
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.txtOldpassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(171, 203);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "button1";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(38, 203);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "button1";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(38, 76);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(59, 20);
            this.txtNewPassword.TabIndex = 5;
            // 
            // txtOldpassword
            // 
            this.txtOldpassword.Location = new System.Drawing.Point(38, 35);
            this.txtOldpassword.Name = "txtOldpassword";
            this.txtOldpassword.Size = new System.Drawing.Size(59, 20);
            this.txtOldpassword.TabIndex = 4;
            // 
            // FrmChangePassword
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtOldpassword);
            this.Name = "FrmChangePassword";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtNewPassword;
        private System.Windows.Forms.TextBox txtOldpassword;
    }
}