﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DB.DBO {
    public class UserDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Username { get; set; }
        [MaxLength(50)]
        public virtual string Name { get; set; }
        [MaxLength(50)]
        public virtual string Surname { get; set; }
        [Required]
        public virtual string Password { get; set; }

        public UserDBO() {
            Username = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Password = string.Empty;
        }
    }
}