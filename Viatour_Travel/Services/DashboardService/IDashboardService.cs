using Viatour_Travel.Dtos.DashboardDtos;

namespace Viatour_Travel.Services.DashboardService
{
    public interface IDashboardService
    {
        Task<ResultDashboardDto> GetDashboardDataAsync();
    }
}