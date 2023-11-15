using System.ComponentModel;

namespace Services.DTO {
    public class JwtDTO {
        public virtual UserDTO User { get; set; }
        public virtual string JWT { get; set; }
        [DefaultValue(true)]
        public virtual bool Active { get; set; }

        public JwtDTO() {
            JWT = string.Empty;
            User = new UserDTO();
            Active = true;
        }
    }
}
