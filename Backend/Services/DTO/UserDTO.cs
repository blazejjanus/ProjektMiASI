using Shared.Enums;
using System.Text.Json.Serialization;

namespace Services.DTO {
    public class UserDTO {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserTypes UserType { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDTO? Address { get; set; }
        public bool IsDeleted { get; set; }

        public UserDTO() { 
            Email = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Password = string.Empty;
            UserType = UserTypes.CUSTOMER;
            PhoneNumber = string.Empty;
            IsDeleted = false;
            Address = new AddressDTO();
        }
    }
}
