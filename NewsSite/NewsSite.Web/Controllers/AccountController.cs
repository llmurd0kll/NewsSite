using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsSite.Web.Models;

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
        public IActionResult Login(string? returnUrl = null)
            {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
            {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
                }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
                {
                return Redirect(model.ReturnUrl ?? "/");
                }

            ModelState.AddModelError("", "Неверный логин или пароль");
            return View(model);
            }

        // GET: /Account/Register
        public IActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
            {
            if (!ModelState.IsValid)
                return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                {
                // по умолчанию обычный пользователь
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
                }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
            }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
            {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
            }
        }
    }
