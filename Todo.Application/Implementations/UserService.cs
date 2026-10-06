using AutoMapper;
using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;
using Todo.Domain.DomainEntities;
using Todo.Domain.RepositoryInterface;

namespace Todo.Application.Implementations
{
    public class UserService(IUserRepository userRepository,
        IMapper mapper,
        IPasswordHasher _passwordHasher) : IUserService
    {
        public async Task<bool> CreateUserAsync(CreateUserDto userDto)
        {
            //convert dto into domain
            //user password into hashed password

            var UserDomain = mapper.Map<UserDomain>(userDto); // Convert CreateUserDto to UserDomain using AutoMapper

            UserDomain.PasswordHash = _passwordHasher.Hash(userDto.Password);  // Hash the password using BCrypt

            await userRepository.AddAsync(UserDomain);

            var response = await userRepository.CommitAsync();

            return response > 0;
        }
    }
}
