using AutoMapper;
using MongoDB.Driver;
using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Review> _reviewCollection;

        public ReviewService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _reviewCollection = database.GetCollection<Review>(databaseSettings.ReviewCollectionName);
            _mapper = mapper;
        }

        public async Task CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var value = _mapper.Map<Review>(createReviewDto);
            value.ReviewDate = DateTime.UtcNow;
            value.Status = false;

            await _reviewCollection.InsertOneAsync(value);
        }

        public async Task<List<ResultReviewDto>> GetAllReviewsAsync()
        {
            var values = await _reviewCollection
                .Find(x => true)
                .SortBy(x => x.Status)
                .ThenByDescending(x => x.ReviewDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReviewDto>>(values);
        }

        public async Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string tourId)
        {
            var values = await _reviewCollection
                .Find(x => x.TourId == tourId && x.Status)
                .SortByDescending(x => x.ReviewDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReviewByTourIdDto>>(values);
        }

        public async Task<GetReviewByIdDto?> GetReviewByIdAsync(string reviewId)
        {
            var value = await _reviewCollection
                .Find(x => x.ReviewId == reviewId)
                .FirstOrDefaultAsync();

            if (value == null)
            {
                return null;
            }

            return _mapper.Map<GetReviewByIdDto>(value);
        }

        public async Task ApproveReviewAsync(string reviewId)
        {
            var filter = Builders<Review>.Filter.Eq(x => x.ReviewId, reviewId);
            var update = Builders<Review>.Update.Set(x => x.Status, true);

            await _reviewCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteReviewAsync(string reviewId)
        {
            await _reviewCollection.DeleteOneAsync(x => x.ReviewId == reviewId);
        }
    }
}