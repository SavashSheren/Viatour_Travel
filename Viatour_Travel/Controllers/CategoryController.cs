using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Dtos.CategoryDtos;
using Viatour_Travel.Services.CategoryService;

namespace Viatour_Travel.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDtos createCategoryDtos)
        {
            createCategoryDtos.CategoryStatus =true;
            await _categoryService.CreateCategoryAsync(createCategoryDtos);
            return RedirectToAction("CategoryList");
        }
    }
}
