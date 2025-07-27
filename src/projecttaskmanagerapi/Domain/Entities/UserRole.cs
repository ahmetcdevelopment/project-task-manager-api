using Core.Persistence.Repositories;

namespace Domain.Entities;
public class UserRole : Entity<int>
{
    public Guid? UserId { get; set; }
    public int? RoleId { get; set; }
    public virtual User? User { get; set; }
    public virtual Role? Role { get; set; }
}
