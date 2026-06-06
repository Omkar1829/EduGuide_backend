using Microsoft.AspNetCore.Mvc;
using EduGuide_Backend.Data;
using Microsoft.EntityFrameworkCore;
namespace EduGuide_Backend.Controllers
{
    [ApiController]
    [Route("api/database")]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DatabaseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            var canConnect = await _context.Database.CanConnectAsync();

            return Ok(new
            {
                success = canConnect,
                connected = canConnect
            });
        }
    }
}
