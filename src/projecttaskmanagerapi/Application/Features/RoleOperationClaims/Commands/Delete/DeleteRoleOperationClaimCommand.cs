using Application.Features.RoleOperationClaims.Constants;
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

namespace Application.Features.RoleOperationClaims.Commands.Delete;

public class DeleteRoleOperationClaimCommand : IRequest<DeletedRoleOperationClaimResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, RoleOperationClaimsOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoleOperationClaims"];

    public class DeleteRoleOperationClaimCommandHandler : IRequestHandler<DeleteRoleOperationClaimCommand, DeletedRoleOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

        public DeleteRoleOperationClaimCommandHandler(IMapper mapper, IRoleOperationClaimRepository roleOperationClaimRepository,
                                         RoleOperationClaimBusinessRules roleOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _roleOperationClaimRepository = roleOperationClaimRepository;
            _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        }

        public async Task<DeletedRoleOperationClaimResponse> Handle(DeleteRoleOperationClaimCommand request, CancellationToken cancellationToken)
        {
            RoleOperationClaim? roleOperationClaim = await _roleOperationClaimRepository.GetAsync(predicate: roc => roc.Id == request.Id, cancellationToken: cancellationToken);
            await _roleOperationClaimBusinessRules.RoleOperationClaimShouldExistWhenSelected(roleOperationClaim);

            await _roleOperationClaimRepository.DeleteAsync(roleOperationClaim!);

            DeletedRoleOperationClaimResponse response = _mapper.Map<DeletedRoleOperationClaimResponse>(roleOperationClaim);
            return response;
        }
    }
}