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

namespace Application.Features.RoleOperationClaims.Commands.Create;

public class CreateRoleOperationClaimCommand : IRequest<CreatedRoleOperationClaimResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int? RoleId { get; set; }
    public int? OperationClaimId { get; set; }

    public string[] Roles => [Admin, Write, RoleOperationClaimsOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoleOperationClaims"];

    public class CreateRoleOperationClaimCommandHandler : IRequestHandler<CreateRoleOperationClaimCommand, CreatedRoleOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

        public CreateRoleOperationClaimCommandHandler(IMapper mapper, IRoleOperationClaimRepository roleOperationClaimRepository,
                                         RoleOperationClaimBusinessRules roleOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _roleOperationClaimRepository = roleOperationClaimRepository;
            _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        }

        public async Task<CreatedRoleOperationClaimResponse> Handle(CreateRoleOperationClaimCommand request, CancellationToken cancellationToken)
        {
            RoleOperationClaim roleOperationClaim = _mapper.Map<RoleOperationClaim>(request);

            await _roleOperationClaimRepository.AddAsync(roleOperationClaim);

            CreatedRoleOperationClaimResponse response = _mapper.Map<CreatedRoleOperationClaimResponse>(roleOperationClaim);
            return response;
        }
    }
}