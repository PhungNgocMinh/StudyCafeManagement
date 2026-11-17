namespace StudyCafeManagement.Forms
{
    partial class FrmRole
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRoleName;
        private System.Windows.Forms.TextBox txtRoleName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button btnAddRole;
        private System.Windows.Forms.Button btnEditRole;
        private System.Windows.Forms.Button btnDeleteRole;
        private System.Windows.Forms.DataGridView dgvRoles;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRoleName = new System.Windows.Forms.Label();
            this.txtRoleName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.btnAddRole = new System.Windows.Forms.Button();
            this.btnEditRole = new System.Windows.Forms.Button();
            this.btnDeleteRole = new System.Windows.Forms.Button();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.SuspendLayout();

            // pnlLeft
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.Controls.Add(this.lblTitle);
            this.pnlLeft.Controls.Add(this.lblRoleName);
            this.pnlLeft.Controls.Add(this.txtRoleName);
            this.pnlLeft.Controls.Add(this.lblDescription);
            this.pnlLeft.Controls.Add(this.txtDescription);
            this.pnlLeft.Controls.Add(this.btnAddRole);
            this.pnlLeft.Controls.Add(this.btnEditRole);
            this.pnlLeft.Controls.Add(this.btnDeleteRole);
            this.pnlLeft.Location = new System.Drawing.Point(15, 15);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(290, 400);
            this.pnlLeft.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(156, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Thêm Nhóm Quyền";

            // lblRoleName
            this.lblRoleName.AutoSize = true;
            this.lblRoleName.Location = new System.Drawing.Point(20, 65);
            this.lblRoleName.Name = "lblRoleName";
            this.lblRoleName.Size = new System.Drawing.Size(92, 13);
            this.lblRoleName.TabIndex = 1;
            this.lblRoleName.Text = "Tên nhóm quyền:";

            // txtRoleName
            this.txtRoleName.Location = new System.Drawing.Point(20, 85);
            this.txtRoleName.Name = "txtRoleName";
            this.txtRoleName.Size = new System.Drawing.Size(245, 20);
            this.txtRoleName.TabIndex = 2;

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 130);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(74, 13);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Mô tả chi tiết:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(20, 150);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(245, 75);
            this.txtDescription.TabIndex = 4;

            // btnAddRole
            this.btnAddRole.Location = new System.Drawing.Point(20, 245);
            this.btnAddRole.Name = "btnAddRole";
            this.btnAddRole.Size = new System.Drawing.Size(245, 38);
            this.btnAddRole.TabIndex = 5;
            this.btnAddRole.Text = "Thêm Quyền";
            this.btnAddRole.UseVisualStyleBackColor = true;
            this.btnAddRole.Click += new System.EventHandler(this.btnAddRole_Click);

            // btnEditRole
            this.btnEditRole.Enabled = false;
            this.btnEditRole.Location = new System.Drawing.Point(20, 295);
            this.btnEditRole.Name = "btnEditRole";
            this.btnEditRole.Size = new System.Drawing.Size(112, 38);
            this.btnEditRole.TabIndex = 6;
            this.btnEditRole.Text = "Cập nhật";
            this.btnEditRole.UseVisualStyleBackColor = true;
            this.btnEditRole.Click += new System.EventHandler(this.btnEditRole_Click);

            // btnDeleteRole
            this.btnDeleteRole.Enabled = false;
            this.btnDeleteRole.Location = new System.Drawing.Point(153, 295);
            this.btnDeleteRole.Name = "btnDeleteRole";
            this.btnDeleteRole.Size = new System.Drawing.Size(112, 38);
            this.btnDeleteRole.TabIndex = 7;
            this.btnDeleteRole.Text = "Xóa";
            this.btnDeleteRole.UseVisualStyleBackColor = true;
            this.btnDeleteRole.Click += new System.EventHandler(this.btnDeleteRole_Click);

            // dgvRoles
            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoles.Location = new System.Drawing.Point(320, 15);
            this.dgvRoles.Name = "dgvRoles";
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoles.Size = new System.Drawing.Size(455, 400);
            this.dgvRoles.TabIndex = 1;
            this.dgvRoles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRoles_CellClick);

            // FrmRole
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 440);
            this.Controls.Add(this.dgvRoles);
            this.Controls.Add(this.pnlLeft);
            this.Name = "FrmRole";
            this.Text = "Quản Lý Nhóm Quyền Hệ Thống";
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.ResumeLayout(false);
        }
    }
}