using Viatour_Travel.Dtos.ReservationDtos;

namespace Viatour_Travel.Services.ReservationService
{
    public interface IReservationService
    {
        Task CreateReservationAsync(CreateReservationDto createReservationDto);
        Task<List<ResultReservationDto>> GetAllReservationsAsync();
        Task<GetReservationByIdDto?> GetReservationByIdAsync(string reservationId);
        Task ApproveReservationAsync(string reservationId);
        Task DeleteReservationAsync(string reservationId);
    }
}