using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmIngredient : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedId = null; // Đã đổi sang kiểu string để khớp với MaNL trong CSDL

        public FrmIngredient()
        {
            InitializeComponent();
        }

        private void FrmIngredient_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        #region Tải và Làm mới dữ liệu

        private void LoadData(string keyword = "")
        {
            try
            {
                var query = db.NguyenLieus.AsQueryable();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(n => n.TenNL.Contains(keyword) || n.MaNL.Contains(keyword));
                }

                dgvIngredient.DataSource = query
                    .Select(n => new
                    {
                        MaNL = n.MaNL,
                        TenNL = n.TenNL,
                        SoLuongTon = n.SoLuongTon,
                        DonViTinh = n.DonViTinh
                    })
                    .ToList();

                if (dgvIngredient.Columns["MaNL"] != null) dgvIngredient.Columns["MaNL"].HeaderText = "Mã NL";
                if (dgvIngredient.Columns["TenNL"] != null) dgvIngredient.Columns["TenNL"].HeaderText = "Tên Nguyên Liệu";
                if (dgvIngredient.Columns["SoLuongTon"] != null) dgvIngredient.Columns["SoLuongTon"].HeaderText = "Số Lượng Tồn";
                if (dgvIngredient.Columns["DonViTinh"] != null) dgvIngredient.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtIngredientName.Clear();
            nudStockQuantity.Value = 0;
            txtUnit.Clear();
            selectedId = null; // Reset về null
            errorProvider1.Clear();
            txtIngredientName.Focus();
        }

        #endregion

        #region Bắt lỗi nhập dữ liệu

        private bool ValidateInput()
        {
            errorProvider1.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtIngredientName.Text))
            {
                errorProvider1.SetError(txtIngredientName, "Tên nguyên liệu không được để trống!");
                valid = false;
            }

            if (nudStockQuantity.Value < 0)
            {
                errorProvider1.SetError(nudStockQuantity, "Số lượng tồn phải >= 0!");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                errorProvider1.SetError(txtUnit, "Đơn vị tính không được để trống!");
                valid = false;
            }

            return valid;
        }

        #endregion

        #region Xử lý các Sự kiện

        // Nút Lưu (Thêm mới / Cập nhật)
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string tenNL = txtIngredientName.Text.Trim();
            int ton = Convert.ToInt32(nudStockQuantity.Value);
            string donVi = txtUnit.Text.Trim();

            try
            {
                if (!string.IsNullOrEmpty(selectedId)) // Cập nhật (Sửa)
                {
                    bool isDuplicate = db.NguyenLieus.Any(n => n.TenNL.ToLower() == tenNL.ToLower() && n.MaNL != selectedId);
                    if (isDuplicate)
                    {
                        errorProvider1.SetError(txtIngredientName, "Tên nguyên liệu này đã tồn tại!");
                        return;
                    }

                    var nl = db.NguyenLieus.Find(selectedId);
                    if (nl != null)
                    {
                        nl.TenNL = tenNL;
                        nl.SoLuongTon = ton;
                        nl.DonViTinh = donVi;
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Thêm mới
                {
                    bool isDuplicate = db.NguyenLieus.Any(n => n.TenNL.ToLower() == tenNL.ToLower());
                    if (isDuplicate)
                    {
                        errorProvider1.SetError(txtIngredientName, "Nguyên liệu này đã tồn tại trong kho!");
                        return;
                    }

                    // Sinh mã tự động dạng NL001, NL002... để tránh lỗi khóa chính bị Null
                    int count = db.NguyenLieus.Count() + 1;
                    string autoMaNL = "NL" + count.ToString("D3");
                    while (db.NguyenLieus.Any(n => n.MaNL == autoMaNL))
                    {
                        count++;
                        autoMaNL = "NL" + count.ToString("D3");
                    }

                    NguyenLieu nl = new NguyenLieu
                    {
                        MaNL = autoMaNL,
                        TenNL = tenNL,
                        SoLuongTon = ton,
                        DonViTinh = donVi
                    };

                    db.NguyenLieus.Add(nl);
                    db.SaveChanges();
                    MessageBox.Show("Thêm nguyên liệu kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        // Nút Xóa
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedId))
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần xóa từ danh sách!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa nguyên liệu này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var nl = db.NguyenLieus.Find(selectedId);
                    if (nl != null)
                    {
                        db.NguyenLieus.Remove(nl);
                        db.SaveChanges();
                        MessageBox.Show("Xóa nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Nút Làm mới
        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        // Click chọn dòng trên DataGridView - ĐÃ SỬA LỖI ÉP KIỂU
        private void dgvIngredient_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvIngredient.Rows[e.RowIndex].Cells["MaNL"].Value != null)
            {
                var row = dgvIngredient.Rows[e.RowIndex];
                selectedId = row.Cells["MaNL"].Value.ToString(); // Gán trực tiếp chuỗi sang string
                txtIngredientName.Text = row.Cells["TenNL"].Value?.ToString();

                if (row.Cells["SoLuongTon"].Value != null)
                    nudStockQuantity.Value = Convert.ToDecimal(row.Cells["SoLuongTon"].Value);

                txtUnit.Text = row.Cells["DonViTinh"].Value?.ToString();

                errorProvider1.Clear();
            }
        }

        // Ô tìm kiếm realtime
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text.Trim());
        }

        #endregion
    }
}