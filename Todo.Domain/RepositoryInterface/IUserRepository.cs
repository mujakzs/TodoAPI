

using Todo.Domain.DomainEntities;

namespace Todo.Domain.RepositoryInterface;

public interface IUserRepository : IGenericRepository<UserDomain>
{
    Task<UserDomain> GetByEmailAsync(string emailAddress);
}
