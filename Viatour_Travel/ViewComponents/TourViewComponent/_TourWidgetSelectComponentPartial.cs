using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.CategoryService;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _TourWidgetSelectComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public _TourWidgetSelectComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string categoryId = null, string searchTerm = null)
        {
            var categories = await _categoryService.GetAllCategoryAsync();

            ViewBag.Categories = categories;
            ViewBag.CategoryId = categoryId;
            ViewBag.SearchTerm = searchTerm;

            return View();
        }
    }
}