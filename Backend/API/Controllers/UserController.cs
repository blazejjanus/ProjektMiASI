using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        public UserController(ILoggingService loggingService, IUserService userService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _userService = userService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        [HttpGet("GetByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByID([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return new StatusCodeResult(401);
                }
                return Result.Pass(_userService.GetUser(ID), "UserController", "GetUserByID");
            } catch(Exception exc) {
                _loggingService.Log(exc, "GetUserByID");
                return new StatusCodeResult(500);
            }
        }
    }
}
