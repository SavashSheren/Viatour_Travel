using MongoDB.Bson;
using MongoDB.Driver;
using Viatour_Travel.Dtos.AdminSearchDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.AdminSearchService
{
    public class AdminSearchService : IAdminSearchService
    {
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Review> _reviewCollection;

        public AdminSearchService(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(databaseSettings.TourCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _reviewCollection = database.GetCollection<Review>(databaseSettings.ReviewCollectionName);
        }

        public async Task<ResultAdminSearchDto> SearchAllAsync(string query, int limitPerType = 5)
        {
            query = query?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(query))
            {
                return new ResultAdminSearchDto();
            }

            var toursTask = SearchToursAsync(query, limitPerType);
            var categoriesTask = SearchCategoriesAsync(query, limitPerType);
            var reservationsTask = SearchReservationsAsync(query, limitPerType);
            var reviewsTask = SearchReviewsAsync(query, limitPerType);

            await Task.WhenAll(toursTask, categoriesTask, reservationsTask, reviewsTask);

            return new ResultAdminSearchDto
            {
                Query = query,
                Tours = await toursTask,
                Categories = await categoriesTask,
                Reservations = await reservationsTask,
                Reviews = await reviewsTask
            };
        }

        public async Task<List<AdminSearchItemDto>> SearchSuggestionsAsync(string query, int totalLimit = 8)
        {
            query = query?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<AdminSearchItemDto>();
            }

            var result = await SearchAllAsync(query, 3);

            var merged = result.Tours
                .Concat(result.Reservations)
                .Concat(result.Reviews)
                .Concat(result.Categories)
                .Take(totalLimit)
                .ToList();

            return merged;
        }

        private async Task<List<AdminSearchItemDto>> SearchToursAsync(string query, int limit)
        {
            var regex = new BsonRegularExpression(query, "i");

            var filter = Builders<Tour>.Filter.Or(
                Builders<Tour>.Filter.Regex(x => x.TourTitle, regex),
                Builders<Tour>.Filter.Regex(x => x.TourDescription, regex),
                Builders<Tour>.Filter.Regex(x => x.Badge, regex),
                Builders<Tour>.Filter.Regex(x => x.Location, regex)
            );

            var tours = await _tourCollection
                .Find(filter)
                .SortByDescending(x => x.TourId)
                .Limit(limit)
                .ToListAsync();

            return tours.Select(x => new AdminSearchItemDto
            {
                EntityType = "Tour",
                EntityId = x.TourId,
                Title = x.TourTitle,
                Subtitle = x.Location,
                Description = $"Badge: {x.Badge} · {x.DayCount} days · Capacity: {x.Capacity}",
                StatusText = x.IsStatus ? "Active" : "Passive",
                ResultUrl = $"/AdminTour/UpdateTour?id={x.TourId}"
            }).ToList();
        }

        private async Task<List<AdminSearchItemDto>> SearchCategoriesAsync(string query, int limit)
        {
            var regex = new BsonRegularExpression(query, "i");

            var filter = Builders<Category>.Filter.Regex(x => x.CategoryName, regex);

            var categories = await _categoryCollection
                .Find(filter)
                .SortByDescending(x => x.CategoryId)
                .Limit(limit)
                .ToListAsync();

            return categories.Select(x => new AdminSearchItemDto
            {
                EntityType = "Category",
                EntityId = x.CategoryId,
                Title = x.CategoryName,
                Subtitle = "Category record",
                Description = "Category management result",
                StatusText = x.CategoryStatus ? "Active" : "Passive",
                ResultUrl = $"/AdminCategory/UpdateCategory?id={x.CategoryId}"
            }).ToList();
        }

        private async Task<List<AdminSearchItemDto>> SearchReservationsAsync(string query, int limit)
        {
            var regex = new BsonRegularExpression(query, "i");

            var filter = Builders<Reservation>.Filter.Or(
                Builders<Reservation>.Filter.Regex(x => x.ReservationNumber, regex),
                Builders<Reservation>.Filter.Regex(x => x.NameSurname, regex),
                Builders<Reservation>.Filter.Regex(x => x.Email, regex),
                Builders<Reservation>.Filter.Regex(x => x.Phone, regex),
                Builders<Reservation>.Filter.Regex(x => x.TourTitle, regex)
            );

            var reservations = await _reservationCollection
                .Find(filter)
                .SortByDescending(x => x.CreatedDate)
                .Limit(limit)
                .ToListAsync();

            return reservations.Select(x => new AdminSearchItemDto
            {
                EntityType = "Reservation",
                EntityId = x.ReservationId,
                Title = x.NameSurname,
                Subtitle = $"{x.TourTitle} · {x.ReservationNumber}",
                Description = $"{x.Email} · {x.PersonCount} person · Travel: {x.TravelDate:dd.MM.yyyy}",
                StatusText = x.Status ? "Approved" : "Pending",
                ResultUrl = "/AdminReservation/Index"
            }).ToList();
        }

        private async Task<List<AdminSearchItemDto>> SearchReviewsAsync(string query, int limit)
        {
            var regex = new BsonRegularExpression(query, "i");

            var filter = Builders<Review>.Filter.Or(
                Builders<Review>.Filter.Regex(x => x.NameSurname, regex),
                Builders<Review>.Filter.Regex(x => x.Detail, regex)
            );

            var reviews = await _reviewCollection
                .Find(filter)
                .SortByDescending(x => x.ReviewDate)
                .Limit(limit)
                .ToListAsync();

            var tourIds = reviews
                .Select(x => x.TourId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            var tours = tourIds.Count == 0
                ? new List<Tour>()
                : await _tourCollection.Find(x => tourIds.Contains(x.TourId)).ToListAsync();

            var tourDictionary = tours.ToDictionary(x => x.TourId, x => x.TourTitle);

            return reviews.Select(x => new AdminSearchItemDto
            {
                EntityType = "Review",
                EntityId = x.ReviewId,
                Title = x.NameSurname,
                Subtitle = tourDictionary.TryGetValue(x.TourId, out var tourTitle) ? tourTitle : "Tour not found",
                Description = x.Detail.Length > 90 ? x.Detail.Substring(0, 90) + "..." : x.Detail,
                StatusText = x.Status ? "Approved" : "Pending",
                ResultUrl = "/AdminReview/Index"
            }).ToList();
        }
    }
}