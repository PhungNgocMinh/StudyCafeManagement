using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("SANPHAM")]
public class SanPham
{
    [Key]
    [StringLength(20)]
    [Column("MaMon")]
    public string MaMon { get; set; }

    [Required]
    [StringLength(100)]
    [Column("TenMon")]
    public string TenMon { get; set; }

    [Required]
    [StringLength(20)]
    [Column("MaDanhMuc")]
    public string MaDanhMuc { get; set; }

    [Column("DonGia", TypeName = "decimal")]
    public decimal DonGia { get; set; }

    [StringLength(255)]
    [Column("HinhAnh")]
    public string HinhAnh { get; set; }

    [Required]
    [StringLength(30)]
    [Column("TrangThai")]
    public string TrangThai { get; set; } = "Còn";

    [ForeignKey("MaDanhMuc")]
    public virtual DanhMucMon DanhMucMon { get; set; }

    public virtual ICollection<CongThuc> CongThucs { get; set; }
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
}