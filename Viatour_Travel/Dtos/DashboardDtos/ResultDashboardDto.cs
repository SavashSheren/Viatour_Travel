namespace Viatour_Travel.Dtos.DashboardDtos
{
    public class ResultDashboardDto
    {
        public int TotalTourCount { get; set; }
        public int TotalCategoryCount { get; set; }

        public int TotalReviewCount { get; set; }
        public int PendingReviewCount { get; set; }
        public int ApprovedReviewCount { get; set; }

        public int TotalReservationCount { get; set; }
        public int PendingReservationCount { get; set; }
        public int ApprovedReservationCount { get; set; }

        public int TotalActionRequiredCount { get; set; }

        public int ApprovedReviewRate { get; set; }
        public int ApprovedReservationRate { get; set; }

        public List<DashboardRecentReservationDto> RecentReservations { get; set; } = new();
        public List<DashboardRecentReviewDto> RecentReviews { get; set; } = new();
    }

    public class DashboardRecentReservationDto
    {
        public string ReservationId { get; set; } = null!;
        public string ReservationNumber { get; set; } = null!;
        public string TourTitle { get; set; } = null!;
        public string NameSurname { get; set; } = null!;
        public int PersonCount { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
    }

    public class DashboardRecentReviewDto
    {
        public string ReviewId { get; set; } = null!;
        public string TourTitle { get; set; } = null!;
        public string NameSurname { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public int Score { get; set; }
        public DateTime ReviewDate { get; set; }
        public bool Status { get; set; }
    }
}