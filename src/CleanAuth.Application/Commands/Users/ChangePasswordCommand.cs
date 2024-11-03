using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CleanAuth.Application.Commands.Users;

public record ChangePasswordRequest(string OldPassword, string NewPassword);
public record ChangePasswordCommand(string? UserName, string OldPassword, string NewPassword) : IRequest<Result>;

internal sealed class ChangePasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly PasswordHasher<User> passwordHasher = new();
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByUsernameAsync(request.UserName ?? string.Empty, cancellationToken);

        if (user is null)
            return Result.Failed("User not found");

        var isOldPasswordProper = passwordHasher.VerifyHashedPassword(user, user.Password, request.OldPassword);

        if (isOldPasswordProper is PasswordVerificationResult.Failed)
            return Result.Failed("Old password incorrect");

        user.Password = passwordHasher.HashPassword(user, request.NewPassword);

        return Result.Ok();
    }
}

internal sealed class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    ChangePasswordRequestValidator()
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage("Both password is required");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Both password is required")
            .MinimumLength(5).WithMessage("New password min length: 5");
    }
}
