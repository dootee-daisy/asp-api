using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApiApp.Data
{
    [Table("Loai")]
    public class Loai
    {
        [Key]
        public int MaLoai { get; set; }
        [Required, MaxLength(50)]
        public string TenLoai { get; set; }

        //ICollection<T> là một interface trong .net cung cấp các chức năng cơ bản của một collection như thểm xoá và đếm phần tử
        public virtual ICollection<HangHoa> HangHoas { get; set; }
    }
}
