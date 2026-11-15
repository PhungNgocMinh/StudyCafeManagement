using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmRole : Form
    {
        public FrmRole()
        {
            InitializeComponent(); // Loads designer layout
            ApplyCafeTheme();      // Applies custom lofi styles
        }

        private void ApplyCafeTheme()
        {
            // 1. Form Background
            this.BackColor = Color.FromArgb(247, 244, 239);

            // 2. Style DataGridView
            if (dgvRoles != null)
            {
                dgvRoles.BackgroundColor = Color.White;
                dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 42, 33); // #3C2A21
                dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvRoles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvRoles.EnableHeadersVisualStyles = false;
                dgvRoles.GridColor = Color.FromArgb(230, 225, 218);
            }

            // 3. Style Add/Action Button
            if (btnAddRole != null)
            {
                btnAddRole.BackColor = Color.FromArgb(200, 138, 88); // Caramel #C88A58
                btnAddRole.ForeColor = Color.White;
                btnAddRole.FlatStyle = FlatStyle.Flat;
                btnAddRole.FlatAppearance.BorderSize = 0;
                btnAddRole.Cursor = Cursors.Hand;
            }
        }

        // Keep your existing role loading logic and click events below...
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.btnAddRole = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRoles
            // 
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Location = new System.Drawing.Point(26, 25);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.Size = new System.Drawing.Size(240, 150);
            this.dgvRoles.TabIndex = 0;
            // 
            // btnAddRole
            // 
            this.btnAddRole.Location = new System.Drawing.Point(26, 228);
            this.btnAddRole.Name = "btnAddRole";
            this.btnAddRole.Size = new System.Drawing.Size(75, 23);
            this.btnAddRole.TabIndex = 1;
            this.btnAddRole.Text = "button1";
            this.btnAddRole.UseVisualStyleBackColor = true;
            // 
            // FrmRole
            // 
            this.ClientSize = new System.Drawing.Size(547, 383);
            this.Controls.Add(this.btnAddRole);
            this.Controls.Add(this.dgvRoles);
            this.Name = "FrmRole";
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnAddRole;

        private System.Windows.Forms.DataGridView dgvRoles;
    }
}