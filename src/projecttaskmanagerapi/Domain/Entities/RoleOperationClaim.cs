using Core.Persistence.Repositories;

namespace Domain.Entities;
public class RoleOperationClaim : Entity<int>
{
    public int? RoleId { get; set; }
    public int? OperationClaimId { get; set; }
    public virtual OperationClaim? OperationClaim { get; set; }
    public virtual Role? Role { get; set; }
}
