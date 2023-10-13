using Microsoft.AspNetCore.Mvc;
using Services.DTO;

namespace Services.Interfaces {
    public interface ICarManagementService {
        public IActionResult GetCarByID(int ID);
        public IActionResult GetCarRegistrationNumber(string registrationNumber);
        public IActionResult GetAllCars(bool includeUnoperational = false, int? count = null, int? startIndex = null);
        public IActionResult AddCar(CarDTO car);
        public IActionResult EditCar(CarDTO car);
        public IActionResult DeleteCar(int ID, bool hard = false);
        public IActionResult IsCarOrdered(int ID);
    }
}
