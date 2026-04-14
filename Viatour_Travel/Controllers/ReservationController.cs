using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.ReservationDtos;
using Viatour_Travel.Services.ReservationService;

namespace Viatour_Travel.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult CreateReservation(
            string tourId,
            string tourTitle,
            string tourImageUrl)
        {
            var model = new CreateReservationDto
            {
                TourId = tourId,
                TourTitle = tourTitle,
                TourImageUrl = tourImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation(CreateReservationDto createReservationDto)
        {
            if (string.IsNullOrWhiteSpace(createReservationDto.TourId) ||
                string.IsNullOrWhiteSpace(createReservationDto.TourTitle) ||
                string.IsNullOrWhiteSpace(createReservationDto.NameSurname) ||
                string.IsNullOrWhiteSpace(createReservationDto.Email) ||
                string.IsNullOrWhiteSpace(createReservationDto.Phone) ||
                createReservationDto.PersonCount < 1)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields correctly.";
                return View(createReservationDto);
            }

            await _reservationService.CreateReservationAsync(createReservationDto);

            TempData["SuccessMessage"] = "Your reservation request has been received. A confirmation email will be sent after Tour approval.";

            return Redirect($"/Tour/TourDetail/{createReservationDto.TourId}?tab=reservation");
        }
    }
}