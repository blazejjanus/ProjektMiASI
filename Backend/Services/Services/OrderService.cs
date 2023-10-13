using DB;
using DB.DBO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.DTO;
using Services.Interfaces;
using Services.Utils;
using Shared.Configuration;
using Shared.Enums;
using System;

namespace Services.Services {
    public class OrderService : IOrderService {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private static List<int>? _blockedCarIds;
        private Config Config { get; }

        public OrderService(Config config) {
            Config = config;
            if(_blockedCarIds == null) {
                _blockedCarIds = new List<int>();
            }
        }

        public IActionResult AddOrder(OrderDTO order) {
            var validation = ValidateOrder(order);
            if (validation != null) {
                return validation;
            }
            //Check if a car is currently locked for editing
            int tryCounter = 0;
            _lock.Wait();
            while (_blockedCarIds.Contains(order.Car.ID)) {
                if (tryCounter > 10) {
                    return new ObjectResult("Car is currently locked for editing!") { StatusCode = StatusCodes.Status429TooManyRequests };
                }
                Thread.Sleep(1000);
                tryCounter++;
            }
            _blockedCarIds?.Add(order.Car.ID);
            using (var context = new DataContext(Config)) {
                try {
                    if (!context.Users.Any(x => x.ID == order.Customer.ID && !x.IsDeleted)) {
                        return new ObjectResult($"User with ID: {order.Customer.ID} cannot be found or is marked as deleted!") { StatusCode = StatusCodes.Status404NotFound };
                    }
                    if (!context.Cars.Any(x => x.ID == order.Car.ID && x.IsOperational && !x.IsDeleted)) {
                        return new ObjectResult($"Car with ID: {order.Car.ID} cannot be found or is not operational!") { StatusCode = StatusCodes.Status404NotFound };
                    }
                    // Check for overlapping reservations excluding canceled orders
                    if (context.Orders.Any(x => x.Car.ID == order.Car.ID &&
                            x.CancelationTime == null &&
                            ((order.RentStart >= x.RentStart && order.RentStart < x.RentEnd) ||
                                (order.RentEnd > x.RentStart && order.RentEnd <= x.RentEnd) ||
                                (order.RentStart <= x.RentStart && order.RentEnd >= x.RentEnd)))) {
                        return new ObjectResult($"Car with ID: {order.Car.ID} is already reserved for the specified time period!") { StatusCode = StatusCodes.Status409Conflict };
                    }
                    if (order.CancelationTime != null) order.CancelationTime = null;
                    var dbo = Mapper.Get().Map<OrderDBO>(order);
                    dbo.Customer = context.Users.First(x => x.ID == order.Customer.ID && !x.IsDeleted);
                    dbo.Car = context.Cars.First(x => x.ID == order.Car.ID && x.IsOperational);
                    context.Orders.Add(dbo);
                    context.SaveChanges();
                    if (_blockedCarIds.Contains(order.Car.ID)) { _blockedCarIds?.Remove(order.Car.ID); }
                    return new ObjectResult($"Reserved car {order.Car.ID} for period {order.RentStart.ToString("dd-MM-yyyy HH:mm")} - {order.RentEnd.ToString("dd-MM-yyyy HH:mm")}") { StatusCode = StatusCodes.Status201Created };
                } catch (Exception ex) {
                    if (_blockedCarIds.Contains(order.Car.ID)) { _blockedCarIds?.Remove(order.Car.ID); }
                    return new ObjectResult($"An error occurred: {ex.ToString()}") { StatusCode = StatusCodes.Status500InternalServerError };
                } finally { _lock.Release(); }
            }
        }

