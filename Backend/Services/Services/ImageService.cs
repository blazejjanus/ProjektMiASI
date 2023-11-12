using DB.DBO;
using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Configuration;

namespace Services.Services {
    public class ImageService : IImageService {
        private Config Config { get; }

        public ImageService(Config config) {
            Config = config;
        }

        public IActionResult GetCarMainImage(int CarID) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == CarID)) {
                    var car = context.Cars.Single(x => x.ID == CarID);
                    if (context.Images.Any(x => x.Car.ID == car.ID && x.IsMain == true)) {
                        var image = context.Images.Single(x => x.Car.ID == car.ID && x.IsMain == true);
                        return new ObjectResult(Convert.ToBase64String(image.Content)) { StatusCode =  StatusCodes.Status200OK};
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                } else {
                    return new ObjectResult("Cannot find car with provided ID!") { StatusCode = StatusCodes.Status404NotFound };
                }
            }
        }

        public IActionResult GetCarImages(int CarID) {
            using (var context = new DataContext(Config)) {
                if (context.Cars.Any(x => x.ID == CarID)) {
                    var car = context.Cars.Single(x => x.ID == CarID);
                    if (context.Images.Any(x => x.Car.ID == car.ID)) {
                        List<ImageDBO> images = new List<ImageDBO>();
                        if (context.Images.Any(x => x.Car.ID == car.ID && x.IsMain == true)) {
                            images.Add(context.Images.Single(x => x.Car.ID == car.ID && x.IsMain == true));
                            images.AddRange(context.Images.Where(x => x.Car.ID == car.ID && x.IsMain == false).ToList());
                        } else {
                            images.AddRange(context.Images.Where(x => x.Car.ID == car.ID).ToList());
                        }
                        List<string> results = new List<string>();
                        foreach (var image in images) {
                            results.Add(Convert.ToBase64String(image.Content));
                        }
                        return new ObjectResult(results) { StatusCode = 200 };
                    } else {
                        return new StatusCodeResult(StatusCodes.Status404NotFound);
                    }
                } else {
                    return new ObjectResult("Cannot find car with provided ID!") { StatusCode = StatusCodes.Status404NotFound };
                }
            }
        }

        public IActionResult AddCarImage(int CarID, string imageContent, bool? isMain = null) {
            byte[] content = Array.Empty<byte>();
            try {
                content = Convert.FromBase64String(imageContent);
            } catch (Exception exc) {
                if (exc is ArgumentException) {
                    return new ObjectResult(exc.Message) { StatusCode = StatusCodes.Status400BadRequest };
                } else {
                    throw new Exception("Cannot convert provided image to database format!", exc);
                }
            }
            using (var context = new DataContext(Config)) {
                if (!context.Cars.Any(x => x.ID == CarID)) {
                    return new ObjectResult("Cannot find car with provided ID!") { StatusCode = StatusCodes.Status404NotFound };
                }
                var dbo = new ImageDBO() {
                    Car = context.Cars.Single(x => x.ID == CarID),
                    Content = content,
                };
                switch (isMain) {
                    case null:
                        if (!context.Images.Any(x => x.Car.ID == CarID && x.IsMain == true)) {
                            dbo.IsMain = true;
                        }
                        break;
                    case true:
                        dbo.IsMain = true;
                        if (context.Images.Any(x => x.Car.ID == CarID && x.IsMain == true)) {
                            var imageToChange = context.Images.Single(x => x.Car.ID == CarID && x.IsMain == true);
                            imageToChange.IsMain = false;
                        }
                        break;
                    case false:
                        if (!context.Images.Any(x => x.Car.ID == CarID && x.IsMain == true)) {
                            return new ObjectResult("You specified the image not to be main, but this car has no mainimage! Set main image first, set isMain header to true or leave it empty.") { StatusCode = StatusCodes.Status409Conflict };
                        }
                        break;
                }
                context.Images.Add(dbo);
                context.SaveChanges();
                return new StatusCodeResult(StatusCodes.Status201Created);
            }
        }

        public IActionResult DeleteImage(int ImageID) {
            using (var context = new DataContext(Config)) {
                if (context.Images.Any(x => x.ID == ImageID)) {
                    var image = context.Images.Single(x => x.ID == ImageID);
                    context.Images.Remove(image);
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }

        public IActionResult EditImage(int ImageID, string imageContent) {
            byte[] content = Array.Empty<byte>();
            try {
                content = Convert.FromBase64String(imageContent);
            } catch (Exception exc) {
                if (exc is ArgumentException) {
                    return new ObjectResult(exc.Message) { StatusCode = StatusCodes.Status400BadRequest };
                } else {
                    throw new Exception("Cannot convert provided image to database format!", exc);
                }
            }
            using (var context = new DataContext(Config)) {
                if (context.Images.Any(x => x.ID == ImageID)) {
                    var dbo = context.Images.Single(x => x.ID == ImageID);
                    dbo.Content = content;
                    dbo.LastUpdate = DateTime.Now;
                    context.SaveChanges();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                } else {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
            }
        }
    }
}
