using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.EmailService;
using Viatour_Travel.Services.ReservationReportService;
using Viatour_Travel.Services.ReservationService;

namespace Viatour_Travel.Controllers
{
    public class AdminReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IEmailService _emailService;
        private readonly IReservationReportService _reservationReportService;

        public AdminReservationController(
            IReservationService reservationService,
            IEmailService emailService,
            IReservationReportService reservationReportService)
        {
            _reservationService = reservationService;
            _emailService = emailService;
            _reservationReportService = reservationReportService;
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
            if (string.IsNullOrWhiteSpace(id))
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
            if (string.IsNullOrWhiteSpace(id))
            {
                TempData["ErrorMessage"] = "Reservation id is required.";
                return RedirectToAction(nameof(Index));
            }

            await _reservationService.DeleteReservationAsync(id);
            TempData["SuccessMessage"] = "Reservation deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExportExcel(string tourId)
        {
            if (string.IsNullOrWhiteSpace(tourId))
            {
                TempData["ErrorMessage"] = "Tour id is required.";
                return RedirectToAction(nameof(Index));
            }

            var reservations = await _reservationService.GetReservationsByTourIdAsync(tourId);

            if (reservations == null || reservations.Count == 0)
            {
                TempData["ErrorMessage"] = "No reservations found for the selected tour.";
                return RedirectToAction(nameof(Index));
            }

            var tourTitle = reservations[0].TourTitle;
            var fileBytes = _reservationReportService.GenerateExcelReport(tourTitle, reservations);
            var fileName = $"{CreateSafeFileName(tourTitle)}-reservations-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }

        [HttpGet]
        public async Task<IActionResult> ExportPdf(string tourId)
        {
            if (string.IsNullOrWhiteSpace(tourId))
            {
                TempData["ErrorMessage"] = "Tour id is required.";
                return RedirectToAction(nameof(Index));
            }

            var reservations = await _reservationService.GetReservationsByTourIdAsync(tourId);

            if (reservations == null || reservations.Count == 0)
            {
                TempData["ErrorMessage"] = "No reservations found for the selected tour.";
                return RedirectToAction(nameof(Index));
            }

            var tourTitle = reservations[0].TourTitle;
            var fileBytes = _reservationReportService.GeneratePdfReport(tourTitle, reservations);
            var fileName = $"{CreateSafeFileName(tourTitle)}-reservations-{DateTime.UtcNow:yyyyMMddHHmmss}.pdf";

            return File(fileBytes, "application/pdf", fileName);
        }

        private static string CreateSafeFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "tour";
            }

            var safeValue = Regex.Replace(value.Trim().ToLowerInvariant(), @"[^a-z0-9]+", "-");
            safeValue = safeValue.Trim('-');

            return string.IsNullOrWhiteSpace(safeValue) ? "tour" : safeValue;
        }
    }
}