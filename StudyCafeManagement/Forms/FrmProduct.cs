using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmProduct : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedId = null;

        public FrmProduct()
        {
            InitializeComponent();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            LoadCategories();
            InitStatusComboBox();
            LoadData();
        }

        private void LoadCategories()
        {
            cboCategory.DataSource = db.DanhMucMons.ToList();
            cboCategory.DisplayMember = "TenDanhMuc";
            cboCategory.ValueMember = "MaDanhMuc";
        }

        private void InitStatusComboBox()
        {
            cboStatus.Items.Clear();
            // Đồng bộ đúng 3 giá trị theo CHECK constraint trong SQL Server: CHECK (TrangThai IN (N'Còn', N'Hết', N'Ngưng bán'))
            cboStatus.Items.AddRange(new string[] { "Còn", "Hết", "Ngưng bán" });
            cboStatus.SelectedIndex = 0;
        }

        private void LoadData(string keyword = "")
        {
            try
            {
                var query = db.SanPhams.AsQueryable();
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(p => p.TenMon.Contains(keyword) || p.MaMon.Contains(keyword));
                }

                dgvProduct.DataSource = query.Select(p => new
                {
                    MaMon = p.MaMon,
                    TenMon = p.TenMon,
                    TenDanhMuc = p.DanhMucMon.TenDanhMuc,
                    DonGia = p.DonGia,
                    TrangThai = p.TrangThai,
                    HinhAnh = p.HinhAnh
                }).ToList();

                if (dgvProduct.Columns["MaMon"] != null) dgvProduct.Columns["MaMon"].HeaderText = "Mã Món";
                if (dgvProduct.Columns["TenMon"] != null) dgvProduct.Columns["TenMon"].HeaderText = "Tên Món";
                if (dgvProduct.Columns["TenDanhMuc"] != null) dgvProduct.Columns["TenDanhMuc"].HeaderText = "Danh Mục";
                if (dgvProduct.Columns["DonGia"] != null) dgvProduct.Columns["DonGia"].HeaderText = "Đơn Giá";
                if (dgvProduct.Columns["TrangThai"] != null) dgvProduct.Columns["TrangThai"].HeaderText = "Trạng Thái";
                if (dgvProduct.Columns["HinhAnh"] != null) dgvProduct.Columns["HinhAnh"].HeaderText = "Hình Ảnh";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            txtProductCode.Clear();
            txtProductCode.ReadOnly = true; // Chỉ hiển thị mã
            txtProductName.Clear();
            nudPrice.Value = 0;
            cboStatus.SelectedIndex = 0;
            txtImageUrl.Clear();
            picProduct.Image = null;
            selectedId = null;
            errorProvider1.Clear();
            txtProductName.Focus();
        }

        private bool ValidateInput()
        {
            errorProvider1.Clear();
            bool valid = true;

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorProvider1.SetError(txtProductName, "Tên món không được để trống!");
                valid = false;
            }
            if (cboCategory.SelectedValue == null)
            {
                errorProvider1.SetError(cboCategory, "Vui lòng chọn danh mục!");
                valid = false;
            }
            if (nudPrice.Value <= 0)
            {
                errorProvider1.SetError(nudPrice, "Đơn giá phải lớn hơn 0!");
                valid = false;
            }
            return valid;
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    txtImageUrl.Text = ofd.FileName;
                    picProduct.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string tenMon = txtProductName.Text.Trim();
            string maDanhMuc = cboCategory.SelectedValue.ToString();
            decimal donGia = nudPrice.Value;
            string trangThai = cboStatus.SelectedItem.ToString();
            string hinhAnh = txtImageUrl.Text;

            try
            {
                if (!string.IsNullOrEmpty(selectedId)) // Cập nhật (Sửa)
                {
                    var prod = db.SanPhams.Find(selectedId);
                    if (prod != null)
                    {
                        prod.TenMon = tenMon;
                        prod.MaDanhMuc = maDanhMuc;
                        prod.DonGia = donGia;
                        prod.TrangThai = trangThai;
                        prod.HinhAnh = hinhAnh;
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Thêm mới (Tự động sinh MaMon dạng MON001, MON002...)
                {
                    int count = db.SanPhams.Count() + 1;
                    string autoMaMon = "MON" + count.ToString("D3");
                    while (db.SanPhams.Any(p => p.MaMon == autoMaMon))
                    {
                        count++;
                        autoMaMon = "MON" + count.ToString("D3");
                    }

                    SanPham newProd = new SanPham
                    {
                        MaMon = autoMaMon,
                        TenMon = tenMon,
                        MaDanhMuc = maDanhMuc,
                        DonGia = donGia,
                        TrangThai = trangThai,
                        HinhAnh = hinhAnh
                    };

                    db.SanPhams.Add(newProd);
                    db.SaveChanges();
                    MessageBox.Show("Thêm sản phẩm mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var prod = db.SanPhams.Find(selectedId);
                    if (prod != null)
                    {
                        db.SanPhams.Remove(prod);
                        db.SaveChanges();
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProduct.Rows[e.RowIndex].Cells["MaMon"].Value != null)
            {
                var row = dgvProduct.Rows[e.RowIndex];
                selectedId = row.Cells["MaMon"].Value.ToString();
                txtProductCode.Text = selectedId; 

                txtProductName.Text = row.Cells["TenMon"].Value?.ToString();
                nudPrice.Value = Convert.ToDecimal(row.Cells["DonGia"].Value);
                cboStatus.SelectedItem = row.Cells["TrangThai"].Value?.ToString();
                txtImageUrl.Text = row.Cells["HinhAnh"].Value?.ToString();

                var categoryName = row.Cells["TenDanhMuc"].Value?.ToString();
                var catObj = db.DanhMucMons.FirstOrDefault(c => c.TenDanhMuc == categoryName);
                if (catObj != null) cboCategory.SelectedValue = catObj.MaDanhMuc;

                if (!string.IsNullOrEmpty(txtImageUrl.Text) && File.Exists(txtImageUrl.Text))
                    picProduct.Image = Image.FromFile(txtImageUrl.Text);
                else
                    picProduct.Image = null;

                errorProvider1.Clear();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => LoadData(txtSearch.Text.Trim());
        private void btnReset_Click(object sender, EventArgs e) => ResetForm();
    }
}