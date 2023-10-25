using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly ILoginService _loginService;
        private ResultTranslation Result { get; }

        public LoginController(ILoggingService loggingService, ILoginService loginService) {
            _loggingService = loggingService;
            _loginService = loginService;
            Result = new ResultTranslation(_loggingService);
        }

        [HttpGet("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromHeader] string username, [FromHeader] string password) {
            try {
                return Result.Pass(_loginService.Login(username, password), "LoginController", "Login");
            } catch (Exception exc) {
                _loggingService.Log(exc, "LoginController:Login");
                return new StatusCodeResult(500);
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] UserDTO user) {
            try {
                return Result.Pass(_loginService.Register(user), "LoginController", "Register");
            } catch (Exception exc) {
                _loggingService.Log(exc, "LoginController:Register");
                return new StatusCodeResult(500);
            }
        }
    }
}
