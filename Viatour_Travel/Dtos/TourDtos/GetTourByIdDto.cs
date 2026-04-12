namespace Viatour_Travel.Dtos.TourDtos
{
    public class GetTourByIdDto
    {
        public string TourId { get; set; }
        public string TourTitle { get; set; }
        public string TourDescription { get; set; }
        public string CoverImageUrl { get; set; }
        public string Badge { get; set; }
        public int DayCount { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public bool IsStatus { get; set; }
        public string CategoryId { get; set; }
        public string MapLocationImageUrl { get; set; }
        public string Location { get; set; }
    }
}
