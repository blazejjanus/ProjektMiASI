using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Shared.Enums;

namespace Services.Interfaces {
    public interface IOrderService {
        public IActionResult AddOrder(OrderDTO order);
        public IActionResult EditOrder(OrderDTO order);
        public IActionResult CancelOrder(int ID);
        public IActionResult GetOrderByID(int ID);
        public IActionResult GetAllOrders(OrderStates? state = null);
        public IActionResult GetUserOrders(int userID);
        public IActionResult GetUserOrders(string email);
        public IActionResult GetCarOrders(int carID);
        public IActionResult GetCarOrders(string registrationNumber);
    }
}
