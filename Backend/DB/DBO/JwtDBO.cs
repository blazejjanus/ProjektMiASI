using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class JwtDBO {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        public virtual UserDBO User { get; set; }
        [MaxLength(2048)]
        public virtual string JWT { get; set; }
        public virtual bool Active { get; set; }

        public JwtDBO() { 
            JWT = string.Empty;
            User = new UserDBO();
            Active = true;
        }
    }
}
