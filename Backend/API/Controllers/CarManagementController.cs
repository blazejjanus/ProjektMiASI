using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class CarManagementController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly ICarManagementService _carManagementService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        public CarManagementController(ILoggingService loggingService, ICarManagementService carManagementService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _carManagementService = carManagementService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        #region GET
        [HttpGet("GetCarByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarByID([FromRoute] int ID) {
            try {
                return Result.Pass(_carManagementService.GetCarByID(ID), "CarManagementController", "GetCarByID");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:GetCarByID");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetCarByRegistrationNumber/{registrationNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarByRegistrationNumber([FromRoute] string registrationNumber) {
            try {
                return Result.Pass(_carManagementService.GetCarRegistrationNumber(registrationNumber), "CarManagementController", "GetCarByRegistrationNumber");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:GetCarByRegistrationNumber");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
        #region Car Management
        [HttpPost("AddCar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCar([FromBody] CarDTO car, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:AddCar: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:AddCar: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.AddCar(car), "CarManagementController", "AddCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:AddCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("UpdateCar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCar([FromBody] CarDTO car, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:UpdateCar: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if(!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:UpdateCar: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.EditCar(car), "CarManagementController", "UpdateCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:UpdateCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteCar/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCar([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:DeleteCar: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:DeleteCar: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.DeleteCar(ID), "CarManagementController", "DeleteCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:DeleteCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
