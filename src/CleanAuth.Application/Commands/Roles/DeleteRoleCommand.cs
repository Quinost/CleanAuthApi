using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CleanAuth.Application.Commands.Roles;
public record DeleteRoleCommand(Guid RoleId) : IRequest<Result>;

internal sealed class DeleteRoleCommandHandler(IRoleRepository roleRepository) : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role is null)
            return Result.Failed("Role not found");

        if (role.Users.Count != 0)
            return Result.Failed("Role used");

        await roleRepository.DeleteAsync(role, cancellationToken);
        return Result.Ok();
    }
}

internal sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    DeleteRoleCommandValidator()
    {
        RuleFor(x => x.RoleId)
            .Must(x => x != Guid.Empty)
            .WithMessage("RoleId is required");
    }
}