﻿using Shared.Enums;
using System.ComponentModel;

namespace Services.DTO {
    public class CarDTO {
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        [DefaultValue(5)]
        public int SeatsNumber { get; set; }
        [DefaultValue(true)]
        public bool IsOperational { get; set; }
        public int ProductionYear { get; set; }
        public double Horsepower { get; set; }
        public double EngineCapacity { get; set; }
        [DefaultValue(FuelTypes.Gasoline)]
        public FuelTypes FuelType { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public double PricePerDay { get; set; }
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public CarDTO() {
            Brand = string.Empty;
            Model = string.Empty;
            RegistrationNumber = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            IsOperational = true;
            IsDeleted = false;
        }
    }
}
