using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.ReviewService;

namespace Viatour_Travel.Controllers
{
    public class AdminReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public AdminReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _reviewService.GetAllReviewsAsync();
            return View(values);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "Review id is required.";
                return RedirectToAction(nameof(Index));
            }

            await _reviewService.ApproveReviewAsync(id);
            TempData["SuccessMessage"] = "Review approved successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "Review id is required.";
                return RedirectToAction(nameof(Index));
            }

            await _reviewService.DeleteReviewAsync(id);
            TempData["SuccessMessage"] = "Review deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}