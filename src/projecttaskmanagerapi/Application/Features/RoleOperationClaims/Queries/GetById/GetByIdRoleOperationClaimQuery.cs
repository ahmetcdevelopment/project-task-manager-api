using Application.Features.RoleOperationClaims.Constants;
using Application.Features.RoleOperationClaims.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using MediatR;
using static Application.Features.RoleOperationClaims.Constants.RoleOperationClaimsOperationClaims;

namespace Application.Features.RoleOperationClaims.Queries.GetById;

public class GetByIdRoleOperationClaimQuery : IRequest<GetByIdRoleOperationClaimResponse>, ISecuredRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Read];

    public class GetByIdRoleOperationClaimQueryHandler : IRequestHandler<GetByIdRoleOperationClaimQuery, GetByIdRoleOperationClaimResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly RoleOperationClaimBusinessRules _roleOperationClaimBusinessRules;

        public GetByIdRoleOperationClaimQueryHandler(IMapper mapper, IRoleOperationClaimRepository roleOperationClaimRepository, RoleOperationClaimBusinessRules roleOperationClaimBusinessRules)
        {
            _mapper = mapper;
            _roleOperationClaimRepository = roleOperationClaimRepository;
            _roleOperationClaimBusinessRules = roleOperationClaimBusinessRules;
        }

        public async Task<GetByIdRoleOperationClaimResponse> Handle(GetByIdRoleOperationClaimQuery request, CancellationToken cancellationToken)
        {
            RoleOperationClaim? roleOperationClaim = await _roleOperationClaimRepository.GetAsync(predicate: roc => roc.Id == request.Id, cancellationToken: cancellationToken);
            await _roleOperationClaimBusinessRules.RoleOperationClaimShouldExistWhenSelected(roleOperationClaim);

            GetByIdRoleOperationClaimResponse response = _mapper.Map<GetByIdRoleOperationClaimResponse>(roleOperationClaim);
            return response;
        }
    }
}