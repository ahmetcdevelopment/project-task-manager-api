using Application.Features.UserRoles.Constants;
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

namespace Application.Features.UserRoles.Commands.Delete;

public class DeleteUserRoleCommand : IRequest<DeletedUserRoleResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public int Id { get; set; }

    public string[] Roles => [Admin, Write, UserRolesOperationClaims.Delete];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetUserRoles"];

    public class DeleteUserRoleCommandHandler : IRequestHandler<DeleteUserRoleCommand, DeletedUserRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly UserRoleBusinessRules _userRoleBusinessRules;

        public DeleteUserRoleCommandHandler(IMapper mapper, IUserRoleRepository userRoleRepository,
                                         UserRoleBusinessRules userRoleBusinessRules)
        {
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _userRoleBusinessRules = userRoleBusinessRules;
        }

        public async Task<DeletedUserRoleResponse> Handle(DeleteUserRoleCommand request, CancellationToken cancellationToken)
        {
            UserRole? userRole = await _userRoleRepository.GetAsync(predicate: ur => ur.Id == request.Id, cancellationToken: cancellationToken);
            await _userRoleBusinessRules.UserRoleShouldExistWhenSelected(userRole);

            await _userRoleRepository.DeleteAsync(userRole!);

            DeletedUserRoleResponse response = _mapper.Map<DeletedUserRoleResponse>(userRole);
            return response;
        }
    }
}