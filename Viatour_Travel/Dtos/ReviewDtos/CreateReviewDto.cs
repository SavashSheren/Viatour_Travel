namespace Viatour_Travel.Dtos.ReviewDtos
{
    public class CreateReviewDto
    {
        public String ReviewId { get; set; }
        public String NameSurname { get; set; }
        public String Detail { get; set; }
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
        public String TourId { get; set; }
    }
}
