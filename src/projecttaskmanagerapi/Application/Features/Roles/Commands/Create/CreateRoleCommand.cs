using Application.Features.Roles.Constants;
using Application.Features.Roles.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Core.Application.Pipelines.Authorization;
using Core.Application.Pipelines.Caching;
using Core.Application.Pipelines.Logging;
using Core.Application.Pipelines.Transaction;
using MediatR;
using static Application.Features.Roles.Constants.RolesOperationClaims;

namespace Application.Features.Roles.Commands.Create;

public class CreateRoleCommand : IRequest<CreatedRoleResponse>, ISecuredRequest, ICacheRemoverRequest, ILoggableRequest, ITransactionalRequest
{
    public required string Name { get; set; }
    public required string ColorCode { get; set; }

    public string[] Roles => [Admin, Write, RolesOperationClaims.Create];

    public bool BypassCache { get; }
    public string? CacheKey { get; }
    public string[]? CacheGroupKey => ["GetRoles"];

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, CreatedRoleResponse>
    {
        private readonly IMapper _mapper;
        private readonly IRoleRepository _roleRepository;
        private readonly RoleBusinessRules _roleBusinessRules;

        public CreateRoleCommandHandler(IMapper mapper, IRoleRepository roleRepository,
                                         RoleBusinessRules roleBusinessRules)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _roleBusinessRules = roleBusinessRules;
        }

        public async Task<CreatedRoleResponse> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            Role role = _mapper.Map<Role>(request);

            await _roleRepository.AddAsync(role);

            CreatedRoleResponse response = _mapper.Map<CreatedRoleResponse>(role);
            return response;
        }
    }
}