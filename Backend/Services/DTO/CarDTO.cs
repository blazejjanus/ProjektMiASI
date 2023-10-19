namespace Services.DTO {
    public class CarDTO {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int SeatsNumber { get; set; }
        public bool IsOperational { get; set; }

        public CarDTO() {
            Brand = string.Empty;
            Model = string.Empty;
            RegistrationNumber = string.Empty;
            IsOperational = true;
        }
    }

}
