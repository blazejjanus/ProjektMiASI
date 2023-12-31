﻿using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class EventDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual EventTypes Type { get; set; }
        [MaxLength(5000)]
        public virtual string? Message { get; set; }
        [MaxLength(10000)]
        public virtual string? Inner { get; set; }
        [MaxLength(5000)]
        public virtual string? Trace { get; set; }

        public EventDBO(string message, EventTypes type) {
            Message = message;
            Type = type;
            DateTime = DateTime.Now;
        }

        public EventDBO(Exception exc, string? message = null) { 
            if(message != null) {
                message += ": Exception: " + exc.Message;
            } else {
                message = exc.Message;
            }
            Message = message;
            Inner = exc.InnerException?.ToString();
            Trace = exc.StackTrace?.ToString();
            Type = EventTypes.ERROR;
            DateTime = DateTime.Now;
        }
    }
}
