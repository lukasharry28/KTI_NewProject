using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SampleSequreWebHary.Models;

namespace SampleSequreWebHary.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewData["Title"] = "About Page";
        ViewBag.username = "Hary";
        string[] fruits = new string[] {"anggur", "pisang", "semangka", "pepaya"};
        string[] animal = new string[] {"Singa", "Kucing", "Anjing", "Burung"};
        ViewBag.fruits = fruits;
        ViewBag.animal = animal;
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
