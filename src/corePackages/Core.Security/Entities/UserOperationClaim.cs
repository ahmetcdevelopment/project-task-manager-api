using Core.Persistence.Repositories;

namespace Core.Security.Entities;
public class UserOperationClaim<TUserId, TOperationClaimId> : Entity<TUserId>
{
    public UserOperationClaim() { }
    public UserOperationClaim(TUserId userId, TOperationClaimId operationClaimId)
    {
        UserId = userId;
        OperationClaimId = operationClaimId;
    }
    public UserOperationClaim(TUserId id, TUserId userId, TOperationClaimId operationClaimId)
    {
        Id = id;
        UserId = userId;
        OperationClaimId = operationClaimId;
    }

    public TUserId UserId { get; set; }
    public TOperationClaimId OperationClaimId { get; set; }
}
