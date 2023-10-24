using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class ImageDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        public virtual DateTime CreationDate { get; set; }
        [Required]
        public virtual byte[] Content { get; set; }
        [Required]
        public virtual bool IsMain { get; set; }
        [Required]
        [ForeignKey("CarID")]
        public virtual CarDBO Car { get; set; }

        public ImageDBO() { 
            Content = Array.Empty<byte>();
            CreationDate = DateTime.Now;
            Car = new CarDBO();
            IsMain = false;
        }
    }
}
