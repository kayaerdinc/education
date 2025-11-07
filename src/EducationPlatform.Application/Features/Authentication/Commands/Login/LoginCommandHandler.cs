using EducationPlatform.Application.Abstractions.Authentication;
using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Authentication;
using EducationPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        IApplicationDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail && u.IsActive, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Geçersiz e-posta veya şifre.");
        }

        var passwordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordSalt, user.PasswordHash);
        if (!passwordValid)
        {
            throw new UnauthorizedAccessException("Geçersiz e-posta veya şifre.");
        }

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString()
        };
    }
}
