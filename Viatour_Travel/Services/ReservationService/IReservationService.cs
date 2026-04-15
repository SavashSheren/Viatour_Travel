using Viatour_Travel.Dtos.ReservationDtos;

namespace Viatour_Travel.Services.ReservationService
{
    public interface IReservationService
    {
        Task CreateReservationAsync(CreateReservationDto createReservationDto);
        Task<List<ResultReservationDto>> GetAllReservationsAsync();
        Task<ResultReservationDto?> GetReservationByIdAsync(string reservationId);
        Task<List<ResultReservationDto>> GetReservationsByTourIdAsync(string tourId);
        Task ApproveReservationAsync(string reservationId);
        Task DeleteReservationAsync(string reservationId);
    }
}