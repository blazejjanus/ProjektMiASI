using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface ICarManagementService {
        public IActionResult GetCarByID(int ID);
        public IActionResult GetCarRegistrationNumber(string registrationNumber);
        public IActionResult GetCarMainImage(int ID);
        public IActionResult GetCarImages(int ID);
        public IActionResult AddCarImage();
        public IActionResult AddCar(CarDTO car);
        public IActionResult EditCar(CarDTO car);
        public IActionResult DeleteCar(int ID);
    }
}
