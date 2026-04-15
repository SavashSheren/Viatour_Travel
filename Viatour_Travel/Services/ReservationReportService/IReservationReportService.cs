using Viatour_Travel.Dtos.ReservationDtos;

namespace Viatour_Travel.Services.ReservationReportService
{
    public interface IReservationReportService
    {
        byte[] GenerateExcelReport(string tourTitle, List<ResultReservationDto> reservations);
        byte[] GeneratePdfReport(string tourTitle, List<ResultReservationDto> reservations);
    }
}