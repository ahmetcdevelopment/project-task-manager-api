using Application.Features.Roles.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using static Application.Features.Roles.Constants.RolesOperationClaims;

namespace Application.Features.Roles.Queries.GetList;

public class GetListRoleQuery : IRequest<GetListResponse<GetListRoleListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListRoles({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetRoles";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListRoleQueryHandler : IRequestHandler<GetListRoleQuery, GetListResponse<GetListRoleListItemDto>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public GetListRoleQueryHandler(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListRoleListItemDto>> Handle(GetListRoleQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Role> roles = await _roleRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListRoleListItemDto> response = _mapper.Map<GetListResponse<GetListRoleListItemDto>>(roles);
            return response;
        }
    }
}