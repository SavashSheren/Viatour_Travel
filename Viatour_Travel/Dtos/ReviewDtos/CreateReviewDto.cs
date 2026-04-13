namespace Viatour_Travel.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        public string NameSurname { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public int Score { get; set; }
        public bool Status { get; set; }
        public string TourId { get; set; } = null!;
    }
}