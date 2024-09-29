using CleanAuth.Domain.Dtos.Users;
using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;

namespace CleanAuth.Application.Queries.Users;

public record GetUserQuery(Guid UserId) : IRequest<UserDto?>;

internal sealed class GetUserQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserQuery request, CancellationToken cancellationToken) 
        => Map(await userRepository.GetUserByIdAsync(request.UserId, cancellationToken));

    private static UserDto? Map(User? user) => user is null ? null : new UserDto(user.Id, user.Username, user.IsActive);
}
