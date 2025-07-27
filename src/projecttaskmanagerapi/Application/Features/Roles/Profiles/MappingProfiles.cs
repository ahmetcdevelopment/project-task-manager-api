using Application.Features.Roles.Commands.Create;
using Application.Features.Roles.Commands.Delete;
using Application.Features.Roles.Commands.Update;
using Application.Features.Roles.Queries.GetById;
using Application.Features.Roles.Queries.GetList;
using AutoMapper;
using Core.Application.Responses;
using Domain.Entities;
using Core.Persistence.Paging;

namespace Application.Features.Roles.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CreateRoleCommand, Role>();
        CreateMap<Role, CreatedRoleResponse>();

        CreateMap<UpdateRoleCommand, Role>();
        CreateMap<Role, UpdatedRoleResponse>();

        CreateMap<DeleteRoleCommand, Role>();
        CreateMap<Role, DeletedRoleResponse>();

        CreateMap<Role, GetByIdRoleResponse>();

        CreateMap<Role, GetListRoleListItemDto>();
        CreateMap<IPaginate<Role>, GetListResponse<GetListRoleListItemDto>>();
    }
}