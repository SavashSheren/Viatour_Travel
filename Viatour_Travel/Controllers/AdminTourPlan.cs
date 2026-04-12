using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Services.TourPlanServices;
using Viatour_Travel.Services.TourServices;
using Viatour_Travel.Services.UploadService;

namespace Viatour_Travel.Controllers
{
    public class AdminTourPlanController : Controller
    {
        private readonly ITourPlanService _tourPlanService;
        private readonly ITourService _tourService;
        private readonly IFileUploadService _fileUploadService;

        public AdminTourPlanController(ITourPlanService tourPlanService, ITourService tourService)
        {
            _tourPlanService = tourPlanService;
            _tourService = tourService;
        }
 

       
        [HttpGet]
        public async Task<IActionResult> Index(string tourId)
        {
            if (string.IsNullOrWhiteSpace(tourId))
                return RedirectToAction("Index", "AdminTour");

            var tour = await _tourService.GetTourByIdAsync(tourId);
            if (tour == null)
                return RedirectToAction("Index", "AdminTour");

            var plans = await _tourPlanService.GetTourPlansByTourIdAsync(tourId);

            ViewBag.TourId = tourId;
            ViewBag.TourTitle = tour.TourTitle;
            ViewBag.DayCount = tour.DayCount;

            return View(plans);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTourPlan(string tourId)
        {
            if (string.IsNullOrWhiteSpace(tourId))
                return RedirectToAction("Index", "AdminTour");

            var tour = await _tourService.GetTourByIdAsync(tourId);
            if (tour == null)
                return RedirectToAction("Index", "AdminTour");

            var existingPlans = await _tourPlanService.GetTourPlansByTourIdAsync(tourId);
            var nextDayNumber = existingPlans.Any() ? existingPlans.Max(x => x.DayNumber) + 1 : 1;

            ViewBag.TourTitle = tour.TourTitle;
            ViewBag.DayCount = tour.DayCount;

            return View(new TourPlanDto
            {
                TourId = tourId,
                DayNumber = nextDayNumber
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTourPlan(TourPlanDto dto)
        {
            if (dto == null)
            {
                return Content("DTO is null");
            }

            if (string.IsNullOrWhiteSpace(dto.TourId))
            {
                ModelState.AddModelError("TourId", "TourId is required.");
            }

            if (dto.DayNumber < 1)
            {
                ModelState.AddModelError("DayNumber", "Day number must be greater than 0.");
            }

            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                ModelState.AddModelError("Title", "Title is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                ModelState.AddModelError("Description", "Description is required.");
            }

            if (!ModelState.IsValid)
            {
                var tour = await _tourService.GetTourByIdAsync(dto.TourId);
                ViewBag.TourTitle = tour?.TourTitle ?? "Tour";
                ViewBag.DayCount = tour?.DayCount ?? 0;
                return View(dto);
            }

            await _tourPlanService.CreateAsync(dto);

            return RedirectToAction(nameof(Index), new { tourId = dto.TourId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateTourPlan(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Index", "AdminTour");

            var value = await _tourPlanService.GetByIdAsync(id);
            if (value == null)
                return RedirectToAction("Index", "AdminTour");

            var tour = await _tourService.GetTourByIdAsync(value.TourId);
            ViewBag.TourTitle = tour?.TourTitle ?? "Tour";
            ViewBag.DayCount = tour?.DayCount ?? 0;

            return View(value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTourPlan(TourPlanDto dto)
        {
            if (!ModelState.IsValid)
            {
                var tour = await _tourService.GetTourByIdAsync(dto.TourId);
                ViewBag.TourTitle = tour?.TourTitle ?? "Tour";
                ViewBag.DayCount = tour?.DayCount ?? 0;
                return View(dto);
            }

            await _tourPlanService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index), new { tourId = dto.TourId });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTourPlan(string id, string tourId)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                await _tourPlanService.DeleteAsync(id);
            }

            return RedirectToAction(nameof(Index), new { tourId });
        }
    }
}