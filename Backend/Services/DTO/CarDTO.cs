using Shared.Enums;

namespace Services.DTO {
    public class CarDTO {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public int SeatsNumber { get; set; }
        public bool IsOperational { get; set; }
        public DateTime ProductionDate { get; set; }
        public double Horsepower { get; set; }
        public double EngineCapacity { get; set; }
        public FuelTypes FuelType { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double PricePerDay { get; set; }

        public CarDTO() {
            Brand = string.Empty;
            Model = string.Empty;
            RegistrationNumber = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            IsOperational = true;
        }
    }
}