        public IActionResult CancelOrder(int ID) {
            using(var context = new DataContext(Config)) {
                if(!context.Orders.Any(x => x.ID == ID)) {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                var dbo = context.Orders.First(x => x.ID == ID);
                if(dbo.CancelationTime != null) {
                    return new ObjectResult("Order is already canceled!") { StatusCode = StatusCodes.Status409Conflict };
                }
                dbo.CancelationTime = DateTime.Now;
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
        }

        public IActionResult EditOrder(OrderDTO order) {
            var validation = ValidateOrder(order);
            if (validation != null) {
                return validation;
            }
            using (var context = new DataContext(Config)) {
                if (!context.Orders.Any(x => x.ID == order.ID)) {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                if (!context.Users.Any(x => x.ID == order.Customer.ID && !x.IsDeleted)) {
                    return new ObjectResult($"User with ID: {order.Customer.ID} cannot be found or is marked as deleted!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if (!context.Cars.Any(x => x.ID == order.Car.ID && x.IsOperational)) {
                    return new ObjectResult($"Car with ID: {order.Car.ID} cannot be found or is not operational!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if(OrderStateHelper.IsOrderState(order, OrderStates.COMPLETED)) {
                      return new ObjectResult("Cannot edit completed order!") { StatusCode = StatusCodes.Status409Conflict };
                }
                if (OrderStateHelper.IsOrderState(order, OrderStates.CANCELED)) {
                    return new ObjectResult("Cannot edit canceled order!") { StatusCode = StatusCodes.Status409Conflict };
                }
                var dbo = context.Orders.First(x => x.ID == order.ID);
                dbo.RentStart = order.RentStart;
                dbo.RentEnd = order.RentEnd;
                dbo.Car = Mapper.Get().Map<CarDBO>(context.Cars.First(x => x.ID == order.Car.ID && x.IsOperational));
                dbo.Customer = Mapper.Get().Map<UserDBO>(context.Users.First(x => x.ID == order.Customer.ID && !x.IsDeleted));
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
        }

        public IActionResult GetAllOrders(OrderStates? state = null) {
            using (var context = new DataContext(Config)) {
                var orders = context.Orders.ToList();
                if(state == null) {
                    return new ObjectResult(Mapper.Get().Map<List<OrderDTO>>(orders)) { StatusCode = StatusCodes.Status200OK };
                } else {
                    var matchingOrders = new List<OrderDTO>();
                    foreach(var order in orders) {
                        if(OrderStateHelper.IsOrderState(Mapper.Get().Map<OrderDTO>(order), state.Value)) {
                            matchingOrders.Add(Mapper.Get().Map<OrderDTO>(order));
                        }
                    }
                    return new ObjectResult(matchingOrders) { StatusCode = StatusCodes.Status200OK };
                }
            }
        }

        public IActionResult GetOrderByID(int ID) {
            using(var context = new DataContext(Config)) {
                if(context.Orders.Any(x => x.ID == ID)) {
                    return new ObjectResult(Mapper.Get().Map<OrderDTO>(context.Orders.Single(x => x.ID == ID))) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetUserOrders(int userID) {
            using (var context = new DataContext(Config)) {
                if(!context.Users.Any(x => x.ID == userID && !x.IsDeleted)) {
                    return new ObjectResult($"User with ID: {userID} cannot be found or is marked as deleted!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if(context.Orders.Any(x => x.Customer.ID == userID)) {
                    return new ObjectResult(Mapper.Get().Map<List<OrderDTO>>(context.Orders.Where(x => x.Customer.ID == userID).ToList())) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetUserOrders(string email) {
            using (var context = new DataContext(Config)) {
                if (!context.Users.Any(x => x.Email == email && !x.IsDeleted)) {
                    return new ObjectResult($"User with Email: {email} cannot be found or is marked as deleted!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if (context.Orders.Any(x => x.Customer.Email == email)) {
                    return new ObjectResult(Mapper.Get().Map<List<OrderDTO>>(context.Orders.Where(x => x.Customer.Email == email).ToList())) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetCarOrders(int carID) {
            using (var context = new DataContext(Config)) {
                if(!context.Cars.Any(x=> x.ID == carID)){
                    return new ObjectResult($"Car with ID: {carID} cannot be found!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if(context.Orders.Any(x => x.Car.ID == carID)) {
                    return new ObjectResult(Mapper.Get().Map<List<OrderDTO>>(context.Orders.Where(x => x.Car.ID == carID).ToList())) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult GetCarOrders(string registrationNumber) {
            using (var context = new DataContext(Config)) {
                if (!context.Cars.Any(x => x.RegistrationNumber == registrationNumber)) {
                    return new ObjectResult($"Car with Registration number: {registrationNumber} cannot be found!") { StatusCode = StatusCodes.Status404NotFound };
                }
                if (context.Orders.Any(x => x.Car.RegistrationNumber == registrationNumber)) {
                    return new ObjectResult(Mapper.Get().Map<List<OrderDTO>>(context.Orders.Where(x => x.Car.RegistrationNumber == registrationNumber).ToList())) { StatusCode = StatusCodes.Status200OK };
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        private IActionResult? ValidateOrder(OrderDTO order) {
            if (order.RentStart > order.RentEnd) {
                return new ObjectResult("Rent start date cannot be after rent end date!") { StatusCode = StatusCodes.Status409Conflict };
            }
            if (order.RentStart < DateTime.Now) {
                return new ObjectResult("Rent start date cannot be in the past!") { StatusCode = StatusCodes.Status409Conflict };
            }
            if (order.RentEnd < order.RentStart) {
                return new ObjectResult("Rent end date cannot be before rent start date!") { StatusCode = StatusCodes.Status409Conflict };
            }
            return null;
        }
    }
}
