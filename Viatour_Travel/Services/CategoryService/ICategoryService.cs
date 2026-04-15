using Viatour_Travel.Dtos.CategoryDtos;

namespace Viatour_Travel.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();
        Task<CreateCategoryDto?> GetCategoryByIdForCreateAsync(string id);
        Task<UpdateCategoryDto?> GetCategoryByIdAsync(string id);
        Task CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryAsync(string id);
    }
}