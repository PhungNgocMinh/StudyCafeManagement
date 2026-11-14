using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudyCafeManagement
{
    /// FrmPayment - Chốt giờ, tính tiền bàn + tiền món, giảm 10% khách "Thân thiết",
    /// thanh toán bằng 1 Database Transaction: lưu + khóa hóa đơn, trừ kho, cộng điểm, giải phóng bàn.
    public partial class FrmPayment : Form
    {
        private class Bill
        {
            public decimal Hours, Rate, TienBan, TienMon, TruocGiam, Giam, Tong;
            public int Diem;
        }

        private readonly string _maHD;
        private readonly string _maNV;
        private string _maBan;
        private string _maKH;
        private decimal _rate;
        private DateTime _start;
        private string _hangKhach;
        private List<OrderLine> _lines = new List<OrderLine>();
        private bool _invalid;
        private bool _paying;

        public FrmPayment(string maHD, string maNV)
        {
            InitializeComponent();
            _maHD = maHD;
            _maNV = maNV;
        }

        private bool IsTakeAway { get { return _maBan == null; } }

        private void FrmPayment_Load(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CoffeeShopDbContext())
                {
                    var hd = db.HoaDons.Find(_maHD);
                    if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER)
                    {
                        MessageBox.Show("Hóa đơn không tồn tại hoặc đã được thanh toán/hủy (hóa đơn đã khóa không thể chỉnh sửa).",
                            "Không thể thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        _invalid = true;
                        return;
                    }
                    _maBan = hd.MaBan;
                    _start = hd.NgayLap;
                    _maKH = hd.MaKH;
                    _rate = hd.DonGiaGioApDung;
                    string tenBan = null;
                    if (_maBan != null)
                    {
                        var ban = db.Bans.Find(_maBan);
                        tenBan = ban != null ? ban.TenBan : _maBan;
                        if (_rate <= 0 && ban != null) _rate = ban.DonGiaGio;
                    }
                    _lines = (from c in db.ChiTietHoaDons
                              join s in db.SanPhams on c.MaMon equals s.MaMon
                              where c.MaHD == _maHD
                              orderby s.TenMon
                              select new OrderLine { MaMon = c.MaMon, TenMon = s.TenMon, SoLuong = c.SoLuong, DonGia = c.DonGiaBan, GhiChu = c.GhiChu }).ToList();

                    lblMaHD.Text = "Hóa đơn: " + _maHD;
                    lblBan.Text = IsTakeAway ? "MUA MANG ĐI" : "Bàn: " + tenBan;
                    lblStart.Text = "Bắt đầu: " + _start.ToString("HH:mm dd/MM/yyyy");
                }

                dgvItems.DataSource = _lines;

                if (IsTakeAway)
                {
                    nudHours.Value = 0;
                    nudHours.Enabled = false;
                    lblElapsed.Text = "Không tính tiền bàn";
                    lblRate.Text = "";
                }
                else
                {
                    var el = DateTime.Now - _start;
                    lblElapsed.Text = string.Format("Đã sử dụng: {0} giờ {1} phút", (int)el.TotalHours, el.Minutes);
                    decimal h = OrderHelper.RoundHours(el.TotalHours);
                    nudHours.Value = Math.Min(h, nudHours.Maximum);
                    lblRate.Text = "x " + OrderHelper.Vnd(_rate) + "/giờ  (đã làm tròn lên mỗi " + AppConst.BUOC_GIO.ToString("0.##") + " giờ, có thể chỉnh)";
                }
                LoadCustomer();
                Recalc(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                _invalid = true;
                MessageBox.Show("Không tải được hóa đơn:\n" + OrderHelper.Msg(ex), "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmPayment_Shown(object sender, EventArgs e)
        {
            if (_invalid)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        // KHÁCH HÀNG (THÂN THIẾT 100 ĐIỂM)
        private void LoadCustomer()
        {
            _hangKhach = null;
            if (_maKH == null) { lblKhachHang.Text = "Khách vãng lai"; return; }
            using (var db = new CoffeeShopDbContext())
            {
                var kh = db.KhachHangs.Find(_maKH);
                if (kh == null) { _maKH = null; lblKhachHang.Text = "Khách vãng lai"; return; }
                _hangKhach = kh.HangKhach;
                lblKhachHang.Text = kh.HoTen + " – " + kh.HangKhach + " (" + kh.DiemTichLuy + " điểm)";
            }
        }

        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnFindKH_Click(sender, EventArgs.Empty); }
        }

        private void btnFindKH_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new CoffeeShopDbContext())
                {
                    var kh = OrderHelper.FindCustomerByPhone(db, txtSDT.Text);
                    if (kh == null)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng với SĐT đã nhập.", "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    _maKH = kh.MaKH;
                }
                LoadCustomer();
                Recalc(null, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show(OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearKH_Click(object sender, EventArgs e)
        {
            _maKH = null;
            txtSDT.Clear();
            LoadCustomer();
            Recalc(null, EventArgs.Empty);
        }

        // TÍNH TIỀN
        private static Bill Calc(decimal hours, decimal rate, decimal tienMon, string hangKhach)
        {
            var b = new Bill { Hours = hours, Rate = rate, TienMon = tienMon };
            b.TienBan = Math.Round(hours * rate, 0, MidpointRounding.AwayFromZero);
            b.TruocGiam = b.TienBan + b.TienMon;
            b.Giam = hangKhach == AppConst.HANG_THAN_THIET
                ? Math.Round(b.TruocGiam * AppConst.TY_LE_GIAM, 0, MidpointRounding.AwayFromZero) : 0m;
            b.Tong = b.TruocGiam - b.Giam;
            b.Diem = (int)Math.Floor(b.Tong / AppConst.TIEN_MOI_DIEM);
            return b;
        }

        private void Recalc(object sender, EventArgs e)
        {
            decimal hours = IsTakeAway ? 0m : nudHours.Value;
            var b = Calc(hours, IsTakeAway ? 0m : _rate, _lines.Sum(l => l.ThanhTien), _hangKhach);
            lblCapBan.Text = IsTakeAway ? "Tiền bàn (mua mang đi):"
                : string.Format("Tiền bàn ({0} giờ x {1}):", hours.ToString("0.##"), OrderHelper.Vnd(_rate));
            lblValBan.Text = OrderHelper.Vnd(b.TienBan);
            lblValMon.Text = OrderHelper.Vnd(b.TienMon);
            lblValTam.Text = OrderHelper.Vnd(b.TruocGiam);
            lblValGiam.Text = b.Giam > 0 ? "- " + OrderHelper.Vnd(b.Giam) : "0đ";
            lblValTong.Text = OrderHelper.Vnd(b.Tong);
            lblDiem.Text = _maKH == null
                ? "Khách vãng lai: không tích điểm."
                : "Điểm tích lũy sẽ cộng: +" + b.Diem + " điểm (10.000đ = 1 điểm)";
        }

        // THANH TOÁN
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (_paying) return;
            decimal hours = IsTakeAway ? 0m : nudHours.Value;
            if (!IsTakeAway && hours <= 0)
            {
                MessageBox.Show("Số giờ sử dụng phải lớn hơn 0.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var preview = Calc(hours, IsTakeAway ? 0m : _rate, _lines.Sum(l => l.ThanhTien), _hangKhach);
            if (MessageBox.Show("Xác nhận thanh toán " + OrderHelper.Vnd(preview.Tong) + " cho hóa đơn " + _maHD + "?\n" +
                                "Sau khi thanh toán, hóa đơn sẽ bị KHÓA vĩnh viễn.",
                                "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _paying = true;
            btnPay.Enabled = false;
            try
            {
                Bill bill; int diemCong = 0; int diemMoi = 0; bool lenHang = false; bool coKhach = false;

                using (var db = new CoffeeShopDbContext())
                using (var tx = db.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Đọc lại hóa đơn trong transaction
                        var hd = db.HoaDons.Find(_maHD);
                        if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER)
                            throw new InvalidOperationException("Hóa đơn đã được thanh toán/hủy trước đó.");
                        var lines = db.ChiTietHoaDons.Where(c => c.MaHD == _maHD).ToList();
                        if (hd.MaBan == null && lines.Count == 0)
                            throw new InvalidOperationException("Đơn mang đi phải có ít nhất 1 món.");

                        // 2. Tính tiền
                        KhachHang kh = _maKH == null ? null : db.KhachHangs.Find(_maKH);
                        decimal rate = 0m;
                        if (hd.MaBan != null)
                        {
                            rate = hd.DonGiaGioApDung;
                            if (rate <= 0) rate = db.Bans.Find(hd.MaBan).DonGiaGio;
                        }
                        decimal tienMon = lines.Sum(l => l.SoLuong * l.DonGiaBan);
                        bill = Calc(hd.MaBan == null ? 0m : hours, rate, tienMon, kh == null ? null : kh.HangKhach);

                        // 3. Trừ kho nguyên liệu theo công thức (điều kiện SoLuongTon >= cần dùng để không bị âm)
                        var maMons = lines.Select(l => l.MaMon).Distinct().ToList();
                        var recipes = db.CongThucs.Where(c => maMons.Contains(c.MaMon)).ToList();
                        var need = new Dictionary<string, long>();
                        foreach (var l in lines)
                            foreach (var r in recipes.Where(x => x.MaMon == l.MaMon))
                            {
                                long cur; need.TryGetValue(r.MaNL, out cur);
                                need[r.MaNL] = cur + (long)r.SoLuongCanDung * l.SoLuong;
                            }
                        foreach (var kv in need)
                        {
                            int rows = db.Database.ExecuteSqlCommand(
                                "UPDATE NGUYENLIEU SET SoLuongTon = SoLuongTon - {0} WHERE MaNL = {1} AND SoLuongTon >= {0}",
                                (int)kv.Value, kv.Key);
                            if (rows != 1)
                            {
                                var nl = db.NguyenLieus.Find(kv.Key);
                                throw new InvalidOperationException("Không đủ nguyên liệu \"" + (nl != null ? nl.TenNL : kv.Key) +
                                    "\" trong kho để chế biến các món của hóa đơn này.");
                            }
                        }

                        // 4. Lưu hóa đơn + khóa (chỉ khi còn "Đang order")
                        object maKHParam = kh == null ? (object)DBNull.Value : kh.MaKH;
                        int rowsHd = db.Database.ExecuteSqlCommand(
                            "UPDATE HOADON SET MaKH = {0}, SoGioSuDung = {1}, DonGiaGioApDung = {2}, TongTienTruocGiam = {3}, " +
                            "TienGiamGia = {4}, TongThanhToan = {5}, TrangThai = {6} WHERE MaHD = {7} AND TrangThai = {8}",
                            maKHParam, bill.Hours, bill.Rate, bill.TruocGiam, bill.Giam, bill.Tong,
                            AppConst.HD_DA_THANH_TOAN, _maHD, AppConst.HD_DANG_ORDER);
                        if (rowsHd != 1)
                            throw new InvalidOperationException("Hóa đơn vừa bị thay đổi bởi người khác, vui lòng thử lại.");

                        // 5. Cộng điểm + tự nâng hạng
                        if (kh != null)
                        {
                            coKhach = true;
                            diemCong = bill.Diem;
                            diemMoi = kh.DiemTichLuy + diemCong;
                            lenHang = kh.HangKhach != AppConst.HANG_THAN_THIET && diemMoi >= AppConst.DIEM_LEN_HANG;
                            db.Database.ExecuteSqlCommand(
                                "UPDATE KHACHHANG SET DiemTichLuy = DiemTichLuy + {0}, " +
                                "HangKhach = CASE WHEN DiemTichLuy + {0} >= {1} THEN {2} ELSE HangKhach END WHERE MaKH = {3}",
                                diemCong, AppConst.DIEM_LEN_HANG, AppConst.HANG_THAN_THIET, kh.MaKH);
                        }

                        // 6. Giải phóng bàn
                        if (hd.MaBan != null)
                            db.Database.ExecuteSqlCommand("UPDATE BAN SET TrangThai = {0} WHERE MaBan = {1}", AppConst.BAN_TRONG, hd.MaBan);

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }

                string msg = "Thanh toán thành công!\n\nTổng thanh toán: " + OrderHelper.Vnd(bill.Tong);
                if (bill.Giam > 0) msg += "\n(Đã giảm " + OrderHelper.Vnd(bill.Giam) + " cho khách Thân thiết)";
                if (coKhach) msg += "\nĐiểm tích lũy: +" + diemCong + " (tổng " + diemMoi + " điểm)";
                if (lenHang) msg += "\nKhách đã được nâng lên hạng \"" + AppConst.HANG_THAN_THIET + "\"!";
                if (!IsTakeAway) msg += "\nBàn đã được giải phóng.";
                MessageBox.Show(msg, "Thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thanh toán thất bại, mọi thay đổi đã được hoàn tác:\n" + OrderHelper.Msg(ex),
                    "Lỗi thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnPay.Enabled = true;
            }
            finally { _paying = false; }
        }
    }
}
