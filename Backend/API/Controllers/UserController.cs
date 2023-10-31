using Microsoft.AspNetCore.Mvc;
using Services.DTO;
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
        #region Get User
        [HttpGet("GetUserByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByID([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:GetUserByID: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.GetUser(ID), "UserController", "GetUserByID");
            } catch(Exception exc) {
                _loggingService.Log(exc, "UserController:GetUserByID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetUserByEmail/{Email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByEmail([FromRoute] string Email, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:GetUserByUsername: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.GetUser(Email), "UserController", "GetUserByID");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:GetUserByEmail");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
        #region Change User
        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:AddUser: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.AddUser(user), "UserController", "AddUser");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:AddUser");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:UpdateUser: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUser(jwt, user.ID)) {
                    _loggingService.Log("UserController:UpdateUser: 403 - provided token is not a token of user being updated.", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_userService.ModifyUser(user), "UserController", "UpdateUser");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:UpdateUser");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteUserByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserByID([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:DeleteUserByID: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.RemoveUser(ID), "UserController", "DeleteUserByID");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:DeleteUserByID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteUserByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserByEmail([FromRoute] string email, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:DeleteUserByEmail: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.RemoveUser(email), "UserController", "DeleteUserByEmail");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:DeleteUserByEmail");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
