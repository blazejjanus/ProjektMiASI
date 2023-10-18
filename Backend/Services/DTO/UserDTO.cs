namespace Services.DTO {
    public class UserDTO {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public AddressDTO Address { get; set; }

        public UserDTO() { 
            Email = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Password = string.Empty;
            Address = new AddressDTO();
        }
    }
}
