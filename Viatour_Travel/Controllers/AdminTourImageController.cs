using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.TourImageDtos;
using Viatour_Travel.Services.TourImageService;
using Viatour_Travel.Services.UploadService;
namespace Viatour_Travel.Controllers
{
    public class AdminTourImageController : Controller
    {
        private readonly ITourImageService _tourImageService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IWebHostEnvironment _environment;

        public AdminTourImageController(
            ITourImageService tourImageService,
            IFileUploadService fileUploadService,
            IWebHostEnvironment environment)
        {
            _tourImageService = tourImageService;
            _fileUploadService = fileUploadService;
            _environment = environment;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string tourId)
        {
            if (string.IsNullOrEmpty(tourId))
            {
                TempData["ErrorMessage"] = "Please select a tour first.";
                return RedirectToAction("TourList", "AdminTour");
            }

            ViewBag.TourId = tourId;

            var values = await _tourImageService.GetByTourIdAsync(tourId);
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadTourImageDto dto)
        {
            if (string.IsNullOrEmpty(dto.TourId))
            {
                TempData["ErrorMessage"] = "TourId is required.";
                return RedirectToAction("TourList", "AdminTour");
            }

            if (dto.Images == null || !dto.Images.Any())
            {
                TempData["ErrorMessage"] = "Please select at least one image.";
                return RedirectToAction(nameof(Index), new { tourId = dto.TourId });
            }

            var createTourImageDtos = new List<CreateTourImageDto>();

            foreach (var image in dto.Images)
            {
                if (image == null || image.Length == 0)
                {
                    continue;
                }

                var imageUrl = await _fileUploadService.SaveImageAsync(image, "tours");

                if (string.IsNullOrEmpty(imageUrl))
                {
                    continue;
                }

                createTourImageDtos.Add(new CreateTourImageDto
                {
                    TourId = dto.TourId,
                    ImageUrl = imageUrl
                });
            }

            if (createTourImageDtos.Any())
            {
                await _tourImageService.AddRangeAsync(createTourImageDtos);
                TempData["SuccessMessage"] = "Images uploaded successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "No valid images were uploaded.";
            }

            return RedirectToAction(nameof(Index), new { tourId = dto.TourId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id, string tourId)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Image id is required.";
                return RedirectToAction(nameof(Index), new { tourId });
            }

            var image = await _tourImageService.GetByIdAsync(id);

            if (image == null)
            {
                TempData["ErrorMessage"] = "Image not found.";
                return RedirectToAction(nameof(Index), new { tourId });
            }

            if (!string.IsNullOrEmpty(image.ImageUrl))
            {
                var fileFullPath = Path.Combine(
                    _environment.WebRootPath,
                    image.ImageUrl.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString())
                );

                if (System.IO.File.Exists(fileFullPath))
                {
                    System.IO.File.Delete(fileFullPath);
                }
            }

            await _tourImageService.DeleteAsync(id);

            TempData["SuccessMessage"] = "Image deleted successfully.";
            return RedirectToAction(nameof(Index), new { tourId });
        }
    }
}