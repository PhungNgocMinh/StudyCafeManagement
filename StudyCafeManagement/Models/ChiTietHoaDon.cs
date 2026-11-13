using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CHITIETHOADON")]
public class ChiTietHoaDon
{
    [Key, Column(Order = 0, TypeName = "varchar"), StringLength(20)]
    public string MaHD { get; set; }

    [Key, Column(Order = 1, TypeName = "varchar"), StringLength(20)]
    public string MaMon { get; set; }

    [Column("SoLuong")]
    public int SoLuong { get; set; }

    [Column("DonGiaBan", TypeName = "decimal")]
    public decimal DonGiaBan { get; set; }

    [StringLength(255)]
    [Column("GhiChu")]
    public string GhiChu { get; set; }

    [ForeignKey("MaHD")]
    public virtual HoaDon HoaDon { get; set; }

    [ForeignKey("MaMon")]
    public virtual SanPham SanPham { get; set; }
}