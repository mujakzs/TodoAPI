using Todo.Application.Contracts;
using Todo.Application.DTOs.Request;
using Todo.Application.Implementation;
using Todo.Application.Mappers;
using Todo.Domain.DomainEntities;
using Todo.Domain.RepositoryInterface;

namespace Todo.Application.Implementations
{
    public class UserService(
        IUserRepository _userRepository,
        IPasswordHasher _passwordHasher,
        IEmailService _emailService) : IUserService
    {
        public async Task<bool> CreateUserAsync(CreateUserDto userDto)
        {
            //convert dto into domain
            //user password into hashed password

            var UserDomain = userDto.ToUserDomain(); // Convert CreateUserDto to UserDomain using AutoMapper

            UserDomain.PasswordHash = _passwordHasher.Hash(userDto.Password);  // Hash the password using BCrypt

            await _userRepository.AddAsync(UserDomain);

            var response = await _userRepository.CommitAsync();

            if (response > 0)
            {
                await _emailService.SendMailAsync(UserDomain.Email);
            }

            return response > 0;
        }
    }
}
