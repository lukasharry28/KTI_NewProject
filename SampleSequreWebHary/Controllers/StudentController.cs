using Microsoft.AspNetCore.Mvc;
using SampleSequreWebHary.Data;
using SampleSequreWebHary.Models;

namespace SampleSequreWebHary.Controllers;

public class StudentController : Controller
{
    private readonly IStudent _studentData;

    public StudentController(IStudent studentData)
    {
        _studentData = studentData;
    }
    public IActionResult Index()
    {
        var students = _studentData.GetStudents();
        return View(students);
    }

    [HttpPost]
    public IActionResult Create(Student student)
    {
        try
        {
            _studentData.AddStudent(student);
            return RedirectToAction("Index");
        }
        catch (System.Exception ex)
        {
            ViewBag.Error = ex.Message;
        }
        return View(student);
    }

    public IActionResult Edit()
    {
        return View();
    }
}