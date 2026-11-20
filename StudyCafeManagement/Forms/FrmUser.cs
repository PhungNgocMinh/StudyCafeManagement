using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmUser : Form
    {
        private string connString = "Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True";
        private string selectedUserId;

        public FrmUser()
        {
            InitializeComponent();
            txtUsername.MaxLength = 50;
            ApplyCafeTheme();
            Load += FrmUser_Load;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            dgvUsers.CellClick += dgvUsers_CellClick;
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
        
        private System.Windows.Forms.CheckBox chkIsActive;

        private System.Windows.Forms.ComboBox cboRoles;

        private System.Windows.Forms.TextBox txtPassword;

        private System.Windows.Forms.TextBox txtUsername;

        private void FrmUser_Load(object sender, EventArgs e)
        {
            LoadRolesComboBox();
            LoadUserData();
        }

        private void LoadRolesComboBox()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT MaNhom AS RoleId, TenNhom AS RoleName FROM dbo.VAITRO ORDER BY TenNhom", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cboRoles.DataSource = dt;
                    cboRoles.DisplayMember = "RoleName";
                    cboRoles.ValueMember = "RoleId";
                    cboRoles.SelectedIndex = dt.Rows.Count == 0 ? -1 : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải nhóm quyền: " + ex.Message, "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadUserData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlDataAdapter da = new SqlDataAdapter(
                    @"SELECT u.MaNV AS UserId, u.TenDangNhap AS Username, r.TenNhom AS RoleName,
                             CAST(CASE WHEN u.TrangThai = N'Đang hoạt động' THEN 1 ELSE 0 END AS bit) AS IsActive
                      FROM dbo.NGUOIDUNG u
                      JOIN dbo.VAITRO r ON u.MaNhomQuyen = r.MaNhom
                      ORDER BY u.TenDangNhap", conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgvUsers.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách tài khoản: " + ex.Message, "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string roleId = Convert.ToString(cboRoles.SelectedValue);
            bool isActive = chkIsActive.Checked;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(roleId))
            {
                MessageBox.Show("Vui lòng chọn nhóm quyền!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string passwordHash = ComputeSha256Hash(password);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable))
                    {
                        string userId = GetNextUserId(conn, transaction);
                        string query = @"INSERT INTO dbo.NGUOIDUNG
                                             (MaNV, TenDangNhap, MatKhau, MaNhomQuyen, TrangThai)
                                         VALUES
                                             (@MaNV, @Username, @PasswordHash, @RoleId, @TrangThai)";
                        using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
                        {
                            cmd.Parameters.Add("@MaNV", SqlDbType.VarChar, 20).Value = userId;
                            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                            cmd.Parameters.Add("@PasswordHash", SqlDbType.VarChar, 256).Value = passwordHash;
                            cmd.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = roleId;
                            cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 30).Value =
                                isActive ? "Đang hoạt động" : "Đã khóa";

                            cmd.ExecuteNonQuery();
                            transaction.Commit();
                        }

                        MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUserData();
                        ClearUserSelection();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private string GetNextUserId(SqlConnection connection, SqlTransaction transaction)
        {
            int maxUserNumber = 0;
            using (SqlCommand command = new SqlCommand(
                "SELECT MaNV FROM dbo.NGUOIDUNG WITH (UPDLOCK, HOLDLOCK)",
                connection, transaction))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string userId = reader.GetString(0);
                    if (userId.StartsWith("NV", StringComparison.OrdinalIgnoreCase) &&
                        int.TryParse(userId.Substring(2), NumberStyles.None, CultureInfo.InvariantCulture,
                            out int userNumber))
                    {
                        maxUserNumber = Math.Max(maxUserNumber, userNumber);
                    }
                }
            }

            return "NV" + (maxUserNumber + 1).ToString("D2", CultureInfo.InvariantCulture);
        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvUsers.Rows[e.RowIndex];
            selectedUserId = Convert.ToString(row.Cells["UserId"].Value);
            txtUsername.Text = Convert.ToString(row.Cells["Username"].Value);
            txtPassword.Clear();
            cboRoles.SelectedIndex = cboRoles.FindStringExact(Convert.ToString(row.Cells["RoleName"].Value));
            chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedUserId))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa.", "Chưa chọn tài khoản",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string username = txtUsername.Text.Trim();
            string roleId = Convert.ToString(cboRoles.SelectedValue);
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(roleId))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập và chọn nhóm quyền.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string status = chkIsActive.Checked ? "Đang hoạt động" : "Đã khóa";
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(
                    @"UPDATE dbo.NGUOIDUNG
                      SET TenDangNhap = @Username, MaNhomQuyen = @RoleId, TrangThai = @TrangThai
                      WHERE MaNV = @UserId", conn))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 50).Value = username;
                    cmd.Parameters.Add("@RoleId", SqlDbType.VarChar, 20).Value = roleId;
                    cmd.Parameters.Add("@TrangThai", SqlDbType.NVarChar, 30).Value = status;
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 20).Value = selectedUserId;
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("Tài khoản đã chọn không còn tồn tại.", "Không thể cập nhật",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadUserData();
                        ClearUserSelection();
                        return;
                    }
                }

                LoadUserData();
                ClearUserSelection();
                MessageBox.Show("Cập nhật tài khoản thành công.", "Tài khoản",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                MessageBox.Show("Tên đăng nhập này đã được sử dụng.", "Tên đăng nhập trùng",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể cập nhật tài khoản: " + ex.Message, "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedUserId))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa.", "Chưa chọn tài khoản",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM dbo.NGUOIDUNG WHERE MaNV = @UserId", conn))
                {
                    cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 20).Value = selectedUserId;
                    conn.Open();
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        MessageBox.Show("Tài khoản đã chọn không còn tồn tại.", "Không thể xóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadUserData();
                        ClearUserSelection();
                        return;
                    }
                }

                LoadUserData();
                ClearUserSelection();
                MessageBox.Show("Đã xóa tài khoản.", "Tài khoản",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                MessageBox.Show(
                    "Không thể xóa tài khoản này vì tài khoản đang được tham chiếu bởi hóa đơn.",
                    "Tài khoản đang được sử dụng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa tài khoản: " + ex.Message, "Lỗi cơ sở dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUserSelection()
        {
            selectedUserId = null;
            txtUsername.Clear();
            txtPassword.Clear();
            chkIsActive.Checked = true;
            if (cboRoles.Items.Count > 0)
                cboRoles.SelectedIndex = 0;
            dgvUsers.ClearSelection();
            txtUsername.Focus();
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;

        private System.Windows.Forms.Button btnAdd;

        private System.Windows.Forms.DataGridView dgvUsers;
    }
}