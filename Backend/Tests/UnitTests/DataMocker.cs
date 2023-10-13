using DB;
using DB.DBO;
using Services.DTO;
using Services.Utils;
using Shared.Enums;

namespace UnitTests {
    internal static class DataMocker {
        private static DataContext Context { get; } = new DataContext(TestEnvironment.GetInstance().Config);
        private static Random RNG = new Random();

        public static UserDTO GetExistingUser(UserTypes? type = null) {
            List<UserDBO> users;
            if (type == null) {
                users = Context.Users.Where(x => !x.IsDeleted).ToList();
            } else {
                users = Context.Users.Where(x => x.UserType == type && !x.IsDeleted).ToList();
            }
            if(users.Count == 0) {
                users.Add(Mapper.Get().Map<UserDBO>(GetNonExistingUser(type)));
            }
            var user = users[RNG.Next(users.Count)];
            return Mapper.Get().Map<UserDTO>(user);
        }

        public static UserDTO GetNonExistingUser(UserTypes? type = null) {
            var users = Context.Users.ToList();
            string uname = "unittest";
            var last = Context.Users.LastOrDefault(x => x.Email.StartsWith(uname));
            if (last == null) {
                uname += 1;
            } else {
                var str = last.Email.Split('@')[0];
                var num = int.Parse(str.Substring(uname.Length));
                uname += (num + 1);
            }
            var userToAdd = new UserDBO() {
                Name = "Test",
                Surname = "User",
                Email = uname + "@test.local",
                PasswordHash = "Qwerty123!@#",
                PasswordSalt = "",
                UserType = type ?? UserTypes.CUSTOMER,
                Address = new AddressDBO(),
            };
            Context.Users.Add(userToAdd);
            Context.SaveChanges();
            return Mapper.Get().Map<UserDTO>(userToAdd);
        }

        public static CarDTO GetExistingCar() {
            var cars = Context.Cars.ToList();
            if (cars.Count == 0) {
                cars.Add(Mapper.Get().Map<CarDBO>(GetNonExistingCar()));
            }
            var car = cars[RNG.Next(cars.Count) - 1];
            return Mapper.Get().Map<CarDTO>(car);
        }

        public static CarDTO GetNonExistingCar() {
            var nonExistingCar = new CarDBO {
                Brand = "Non-Existing",
                Model = "Car",
                RegistrationNumber = "N/A",
                SeatsNumber = 0,
                IsOperational = true,
                ProductionYear = 0,
                Horsepower = 120,
                EngineCapacity = 3,
                FuelType = FuelTypes.Gasoline,
                ShortDescription = "This car does not exist.",
                LongDescription = "This car does not exist in the database.",
                PricePerDay = 0,
                IsDeleted = false
            };
            Context.Cars.Add(nonExistingCar);
            Context.SaveChanges();
            return Mapper.Get().Map<CarDTO>(nonExistingCar);
        }

    }
}
