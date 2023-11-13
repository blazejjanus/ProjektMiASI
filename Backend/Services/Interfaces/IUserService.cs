using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface IUserService {
        public IActionResult AddUser(UserDTO user);
        public IActionResult RemoveUser(string email, bool hard = false);
        public IActionResult RemoveUser(int ID, bool hard = false);
        public IActionResult ModifyUser(UserDTO user);
        public IActionResult GetUser(int ID, bool includeDeleted = false);
        public IActionResult GetUser(string email, bool includeDeleted = false);
        public IActionResult GetAllUsers(bool includeDeleted = false);
    }
}
