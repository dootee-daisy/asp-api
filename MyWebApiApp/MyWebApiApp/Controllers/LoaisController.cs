using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Data;
using MyWebApiApp.Models;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaisController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LoaisController(MyDbContext context) {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dsLoai = _context.Loais.ToList();
            return Ok(dsLoai);
        }
        [HttpGet("{id}")]
        public IActionResult getById(int id)
        {
            try
            {
                var loai = _context.Loais.SingleOrDefault(l => l.MaLoai == id);
                return loai != null ? Ok(loai) : NotFound();
            }catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult CreateNew(Models.LoaiModel model)
        {
            try
            {
                var loai = new Data.Loai
                {
                    TenLoai = model.TenLoai
                };
                _context.Add(model);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status201Created, loai);
            }catch
            {
                return BadRequest();
            }

        }
        [HttpPut("{id}")]
        public IActionResult UpdateLoaiById(int id, Models.LoaiModel model)
        {
           var loai = _context.Loais.SingleOrDefault(l => l.MaLoai == id);
            if (loai != null)
            {
                loai.TenLoai = model.TenLoai;
                return NoContent();
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteLoaiById(int id, Models.LoaiModel model)
        {
            var loai = _context.Loais.SingleOrDefault(l => l.MaLoai == id);
            if (loai != null)
            {
                _context.Remove(loai);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status200OK);
            }
            return NotFound();
        }
    }
    
       
}
