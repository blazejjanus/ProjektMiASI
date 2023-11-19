using DB.DBO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.DTO;
using Services.Interfaces;
using Services.Utils;
using Shared.Configuration;
using Shared.Enums;

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

        public IActionResult DeleteCar(int ID, bool hard = false) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == ID)) {
                    var dbo = context.Cars.First(x => x.ID == ID);
                    var orders = context.Orders.Where(x => x.Car.ID == dbo.ID);
                    foreach (var order in orders) {
                        if (OrderStateHelper.GetOrderState(order) == OrderStates.ACTIVE) {
                            return new ObjectResult("User has active orders!") { StatusCode = StatusCodes.Status409Conflict };
                        }
                        if (OrderStateHelper.GetOrderState(order) == OrderStates.PENDING) {
                            return new ObjectResult("User has pending orders!") { StatusCode = StatusCodes.Status409Conflict };
                        }
                    }
                    if (hard) {
                        context.Cars.Remove(dbo);
                        context.Orders.RemoveRange(orders);
                    } else {
                        dbo.IsDeleted = true;
                    }
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult EditCar(CarDTO car) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == car.ID)) {
                    var dbo = (context.Cars.Single(x => x.ID == car.ID));
                    dbo.SeatsNumber = car.SeatsNumber;
                    dbo.RegistrationNumber = car.RegistrationNumber;
                    dbo.Brand = car.Brand;
                    dbo.Model = car.Model;
                    dbo.IsOperational = car.IsOperational;
                    dbo.ProductionYear = car.ProductionYear;
                    dbo.Horsepower = car.Horsepower;
                    dbo.EngineCapacity = car.EngineCapacity;
                    dbo.FuelType = car.FuelType;
                    dbo.ShortDescription = car.ShortDescription;
                    dbo.LongDescription = car.LongDescription;
                    dbo.PricePerDay = car.PricePerDay;
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetCarByID(int ID) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == ID)) {
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

        public IActionResult GetAllCars(bool includeUnoperational = false, int? count = null, int? startIndex = null) {
            using(var context = new DataContext(Config)) {
                if (!includeUnoperational) {
                    return new ObjectResult(Mapper.Get().Map<List<CarDTO>>(
                        context.Cars.Where(x => x.IsOperational).ToList().Take(count ?? 100).Skip(startIndex ?? 0)
                    )) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new ObjectResult(Mapper.Get().Map<List<CarDTO>>(
                        context.Cars.ToList().Take(count ?? 100).Skip(startIndex ?? 0)
                    )) { StatusCode = StatusCodes.Status200OK };
                }
            }
        }

        public IActionResult IsCarOrdered(int ID) {
            using (var context = new DataContext(Config)) { 
                if(!context.Cars.Any(x => x.ID == ID)) { return new StatusCodeResult(StatusCodes.Status404NotFound); }
                var orders = context.Orders.Where(x => x.ID == ID).ToList();
                foreach(var order in orders) {
                    var state = OrderStateHelper.GetOrderState(order);
                    if(state == OrderStates.ACTIVE || state == OrderStates.PENDING) {
                        return new ObjectResult(true) { StatusCode = StatusCodes.Status200OK };
                    }
                }
                return new ObjectResult(false) { StatusCode = StatusCodes.Status200OK };
            }
        }
    }
}
