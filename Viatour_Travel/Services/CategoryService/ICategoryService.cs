using Viatour_Travel.Dtos.CategoryDtos;

namespace Viatour_Travel.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllCategoryAsync();

        Task CreateCategoryAsync(CreateCategoryDtos createCategoryDto);
        Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto);
        Task DeleteCategoryDto(string id);
        Task<GetCategoryByIdDto> getCategoryByIdDto(string id);

    }


}
