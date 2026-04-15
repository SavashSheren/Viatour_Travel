using AutoMapper;
using MongoDB.Driver;
using Viatour_Travel.Dtos.ReservationDtos;
using Viatour_Travel.Entities;
using Viatour_Travel.Settings;

namespace Viatour_Travel.Services.ReservationService
{
    public class ReservationService : IReservationService
    {
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMapper _mapper;

        public ReservationService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _reservationCollection = database.GetCollection<Reservation>(databaseSettings.ReservationCollectionName);
            _mapper = mapper;
        }

        public async Task CreateReservationAsync(CreateReservationDto createReservationDto)
        {
            var value = _mapper.Map<Reservation>(createReservationDto);
            value.CreatedDate = DateTime.UtcNow;
            value.Status = false;
            value.ReservationNumber = $"VT-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(100, 999)}";

            await _reservationCollection.InsertOneAsync(value);
        }

        public async Task<List<ResultReservationDto>> GetAllReservationsAsync()
        {
            var values = await _reservationCollection
                .Find(x => true)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(values);
        }

        public async Task<ResultReservationDto?> GetReservationByIdAsync(string reservationId)
        {
            var value = await _reservationCollection
                .Find(x => x.ReservationId == reservationId)
                .FirstOrDefaultAsync();

            if (value == null)
            {
                return null;
            }

            return _mapper.Map<ResultReservationDto>(value);
        }

        public async Task<List<ResultReservationDto>> GetReservationsByTourIdAsync(string tourId)
        {
            var values = await _reservationCollection
                .Find(x => x.TourId == tourId)
                .SortByDescending(x => x.CreatedDate)
                .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(values);
        }

        public async Task ApproveReservationAsync(string reservationId)
        {
            var filter = Builders<Reservation>.Filter.Eq(x => x.ReservationId, reservationId);
            var update = Builders<Reservation>.Update.Set(x => x.Status, true);

            await _reservationCollection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteReservationAsync(string reservationId)
        {
            await _reservationCollection.DeleteOneAsync(x => x.ReservationId == reservationId);
        }
    }
}