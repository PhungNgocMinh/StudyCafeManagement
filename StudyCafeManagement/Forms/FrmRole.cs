using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmRole : Form
    {
        private readonly string connString = "Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True";
        private string selectedRoleId;

        public FrmRole()
        {
            InitializeComponent();
            txtRoleName.MaxLength = 50;
            txtDescription.MaxLength = 500;
            ApplyCafeTheme();
            Load += FrmRole_Load;
            btnEditRole.BackColor = Color.FromArgb(95, 125, 85);
            btnEditRole.ForeColor = Color.White;
            btnEditRole.FlatStyle = FlatStyle.Flat;
            btnEditRole.FlatAppearance.BorderSize = 0;
            btnDeleteRole.BackColor = Color.FromArgb(160, 70, 60);
            btnDeleteRole.ForeColor = Color.White;
            btnDeleteRole.FlatStyle = FlatStyle.Flat;
            btnDeleteRole.FlatAppearance.BorderSize = 0;
        }
        
        private void ApplyCafeTheme()
        {
            BackColor = Color.FromArgb(247, 244, 239);
            dgvRoles.BackgroundColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(60, 42, 33);
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.GridColor = Color.FromArgb(230, 225, 218);
            btnAddRole.BackColor = Color.FromArgb(200, 138, 88);
            btnAddRole.ForeColor = Color.White;
            btnAddRole.FlatStyle = FlatStyle.Flat;
            btnAddRole.FlatAppearance.BorderSize = 0;
            btnAddRole.Cursor = Cursors.Hand;
        }

        private void FrmRole_Load(object sender, EventArgs e)
        {
            LoadRoleData();
        }

        private void LoadRoleData()
        {
            try
            {
                using (var connection = new SqlConnection(connString))
                using (var adapter = new SqlDataAdapter(
                    "SELECT MaNhom AS RoleId, TenNhom AS RoleName, Description FROM dbo.VAITRO ORDER BY TenNhom", connection))
                {
                    var roles = new DataTable();
                    adapter.Fill(roles);
                    dgvRoles.DataSource = roles;
                    dgvRoles.Columns["RoleId"].HeaderText = "Role ID";
                    dgvRoles.Columns["RoleName"].HeaderText = "Role name";
                    dgvRoles.Columns["Description"].HeaderText = "Description";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load roles: " + ex.Message, "Database error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            string roleName = txtRoleName.Text.Trim();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                MessageBox.Show("Enter a role name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var duplicateCommand = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.VAITRO WHERE TenNhom = @RoleName",
                            connection, transaction))
                        {
                            duplicateCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = roleName;
                            if ((int)duplicateCommand.ExecuteScalar() > 0)
                            {
                                MessageBox.Show("A role with this name already exists.", "Duplicate role",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        string roleId;
                        int number = 1;
                        using (var idCommand = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.VAITRO WHERE MaNhom = @RoleId",
                            connection, transaction))
                        {
                            idCommand.Parameters.Add("@RoleId", SqlDbType.VarChar, 20);
                            do
                            {
                                roleId = "ROLE_" + number.ToString("D3");
                                idCommand.Parameters["@RoleId"].Value = roleId;
                                number++;
                            }
                            while ((int)idCommand.ExecuteScalar() > 0);
                        }

                        using (var insertCommand = new SqlCommand(
                            "INSERT INTO dbo.VAITRO (MaNhom, TenNhom, Description) VALUES (@RoleId, @RoleName, @Description)",
                            connection, transaction))
                        {
                            insertCommand.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = roleId;
                            insertCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = roleName;
                            insertCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value =
                                string.IsNullOrWhiteSpace(txtDescription.Text)
                                    ? (object)DBNull.Value
                                    : txtDescription.Text.Trim();
                            insertCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                ResetRoleForm();
                LoadRoleData();
            }
            catch (SqlException ex) when (ex.Number == 547 && ex.Message.Contains("CK_VaiTro_TenNhom"))
            {
                MessageBox.Show(
                    "The database still restricts role names to the built-in roles. Run Update_Role_Constraint.sql against QuanLyCaPheHocTap, then try again.",
                    "Database schema update required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not add role: " + ex.Message, "Database error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRoles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var row = dgvRoles.Rows[e.RowIndex];
            selectedRoleId = Convert.ToString(row.Cells["RoleId"].Value);
            txtRoleName.Text = Convert.ToString(row.Cells["RoleName"].Value);
            txtDescription.Text = row.Cells["Description"].Value == DBNull.Value
                ? string.Empty
                : Convert.ToString(row.Cells["Description"].Value);
            btnEditRole.Enabled = true;
            btnDeleteRole.Enabled = true;
        }

        private void btnEditRole_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedRoleId))
            {
                MessageBox.Show("Select a role to update.", "No role selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string roleName = txtRoleName.Text.Trim();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                MessageBox.Show("Enter a role name.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoleName.Focus();
                return;
            }

            try
            {
                using (var connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        using (var duplicateCommand = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.VAITRO WHERE TenNhom = @RoleName AND MaNhom <> @RoleId",
                            connection, transaction))
                        {
                            duplicateCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = roleName;
                            duplicateCommand.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = selectedRoleId;
                            if ((int)duplicateCommand.ExecuteScalar() > 0)
                            {
                                MessageBox.Show("A role with this name already exists.", "Duplicate role",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        using (var updateCommand = new SqlCommand(
                            "UPDATE dbo.VAITRO SET TenNhom = @RoleName, Description = @Description WHERE MaNhom = @RoleId",
                            connection, transaction))
                        {
                            updateCommand.Parameters.Add("@RoleName", SqlDbType.NVarChar, 50).Value = roleName;
                            updateCommand.Parameters.Add("@Description", SqlDbType.NVarChar, 500).Value =
                                string.IsNullOrWhiteSpace(txtDescription.Text)
                                    ? (object)DBNull.Value
                                    : txtDescription.Text.Trim();
                            updateCommand.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = selectedRoleId;
                            if (updateCommand.ExecuteNonQuery() == 0)
                            {
                                MessageBox.Show("The selected role no longer exists.", "Update failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        transaction.Commit();
                    }
                }

                ResetRoleForm();
                LoadRoleData();
                MessageBox.Show("Role updated successfully.", "Roles",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not update role: " + ex.Message, "Database error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDeleteRole_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedRoleId))
            {
                MessageBox.Show("Select a role to delete.", "No role selected",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show(
                "Delete the selected authority group? This cannot be undone.",
                "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (var connection = new SqlConnection(connString))
                using (var command = new SqlCommand(
                    "DELETE FROM dbo.VAITRO WHERE MaNhom = @RoleId", connection))
                {
                    command.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = selectedRoleId;
                    connection.Open();
                    if (command.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("The selected role no longer exists.", "Delete failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                ResetRoleForm();
                LoadRoleData();
                MessageBox.Show("Role deleted successfully.", "Roles",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show(
                    "This authority group is assigned to one or more users and cannot be deleted. Reassign those users first.",
                    "Role in use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not delete role: " + ex.Message, "Database error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetRoleForm()
        {
            selectedRoleId = null;
            txtRoleName.Clear();
            txtDescription.Clear();
            btnEditRole.Enabled = false;
            btnDeleteRole.Enabled = false;
            dgvRoles.ClearSelection();
        }
    }
}
