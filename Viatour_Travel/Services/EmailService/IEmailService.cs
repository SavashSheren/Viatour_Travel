namespace Viatour_Travel.Services.EmailService
{
    public interface IEmailService
    {
        Task SendReservationApprovedEmailAsync(
            string toEmail,
            string guestName,
            string reservationNumber,
            string tourTitle,
            string tourImageUrl,
            int personCount,
            DateTime travelDate);
    }
}