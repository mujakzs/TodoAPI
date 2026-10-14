using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todo.Domain.DomainEntities;
using Todo.Domain.RepositoryInterface;
using Todo.Infrastructure.Persistence.Entities;

namespace Todo.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<UserDomain, User>, IUserRepository
    {
        public UserRepository(TodoAppDbContext todoAppDbContext,IMapper _mapper) 
            : base(todoAppDbContext, _mapper)
        {

        }

        public async Task<UserDomain> GetByEmailAsync(string emailAddress)
        {

            var user = await _todoAppDbContext.Users.FirstOrDefaultAsync(x => x.Email == emailAddress); // Find me the first user with this email. If nobody has it, just return nothing.

            return _mapper.Map<UserDomain>(user); //Your database query returns a database entity, but your repository contract expects a UserDomain.
        }
    }
}

// Database entity → mapping → UserDomain → Token service



//Let's break this into three parts.

//A. _todoAppDbContext.Users
//- _todoAppDbContext is your Entity Framework Core database context.
//- .Users represents the database entity set mapped to your users table.
//Think of it as accessing the collection of users stored in your database.

//B. FirstOrDefaultAsync(...)
//This method searches for the first record matching a condition.


//AUTO MAPPER

// The important point is that AutoMapper transfers data between objects.
// It does not query the database; EF Core already did tha


//EF Core retrieves the user, AutoMapper converts the database entity into UserDomain,
//and your token service uses that domain object to continue authentication