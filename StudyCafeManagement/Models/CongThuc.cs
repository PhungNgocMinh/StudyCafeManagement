using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CONGTHUC")]
public class CongThuc
{
    [Key, Column(Order = 0, TypeName = "varchar"), StringLength(20)]
    public string MaMon { get; set; }

    [Key, Column(Order = 1, TypeName = "varchar"), StringLength(20)]
    public string MaNL { get; set; }

    [Column("SoLuongCanDung")]
    public int SoLuongCanDung { get; set; }

    [ForeignKey("MaMon")]
    public virtual SanPham SanPham { get; set; }

    [ForeignKey("MaNL")]
    public virtual NguyenLieu NguyenLieu { get; set; }
}