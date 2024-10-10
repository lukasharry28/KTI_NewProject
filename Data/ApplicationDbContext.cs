using System;
using Microsoft.EntityFrameworkCore;
using SecureWeb.Models;

namespace SecureWeb.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
    {

    }

    public DbSet<Student> Students { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

}
