using CleanAuth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.EF;
internal class CleanDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CleanDbContext).Assembly);
        AddDefaultUser(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void AddDefaultUser(ModelBuilder modelBuilder)
    {
        var adminRole = new Role
        {
            Id = new Guid("f3f533fd-41c4-44f1-a67e-8062e6d207a6"),
            Name = "Admin"
        };

        modelBuilder.Entity<Role>().HasData(adminRole);

        var defaultUser = new User
        {
            Id = new Guid("aa5d84da-88cd-45a2-b5fb-233fd99b061d"),
            Username = "AdminUser",
            IsActive = true,
            Password = "",
            RoleId = adminRole.Id
        };

        defaultUser.Password = new PasswordHasher<User>().HashPassword(defaultUser, "securePass");
        modelBuilder.Entity<User>().HasData(defaultUser);
    }
}
