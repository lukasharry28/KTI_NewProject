using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SecureWeb.Data;
using SecureWeb.Models;
using SecureWeb.ViewModel;

namespace SecureWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser _user;
        public AccountController(IUser user)
        {
            _user = user;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(RegistrationViewModell registrationViewModell)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!IsValidPassword(registrationViewModell.Password))
                    {
                        ModelState.AddModelError("Password", "- Min character 12.-Harus mengandung huruf besar, huruf kecil, dan angka");
                        return View(registrationViewModell); // Tetap di halaman registrasi jika validasi gagal
                    }
                    var user = new Models.User
                    {
                        Username = registrationViewModell.Username,
                        Password = registrationViewModell.Password,
                        Role = "Contributor"
                    };
                    _user.Registratiion(user);
                    return RedirectToAction("Login", "Account");
                }
                return View(registrationViewModell);
            }
            catch (System.Exception ex)
            {
                ViewBag.error = ex.Message;

            }
            return View(registrationViewModell);
        }

        private bool IsValidPassword(string password)
        {
            if (password.Length < 12) return false;
            if (!password.Any(char.IsUpper)) return false;
            if (!password.Any(char.IsLower)) return false;
            if (!password.Any(char.IsDigit)) return false;
            return true;
        }

        public ActionResult Login()
        {
            return View();
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
                    ViewBag.error = "Invalid username or password";
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
    }
}
