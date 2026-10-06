
using Todo.Application.Contracts;

namespace Todo.Application.Implementations
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password); // Hash the password using BCrypt
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword); // Verify the password using BCrypt
        }
    }
}

       