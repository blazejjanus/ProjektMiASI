using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.Interfaces {
    public interface IImageService {
        public IActionResult GetCarMainImage(int CarID);
        public IActionResult GetCarImages(int CarID);
        public IActionResult AddCarImage(int CarID, string imageContent, bool? isMain = null);
        public IActionResult DeleteImage(int ImageID);
        public IActionResult EditImage(int ImageID, string imageContent);
    }
}
