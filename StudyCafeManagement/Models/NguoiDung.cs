using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("NGUOIDUNG")]
public class NguoiDung
{
    [Key]
    [StringLength(20)]
    [Column("MaNV")]
    public string MaNV { get; set; }

    [Required]
    [StringLength(50)]
    [Column("TenDangNhap")]
    public string TenDangNhap { get; set; }

    [Required]
    [StringLength(256)]
    [Column("MatKhau")]
    public string MatKhau { get; set; }

    [Required]
    [StringLength(20)]
    [Column("MaNhomQuyen")]
    public string MaNhomQuyen { get; set; }

    [Required]
    [StringLength(30)]
    [Column("TrangThai")]
    public string TrangThai { get; set; } = "Đang hoạt động";

    [ForeignKey("MaNhomQuyen")]
    public virtual VaiTro VaiTro { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; }
}