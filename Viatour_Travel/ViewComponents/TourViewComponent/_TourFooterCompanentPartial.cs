using Microsoft.AspNetCore.Mvc;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _TourFooterCompanentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
