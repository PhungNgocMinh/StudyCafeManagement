using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmTable : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedId = null;

        public FrmTable()
        {
            InitializeComponent();
        }

        private void FrmTable_Load(object sender, EventArgs e)
        {
            cboStatus.Items.Clear();
            cboStatus.Items.AddRange(new string[] { "Trống", "Đang phục vụ"});
            cboStatus.SelectedIndex = 0;
            LoadData();
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                var query = db.Bans.AsQueryable();
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(t => t.TenBan.Contains(keyword) || t.MaBan.Contains(keyword));

                dgvTable.DataSource = query.Select(t => new
                {
                    MaBan = t.MaBan,
                    TenBan = t.TenBan,
                    DonGiaGio = t.DonGiaGio,
                    TrangThai = t.TrangThai
                }).ToList();

                if (dgvTable.Columns["MaBan"] != null) dgvTable.Columns["MaBan"].HeaderText = "Mã Bàn";
                if (dgvTable.Columns["TenBan"] != null) dgvTable.Columns["TenBan"].HeaderText = "Tên Bàn";
                if (dgvTable.Columns["DonGiaGio"] != null) dgvTable.Columns["DonGiaGio"].HeaderText = "Đơn Giá/Giờ";
                if (dgvTable.Columns["TrangThai"] != null) dgvTable.Columns["TrangThai"].HeaderText = "Trạng Thái";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtTableCode.Clear();
            txtTableCode.ReadOnly = true;
            txtTableName.Clear();
            nudHourlyRate.Value = 15000;
            cboStatus.SelectedIndex = 0;
            selectedId = null;
            errorProvider1.Clear();
        }

        private bool ValidateInput()
        {
            errorProvider1.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtTableName.Text))
            {
                errorProvider1.SetError(txtTableName, "Tên bàn không được để trống!");
                valid = false;
            }
            return valid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string tenBan = txtTableName.Text.Trim();
            decimal donGia = nudHourlyRate.Value;
            string trangThai = cboStatus.SelectedItem.ToString();

            try
            {
                // NẾU CÓ SELECTEDID -> CẬP NHẬT TRẠNG THÁI / THÔNG TIN BÀN CŨ
                if (!string.IsNullOrEmpty(selectedId))
                {
                    var tbl = db.Bans.Find(selectedId);
                    if (tbl != null)
                    {
                        tbl.TenBan = tenBan;
                        tbl.DonGiaGio = donGia;
                        tbl.TrangThai = trangThai; // Đổi từ Trống -> Đang dùng hoặc ngược lại
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật thông tin bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // NẾU SELECTEDID NULL -> THÊM BÀN MỚI
                {
                    int count = db.Bans.Count() + 1;
                    string autoMaBan = "BAN" + count.ToString("D3");
                    while (db.Bans.Any(b => b.MaBan == autoMaBan))
                    {
                        count++;
                        autoMaBan = "BAN" + count.ToString("D3");
                    }

                    Ban newTbl = new Ban
                    {
                        MaBan = autoMaBan,
                        TenBan = tenBan,
                        DonGiaGio = donGia,
                        TrangThai = trangThai
                    };
                    db.Bans.Add(newTbl);
                    db.SaveChanges();
                    MessageBox.Show("Thêm bàn mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LoadData();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                var inner = ex.InnerException?.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Lỗi cập nhật CSDL: {inner}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Vui lòng chọn bàn cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bàn này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var tbl = db.Bans.Find(selectedId);
                    if (tbl != null)
                    {
                        db.Bans.Remove(tbl);
                        db.SaveChanges();
                        MessageBox.Show("Xóa bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvTable.Rows[e.RowIndex].Cells["MaBan"].Value != null)
            {
                var row = dgvTable.Rows[e.RowIndex];
                selectedId = row.Cells["MaBan"].Value.ToString(); // LƯU LẠI MÃ BÀN KHI CLICK CHỌN
                txtTableCode.Text = selectedId;                  // HIỂN THỊ MÃ BÀN LÊN TEXTBOX
                txtTableName.Text = row.Cells["TenBan"].Value?.ToString();
                nudHourlyRate.Value = Convert.ToDecimal(row.Cells["DonGiaGio"].Value);
                cboStatus.SelectedItem = row.Cells["TrangThai"].Value?.ToString();
                errorProvider1.Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadData(txtSearch.Text.Trim());
        private void btnReset_Click(object sender, EventArgs e) => ResetForm();
    }
}