namespace Domain.Entities;

public class RefreshToken : Core.Security.Entities.RefreshToken<Guid>
{
    public virtual User User { get; set; } = default!;
}
