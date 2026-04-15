using Viatour_Travel.Dtos.AdminSearchDtos;

namespace Viatour_Travel.Services.AdminSearchService
{
    public interface IAdminSearchService
    {
        Task<ResultAdminSearchDto> SearchAllAsync(string query, int limitPerType = 5);
        Task<List<AdminSearchItemDto>> SearchSuggestionsAsync(string query, int totalLimit = 8);
    }
}