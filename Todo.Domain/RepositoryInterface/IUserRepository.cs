

using Todo.Domain.DomainEntities;

namespace Todo.Domain.RepositoryInterface;

public interface IUserRepository : IGenericRepository<UserDomain>
{
    Task<UserDomain> GetByEmailAsync(string emailAddress);
}


// The key concept is that IUserRepository defines the contract, UserRepository implements the database access,
// and GetTokenAsync() consumes that contract to authenticate the user.