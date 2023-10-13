using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface ILoginService {
        public IActionResult Register(UserDTO user);
        public IActionResult Login(string email, string password);
    }
}
