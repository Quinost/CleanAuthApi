using CleanAuth.Domain.Dtos.Users;
using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using System.Data;

namespace CleanAuth.Application.Queries.Users;
public record GetUsersQuery(string Filter, int PageNumber = 1, int PageSize = 25) : IRequest<ICollection<UserDto>>;

internal sealed class GetUsersQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUsersQuery, ICollection<UserDto>>
{
    public async Task<ICollection<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken) 
        => Map(await userRepository.GetUsersAsync(request.Filter, request.PageNumber, request.PageSize));

    private static ICollection<UserDto> Map(ICollection<User> users)
        => [..users.Select(x => new UserDto(x.Id, x.Username, x.IsActive))];
}
