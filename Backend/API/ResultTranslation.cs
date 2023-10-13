using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace API {
    /// <summary>
    /// Class used for translating services results and logging it
    /// </summary>
    public class ResultTranslation {
        private readonly ILoggingService _loggingService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loggingService"></param>
        public ResultTranslation(ILoggingService loggingService) {
            _loggingService = loggingService;
        }

        /// <summary>
        /// Pass services IActionResult result and log it
        /// </summary>
        /// <param name="result">IActionResult result from service</param>
        /// <param name="controller">Controller's name</param>
        /// <param name="method">Method's name</param>
        /// <param name="message">[Optional] Additional logging message</param>
        /// <returns></returns>
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
