using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace API.Controllers {
    /// <summary>
    /// User controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="userService"></param>
        /// <param name="authenticationService"></param>
        public UserController(ILoggingService loggingService, IUserService userService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _userService = userService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        #region Get User
        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="jwt"></param>
        /// <param name="includeDeleted">[false] Include users marked as deleted.</param>
        /// <returns></returns>
        [HttpGet("GetUserByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByID([FromRoute] int ID, [FromHeader] string jwt, [FromHeader] bool includeDeleted = false) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:GetUserByID: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.GetUser(ID, includeDeleted), "UserController", "GetUserByID");
            } catch(Exception exc) {
                _loggingService.Log(exc, "UserController:GetUserByID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="jwt"></param>
        /// <param name="includeDeleted">[false] Include users marked as deleted.</param>
        /// <returns></returns>
        [HttpGet("GetUserByEmail/{Email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserByEmail([FromRoute] string Email, [FromHeader] string jwt, [FromHeader] bool includeDeleted = false) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:GetUserByUsername: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.GetUser(Email, includeDeleted), "UserController", "GetUserByID");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:GetUserByEmail");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Get all users (only admin or employee)
        /// </summary>
        /// <param name="jwt"></param>
        /// <param name="includeDeleted">[false] Include users marked as deleted.</param>
        /// <returns></returns>
        [HttpGet("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllUsers([FromHeader] string jwt, [FromHeader] bool includeDeleted = false) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:GetAllUsers: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if(!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("UserController:GetAllUsers: 403 - provided token is not a token of admin or employee.", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_userService.GetAllUsers(includeDeleted), "UserController", "GetAllUsers");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:GetAllUsers");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }   
        #endregion
        #region Change User
        /// <summary>
        /// Add user (only admin or employee)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:AddUser: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.AddUser(user), "UserController", "AddUser");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:AddUser");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update user (only admin or employee)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateUser([FromBody] UserDTO user, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:UpdateUser: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUser(jwt, user.ID) && !_authenticationService.IsHigherType(jwt, user.ID)) {
                    _loggingService.Log("UserController:UpdateUser: 403 - provided token is not a token of user being updated.", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_userService.ModifyUser(user), "UserController", "UpdateUser");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:UpdateUser");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete user (only admin or employee)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="jwt"></param>
        /// <param name="deleteHard">[false] true - completely remove from DB, false - mark entity as deleted</param>
        /// <returns></returns>
        [HttpDelete("DeleteUserByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserByID([FromRoute] int ID, [FromHeader] string jwt, [FromHeader] bool deleteHard = false) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:DeleteUserByID: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.RemoveUser(ID, deleteHard), "UserController", "DeleteUserByID");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:DeleteUserByID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete user (only admin or employee)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="jwt"></param>
        /// <param name="deleteHard">[false] true - completely remove from DB, false - mark entity as deleted</param>
        /// <returns></returns>
        [HttpDelete("DeleteUserByEmail/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteUserByEmail([FromRoute] string email, [FromHeader] string jwt, [FromHeader] bool deleteHard = false) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("UserController:DeleteUserByEmail: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                return Result.Pass(_userService.RemoveUser(email, deleteHard), "UserController", "DeleteUserByEmail");
            } catch (Exception exc) {
                _loggingService.Log(exc, "UserController:DeleteUserByEmail");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
