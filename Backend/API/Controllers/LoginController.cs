using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace API.Controllers {
    /// <summary>
    /// Login controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly ILoginService _loginService;
        private ResultTranslation Result { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="loginService"></param>
        public LoginController(ILoggingService loggingService, ILoginService loginService) {
            _loggingService = loggingService;
            _loginService = loginService;
            Result = new ResultTranslation(_loggingService);
        }

        /// <summary>
        /// Login method
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>User's token</returns>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Login([FromHeader] string email, [FromHeader] string password) {
            try {
                return Result.Pass(_loginService.Login(email, password), "LoginController", "Login");
            } catch (Exception exc) {
                _loggingService.Log(exc, "LoginController:Login");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Register method
        /// </summary>
        /// <param name="user"></param>
        /// <returns>User's token</returns>
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Register([FromBody] UserDTO user) {
            try {
                return Result.Pass(_loginService.Register(user), "LoginController", "Register");
            } catch (Exception exc) {
                _loggingService.Log(exc, "LoginController:Register");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
