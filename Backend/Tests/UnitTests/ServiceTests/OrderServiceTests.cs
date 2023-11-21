using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Shared.Validation;
using System.Diagnostics;

namespace UnitTests.ServiceTests {
    [TestClass]
    public class OrderServiceTests {
        private TestEnvironment _testEnvironment = TestEnvironment.GetInstance();
        private IOrderService Service => _testEnvironment.OrderService;

        [TestMethod]
        public void TestOrderConcurency() {
            var carId = 1;
            var userId = 13;
            var order1 = new OrderDTO() {
                Customer = new UserDTO() { ID = userId },
                Car = new CarDTO() { ID = carId },
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = new UserDTO() { ID = userId },
                Car = new CarDTO() { ID = carId },
                RentStart = DateTime.Now + new TimeSpan(48, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            var response1 = task1.Result;
            var response2 = task2.Result;
            Debug.WriteLine("Response1: " + ResponseValidator.GetResponseString(response1));
            Debug.WriteLine("Response2: " + ResponseValidator.GetResponseString(response2));
            Assert.IsTrue((ResponseValidator.IsHttpResponseValid(response1) && !ResponseValidator.IsHttpResponseValid(response2)) 
                        || (!ResponseValidator.IsHttpResponseValid(response1) && ResponseValidator.IsHttpResponseValid(response2)));
        }

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
