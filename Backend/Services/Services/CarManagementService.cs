using DB.DBO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Shared;
using Services.Utils;

namespace Services.Services {
    public class CarManagementService : ICarManagementService {
        private Config Config { get; }

        public CarManagementService(Config config) {
            Config = config;
        }

        public IActionResult AddCar(CarDTO car) {
            using (var context = new DataContext(Config)) {
                if (!context.Cars.Any(x => x.RegistrationNumber == car.RegistrationNumber)) {
                    context.Cars.Add(Mapper.Get().Map<CarDBO>(car));
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status201Created);
                } else {
                    return new ObjectResult("Car with the same registration number already exists!") { StatusCode = StatusCodes.Status409Conflict };
                }
            }
        }

        public IActionResult DeleteCar(int ID) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == ID)) {
                    var dbo = context.Cars.Single(x => x.ID == ID);
                    context.Cars.Remove(dbo);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult EditCar(CarDTO car) {
            using(var context = new DataContext(Config)) {
                if(context.Cars.Any(x => x.ID == car.ID)) {
                    var dbo = (context.Cars.Single(x => x.ID == car.ID));
                    dbo.SeatsNumber = car.SeatsNumber;
                    dbo.RegistrationNumber = car.RegistrationNumber;
                    dbo.Brand = car.Brand;
                    dbo.Model = car.Model;
                    dbo.IsOperational = car.IsOperational;
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetCarByID(int ID) {
            using(var context = new DataContext(Config)) {
                if(context.Cars.Any(x => x.ID == ID)) {
                    var dbo = context.Cars.Single(x => x.ID == ID);
                    return new ObjectResult(Mapper.Get().Map<CarDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetCarRegistrationNumber(string registrationNumber) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.RegistrationNumber == registrationNumber)) {
                    var dbo = context.Cars.Single(x => x.RegistrationNumber == registrationNumber);
                    return new ObjectResult(Mapper.Get().Map<CarDTO>(dbo)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
