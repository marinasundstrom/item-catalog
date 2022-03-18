﻿
using MediatR;

using Microsoft.EntityFrameworkCore;

using Catalog.Application.Common.Interfaces;
using Catalog.Domain.Exceptions;

namespace Catalog.Application.Users.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public GetUserQuery(string userId)
    {
        UserId = userId;
    }

    public string UserId { get; }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    { 
        readonly ICatalogContext _context;

        public GetUserQueryHandler(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return null!;
            }

            return new UserDto(user.Id, user.FirstName, user.LastName, user.DisplayName, user.Email, user.Created, user.LastModified);
        }
    }
}