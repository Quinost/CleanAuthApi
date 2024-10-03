using CleanAuth.Domain.Entities;
using CleanAuth.Infrastructure.EF;
using CleanAuth.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.Auth;

internal sealed class UserManager(CleanDbContext context) : IUserManager
{
    public Task<User?> FindByNameAsync(string username, CancellationToken token = default)
    {
        return context.Users
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == username, token);
    }

    public bool CheckPassword(User user, string password)
    {
        var isProper = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);
        return isProper is PasswordVerificationResult.Success;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        var result = await context.SaveChangesAsync();
        return result is 1;
    }

    public void Deactivate(User user)
    {
        user.IsActive = false;
    }

    public void Activate(User user)
    {
        user.IsActive = true;
    }
}
