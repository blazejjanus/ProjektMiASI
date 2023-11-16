using DB.DBO;
using Services.DTO;
using Shared.Enums;

namespace Services.Interfaces {
    public interface IAuthenticationService {
        public bool IsValid(string jwt);
        public bool IsUser(string jwt, int userID);
        public bool IsUser(string jwt, string username);
        public bool IsUserType(string jwt, UserTypes userType);
        public List<JwtDBO> GetUsersToken(int userID);
        public UserDTO GetUser(string jwt);
        public int GetUserID(string jwt);
        public bool CheckJwtValid(string jwt);
        public bool IsHigherType(string jwt, UserTypes userType);
        public bool IsHigherType(string jwt, int userID);
        public bool IsHigherType(string jwt, string username);
    }
}
