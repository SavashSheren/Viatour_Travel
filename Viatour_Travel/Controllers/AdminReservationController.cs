using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.EmailService;
using Viatour_Travel.Services.ReservationService;

namespace Viatour_Travel.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IEmailService _emailService;

        public AdminReservationController(
            IReservationService reservationService,
            IEmailService emailService)
        {
            _reservationService = reservationService;
            _emailService = emailService;
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

            var reservation = await _reservationService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                TempData["ErrorMessage"] = "Reservation not found.";
                return RedirectToAction(nameof(Index));
            }

            await _reservationService.ApproveReservationAsync(id);

            await _emailService.SendReservationApprovedEmailAsync(
                reservation.Email,
                reservation.NameSurname,
                reservation.ReservationNumber,
                reservation.TourTitle,
                reservation.TourImageUrl,
                reservation.PersonCount,
                reservation.TravelDate);

            TempData["SuccessMessage"] = "Reservation approved successfully and confirmation email was sent.";
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