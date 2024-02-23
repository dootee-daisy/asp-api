using System.ComponentModel.DataAnnotations;
namespace MyWebApiApp.Models
{
    public class HangHoaVM
    {
        [Required(ErrorMessage = "Tên hàng hoá không được để trống")]
        public string TenHangHoa { get; set; }
        public double DonGia {  get; set; }
    }
    public class HangHoa : HangHoaVM
    {
        public Guid MaHangHoa { get; set; }
    }
}
