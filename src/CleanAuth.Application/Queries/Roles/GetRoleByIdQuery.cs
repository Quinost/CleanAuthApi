using CleanAuth.Domain.Dtos.Roles;

namespace CleanAuth.Application.Queries.Roles;
public record GetRoleByIdQuery(Guid RoleId) : IRequest<RoleDto?>;

internal sealed class GetRoleByIdQueryHandler(IRoleRepository roleRepository) : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken) 
        => Map(await roleRepository.GetByIdAsync(request.RoleId, cancellationToken));

    private static RoleDto? Map(Role? role) => role is null ? null : new RoleDto(role.Id, role.Name);
}
