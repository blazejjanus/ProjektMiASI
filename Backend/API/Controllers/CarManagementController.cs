using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace API.Controllers {
    /// <summary>
    /// Car management controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class CarManagementController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly ICarManagementService _carManagementService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        /// <summary>
        /// CarManagementController constructor
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="carManagementService"></param>
        /// <param name="authenticationService"></param>
        public CarManagementController(ILoggingService loggingService, ICarManagementService carManagementService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _carManagementService = carManagementService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        #region GET
        /// <summary>
        /// Get car by ID
        /// </summary>
        /// <param name="ID">Car ID</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get car by registration number
        /// </summary>
        /// <param name="registrationNumber">Car's registration number</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all cars (only admin or employee)
        /// </summary>
        /// <param name="includeUnoperational">[false] shall the result include unoperational cars</param>
        /// <param name="count">[Optional] number of records to take</param>
        /// <param name="startIndex">[Optional] starting record</param>
        /// <returns></returns>
        [HttpGet("GetAllCars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllCars([FromHeader] bool includeUnoperational = false, [FromHeader] int? count = null, [FromHeader] int? startIndex = null) {
            try {
                return Result.Pass(_carManagementService.GetAllCars(), "CarManagementController", "GetAllCars");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:GetAllCars");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
        #region Car Management
        /// <summary>
        /// Add car (only admin or employee)
        /// </summary>
        /// <param name="car"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpPost("AddCar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCar([FromBody] CarDTO car, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:AddCar: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:AddCar: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.AddCar(car), "CarManagementController", "AddCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:AddCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Update car (only admin or employee)
        /// </summary>
        /// <param name="car"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpPut("UpdateCar")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCar([FromBody] CarDTO car, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:UpdateCar: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if(!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:UpdateCar: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.EditCar(car), "CarManagementController", "UpdateCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:UpdateCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete car (only admin or employee)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCar/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCar([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("CarManagementController:DeleteCar: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:DeleteCar: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.DeleteCar(ID), "CarManagementController", "DeleteCar");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:DeleteCar");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Check if car is ordered (only admin or employee)
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpGet("IsCarOrdered/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult IsCarOrdered([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("CarManagementController:DeleteCar: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_carManagementService.IsCarOrdered(ID), "CarManagementController", "IsCarOrdered");
            } catch (Exception exc) {
                _loggingService.Log(exc, "CarManagementController:IsCarOrdered");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        #endregion
    }
}
