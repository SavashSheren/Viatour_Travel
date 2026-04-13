using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Services.ReviewService;

namespace Viatour_Travel.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult CreateReview(string tourId)
        {
            var model = new CreateReviewDto
            {
                TourId = tourId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage) ? "Invalid field." : x.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join(" | ", errors);
                return View(createReviewDto);
            }

            createReviewDto.Status = false;

            await _reviewService.CreateReviewAsync(createReviewDto);

            TempData["SuccessMessage"] = "Your review has been submitted successfully and is waiting for approval.";

            return Redirect($"/Tour/TourDetail/{createReviewDto.TourId}?tab=reviews");
        }

        [HttpGet]
        public async Task<IActionResult> GetReviewByTourId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Tour id is required.");
            }

            var values = await _reviewService.GetAllReviewsByTourIdAsync(id);
            return View(values);
        }
    }
}