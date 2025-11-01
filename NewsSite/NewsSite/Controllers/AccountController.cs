using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NewsSite.Web.Controllers
    {
    public class AccountController : Controller
        {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
            {
            _signInManager = signInManager;
            _userManager = userManager;
            }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
            {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
            }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
            {
            ViewData["ReturnUrl"] = returnUrl;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                ModelState.AddModelError("", "Введите email и пароль");
                return View();
                }

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
                {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
                }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View();
            }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
            {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            }
        }
    }
