using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API {
    public class ResultTranslation {
        private readonly ILoggingService _loggingService;

        public ResultTranslation(ILoggingService loggingService) {
            _loggingService = loggingService;
        }

        public IActionResult Pass(IActionResult result, string controller, string method, string? message = null) {
            if (result.GetType() == typeof(ObjectResult)) {
                return PassObjectResult((ObjectResult)result, controller, method);
            }
            if (result.GetType() == typeof(StatusCodeResult)) {
                return PassStatusCodeResult((StatusCodeResult)result, controller, method);
            }
            return result;
        }

        private IActionResult PassObjectResult(ObjectResult result, string controller, string method, string? message = null) {
            _loggingService.Log(result.StatusCode ?? 200, controller, method, message);
            return result;
        }

        private IActionResult PassStatusCodeResult(StatusCodeResult result, string controller, string method, string? message = null) {
            _loggingService.Log(result.StatusCode, controller, method, message);
            return result;
        }
    }
}
