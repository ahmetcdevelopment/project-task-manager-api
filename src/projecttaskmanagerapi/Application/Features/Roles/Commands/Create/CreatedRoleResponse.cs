using Core.Application.Responses;

namespace Application.Features.Roles.Commands.Create;

public class CreatedRoleResponse : IResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ColorCode { get; set; }
}