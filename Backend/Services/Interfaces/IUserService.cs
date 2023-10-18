using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface IUserService {
        public IActionResult AddUser(UserDTO user);
        public IActionResult RemoveUser(string email);
        public IActionResult ModifyUser(UserDTO user);
        public IActionResult GetUser(int ID);
        public IActionResult GetUser(string email);
    }
}
