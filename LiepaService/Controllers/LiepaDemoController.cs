using System.Threading.Tasks;
using LiepaService.Models;
using LiepaService.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LiepaService.Services;

namespace LiepaService.Controllers
{
    [ApiController]
    [Authorize]
    public class LiepaDemoController : ControllerBase
    {
        private readonly ILogger<LiepaDemoController> _logger;
        private readonly IUserDatabaseAccessService _databaseService;
        public LiepaDemoController(ILogger<LiepaDemoController> logger, IUserDatabaseAccessService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet]
        [Route("public/[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> UserInfo([FromQuery(Name = "id")] int id)
        {
            var user = await _databaseService.Get(id);
            return Ok(new UserView(user));
        }

        [HttpPost]//[HttpPut]
        [Route("auth/[action]")]
        public async Task<IActionResult> CreateUser(RequestView requestView)
        {
            if(requestView.User == null)
                return NoContent();

            var userStatus = await _databaseService.GetStatus(requestView.User.Status);
            var user = new User {
                UserId = requestView.User.Id,
                Name = requestView.User.Name,
                StatusId = userStatus.StatusId
            };

            var createdUser = await _databaseService.Put(user);

            return Created("", new UserView(createdUser) );
        }

        [HttpPost]//[HttpDelete]
        [Route("auth/[action]")]
        public async Task<IActionResult> RemoveUser(DeleteRequestView removedUserView)
        {
            var removedUser = await _databaseService.Delete(removedUserView.RemoveUser.Id);

            return Ok(new UserView(removedUser) );
        }

        [HttpPost]//[HttpPatch]
        [Route("auth/[action]")]
        public async Task<IActionResult> SetStatus([FromForm]int id, [FromForm]string newStatus)
        {
            var user = await _databaseService.Get(id);

            var status = await _databaseService.GetStatus(newStatus);

            user.StatusId = status.StatusId;
            user.Status = status;

            var userWithUpdatedStatus = await _databaseService.Update(user);

            return Ok(new UserView(userWithUpdatedStatus));
        }
    }
}
