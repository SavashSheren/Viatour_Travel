using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Services.TourServices;

namespace Viatour_Travel.Controllers
{
    public class TourController : Controller
    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        public IActionResult TourList()
        {
            return View();
        }
        public IActionResult ToursByCategory(string categoryId, int page = 1)
        {
            ViewBag.CategoryId = categoryId;
            ViewBag.Page = page;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> TourDetail(string id)
        {
            
                if (string.IsNullOrWhiteSpace(id))
                {
                    return RedirectToAction("TourList");
                }

                var value = await _tourService.GetTourDetailAsync(id);

                if (value == null)
                {
                    return NotFound();
                }

                return View(value);
            }
        }

    }

