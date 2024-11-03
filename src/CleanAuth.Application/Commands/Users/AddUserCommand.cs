using FluentValidation;
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

internal sealed class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    AddUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username min length is 3");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(5).WithMessage("Password min length is 5");

        RuleFor(x => x.RoleId)
            .Must(x => x != Guid.Empty).WithMessage("Role is required");
    }
}