using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.DataAccessLayer.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
}
