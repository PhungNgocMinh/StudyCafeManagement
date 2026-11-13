using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VAITRO")]
public class VaiTro
{
    [Key]
    [StringLength(20)]
    [Column("MaNhom")]
    public string MaNhom { get; set; }

    [Required]
    [StringLength(50)]
    [Column("TenNhom")]
    public string TenNhom { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("VAITRO")]
public class VaiTro
{
    [Key]
    [StringLength(20)]
    [Column("MaNhom")]
    public string MaNhom { get; set; }

    [Required]
    [StringLength(50)]
    [Column("TenNhom")]
    public string TenNhom { get; set; }

    public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
}