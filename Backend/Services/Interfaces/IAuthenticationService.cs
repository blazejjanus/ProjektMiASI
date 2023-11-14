using DB.DBO;
using Services.DTO;
using Shared.Enums;

namespace Services.Interfaces {
    public interface IAuthenticationService {
        public bool IsValid(string jwt);
        public bool IsUser(string jwt, int userID);
        public bool IsUserType(string jwt, UserType userType);
        public List<JwtDBO> GetUsersToken(int userID);
        public UserDTO GetUser(string jwt);
        public int GetUserID(string jwt);
        public bool CheckJwtValid(string jwt);
        public bool IsHigherType(string jwt, UserType userType);
        public bool IsHigherType(string jwt, int userID);
    }
}
