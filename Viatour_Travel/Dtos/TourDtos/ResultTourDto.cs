namespace Viatour_Travel.Dtos.TourDtos
{
    public class ResultTourDto
    {
        public string TourId { get; set; } = null!;
        public string TourTitle { get; set; } = null!;
        public string TourDescription { get; set; } = null!;
        public string CoverImageUrl { get; set; } = null!;
        public string Badge { get; set; } = null!;
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public string CategoryId { get; set; } = null!;

        public int ReviewCount { get; set; }
        public double AverageScore { get; set; }
    }
}