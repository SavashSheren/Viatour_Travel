using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.EmailService
{
    public class MailKitEmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public MailKitEmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendReservationApprovedEmailAsync(
            string toEmail,
            string guestName,
            string reservationNumber,
            string tourTitle,
            string tourImageUrl,
            int personCount,
            DateTime travelDate)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"Your reservation is confirmed - {reservationNumber}";

            var builder = new BodyBuilder
            {
                HtmlBody = BuildReservationApprovedHtml(
                    guestName,
                    reservationNumber,
                    tourTitle,
                    tourImageUrl,
                    personCount,
                    travelDate)
            };

            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _mailSettings.Host,
                _mailSettings.Port,
                _mailSettings.UseSsl);

            await client.AuthenticateAsync(
                _mailSettings.Username,
                _mailSettings.Password);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private string BuildReservationApprovedHtml(
            string guestName,
            string reservationNumber,
            string tourTitle,
            string tourImageUrl,
            int personCount,
            DateTime travelDate)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Reservation Confirmed</title>
</head>
<body style='margin:0;padding:0;background:#f4f7fb;font-family:Arial,sans-serif;color:#1f2937;'>
    <div style='max-width:680px;margin:30px auto;background:#ffffff;border-radius:18px;overflow:hidden;box-shadow:0 12px 35px rgba(0,0,0,0.08);'>
        
        <div style='background:linear-gradient(135deg,#10b981,#14b8a6);padding:24px 28px;color:#fff;'>
            <h1 style='margin:0;font-size:28px;'>Reservation Confirmed</h1>
            <p style='margin:8px 0 0;font-size:14px;opacity:.95;'>
                Your reservation has been approved successfully.
            </p>
        </div>

        <div style='padding:28px;'>
            <img src='{tourImageUrl}' alt='{tourTitle}' style='width:100%;height:280px;object-fit:cover;border-radius:14px;display:block;margin-bottom:22px;' />

            <h2 style='margin:0 0 14px;font-size:24px;color:#0f172a;'>{tourTitle}</h2>

            <p style='margin:0 0 20px;font-size:15px;line-height:1.8;color:#475569;'>
                Hello <strong>{guestName}</strong>,<br/>
                Your reservation request has been reviewed and approved by our team.
            </p>

            <div style='background:#f8fafc;border:1px solid #e2e8f0;border-radius:14px;padding:18px 20px;margin-bottom:22px;'>
                <table style='width:100%;border-collapse:collapse;font-size:14px;'>
                    <tr>
                        <td style='padding:8px 0;color:#64748b;'>Reservation Number</td>
                        <td style='padding:8px 0;text-align:right;font-weight:700;color:#0f172a;'>{reservationNumber}</td>
                    </tr>
                    <tr>
                        <td style='padding:8px 0;color:#64748b;'>Guest Name</td>
                        <td style='padding:8px 0;text-align:right;font-weight:700;color:#0f172a;'>{guestName}</td>
                    </tr>
                    <tr>
                        <td style='padding:8px 0;color:#64748b;'>Person Count</td>
                        <td style='padding:8px 0;text-align:right;font-weight:700;color:#0f172a;'>{personCount}</td>
                    </tr>
                    <tr>
                        <td style='padding:8px 0;color:#64748b;'>Travel Date</td>
                        <td style='padding:8px 0;text-align:right;font-weight:700;color:#0f172a;'>{travelDate:dd.MM.yyyy}</td>
                    </tr>
                    <tr>
                        <td style='padding:8px 0;color:#64748b;'>Status</td>
                        <td style='padding:8px 0;text-align:right;font-weight:700;color:#059669;'>Confirmed</td>
                    </tr>
                </table>
            </div>

            <div style='background:#ecfdf5;border:1px solid #a7f3d0;border-radius:14px;padding:16px 18px;margin-bottom:22px;color:#065f46;font-size:14px;line-height:1.7;'>
                If you have any questions regarding your reservation, you can contact our support team at
                <strong>{_mailSettings.SupportEmail}</strong>.
            </div>

            <p style='margin:0;font-size:13px;line-height:1.8;color:#64748b;'>
                Thank you for choosing Viatour.<br/>
                We look forward to welcoming you.
            </p>
        </div>

        <div style='padding:18px 28px;background:#f8fafc;border-top:1px solid #e2e8f0;font-size:12px;color:#94a3b8;text-align:center;'>
            © Viatour - Reservation Confirmation
        </div>
    </div>
</body>
</html>";
        }
    }
}