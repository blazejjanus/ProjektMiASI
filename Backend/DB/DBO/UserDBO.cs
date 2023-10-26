using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace DB.DBO {
    public class UserDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Name { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Surname { get; set; }
        [Required]
        public virtual string PasswordHash { get; set; }
        [Required]
        public virtual string PasswordSalt { get; set; }
        [Required]
        public virtual UserType UserType { get; set; }
        [ForeignKey("AddressID")]
        public virtual AddressDBO Address { get; set; }

        public UserDBO() {
            Email = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
            UserType = UserType.CUSTOMER;
            Address = new AddressDBO();
        }
    }
}
