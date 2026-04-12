using Microsoft.AspNetCore.Mvc;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _TourHeadComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
