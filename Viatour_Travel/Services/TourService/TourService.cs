using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Dtos.TourImageDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.TourServices
{
    public class TourService : ITourService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMongoCollection<TourImage> _tourImageCollection;
        private readonly IMongoCollection<TourPlan> _tourPlanCollection;
        private readonly IMongoCollection<Review> _reviewCollection;

        public TourService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);

            _tourCollection = database.GetCollection<Tour>(_databaseSettings.TourCollectionName);
            _tourImageCollection = database.GetCollection<TourImage>(_databaseSettings.TourImageCollectionName);
            _tourPlanCollection = database.GetCollection<TourPlan>(_databaseSettings.TourPlanCollectionName);
            _reviewCollection = database.GetCollection<Review>(_databaseSettings.ReviewCollectionName);

            _mapper = mapper;
        }

        public async Task<(List<ResultTourDto> Tours, int TotalCount)> GetPagedToursAsync(
       int page,
       int pageSize,
       string searchTerm,
       string categoryId,
       string duration,
       int? guestCount)
        {
            var filterBuilder = Builders<Tour>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchFilter =
                    filterBuilder.Regex(x => x.TourTitle, new BsonRegularExpression(searchTerm, "i")) |
                    filterBuilder.Regex(x => x.TourDescription, new BsonRegularExpression(searchTerm, "i")) |
                    filterBuilder.Regex(x => x.Location, new BsonRegularExpression(searchTerm, "i"));

                filter &= searchFilter;
            }

            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                filter &= filterBuilder.Eq(x => x.CategoryId, categoryId);
            }

            if (!string.IsNullOrWhiteSpace(duration))
            {
                switch (duration)
                {
                    case "1-3":
                        filter &= filterBuilder.Gte(x => x.DayCount, 1) & filterBuilder.Lte(x => x.DayCount, 3);
                        break;

                    case "4-7":
                        filter &= filterBuilder.Gte(x => x.DayCount, 4) & filterBuilder.Lte(x => x.DayCount, 7);
                        break;

                    case "8-12":
                        filter &= filterBuilder.Gte(x => x.DayCount, 8) & filterBuilder.Lte(x => x.DayCount, 12);
                        break;

                    case "12plus":
                        filter &= filterBuilder.Gte(x => x.DayCount, 12);
                        break;
                }
            }

            if (guestCount.HasValue && guestCount.Value > 0)
            {
                filter &= filterBuilder.Gte(x => x.Capacity, guestCount.Value);
            }

            var totalCount = (int)await _tourCollection.CountDocumentsAsync(filter);

            var tours = await _tourCollection
                .Find(filter)
                .SortByDescending(x => x.TourId)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            var mappedTours = _mapper.Map<List<ResultTourDto>>(tours);

            return (mappedTours, totalCount);
        }
        public async Task<List<ResultTourDto>> GetAllTourAsync()
        {
            var values = await _tourCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultTourDto>>(values);
        }

        public async Task<GetTourByIdDto> GetTourByIdAsync(string id)
        {
            var value = await _tourCollection.Find(x => x.TourId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetTourByIdDto>(value);
        }

        public async Task<GetTourDetailDto> GetTourDetailAsync(string id)
        {
            var tour = await _tourCollection
                .Find(x => x.TourId == id)
                .FirstOrDefaultAsync();

            if (tour == null)
            {
                return null;
            }

            var images = await _tourImageCollection
                .Find(x => x.TourId == id)
                .ToListAsync();

            var plans = await _tourPlanCollection
                .Find(x => x.TourId == id)
                .SortBy(x => x.DayNumber)
                .ToListAsync();

            var reviews = await _reviewCollection
                .Find(x => x.TourId == id)
                .ToListAsync();

            var result = _mapper.Map<GetTourDetailDto>(tour);

            result.TourImages = _mapper.Map<List<ResultTourImageDto>>(images);
            result.TourPlans = _mapper.Map<List<TourPlanDto>>(plans);
            result.Reviews = _mapper.Map<List<ResultReviewByTourIdDto>>(reviews);

            return result;
        }

        public async Task CreateTourAsync(CreateTourDto createTourDto)
        {
            var value = _mapper.Map<Tour>(createTourDto);
            await _tourCollection.InsertOneAsync(value);
        }

        public async Task UpdateTourAsync(UpdateTourDto updateTourDto)
        {
            var existingTour = await _tourCollection
                .Find(x => x.TourId == updateTourDto.TourId)
                .FirstOrDefaultAsync();

            if (existingTour == null)
                throw new Exception("Tour not found.");

            var value = _mapper.Map<Tour>(updateTourDto);

            // Kritik alanları garantiye al
            value.TourId = existingTour.TourId;

            // Eğer formdan boş geldiyse mevcut veriyi koru
            if (string.IsNullOrWhiteSpace(value.CategoryId))
                value.CategoryId = existingTour.CategoryId;

            if (string.IsNullOrWhiteSpace(value.MapLocationImageUrl))
                value.MapLocationImageUrl = existingTour.MapLocationImageUrl;

            var result = await _tourCollection.ReplaceOneAsync(
                x => x.TourId == updateTourDto.TourId,
                value);

            if (result.MatchedCount == 0)
                throw new Exception("Tour update failed. No matching record found.");
        }

        public async Task DeleteTourAsync(string id)
        {
            await _tourCollection.DeleteOneAsync(x => x.TourId == id);
        }
    }
}