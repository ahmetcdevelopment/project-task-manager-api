using Domain.Entities;
using Core.Persistence.Repositories;

namespace Application.Services.Repositories;

public interface IUserRoleRepository : IAsyncRepository<UserRole, int>, IRepository<UserRole, int>
{
}