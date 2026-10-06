using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Domain.RepositoryInterface
{
    public interface IGenericRepository<TDomain> where TDomain : class
    {
        Task<TDomain> GetByIdAsync(object id); //Give me a domain object based on its ID.

        Task<IEnumerable<TDomain>> GetAllAsync(); //Give me all domain objects.

        Task AddAsync(TDomain domain); //Give me a domain object and I'll add it.

        Task<int> CommitAsync(); //Commit the changes to the database and return the number of affected rows.

    }
}


/*
 * 
 where TDomain : class
        ↓
TDomain must be a class/reference type

 */
