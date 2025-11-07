namespace EducationPlatform.Application.DTOs.Authentication;

public class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
}
