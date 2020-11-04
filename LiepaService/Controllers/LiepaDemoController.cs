using System.Linq;
using System.Threading.Tasks;
using LiepaService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiepaService.Controllers
{
    [ApiController]
    public class LiepaDemoController : ControllerBase
    {
        private readonly ILogger<LiepaDemoController> _logger;
        private readonly LiepaDemoDatabaseContext _context;
        public LiepaDemoController(ILogger<LiepaDemoController> logger, LiepaDemoDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("public/[action]/{id}")]
        public async Task<IActionResult> UserInfo(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return BadRequest();

            return Ok(user);
        }
    }
}
