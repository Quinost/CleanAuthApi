using CleanAuth.Domain.Dtos.Auth;
using FluentValidation;

namespace CleanAuth.Application.Commands.Auth;
public record LoginCommand(string UserName, string Password) : IRequest<Result<TokenResultDto>>;

internal sealed class LoginCommandHandler(ITokenService tokenService) : IRequestHandler<LoginCommand, Result<TokenResultDto>>
{
    public Task<Result<TokenResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return tokenService.Login(request.UserName, request.Password, cancellationToken);
    }
}

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    LoginCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}