using System;
using System.ComponentModel.DataAnnotations;

namespace SecureWeb.Models;

public class Student
{
    [Key]
    public string nim { get; set; } = null!;

    public string name { get; set; } = null!;
}
