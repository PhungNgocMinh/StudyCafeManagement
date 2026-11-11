using System.Data.Entity;

public class CoffeeShopDbContext : DbContext
{
    public CoffeeShopDbContext() : base("name=CoffeeShopDbContext")
    {
    }

    public DbSet<VaiTro> VaiTros { get; set; }
    public DbSet<NguoiDung> NguoiDungs { get; set; }
    public DbSet<DanhMucMon> DanhMucMons { get; set; }
    public DbSet<SanPham> SanPhams { get; set; }
    public DbSet<Ban> Bans { get; set; }
    public DbSet<KhachHang> KhachHangs { get; set; }
    public DbSet<NguyenLieu> NguyenLieus { get; set; }
    public DbSet<CongThuc> CongThucs { get; set; }
    public DbSet<HoaDon> HoaDons { get; set; }
    public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
}