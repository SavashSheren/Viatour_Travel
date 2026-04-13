using Viatour_Travel.Dtos.TourImageDtos;

namespace Viatour_Travel.Services.TourImageService
{
    public interface ITourImageService
    {

        Task<List<ResultTourImageDto>> GetByTourIdAsync(string tourId);
        Task<ResultTourImageDto?> GetByIdAsync(string tourImageId);
        Task AddRangeAsync(List<CreateTourImageDto> createTourImageDtos);
        Task DeleteAsync(string tourImageId);
    }
}