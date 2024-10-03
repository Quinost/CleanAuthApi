using CleanAuth.Domain.Dtos.Auth;

namespace CleanAuth.Application.Commands.Auth;
public record LoginCommand(string UserName, string Password) : IRequest<Result<TokenResultDto>>;

internal sealed class LoginCommandHandler(ITokenService tokenService) : IRequestHandler<LoginCommand, Result<TokenResultDto>>
{
    public Task<Result<TokenResultDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        return tokenService.Login(request.UserName, request.Password, cancellationToken);
    }
}
