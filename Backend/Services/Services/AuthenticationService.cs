using DB;
using DB.DBO;
using Services.DTO;
using Services.Interfaces;
using Services.Utils;
using Shared.Configuration;
using Shared.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Services.Services {
    public class AuthenticationService : IAuthenticationService {
        private Config Config { get; }
        public AuthenticationService(Config config) {
            Config = config;
        }

        public bool CheckJwtValid(string jwt) {
            JwtSecurityToken token;
            try {
                token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            } catch (Exception) {
                return false;
            }

            return token.ValidTo > DateTime.UtcNow;
        }

        public List<JwtDBO> GetUsersToken(int userID) {
            var result = new List<JwtDBO>();
            using (var context = new DataContext(Config)) {
                if (context.Jwt.Any(x => x.User.ID == userID && x.Active)) {
                    result = context.Jwt.Where(x => x.User.ID == userID && x.Active).ToList();
                }
            }
            return result;
        }

        public int GetUserID(string jwt) {
            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.ReadToken(jwt) as JwtSecurityToken;
            int id;
            if (int.TryParse(token?.Payload["sub"].ToString(), out id)) {
                return id;
            } else {
                throw new Exception("Malformed token!");
            }
        }

        public UserDTO GetUser(string jwt) {
            int id = GetUserID(jwt);
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == id)) {
                    return Mapper.Get().Map<UserDTO>(context.Users.Single(x => x.ID == id));
                } else {
                    throw new Exception($"Provided JWT token points an user with ID {id}. User with that ID was not found!");
                }
            }
        }

        public bool IsUser(string jwt, int userID) {
            int id = GetUserID(jwt);
            return id == userID;
        }

        public bool IsUser(string jwt, string username) {
            var user = GetUser(jwt);
            return user.Email == username;
        }

        public bool IsUserType(string jwt, UserTypes userType) {
            var user = GetUser(jwt);
            return user.UserType <= userType;
        }

        public bool IsHigherType(string jwt, UserTypes userType) {
            var user = GetUser(jwt);
            return user.UserType < userType;
        }

        public bool IsHigherType(string jwt, int userID) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.ID == userID)) {
                    var user = Mapper.Get().Map<UserDTO>(context.Users.Single(x => x.ID == userID));
                    return IsHigherType(jwt, user.UserType);
                } else {
                    throw new Exception($"Provided JWT token points an user with ID {userID}. User with that ID was not found!");
                }
            }
        }

        public bool IsHigherType(string jwt, string username) {
            using (var context = new DataContext(Config)) {
                if (context.Users.Any(x => x.Email == username)) {
                    var user = Mapper.Get().Map<UserDTO>(context.Users.Single(x => x.Email == username));
                    return IsHigherType(jwt, user.UserType);
                } else {
                    throw new Exception($"Provided JWT token points an user with Email {username}. User with that ID was not found!");
                }
            }
        }

        public bool IsValid(string jwt) {
            if (!CheckJwtValid(jwt)) {
                return false;
            }
            var userTokens = GetUsersToken(GetUserID(jwt));
            if (userTokens == null || userTokens.Count < 1) {
                throw new Exception("JWT user not found!");
            }
            foreach (var userToken in userTokens) {
                if (userToken.JWT == jwt) {
                    return true;
                }
            }
            return false;
        }
    }
}
