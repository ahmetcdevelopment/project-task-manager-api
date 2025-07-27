using Application.Features.UserRoles.Constants;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Requests;
using Core.Application.Responses;
using Core.Persistence.Paging;
using MediatR;
using static Application.Features.UserRoles.Constants.UserRolesOperationClaims;

namespace Application.Features.UserRoles.Queries.GetList;

public class GetListUserRoleQuery : IRequest<GetListResponse<GetListUserRoleListItemDto>>, ISecuredRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; }

    public string[] Roles => [Admin, Read];

    public bool BypassCache { get; }
    public string? CacheKey => $"GetListUserRoles({PageRequest.PageIndex},{PageRequest.PageSize})";
    public string? CacheGroupKey => "GetUserRoles";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListUserRoleQueryHandler : IRequestHandler<GetListUserRoleQuery, GetListResponse<GetListUserRoleListItemDto>>
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IMapper _mapper;

        public GetListUserRoleQueryHandler(IUserRoleRepository userRoleRepository, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _mapper = mapper;
        }

        public async Task<GetListResponse<GetListUserRoleListItemDto>> Handle(GetListUserRoleQuery request, CancellationToken cancellationToken)
        {
            IPaginate<UserRole> userRoles = await _userRoleRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize, 
                cancellationToken: cancellationToken
            );

            GetListResponse<GetListUserRoleListItemDto> response = _mapper.Map<GetListResponse<GetListUserRoleListItemDto>>(userRoles);
            return response;
        }
    }
}