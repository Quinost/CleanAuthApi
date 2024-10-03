using CleanAuth.Domain.Dtos.Users;

namespace CleanAuth.Application.Queries.Users;

public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;

internal sealed class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken) 
        => Map(await userRepository.GetByIdAsync(request.UserId, cancellationToken));

    private static UserDto? Map(User? user) => user is null ? null : new UserDto(user.Id,
                                                                                 user.Username,
                                                                                 user.IsActive,
                                                                                 user.RoleId,
                                                                                 user.Role?.Name);
}
