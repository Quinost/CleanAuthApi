namespace CleanAuth.Application.Commands.Users;

public record DeleteUserCommand(Guid UserId) : IRequest<Result>;

internal sealed class DeleteUserCommandHandler(IUserRepository userRepository) : IRequestHandler<DeleteUserCommand, Result>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return Result.Failed("User not found");

        await userRepository.DeleteAsync(user, cancellationToken);
        return Result.Ok();
    }
}
