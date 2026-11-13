using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("NGUYENLIEU")]
public class NguyenLieu
{
    [Key]
    [StringLength(20)]
    [Column("MaNL")]
    public string MaNL { get; set; }

    [Required]
    [StringLength(100)]
    [Column("TenNL")]
    public string TenNL { get; set; }

    [Column("SoLuongTon")]
    public int SoLuongTon { get; set; } = 0;

    [Required]
    [StringLength(30)]
    [Column("DonViTinh")]
    public string DonViTinh { get; set; }

    public virtual ICollection<CongThuc> CongThucs { get; set; }
}