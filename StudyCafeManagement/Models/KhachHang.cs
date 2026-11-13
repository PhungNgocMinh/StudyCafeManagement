using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("KHACHHANG")]
public class KhachHang
{
    [Key]
    [StringLength(20)]
    [Column("MaKH")]
    public string MaKH { get; set; }

    [Required]
    [StringLength(100)]
    [Column("HoTen")]
    public string HoTen { get; set; }

    [Required]
    [StringLength(15)]
    [Column("SDT")]
    public string SDT { get; set; }

    [Column("DiemTichLuy")]
    public int DiemTichLuy { get; set; } = 0;

    [Required]
    [StringLength(30)]
    [Column("HangKhach")]
    public string HangKhach { get; set; } = "Thường";

    public virtual ICollection<HoaDon> HoaDons { get; set; }
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("KHACHHANG")]
public class KhachHang
{
    [Key]
    [StringLength(20)]
    [Column("MaKH")]
    public string MaKH { get; set; }

    [Required]
    [StringLength(100)]
    [Column("HoTen")]
    public string HoTen { get; set; }

    [Required]
    [StringLength(15)]
    [Column("SDT")]
    public string SDT { get; set; }

    [Column("DiemTichLuy")]
    public int DiemTichLuy { get; set; } = 0;

    [Required]
    [StringLength(30)]
    [Column("HangKhach")]
    public string HangKhach { get; set; } = "Thường";

    public virtual ICollection<HoaDon> HoaDons { get; set; }
}