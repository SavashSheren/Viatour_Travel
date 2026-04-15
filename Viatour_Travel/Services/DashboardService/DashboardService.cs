using MongoDB.Driver;
using Viatour_Travel.Dtos.DashboardDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;

        public DashboardService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _reviewCollection = database.GetCollection<Review>(databaseSettings.ReviewCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
        }

        public async Task<ResultDashboardDto> GetDashboardDataAsync()
        {
            var totalTourTask = _tourCollection.CountDocumentsAsync(Builders<Tour>.Filter.Empty);
            var totalCategoryTask = _categoryCollection.CountDocumentsAsync(Builders<Category>.Filter.Empty);

            var totalReviewTask = _reviewCollection.CountDocumentsAsync(Builders<Review>.Filter.Empty);
            var pendingReviewTask = _reviewCollection.CountDocumentsAsync(x => !x.Status);
            var approvedReviewTask = _reviewCollection.CountDocumentsAsync(x => x.Status);

            var totalReservationTask = _reservationCollection.CountDocumentsAsync(Builders<Reservation>.Filter.Empty);
            var pendingReservationTask = _reservationCollection.CountDocumentsAsync(x => !x.Status);
            var approvedReservationTask = _reservationCollection.CountDocumentsAsync(x => x.Status);

            var recentReservationsTask = _reservationCollection
                .Find(Builders<Reservation>.Filter.Empty)
                .SortByDescending(x => x.CreatedDate)
                .Limit(5)
                .ToListAsync();

            var recentReviewsTask = _reviewCollection
                .Find(Builders<Review>.Filter.Empty)
                .SortByDescending(x => x.ReviewDate)
                .Limit(5)
                .ToListAsync();

            await Task.WhenAll(
                totalTourTask,
                totalCategoryTask,
                totalReviewTask,
                pendingReviewTask,
                approvedReviewTask,
                totalReservationTask,
                pendingReservationTask,
                approvedReservationTask,
                recentReservationsTask,
                recentReviewsTask);

            var recentReviews = await recentReviewsTask;
            var recentReservations = await recentReservationsTask;

            var reviewTourIds = recentReviews
                .Select(x => x.TourId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var reviewTours = reviewTourIds.Count == 0
                ? new List<Tour>()
                : await _tourCollection.Find(x => reviewTourIds.Contains(x.TourId)).ToListAsync();

            var tourDictionary = reviewTours.ToDictionary(x => x.TourId, x => x.TourTitle);

            var totalReviewCount = (int)await totalReviewTask;
            var pendingReviewCount = (int)await pendingReviewTask;
            var approvedReviewCount = (int)await approvedReviewTask;

            var totalReservationCount = (int)await totalReservationTask;
            var pendingReservationCount = (int)await pendingReservationTask;
            var approvedReservationCount = (int)await approvedReservationTask;

            var model = new ResultDashboardDto
            {
                TotalTourCount = (int)await totalTourTask,
                TotalCategoryCount = (int)await totalCategoryTask,

                TotalReviewCount = totalReviewCount,
                PendingReviewCount = pendingReviewCount,
                ApprovedReviewCount = approvedReviewCount,

                TotalReservationCount = totalReservationCount,
                PendingReservationCount = pendingReservationCount,
                ApprovedReservationCount = approvedReservationCount,

                TotalActionRequiredCount = pendingReviewCount + pendingReservationCount,

                ApprovedReviewRate = totalReviewCount == 0
                    ? 0
                    : (int)Math.Round((double)approvedReviewCount / totalReviewCount * 100),

                ApprovedReservationRate = totalReservationCount == 0
                    ? 0
                    : (int)Math.Round((double)approvedReservationCount / totalReservationCount * 100),

                RecentReservations = recentReservations.Select(x => new DashboardRecentReservationDto
                {
                    ReservationId = x.ReservationId,
                    ReservationNumber = x.ReservationNumber,
                    TourTitle = x.TourTitle,
                    NameSurname = x.NameSurname,
                    PersonCount = x.PersonCount,
                    TravelDate = x.TravelDate,
                    CreatedDate = x.CreatedDate,
                    Status = x.Status
                }).ToList(),

                RecentReviews = recentReviews.Select(x => new DashboardRecentReviewDto
                {
                    ReviewId = x.ReviewId,
                    TourTitle = tourDictionary.TryGetValue(x.TourId, out var tourTitle)
                        ? tourTitle
                        : "Tour not found",
                    NameSurname = x.NameSurname,
                    Detail = x.Detail,
                    Score = x.Score,
                    ReviewDate = x.ReviewDate,
                    Status = x.Status
                }).ToList()
            };

            return model;
        }
    }
}