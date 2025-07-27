using Core.Application.Dtos;

namespace Application.Features.Roles.Queries.GetList;

public class GetListRoleListItemDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }
}