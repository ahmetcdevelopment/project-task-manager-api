using Application.Features.RoleOperationClaims.Constants;
using Application.Features.RoleOperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.RoleOperationClaims.Constants.RoleOperationClaimsOperationClaims;

namespace Application.Features.RoleOperationClaims.Commands.Update;

public class UpdateRoleOperationClaimCommand : IRequest<UpdatedRoleOperationClaimResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public int? RoleId { get; set; }
    public int? OperationClaimId { get; set; }

    public string[] Roles => [Admin, Write, RoleOperationClaimsOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoleOperationClaims"];

    public class UpdateRoleOperationClaimCommandHandler : IRequestHandler<UpdateRoleOperationClaimCommand, UpdatedRoleOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

        public UpdateRoleOperationClaimCommandHandler(IMapper mapper, IRoleOperationClaimRepository roleOperationClaimRepository,
                                         RoleOperationClaimBusinessRules roleOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _roleOperationClaimRepository = roleOperationClaimRepository;
            _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        }

        public async Task<UpdatedRoleOperationClaimResponse> Handle(UpdateRoleOperationClaimCommand request, CancellationToken cancellationToken)
        {
            RoleOperationClaim? roleOperationClaim = await _roleOperationClaimRepository.GetAsync(predicate: roc => roc.Id == request.Id, cancellationToken: cancellationToken);
            await _roleOperationClaimBusinessRules.RoleOperationClaimShouldExistWhenSelected(roleOperationClaim);
            roleOperationClaim = _mapper.Map(request, roleOperationClaim);

            await _roleOperationClaimRepository.UpdateAsync(roleOperationClaim!);

            UpdatedRoleOperationClaimResponse response = _mapper.Map<UpdatedRoleOperationClaimResponse>(roleOperationClaim);
            return response;
        }
    }
}