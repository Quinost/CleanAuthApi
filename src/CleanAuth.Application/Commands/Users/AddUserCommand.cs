using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CleanAuth.Application.Commands.Users;

public record AddUserCommand(string UserName, string Password) : IRequest<Result>;

internal sealed class AddUserCommandHandler(IUserRepository userRepository) : IRequestHandler<AddUserCommand, Result>
{
    public async Task<Result> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var isUsernameUsed = await userRepository.CheckIfUsernameIsUsed(request.UserName, cancellationToken);
        if (isUsernameUsed)
            return Result.Failed("Username is used");

        var newUser = new User()
        {
            Username = request.UserName,
            Password = string.Empty
        };
        newUser.Password = new PasswordHasher<User>().HashPassword(newUser, request.Password);

        await userRepository.AddUserAsync(newUser);
        return Result.Ok();
    }
}