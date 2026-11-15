using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmMain : Form
    {
        private string currentUser;
        private string currentRole;

        public FrmMain(string username, string role)
        {
            currentUser = username;
            currentRole = role;
            InitializeComponent(); // Loads your designer-placed menu/status strips
            ApplyCafeTheme();      // Applies the lofi theme
        }

        private void ApplyCafeTheme()
        {
            // 1. MDI Container Background
            this.IsMdiContainer = true;
            this.BackColor = Color.FromArgb(247, 244, 239); // Warm Oatmeal #F7F4EF
            this.Text = $"Study Coffee Shop Manager • Tài khoản: {currentUser} [{currentRole}]";

            // 2. Style MenuStrip (Deep Espresso Theme)
            if (menuStrip1 != null)
            {
                menuStrip1.BackColor = Color.FromArgb(60, 42, 33); // #3C2A21
                menuStrip1.ForeColor = Color.White;
                menuStrip1.Font = new Font("Segoe UI", 10);
            }

            // 3. Style StatusStrip
            if (statusStrip1 != null)
            {
                statusStrip1.BackColor = Color.FromArgb(60, 42, 33);
                statusStrip1.ForeColor = Color.FromArgb(247, 244, 239);
            }

            ApplyRolePermissions();
        }

        private void ApplyRolePermissions()
        {
            // Example menu visibility based on role (adjust names to match your designer items)
            switch (currentRole)
            {
                case "Admin":
                    break;
                case "ThuNgân":
                    if (mnuManagement != null) mnuManagement.Visible = false;
                    break;
                case "PhaChế":
                    if (mnuManagement != null) mnuManagement.Visible = false;
                    break;
                default:
                    if (mnuManagement != null) mnuManagement.Visible = false;
                    break;
            }
        }

        // Keep your existing navigation and event handlers below...
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuManagement = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.mnuManagement });
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuManagement
            // 
            this.mnuManagement.Name = "mnuManagement";
            this.mnuManagement.Size = new System.Drawing.Size(125, 20);
            this.mnuManagement.Text = "toolStripMenuItem1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 239);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(284, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // FrmMain
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ToolStripMenuItem mnuManagement;

        private System.Windows.Forms.StatusStrip statusStrip1;

        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}