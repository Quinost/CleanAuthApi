using CleanAuth.Domain.Dtos.Roles;

namespace CleanAuth.Application.Queries.Roles;

public record GetRolesQuery(string Filter, int PageNumber = 1, int PageSize = 25) : IRequest<ICollection<RoleDto>>;

internal sealed class GetRolesQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRolesQuery, ICollection<RoleDto>>
{
    public async Task<ICollection<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        => Map(await roleRepository.GetAllPaginatedAsync(request.Filter, request.PageNumber, request.PageSize, cancellationToken));

    private static ICollection<RoleDto> Map(ICollection<Role> roles)
        => [.. roles.Select(x => new RoleDto(x.Id, x.Name))];
}