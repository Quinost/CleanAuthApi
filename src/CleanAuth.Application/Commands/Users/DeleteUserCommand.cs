﻿using CleanAuth.Domain.Interfaces;

namespace CleanAuth.Application.Commands.Users;

public record DeleteUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failed("User not found");

        await userRepository.DeleteUserAsync(user, cancellationToken);
        return Result.Ok();
    }
}
