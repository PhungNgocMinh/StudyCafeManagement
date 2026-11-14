using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace StudyCafeManagement
{
    /// FrmOrder - Sơ đồ bàn động + gọi món (thuê bàn theo giờ / mua mang đi) + kiểm tra tồn kho theo công thức.

    public partial class FrmOrder : Form
    {
        private class ProductItem
        {
            public SanPham SP;
            public string Text;
            public string Norm;
            public override string ToString() { return Text; }
        }

        private string _maNV;
        private bool _takeAway;                 // đang ở chế độ mua mang đi
        private string _selectedMaBan;          // bàn đang chọn (null nếu mang đi / chưa chọn)
        private string _currentMaHD;            // hóa đơn "Đang order" đang thao tác
        private string _pendingMaKH;            // khách gắn trước cho đơn mang đi chưa tạo
        private DateTime _currentStart;
        private decimal _currentRate;
        private string _currentBanName;
        private bool _busy;
        private bool _loading;

        private List<ProductItem> _allProducts = new List<ProductItem>();
        private List<OrderLine> _lines = new List<OrderLine>();
        private readonly Dictionary<string, Button> _tiles = new Dictionary<string, Button>();
        private readonly ToolTip _tip = new ToolTip();

        private static readonly Color FreeColor = Color.FromArgb(76, 175, 80);
        private static readonly Color BusyColor = Color.FromArgb(229, 57, 53);

        public FrmOrder() : this(null) { }

        /// <param name="maNV">Mã nhân viên đang đăng nhập (lấy từ FrmLogin/FrmMain). Null = lấy nhân viên đang hoạt động đầu tiên (chỉ để test).</param>
        public FrmOrder(string maNV)
        {
            InitializeComponent();
            _maNV = maNV;
        }

        private void FrmOrder_Load(object sender, EventArgs e)
        {
            try
            {
                _loading = true;
                using (var db = new CoffeeShopDbContext())
                {
                    NguoiDung nv = !string.IsNullOrEmpty(_maNV)
                        ? db.NguoiDungs.Find(_maNV)
                        : db.NguoiDungs.FirstOrDefault(u => u.TrangThai == "Đang hoạt động");
                    if (nv == null)
                    {
                        MessageBox.Show("Không tìm thấy nhân viên hợp lệ trong bảng NGUOIDUNG để lập hóa đơn.",
                            "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        BeginInvoke(new Action(Close));
                        return;
                    }
                    _maNV = nv.MaNV;
                    lblNhanVien.Text = "Nhân viên: " + nv.TenDangNhap + " (" + nv.MaNV + ")";

                    var cats = new List<KeyValuePair<string, string>>();
                    cats.Add(new KeyValuePair<string, string>("", "Tất cả danh mục"));
                    foreach (var c in db.DanhMucMons.OrderBy(d => d.TenDanhMuc).ToList())
                        cats.Add(new KeyValuePair<string, string>(c.MaDanhMuc, c.TenDanhMuc));
                    cboCategory.DisplayMember = "Value";
                    cboCategory.ValueMember = "Key";
                    cboCategory.DataSource = cats;
                }
                _loading = false;
                LoadProducts();
                RefreshMap();
                RefreshTakeAwayList(null);
                UpdateInvoiceUI();
                timerRefresh.Start();
            }
            catch (Exception ex)
            {
                _loading = false;
                MessageBox.Show("Không tải được dữ liệu:\n" + OrderHelper.Msg(ex), "Lỗi CSDL",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            using (var db = new CoffeeShopDbContext())
            {
                _allProducts = db.SanPhams.OrderBy(p => p.TenMon).ToList().Select(p => new ProductItem
                {
                    SP = p,
                    Text = p.TenMon + " – " + OrderHelper.Vnd(p.DonGia) + (p.TrangThai == AppConst.MON_CON ? "" : "  [" + p.TrangThai + "]"),
                    Norm = OrderHelper.RemoveDiacritics(p.TenMon)
                }).ToList();
            }
            ApplyProductFilter();
        }

        private void ApplyProductFilter()
        {
            if (_loading) return;
            string cat = cboCategory.SelectedValue as string;
            string kw = OrderHelper.RemoveDiacritics(txtSearch.Text.Trim());
            var list = _allProducts.Where(p =>
                (string.IsNullOrEmpty(cat) || p.SP.MaDanhMuc == cat) &&
                (kw.Length == 0 || p.Norm.Contains(kw))).ToList();
            cboProduct.DataSource = list;
        }

        private void FilterChanged(object sender, EventArgs e) { ApplyProductFilter(); }

        // SƠ ĐỒ BÀN ĐỘNG
        private void RefreshMap()
        {
            int selIdx = tabFloors.SelectedIndex;
            List<Ban> bans;
            Dictionary<string, KeyValuePair<string, DateTime>> open;   // MaBan -> (MaHD, NgayLap)
            using (var db = new CoffeeShopDbContext())
            {
                bans = db.Bans.ToList();
                var inv = db.HoaDons.Where(h => h.TrangThai == AppConst.HD_DANG_ORDER && h.MaBan != null)
                            .Select(h => new { h.MaBan, h.MaHD, h.NgayLap }).ToList();
                open = inv.GroupBy(x => x.MaBan).ToDictionary(g => g.Key,
                    g => { var f = g.OrderByDescending(x => x.NgayLap).First(); return new KeyValuePair<string, DateTime>(f.MaHD, f.NgayLap); });
            }

            tabFloors.SuspendLayout();
            var oldPages = tabFloors.TabPages.Cast<TabPage>().ToList();
            tabFloors.TabPages.Clear();
            foreach (var p in oldPages) p.Dispose();
            _tiles.Clear();

            var infos = bans.Select(OrderHelper.Describe).ToList();
            foreach (var floor in infos.GroupBy(i => i.Tang).OrderBy(g => g.Key))
            {
                var page = new TabPage("Tầng " + floor.Key);
                var flow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = true,
                    Padding = new Padding(8)
                };
                flow.SizeChanged += Flow_SizeChanged;
                page.Controls.Add(flow);
                tabFloors.TabPages.Add(page);

                foreach (var grp in floor.GroupBy(i => i.Loai).OrderBy(g => g.Key))
                {
                    var first = grp.First().Ban;
                    var header = new Label
                    {
                        AutoSize = false,
                        Height = 26,
                        Width = 700,
                        Margin = new Padding(4, 10, 4, 2),
                        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleLeft,
                        Text = string.Format("{0} – {1}/giờ ({2} bàn)", OrderHelper.LoaiText(grp.Key),
                            OrderHelper.Vnd(first.DonGiaGio), grp.Count())
                    };
                    flow.Controls.Add(header);
                    flow.SetFlowBreak(header, true);

                    Button last = null;
                    foreach (var info in grp.OrderBy(i => i.Ban.MaBan, StringComparer.Ordinal))
                    {
                        KeyValuePair<string, DateTime> inv;
                        bool busy = open.TryGetValue(info.Ban.MaBan, out inv) || info.Ban.TrangThai == AppConst.BAN_DANG_PHUC_VU;
                        last = CreateTile(info, busy, open.TryGetValue(info.Ban.MaBan, out inv) ? inv.Key : null,
                                          open.ContainsKey(info.Ban.MaBan) ? (DateTime?)inv.Value : null);
                        flow.Controls.Add(last);
                        _tiles[info.Ban.MaBan] = last;
                    }
                    if (last != null) flow.SetFlowBreak(last, true);
                }
            }
            if (selIdx >= 0 && selIdx < tabFloors.TabPages.Count) tabFloors.SelectedIndex = selIdx;
            tabFloors.ResumeLayout();
            HighlightSelection();
        }

        private void Flow_SizeChanged(object sender, EventArgs e)
        {
            var flow = (FlowLayoutPanel)sender;
            int w = Math.Max(200, flow.ClientSize.Width - 30);
            foreach (Control c in flow.Controls)
                if (c is Label) c.Width = w;
        }

        private Button CreateTile(BanInfo info, bool busy, string maHD, DateTime? start)
        {
            var ban = info.Ban;
            var size = OrderHelper.TileSize(info.Loai);
            var b = new Button
            {
                Size = size,
                Margin = new Padding(6),
                FlatStyle = FlatStyle.Flat,
                BackColor = busy ? BusyColor : FreeColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.25F, FontStyle.Bold),
                UseVisualStyleBackColor = false,
                Tag = info,
                Cursor = Cursors.Hand
            };
            b.FlatAppearance.BorderSize = 2;
            b.FlatAppearance.BorderColor = OrderHelper.TileBorder(info.Loai);
            if (busy)
            {
                b.Text = ban.TenBan + "\n" + (maHD ?? "(không có HĐ)") + "\n" +
                         (start.HasValue ? "Từ " + start.Value.ToString("HH:mm") : "Đang phục vụ");
            }
            else
            {
                b.Text = ban.TenBan + "\n" + OrderHelper.Vnd(ban.DonGiaGio) + "/giờ\nTrống";
            }
            if (info.Loai == LoaiBan.Tron)
            {
                using (var gp = new GraphicsPath())
                {
                    gp.AddEllipse(0, 0, size.Width, size.Height);
                    b.Region = new Region(gp);
                }
            }
            _tip.SetToolTip(b, ban.TenBan + " – " + OrderHelper.LoaiText(info.Loai) + " – " +
                               OrderHelper.Vnd(ban.DonGiaGio) + "/giờ – " + (busy ? "Đang phục vụ" : "Trống"));
            b.Click += Tile_Click;
            return b;
        }

        private void HighlightSelection()
        {
            foreach (var kv in _tiles)
            {
                var info = (BanInfo)kv.Value.Tag;
                bool sel = !_takeAway && _selectedMaBan != null && kv.Key == _selectedMaBan;
                kv.Value.FlatAppearance.BorderSize = sel ? 5 : 2;
                kv.Value.FlatAppearance.BorderColor = sel ? Color.RoyalBlue : OrderHelper.TileBorder(info.Loai);
            }
        }

        // FIX CLICK VÀO BÀN
        private void Tile_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            _busy = true;
            try
            {
                var info = (BanInfo)((Button)sender).Tag;
                string maHD = null;
                string maBan = info.Ban.MaBan;
                using (var db = new CoffeeShopDbContext())
                {
                    var ban = db.Bans.Find(maBan);
                    if (ban == null)
                    {
                        MessageBox.Show("Bàn này không còn tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        RefreshMap();
                        return;
                    }

                    if (ban.TrangThai == AppConst.BAN_TRONG)
                    {
                        var ask = MessageBox.Show(
                            string.Format("Mở phiên thuê bàn \"{0}\" ({1}/giờ)?", ban.TenBan, OrderHelper.Vnd(ban.DonGiaGio)),
                            "Mở bàn", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (ask != DialogResult.Yes) return;
                        maHD = OpenTable(db, ban);
                        if (maHD == null) { RefreshMap(); return; }
                    }
                    else
                    {
                        var hd = db.HoaDons.Where(h => h.MaBan == maBan && h.TrangThai == AppConst.HD_DANG_ORDER)
                                           .OrderByDescending(h => h.NgayLap).FirstOrDefault();
                        if (hd == null)
                        {
                            var ask = MessageBox.Show(
                                "Bàn đang ở trạng thái \"Đang phục vụ\" nhưng không có hóa đơn đang order.\nGiải phóng bàn về \"Trống\"?",
                                "Dữ liệu không đồng bộ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (ask == DialogResult.Yes)
                                db.Database.ExecuteSqlCommand("UPDATE BAN SET TrangThai = {0} WHERE MaBan = {1}", AppConst.BAN_TRONG, maBan);
                            RefreshMap();
                            return;
                        }
                        maHD = hd.MaHD;
                    }
                }
                SelectInvoice(maHD, maBan);
                RefreshMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn bàn:\n" + OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _busy = false; }
        }

        //Mở bàn: đổi bàn sang "Đang phục vụ" + tạo hóa đơn "Đang order" trong 1 transaction. Trả về MaHD hoặc null nếu bàn vừa bị người khác mở. Tránh cả 2 đều mở 1 lúc
        private string OpenTable(CoffeeShopDbContext db, Ban ban)
        {
            using (var tx = db.Database.BeginTransaction())
            {
                // Chỉ đổi được khi bàn còn "Trống" -> chặn mở chồng hóa đơn.
                int rows = db.Database.ExecuteSqlCommand(
                    "UPDATE BAN SET TrangThai = {0} WHERE MaBan = {1} AND TrangThai = {2}",
                    AppConst.BAN_DANG_PHUC_VU, ban.MaBan, AppConst.BAN_TRONG);
                if (rows != 1)
                {
                    tx.Rollback();
                    MessageBox.Show("Bàn này vừa được mở bởi thu ngân khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return null;
                }
                var hd = new HoaDon
                {
                    MaHD = OrderHelper.NewMaHD(db),
                    MaBan = ban.MaBan,
                    MaNV = _maNV,
                    NgayLap = DateTime.Now,
                    SoGioSuDung = 0,
                    DonGiaGioApDung = ban.DonGiaGio,
                    TongTienTruocGiam = 0,
                    TienGiamGia = 0,
                    TongThanhToan = 0,
                    TrangThai = AppConst.HD_DANG_ORDER
                };
                db.HoaDons.Add(hd);
                db.SaveChanges();
                tx.Commit();
                return hd.MaHD;
            }
        }

        // MUA MANG ĐI
        private void btnTakeAway_Click(object sender, EventArgs e)
        {
            _takeAway = true;
            _selectedMaBan = null;
            _currentMaHD = null;
            _pendingMaKH = null;
            _lines = new List<OrderLine>();
            HighlightSelection();
            UpdateInvoiceUI();
            RefreshTakeAwayList(null);
        }

        private void RefreshTakeAwayList(string selectMaHD)
        {
            var list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("", "— Đơn mang đi đang mở —"));
            using (var db = new CoffeeShopDbContext())
            {
                var tk = db.HoaDons.Where(h => h.MaBan == null && h.TrangThai == AppConst.HD_DANG_ORDER)
                           .OrderBy(h => h.NgayLap).Select(h => new { h.MaHD, h.NgayLap }).ToList();
                foreach (var h in tk)
                    list.Add(new KeyValuePair<string, string>(h.MaHD, h.MaHD + " – " + h.NgayLap.ToString("HH:mm dd/MM")));
            }
            _loading = true;
            cboTakeAway.DisplayMember = "Value";
            cboTakeAway.ValueMember = "Key";
            cboTakeAway.DataSource = list;
            string want = selectMaHD ?? (_takeAway ? _currentMaHD : null) ?? "";
            cboTakeAway.SelectedValue = want;
            _loading = false;
        }

        private void cboTakeAway_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_loading || _busy) return;
            string id = cboTakeAway.SelectedValue as string;
            if (string.IsNullOrEmpty(id)) return;
            _takeAway = true;
            SelectInvoice(id, null);
        }

        // NẠP HÓA ĐƠN ĐANG CHỌN
        private void SelectInvoice(string maHD, string maBan)
        {
            _currentMaHD = maHD;
            _selectedMaBan = maBan;
            _takeAway = (maBan == null);
            _pendingMaKH = null;
            LoadInvoice();
            HighlightSelection();
        }

        private string _khText = "Khách vãng lai";

        private void LoadInvoice()
        {
            if (_currentMaHD == null) { UpdateInvoiceUI(); return; }
            using (var db = new CoffeeShopDbContext())
            {
                var hd = db.HoaDons.Find(_currentMaHD);
                if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER)
                {
                    MessageBox.Show("Hóa đơn này không còn ở trạng thái \"Đang order\".", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearSelection();
                    return;
                }
                _currentStart = hd.NgayLap;
                _currentRate = hd.DonGiaGioApDung;
                _currentBanName = null;
                if (hd.MaBan != null)
                {
                    var ban = db.Bans.Find(hd.MaBan);
                    _currentBanName = ban != null ? ban.TenBan : hd.MaBan;
                    if (_currentRate <= 0 && ban != null) _currentRate = ban.DonGiaGio;
                }
                _lines = (from c in db.ChiTietHoaDons
                          join s in db.SanPhams on c.MaMon equals s.MaMon
                          where c.MaHD == _currentMaHD
                          orderby s.TenMon
                          select new OrderLine { MaMon = c.MaMon, TenMon = s.TenMon, SoLuong = c.SoLuong, DonGia = c.DonGiaBan, GhiChu = c.GhiChu }).ToList();
                _khText = DescribeCustomer(hd.MaKH == null ? null : db.KhachHangs.Find(hd.MaKH));
            }
            UpdateInvoiceUI();
        }

        private static string DescribeCustomer(KhachHang kh)
        {
            if (kh == null) return "Khách vãng lai";
            string s = kh.HoTen + " – " + kh.HangKhach + " (" + kh.DiemTichLuy + " điểm)";
            if (kh.HangKhach == AppConst.HANG_THAN_THIET) s += " • giảm 10%";
            return s;
        }

        private void ClearSelection()
        {
            _currentMaHD = null;
            _selectedMaBan = null;
            _takeAway = false;
            _pendingMaKH = null;
            _currentBanName = null;
            _khText = "Khách vãng lai";
            _lines = new List<OrderLine>();
            HighlightSelection();
            UpdateInvoiceUI();
            RefreshTakeAwayList(null);
        }

        private void UpdateInvoiceUI()
        {
            bool has = _currentMaHD != null;
            if (has)
            {
                if (_selectedMaBan != null)
                {
                    lblSelected.Text = "Bàn: " + _currentBanName;
                    lblInvoiceInfo.Text = "Mã HĐ: " + _currentMaHD + "    Bắt đầu: " + _currentStart.ToString("HH:mm dd/MM/yyyy") +
                                          "\nĐơn giá thuê: " + OrderHelper.Vnd(_currentRate) + "/giờ";
                }
                else
                {
                    lblSelected.Text = "MUA MANG ĐI";
                    lblInvoiceInfo.Text = "Mã HĐ: " + _currentMaHD + "    Lập lúc: " + _currentStart.ToString("HH:mm dd/MM/yyyy");
                }
            }
            else if (_takeAway)
            {
                lblSelected.Text = "MUA MANG ĐI";
                lblInvoiceInfo.Text = "Chưa có hóa đơn – hóa đơn sẽ được tạo khi thêm món đầu tiên (bắt buộc ≥ 1 món).";
            }
            else
            {
                lblSelected.Text = "Chưa chọn bàn";
                lblInvoiceInfo.Text = "Bấm vào ô bàn XANH để mở bàn mới, ô bàn ĐỎ để xem/gọi thêm món,\nhoặc bấm \"Mua mang đi\".";
            }
            lblKhachHang.Text = _khText;
            dgvOrder.DataSource = null;
            dgvOrder.DataSource = _lines;

            btnAdd.Enabled = has || _takeAway;
            btnFindKH.Enabled = has || _takeAway;
            btnRemove.Enabled = has;
            btnCancelInvoice.Enabled = has;
            btnPay.Enabled = has;
            UpdateTotals();
        }

        private void UpdateTotals()
        {
            decimal mon = _lines.Sum(l => l.ThanhTien);
            if (_currentMaHD != null && _selectedMaBan != null)
            {
                decimal gio = OrderHelper.RoundHours((DateTime.Now - _currentStart).TotalHours);
                decimal ban = Math.Round(gio * _currentRate, 0, MidpointRounding.AwayFromZero);
                lblTongTien.Text = "Tiền món: " + OrderHelper.Vnd(mon) + "\nTiền bàn tạm tính (" + gio.ToString("0.##") + " giờ): " + OrderHelper.Vnd(ban);
            }
            else
            {
                lblTongTien.Text = "Tiền món: " + OrderHelper.Vnd(mon);
            }
        }

        // THÊM MÓN AND CHECK TỒN KHO
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            var item = cboProduct.SelectedItem as ProductItem;
            if (item == null)
            {
                MessageBox.Show("Hãy chọn món cần gọi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_currentMaHD == null && !_takeAway)
            {
                MessageBox.Show("Hãy chọn một bàn (hoặc bấm \"Mua mang đi\") trước khi gọi món.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _busy = true;
            try
            {
                int qty = (int)nudQty.Value;
                string note = txtNote.Text.Trim();
                string maHD;
                bool gone = false;

                using (var db = new CoffeeShopDbContext())
                {
                    var sp = db.SanPhams.Find(item.SP.MaMon);
                    if (sp == null || sp.TrangThai != AppConst.MON_CON)
                    {
                        MessageBox.Show("Món \"" + item.SP.TenMon + "\" hiện ở trạng thái \"" + (sp == null ? "đã xóa" : sp.TrangThai) +
                            "\" nên không thể gọi.", "Không thể gọi món", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadProducts();
                        return;
                    }

                    HoaDon hd = null;
                    if (_currentMaHD != null)
                    {
                        hd = db.HoaDons.Find(_currentMaHD);
                        if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER) gone = true;
                    }
                    if (gone)
                    {
                        MessageBox.Show("Hóa đơn này đã được thanh toán/hủy, không thể thêm món.", "Hóa đơn bị khóa",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string err = OrderHelper.CheckStock(db, sp.MaMon, qty);
                        if (err != null)
                        {
                            MessageBox.Show("Hết nguyên liệu – không thể gọi \"" + sp.TenMon + "\" x" + qty + ":\n\n" + err,
                                "Hết nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        using (var tx = db.Database.BeginTransaction())
                        {
                            if (hd == null)   // đơn mang đi: tạo hóa đơn cùng lúc với món đầu tiên
                            {
                                string maKH = null;
                                if (_pendingMaKH != null && db.KhachHangs.Any(k => k.MaKH == _pendingMaKH)) maKH = _pendingMaKH;
                                hd = new HoaDon
                                {
                                    MaHD = OrderHelper.NewMaHD(db),
                                    MaBan = null,
                                    MaNV = _maNV,
                                    MaKH = maKH,
                                    NgayLap = DateTime.Now,
                                    SoGioSuDung = 0,
                                    DonGiaGioApDung = 0,
                                    TongTienTruocGiam = 0,
                                    TienGiamGia = 0,
                                    TongThanhToan = 0,
                                    TrangThai = AppConst.HD_DANG_ORDER
                                };
                                db.HoaDons.Add(hd);
                                db.SaveChanges();
                            }

                            // PK của CHITIETHOADON là (MaHD, MaMon) -> mỗi món chỉ 1 dòng: gọi lại thì cộng dồn số lượng.
                            var line = db.ChiTietHoaDons.Find(hd.MaHD, sp.MaMon);
                            if (line == null)
                            {
                                db.ChiTietHoaDons.Add(new ChiTietHoaDon
                                {
                                    MaHD = hd.MaHD,
                                    MaMon = sp.MaMon,
                                    SoLuong = qty,
                                    DonGiaBan = sp.DonGia,
                                    GhiChu = note.Length == 0 ? null : note
                                });
                            }
                            else
                            {
                                line.SoLuong += qty;
                                if (note.Length > 0 && (line.GhiChu == null || line.GhiChu.IndexOf(note, StringComparison.OrdinalIgnoreCase) < 0))
                                {
                                    string merged = string.IsNullOrEmpty(line.GhiChu) ? note : line.GhiChu + "; " + note;
                                    line.GhiChu = merged.Length > 255 ? merged.Substring(0, 255) : merged;
                                }
                            }
                            db.SaveChanges();
                            tx.Commit();
                        }
                    }
                    maHD = hd == null ? null : hd.MaHD;
                }

                if (gone) { ClearSelection(); RefreshMap(); return; }

                bool wasNew = (_currentMaHD == null);
                _currentMaHD = maHD;
                txtNote.Clear();
                nudQty.Value = 1;
                LoadInvoice();
                if (wasNew) RefreshTakeAwayList(maHD);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thêm được món:\n" + OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _busy = false; }
        }

        // XÓA MÓN
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_busy || _currentMaHD == null) return;
            if (dgvOrder.CurrentRow == null) return;
            var line = dgvOrder.CurrentRow.DataBoundItem as OrderLine;
            if (line == null) return;

            if (_selectedMaBan == null && _lines.Count == 1)
            {
                var ask = MessageBox.Show("Đơn mang đi phải có ít nhất 1 món.\nBạn muốn HỦY luôn hóa đơn này?",
                    "Xóa món cuối", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ask == DialogResult.Yes) DoCancelInvoice();
                return;
            }
            if (MessageBox.Show("Xóa \"" + line.TenMon + "\" (x" + line.SoLuong + ") khỏi hóa đơn?", "Xóa món",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _busy = true;
            try
            {
                using (var db = new CoffeeShopDbContext())
                {
                    var hd = db.HoaDons.Find(_currentMaHD);
                    if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER)
                    {
                        MessageBox.Show("Hóa đơn đã bị khóa, không thể chỉnh sửa.", "Hóa đơn bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var ct = db.ChiTietHoaDons.Find(_currentMaHD, line.MaMon);
                        if (ct != null)
                        {
                            db.ChiTietHoaDons.Remove(ct);
                            db.SaveChanges();
                        }
                    }
                }
                LoadInvoice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không xóa được món:\n" + OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _busy = false; }
        }

        // HỦY HÓA ĐƠN
        private void btnCancelInvoice_Click(object sender, EventArgs e)
        {
            if (_busy || _currentMaHD == null) return;
            if (MessageBox.Show("Hủy hóa đơn " + _currentMaHD + "?\nBàn (nếu có) sẽ được giải phóng về \"Trống\".",
                    "Hủy hóa đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            DoCancelInvoice();
        }

        private void DoCancelInvoice()
        {
            _busy = true;
            try
            {
                using (var db = new CoffeeShopDbContext())
                {
                    var hd = db.HoaDons.Find(_currentMaHD);
                    string maBan = hd == null ? null : hd.MaBan;
                    using (var tx = db.Database.BeginTransaction())
                    {
                        int rows = db.Database.ExecuteSqlCommand(
                            "UPDATE HOADON SET TrangThai = {0} WHERE MaHD = {1} AND TrangThai = {2}",
                            AppConst.HD_DA_HUY, _currentMaHD, AppConst.HD_DANG_ORDER);
                        if (rows != 1)
                        {
                            tx.Rollback();
                            MessageBox.Show("Hóa đơn đã bị khóa (đã thanh toán/hủy), không thể hủy.", "Hóa đơn bị khóa",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (maBan != null)
                                db.Database.ExecuteSqlCommand("UPDATE BAN SET TrangThai = {0} WHERE MaBan = {1}", AppConst.BAN_TRONG, maBan);
                            tx.Commit();
                        }
                    }
                }
                ClearSelection();
                RefreshMap();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không hủy được hóa đơn:\n" + OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { _busy = false; }
        }

        // KHÁCH HÀNG
        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; btnFindKH_Click(sender, EventArgs.Empty); }
        }

        private void btnFindKH_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            if (_currentMaHD == null && !_takeAway)
            {
                MessageBox.Show("Hãy chọn bàn hoặc đơn mang đi trước.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                string phone = OrderHelper.NormalizePhone(txtSDT.Text);
                using (var db = new CoffeeShopDbContext())
                {
                    KhachHang kh = OrderHelper.FindCustomerByPhone(db, phone);
                    if (kh == null && phone.Length > 0)
                    {
                        MessageBox.Show("Không tìm thấy khách hàng có SĐT " + phone + ".\nHãy thêm khách tại màn hình quản lý khách hàng.",
                            "Không tìm thấy", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    if (_currentMaHD != null)
                    {
                        var hd = db.HoaDons.Find(_currentMaHD);
                        if (hd == null || hd.TrangThai != AppConst.HD_DANG_ORDER)
                        {
                            MessageBox.Show("Hóa đơn đã bị khóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        hd.MaKH = kh == null ? null : kh.MaKH;   // để trống SĐT rồi bấm = bỏ gắn khách
                        db.SaveChanges();
                    }
                    else
                    {
                        _pendingMaKH = kh == null ? null : kh.MaKH;
                        _khText = DescribeCustomer(kh);
                        lblKhachHang.Text = _khText;
                    }
                }
                if (_currentMaHD != null) LoadInvoice();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gắn khách hàng:\n" + OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // THANH TOÁN
        private void btnPay_Click(object sender, EventArgs e)
        {
            if (_busy || _currentMaHD == null) return;
            if (_selectedMaBan == null && _lines.Count == 0)
            {
                MessageBox.Show("Đơn mang đi phải có ít nhất 1 món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _busy = true;
            try
            {
                using (var f = new FrmPayment(_currentMaHD, _maNV))
                {
                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        ClearSelection();
                        RefreshMap();
                        LoadProducts();
                    }
                    else
                    {
                        LoadInvoice();
                    }
                }
            }
            finally { _busy = false; }
        }

        // Refresher Orb (30s cooldown)
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_busy) return;
            try
            {
                LoadProducts();
                RefreshMap();
                RefreshTakeAwayList(null);
                if (_currentMaHD != null) LoadInvoice();
            }
            catch (Exception ex)
            {
                MessageBox.Show(OrderHelper.Msg(ex), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (_busy || !Visible) return;
            try
            {
                RefreshMap();
                UpdateTotals();
            }
            catch { }
        }
    }
}
