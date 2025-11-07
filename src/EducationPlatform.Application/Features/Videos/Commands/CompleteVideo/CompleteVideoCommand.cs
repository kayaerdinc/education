using MediatR;

namespace EducationPlatform.Application.Features.Videos.Commands.CompleteVideo;

public record CompleteVideoCommand(Guid UserId, Guid VideoId) : IRequest;
