using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent(); // Loads the visual designer layout
            ApplyCafeTheme();      // Applies your cozy lofi colors on top
        }

        private void ApplyCafeTheme()
        {
            // 1. Form Background
            this.BackColor = Color.FromArgb(247, 244, 239); // Warm Oatmeal #F7F4EF

            // 2. Style Buttons (assuming you named them btnLogin and btnExit)
            if (btnLogin != null)
            {
                btnLogin.BackColor = Color.FromArgb(200, 138, 88); // Soft Caramel #C88A58
                btnLogin.ForeColor = Color.White;
                btnLogin.FlatStyle = FlatStyle.Flat;
                btnLogin.FlatAppearance.BorderSize = 0;
                btnLogin.Cursor = Cursors.Hand;
            }

            if (btnExit != null)
            {
                btnExit.BackColor = Color.FromArgb(230, 225, 218);
                btnExit.ForeColor = Color.FromArgb(60, 42, 33);
                btnExit.FlatStyle = FlatStyle.Flat;
                btnExit.FlatAppearance.BorderSize = 0;
                btnExit.Cursor = Cursors.Hand;
            }

            // 3. Style TextBoxes
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is TextBox txt)
                {
                    txt.Font = new Font("Segoe UI", 11);
                }
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TextBox txtUsername;
            this.btnExit = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            txtUsername = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new System.Drawing.Point(38, 35);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new System.Drawing.Size(59, 20);
            txtUsername.TabIndex = 8;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(171, 203);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 11;
            this.btnExit.Text = "button1";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(38, 203);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 10;
            this.btnLogin.Text = "button1";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(38, 76);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(59, 20);
            this.txtPassword.TabIndex = 9;
            // 
            // FrmLogin
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(txtUsername);
            this.Name = "FrmLogin";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
    }
}