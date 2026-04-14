using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.ReservationService;

namespace Viatour_Travel.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public AdminReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _reservationService.GetAllReservationsAsync();
            return View(values);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Reservation id is required.";
                return RedirectToAction(nameof(Index));
            }

            await _reservationService.ApproveReservationAsync(id);
            TempData["SuccessMessage"] = "Reservation approved successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Reservation id is required.";
                return RedirectToAction(nameof(Index));
            }

            await _reservationService.DeleteReservationAsync(id);
            TempData["SuccessMessage"] = "Reservation deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}