using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmCustomer : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedId = null; // Đã đổi sang string để tránh FormatException

        public FrmCustomer()
        {
            InitializeComponent();
        }

        private void FrmCustomer_Load(object sender, EventArgs e)
        {
            cboCustomerTier.Items.Clear();
            cboCustomerTier.Items.AddRange(new string[] { "Thường", "Thân thiết" });
            cboCustomerTier.SelectedIndex = 0;
            LoadData();
        }

        #region Tải dữ liệu

        private void LoadData(string keyword = "")
        {
            try
            {
                var query = db.KhachHangs.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(k => k.HoTen.Contains(keyword) || k.SDT.Contains(keyword) || k.MaKH.Contains(keyword));
                }

                dgvCustomer.DataSource = query
                    .Select(k => new
                    {
                        MaKH = k.MaKH,
                        HoTen = k.HoTen,
                        SDT = k.SDT,
                        DiemTichLuy = k.DiemTichLuy,
                        HangKhach = k.HangKhach
                    })
                    .ToList();

                if (dgvCustomer.Columns["MaKH"] != null) dgvCustomer.Columns["MaKH"].HeaderText = "Mã KH";
                if (dgvCustomer.Columns["HoTen"] != null) dgvCustomer.Columns["HoTen"].HeaderText = "Họ và Tên";
                if (dgvCustomer.Columns["SDT"] != null) dgvCustomer.Columns["SDT"].HeaderText = "Số Điện Thoại";
                if (dgvCustomer.Columns["DiemTichLuy"] != null) dgvCustomer.Columns["DiemTichLuy"].HeaderText = "Điểm Tích Lũy";
                if (dgvCustomer.Columns["HangKhach"] != null) dgvCustomer.Columns["HangKhach"].HeaderText = "Hạng Khách";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtFullName.Clear();
            txtPhoneNumber.Clear();
            nudRewardPoints.Value = 0;
            cboCustomerTier.SelectedIndex = 0;
            selectedId = null;
            errorProvider1.Clear();
            txtFullName.Focus();
        }

        #endregion

        #region Kiểm tra dữ liệu nhập

        private bool ValidateInput()
        {
            errorProvider1.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                errorProvider1.SetError(txtFullName, "Họ tên không được để trống!");
                valid = false;
            }

            string sdt = txtPhoneNumber.Text.Trim();
            if (!Regex.IsMatch(sdt, @"^0\d{9}$"))
            {
                errorProvider1.SetError(txtPhoneNumber, "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng số 0!");
                valid = false;
            }

            return valid;
        }

        #endregion

        #region Sự kiện

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string sdt = txtPhoneNumber.Text.Trim();
            string hoTen = txtFullName.Text.Trim();
            int diem = (int)nudRewardPoints.Value;
            string hang = cboCustomerTier.SelectedItem?.ToString() ?? "Thường";

            try
            {
                if (!string.IsNullOrEmpty(selectedId)) // Cập nhật
                {
                    bool isDuplicate = db.KhachHangs.Any(k => k.SDT == sdt && k.MaKH != selectedId);
                    if (isDuplicate)
                    {
                        errorProvider1.SetError(txtPhoneNumber, "Số điện thoại này đã thuộc về khách hàng khác!");
                        return;
                    }

                    var cust = db.KhachHangs.Find(selectedId);
                    if (cust != null)
                    {
                        cust.HoTen = hoTen;
                        cust.SDT = sdt;
                        cust.DiemTichLuy = diem;
                        cust.HangKhach = hang;
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Thêm mới (Tự động sinh MaKH dạng KH001)
                {
                    if (db.KhachHangs.Any(k => k.SDT == sdt))
                    {
                        errorProvider1.SetError(txtPhoneNumber, "Số điện thoại này đã được đăng ký!");
                        return;
                    }

                    int count = db.KhachHangs.Count() + 1;
                    string autoMaKH = "KH" + count.ToString("D3");
                    while (db.KhachHangs.Any(k => k.MaKH == autoMaKH))
                    {
                        count++;
                        autoMaKH = "KH" + count.ToString("D3");
                    }

                    KhachHang kh = new KhachHang
                    {
                        MaKH = autoMaKH, // Gán Mã KH không bị Null[cite: 8, 19]
                        HoTen = hoTen,
                        SDT = sdt,
                        DiemTichLuy = diem,
                        HangKhach = hang
                    };

                    db.KhachHangs.Add(kh);
                    db.SaveChanges();
                    MessageBox.Show("Thêm khách hàng mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadData();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                string errorMessages = string.Join("\n", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage));
                MessageBox.Show($"Lỗi dữ liệu:\n{errorMessages}", "Lỗi Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedId))
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var cust = db.KhachHangs.Find(selectedId);
                    if (cust != null)
                    {
                        db.KhachHangs.Remove(cust);
                        db.SaveChanges();
                        MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e) => ResetForm();

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomer.Rows[e.RowIndex].Cells["MaKH"].Value != null)
            {
                var row = dgvCustomer.Rows[e.RowIndex];
                selectedId = row.Cells["MaKH"].Value.ToString(); // Đọc dạng Chuỗi để sửa FormatException[cite: 20]
                txtFullName.Text = row.Cells["HoTen"].Value?.ToString();
                txtPhoneNumber.Text = row.Cells["SDT"].Value?.ToString();

                if (row.Cells["DiemTichLuy"].Value != null)
                    nudRewardPoints.Value = Convert.ToDecimal(row.Cells["DiemTichLuy"].Value);

                if (row.Cells["HangKhach"].Value != null)
                    cboCustomerTier.SelectedItem = row.Cells["HangKhach"].Value.ToString();

                errorProvider1.Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadData(txtSearch.Text.Trim());

        #endregion
    }
}