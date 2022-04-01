
using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.IdentityService.Application.Common.Interfaces;
using Catalog.IdentityService.Domain.Entities;

namespace Catalog.IdentityService.Application.Users.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public GetUserQuery(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IApplicationDbContext _context;

        public GetUserQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .AsNoTracking()
                .AsSplitQuery()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return null!;
            }

            return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName, user.Roles.First().Name, user.Email,
                    user.Created, user.LastModified);
        }
    }
}