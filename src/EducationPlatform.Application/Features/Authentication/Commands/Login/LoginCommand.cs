using EducationPlatform.Application.DTOs.Authentication;
using MediatR;

namespace EducationPlatform.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponseDto>;
