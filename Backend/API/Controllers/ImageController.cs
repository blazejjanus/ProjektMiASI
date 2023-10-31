using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly IImageService _imageService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        public ImageController(ILoggingService loggingService, IImageService imageService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _imageService = imageService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        [HttpGet("GetCarMainImage/{CarID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarMainImage([FromRoute] int CarID) {
            try {
                return Result.Pass(_imageService.GetCarMainImage(CarID), "ImageController", "GetCarMainImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:GetCarMainImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetCarImages/{CarID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarImages([FromRoute] int CarID) {
            try {
                return Result.Pass(_imageService.GetCarImages(CarID), "ImageController", "GetCarImages");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:GetCarImages");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("AddCarImage/{CarID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCarImage([FromRoute] int CarID, [FromForm] IFormFile image, [FromHeader] string jwt, [FromHeader] bool? isMain = null) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_imageService.AddCarImage(CarID, image, isMain), "ImageController", "AddCarImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:AddCarImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteImage/{ImageID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteImage([FromRoute] int ImageID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_imageService.DeleteImage(ImageID), "ImageController", "DeleteImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:DeleteImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("EditImage/{ImageID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EditImage([FromRoute] int ImageID, [FromForm] IFormFile image, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserType.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventType.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_imageService.EditImage(ImageID, image), "ImageController", "AddCarImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:AddCarImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
