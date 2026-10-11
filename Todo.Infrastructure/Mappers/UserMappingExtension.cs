using AutoMapper;
using Todo.Domain.DomainEntities;
using Todo.Infrastructure.Persistence.Entities;

namespace Todo.Infrastructure.Mappers
{
    public class UserMappingExtension : Profile
    {
        public UserMappingExtension()
        {
            CreateMap<User, UserDomain>().ReverseMap(); // Mapping from User entity to UserDomain - reverse mapping = CreateMap<UserDomain, User>

        }
    }
}
