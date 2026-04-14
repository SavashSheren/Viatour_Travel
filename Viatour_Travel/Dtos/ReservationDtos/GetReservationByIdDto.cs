namespace Viatour_Travel.Dtos.ReservationDtos
{
    public class GetReservationByIdDto
    {
        public string ReservationId { get; set; } = null!;
        public string ReservationNumber { get; set; } = null!;
        public string TourId { get; set; } = null!;
        public string TourTitle { get; set; } = null!;
        public string TourImageUrl { get; set; } = null!;

        public string NameSurname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;

        public int PersonCount { get; set; }
        public DateTime TravelDate { get; set; }
        public string? Note { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }
}