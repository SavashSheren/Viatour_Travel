using Microsoft.AspNetCore.Mvc;

namespace Viatour_Travel.ViewComponents.TourViewComponent
{
    public class _TourBrandComponentPartial :ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
