using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Viatour_Travel.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult Change(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
            );

            if (string.IsNullOrEmpty(returnUrl))
            {
                return RedirectToAction("TourList", "Tour");
            }

            return LocalRedirect(returnUrl);
        }
    }
}