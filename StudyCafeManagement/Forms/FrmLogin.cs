using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace StudyCafeManagement.Forms
{
    public partial class FrmLogin : Form
    {
        private string connString = "Data Source=.;Initial Catalog=QuanLyCaPheHocTap;Integrated Security=True;TrustServerCertificate=True";
        public string LoggedInUser { get; private set; }
        public string LoggedInRole { get; private set; }
        public string LoggedInEmployeeId { get; private set; }
        
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
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin tài khoản!", "Nhắc nhở", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string passwordHash = ComputeSha256Hash(password);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT u.MaNV, u.TenDangNhap, r.TenNhom
                                     FROM dbo.NGUOIDUNG u
                                     JOIN dbo.VAITRO r ON u.MaNhomQuyen = r.MaNhom
                                     WHERE u.TenDangNhap = @Username
                                       AND u.MatKhau = @PasswordHash
                                       AND u.TrangThai = N'Đang hoạt động'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                LoggedInUser = reader["TenDangNhap"].ToString();
                                LoggedInRole = reader["TenNhom"].ToString();
                                LoggedInEmployeeId = reader["MaNV"].ToString();
                                this.DialogResult = DialogResult.OK;
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Sai tên đăng nhập, mật khẩu hoặc tài khoản đã bị khóa.",
                                    "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi kết nối CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát ứng dụng?", 
                "Xác nhận thoát", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
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

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.TextBox txtPassword;
    }
}