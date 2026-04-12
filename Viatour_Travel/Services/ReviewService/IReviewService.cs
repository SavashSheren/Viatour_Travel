using Viatour_Travel.Dtos.ReviewDtos;

namespace Viatour_Travel.Services.ReviewService
{
    public interface IReviewService
    {
        Task<List<ResultReviewDto>> GetAllReviewAsync();
        Task CreateReviewAsync(CreateReviewDto createReviewDto);
        Task UpdateReviewAsync(UpdateReviewDto updateReviewDto);
        Task DeleteReviewAsync(String id);
        Task<GetReviewById> GetReviewById(string id);
        Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string id);
    }
}
