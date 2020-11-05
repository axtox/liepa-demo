using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LiepaService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LiepaService.Controllers
{
    [XmlRoot("User")]
    public class UserView { 
        [XmlAttribute]
        public int Id {get;set;}
        [XmlAttribute]
        public string Name {get;set;}
        public string Status {get;set;}
    }
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
        private readonly LiepaDemoDatabaseContext _context;
        public LiepaDemoController(ILogger<LiepaDemoController> logger, LiepaDemoDatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("public/[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UserInfo(int id)
        {
            var user = await _context.Users.FindAsync(id);
           /* if(user == null)
                return BadRequest();*/
//var ok = Ok(user);
            return Ok (new ResponseResult { 
                User = new UserView {
                Id = user.UserId,
                Name = user.Name,
                Status = user.Status.Value
            }});
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
