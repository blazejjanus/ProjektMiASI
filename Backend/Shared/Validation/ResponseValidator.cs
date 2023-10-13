using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Shared.Validation {
    public static class ResponseValidator {
        public static bool IsSuccess(HttpStatusCode code) {
            if ((int)code >= 200 && (int)code < 300) {
                return true;
            }
            return false;
        }

        public static bool IsSuccess(int code) {
            if (code >= 200 && code < 300) {
                return true;
            }
            return false;
        }

        public static bool IsSuccess(int? code) {
            if (code == null) {
                return false;
            }
            if (code >= 200 && code < 300) {
                return true;
            }
            return false;
        }

        public static string GetResponseString(IActionResult result) {
            var text = "";
            if (result is ObjectResult) {
                text = ((ObjectResult)result).StatusCode.ToString() ?? "";
                var message = ((ObjectResult)result).Value.ToString();
                if(!string.IsNullOrEmpty(message)) {
                    text += ": " + message;
                }
            }
            if (result is StatusCodeResult) {
                text = ((StatusCodeResult)result).StatusCode.ToString() ?? "";
            }
            return text;
        }

        public static bool IsHttpResponseValid(IActionResult result) {
            if (result is ObjectResult)
                return IsSuccess(((ObjectResult)result).StatusCode);
            if (result is StatusCodeResult)
                return IsSuccess(((StatusCodeResult)result).StatusCode);
            if (result is FileContentResult)
                return true;
            return false;
        }

        public static int? GetStatusCode(IActionResult result) {
            if (result is ObjectResult)
                return ((ObjectResult)result).StatusCode;
            if (result is StatusCodeResult)
                return ((StatusCodeResult)result).StatusCode;
            if (result is FileContentResult)
                return 200;
            return null;
        }
    }
}
