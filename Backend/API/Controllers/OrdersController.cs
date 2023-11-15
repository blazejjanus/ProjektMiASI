using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Shared.Enums;
using System.Reflection;

namespace API.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase {
        private readonly ILoggingService _loggingService;
        private readonly IOrderService _orderService;
        private readonly IAuthenticationService _authenticationService;
        private ResultTranslation Result { get; }

        public OrdersController(ILoggingService loggingService, IOrderService orderService, IAuthenticationService authenticationService) {
            _loggingService = loggingService;
            _orderService = orderService;
            _authenticationService = authenticationService;
            Result = new ResultTranslation(_loggingService);
        }

        #region GET
        [HttpGet("GetOrderByID/{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetOrderByID([FromRoute] int ID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                return Result.Pass(_orderService.GetOrderByID(ID), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }

        [HttpGet("GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllOrders([FromHeader] string jwt, [FromHeader] OrderStates? orderState = null) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUserType(jwt, UserTypes.EMPLOYEE)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", "Provided token is not a token of admin or employee.");
                }
                return Result.Pass(_orderService.GetAllOrders(orderState), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }
        #region GET For User
        [HttpGet("GetUserOrders{userID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserOrders([FromRoute] int userID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUser(jwt, userID) && !_authenticationService.IsHigherType(jwt, userID)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                return Result.Pass(_orderService.GetUserOrders(userID), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }

        [HttpGet("GetUserOrders{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetUserOrders([FromRoute] string email, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUser(jwt, email) && !_authenticationService.IsHigherType(jwt, email)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                return Result.Pass(_orderService.GetUserOrders(email), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }
        #endregion
        #region GET For Car
        [HttpGet("GetCarOrders{ID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarOrders([FromRoute] int carID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUserType(jwt, UserTypes.EMPLOYEE)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", "Provided token is not a token of admin or employee.");
                }
                return Result.Pass(_orderService.GetCarOrders(carID), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }

        [HttpGet("GetCarOrders{registrationNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCarOrders([FromRoute] string registrationNumber, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUserType(jwt, UserTypes.EMPLOYEE)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", "Provided token is not a token of admin or employee.");
                }
                return Result.Pass(_orderService.GetCarOrders(registrationNumber), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }
        #endregion
        #endregion
        #region POST
        [HttpPost("PostOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddOrder([FromBody] OrderDTO order, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUserType(jwt, UserTypes.EMPLOYEE)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", "Provided token is not a token of admin or employee.");
                }
                return Result.Pass(_orderService.AddOrder(order), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateOrder([FromBody] OrderDTO order, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                if (!_authenticationService.IsUser(jwt, order.Customer.ID) && !_authenticationService.IsHigherType(jwt, UserTypes.CUSTOMER)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                return Result.Pass(_orderService.EditOrder(order), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }

        [HttpPut("CancelOrder{orderID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CancelOrder([FromRoute] int orderID, [FromHeader] string jwt) {
            try {
                if (!_authenticationService.IsValid(jwt)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status401Unauthorized), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                OrderDTO order = (_orderService.GetOrderByID(orderID) as ObjectResult)?.Value as OrderDTO ?? throw new InvalidCastException();
                if (!_authenticationService.IsUser(jwt, order.Customer.ID) && !_authenticationService.IsHigherType(jwt, UserTypes.CUSTOMER)) {
                    return Result.Pass(new StatusCodeResult(StatusCodes.Status403Forbidden), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
                }
                return Result.Pass(_orderService.CancelOrder(orderID), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "");
            } catch (Exception exc) {
                return Result.Pass(new StatusCodeResult(StatusCodes.Status500InternalServerError), GetType().Name, MethodBase.GetCurrentMethod()?.Name ?? "", exc.Message);
            }
        }
        #endregion
    }
}
