using AutoMapper;
using MongoDB.Driver;
using Viatour_Travel.Dtos.TourDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.TourPlanServices
{
    public class TourPlanManager : ITourPlanService
    {
        private readonly IMongoCollection<TourPlan> _tourPlanCollection;
        private readonly IMapper _mapper;

        public TourPlanManager(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _tourPlanCollection = database.GetCollection<TourPlan>("TourPlans");
            _mapper = mapper;
        }

        public async Task<List<TourPlanDto>> GetTourPlansByTourIdAsync(string tourId)
        {
            var values = await _tourPlanCollection
                .Find(x => x.TourId == tourId)
                .ToListAsync();

            return values
                .OrderBy(x => x.DayNumber)
                .Select(x => _mapper.Map<TourPlanDto>(x))
                .ToList();
        }

        public async Task<TourPlanDto> GetByIdAsync(string id)
        {
            var value = await _tourPlanCollection
                .Find(x => x.TourPlanId == id)
                .FirstOrDefaultAsync();

            if (value == null)
                return null;

            return _mapper.Map<TourPlanDto>(value);
        }

        public async Task CreateAsync(TourPlanDto dto)
        {
            var value = _mapper.Map<TourPlan>(dto);
            await _tourPlanCollection.InsertOneAsync(value);
        }

        public async Task UpdateAsync(TourPlanDto dto)
        {
            var value = _mapper.Map<TourPlan>(dto);

            await _tourPlanCollection.ReplaceOneAsync(
                x => x.TourPlanId == dto.TourPlanId,
                value);
        }

        public async Task DeleteAsync(string id)
        {
            await _tourPlanCollection.DeleteOneAsync(x => x.TourPlanId == id);
        }
    }
}