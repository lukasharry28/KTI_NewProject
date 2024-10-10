using System;
using SecureWeb.Models;

namespace SecureWeb.Data;

public interface IStudent
{
    IEnumerable<Student> GetStudents();
    Student GetStudent(string nim);
    Student AddStudent(Student student);
    Student UpdateStudent(Student student);
    void DeleteStudent(string nim);
}
