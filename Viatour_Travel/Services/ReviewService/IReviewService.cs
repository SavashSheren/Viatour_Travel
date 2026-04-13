using Viatour_Travel.Dtos.ReviewDtos;

namespace Viatour_Travel.Services.ReviewService
{
    public interface IReviewService
    {
        Task CreateReviewAsync(CreateReviewDto createReviewDto);
        Task<List<ResultReviewDto>> GetAllReviewsAsync();
        Task<List<ResultReviewByTourIdDto>> GetAllReviewsByTourIdAsync(string tourId);
        Task<GetReviewByIdDto?> GetReviewByIdAsync(string reviewId);
        Task ApproveReviewAsync(string reviewId);
        Task DeleteReviewAsync(string reviewId);
    }
}