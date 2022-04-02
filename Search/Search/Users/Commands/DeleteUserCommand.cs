
using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Search.Application.Common.Interfaces;
using Catalog.Search.Domain.Exceptions;

namespace Catalog.Search.Application.Users.Commands;

public class DeleteUserCommand : IRequest
{
    public DeleteUserCommand(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly ISearchContext _context;

        public DeleteUserCommandHandler(ISearchContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                        .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}