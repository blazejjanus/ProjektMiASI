using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.DBO {
    public class EventDBO {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int ID { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual EventType Type { get; set; }
        [MaxLength(250)]
        public virtual string? Message { get; set; }
        [MaxLength(250)]
        public virtual string? Inner { get; set; }
        [MaxLength(100)]
        public virtual string? Trace { get; set; }

        public EventDBO(string message, EventType type) {
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
            Type = EventType.ERROR;
            DateTime = DateTime.Now;
        }
    }
}
