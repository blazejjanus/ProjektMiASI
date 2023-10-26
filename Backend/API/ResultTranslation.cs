using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Enums;
using Shared.Validation;

namespace API {
    public class ResultTranslation {
        private readonly ILoggingService _loggingService;

        public ResultTranslation(ILoggingService loggingService) {
            _loggingService = loggingService;
        }

        public IActionResult Pass(IActionResult result, string controller, string method) {
            if (result.GetType() == typeof(ObjectResult)) {
                return PassObjectResult((ObjectResult)result, controller, method);
            }
            if (result.GetType() == typeof(StatusCodeResult)) {
                return PassStatusCodeResult((StatusCodeResult)result, controller, method);
            }
            return result;
        }

        private IActionResult PassObjectResult(ObjectResult result, string controller, string method) {
            if (!ResponseValidator.IsSuccess(result.StatusCode ?? 200)) {
                string message = controller + ", " + method;
                if (result.StatusCode.HasValue) {
                    message += ": " + result.StatusCode.Value.ToString();
                }
                if (result.Value != null) {
                    message += ": " + result.Value.ToString();
                }
                _loggingService.Log(message, EventType.ERROR);
            }
            return result;
        }

        private IActionResult PassStatusCodeResult(StatusCodeResult result, string controller, string method) {
            if (!ResponseValidator.IsSuccess((int)result.StatusCode)) {
                string message = controller + ", " + method + ": " + result.StatusCode.ToString();
                _loggingService.Log(message, EventType.ERROR);
            }
            return result;
        }
    }
}
