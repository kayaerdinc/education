using EducationPlatform.Domain.Entities;

namespace EducationPlatform.Application.Abstractions.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
