using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;

namespace UnitTests.ServiceTests {
    [TestClass]
    public class OrderServiceTests {
        private TestEnvironment _testEnvironment = TestEnvironment.GetInstance();
        private IOrderService Service => _testEnvironment.OrderService;

        [TestMethod]
        public void AddOrder_ShouldReturnCreatedResult_WhenOrderIsAddedSuccessfully() {
            var validOrder = new OrderDTO {
                //TODO: Set valid data for the test
            };
            var result = Service.AddOrder(validOrder);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result;
            Assert.AreEqual(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public void AddOrder_ShouldReturnBadRequestResult_WhenOrderValidationFails() {
            var invalidOrder = new OrderDTO {
                //TODO: Set invalid data for the test
            };
            var result = Service.AddOrder(invalidOrder);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        public void AddOrder_ShouldReturnNotFoundResult_WhenUserNotFound() {
            var orderWithNonexistentUser = new OrderDTO {
                //TODO: Set valid data for the test
            };
            var result = Service.AddOrder(orderWithNonexistentUser);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [TestMethod]
        public void AddOrder_ShouldReturnNotFoundResult_WhenCarNotFound() {
            var orderWithNonexistentCar = new OrderDTO {
                //TODO: Set data for the test with a non-existing car
            };
            var result = Service.AddOrder(orderWithNonexistentCar);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result;
            Assert.AreEqual(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }
    }
}
