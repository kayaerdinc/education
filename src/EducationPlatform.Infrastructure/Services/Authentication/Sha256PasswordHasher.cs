using System.Security.Cryptography;
using System.Text;
using EducationPlatform.Application.Abstractions.Authentication;

namespace EducationPlatform.Infrastructure.Services.Authentication;

public class Sha256PasswordHasher : IPasswordHasher
{
    public (byte[] hash, byte[] salt) HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(32);
        var hash = ComputeHash(password, salt);
        return (hash, salt);
    }

    public bool VerifyPassword(string password, byte[] salt, byte[] expectedHash)
    {
        var computed = ComputeHash(password, salt);
        return CryptographicOperations.FixedTimeEquals(computed, expectedHash);
    }

    private static byte[] ComputeHash(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        var combined = new byte[passwordBytes.Length + salt.Length];
        Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
        Buffer.BlockCopy(salt, 0, combined, passwordBytes.Length, salt.Length);

        return SHA256.HashData(combined);
    }
}
