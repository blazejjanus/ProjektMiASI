using DB;
using DB.DBO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Services.Utils;
using Shared.Configuration;

namespace Services.Services {
    public class UserService : IUserService {
        private Config Config { get; }

        public UserService(Config config) {
            Config = config;
        }

        public IActionResult AddUser(UserDTO user) {
            using (var context = new DataContext(Config)) {
                if (!context.Users.Any(x => x.Email == user.Email)) {
                    var dbo = Mapper.Get().Map<UserDBO>(user);
                    //Hash password
                    using (var hashingHelper = new HashingHelper(Config)) {
                        var result = hashingHelper.HashPassword(user.Password);
                        dbo.PasswordHash = result.Hash;
                        dbo.PasswordSalt = result.Salt;
                    }
                    //Check if address exists in DB
                    if(context.Address.AsEnumerable().Any(x => AddressDBO.Comparator(x, Mapper.Get().Map<AddressDBO>(user.Address)))){
                        dbo.Address = context.Address.AsEnumerable().Single(x => AddressDBO.Comparator(x, Mapper.Get().Map<AddressDBO>(user.Address)));
                    }
                    context.Users.Add(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status201Created);
                } else {
                    return new ObjectResult("User with same username already exists!") { StatusCode = StatusCodes.Status409Conflict };
                }
            }
        }

        public IActionResult GetUser(int ID, bool includeDeleted = false) {
            using (var context = new DataContext(Config)) {
                if (includeDeleted) {
                    if (context.Users.Any(x => x.ID == ID)) {
                        UserDBO dbo = context.Users.Single(x => x.ID == ID);
                        return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                } else {
                    if (context.Users.Any(x => x.ID == ID && !x.IsDeleted)) {
                        UserDBO dbo = context.Users.Single(x => x.ID == ID && !x.IsDeleted);
                        return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                }
            }
        }

        public IActionResult GetUser(string email, bool includeDeleted = false) {
            using (var context = new DataContext(Config)) {
                if (includeDeleted) {
                    if (context.Users.Any(x => x.Email == email)) {
                        UserDBO dbo = context.Users.Single(x => x.Email == email);
                        return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                } else {
                    if (context.Users.Any(x => x.Email == email && !x.IsDeleted)) {
                        UserDBO dbo = context.Users.Single(x => x.Email == email && !x.IsDeleted);
                        return new ObjectResult(Mapper.Get().Map<UserDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                }
            }
        }

        public IActionResult GetAllUsers(bool includeDeleted = false) {
            using (var context = new DataContext(Config)) {
                if (includeDeleted) {
                    return new ObjectResult(Mapper.Get().Map<List<UserDTO>>(context.Users.ToList())) { StatusCode = 200 };
                } else {
                    return new ObjectResult(Mapper.Get().Map<List<UserDTO>>(context.Users.Where(x => !x.IsDeleted).ToList())) { StatusCode = 200 };
                }
            }
        }

        public IActionResult ModifyUser(UserDTO user) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == user.ID && !x.IsDeleted)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == user.ID && !x.IsDeleted);
                    if (!string.IsNullOrEmpty(user.Email)) dbo.Email = user.Email;
                    if (!string.IsNullOrEmpty(user.Name)) dbo.Name = user.Name;
                    if (!string.IsNullOrEmpty(user.Surname)) dbo.Surname = user.Surname;
                    if (!string.IsNullOrEmpty(user.Password)) {
                        //Hash password
                        using (var hashingHelper = new HashingHelper(Config)) {
                            var result = hashingHelper.HashPassword(user.Password);
                            dbo.PasswordHash = result.Hash;
                            dbo.PasswordSalt = result.Salt;
                        }
                    }
                    if(user.Address != null && (!user.Address?.Equals(dbo.Address) ?? false)) {
                        //Check if address exists in DB
                        if (context.Address.AsEnumerable().Any(x => AddressDBO.Comparator(x, Mapper.Get().Map<AddressDBO>(user.Address)))) {
                            dbo.Address = context.Address.AsEnumerable().Single(x => AddressDBO.Comparator(x, Mapper.Get().Map<AddressDBO>(user.Address)));
                        } else {
                            dbo.Address = Mapper.Get().Map<AddressDBO>(user.Address);
                        }
                    }
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RemoveUser(string email, bool hard = false) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.Email == email && !x.IsDeleted)) {
                    UserDBO dbo = context.Users.Single(x => x.Email == email && !x.IsDeleted);
                    if (hard) {
                        context.Users.Remove(dbo);
                    } else {
                        dbo.IsDeleted = true;
                    }
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult RemoveUser(int ID, bool hard = false) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == ID)) {
                    UserDBO dbo = context.Users.Single(x => x.ID == ID);
                    if (hard) {
                        context.Users.Remove(dbo);
                    } else {
                        dbo.IsDeleted = true;
                    }
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
