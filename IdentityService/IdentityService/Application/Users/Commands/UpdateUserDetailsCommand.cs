using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Contracts;
using Catalog.IdentityService.Domain.Exceptions;

namespace Catalog.IdentityService.Application.Users.Commands;

public class UpdateUserDetailsCommand : IRequest<UserDto>
{
    public UpdateUserDetailsCommand(string userId, string firstName, string lastName, string? displayName, string email)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        DisplayName = displayName;
        Email = email;
    }

    public string UserId { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string? DisplayName { get; }

    public string Email { get; }

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
