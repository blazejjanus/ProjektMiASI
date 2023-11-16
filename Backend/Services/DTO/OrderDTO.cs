namespace Services.DTO {
    public class OrderDTO {
        public int ID { get; set; }
        public UserDTO Customer { get; set; }
        public CarDTO Car { get; set; }
        public DateTime RentStart { get; set; }
        public DateTime RentEnd { get; set; }
        public DateTime? CancelationTime { get; set; }

        public OrderDTO() {
            Customer = new UserDTO();
            Car = new CarDTO();
            RentStart = DateTime.Now;
            RentEnd = DateTime.Now;
            CancelationTime = null;
        }
    }
}
