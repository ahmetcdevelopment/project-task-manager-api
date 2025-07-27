using Application.Features.UserRoles.Constants;
using Application.Features.UserRoles.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.UserRoles.Constants.UserRolesOperationClaims;

namespace Application.Features.UserRoles.Commands.Update;

public class UpdateUserRoleCommand : IRequest<UpdatedUserRoleResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }
    public Guid? UserId { get; set; }
    public int? RoleId { get; set; }

    public string[] Roles => [Admin, Write, UserRolesOperationClaims.Update];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetUserRoles"];

    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UpdatedUserRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserRoleBusinessRules _userRoleBusinessRules;

        public UpdateUserRoleCommandHandler(IMapper mapper, IUserRoleRepository userRoleRepository,
                                         UserRoleBusinessRules userRoleBusinessRules)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _userRoleBusinessRules = userRoleBusinessRules;
        }

        public async Task<UpdatedUserRoleResponse> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            UserRole? userRole = await _userRoleRepository.GetAsync(predicate: ur => ur.Id == request.Id, cancellationToken: cancellationToken);
            await _userRoleBusinessRules.UserRoleShouldExistWhenSelected(userRole);
            userRole = _mapper.Map(request, userRole);

            await _userRoleRepository.UpdateAsync(userRole!);

            UpdatedUserRoleResponse response = _mapper.Map<UpdatedUserRoleResponse>(userRole);
            return response;
        }
    }
}