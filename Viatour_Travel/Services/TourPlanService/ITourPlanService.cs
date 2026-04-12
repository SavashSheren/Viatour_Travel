using Viatour_Travel.Dtos.TourDtos;

namespace Viatour_Travel.Services.TourPlanServices
{
    public interface ITourPlanService
    {
        Task<List<TourPlanDto>> GetTourPlansByTourIdAsync(string tourId);
        Task<TourPlanDto> GetByIdAsync(string id);
        Task CreateAsync(TourPlanDto dto);
        Task UpdateAsync(TourPlanDto dto);
        Task DeleteAsync(string id);
    }
}