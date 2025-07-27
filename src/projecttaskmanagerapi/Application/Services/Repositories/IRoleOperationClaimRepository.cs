using Domain.Entities;
using Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IRoleOperationClaimRepository : IAsyncRepository<RoleOperationClaim, int>, IRepository<RoleOperationClaim, int>
{
    Task<IList<OperationClaim>> GetRoleOperationClaimsByUserIdAsync(Guid userId);
}