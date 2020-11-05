using System.Threading.Tasks;
using System.Xml.Serialization;
using LiepaService.Models;
using LiepaService.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiepaService.Services;

namespace LiepaService.Controllers
{
    [XmlRoot("Response")]
    [XmlType(IncludeInSchema = false, AnonymousType = true)]
    public class ResponseResult  {
  
        [XmlAttribute]
        public bool Success {get; set;}
        
        [XmlAttribute]
        public int ErrorId { get; set;}
        public UserView User{ get; set;}
    }

    [ApiController]
    [Authorize]
    public class LiepaDemoController : ControllerBase
    {
        private readonly ILogger<LiepaDemoController> _logger;
        private readonly IDatabaseAccessService _databaseService;
        public LiepaDemoController(ILogger<LiepaDemoController> logger, IDatabaseAccessService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("public/[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserInfo(int id)
        {
            //var user = await _databaseService.Users.FindAsync(id);
           /* if(user == null)
                return BadRequest();*/
            return Ok();
        }

        [HttpPut]
        [Route("auth/[action]")]
        public async Task<IActionResult> CreateUser()
        {
            /*if(newUser == null)
                return NoContent();*/

            return Created("", null );
        }

        /*private ResponseResult ResponseResult(User user) {
            return new ResponseResult(new UserView {
                Id = user.UserId,
                Name = user.Name,
                Status = user.Status.Value
            });
        }*/
    }
}
