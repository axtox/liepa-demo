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
        //[Route("[controller]/[action]")]
        public async Task<ObjectResult> UserInfo()
        {
            //_context.
            var df = _context.Users.First();
            var statise = await _context.UserStatuses.ToListAsync();
            var users = await _context.Users.ToListAsync();

            var p =users[0].Status.Value;
            //var dfd = _context.UserStatuses.Where(p => p.StatusId == df.StatusId);
            return Ok(users);
        }
    }
}
