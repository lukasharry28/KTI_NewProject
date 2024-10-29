using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureWeb.Data;
using SecureWeb.Models;
using SecureWeb.ViewModel;

namespace SecureWeb.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IUser _user;

        public AccountController(IUser user)
        {
            _user = user;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationViewModell registrationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IsValidPassword(registrationViewModel.Password))
                    {
                        ModelState.AddModelError("Password", "Password harus minimal 12 karakter dan mengandung huruf besar, huruf kecil, dan angka.");
                        return View(registrationViewModel);
                    }

                    var user = new User
                    {
                        Username = registrationViewModel.Username,
                        Password = BCrypt.Net.BCrypt.HashPassword(registrationViewModel.Password),
                        Role = "Contributor"
                    };

                    _user.Registratiion(user);
                    return RedirectToAction("Login", "Account");
                }
                return View(registrationViewModel);
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;
            }
            return View(registrationViewModel);
        }

        private bool IsValidPassword(string password)
        {
            return password.Length >= 12 && 
                   password.Any(char.IsUpper) && 
                   password.Any(char.IsLower) && 
                   password.Any(char.IsDigit);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var user = new User
                {
                    Username = loginViewModel.Username,
                    Password = loginViewModel.Password
                };

                var loginUser = _user.Login(user);
                if (loginUser == null)
                {
                    ViewBag.error = "Username atau password tidak valid.";
                    return View(loginViewModel);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = loginViewModel.RememberMe
                    }
                );

                return RedirectToAction("Index", "Home");
            }
            catch (System.Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(loginViewModel);
        }

        public ActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel
            {
                Username = User.Identity.Name
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid || !IsValidPassword(model.NewPassword))
            {
                if (!IsValidPassword(model.NewPassword))
                {
                    ModelState.AddModelError("NewPassword", "Password baru harus minimal 12 karakter dan mengandung huruf besar, huruf kecil, dan angka.");
                }
                return View(model);
            }

            var user = _user.GetUserByUsername(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Pengguna tidak ditemukan.");
                return View(model);
            }

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                ModelState.AddModelError("", "Password lama tidak benar.");
                return View(model);
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            _user.UpdatePassword(user);

            ViewBag.Message = "Password berhasil diubah.";
            return View(model);
        }
    }
}
