﻿using FluentValidation;

namespace CleanAuth.Application.Commands.Auth;
public record LogoutCommand(string AccessToken) : IRequest;

internal sealed class LogoutCommandHandler(ITokenService tokenService) : IRequestHandler<LogoutCommand>
{
    public Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(tokenService.Logout(request.AccessToken));
    }
}

internal sealed class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    LogoutCommandValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Token is required");
    }
}