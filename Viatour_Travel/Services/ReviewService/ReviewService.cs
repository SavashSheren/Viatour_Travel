using AutoMapper;
using MongoDB.Driver;
using Viatour_Travel.Dtos.CategoryDtos;
using Viatour_Travel.Dtos.ReviewDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.ReviewService
{
    public class ReviewService : IReviewService
    {

        private readonly IMapper _mapper;
        private readonly IMongoCollection<Review> _reviewCollection;
        public ReviewService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _reviewCollection = database.GetCollection<Review>(_databaseSettings.ReviewCollectionName);
            _mapper = mapper;
        }
        public async Task CreateReviewAsync(CreateReviewDto createReviewDto)
        {
            var value = _mapper.Map<Review>(createReviewDto);
            await _reviewCollection.InsertOneAsync(value);
        }

        public async Task DeleteReviewAsync(string id)
        {
            await _reviewCollection.DeleteOneAsync(x => x.ReviewId == id);
        }

        public async Task<List<ResultReviewDto>> GetAllReviewAsync()
        {

            var Value = await _reviewCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultReviewDto>>(Value);
        }

        public async Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string id)
        {
            var values = await _reviewCollection.Find(x => x.TourId == id).ToListAsync();
            return _mapper.Map<List<ResultReviewByTourIdDto>>(values);
        }

        public async Task<GetReviewById> GetReviewById(string id)
        {
            var value = await _reviewCollection.Find(x => x.ReviewId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetReviewById>(value);
        }

        public async Task UpdateReviewAsync(UpdateReviewDto updateReviewDto)
        {
            var value = _mapper.Map<Review>(updateReviewDto);
            await _reviewCollection.FindOneAndReplaceAsync(x => x.ReviewId == updateReviewDto.ReviewId, value);
        }
    }
}
