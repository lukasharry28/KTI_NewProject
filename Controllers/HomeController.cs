using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecureWeb.Models;
using Microsoft.AspNetCore.Authorization;

namespace SecureWeb.Controllers;

// Controller ini hanya bisa diakses oleh pengguna yang sudah login
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Aksi ini akan diarahkan ke login jika pengguna tidak terautentikasi
    public IActionResult Index()
    {
        ViewBag.username = User.Identity.Name;
        string[] fruits = new string[] { "Banana", "Mango", "Orange" };
        ViewBag.fruits = fruits;
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
