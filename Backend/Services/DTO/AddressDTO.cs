namespace Services.DTO {
    public class AddressDTO {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public AddressDTO() {
            Street = string.Empty;
            HouseNumber = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;
        }
    }
}
