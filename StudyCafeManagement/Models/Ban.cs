using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BAN")]
public class Ban
{
    [Key]
    [StringLength(20)]
    [Column("MaBan")]
    public string MaBan { get; set; }

    [Required]
    [StringLength(50)]
    [Column("TenBan")]
    public string TenBan { get; set; }

    [Column("DonGiaGio", TypeName = "decimal")]
    public decimal DonGiaGio { get; set; } = 15000;

    [Required]
    [StringLength(30)]
    [Column("TrangThai")]
    public string TrangThai { get; set; } = "Trống";

    public virtual ICollection<HoaDon> HoaDons { get; set; }
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("BAN")]
public class Ban
{
    [Key]
    [StringLength(20)]
    [Column("MaBan")]
    public string MaBan { get; set; }

    [Required]
    [StringLength(50)]
    [Column("TenBan")]
    public string TenBan { get; set; }

    [Column("DonGiaGio", TypeName = "decimal")]
    public decimal DonGiaGio { get; set; } = 15000;

    [Required]
    [StringLength(30)]
    [Column("TrangThai")]
    public string TrangThai { get; set; } = "Trống";

    public virtual ICollection<HoaDon> HoaDons { get; set; }
}