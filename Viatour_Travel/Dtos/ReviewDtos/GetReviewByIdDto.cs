namespace Viatour_Travel.Dtos.ReviewDtos
{
    public class GetReviewByIdDto
    {
        public string ReviewId { get; set; } = null!;
        public string TourId { get; set; } = null!;
        public string NameSurname { get; set; } = null!;
        public string Comment { get; set; } = null!;
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
    }
}