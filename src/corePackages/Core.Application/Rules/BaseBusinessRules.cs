using Core.Persistence.Repositories;

namespace Core.Application.Rules;

public abstract class BaseBusinessRules
{
    public async Task<T> EntityIsExist<T, TId>(T entity) where T : Entity<TId>, new()
    {
        bool doesExist = entity == null;

        if (doesExist)
            return await Task.FromResult(new T()); // Eğer entity null ise yeni bir T nesnesi döner.

        return await Task.FromResult(entity);
    }
}
