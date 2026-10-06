using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Domain.DomainEntities;
using Todo.Domain.RepositoryInterface;
using Todo.Infrastructure.Persistence.Entities;

namespace Todo.Infrastructure.Repository
{
    public class GenericRepository<TDomain, TEntity> : IGenericRepository<TDomain>
        where TDomain : class 
        where TEntity : class
    {

        protected readonly TodoAppDbContext _todoAppDbContext; //This class can use it, and classes that inherit from this class can use it.
        protected readonly IMapper _mapper;

        public GenericRepository(TodoAppDbContext todoAppDbContext, IMapper mapper)
        {
            _todoAppDbContext = todoAppDbContext;
            _mapper = mapper;
        }

        public async Task AddAsync(TDomain domain)
        {
            var entity = _mapper.Map<TEntity>(domain); // Map the domain entity to the database entity
            await _todoAppDbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CommitAsync()
        {
            return await _todoAppDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TDomain>> GetAllAsync()
        {
            return await _todoAppDbContext.Set<TEntity>()
                .ProjectTo<TDomain>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TDomain?> GetByIdAsync(object id)
        {
            var entity = await _todoAppDbContext.Set<TEntity>().FindAsync(id);
            return entity == null ? null : _mapper.Map<TDomain>(entity); // Map the database entity back to the domain entity
        }
    }
}

/*
 
UserDomain
     ↓
 business/domain representation

User
     ↓
 persistence/database representation


TDomain = UserDomain
TEntity = User

*/
