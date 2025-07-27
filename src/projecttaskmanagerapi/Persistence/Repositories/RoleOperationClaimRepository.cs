using Application.Services.Repositories;
using Core.Persistence.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class RoleOperationClaimRepository : EfRepositoryBase<RoleOperationClaim, int, BaseDbContext>, IRoleOperationClaimRepository
{
    public RoleOperationClaimRepository(BaseDbContext context) : base(context)
    {
    }
    public async Task<IList<OperationClaim>> GetRoleOperationClaimsByUserIdAsync(Guid userId)
    {
        var roles = BaseContext.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();
        List<OperationClaim> operationClaims = await Query()
            .AsNoTracking()
            .Where(p => roles.Contains(p.RoleId))

            .Select(p => new OperationClaim { Id = p.OperationClaimId ?? 0, Name = p.OperationClaim.Name })
            .ToListAsync();
        return operationClaims;
    }
}