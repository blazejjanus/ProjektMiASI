using DB.DBO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Shared;
using Services.Utils;

namespace Services.Services
{
    public class LoginService : ILoginService {
        private readonly IAuthenticationService _authenticationService;
        private Config Config { get; }
        private TokenGenerator Generator { get; }

        public LoginService(IAuthenticationService authenticationService, Config config) {
            _authenticationService = authenticationService;
            Config = config;
            Generator = new TokenGenerator(config);
        }

        public IActionResult Register(UserDTO user) {
            ObjectResult validation = ValidateUser(user);
            if (validation.StatusCode == 200) {
                using (var context = new DataContext()) {
                    if (context.Users.Any(x => x.Email == user.Email)) {
                        return new ObjectResult("User with same email already registered!") { StatusCode = StatusCodes.Status409Conflict };
                    }
                    var dbo = Mapper.Get().Map<UserDBO>(user);
                    context.Add(dbo);
                    context.SaveChanges();
                    var token = Generator.UserToken(dbo);
                    var jwtDBO = new JwtDBO() {
                        User = dbo,
                        JWT = token,
                        Active = true
                    };
                    context.Jwt.Add(jwtDBO);
                    context.SaveChanges();
                    return new ObjectResult(token) { StatusCode = StatusCodes.Status200OK };
                }
            } else {
                return validation;
            }
        }

        public IActionResult Login(string email, string password) {
            using (var context = new DataContext()) {
                if (context.Users.Any(x => x.Email == email)) {
                    UserDBO user = context.Users.Single(x => x.Email == email);
                    if (user.Password == password.Trim()) {
                        string? token = null;
                        if (context.Jwt.Any(x => x.User.ID == user.ID)) {
                            var userTokens = context.Jwt.Where(x => x.User.ID == user.ID).ToList();
                            foreach (var userToken in userTokens) {
                                if (!_authenticationService.CheckJwtValid(userToken.JWT)) {
                                    userToken.Active = false; //Deactivate expired token
                                } else {
                                    token = userToken.JWT;
                                    break;
                                }
                            }
                        }
                        if (token == null) {
                            token = Generator.UserToken(user);
                        }
                        return new ObjectResult(token) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new ObjectResult("Wrong username or password!") { StatusCode = StatusCodes.Status401Unauthorized };
                    }
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        private ObjectResult ValidateUser(UserDTO user) {
            if (user.Email == null || user.Email.Length < 4) {
                return new ObjectResult("Username not valid.") { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            if (!ValidateString(user.Name ?? "")) {
                return new ObjectResult("Name contains forbiden character.") { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            if (!ValidateString(user.Surname ?? "")) {
                return new ObjectResult("Surname contains forbiden character.") { StatusCode = StatusCodes.Status406NotAcceptable };
            }
            return new ObjectResult("") { StatusCode = 200 };
        }

        private bool ValidateString(string str) {
            foreach (char c in str) {
                if (!char.IsLetter(c)) { return false; }
            }
            return true;
        }
    }
}
