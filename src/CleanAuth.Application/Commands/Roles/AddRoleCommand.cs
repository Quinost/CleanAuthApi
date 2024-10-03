namespace CleanAuth.Application.Commands.Roles;
public record AddRoleCommand(string Name) : IRequest<Result>;

internal sealed class AddRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<AddRoleCommand, Result>
{
    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        if (await roleRepository.IsNameUsedAsync(request.Name, cancellationToken))
            return Result.Failed("Role name is used");

        var role = new Role
        {
            Name = request.Name,
        };

        await roleRepository.AddAsync(role);
        return Result.Ok();
    }
}
