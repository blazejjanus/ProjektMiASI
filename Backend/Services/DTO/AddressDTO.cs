using DB.DBO;

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

        public bool Equals(AddressDTO address) {
            if(Street != address.Street) return false;
            if(HouseNumber != address.HouseNumber) return false;
            if(PostalCode != address.PostalCode) return false;
            if(City != address.City) return false;
            return true;
        }

        public bool Equals(AddressDBO address) {
            if (Street != address.Street) return false;
            if (HouseNumber != address.HouseNumber) return false;
            if (PostalCode != address.PostalCode) return false;
            if (City != address.City) return false;
            return true;
        }
    }
}
