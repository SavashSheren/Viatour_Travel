using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Dtos.TourImageDtos;

namespace Viatour_Travel.Dtos.TourDtos
{
    public class GetTourDetailDto
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

        public string Location { get; set; }
        public string MapLocationImageUrl { get; set; }

        public List<TourPlanDto> TourPlans { get; set; } = new();
        public List<ResultTourImageDto> TourImages { get; set; } = new();
        public List<ResultReviewByTourIdDto> Reviews { get; set; } = new();
    }
}