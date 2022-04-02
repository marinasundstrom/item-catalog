using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Contracts;
using Catalog.IdentityService.Domain.Exceptions;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Catalog.IdentityService.Application.Users.Commands;

public record UpdateUserDetailsCommand(
    string UserId, string FirstName, string LastName, string? DisplayName, string Email) : IRequest<UserDto>
{
    public class UpdateUserDetailsCommandHandler : IRequestHandler<UpdateUserDetailsCommand, UserDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventPublisher _eventPublisher;

        public UpdateUserDetailsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IEventPublisher eventPublisher)
        {
            _context = context;
            _currentUserService = currentUserService;
            _eventPublisher = eventPublisher;
        }

        public async Task<UserDto> Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.DisplayName = request.DisplayName;
            user.Email = request.Email;

            await _context.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishEvent(new UserUpdated(user.Id, _currentUserService.UserId));

            return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName, user.Roles.First().Name, user.Email,
                    user.Created, user.LastModified);
        }
    }
}
