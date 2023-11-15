using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface IOrderService {
        IActionResult AddOrder(OrderDTO order);
        IActionResult DeleteOrder(int ID);
        IActionResult EditOrder(OrderDTO order);
        IActionResult GetOrderByID(int ID);
        IActionResult GetAllOrders();
        IActionResult GetUserOrders(int userID);
        IActionResult GetUserOrders(string email);
    }
}
