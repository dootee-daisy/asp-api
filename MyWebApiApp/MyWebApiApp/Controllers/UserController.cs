using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyWebApiApp.Data;
using MyWebApiApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly AppSettings _appSettings;

        public UserController(MyDbContext context, IOptionsMonitor<AppSettings> optionsMonitor) {
            _context = context;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel model)
        {
            var user = _context.NguoiDungs.SingleOrDefault(p => p.UserName == model.UserName && p.Password == model.UserName);
            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid usename/password"
                });
            }
            //cap token
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate success",
                Data = GenerateToken(user)
            });
            
        }
        private string GenerateToken(NguoiDung nguoiDung)
        {
            // Tạo một thể hiện của JwtSecurityTokenHandler để tạo và xác minh JWT.
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            // Chuyển đổi secret key từ string sang byte array. Đây sẽ là khóa được sử dụng để ký JWT.
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            // Cài đặt các thông số cho JWT.
            var tokenDescription = new SecurityTokenDescriptor
            {
                // Tạo ClaimsIdentity với một chuỗi các Claim.
                // Claim chứa thông tin nhận dạng và thông tin khác về người dùng.
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nguoiDung.HoTen),
                    new Claim(ClaimTypes.Email, nguoiDung.Email),
                    new Claim("UserName", nguoiDung.UserName),
                    new Claim("Id", nguoiDung.Id.ToString()),

                    //roles

                    //Claim tự định nghĩa chứa một identifier duy nhất cho token (để ngăn chặn việc tái sử dụng token).
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                // Thiết đặt thời gian hết hạn của token.
                Expires = DateTime.UtcNow.AddMinutes(1),
                // Cấu hình thông tin để ký token, sử dụng algorith HMAC SHA-512 và khóa bí mật đã biết đổi trước đó.
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            // Tạo token dựa trên các thông số đã cấu hình.
            var token = jwtTokenHandler.CreateToken(tokenDescription);

            // Chuyển đổi token thành chuỗi và trả lại.
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
