using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace NewsSite.Web.Controllers
    {
    public class CultureController : Controller
        {
        [HttpGet]
        public IActionResult Set(string culture, string returnUrl)
            {
            if (string.IsNullOrWhiteSpace(culture))
                culture = "ru"; // дефолт

            // Записываем cookie с выбранной культурой
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Возвращаемся на ту же страницу
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
            }
        }
    }
