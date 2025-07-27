using Application.Features.RoleOperationClaims.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using static Application.Features.RoleOperationClaims.Constants.RoleOperationClaimsOperationClaims;

namespace Application.Features.RoleOperationClaims.Queries.GetList;

public class GetListRoleOperationClaimQuery : IRequest<GetListResponse<GetListRoleOperationClaimListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListRoleOperationClaims({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetRoleOperationClaims";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListRoleOperationClaimQueryHandler : IRequestHandler<GetListRoleOperationClaimQuery, GetListResponse<GetListRoleOperationClaimListItemDto>>
    {
        private readonly IRoleOperationClaimRepository _roleOperationClaimRepository;
        private readonly IMapper _mapper;

        public GetListRoleOperationClaimQueryHandler(IRoleOperationClaimRepository roleOperationClaimRepository, IMapper mapper)
        {
            _roleOperationClaimRepository = roleOperationClaimRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRoleOperationClaimListItemDto>> Handle(GetListRoleOperationClaimQuery request, CancellationToken cancellationToken)
        {
            IPaginate<RoleOperationClaim> roleOperationClaims = await _roleOperationClaimRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRoleOperationClaimListItemDto> response = _mapper.Map<GetListResponse<GetListRoleOperationClaimListItemDto>>(roleOperationClaims);
            return response;
        }
    }
}