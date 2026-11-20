using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            string oldPass = txtOldPassword.Text;
            string newPass = txtNewPassword.Text;
            string confirmPass = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(oldPass) || string.IsNullOrEmpty(newPass) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Mật khẩu mới và xác nhận không trùng khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string oldHash = ComputeSha256Hash(oldPass);
            string newHash = ComputeSha256Hash(newPass);

            try
            {
                using (CoffeeShopDbContext db = new CoffeeShopDbContext())
                {
                    var user = db.NguoiDungs.FirstOrDefault(u =>
                        u.TenDangNhap == username && u.MatKhau == oldHash);
                    if (user == null)
                    {
                        MessageBox.Show("Mật khẩu cũ không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    user.MatKhau = newHash;
                    db.SaveChanges();
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đổi mật khẩu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn huỷ?", 
                "Xác nhận huỷ", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question
            );
            if (result == DialogResult.Yes)
            {
                this.Close();
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

        // Keep your existing validation and database code below...
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        

    }
}