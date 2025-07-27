using FluentValidation;

namespace Application.Features.RoleOperationClaims.Commands.Update;

public class UpdateRoleOperationClaimCommandValidator : AbstractValidator<UpdateRoleOperationClaimCommand>
{
    public UpdateRoleOperationClaimCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}