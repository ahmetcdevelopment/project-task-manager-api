using Application.Features.RoleOperationClaims.Commands.Create;
using Application.Features.RoleOperationClaims.Commands.Delete;
using Application.Features.RoleOperationClaims.Commands.Update;
using Application.Features.RoleOperationClaims.Queries.GetById;
using Application.Features.RoleOperationClaims.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Domain.Entities;
using Core.Persistence.Paging;

namespace Application.Features.RoleOperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRoleOperationClaimCommand, RoleOperationClaim>();
        CreateMap<RoleOperationClaim, CreatedRoleOperationClaimResponse>();

        CreateMap<UpdateRoleOperationClaimCommand, RoleOperationClaim>();
        CreateMap<RoleOperationClaim, UpdatedRoleOperationClaimResponse>();

        CreateMap<DeleteRoleOperationClaimCommand, RoleOperationClaim>();
        CreateMap<RoleOperationClaim, DeletedRoleOperationClaimResponse>();

        CreateMap<RoleOperationClaim, GetByIdRoleOperationClaimResponse>();

        CreateMap<RoleOperationClaim, GetListRoleOperationClaimListItemDto>();
        CreateMap<IPaginate<RoleOperationClaim>, GetListResponse<GetListRoleOperationClaimListItemDto>>();
    }
}