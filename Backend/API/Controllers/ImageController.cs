using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API.Controllers {
    /// <summary>
    /// Image controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly IImageService _imageService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"></param>
        /// <param name="imageService"></param>
        /// <param name="authenticationService"></param>
        public ImageController(ILoggingService loggingService, IImageService imageService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _imageService = imageService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        /// <summary>
        /// Get main image of a car
        /// </summary>
        /// <param name="CarID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all images of a car
        /// </summary>
        /// <param name="CarID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add car iamge (only admin or employee)
        /// </summary>
        /// <param name="CarID"></param>
        /// <param name="file">Base64 encoded image content</param>
        /// <param name="jwt"></param>
        /// <param name="isMain">[Optional] If the image is car's main image. Default null - if car has no images it will be main, true - main, false - not main</param>
        /// <returns></returns>
        [HttpPost("AddCarImage/{CarID}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddCarImage([FromRoute] int CarID, [FromForm] IFormFile file, [FromHeader] string jwt, [FromHeader] bool? isMain = null) { 
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                string imageContent;
                using (var ms = new MemoryStream()) {
                    file.CopyTo(ms);
                    byte[] fileBytes = ms.ToArray();
                    imageContent = Convert.ToBase64String(fileBytes);
                }
                return Result.Pass(_imageService.AddCarImage(CarID, imageContent, isMain), "ImageController", "AddCarImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:AddCarImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Delete car image (only admin or employee)
        /// </summary>
        /// <param name="ImageID"></param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpDelete("DeleteImage/{ImageID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteImage([FromRoute] int ImageID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_imageService.DeleteImage(ImageID), "ImageController", "DeleteImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:DeleteImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Edit car image (only admin or employee)
        /// </summary>
        /// <param name="ImageID"></param>
        /// <param name="imageContent">Base64 encoded image</param>
        /// <param name="jwt"></param>
        /// <returns></returns>
        [HttpPut("EditImage/{ImageID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EditImage([FromRoute] int ImageID, [FromBody] string imageContent, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    _loggingService.Log("ImageController:AddCarImage: 401", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status401Unauthorized);
                }
                if (!_authenticationService.IsUserType(jwt, Shared.Enums.UserTypes.EMPLOYEE)) {
                    _loggingService.Log("ImageController:AddCarImage: 403", Shared.Enums.EventTypes.ERROR);
                    return new StatusCodeResult(StatusCodes.Status403Forbidden);
                }
                return Result.Pass(_imageService.EditImage(ImageID, imageContent), "ImageController", "AddCarImage");
            } catch (Exception exc) {
                _loggingService.Log(exc, "ImageController:AddCarImage");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
