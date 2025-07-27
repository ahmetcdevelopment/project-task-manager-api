using FluentValidation;

namespace Application.Features.RoleOperationClaims.Commands.Create;

public class CreateRoleOperationClaimCommandValidator : AbstractValidator<CreateRoleOperationClaimCommand>
{
    public CreateRoleOperationClaimCommandValidator()
    {
    }
}