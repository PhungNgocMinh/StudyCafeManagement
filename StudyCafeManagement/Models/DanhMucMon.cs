using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("DANHMUCMON")]
public class DanhMucMon
{
    [Key]
    [StringLength(20)]
    [Column("MaDanhMuc")]
    public string MaDanhMuc { get; set; }

    [Required]
    [StringLength(100)]
    [Column("TenDanhMuc")]
    public string TenDanhMuc { get; set; }

    public virtual ICollection<SanPham> SanPhams { get; set; }
}