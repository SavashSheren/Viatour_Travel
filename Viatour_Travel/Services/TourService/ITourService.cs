using System.Collections.Generic;
using Viatour_Travel.Dtos.TourDtos;

namespace Viatour_Travel.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllTourAsync();
        Task CreateTourAsync(CreateTourDto createTourDto);
        Task UpdateTourAsync(UpdateTourDto updateTourDto);
        Task DeleteTourAsync(string id);
        Task<GetTourByIdDto> GetTourByIdAsync(string id);
        Task<(List<ResultTourDto> Tours, int TotalCount)> GetPagedToursAsync(int page, int pageSize, string searchTerm, string categoryId ,string duration,
    int? guestCount);
        Task<GetTourDetailDto> GetTourDetailAsync(string id);

    }
}
