using Microsoft.AspNetCore.Mvc;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _TourHeaderComponentPartial : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();

        }
    }
}
