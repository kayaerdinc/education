namespace EducationPlatform.Application.Abstractions.Authentication;

public interface IPasswordHasher
{
    (byte[] hash, byte[] salt) HashPassword(string password);
    bool VerifyPassword(string password, byte[] salt, byte[] expectedHash);
}
