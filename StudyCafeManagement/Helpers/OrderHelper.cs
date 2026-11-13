using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StudyCafeManagement
{
    /// <summary>Các hằng số nghiệp vụ dùng chung cho FrmOrder / FrmPayment.</summary>
    public static class AppConst
    {
        public const string BAN_TRONG = "Trống";
        public const string BAN_DANG_PHUC_VU = "Đang phục vụ";

        public const string HD_DANG_ORDER = "Đang order";
        public const string HD_DA_THANH_TOAN = "Đã thanh toán";
        public const string HD_DA_HUY = "Đã hủy";

        public const string HANG_THUONG = "Thường";
        public const string HANG_THAN_THIET = "Thân thiết";

        public const string MON_CON = "Còn";

        /// <summary>Tỷ lệ chiết khấu cho khách hạng "Thân thiết".</summary>
        public const decimal TY_LE_GIAM = 0.10m;
        /// <summary>Mỗi 10.000đ chi tiêu = 1 điểm.</summary>
        public const decimal TIEN_MOI_DIEM = 10000m;
        /// <summary>Đủ ngần này điểm thì tự nâng lên hạng "Thân thiết" (CHỈNH THEO QUY ĐỊNH CỦA QUÁN).</summary>
        public const int DIEM_LEN_HANG = 100;
        /// <summary>Bước làm tròn số giờ thuê bàn (giờ). 0.5 = làm tròn lên mỗi 30 phút.</summary>
        public const decimal BUOC_GIO = 0.5m;
    }

    public enum LoaiBan { Don = 1, Doi = 2, Tron = 3, Nhom = 4, Lon = 5 }

    /// <summary>Thông tin hiển thị của bàn (tầng + loại), suy ra từ MaBan dạng T1-D01 (T{tầng}-{loại}{số}).</summary>
    public class BanInfo
    {
        public Ban Ban;
        public int Tang;
        public LoaiBan Loai;
    }

    /// <summary>Một dòng món trong hóa đơn (dùng cho DataGridView).</summary>
    public class OrderLine
    {
        public string MaMon { get; set; }
        public string TenMon { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string GhiChu { get; set; }
        public decimal ThanhTien { get { return SoLuong * DonGia; } }
    }

    public static class OrderHelper
    {
        private static readonly CultureInfo Vi = GetVi();
        private static CultureInfo GetVi()
        {
            try { return new CultureInfo("vi-VN"); } catch { return CultureInfo.InvariantCulture; }
        }

        public static string Vnd(decimal d)
        {
            return d.ToString("N0", Vi) + "đ";
        }

        public static string Msg(Exception ex)
        {
            while (ex.InnerException != null) ex = ex.InnerException;
            return ex.Message;
        }

        // ---------- Phân loại bàn ----------
        private static readonly Regex MaBanRegex = new Regex(@"^T(\d+)-([DPRCL])(\d+)$", RegexOptions.IgnoreCase);

        public static BanInfo Describe(Ban b)
        {
            var info = new BanInfo { Ban = b, Tang = 1, Loai = LoaiBan.Don };
            var m = MaBanRegex.Match(b.MaBan ?? "");
            if (m.Success)
            {
                info.Tang = int.Parse(m.Groups[1].Value);
                switch (char.ToUpperInvariant(m.Groups[2].Value[0]))
                {
                    case 'D': info.Loai = LoaiBan.Don; break;
                    case 'P': info.Loai = LoaiBan.Doi; break;
                    case 'R': info.Loai = LoaiBan.Tron; break;
                    case 'C': info.Loai = LoaiBan.Nhom; break;
                    default: info.Loai = LoaiBan.Lon; break;
                }
            }
            else
            {
                // Mã bàn không theo quy ước: đoán loại theo đơn giá giờ, mặc định tầng 1.
                decimal g = b.DonGiaGio;
                if (g <= 12000m) info.Loai = LoaiBan.Don;
                else if (g <= 28000m) info.Loai = LoaiBan.Doi;
                else if (g <= 70000m) info.Loai = LoaiBan.Tron;
                else if (g <= 175000m) info.Loai = LoaiBan.Nhom;
                else info.Loai = LoaiBan.Lon;
            }
            return info;
        }

        public static string LoaiText(LoaiBan l)
        {
            switch (l)
            {
                case LoaiBan.Don: return "Bàn đơn";
                case LoaiBan.Doi: return "Bàn đôi";
                case LoaiBan.Tron: return "Bàn tròn (3-8 người)";
                case LoaiBan.Nhom: return "Bàn 10-20 người";
                default: return "Bàn 50 người";
            }
        }

        public static Size TileSize(LoaiBan l)
        {
            switch (l)
            {
                case LoaiBan.Don: return new Size(88, 66);
                case LoaiBan.Doi: return new Size(108, 76);
                case LoaiBan.Tron: return new Size(108, 108);
                case LoaiBan.Nhom: return new Size(160, 90);
                default: return new Size(260, 100);
            }
        }

        public static Color TileBorder(LoaiBan l)
        {
            switch (l)
            {
                case LoaiBan.Nhom: return Color.DarkOrange;
                case LoaiBan.Lon: return Color.Firebrick;
                default: return Color.Black;
            }
        }

        // ---------- Mã hóa đơn ----------
        public static string NewMaHD(CoffeeShopDbContext db)
        {
            var ids = db.HoaDons.Where(h => h.MaHD.StartsWith("HD")).Select(h => h.MaHD).ToList();
            long max = 0;
            foreach (var id in ids)
            {
                long n;
                if (long.TryParse(id.Substring(2), out n) && n > max) max = n;
            }
            return "HD" + (max + 1).ToString("D5");
        }

        // ---------- Tồn kho theo công thức ----------
        /// <summary>Lượng nguyên liệu đã "giữ chỗ" bởi TẤT CẢ hóa đơn đang order (chưa trừ kho).</summary>
        public static Dictionary<string, long> ReservedStock(CoffeeShopDbContext db)
        {
            var lines = (from c in db.ChiTietHoaDons
                         join h in db.HoaDons on c.MaHD equals h.MaHD
                         where h.TrangThai == AppConst.HD_DANG_ORDER
                         select new { c.MaMon, c.SoLuong }).ToList();
            var recipes = db.CongThucs.ToList().ToLookup(r => r.MaMon);
            var res = new Dictionary<string, long>();
            foreach (var l in lines)
            {
                foreach (var r in recipes[l.MaMon])
                {
                    long cur;
                    res.TryGetValue(r.MaNL, out cur);
                    res[r.MaNL] = cur + (long)r.SoLuongCanDung * l.SoLuong;
                }
            }
            return res;
        }

        /// <summary>Kiểm tra thêm addQty phần món maMon có đủ nguyên liệu không. Trả về null nếu đủ, ngược lại là thông báo.</summary>
        public static string CheckStock(CoffeeShopDbContext db, string maMon, int addQty)
        {
            var recipe = db.CongThucs.Where(c => c.MaMon == maMon).ToList();
            if (recipe.Count == 0) return null; // món không có công thức -> không kiểm tra kho
            var reserved = ReservedStock(db);
            var stock = db.NguyenLieus.ToList().ToDictionary(n => n.MaNL);
            var sb = new StringBuilder();
            foreach (var ct in recipe)
            {
                NguyenLieu nl;
                if (!stock.TryGetValue(ct.MaNL, out nl))
                {
                    sb.AppendLine("- Nguyên liệu " + ct.MaNL + " không có trong kho.");
                    continue;
                }
                long res;
                reserved.TryGetValue(ct.MaNL, out res);
                long need = (long)ct.SoLuongCanDung * addQty;
                long available = nl.SoLuongTon - res;
                if (need > available)
                    sb.AppendLine(string.Format("- {0}: cần {1} {2}, chỉ còn khả dụng {3} {2}.",
                        nl.TenNL, need, nl.DonViTinh, Math.Max(0, available)));
            }
            return sb.Length == 0 ? null : sb.ToString().TrimEnd();
        }

        // ---------- Khách hàng ----------
        public static string NormalizePhone(string s)
        {
            if (s == null) return "";
            return new string(s.Where(char.IsDigit).ToArray());
        }

        public static KhachHang FindCustomerByPhone(CoffeeShopDbContext db, string phone)
        {
            string p = NormalizePhone(phone);
            if (p.Length == 0) return null;
            return db.KhachHangs.FirstOrDefault(k => k.SDT == p);
        }

        public static string RemoveDiacritics(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string d = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char c in d)
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) sb.Append(c);
            return sb.ToString().Replace('đ', 'd').Replace('Đ', 'D').ToLowerInvariant();
        }

        public static decimal RoundHours(double elapsedHours)
        {
            decimal step = AppConst.BUOC_GIO;
            decimal h = Math.Ceiling((decimal)elapsedHours / step) * step;
            return h < step ? step : h;
        }
    }
}
