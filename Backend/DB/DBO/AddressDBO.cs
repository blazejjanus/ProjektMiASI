using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class AddressDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [MaxLength(64)]
        public virtual string Street { get; set; }
        [Required]
        [MaxLength(8)]
        public virtual string HouseNumber { get; set; }
        public virtual int? ApartmentNumber { get; set; }
        [Required]
        [MaxLength(8)]
        public virtual string PostalCode { get; set; }
        [Required]
        [MaxLength(64)]
        public virtual string City { get; set; }

        public AddressDBO() {
            Street = string.Empty;
            HouseNumber = string.Empty;
            ApartmentNumber = null;
            PostalCode = string.Empty;
            City = string.Empty;
        }
    }
}
