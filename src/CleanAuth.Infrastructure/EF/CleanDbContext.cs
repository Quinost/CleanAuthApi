using CleanAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.EF;
internal class CleanDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanDbContext).Assembly);
        AddDefaultUser(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void AddDefaultUser(ModelBuilder modelBuilder)
    {
        var defaultUser = new User
        {
            Id = Guid.NewGuid(),
            Username = "DefaultUser",
            IsActive = true,
            Password = ""
        };
        defaultUser.Password = new PasswordHasher<User>().HashPassword(defaultUser, "securePass");
        modelBuilder.Entity<User>().HasData(defaultUser);
    }
}
