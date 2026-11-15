using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmCategory : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedId = null;

        public FrmCategory()
        {
            InitializeComponent();
        }

        private void FrmCategory_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                var query = db.DanhMucMons.AsQueryable();
                if (!string.IsNullOrWhiteSpace(keyword))
                    query = query.Where(c => c.TenDanhMuc.Contains(keyword) || c.MaDanhMuc.Contains(keyword));

                dgvCategory.DataSource = query.Select(c => new
                {
                    MaDanhMuc = c.MaDanhMuc,
                    TenDanhMuc = c.TenDanhMuc
                }).ToList();

                if (dgvCategory.Columns["MaDanhMuc"] != null) dgvCategory.Columns["MaDanhMuc"].HeaderText = "Mã Danh Mục";
                if (dgvCategory.Columns["TenDanhMuc"] != null) dgvCategory.Columns["TenDanhMuc"].HeaderText = "Tên Danh Mục";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtCategoryCode.Clear();
            txtCategoryCode.ReadOnly = true; // Mã tự sinh nên ẩn chỉnh sửa thủ công
            txtCategoryName.Clear();
            selectedId = null;
            errorProvider1.Clear();
            txtCategoryName.Focus();
        }

        private bool ValidateInput()
        {
            errorProvider1.Clear();
            bool valid = true;
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                errorProvider1.SetError(txtCategoryName, "Tên danh mục không được để trống!");
                valid = false;
            }
            return valid;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string tenDM = txtCategoryName.Text.Trim();

            try
            {
                // KIỂM TRA: Nếu selectedId KHÔNG NULL -> CẬP NHẬT (SỬA TRẠNG THÁI / TÊN)
                if (!string.IsNullOrEmpty(selectedId))
                {
                    bool isDuplicate = db.DanhMucMons.Any(c => c.TenDanhMuc.ToLower() == tenDM.ToLower() && c.MaDanhMuc != selectedId);
                    if (isDuplicate)
                    {
                        errorProvider1.SetError(txtCategoryName, "Tên danh mục này đã tồn tại!");
                        return;
                    }

                    var cat = db.DanhMucMons.Find(selectedId);
                    if (cat != null)
                    {
                        cat.TenDanhMuc = tenDM;
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // NẾU SELECTEDID NULL -> THÊM MỚI (TỰ ĐỘNG SINH MÃ)
                {
                    bool isDuplicate = db.DanhMucMons.Any(c => c.TenDanhMuc.ToLower() == tenDM.ToLower());
                    if (isDuplicate)
                    {
                        errorProvider1.SetError(txtCategoryName, "Tên danh mục này đã tồn tại!");
                        return;
                    }

                    int count = db.DanhMucMons.Count() + 1;
                    string autoMaDM = "DM" + count.ToString("D3");
                    while (db.DanhMucMons.Any(c => c.MaDanhMuc == autoMaDM))
                    {
                        count++;
                        autoMaDM = "DM" + count.ToString("D3");
                    }

                    DanhMucMon newCat = new DanhMucMon
                    {
                        MaDanhMuc = autoMaDM,
                        TenDanhMuc = tenDM
                    };

                    db.DanhMucMons.Add(newCat);
                    db.SaveChanges();
                    MessageBox.Show("Thêm danh mục mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng chọn danh mục cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Xác nhận xóa danh mục này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var cat = db.DanhMucMons.Find(selectedId);
                    if (cat != null)
                    {
                        db.DanhMucMons.Remove(cat);
                        db.SaveChanges();
                        MessageBox.Show("Xóa danh mục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCategory.Rows[e.RowIndex].Cells["MaDanhMuc"].Value != null)
            {
                var row = dgvCategory.Rows[e.RowIndex];
                selectedId = row.Cells["MaDanhMuc"].Value.ToString(); // GIỮ LẠI MÃ ĐỂ CẬP NHẬT
                txtCategoryCode.Text = selectedId;                   // HIỂN THỊ MÃ LÊN TEXTBOX
                txtCategoryName.Text = row.Cells["TenDanhMuc"].Value?.ToString();
                errorProvider1.Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadData(txtSearch.Text.Trim());
        private void btnReset_Click(object sender, EventArgs e) => ResetForm();
    }
}