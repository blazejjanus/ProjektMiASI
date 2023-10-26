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

        public static Func<AddressDBO, AddressDBO, bool> Comparator { get; } = (first, second) =>
            first.City == second.City &&
            first.PostalCode == second.PostalCode &&
            first.Street == second.Street &&
            first.HouseNumber == second.HouseNumber &&
            first.ApartmentNumber == second.ApartmentNumber;

        public AddressDBO() {
            Street = string.Empty;
            HouseNumber = string.Empty;
            ApartmentNumber = null;
            PostalCode = string.Empty;
            City = string.Empty;
        }
    }
}
