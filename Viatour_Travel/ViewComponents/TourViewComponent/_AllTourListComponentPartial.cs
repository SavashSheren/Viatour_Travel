using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.TourServices;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _AllTourListComponentPartial : ViewComponent
    {
        private readonly ITourService _tourService;
        private const int PageSize = 6;

        public _AllTourListComponentPartial(ITourService tourService)
        {
            _tourService = tourService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int page = 1, string searchTerm = null, string categoryId = null,
            string duration = null,
            int? guestCount = null)
        {
            var (tours, totalCount) = await _tourService.GetPagedToursAsync(page, PageSize, searchTerm, categoryId,
                duration,
                guestCount);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
            ViewBag.SearchTerm = searchTerm;
            ViewBag.CategoryId = categoryId;
            ViewBag.Duration = duration;
            ViewBag.GuestCount = guestCount;


            return View(tours);
        }
    }
}