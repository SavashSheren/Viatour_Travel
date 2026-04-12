using Microsoft.AspNetCore.Mvc;
using Viatour_Travel.Services.TourServices;

public class _TourListByCategoryComponentPartial : ViewComponent
{
    private readonly ITourService _tourService;

    public _TourListByCategoryComponentPartial(ITourService tourService)
    {
        _tourService = tourService;
    }

    public async Task<IViewComponentResult> InvokeAsync(string categoryId, int page = 1)
    {
        int pageSize = 9;

        var values = await _tourService.GetPagedToursAsync(
            page,
            pageSize,
            null,
            categoryId,
            null,
            null
        );

        ViewBag.CategoryId = categoryId;
        ViewBag.Page = page;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalCount = values.TotalCount;

        return View(values.Tours);
    }
}