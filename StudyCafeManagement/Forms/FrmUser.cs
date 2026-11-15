using System;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmUser : Form
    {
        public FrmUser()
        {
            InitializeComponent(); // Loads your visual layout
            ApplyCafeTheme();      // Styles table headers, buttons, and background
        }

        private void ApplyCafeTheme()
        {
            // 1. Form Background
            this.BackColor = Color.FromArgb(247, 244, 239);

            // 2. Style DataGridView (Espresso headers)
            if (dgvUsers != null)
            {
                dgvUsers.BackgroundColor = Color.White;
                dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 42, 33); // #3C2A21
                dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvUsers.EnableHeadersVisualStyles = false;
                dgvUsers.GridColor = Color.FromArgb(230, 225, 218);
            }

            // 3. Style Action Buttons
            if (btnAdd != null)
            {
                btnAdd.BackColor = Color.FromArgb(200, 138, 88); // Caramel #C88A58
                btnAdd.ForeColor = Color.White;
                btnAdd.FlatStyle = FlatStyle.Flat;
                btnAdd.FlatAppearance.BorderSize = 0;
            }

            if (btnUpdate != null)
            {
                btnUpdate.BackColor = Color.FromArgb(100, 120, 100); // Muted Sage
                btnUpdate.ForeColor = Color.White;
                btnUpdate.FlatStyle = FlatStyle.Flat;
                btnUpdate.FlatAppearance.BorderSize = 0;
            }

            if (btnDelete != null)
            {
                btnDelete.BackColor = Color.FromArgb(180, 80, 80); // Soft Terracotta/Red
                btnDelete.ForeColor = Color.White;
                btnDelete.FlatStyle = FlatStyle.Flat;
                btnDelete.FlatAppearance.BorderSize = 0;
            }
        }

        // Keep your existing database loading and click events below...
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvUsers
            // 
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(35, 66);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.Size = new System.Drawing.Size(118, 53);
            this.dgvUsers.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(35, 189);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "button1";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(132, 189);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 2;
            this.btnUpdate.Text = "button1";
            this.btnUpdate.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(244, 189);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "button1";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // FrmUser
            // 
            this.ClientSize = new System.Drawing.Size(408, 299);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvUsers);
            this.Name = "FrmUser";
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;

        private System.Windows.Forms.Button btnAdd;

        private System.Windows.Forms.DataGridView dgvUsers;
    }
}