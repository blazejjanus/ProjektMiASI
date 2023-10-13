using Microsoft.AspNetCore.Mvc;
using Shared.Validation;
using System.Net;

namespace Shared
{
    public class OperationResult {
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpStatus { get; set; }
        public object? Content { get; set; }

        public OperationResult(bool success = true) {
            if(success) {
                IsSuccess = true;
                HttpStatus = HttpStatusCode.OK;
            } else {
                IsSuccess = false;
                HttpStatus = HttpStatusCode.InternalServerError;
            }
        }

        public OperationResult(HttpStatusCode httpStatus) {
            HttpStatus = httpStatus;
            IsSuccess = ResponseValidator.IsSuccess(httpStatus);
        }

        public OperationResult(HttpStatusCode httpStatus, object? content) {
            IsSuccess = ResponseValidator.IsSuccess(httpStatus);
            HttpStatus = httpStatus;
            Content = content;
        }

        public IActionResult GetResult() {
            if (Content == null) {
                return new StatusCodeResult((int)HttpStatus);
            } else {
                return new ObjectResult(Content) { StatusCode = (int)HttpStatus };
            }
        }
    }
}
