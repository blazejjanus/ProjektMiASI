using DB;
using DB.DBO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Services.Utils;
using Shared;

namespace Services.Services
{
    public class UserService : IUserService {
        private Config Config { get; }

        public UserService(Config config) {
            Config = config;
        }

        public IActionResult AddUser(UserDTO user) {
            using (var context = new DataContext(Config)) {
                if (!context.Users.Any(x => x.Email == user.Email)) {
                    context.Users.Add(Mapper.Get().Map<UserDBO>(user));
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status201Created);
                } else {
                    return new ObjectResult("User with same username already exists!") { StatusCode = StatusCodes.Status409Conflict };
                }
            }
        }

        public IActionResult GetUser(int ID) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == ID);
                    return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetUser(string email) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.Email == email)) {
                    UserDBO dbo = context.Users.Single(x => x.Email == email);
                    return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult ModifyUser(UserDTO user) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.Email == user.Email)) {
                    UserDBO dbo = context.Users.Single(x => x.Email == user.Email);
                    dbo.Email = user.Email;
                    dbo.Name = user.Name;
                    dbo.Surname = user.Surname;
                    dbo.Password = user.Password;
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RemoveUser(string email) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.Email == email)) {
                    UserDBO dbo = context.Users.Single(x => x.Email == email);
                    context.Users.Remove(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RemoveUser(int ID) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == ID);
                    context.Users.Remove(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
