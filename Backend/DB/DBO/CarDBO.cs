using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class CarDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Brand { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Model { get; set; }
        [Required]
        [MaxLength(10)]
        public virtual string RegistrationNumber { get; set; }
        [Required]
        public virtual int SeatsNumber { get; set; }
        [Required]
        public virtual bool IsOperational { get; set; }
        [Required]
        public virtual DateTime ProductionDate { get; set; }
        [Required]
        public virtual double Horsepower { get; set; }
        [Required]
        public virtual double EngineCapacity { get; set; }
        [Required]
        public virtual FuelTypes FuelType { get; set; }
        [Required]
        [MaxLength(500)]
        public virtual string ShortDescription { get; set; }
        [Required]
        [MaxLength(5000)]
        public virtual string LongDescription { get; set; }
        [Required]
        public virtual double PricePerDay { get; set; }

        public CarDBO() {
            Brand = string.Empty;
            Model = string.Empty;
            RegistrationNumber = string.Empty;
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            IsOperational = true;
        }
    }
}
