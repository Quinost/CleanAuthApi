namespace CleanAuth.Domain.Entities;
public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool IsActive { get; set; } = true;
}
