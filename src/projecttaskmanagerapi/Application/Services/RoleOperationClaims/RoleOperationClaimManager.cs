using Application.Features.RoleOperationClaims.Rules;
using Application.Services.Repositories;
using Core.Persistence.Paging;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services.RoleOperationClaims;

public class RoleOperationClaimManager : IRoleOperationClaimService
{
    private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
    private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

    public RoleOperationClaimManager(IRoleOperationClaimRepository roleOperationClaimRepository, RoleOperationClaimBusinessRules roleOperationClaimBusinessRules)
    {
        _roleOperationClaimRepository = roleOperationClaimRepository;
        _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
    }

    public async Task<RoleOperationClaim?> GetAsync(
        Expression<Func<RoleOperationClaim, bool>> predicate,
        Func<IQueryable<RoleOperationClaim>, IIncludableQueryable<RoleOperationClaim, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        RoleOperationClaim? roleOperationClaim = await _roleOperationClaimRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return roleOperationClaim;
    }

    public async Task<IPaginate<RoleOperationClaim>?> GetListAsync(
        Expression<Func<RoleOperationClaim, bool>>? predicate = null,
        Func<IQueryable<RoleOperationClaim>, IOrderedQueryable<RoleOperationClaim>>? orderBy = null,
        Func<IQueryable<RoleOperationClaim>, IIncludableQueryable<RoleOperationClaim, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IPaginate<RoleOperationClaim> roleOperationClaimList = await _roleOperationClaimRepository.GetListAsync(
            predicate,
            orderBy,
            include,
            index,
            size,
            withDeleted,
            enableTracking,
            cancellationToken
        );
        return roleOperationClaimList;
    }

    public async Task<RoleOperationClaim> AddAsync(RoleOperationClaim roleOperationClaim)
    {
        RoleOperationClaim addedRoleOperationClaim = await _roleOperationClaimRepository.AddAsync(roleOperationClaim);

        return addedRoleOperationClaim;
    }

    public async Task<RoleOperationClaim> UpdateAsync(RoleOperationClaim roleOperationClaim)
    {
        RoleOperationClaim updatedRoleOperationClaim = await _roleOperationClaimRepository.UpdateAsync(roleOperationClaim);

        return updatedRoleOperationClaim;
    }

    public async Task<RoleOperationClaim> DeleteAsync(RoleOperationClaim roleOperationClaim, bool permanent = false)
    {
        RoleOperationClaim deletedRoleOperationClaim = await _roleOperationClaimRepository.DeleteAsync(roleOperationClaim);

        return deletedRoleOperationClaim;
    }
}
