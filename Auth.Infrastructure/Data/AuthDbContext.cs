using Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data;

public class AuthDbContext(DbContextOptions<AuthDbContext> options) : DbContext(options)
{
    // TODO: refactor this piece of shit
    public DbSet<Moderator> Moderators { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
}
