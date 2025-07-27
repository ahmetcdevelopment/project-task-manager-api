using Core.Persistence.Repositories;

namespace Domain.Entities;
public class Role : Entity<int>
{
    public string Name { get; set; }
    public string ColorCode { get; set; }
    public virtual ICollection<RoleOperationClaim> RoleOperationClaims { get; set; } = default!;
    public virtual ICollection<UserRole> UserRoles { get; set; } = default!;
}
