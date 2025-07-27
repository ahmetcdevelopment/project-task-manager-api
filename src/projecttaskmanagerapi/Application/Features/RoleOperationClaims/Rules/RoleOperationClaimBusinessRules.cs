using Application.Features.RoleOperationClaims.Constants;
using Application.Services.Repositories;
using Core.Application.Rules;
using Core.CrossCuttingConcerns.Exception.Types;
using Core.Localization.Abstraction;
using Domain.Entities;

namespace Application.Features.RoleOperationClaims.Rules;

public class RoleOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
    private readonly ILocalizationService _localizationService;

    public RoleOperationClaimBusinessRules(IRoleOperationClaimRepository roleOperationClaimRepository, ILocalizationService localizationService)
    {
        _roleOperationClaimRepository = roleOperationClaimRepository;
        _localizationService = localizationService;
    }

    private async Task throwBusinessException(string messageKey)
    {
        string message = await _localizationService.GetLocalizedAsync(messageKey, RoleOperationClaimsBusinessMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task RoleOperationClaimShouldExistWhenSelected(RoleOperationClaim? roleOperationClaim)
    {
        if (roleOperationClaim == null)
            await throwBusinessException(RoleOperationClaimsBusinessMessages.RoleOperationClaimNotExists);
    }

    public async Task RoleOperationClaimIdShouldExistWhenSelected(int id, CancellationToken cancellationToken)
    {
        RoleOperationClaim? roleOperationClaim = await _roleOperationClaimRepository.GetAsync(
            predicate: roc => roc.Id == id,
            enableTracking: false,
            cancellationToken: cancellationToken
        );
        await RoleOperationClaimShouldExistWhenSelected(roleOperationClaim);
    }
}