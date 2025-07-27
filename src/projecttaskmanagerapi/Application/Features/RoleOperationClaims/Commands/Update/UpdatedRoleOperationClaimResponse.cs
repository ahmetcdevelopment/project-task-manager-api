using Core.Application.Responses;

namespace Application.Features.RoleOperationClaims.Commands.Update;

public class UpdatedRoleOperationClaimResponse : IResponse
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public int? OperationClaimId { get; set; }
}