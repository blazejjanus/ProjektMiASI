namespace Services.DTO {
    public class AddressDTO {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }

        public AddressDTO() {
            Street = string.Empty;
            HouseNumber = string.Empty;
            ApartmentNumber = null;
            PostalCode = string.Empty;
            City = string.Empty;
        }
    }
}
