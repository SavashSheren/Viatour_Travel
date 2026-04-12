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

        public IActionResult CreateReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewDto createReviewDto)
        {
            createReviewDto.Status = false;
            await _reviewService.CreateReviewAsync(createReviewDto);
            return RedirectToAction("ReviewList");
        }
        public async Task<IActionResult> GetReviewByTourId(string id)
        {
            var value = await _reviewService.GetAllReviewsByTourIdAsync(id);
            return View(value);

        }
    }
}
