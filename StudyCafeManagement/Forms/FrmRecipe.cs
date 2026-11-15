using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using StudyCafeManagement;

namespace StudyCafeManagement.Forms
{
    public partial class FrmRecipe : Form
    {
        private readonly CoffeeShopDbContext db = new CoffeeShopDbContext();
        private string selectedMaMon = null;
        private string selectedMaNL = null;

        public FrmRecipe()
        {
            InitializeComponent();
        }

        private void FrmRecipe_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            LoadData();
        }

        private void LoadComboBoxes()
        {
            cboProduct.DataSource = db.SanPhams.ToList();
            cboProduct.DisplayMember = "TenMon";
            cboProduct.ValueMember = "MaMon";

            cboIngredient.DataSource = db.NguyenLieus.ToList();
            cboIngredient.DisplayMember = "TenNL";
            cboIngredient.ValueMember = "MaNL";
        }

        private void LoadData()
        {
            try
            {
                dgvRecipe.DataSource = db.CongThucs.Select(r => new
                {
                    MaMon = r.MaMon,
                    TenMon = r.SanPham.TenMon,
                    MaNL = r.MaNL,
                    TenNL = r.NguyenLieu.TenNL,
                    SoLuongCanDung = r.SoLuongCanDung,
                    DonViTinh = r.NguyenLieu.DonViTinh
                }).ToList();

                if (dgvRecipe.Columns["MaMon"] != null) dgvRecipe.Columns["MaMon"].HeaderText = "Mã Món";
                if (dgvRecipe.Columns["TenMon"] != null) dgvRecipe.Columns["TenMon"].HeaderText = "Tên Món";
                if (dgvRecipe.Columns["MaNL"] != null) dgvRecipe.Columns["MaNL"].HeaderText = "Mã NL";
                if (dgvRecipe.Columns["TenNL"] != null) dgvRecipe.Columns["TenNL"].HeaderText = "Tên Nguyên Liệu";
                if (dgvRecipe.Columns["SoLuongCanDung"] != null) dgvRecipe.Columns["SoLuongCanDung"].HeaderText = "Định Mức Cần";
                if (dgvRecipe.Columns["DonViTinh"] != null) dgvRecipe.Columns["DonViTinh"].HeaderText = "Đơn Vị Tính";

                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetForm()
        {
            nudQuantityNeeded.Value = 1;
            selectedMaMon = null;
            selectedMaNL = null;
            cboProduct.Enabled = true;
            cboIngredient.Enabled = true;
            errorProvider1.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cboProduct.SelectedValue == null || cboIngredient.SelectedValue == null || nudQuantityNeeded.Value <= 0)
            {
                MessageBox.Show("Vui lòng chọn món, nguyên liệu và định mức cần dùng > 0!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string maMon = cboProduct.SelectedValue.ToString();
            string maNL = cboIngredient.SelectedValue.ToString();
            int soLuong = Convert.ToInt32(nudQuantityNeeded.Value);

            try
            {
                if (selectedMaMon != null && selectedMaNL != null) // Cập nhật
                {
                    var recipe = db.CongThucs.Find(selectedMaMon, selectedMaNL);
                    if (recipe != null)
                    {
                        recipe.SoLuongCanDung = soLuong;
                        db.SaveChanges();
                        MessageBox.Show("Cập nhật định mức công thức thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else // Thêm mới
                {
                    bool exists = db.CongThucs.Any(r => r.MaMon == maMon && r.MaNL == maNL);
                    if (exists)
                    {
                        MessageBox.Show("Món này đã có định mức cho nguyên liệu đang chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    CongThuc newRecipe = new CongThuc
                    {
                        MaMon = maMon,
                        MaNL = maNL,
                        SoLuongCanDung = soLuong
                    };
                    db.CongThucs.Add(newRecipe);
                    db.SaveChanges();
                    MessageBox.Show("Thêm công thức mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedMaMon == null || selectedMaNL == null)
            {
                MessageBox.Show("Vui lòng chọn định mức cần xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa định mức công thức này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    var recipe = db.CongThucs.Find(selectedMaMon, selectedMaNL);
                    if (recipe != null)
                    {
                        db.CongThucs.Remove(recipe);
                        db.SaveChanges();
                        MessageBox.Show("Xóa định mức thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvRecipe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRecipe.Rows[e.RowIndex].Cells["MaMon"].Value != null)
            {
                var row = dgvRecipe.Rows[e.RowIndex];
                selectedMaMon = row.Cells["MaMon"].Value.ToString();
                selectedMaNL = row.Cells["MaNL"].Value.ToString();

                cboProduct.SelectedValue = selectedMaMon;
                cboIngredient.SelectedValue = selectedMaNL;
                cboProduct.Enabled = false;
                cboIngredient.Enabled = false;

                nudQuantityNeeded.Value = Convert.ToDecimal(row.Cells["SoLuongCanDung"].Value);
                errorProvider1.Clear();
            }
        }

        private void btnReset_Click(object sender, EventArgs e) => ResetForm();
    }
}