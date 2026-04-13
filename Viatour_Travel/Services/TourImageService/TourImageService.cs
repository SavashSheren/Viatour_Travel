using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Viatour_Travel.Dtos.TourImageDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.TourImageService
{
    public class TourImageService : ITourImageService
    {
        private readonly IMongoCollection<TourImage> _tourImageCollection;
        private readonly IMapper _mapper;

        public TourImageService(IOptions<DatabaseSettings> databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(databaseSettings.Value.DatabaseName);
            _tourImageCollection = database.GetCollection<TourImage>(databaseSettings.Value.TourImageCollectionName);
            _mapper = mapper;
        }

        public async Task<List<ResultTourImageDto>> GetByTourIdAsync(string tourId)
        {
            var values = await _tourImageCollection
                .Find(x => x.TourId == tourId)
                .ToListAsync();

            return _mapper.Map<List<ResultTourImageDto>>(values);
        }

        public async Task<ResultTourImageDto?> GetByIdAsync(string tourImageId)
        {
            var value = await _tourImageCollection
                .Find(x => x.TourImageId == tourImageId)
                .FirstOrDefaultAsync();

            if (value == null)
            {
                return null;
            }

            return _mapper.Map<ResultTourImageDto>(value);
        }

        public async Task AddRangeAsync(List<CreateTourImageDto> createTourImageDtos)
        {
            if (createTourImageDtos == null || !createTourImageDtos.Any())
            {
                return;
            }

            var values = _mapper.Map<List<TourImage>>(createTourImageDtos);
            await _tourImageCollection.InsertManyAsync(values);
        }

        public async Task DeleteAsync(string tourImageId)
        {
            await _tourImageCollection.DeleteOneAsync(x => x.TourImageId == tourImageId);
        }
    }
}