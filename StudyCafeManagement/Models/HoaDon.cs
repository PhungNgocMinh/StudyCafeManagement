using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("HOADON")]
public class HoaDon
{
    [Key]
    [StringLength(20)]
    [Column("MaHD")]
    public string MaHD { get; set; }

    [StringLength(20)]
    [Column("MaBan")]
    public string MaBan { get; set; }

    [Required]
    [StringLength(20)]
    [Column("MaNV")]
    public string MaNV { get; set; }

    [StringLength(20)]
    [Column("MaKH")]
    public string MaKH { get; set; }

    [Column("NgayLap")]
    public DateTime NgayLap { get; set; } = DateTime.Now;

    [Column("SoGioSuDung", TypeName = "decimal")]
    public decimal SoGioSuDung { get; set; } = 0;

    [Column("DonGiaGioApDung", TypeName = "decimal")]
    public decimal DonGiaGioApDung { get; set; } = 0;

    [Column("TongTienTruocGiam", TypeName = "decimal")]
    public decimal TongTienTruocGiam { get; set; } = 0;

    [Column("TienGiamGia", TypeName = "decimal")]
    public decimal TienGiamGia { get; set; } = 0;

    [Column("TongThanhToan", TypeName = "decimal")]
    public decimal TongThanhToan { get; set; } = 0;

    [Required]
    [StringLength(30)]
    [Column("TrangThai")]
    public string TrangThai { get; set; } = "Đang order";

    [ForeignKey("MaBan")]
    public virtual Ban Ban { get; set; }

    [ForeignKey("MaNV")]
    public virtual NguoiDung NguoiDung { get; set; }

    [ForeignKey("MaKH")]
    public virtual KhachHang KhachHang { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
}