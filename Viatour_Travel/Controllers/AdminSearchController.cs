using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.AdminSearchService;

namespace Viatour_Travel.Controllers
{
    public class AdminSearchController : Controller
    {
        private readonly IAdminSearchService _adminSearchService;

        public AdminSearchController(IAdminSearchService adminSearchService)
        {
            _adminSearchService = adminSearchService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query)
        {
            var model = await _adminSearchService.SearchAllAsync(query);
            ViewBag.SearchQuery = query;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LiveSearch(string query)
        {
            var results = await _adminSearchService.SearchSuggestionsAsync(query);
            return Json(results);
        }
    }
}