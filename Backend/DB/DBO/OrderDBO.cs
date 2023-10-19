using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class OrderDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [ForeignKey("CustomerID")]
        public virtual UserDBO Customer { get; set; }
        [Required]
        [ForeignKey("CarID")]
        public virtual CarDBO Car { get; set; }
        [Required]
        public virtual DateTime RentStart { get; set; }
        [Required]
        public virtual DateTime RentEnd { get; set; }
        public virtual DateTime? CancelationTime { get; set; }

        public OrderDBO() {
            Customer = new UserDBO();
            Car = new CarDBO();
            RentStart = DateTime.Now;
            RentEnd = DateTime.Now;
            CancelationTime = null;
        }
    }
}
