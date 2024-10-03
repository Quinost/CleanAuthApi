using Microsoft.AspNetCore.Identity;

namespace CleanAuth.Application.Commands.Users;

public record AddUserCommand(string UserName, string Password, Guid RoleId) : IRequest<Result>;

internal sealed class AddUserCommandHandler(IUserRepository userRepository) : IRequestHandler<AddUserCommand, Result>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var isUsernameUsed = await userRepository.CheckIfUsernameIsUsedAsync(request.UserName, cancellationToken);
        if (isUsernameUsed)
            return Result.Failed("Username is used");

        var newUser = new User()
        {
            Username = request.UserName,
            Password = string.Empty,
            RoleId = request.RoleId
        };
        newUser.Password = new PasswordHasher<User>().HashPassword(newUser, request.Password);

        await userRepository.AddAsync(newUser);
        return Result.Ok();
    }
}