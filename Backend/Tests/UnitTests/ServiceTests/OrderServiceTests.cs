using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Shared.Validation;
using System.Diagnostics;
using System.Text;

namespace UnitTests.ServiceTests {
    [TestClass]
    public class OrderServiceTests {
        private TestEnvironment _testEnvironment = TestEnvironment.GetInstance();
        private IOrderService Service => _testEnvironment.OrderService;

        [TestInitialize]
        public void WriteLogHeader() {
            File.AppendAllText(_testEnvironment.TestLogPath, $"\nOrderServiceTests {DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")} result:\n");
        }

        [TestMethod]
        public void TestOrderConcurency_GetSameCarForSamePeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetSameCarForSamePeriod");
            Assert.IsTrue(IsOneError(task1.Result, task2.Result));
        }

        [TestMethod]
        public void TestOrderConcurency_GetSameCarForMatchingPeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(48, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetSameCarForMatchingPeriod");
            Assert.IsTrue(IsOneError(task1.Result, task2.Result));
        }

        [TestMethod]
        public void TestOrderConcurency_GetSameCarForDifferentPeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(72, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car,
                RentStart = DateTime.Now + new TimeSpan(120, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(192, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetSameCarForDifferentPeriod");
            Assert.IsTrue(IsNoError(task1.Result, task2.Result));
        }

        [TestMethod]
        public void TestOrderConcurency_GetDifferentCarForDifferentPeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car1 = DataMocker.GetNonExistingCar();
            var car2 = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car1,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(72, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car2,
                RentStart = DateTime.Now + new TimeSpan(120, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(192, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetDifferentCarForDifferentPeriod");
            Assert.IsTrue(IsNoError(task1.Result, task2.Result));
        }

        [TestMethod]
        public void TestOrderConcurency_GetDifferentCarForMatchingPeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car1 = DataMocker.GetNonExistingCar();
            var car2 = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car1,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car2,
                RentStart = DateTime.Now + new TimeSpan(48, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetDifferentCarForMatchingPeriod");
            Assert.IsTrue(IsNoError(task1.Result, task2.Result));
        }

        [TestMethod]
        public void TestOrderConcurency_GetDifferentCarForSamePeriod() {
            var user = DataMocker.GetExistingUser(Shared.Enums.UserTypes.CUSTOMER);
            var car1 = DataMocker.GetNonExistingCar();
            var car2 = DataMocker.GetNonExistingCar();
            var order1 = new OrderDTO() {
                Customer = user,
                Car = car1,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var order2 = new OrderDTO() {
                Customer = user,
                Car = car2,
                RentStart = DateTime.Now + new TimeSpan(24, 0, 0),
                RentEnd = DateTime.Now + new TimeSpan(120, 0, 0),
            };
            var task1 = Task.Run(() => Service.AddOrder(order1));
            var task2 = Task.Run(() => Service.AddOrder(order2));
            Task.WaitAll(task1, task2);
            PrintResponses(task1.Result, task2.Result, "TestOrderConcurency_GetDifferentCarForSamePeriod");
            Assert.IsTrue(IsNoError(task1.Result, task2.Result));
        }

        private void PrintResponses(IActionResult service1, IActionResult service2, string sender) {
            var sb = new StringBuilder();
            sb.AppendLine(sender + ":");
            sb.AppendLine("\tResponse1: " + ResponseValidator.GetResponseString(service1));
            sb.AppendLine("\tResponse2: " + ResponseValidator.GetResponseString(service2));
            Debug.WriteLine(sb.ToString());
            File.AppendAllText(_testEnvironment.TestLogPath, sb.ToString());
        }
        private static bool IsOneError(IActionResult service1, IActionResult service2) => IsOneError(ResponseValidator.IsHttpResponseValid(service1), ResponseValidator.IsHttpResponseValid(service2));
        private static bool IsBothError(IActionResult service1, IActionResult service2) => IsBothError(ResponseValidator.IsHttpResponseValid(service1), ResponseValidator.IsHttpResponseValid(service2));
        private static bool IsNoError(IActionResult service1, IActionResult service2) => IsNoError(ResponseValidator.IsHttpResponseValid(service1), ResponseValidator.IsHttpResponseValid(service2));
        private static bool IsOneError(bool status1, bool status2) {
            return ((status1 && !status2) || (!status1 && status2));
        }
        private static bool IsBothError(bool status1, bool status2) {
            return (!status1 && !status2);
        }
        private static bool IsNoError(bool status1, bool status2) {
            return (status1 && status2);
        }
    }
}
